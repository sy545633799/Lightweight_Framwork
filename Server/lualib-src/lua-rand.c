#include <lua.h>
#include <lauxlib.h>

#include <stdint.h>
#include <stdlib.h>

#define INT64_SUPPORTED // Remove this if the compiler doesn't support 64-bit integers

// Choose which version of Mersenne Twister you want:
#if 0 
// Define constants for type MT11213A:
#define MERS_N   351
#define MERS_M   175
#define MERS_R   19
#define MERS_U   11
#define MERS_S   7
#define MERS_T   15
#define MERS_L   17
#define MERS_A   0xE4BD75F5
#define MERS_B   0x655E5280
#define MERS_C   0xFFD58000
#else    
// or constants for type MT19937:
#define MERS_N   624
#define MERS_M   397
#define MERS_R   31
#define MERS_U   11
#define MERS_S   7
#define MERS_T   15
#define MERS_L   18
#define MERS_A   0x9908B0DF
#define MERS_B   0x9D2C5680
#define MERS_C   0xEFC60000
#endif

struct rand_object {
    uint32_t mt[MERS_N];                // State vector
    int mti;                            // Index into mt
    uint32_t LastInterval;              // Last interval length for IRandomX
    uint32_t RLimit;                    // Rejection limit used by IRandomX
};

static void 
rand_init0(struct rand_object *obj, int seed) {
    // Seed generator
    const uint32_t factor = 1812433253UL;
    obj->mt[0]= seed;
    for (obj->mti=1; obj->mti < MERS_N; obj->mti++) {
        obj->mt[obj->mti] = (factor * (obj->mt[obj->mti-1] ^ (obj->mt[obj->mti-1] >> 30)) + obj->mti);
    }
}

static uint32_t 
rand_b(struct rand_object *obj) {
    // Generate 32 random bits
    uint32_t y;

    if (obj->mti >= MERS_N) {
        // Generate MERS_N words at one time
        const uint32_t LOWER_MASK = (1LU << MERS_R) - 1;       // Lower MERS_R bits
        const uint32_t UPPER_MASK = 0xFFFFFFFF << MERS_R;      // Upper (32 - MERS_R) bits
        static const uint32_t mag01[2] = {0, MERS_A};

        int kk;
        for (kk=0; kk < MERS_N-MERS_M; kk++) {    
            y = (obj->mt[kk] & UPPER_MASK) | (obj->mt[kk+1] & LOWER_MASK);
            obj->mt[kk] = obj->mt[kk+MERS_M] ^ (y >> 1) ^ mag01[y & 1];}

        for (; kk < MERS_N-1; kk++) {    
            y = (obj->mt[kk] & UPPER_MASK) | (obj->mt[kk+1] & LOWER_MASK);
            obj->mt[kk] = obj->mt[kk+(MERS_M-MERS_N)] ^ (y >> 1) ^ mag01[y & 1];}      

        y = (obj->mt[MERS_N-1] & UPPER_MASK) | (obj->mt[0] & LOWER_MASK);
        obj->mt[MERS_N-1] = obj->mt[MERS_M-1] ^ (y >> 1) ^ mag01[y & 1];
        obj->mti = 0;
    }
    y = obj->mt[obj->mti++];

    // Tempering (May be omitted):
    y ^=  y >> MERS_U;
    y ^= (y << MERS_S) & MERS_B;
    y ^= (y << MERS_T) & MERS_C;
    y ^=  y >> MERS_L;

    return y;
}

static void 
rand_init(struct rand_object *obj, int seed) {
    int i;
    // Initialize and seed
    rand_init0(obj, seed);

    // Randomize some more
    for (i = 0; i < 37; i++) rand_b(obj);
}

static double 
rand_f(struct rand_object *obj) {
    // Output random float number in the interval 0 <= x < 1
    // Multiply by 2^(-32)
    return (double)rand_b(obj) * (1./(65536.*65536.));
}

static int 
rand_i(struct rand_object *obj, int min, int max) {
    int r;
    // Output random integer in the interval min <= x <= max
    // Relative error on frequencies < 2^-32
    if (max <= min) {
        if (max == min) return min; else return 0x80000000;
    }
    // Multiply interval with random and truncate
    r = (int)((double)(uint32_t)(max - min + 1) * rand_f(obj) + min); 
    if (r > max) r = max;
    return r;
}

static int 
rand_x(struct rand_object *obj, int min, int max) {
    // Output random integer in the interval min <= x <= max
    // Each output value has exactly the same probability.
    // This is obtained by rejecting certain bit values so that the number
    // of possible bit values is divisible by the interval length
#ifdef  INT64_SUPPORTED
    // 64 bit integers available. Use multiply and shift method
    uint32_t interval;                    // Length of interval
    uint64_t longran;                     // Random bits * interval
    uint32_t iran;                        // Longran / 2^32
    uint32_t remainder;                   // Longran % 2^32

    if (max <= min) {
        if (max == min) return min; else return 0x80000000;
    }

    interval = (uint32_t)(max - min + 1);
    if (interval != obj->LastInterval) {
        // Interval length has changed. Must calculate rejection limit
        // Reject when remainder >= 2^32 / interval * interval
        // RLimit will be 0 if interval is a power of 2. No rejection then
        obj->RLimit = (uint32_t)(((uint64_t)1 << 32) / interval) * interval - 1;
        obj->LastInterval = interval;
    }
    do { // Rejection loop
        longran  = (uint64_t)rand_b(obj) * interval;
        iran = (uint32_t)(longran >> 32);
        remainder = (uint32_t)longran;
    } while (remainder > obj->RLimit);
    // Convert back to signed and return result
    return (int32_t)iran + min;
#else
    // 64 bit integers not available. Use modulo method
    uint32_t interval;                    // Length of interval
    uint32_t bran;                        // Random bits
    uint32_t iran;                        // bran / interval
    uint32_t remainder;                   // bran % interval

    if (max <= min) {
        if (max == min) return min; else return 0x80000000;
    }

    interval = uint32_t(max - min + 1);
    if (interval != obj->LastInterval) {
        // Interval length has changed. Must calculate rejection limit
        // Reject when iran = 2^32 / interval
        // We can't make 2^32 so we use 2^32-1 and correct afterwards
        obj->RLimit = (uint32_t)0xFFFFFFFF / interval;
        if ((uint32_t)0xFFFFFFFF % interval == interval - 1) obj->RLimit++;
    }
    do { // Rejection loop
        bran = rand_b(obj);
        iran = bran / interval;
        remainder = bran % interval;
    } while (iran >= obj->RLimit);
    // Convert back to signed and return result
    return (int32_t)remainder + min;
#endif
}

static struct rand_object *
rand_new(int seed) {
    struct rand_object *obj = (struct rand_object *)malloc(sizeof(*obj));
    rand_init(obj, seed);
    obj->LastInterval = 0;
    return obj;
}

static void
rand_release(struct rand_object *obj) {
    free(obj);
}

// lua binding

struct box_rand {
    struct rand_object *obj;
};

static int
lnew(lua_State *L) {
    int seed = luaL_checkinteger(L, 1);
    struct box_rand *box = lua_newuserdata(L, sizeof(*box));
    box->obj = rand_new(seed);
    lua_pushvalue(L, lua_upvalueindex(1));
    lua_setmetatable(L, -2);
    return 1;
}

static int
ldelete(lua_State *L) {
    struct box_rand *box = lua_touserdata(L, 1);
    rand_release(box->obj);
    box->obj = NULL;
    return 0;
}

static int
linit(lua_State *L) {
    struct box_rand *box = lua_touserdata(L, 1);
    int seed = luaL_checkinteger(L, 2);
    rand_init(box->obj, seed);
    return 0;
}

static int
lrandi(lua_State *L) {
    struct box_rand *box = lua_touserdata(L, 1);
	int min = luaL_checkinteger(L, 2);
    int max = luaL_checkinteger(L, 3);
    int r = rand_i(box->obj, min, max);
    lua_pushinteger(L, r);
    return 1;
}

static int 
lrandf(lua_State *L) {
	struct box_rand *box = lua_touserdata(L, 1);
	double r = rand_f(box->obj);
	lua_pushnumber(L, r);
	return 1;
}

static int
lrandx(lua_State *L) {
	struct box_rand *box = lua_touserdata(L, 1);
	int min = luaL_checkinteger(L, 2);
	int max = luaL_checkinteger(L, 3);
	int r = rand_x(box->obj, min, max);
	lua_pushinteger(L, r);
	return 1;
}

int
luaopen_rand(lua_State *L) {
    luaL_Reg l[] = {
        { "new", lnew },
        { "init", linit },
        { "randi", lrandi },
		{ "randf", lrandf },
		{ "randx", lrandx },
        { NULL, NULL },
    };
#ifdef luaL_checkversion
    luaL_checkversion(L);
#endif
    lua_createtable(L, 0, 5);
    lua_createtable(L, 0, 1);
    lua_pushcfunction(L, ldelete),
	lua_setfield(L, -2, "__gc");
    luaL_setfuncs(L, l, 1);
    return 1;
}

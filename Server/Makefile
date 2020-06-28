include platform.mk

LUA_CLIB_PATH ?= ./luaclib

CFLAGS = -g -O2 -Wall -I$(LUA_INC) $(MYCFLAGS)

LUA_INC ?= ./skynet/3rd/lua

LUA_CLIB = rand cjson uuid timestamp

all : \
  $(foreach v, $(LUA_CLIB), $(LUA_CLIB_PATH)/$(v).so)

$(LUA_CLIB_PATH) :
	mkdir $(LUA_CLIB_PATH)

$(LUA_CLIB_PATH)/rand.so : lualib-src/lua-rand.c | $(LUA_CLIB_PATH)
	$(CC) $(CFLAGS) $(SHARED) $^ -o $@

$(LUA_CLIB_PATH)/uuid.so : lualib-src/lua-uuid.c | $(LUA_CLIB_PATH)
	$(CC) $(CFLAGS) $(SHARED) $^ -o $@

$(LUA_CLIB_PATH)/timestamp.so : lualib-src/lua-timestamp.c | $(LUA_CLIB_PATH)
	$(CC) $(CFLAGS) $(SHARED) $^ -o $@
	
$(LUA_CLIB_PATH)/cjson.so : 3rd/lua-cjson/lua_cjson.c 3rd/lua-cjson/strbuf.c 3rd/lua-cjson/fpconv.c | $(LUA_CLIB_PATH)
	$(CC) $(CFLAGS) $(SHARED) -DNDEBUG -I3rd/lua-cjson $^ -o $@

clean :
	rm -f $(foreach v, $(LUA_CLIB), $(LUA_CLIB_PATH)/$(v).so)

cleanall : clean
	cd ./skynet && $(MAKE) cleanall

-- This is generate by program !!!
-- Don't change it manaully. 

---@class Common_Item_Bag
local bagClou = {
    ID = 1,
    Order = 2,
    Name = 3,
    Includ = 5,
}

---@class Common_Item_ItemType
local ItemTypeClou = {
    ID = 1,
    Name = 2,
    BagTab = 5,
    SortIndex = 6,
}

local empty_table = {}

---@class Common_Item
local tc = {
    ["Bag"] = {
        ['clou'] = bagClou
    },
    ["ItemType"] = {
        ['clou'] = ItemTypeClou
    },
    ["Currency"] = {
        ['clou'] = {
            ID = 1,
        },
    },
    ["Item"] = {
        ['clou'] = {
            ID = 1,
            Name = 3,
            Description = 5,
            Type = 6,
            Para1 = 7,
            Para2 = 8,
            Para3 = 9,
            LevelLimit = 10,
            AutoUse = 11,
            Show = 12,
            Countdown = 13,
            Period = 14,
            Expire = 15,
            Exchange = 16,
            Quality = 17,
            OverlayNum = 18,
            Icon = 19,
            IngotPrice = 20,
            SilveringotPrice = 21,
            SliverPrice = 22,
            Prompt = 23,
            Way = 24,
        },
    },
    ["Way"] = {
        ['clou'] = {
            ID = 1,
        },
    },
    ["ItemQuality"] = {
        ['clou'] = {
            ID = 1,
            Background = 2,
            Name = 3,
        },
    },
    ["Parameter"] = {
        ['clou'] = {
            ID = 1,
            Parameter = 2,
        },
    },
}
local t = {
    ---@type table<number, Common_Item_Bag>
    ["Bag"] = {
        --ID: int    包裹ID
        --Order: int    包裹排序
        --Name: int    包裹名称
        --Includ: list_int    包含道具

        [1] = {
            [1] = 1,
            [2] = 1,
            [3] = 0,
            [5] = empty_table,
        },
        [2] = { 
            [1] = 2,
            [2] = 2,
            [3] = 0,
            [5] = empty_table,
        },
        [3] = { 
            [1] = 3,
            [2] = 3,
            [3] = 0,
            [5] = empty_table,
        },
        [4] = { 
            [1] = 4,
            [2] = 4,
            [3] = 0,
            [5] = empty_table,
        },
        [5] = { 
            [1] = 5,
            [2] = 5,
            [3] = 0,
            [5] = empty_table,
        },
    },

    ---@type table<number, Common_Item_ItemType>
    ["ItemType"] = {
        --ID: int    类型ID
        --Name: string    显示类型
        --BagTab: list_int    背包页签
        --SortIndex: int    排序先后
        [100] = { 
            [1] = 100,
            [2] = [===[货币]===],
            [5] = empty_table,
            [6] = 0,
        },
        [101] = { 
            [1] = 101,
            [2] = [===[经验]===],
            [5] = empty_table,
            [6] = 0,
        },
        [102] = { 
            [1] = 102,
            [2] = [===[材料]===],
            [5] = empty_table,
            [6] = 0,
        },
        [103] = { 
            [1] = 103,
            [2] = [===[材料]===],
            [5] = empty_table,
            [6] = 0,
        },
        [200] = { 
            [1] = 200,
            [2] = [===[礼包]===],
            [5] = empty_table,
            [6] = 0,
        },
        [201] = { 
            [1] = 201,
            [2] = [===[礼包]===],
            [5] = empty_table,
            [6] = 0,
        },
        [202] = { 
            [1] = 202,
            [2] = [===[礼包]===],
            [5] = empty_table,
            [6] = 0,
        },
        [203] = { 
            [1] = 203,
            [2] = [===[经验]===],
            [5] = empty_table,
            [6] = 0,
        },
        [301] = { 
            [1] = 301,
            [2] = [===[头像]===],
            [5] = empty_table,
            [6] = 0,
        },
        [302] = { 
            [1] = 302,
            [2] = [===[头像框]===],
            [5] = empty_table,
            [6] = 0,
        },
        [303] = { 
            [1] = 303,
            [2] = [===[时装]===],
            [5] = empty_table,
            [6] = 0,
        },
        [304] = { 
            [1] = 304,
            [2] = [===[武器时装]===],
            [5] = empty_table,
            [6] = 0,
        },
        [400] = { 
            [1] = 400,
            [2] = [===[装备]===],
            [5] = empty_table,
            [6] = 0,
        },
        [401] = { 
            [1] = 401,
            [2] = [===[武器]===],
            [5] = empty_table,
            [6] = 0,
        },
        [500] = { 
            [1] = 500,
            [2] = [===[角色碎片]===],
            [5] = empty_table,
            [6] = 0,
        },
        [501] = { 
            [1] = 501,
            [2] = [===[道具碎片]===],
            [5] = empty_table,
            [6] = 0,
        },
    },

    ["Currency"] = {
        --ID: int    道具ID
    },

    ["Item"] = {
        --ID: int    道具ID
        --Name: string    名称
        --Description: string    描述
        --Type: int    类型
        --Para1: string    参数1
        --Para2: string    参数2
        --Para3: string    参数3
        --LevelLimit: int    使用等级
        --AutoUse: int    获得时自动使用
        --Show: int    是否显示在背包
        --Countdown: list_int    道具时限1
        --Period: list_int    道具时限2
        --Expire: int    到期处理
        --Exchange: list_int    到期兑换
        --Quality: int    品质
        --OverlayNum: int    叠加数量上限
        --Icon: string    icon资源
        --IngotPrice: float    钻石价格
        --SilveringotPrice: float    绑钻价格
        --SliverPrice: float    绑金价格
        --Prompt: int    获得提示
        --Way: list_int    获取途径
        [100001] = { 
            [1] = 100001,
            [3] = [===[]===],
            [5] = [===[]===],
            [6] = 100,
            [7] = [===[]===],
            [8] = [===[]===],
            [9] = [===[]===],
            [10] = 0,
            [11] = 0,
            [12] = 0,
            [13] = {1, 30, },
            [14] = {2020, 7, 30, 10, 30, 2020, 7, 30, 10, 30, },
            [15] = 0,
            [16] = empty_table,
            [17] = 0,
            [18] = 0,
            [19] = [===[]===],
            [20] = 0.0,
            [21] = 0.0,
            [22] = 0.0,
            [23] = 0,
            [24] = empty_table,
        },
    },

    ["Way"] = {
        --ID: int    ID
    },

    ["ItemQuality"] = {
        --ID: int    品质ID
        --Background: string    品质框
        --Name: int    品质名称
        [1] = { 
            [1] = 1,
            [2] = [===[]===],
            [3] = 0,
        },
        [2] = { 
            [1] = 2,
            [2] = [===[]===],
            [3] = 0,
        },
        [3] = { 
            [1] = 3,
            [2] = [===[]===],
            [3] = 0,
        },
        [4] = { 
            [1] = 4,
            [2] = [===[]===],
            [3] = 0,
        },
        [5] = { 
            [1] = 5,
            [2] = [===[]===],
            [3] = 0,
        },
        [6] = { 
            [1] = 6,
            [2] = [===[]===],
            [3] = 0,
        },
    },

    ["Parameter"] = {
        --ID: int    ID
        --Parameter: list_int    参数
    },

}

local pagefun  = function (pagetable,pagetablecol)

    local keyfun = function (table, key)

        if(pagetable == nil) then
            error("pagetable nil")
        end

        if(pagetablecol == nil) then
            error("pagetablecol nil")
        end


        if(pagetablecol["clou"] == nil)  then
            error("pagetablecol  clou nil")
        end


        if( pagetablecol["clou"][key] == nil) then
            return nil
        end


        local mykey = pagetablecol["clou"][key]
        if( mykey == nil) then
            return nil
        else
            return table[mykey]
        end
    end

    local base = {
        __index = keyfun,
         __newindex = function()
        end
    }

    for k,v in pairs(pagetable) do
        if(k ~= "clou")then
            setmetatable(v,base);
        end
    end

     --通过index 寻找列名
    local indextoname = {

    }
    for k,v in pairs(pagetablecol["clou"]) do
        indextoname[v] = k
    end

    --用于列名查找
    local pagekey = {
        __index = function (table, key)
            if(key == "key_name") then
                return indextoname
            end
        end
    }

    setmetatable(pagetable,pagekey);

end

for k, v in pairs(t) do
    pagefun(v,tc[k])
end

return t

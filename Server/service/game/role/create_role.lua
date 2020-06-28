--[[
    author:shenyi
    time:2019-02-13 22:51:42
]]

local skynet = require "skynet"
local snax = require "skynet.snax"
local uuid = require "utils.uuid"
local settingTable = require "table.Setting"

local mongod
local uuid_inst

function init( ... )
	mongod = snax.uniqueservice("common/mongod")
    uuid_inst = uuid.New()
end

function response.create(account, args)
	--是否为空
    if args.nickname == nil or args.nickname == "" then
        return { ok = false, errCode = ERROR_CODE.Name_Is_Null }
    end
    
    -- if condition then
    --     --长度,屏蔽字等等
    --     return { ok = false, errCode = errors.Name_UnLegal }
    -- end

    --是否存在
    local accountInfo = mongod.req.findOne("account", { Account = account })
    if accountInfo then
        return { ok = false, errCode = ERROR_CODE.Account_Already_Exist }
    end

    --同名
    local attribInfo = mongod.req.findOne("role_attrib", { Name = args.nickname })
    if attribInfo then
        return { ok = false, errCode = ERROR_CODE.Account_Already_Name }
    end
    
    --生成roleId
    local ok, roleId = pcall(uuid_inst.Gen, uuid_inst)
    if not ok  then
        LOG_ERROR("gen uid error")
        return { ok = false, errCode = ERROR_CODE.UnKnow_Error }
    end

    
    local initHeros = settingTable.InitHero
    local heroPackage = {}
    for _,heroId in pairs(initHeros) do
        local ok, uid = pcall(uuid_inst.Gen, uuid_inst)
        if not ok  then
            LOG_ERROR("gen uid error")
            return { ok = false, errCode = ERROR_CODE.UnKnow_Error }
        end
        heroPackage[tostring(uid)] = {
            Id = tostring(uid),
            ConfigId = heroId,
            Level = 1,
            Star = 1,
            TotalFight = 100
        }
    end

    local initItems = settingTable.InitItem
    local itemPackage = {}
    for _,itemInfo in pairs(initItems) do
        local ok, uid = pcall(uuid_inst.Gen, uuid_inst)
        if not ok  then
            LOG_ERROR("gen uid error")
            return { ok = false, errCode = ERROR_CODE.UnKnow_Error }
        end
        itemPackage[tostring(uid)] = {
            Id = tostring(uid),
            ConfigId = itemInfo[1],
            Count = itemInfo[2]
        }
    end

    local result1 = mongod.req.insert("account", { Account = account, Channel = args.channel, RoleId = roleId })
    local result2 = mongod.req.insert("role_attrib", { 
        RoleId = roleId,
        Name = args.nickname,
        Vip = 1,
        TotalFight = 0,
        Progress = 1,
        PVPScore = 0,
        HeadIconId = settingTable.InitHeadIcon,
        HeadFrameId = settingTable.InitHeadFrame,
        Crystal = 0,
        Gold = 0,
        Silver = 0,
        Achive = { 0 },
        Guide = { 0 },
        GuildEnable = false,
        VipExp = 0,
        VipGift = 0,
        MouthCard = 0,
        Emotion = { 0 }, 
        GuildId = 0,
        DaySign = 0
    })
    local result3 = mongod.req.insert("hero_package", { RoleId = roleId, Package = heroPackage })
    local result4 = mongod.req.insert("item_package", { RoleId = roleId, Package = itemPackage })
    return { ok = result1 and result2 and result3 and result4 }
end

function exit( ... )
	-- body
end

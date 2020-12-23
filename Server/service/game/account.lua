local skynet = require "skynet"
local snax = require "skynet.snax"
local setting = require "config.Setting"
local job_config = require "config.Job"

local game_config
local all_job_configs = {}
---@type Mongod_Req
local mongod_req
---@type Mongod_Post
local mongod_post
local snax_uid

---@class Account_Req
local response = response
---@class Account_Post
local accept = accept

function init( ... )
    game_config = require(skynet.getenv("config"))
    snax_uid = snax.uniqueservice("common/uid")
    local snax_mongod = snax.uniqueservice("common/mongod")
    mongod_req = snax_mongod.req
    mongod_post = snax_mongod.post

    for k, v in pairs(job_config) do
        all_job_configs[k] = require("config." .. v.Name)
    end
end

---@public 后期这样添加属性?
local function getDefaultAttrib(attrib)
    if not attrib.sceneId then
        attrib.sceneId = setting.DefaultScene
    end
end

---@return table<number, RoleInfo>
function response.get_role_list(account)
    local list = mongod_req.findOne("role", account)

    return list
end

---@return RoleInfo
function response.create_role(account, job, name)
    ---TODO 验证名字合法性
    ---再确认一次
    local config = all_job_configs[job]
    if  not config then
        return false
    end

    local roleId = tostring(snax_uid.req.gen("role"))
    ---@class RoleAttrib
    local attrib = {
        name           = name,
        job            = job,
        level          = 1,
        exp            = 0,
        vip            = 0,
        crystal        = 0,
        gold           = 0,
        silver         = 0,
        daySign        = 0,
        headIconId     = 0,
        headFrameId    = 0,
        sceneId        = setting.DefaultScene,
        achive         = 0,
        vipExp         = 0,
        vipGift        = 0,
        mouthCard      = 0,
        ---这里先写死
        modelId        = job_config[job].ModelId,
    }
    getDefaultAttrib(attrib)
    ---@class RoleTrans
    local trans =
    {
        pos_x          = -9.848041,
        pos_y          = -1.09,
        pos_z          = -2.997883,
        forward         = 0,
    }

    local config_lv1 = config[1]
    ---@class RoleStatus
    local status =
    {
        str = config_lv1.STR,
        mag = config_lv1.MAG,
        dex = config_lv1.DEX,
        max_hp = config_lv1.HP,
        hp = config_lv1.HP,
        max_mp = config_lv1.MP,
        mp = config_lv1.MP,
        atn = config_lv1.STR,
        int = config_lv1.INT,
        def = config_lv1.DEF,
        res = config_lv1.RES,
        spd = config_lv1.SPD,
        crt = config_lv1.CRT,
    }

    ---@class RoleInfo
    local roleInfo = {
        roleId = roleId,
        status = status,
        trans = trans,
        attrib = attrib,
        itemPackage = {}
    }
    local role_list = mongod_req.findOne("role", account)

    if not role_list then
        role_list = { account = account, [roleId] = roleInfo }
        mongod_req.insert("role", role_list)
    else

        mongod_req.update("role", account, { [roleId] = roleInfo })
    end

    return true, roleInfo
end

function accept.save_role(account, roleInfo)
    mongod_req.update("role", account, { [roleInfo.roleId] = roleInfo })
end

function exit( ... )

end
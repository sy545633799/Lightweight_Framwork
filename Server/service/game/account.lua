local skynet = require "skynet"
local snax = require "skynet.snax"
local setting = require "config.Setting"

local config
local snax_mongod, snax_uid

function init( ... )
    config = require(skynet.getenv("config"))
    snax_uid = snax.uniqueservice("common/uid")
    snax_mongod = snax.uniqueservice("common/mongod")
end

function exit( ... )

end

local function getDefaultAttrib(attrib)
    if not attrib.scene then
        attrib.scene = config.DefaultScene
    end


end


function response.get_roleInfo(account)
    local roleInfo = snax_mongod.req.findOne("role", { account = account })
    if roleInfo then
        roleInfo.account = nil
    else
        getDefaultAttrib(roleInfo.attrib)
    end
    return roleInfo
end

--TODO 这个函数是否要锁一下
 function response.create_role(account, name)
     ---TODO 验证名字合法性
     -----插入
     --mongod.req.insert("test", { userid = 1, money = 123, age = 28 , test = {
     --	["1001"] = { id = "1001", star = 1, level = 1},
     --	["1002"] = { id = "1002", star = 1, level = 1}
     --}})
     -----增加
     --mongod.post.update("test", { userid = 1 }, { ["$set"] = { ["test.1003"] = { id = "1003", star = 1, level = 1} } })
     -----删除
     --mongod.post.update("test", { userid = 1 }, { ["$unset"] = { ["test.1003"] = 1 } })
     -----更改
     --mongod.post.update("test", { userid = 1 }, { ["$set"] = { ["test.1001.star"] = 3 } })
     ---再确认一次
     local role_attrib = snax_mongod.req.findOne("role", { account = account })
     if not role_attrib then
         local roleId = tostring(snax_uid.req.gen("role"))
         local attrib = {
             roleId = roleId,
             name = name,
             level = 1,
             exp = 0,
             vip = 0,
             totalFight = 0,
             progress = 0,
             pVPScore = 0,
             headIconId = 1,
             headFrameId = 1,
             crystal = 0,
             gold = 0,
             silver = 0,
             energy = 0,
             achive = 0,
             guide = 0,
             vipExp = 0,
             vipGift = 0,
             mouthCard = 0,
             emotion = 0,
             guildId = 0,
             daySign = 0
         }
         getDefaultAttrib(attrib)

         local roleInfo = {
             account = account,
             attrib = attrib,
             heroPackage = {},
             itemPackage = {}
         }
         snax_mongod.req.insert("role", roleInfo)
         roleInfo.account = nil
         return true, roleInfo
    else
         return false
    end
end
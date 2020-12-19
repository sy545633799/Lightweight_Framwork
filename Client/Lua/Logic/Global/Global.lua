---框架
require "Framework/Framework"
local registerModule = RegisterModule
local registerGlobalModule = RegisterGlobalModule
---Util
registerModule "Logic/Global/PropertyNames"
registerModule("Logic/Common/LogicUtil")
registerModule("Logic/Common/ConfigUtil")
registerModule("Logic/Entity/EntityType")
registerModule("Logic/Entity/PropertyNames")
registerModule("Logic/Entity/Entity")

---Model
---@type TestModel
TestModel = registerGlobalModule("Logic/Model/TestModel")
---@type RoleModel
RoleModel = registerGlobalModule("Logic/Model/RoleModel")

---Controller
---@type TestController
TestController = registerGlobalModule("Logic/Controller/TestController")
---@type LoginController
LoginController = registerGlobalModule("Logic/Controller/LoginController")
---@type RegisterController
RegisterController = registerGlobalModule("Logic/Controller/RegisterController")
---@type AOIController
AOIController = registerGlobalModule("Logic/Controller/AOIController")
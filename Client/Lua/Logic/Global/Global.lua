---框架
require "Framework/Framework"
local registerModule = RegisterModule
local registerGlobalModule = RegisterGlobalModule
---Util
registerModule "Logic/Global/PropertyNames"
registerModule("Logic/Common/LogicUtil")
registerModule("Logic/Common/ConfigUtil")

---Model
---@type TestModel
TestModel = registerGlobalModule("Logic/Model/TestModel")
---@type RoleModel
RoleModel = registerGlobalModule("Logic/Model/RoleModel")
---@type ChapterModel
ChapterModel = registerGlobalModule("Logic/Model/ChapterModel")

---Controller
---@type TestController
TestController = registerGlobalModule("Logic/Controller/TestController")
---@type LoginController
LoginController = registerGlobalModule("Logic/Controller/LoginController")
---@type RegisterController
RegisterController = registerGlobalModule("Logic/Controller/RegisterController")
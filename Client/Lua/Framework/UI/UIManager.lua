--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by shenyi.
--- DateTime: 2020/3/20 15:43

require("Framework.UI.LayerGroup")
require("Framework.UI.UIConfig")
---@class UIManager:Updatable
local UIManager = BaseClass("UIManager", Updatable)
local uiutil = UIUtil
local commonUtil = CommonUtil
local resourceManager = ResourceManager

function UIManager:ctor(config)
    self.canvas = {}
    self.loadedCtrl = {}
    self.root = commonUtil.FindGo("Canvas")
    for name, path in pairs(LayerGroup) do
        self.canvas[name] = CommonUtil.FindGo(self.root, path)
        self.loadedCtrl[name] = {}
    end
    self.ctrlInfo = {}
end

function UIManager:IsViewOpen(config)
    local viewInfo = self.ctrlInfo[config]
    if viewInfo then
        return viewInfo.isLoaded and not viewInfo.isLoading
    else
        return false
    end
end

local function CreateCtrl(self, config, go, active)
    commonUtil.SetParent(self.canvas[config.Layer], go)
    local view = require(config.View).New(go)
    view:SetActive(active)
    local ctrl = require(config.Ctrl).New(view)
    self.loadedCtrl[config] = ctrl
    return ctrl
end

function UIManager:LoadView(config, ...)
    --正在加载
    if not self.ctrlInfo[config] then
        self.ctrlInfo[config] = {}
    end

    self.ctrlInfo[config].isLoaded = true
    if self.ctrlInfo[config].isLoading then
        self.ctrlInfo[config].args = LuaUtil.SafePack(...) return
    else
        local ctrl = self.loadedCtrl[config]
        if ctrl then
            ctrl:Load(...) return
        else
            self.ctrlInfo[config].args = LuaUtil.SafePack(...)
        end
    end

    self.ctrlInfo[config].isLoading = true
    resourceManager.LoadPrefab(config.Path, function (go)

        local ctrlInfo = self.ctrlInfo[config]
        ctrlInfo.isLoading = false
        if IsNull(go) then
            ctrlInfo.isLoaded = false
            return
        end

        if not ctrlInfo.isLoaded then
            if config.DestroyWhenUnload or ctrlInfo.destroy then
                resourceManager.UnloadAssets(go)
                return
            else
                CreateCtrl(self, config, go, false)
                return
            end
        end

        local ctrl = CreateCtrl(self, config, go, true)
        ctrl:Load(LuaUtil.SafeUnpack(self.ctrlInfo[config].args))
        ctrlInfo.isLoaded = true
        EventManager:Broadcast(EventNames.UI.LoadUI, config)
    end)
end

function UIManager:UnLoadView(config, destroy)
    local ctrlInfo = self.ctrlInfo[config]
    if not ctrlInfo then return end
    if ctrlInfo.isLoading then
        ctrlInfo.destroy = destroy or false
        ctrlInfo.isLoaded = false
        return
    end

    local ctrl = self.loadedCtrl[config]
    if ctrl then
        if ctrlInfo.isLoaded then
            ctrl:UnLoad()
            ctrlInfo.isLoaded = false
            EventManager:Broadcast(EventNames.UI.UnloadUI, config)
        end
        if config.DestroyWhenUnload or destroy then
            ctrl:Delete()
            self.loadedCtrl[config] = nil
        end
    end
end

function UIManager:UnLoadViewByLayer(layer, destroy)
    for config, info in pairs(self.ctrlInfo) do
        if config.Layer == layer and not info.isLoading then
            self:UnLoadView(config, destroy)
        end
    end
end

function UIManager:UnLoadAllView(destroy)
    for config, info in pairs(self.ctrlInfo) do
        self:UnLoadView(config, destroy or false)
    end
end

return UIManager
---------------------------------------------------
--- Created by shenyi
--- DateTime: 2019.6.5
---------------------------------------------------
---@class UIContent:ModelListener
UIContent = BaseClass("UIContent", ModelListener)
--------------------------------------------------------------------------
local commonUtil = CommonUtil
local uiUtil = UIUtil

function UIContent:ctor(container,...)
    ---@type table<string, UIContent>
    self.components = {}
    ---@type boolean
    self.enaled = false
end

function UIContent:SetActive(bVal)
    logError("请使用canvasgroup")
end

---@protected
function UIContent:Reset()
    for _, compMap in pairs(self.components) do
        for comp, _ in pairs(compMap) do
            comp:Reset()
        end
    end
    self:RemoveAllTimer()
    self:RemoveAllTweener()
    self:RemoveAllEventListener()
    self:RemoveAllModelListener()
end

-------------------------------------------------------组件相关-------------------------------------------------------
---@public 添加组件
---@return UIContent
function UIContent:AddComponent(class,comp,...)
    if not IsClass(class) then
        logError("界面添加类型错误")
        return
    end
    local comp = class.New(comp,...)
    if not self.components[class.__cname] then
        self.components[class.__cname] = {}
    end
    self.components[class.__cname][comp] = true
    return comp
end

function UIContent:RemoveComponent(comp)
    if not IsInstance(comp) or not self.components[comp._class_type.__cname] then
        logError("删除的组件为空， 或者没有找到对应类型")
        return
    end
    self.components[comp._class_type.__cname][comp] = nil
    comp:Delete()
end

function UIContent:RemoveAllComponents()
    for _, compMap in pairs(self.components) do
        for comp, _ in pairs(compMap) do
            comp:Delete()
        end
    end
    self.components = {}
end

---@return UIWidget
function UIContent:AddWidget(component)
    return self:AddComponent(UIWidget, component)
end

---@return UIText
function UIContent:AddText(component)
    return self:AddComponent(UIText, component)
end

---@return UIButton
function UIContent:AddButton(component)
    return self:AddComponent(UIButton, component)
end

---@return UIImage
function UIContent:AddImage(component)
    return self:AddComponent(UIImage, component)
end

---@return UIToggle
function UIContent:AddToggle(component)
    return self:AddComponent(UIToggle, component)
end

---@return UIToggleGroup
function UIContent:AddToggleGroup(component)
    return self:AddComponent(UIToggleGroup, component)
end


---@return UIDropDown
function UIContent:AddDropDown(component)
    return self:AddComponent(UIDropDown, component)
end

---@return UISlider
function UIContent:AddSlider(component)
    return self:AddComponent(UISlider, component)
end

---@return UIListView
function UIContent:AddListView(component)
    return self:AddComponent(UIListView, component)
end

---@return UIInput
function UIContent:AddInput(component)
    return self:AddComponent(UIInput, component)
end

---@return UICanvasGroup
function UIContent:AddCanvasGroup(component)
    return self:AddComponent(UICanvasGroup, component)
end

---@return UIModel
function UIContent:AddModel(component)
    return self:AddComponent(UIModel, component)
end

--Animation
function UIContent:PlayUIAnimation(name,reverse,speed,during)
    if self.Animation and type(name) == 'string' then
        local rate = 1
        if speed and type(speed) == 'number' then
            rate = speed
        end
        local time = 0
        if during and type(during) == 'number' then
            time = during
        end
        --UIUtil.PlayAnimation(self.Animation,name,rate,time,reverse)
    end
end

---删除预制物(删除所有组件后再执行这个方法)
function UIContent:OnDestroy() end

function UIContent:dtor()
    self:RemoveAllComponents()
    self:OnDestroy()
end

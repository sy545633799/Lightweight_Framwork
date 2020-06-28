---------------------------------------------------
--- Created by shenyi
--- DateTime: 2019.6.5
---------------------------------------------------
---@class UIContent:ModelListener
UIContent = BaseClass("UIContent", ModelListener)
--------------------------------------------------------------------------
--子类实现
function UIContent:OnCreate(...) end

function UIContent:ctor(comp,...)
    self.gameObject = comp.gameObject
    self.transform = comp.transform
    self.components = {}
    self:OnCreate(comp, ...)
    self.active = true
end

function UIContent:SetActive(bVal)
    --self.gameObject:SetActive(bVal)
    if bVal and not self.active then
        self.gameObject:SetActive(true)
        self.active = true
        --TODO 是否统一操作
        for _, compMap in pairs(self.components) do
            for comp, _ in pairs(compMap) do
                comp:SetActive(bVal)
            end
        end
    elseif not bVal and self.active then
        for _, compMap in pairs(self.components) do
            for comp, _ in pairs(compMap) do
                comp:SetActive(bVal)
            end
        end
        self:RemoveAllTimer()
        self:RemoveAllTweener()
        self:RemoveAllEventListener()
        self:RemoveAllModelListener()
        self.active = false
        self.gameObject:SetActive(false)
    end
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

--子类实现
function UIContent:OnDestroy() end

function UIContent:dtor()
    self:RemoveAllComponents()
    self:OnDestroy()
end

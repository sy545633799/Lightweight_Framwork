---------------------------------------------------
--- Created by shenyi
--- DateTime: 2019.6.5
---------------------------------------------------
------@class UITabCtrl:UIBaseCtrl
UITabCtrl = BaseClass("UITabCtrl", UIBaseCtrl)
------------------------------------------------------------------------
----------------------------子界面相关逻辑--------------------------------
local loadTabCount = 0

function UITabCtrl:Load(...)
    base.Load(self, ...)
    if self.view.btnClose then
        self.view:RemoveClick(self.view.btnClose)
        self.view:AddClick(self.view.btnClose, function ()
            self:Unload()
            self:DeleteView()
        end)
    end
    --UIManager:PushView(ViewAssets.PlayerResourceUI)
    loadTabCount = loadTabCount + 1
end

---打开BG下不同Tab
function UITabCtrl:OnClickTab(sub, nextTab, ...)
    if not self.tab_open_condition or self.tab_open_condition(sub.SystemId) then
        if self.current_subview == sub then
            return
        end
        --关闭当前Tab窗口（互斥）
        if self.current_subview then
            if self.current_subview.View then
                UIManager:CloseView(self.current_subview.View)
            elseif self.current_subview.ViewAssetsTable then
                for k,v in pairs(self.current_subview.ViewAssetsTable) do
                    UIManager:CloseView(v)
                end
            end
        end
        self.current_subview = sub

        ---定位实际要打开的这个页签的首页
        local FirstViewUI
        local FirstViewUICtrl
        if self.current_subview.View then
            FirstViewUI = sub.View
            FirstViewUICtrl = sub.View[2]
        elseif self.current_subview.ViewAssetsTable then
            FirstViewUI = sub.ViewAssetsTable[1]
            FirstViewUICtrl = sub.ViewAssetsTable[1][2]
        end
        UIManager:PushView(FirstViewUI, nextTab, ...)
    end
end
---初始化Tab，open_condition:开启界面的条件
function UITabCtrl:InitSubTab(subViewInfo, open_condition)
    self.childAssts = {}
    self.tab_open_condition = open_condition

    for i, v in pairs(subViewInfo) do
        if SystemOpenManager:CheckSystem(i) then
            if (v.View or v.ViewAssetsTable) then
                self.childAssts[i] = v
            else
                logError("传入类型缺少必要属性 View or ViewAssetsTable ")
            end
        else
            print("系统未开启: " .. i)
        end
    end
end

---firstSysId:同BG下的大类SysId，secondSysId:同BG下的firstSysId的小类SysId
function UITabCtrl:OpenViewByTab(firstSysId, secondSysId, ...)
    if self.childAssts and table.count(self.childAssts) > 0 then
        local sub = self.childAssts[firstSysId]
        if sub and (not self.tab_open_condition or self.tab_open_condition(sub.SystemId)) then
            self:OnClickTab(sub, secondSysId, ...)
        else
            for index, tab in pairs(self.childAssts) do
                if not self.tab_open_condition or self.tab_open_condition(tab.SystemId) then
                    self:OpenViewByTab(index, secondSysId, ...)
                    return
                end
            end
            logError("未找到已经开放并且能够打开的界面")
        end
    else
        logError("当前BG下没有已开放功能")
    end
end

function UITabCtrl:UnLoad()
    loadTabCount = loadTabCount - 1
    if loadTabCount <= 0 then
        --UIManager:UnloadView(ViewAssets.PlayerResourceUI)
    end

    if self.current_subview then
        self.current_subview = nil
    end

    if self.childAssts then
        --子界面处理
        for k, v in pairs(self.childAssts) do
            if v.View then
                UIManager:UnloadView(v.View)
            elseif v.ViewAssetsTable then
                for m, n in pairs(v.ViewAssetsTable) do
                    UIManager:UnloadView(n)
                end
            end
        end
    end
    base.Unload(self)
end

return UITabCtrl
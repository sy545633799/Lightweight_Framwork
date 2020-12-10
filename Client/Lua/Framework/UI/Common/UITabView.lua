---------------------------------------------------
--- view基类
--- Created by shenyi
--- DateTime: 2019.7.15
---------------------------------------------------
local base = UIBaseView
---@class UITabView:UIBaseView
UITabView = BaseClass("UITabView", base)
---------------------------------------------------
---
local tabGroup =
{
    [1] = {"canvasGroup_tab01", "btn_unselect01", "text_unselect01", "canvasGroup_select01", "text_select01" },
    [2] = {"canvasGroup_tab02", "btn_unselect02", "text_unselect02", "canvasGroup_select02", "text_select02" },
    [3] = {"canvasGroup_tab03", "btn_unselect03", "text_unselect03", "canvasGroup_select03", "text_select03" },
    [4] = {"canvasGroup_tab04", "btn_unselect04", "text_unselect04", "canvasGroup_select04", "text_select04" },
    [5] = {"canvasGroup_tab05", "btn_unselect05", "text_unselect05", "canvasGroup_select05", "text_select05" },
    [6] = {"canvasGroup_tab06", "btn_unselect06", "text_unselect06", "canvasGroup_select06", "text_select06" },
    [7] = {"canvasGroup_tab07", "btn_unselect07", "text_unselect07", "canvasGroup_select07", "text_select07" },
    [8] = {"canvasGroup_tab08", "btn_unselect08", "text_unselect08", "canvasGroup_select08", "text_select08" },
}

--[[viewInfo = {
    [key] = {   --自定义key
        Btn =   --页签顺序，从1开始连续整数
        Tab =   --功能UI序号
        Text =  Btn文字
        Info =  --自定义传到Tab信息，可为nil
        Verify = 效验是否能跳转到功能UI,可为nil
    }
}]]--

---@public 通过页签Id打开界面
function UITabView:SelectTab(btnIndex, ...)
    assert(type(btnIndex) == "number" and btnIndex >= 1 and btnIndex <=8 and self[tabGroup[btnIndex][2]])
    local verify = self.tab2view[btnIndex].Verify
    if verify and not verify(btnIndex) then
        return
    end
    if self.btnIndex then
        self[tabGroup[self.btnIndex][4]]:SetActive(false)
        local tabItem = self[self.tab2view[self.btnIndex].Tab]
        tabItem:Dispose()
        tabItem:SetActive(false)
    end

    self[tabGroup[btnIndex][4]]:SetActive(true)
    local tab = self.tab2view[btnIndex].Tab
    self[tab]:SetActive(true)
    local info = self.tab2view[btnIndex].Info
    if info then
        self[tab]:Refresh(info,...)
    else
        self[tab]:Refresh(...)
    end
    self.btnIndex = btnIndex
end

---@private 重置所有页签 无法关闭子界面
function UITabView:ResetAllTab()
    for i = 1, 8 do
        if self[tabGroup[i][1]] then
            self[tabGroup[i][4]]:SetActive(false)
        end
    end
    self.btnIndex = nil
    self.tab2view = nil
end

---@public 设置页签(子类构造函数中调用)
---@param tabInfo  table 手动设置tabs信息
function UITabView:InitTabs(tab2view)
    self:ResetAllTab()
    local len = table.count(tab2view)
    for i = 1, 8 do
        if self[tabGroup[i][1]] then
            self[tabGroup[i][2]]:RemoveAllClick()
            if i <= len then
                self[tabGroup[i][1]]:SetActive(true)
            else
                self[tabGroup[i][1]]:SetActive(false)
            end
        end
    end

    self.tab2view = tab2view
    for index, info in pairs(tab2view) do
        self.tab2view[index] = info
        --初始化的子界面才关闭，其他不处理
        self[info.Tab]:SetActive(false)
        local tabTexts = tabGroup[index]
        self[tabTexts[2]]:AddClick(self.SelectTab, self, index)
        if info.Text then
            self[tabTexts[3]]:SetTextID(info.TextID)
            self[tabTexts[5]]:SetTextID(info.TextID)
        end
    end

    if self.btn_close then
        self.btn_close:AddClick(self.Close, self)
    end
end

function UITabView:UnLoad()
    base.UnLoad(self)
    self:ResetAllTab()
end

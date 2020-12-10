---@class UIDropDown:UIContent
UIDropDown = BaseClass("UIDropDown", UIContent)

function UIDropDown:ctor(component)
    self.component = component
end

function UIDropDown:SetValue(value)
    self.component.value = value
end

function UIDropDown:AddIdxChgCb(callback)
    --self.component.onValueChanged:AddListener(function(index)
    --    callback(index+1,self.component.gameObject)
    --end)
end

function UIDropDown:SetOptionsList(optionList)
    self.component:ClearOptions()
    local compType=CS.Game.UIDropDown
    local list = compType.OptionDataList()
    for i, option in ipairs(optionList) do
        local data = compType.OptionData()
        data.text = option.Text
        list.options:Add(data)
    end

    self.component.options = list.options
end

-- 销毁
function UIDropDown:dtor()

end
---@class UIListView:UIContent
UIListView = BaseClass("UIListView", UIContent)
-- 创建
function UIListView:OnCreate(list_view)
	self.unity_listview = list_view
end


function UIListView:OnItemSizeChanged(index)
	self.unity_listview:OnItemSizeChanged(index - 1)
end

function UIListView:InitListView(itemTotalCount, onGetItemByIndex)
	self.unity_listview:InitListView(itemTotalCount, function (listview, index)
		return onGetItemByIndex(listview, index + 1)
	end)
end

function UIListView:SetListItemCount(count, resetPos)
	self.unity_listview:SetListItemCount(count, resetPos or false)
end
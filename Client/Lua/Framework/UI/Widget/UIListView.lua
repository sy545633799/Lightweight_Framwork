---@class UIListView:UIContent
UIListView = BaseClass("UIListView", UIContent)
-- 创建
function UIListView:ctor(component)
	---@type SuperScrollView.LoopListView2
	self.component = component
	self.TotalItemCount = 0
end


function UIListView:OnItemSizeChanged(index)
	self.component:OnItemSizeChanged(index - 1)
end

function UIListView:InitListView(itemTotalCount, onGetItemByIndex, handle)
	self.TotalItemCount = 0
	self.component:InitListView(itemTotalCount, function (listview, index)
		if handle then
			return onGetItemByIndex(handle, listview, index + 1)
		else
			return onGetItemByIndex(listview, index + 1)
		end
	end)

end

function UIListView:MovePanelToItemIndex(targetIndex)
	self.component:MovePanelToItemIndex(targetIndex, 0)
end


function UIListView:SetListItemCount(count, resetPos)
	self.TotalItemCount = count
	self.component:SetListItemCount(count, resetPos or false)
end

function UIListView:RefreshAllShownItem()
	self.component:RefreshAllShownItem()
end

function UIListView:dtor()
	--self.component:RemoveListener()
end
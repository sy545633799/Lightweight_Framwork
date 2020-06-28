local clsClass = BaseClass("HotUpdateFile", BaseModel)

function clsClass:OnCreate()
    self.NeedUpdateTable = {}
    self:InitNeedUpdatePath()
end

function clsClass:InitNeedUpdatePath()
    self.BagUINewCtrlPath = "UI/Controller/BagSys/BagUINewCtrl"
end

function clsClass:RequireModelAgain()
    self.BagUINewCtrl =  require(self.BagUINewCtrlPath)
end

function clsClass:SetNeedUpdateTable()
    self:RequireModelAgain()
    self.NeedUpdateTable[self.BagUINewCtrlPath] = {
        ["TestNum"] = 99999,
        ["TestHotFix"] = function()
            --CommonUIUtil.ShowNotice("热更测试！")
        end,
    }
end


function clsClass:GetNeedUpdateTable()
    return self.NeedUpdateTable
end

return clsClass.New()
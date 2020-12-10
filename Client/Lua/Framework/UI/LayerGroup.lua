---@class LayerGroup
LayerGroup =
{
    Scene ="Scene",
    Damage = "Damage",
    Base = "Base",              --常驻UI界面，不销毁 （只存在隐藏） 如果Base层跳转到Pop层界面，则将整个Base层UI移出视野范围
    Pop = "Pop",                --系统UI界面，与Base层互斥, 每当打开一个pop层ui会自动隐藏当前的pop层ui，关闭当前的pop层ui会返回上一个pop层ui
    Notice = "Notice",
    Guide = "Guide",
    Lock = "Lock",
    Network = "Network",
    Loading = "Loading"
}
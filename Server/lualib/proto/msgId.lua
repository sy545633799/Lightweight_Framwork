
--- 0-->10000 服务器->客户端
--- 10000-->20000 客户端->服务器
--- >20000    RPC消息(ack = req + 1)

NetMsgId = {

    ---同步相关
    asyn_pos = 10101,  --客户端同步消息给服务器

    ---登录相关
    req_login = 20101, --成就消息
    ack_login = 20102,
    req_register = 20103,
    ack_register = 20104,

    
}
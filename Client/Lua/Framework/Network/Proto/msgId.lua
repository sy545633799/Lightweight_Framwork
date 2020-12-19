--- 10000-->20000 (单向)客户端<->服务器
--- >20000    RPC消息(ack = req + 1)

NetMsgId = {

    ---同步相关
    sync_pos = 10101,  --客户端同步消息给服务器
    ---同步状态
    sync_status = 10102, --服务端发送玩家状态给客户端

    sync_create_entity = 10103,
    sync_delete_entity = 10104,

    ---登录相关
    req_login = 20101,
    ack_login = 20102,
    req_register = 20103,
    ack_register = 20104,
    req_enter_game = 20105,
    ack_enter_game = 20106,
}
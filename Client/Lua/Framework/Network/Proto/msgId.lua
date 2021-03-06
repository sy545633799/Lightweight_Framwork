--- <10000          客户端->服务器
--- 10000-->20000   服务端->客户端
--- >20000          RPC消息(ack = req + 1)

NetMsgId = {

    ---同步相关
    c2s_sync_trans = 101,  --客户端同步消息给服务器

    ---同步状态
    s2c_aoi_trans = 10101, --服务端发送玩家状态给客户端
    s2c_create_entities = 10102,
    s2c_delete_entities = 10103,

    ---登录相关
    req_role_list = 20101,
    ack_role_list = 20102,
    req_login = 20103,
    ack_login = 20104,
    req_create_role = 20105,
    ack_create_role = 20106,
    req_enter_game = 20107,
    ack_enter_game = 20108,
}
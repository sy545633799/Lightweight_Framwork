local msg = [[

.role_attrib {
	name 0 : string
	job 1 : integer
	level 2 : integer
	exp 3 : integer
	vip 4 : integer
	crystal 5 : integer
	gold 6 : integer
	silver 7 : integer
	daySign 8 : integer
	headIconId 9 : integer
	headFrameId 10 : integer
    sceneId 11 : integer
	achive 12 : integer
	vipExp 13 : integer
	vipGift 14 : integer
	mouthCard 15 : integer
	modelId 16 : integer
}

.role_status {
    str 0 : double
    mag 1 : double
    dex 2 : double
    max_hp 3 : double
    hp 4 : double
    max_mp 5 : double
    mp 6 : double
    atn 7 : double
    int 8 : double
    def 9 : double
    res 10 : double
    spd 11 : double
    crt 12 : double
}

.equipExtraAttrib {
	key 0 : integer
	value 1 : integer
	des 2 : string
}

.item {
	id 0 : string
	configId 1 : integer
	mainType 2 : integer
	count 3 : integer
	extraAttrib 4 : *equipExtraAttrib
}

.roleInfo {
    roleId 0 : string
    attrib 1 : role_attrib
    status 2 : role_status
	itemPackage 3 : *item(id)
}

.roleAttrib {
    roleId 0 : string
    attrib 1 : role_attrib
}

.req_role_list {
    account 0 : string
}

.ack_role_list {
    list 0 : *roleAttrib
}

.req_login {
	roleId 0 : string
}

.ack_login {
	roleInfo 0 : roleInfo
}

.req_create_role {
    job 0 : integer
	nickname 1 : string
}

.ack_create_role {
	error 0 : integer
	roleInfo 1 : roleInfo
}


.sync_attrib {
    paramId 0 : string
    type 1 : integer
    name 2 : string
    modelId 3 : integer
}

.aoiAttrib {
    aoiId 0 : integer
    attrib 1 : sync_attrib
}

.sync_trans {
    pos_x 0 :double
    pos_y 1 :double
    pos_z 2 :double
    forward 3 :double
}

.c2s_sync_trans {
    trans 0 : sync_trans
}

.aoiTrans {
    aoiId 0 : integer
    trans 1 : sync_trans
}

.s2c_aoi_trans {
    list 0 : *aoiTrans(aoiId)
}

.sync_status {
    max_hp 0 : double
    hp 1 : double
    is_crit 2 : boolean
}

.aoiStatus {
    aoiId 0 : integer
    status 1 : sync_status
}

.s2c_aoi_status {
    list 0 : *aoiStatus(aoiId)
}

.aoiData {
    aoiId 0 : integer
    attrib 1 : sync_attrib
    trans 5 : sync_trans
}

.ack_enter_game {
    ok 0 : boolean
    aoi_map 1 : *aoiData(aoiId)
}

.s2c_create_entities{
    data 0 : *aoiData(aoiId)
}

.s2c_delete_entities{
    ids 0 : *integer
}



]]

return msg
local config = [[
.package {
	type 0 : integer
	session 1 : integer
}

push 1 {
	request {
		text 0 : string
	}
}

.mainAttrib {
	RoleId 0 : string
	Name 1 : string
	Level 2 : integer
	Exp 3 : integer
	Vip 4 : integer
	TotalFight 5 : integer
	Progress 6 : integer
	PVPScore 7 : integer
	HeadIconId 8 : integer
	HeadFrameId 9 : integer

	Crystal 10 : integer
	Gold 11 : integer
	Silver 12 : integer
	Energy 13 : integer
	Achive 14 : *integer
	Guide 15 : *integer
	GuildEnable 16 : boolean
	VipExp 17 : integer
	VipGift 18 : integer
	MouthCard 19 : integer
	Emotion 20 : *integer
	GuildId 21 : integer
	DaySign 22 : integer
}

.hero {
	Id 0 : string
	ConfigId 1 : integer
	Level 2 : integer
	Star 3 : integer
	TotalFight 4 : integer
}

.chip {
	Id 0 : string
	ConfigId 1 : integer
	Count 3 : integer
}

.item {
	Id 0 : string
	ConfigId 1 : integer
	MainType 2 : integer
	Count 3 : integer
}

.equipExtraAttrib {
	Key 0 : integer
	Value 1 : integer
}

.equip {
	Id 0 : string
	ConfigId 1 : integer
	Star 2 : integer
	FeatureId 3 : integer
	ExtraAttrib 4 : *equipExtraAttrib
}

ack_roleInfo 1001 {
	request {
		Attrib 0 : mainAttrib
		HeroPackage 1 : *hero(Id)
		ChipPackage 2 : *chip(Id)
		ItemPackage 3 : *item(Id)
		EquipPackage 4 : *equip(Id)
	}
}


]]

return config
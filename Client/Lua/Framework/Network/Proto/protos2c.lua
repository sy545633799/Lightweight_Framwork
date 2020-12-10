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

.stringVal {
	key 0 : string
	val 1 : string
}

.integerVal {
	key 0 : string
	val 1 : integer
}

ack_updateAttrib 101 {
	request {
	    intAttrib   0 : *integerVal
		strAttrib   1 : *stringVal
		msg         2 : integer
	}
}

]]

return config
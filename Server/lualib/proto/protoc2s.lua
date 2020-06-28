local config = [[
.package {
	type 0 : integer
	session 1 : integer
}

ping 101 {}

req_handshake 102 {
	request {
		handshake 0 : string
	}
	response {
		result 0 : string
		time 1 : integer
		hasAccount 2 : boolean
	}
}

req_register 103 {
	request {
		nickname 0 : string
		channel 1 : integer
	}
	response {
		ok 0 : boolean
		errCode 1 : integer
	}
}

req_login 104 {
	request {

	}
	response {
		ok 0 : boolean
		errCode 1 : integer
	}
}

]]

return config
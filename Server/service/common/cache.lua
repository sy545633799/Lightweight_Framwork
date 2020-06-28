--[[
    author:shenyi
    time:2018-12-26 10:35:51
]]

local skynet = require "skynet"
local sharedata = require "skynet.sharedata"

local sprotoparser = require "sprotoparser"
local sproto = require "sproto"
local c2s = require "proto.protoc2s"
local s2c = require "proto.protos2c"

skynet.start(function()
    local setting = require("table.Setting")
    local heroInfo = require("table.HeroInfo")

    --非连续的键会报错
    -- sharedata.new("Setting", setting)
    -- local setting = sharedata.query("Setting")

end)

-----插入
--mongod.req.insert("role", { account = 1, money = 123, age = 28 , test = {
--	["1001"] = { id = "1001", star = 1, level = 1},
--	["1002"] = { id = "1002", star = 1, level = 1}
--}})
-----增加
--mongod.post.update("role", { account = 1 }, { ["$set"] = { ["test.1003"] = { id = "1003", star = 1, level = 1} } })
-----删除
--mongod.post.update("role", { account = 1 }, { ["$unset"] = { ["test.1003"] = 1 } })
-----更改
--mongod.post.update("role", { account = 1 }, { ["$set"] = { ["test.1001.star"] = 3 } })

local skynet = require "skynet"
local mongo = require "skynet.db.mongo"
-- local json = require "cjson"

local DEBUG
local bson_encode

---@class Mongod_Req
local response = response
---@class Mongod_Post
local accept = accept


local db, db_name
function init( ... )
	DEBUG = skynet.getenv("DEBUG")
	if DEBUG then
		local bson = require "bson"
		bson_encode = bson.encode
	end

	local config = require(skynet.getenv "config")
	db_name = config.db.dbname
	local host = config.db.host
	local port = config.db.port
	local username = config.db.username
	local password = config.db.password
	db = mongo.client({ host = host, port = port })
	
	if db == nil then
		skynet.error("mongod failed to connect")
	end

	-- print(db)
end

function exit( ... )
	-- body
end

-- function response.error( ... )
-- 	error "throw and error"
-- end

function response.insert( tableName, tbl )
	if DEBUG then
		local ok = pcall(bson_encode, tbl)
		if not ok then
			skynet.error("mongod insert:", tostring(tbl))
		end
	end

	
	local ret = db[db_name][tableName]:safe_insert(tbl)
	-- skynet.error(ret)
	-- if ret == nil or ret.n ~= 1 then
	-- 	skynet.error("mongod insert, errno:", json.encode(ret), json.encode(tbl))
	-- end
	return ret
end

-- 异步更新
-- upsert:如果查找不到记录是否生成新的记录
-- multi:是否更新多条，默认一条
function accept.update( tableName, keyName, tbl, upsert, multi )
	if DEBUG then
		local ok = pcall(bson_encode, tbl)
		if not ok then
			skynet.error("mongod update:", keyName, tostring(tbl))
		end
	end

	if upsert == nil then upsert = true end
	db[db_name][tableName]:update({ uid = keyName }, { ["$set"] = tbl }, upsert, multi)
end

-- 同步更新
function response.update( tableName, keyName, tbl, upsert, multi )
	if DEBUG then
		local ok = pcall(bson_encode, tbl)
		if not ok then
			skynet.error("mongod update:", keyName, tostring(tbl))
		end
	end

	if upsert == nil then upsert = true end
	local ret = db[db_name][tableName]:update({ uid = keyName }, { ["$set"] = tbl }, upsert, multi)
	--if ret == nil then
	--	skynet.error("mongod update:", keyName, tostring(tbl))
	--end

	return ret
end

-- sort:{key:-1} -1表示倒序，默认增序
-- skip: n 表示跳过n个
-- limit: n 表示只显示n个
function response.find( tableName, keyName, sort, skip, limit, selector )
	local ret = db[db_name][tableName]:find( { uid = keyName }, selector)
	if ret == nil then
		skynet.error("mongod update:", keyName)
	end

	if sort then
		ret = ret:sort(sort)
	end

	if skip then
		ret = ret:skip(skip)
	end

	if limit then
		ret = ret:limit(limit)
	end

	local result = {}
	while ret:hasNext() do
		local item = ret:next()
		table.insert(result, item)
	end

	return result
end

function response.findOne( tableName, keyName, selector )
	local info = db[db_name][tableName]:findOne({ uid = keyName }, selector)
	if info then
		info._id = nil
	end
	return info
end

function response.findMulti( tableNameList, tbl )
	local result = {}
	for k, tableName in ipairs(tableNameList) do
		local info = db[db_name][tableName]:findOne(tbl)
		if info then
			info._id = nil
		end
		result[k] = info
	end

	return result
end

function response.findAndModify( tableName, keyName )
	local ret = db[db_name][tableName]:findAndModify({ uid = keyName })
	if ret == nil then
		skynet.error("mongod findAndModify, errno:", keyName)
	end

	return ret
end

-- single:1 表示删除一条
function response.delete( tableName, tbl, single )
	local ret = db[db_name][tableName]:delete(tbl, single)
	if ret == nil then
		skynet.error("mongod delete, errno:", tostring(tbl))
	end

	return ret
end


-- 大量删除数据，同步删表重建
function accept.drop( tableName )
	local ret = db[db_name][tableName]:drop()
	if ret == nil then
		skynet.error("mongod drop, errno:", tableName)
	end

	return ret
end

-- 创建索引
function accept.ensureIndex( tableName, tbl, options )
local ret = db[db_name][tableName]:ensureIndex(tbl, options)
if ret == nil then
skynet.error("mongod ensureIndex, errno:", tableName)
end

return ret
end


-- 关闭服务器的时候调用
function response.stop( ... )
	skynet.error("mongod stoping...", skynet.mqlen())
	while skynet.mqlen() > 0 do
		skynet.sleep(100)
	end

	return 0
end
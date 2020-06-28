# -*- coding: utf-8 -*-
import os
import os.path
import sys
import shutil
type = sys.getfilesystemencoding()

sourceDir = "./lualib/proto/"
clientDir = "../Client/Assets/LuaScripts/Network/Proto"

def copyFiles(sourceDir, targetDir):
	for file in os.listdir(sourceDir):
		if ".lua" in file:
			sourceFile = os.path.join(sourceDir,  file) 
			targetFile = os.path.join(targetDir,  file) 
			shutil.copyfile(sourceFile, targetFile)
			print("copy file:{0}".format(file))
copyFiles(sourceDir, clientDir)

# def getstring(s):
# 	print(s.decode('utf-8').encode(type))

# def main():
#     copyFiles(sourceDir, clientDir)
#     if len(sys.argv) == 1:
# 		while True:
# 			cmd = raw_input(getstring("任意键结束"))
# 			break
			
# if __name__ == '__main__':
# 	main()
@echo off

>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"

if '%errorlevel%' NEQ '0' (

echo permission...

goto UACPrompt

) else ( goto gotAdmin )

:UACPrompt

echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"

echo UAC.ShellExecute "%~s0", "", "", "runas", 1 >> "%temp%\getadmin.vbs"

"%temp%\getadmin.vbs"

exit /B

:gotAdmin


set targetName=client_lnk
set targetDir=%~dp0\..\..\%targetName%
set sourceDir=%~dp0\..

mkdir %targetDir%
mklink /d %targetDir%\Assets  %sourceDir%\Assets
mklink /d %targetDir%\Library  %sourceDir%\Library
mklink /d %targetDir%\ProjectSettings  %sourceDir%\ProjectSettings
mklink /d %targetDir%\Packages  %sourceDir%\Packages
mklink /d %targetDir%\Lua  %sourceDir%\Lua
pause
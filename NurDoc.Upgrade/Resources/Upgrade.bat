if not "%1"=="wind" mshta vbscript:createobject("wscript.shell").run("""%~f0"" wind",vbhide)(window.close)&&exit 
@echo off
@echo *********
@echo 正在为您安装新版护理电子病历系统，请稍候...
@echo 版本号：{app_version}
@echo pause

ping -n 1 127.0.0.1

if not exist "{app_path}\Skins" (md "{app_path}\Skins")
xcopy "{app_path}\Upgrade\Skins\*.*" "{app_path}\Skins" /e /q /h /r /y

if not exist "{app_path}\Templets" (md "{app_path}\Templets")
xcopy "{app_path}\Upgrade\Templets\*.*" "{app_path}\Templets" /e /q /h /r /y



xcopy "{app_path}\Upgrade\*.*" "{app_path}" /q /y 
if errorlevel 1 goto err
if errorlevel 2 goto err
if errorlevel 3 goto err
if errorlevel 4 goto err
if errorlevel 5 goto err
echo pause

start "护理电子病历系统" "{app_path}\NurDoc.exe" {app_args}

:err
echo pause
if exist "{app_path}\Upgrade" (rd "{app_path}\Upgrade" /s /q)
if exist "{app_path}\*.zip" (del "{app_path}\*.zip" /q /f)
if exist "{app_path}\NurDocSys.xml" (del "{app_path}\NurDocSys.xml" /q /f)
if exist "{app_path}\Upgrade.bat" (del "{app_path}\Upgrade.bat" /q /f)
if exist "{app_path}\护理电子病历系统.lnk" (del "{app_path}\护理电子病历系统.lnk" /q /f)
if exist "{app_path}\AutoUpgrade.bat" (del "{app_path}\AutoUpgrade.bat" /q /f)

@echo off
echo ===================================
echo Thor Service Installation Script
echo ===================================

:: 检查管理员权限
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
if %errorlevel% neq 0 (
    echo 请以管理员身份运行此脚本!
    echo 右键点击脚本选择"以管理员身份运行"
    pause
    exit /B 1
)

:: 设置服务参数
set SERVICE_NAME=ThorService
set DISPLAY_NAME=Thor AI Gateway Service
set DESCRIPTION="Thor AI Gateway Service for managing AI model access"
set BIN_PATH=%cd%\Thor.Service.exe
echo 正在检查服务是否已存在...

sc query %SERVICE_NAME% > nul
if %errorlevel% equ 0 (
    echo 服务已存在，正在删除旧服务...
    sc stop %SERVICE_NAME%
    sc delete %SERVICE_NAME%
    timeout /t 2 /nobreak > nul
)

echo 正在创建 %SERVICE_NAME% 服务...
sc create %SERVICE_NAME% binPath= "%BIN_PATH%" DisplayName= "%DISPLAY_NAME%" start= auto
sc description %SERVICE_NAME% %DESCRIPTION%

if %errorlevel% neq 0 (
    echo 服务创建失败！请检查路径和权限。
    pause
    exit /B 1
)

echo 正在启动服务...
sc start %SERVICE_NAME%

if %errorlevel% neq 0 (
    echo 服务启动失败！请检查日志获取更多信息。
) else (
    echo 服务安装并启动成功！
)

echo.
echo 服务详情:
echo 名称: %SERVICE_NAME%
echo 显示名称: %DISPLAY_NAME%
echo 可执行文件路径: %BIN_PATH%
echo.

pause

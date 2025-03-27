@echo off
echo ===================================
echo Thor Service Uninstallation Script
echo ===================================

:: 检查管理员权限
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
if %errorlevel% neq 0 (
    echo 请以管理员身份运行此脚本!
    echo 右键点击脚本选择"以管理员身份运行"
    pause
    exit /B 1
)

set SERVICE_NAME=ThorService

echo 正在停止 %SERVICE_NAME% 服务...
sc stop %SERVICE_NAME%
timeout /t 2 /nobreak > nul

echo 正在删除 %SERVICE_NAME% 服务...
sc delete %SERVICE_NAME%

if %errorlevel% neq 0 (
    echo 服务删除失败！
) else (
    echo 服务已成功删除。
)

pause

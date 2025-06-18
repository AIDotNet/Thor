@echo off
setlocal EnableDelayedExpansion

:: 设置构建参数
set CONFIGURATION=Release
set PROJECT_PATH=src\Thor.Service\Thor.Service.csproj
set FRONTEND_PATH=lobe
set OUTPUT_DIR=publish

echo ========================================
echo Thor项目自动化构建脚本
echo ========================================
echo.

:: 检查是否安装了必要的工具
echo 检查必要工具...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [错误] 未找到.NET SDK，请先安装.NET 9.0 SDK
    exit /b 1
)

node --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [错误] 未找到Node.js，请先安装Node.js
    exit /b 1
)

npm --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [错误] 未找到npm，请先安装npm
    exit /b 1
)

echo [✓] 工具检查完成
echo.

:: 清理旧的构建产物
if exist "%OUTPUT_DIR%" (
    echo 清理旧的构建产物...
    rmdir /s /q "%OUTPUT_DIR%"
)
mkdir "%OUTPUT_DIR%"

:: 构建前端
echo ========================================
echo 1. 构建前端项目
echo ========================================
cd "%FRONTEND_PATH%"
if not exist "node_modules" (
    echo 安装前端依赖...
    npm install --force
    if %errorlevel% neq 0 (
        echo [错误] 前端依赖安装失败
        cd ..
        exit /b 1
    )
) else (
    echo 前端依赖已存在，跳过安装
)

echo 构建前端...
npm run build
if %errorlevel% neq 0 (
    echo [错误] 前端构建失败
    cd ..
    exit /b 1
)

cd ..
echo [✓] 前端构建完成

:: 复制前端构建产物到后端wwwroot
echo.
echo 复制前端构建产物到后端...
if exist "src\Thor.Service\wwwroot" (
    rmdir /s /q "src\Thor.Service\wwwroot"
)
mkdir "src\Thor.Service\wwwroot"
xcopy "%FRONTEND_PATH%\dist\*" "src\Thor.Service\wwwroot\" /s /e /y
echo [✓] 前端集成完成

:: 构建后端
echo.
echo ========================================
echo 2. 构建后端项目
echo ========================================

:: 还原NuGet包
echo 还原NuGet包...
dotnet restore "%PROJECT_PATH%"
if %errorlevel% neq 0 (
    echo [错误] NuGet包还原失败
    exit /b 1
)

:: 构建项目
echo 构建项目...
dotnet build "%PROJECT_PATH%" --configuration %CONFIGURATION% --no-restore
if %errorlevel% neq 0 (
    echo [错误] 项目构建失败
    exit /b 1
)

echo [✓] 后端构建完成

:: 发布不同版本
echo.
echo ========================================
echo 3. 发布应用程序
echo ========================================

:: 定义要构建的运行时和模式
set runtimes=win-x64 win-arm64 linux-x64 linux-arm64 osx-x64 osx-arm64
set deployments=framework-dependent self-contained

for %%r in (%runtimes%) do (
    for %%d in (%deployments%) do (
        echo.
        echo 发布 %%r (%%d)...
        
        if "%%d"=="framework-dependent" (
            set SELF_CONTAINED=false
            set SUFFIX=fd
        ) else (
            set SELF_CONTAINED=true
            set SUFFIX=sc
        )
        
        set OUTPUT_PATH=%OUTPUT_DIR%\%%r-!SUFFIX!
        
        if "%%d"=="framework-dependent" (
            dotnet publish "%PROJECT_PATH%" ^
                --configuration %CONFIGURATION% ^
                --runtime %%r ^
                --no-build ^
                --output "!OUTPUT_PATH!" ^
                /p:PublishSingleFile=true ^
                /p:PublishReadyToRun=true
        ) else (
            dotnet publish "%PROJECT_PATH%" ^
                --configuration %CONFIGURATION% ^
                --runtime %%r ^
                --self-contained true ^
                --no-build ^
                --output "!OUTPUT_PATH!" ^
                /p:PublishSingleFile=true ^
                /p:PublishReadyToRun=true ^
                /p:IncludeNativeLibrariesForSelfExtract=true
        )
        
        if !errorlevel! neq 0 (
            echo [错误] %%r (%%d) 发布失败
        ) else (
            echo [✓] %%r (%%d) 发布完成
            
            :: 创建压缩包
            set PACKAGE_NAME=Thor-%%r-!SUFFIX!.zip
            echo 创建压缩包: !PACKAGE_NAME!
            powershell -Command "Compress-Archive -Path '!OUTPUT_PATH!\*' -DestinationPath '%OUTPUT_DIR%\!PACKAGE_NAME!' -Force"
        )
    )
)

echo.
echo ========================================
echo 构建完成！
echo ========================================
echo 构建产物位置: %OUTPUT_DIR%
echo.
echo 发布包说明:
echo - fd = Framework Dependent (依赖框架)
echo - sc = Self Contained (自包含)
echo.
echo 支持的运行时:
echo - win-x64: Windows x64
echo - win-arm64: Windows ARM64
echo - linux-x64: Linux x64
echo - linux-arm64: Linux ARM64
echo - osx-x64: macOS x64
echo - osx-arm64: macOS ARM64
echo.
dir "%OUTPUT_DIR%\*.zip" /b
echo.
pause 
@echo off
chcp 65001 >nul
echo ========================================
echo OriginSystemMod 编译脚本
echo ========================================
echo.

cd /d "%~dp0"

echo 正在编译项目...
echo.

dotnet build "OriginSystemMod.csproj" -c Release --no-incremental

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo 编译成功！
    echo ========================================
    echo.
    echo DLL 位置: bin\Win64_Shipping_Client\OriginSystemMod.dll
    echo.
    pause
) else (
    echo.
    echo ========================================
    echo 编译失败！请检查错误信息
    echo ========================================
    echo.
    pause
)









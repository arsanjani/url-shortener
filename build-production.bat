@echo off
setlocal enabledelayedexpansion

echo ==========================================
echo   ScissorLink Production Build Script
echo ==========================================
echo.

REM Set variables
set BUILD_DIR=publish
set CLIENT_BUILD_DIR=client\build
set SRC_DIR=src
set PUBLISH_CONFIG=Release
set PUBLISH_FRAMEWORK=net9.0

echo [INFO] Starting production build process...
echo.

REM Clean previous build
echo [STEP 1/5] Cleaning previous build artifacts...
if exist "%BUILD_DIR%" (
    echo Removing existing publish directory...
    rmdir /s /q "%BUILD_DIR%"
)
if exist "%CLIENT_BUILD_DIR%" (
    echo Removing existing client build directory...
    rmdir /s /q "%CLIENT_BUILD_DIR%"
)

REM Create publish directory
echo Creating fresh publish directory...
mkdir "%BUILD_DIR%"
echo.

REM Build React frontend
echo [STEP 2/5] Building React frontend for production...
cd client
echo Installing/updating npm dependencies...
call npm install
if !errorlevel! neq 0 (
    echo [ERROR] Failed to install npm dependencies!
    pause
    exit /b 1
)

echo Building React application...
call npm run build
if !errorlevel! neq 0 (
    echo [ERROR] Failed to build React application!
    pause
    exit /b 1
)
cd ..
echo React build completed successfully.
echo.

REM Copy React build to wwwroot
echo [STEP 3/5] Copying React build to backend wwwroot...
if exist "%SRC_DIR%\wwwroot\static" (
    rmdir /s /q "%SRC_DIR%\wwwroot\static"
)
xcopy "%CLIENT_BUILD_DIR%\*" "%SRC_DIR%\wwwroot\" /E /Y /I
if !errorlevel! neq 0 (
    echo [ERROR] Failed to copy React build files!
    pause
    exit /b 1
)
echo React files copied to wwwroot successfully.
echo.

REM Build and publish .NET application
echo [STEP 4/5] Building and publishing .NET application...
cd "%SRC_DIR%"
echo Restoring .NET packages...
dotnet restore
if !errorlevel! neq 0 (
    echo [ERROR] Failed to restore .NET packages!
    pause
    exit /b 1
)

echo Building .NET application in Release mode...
dotnet build --configuration %PUBLISH_CONFIG% --no-restore
if !errorlevel! neq 0 (
    echo [ERROR] Failed to build .NET application!
    pause
    exit /b 1
)

echo Publishing .NET application...
dotnet publish --configuration %PUBLISH_CONFIG% --framework %PUBLISH_FRAMEWORK% --output "..\%BUILD_DIR%" --no-build --self-contained false
if !errorlevel! neq 0 (
    echo [ERROR] Failed to publish .NET application!
    pause
    exit /b 1
)
cd ..
echo .NET application published successfully.
echo.

REM Create additional production files
echo [STEP 5/5] Creating production deployment files...

REM Create web.config for IIS deployment (optional)
echo Creating web.config for IIS deployment...
(
echo ^<?xml version="1.0" encoding="utf-8"?^>
echo ^<configuration^>
echo   ^<location path="." inheritInChildApplications="false"^>
echo     ^<system.webServer^>
echo       ^<handlers^>
echo         ^<add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" /^>
echo       ^</handlers^>
echo       ^<aspNetCore processPath="dotnet" arguments=".\ScissorLink.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess" /^>
echo       ^<security^>
echo         ^<requestFiltering removeServerHeader="true" /^>
echo       ^</security^>
echo     ^</system.webServer^>
echo   ^</location^>
echo ^</configuration^>
) > "%BUILD_DIR%\web.config"

REM Create deployment README
echo Creating deployment instructions...
(
echo # ScissorLink Production Deployment
echo.
echo This folder contains the production-ready build of ScissorLink.
echo.
echo ## Files Included:
echo - ScissorLink.dll - Main application
echo - wwwroot/ - Static web assets including React frontend
echo - web.config - IIS configuration file
echo - All necessary .NET runtime dependencies
echo.
echo ## Deployment Instructions:
echo.
echo ### For IIS:
echo 1. Copy all files to your IIS application directory
echo 2. Ensure .NET 9.0 Runtime is installed on the server
echo 3. Set up the application pool for "No Managed Code"
echo 4. Configure connection strings in appsettings.json if needed
echo.
echo ### For Kestrel/Self-hosted:
echo 1. Copy all files to your server
echo 2. Run: dotnet ScissorLink.dll
echo 3. Configure reverse proxy if needed (nginx/Apache)
echo.
echo ## Configuration:
echo - Update appsettings.json for production settings
echo - Configure database connection strings
echo - Set up HTTPS certificates
echo - Configure logging settings
echo.
echo Build completed on: %date% at %time%
) > "%BUILD_DIR%\DEPLOYMENT-README.md"

REM Create startup script for production
echo Creating production startup script...
(
echo @echo off
echo echo Starting ScissorLink in Production Mode...
echo dotnet ScissorLink.dll
echo pause
) > "%BUILD_DIR%\start-production.bat"

echo.
echo ==========================================
echo   BUILD COMPLETED SUCCESSFULLY!
echo ==========================================
echo.
echo Production files are ready in the '%BUILD_DIR%' directory:
echo.
dir "%BUILD_DIR%" /B
echo.
echo Key files:
echo - ScissorLink.dll         : Main application
echo - wwwroot/               : Web assets and React frontend
echo - web.config             : IIS configuration
echo - appsettings.json       : Application configuration
echo - start-production.bat   : Production startup script
echo - DEPLOYMENT-README.md   : Deployment instructions
echo.
echo The application is ready for deployment to production!
echo.
echo Press any key to exit...
pause >nul

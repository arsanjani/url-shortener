@echo off
echo ScissorLink Docker Production Build
echo ===================================

set IMAGE_NAME=scissorlink
set IMAGE_TAG=latest

echo Building Docker image: %IMAGE_NAME%:%IMAGE_TAG%
echo.

docker build -t %IMAGE_NAME%:%IMAGE_TAG% .

if %errorlevel% neq 0 (
    echo [ERROR] Docker build failed!
    pause
    exit /b 1
)

echo.
echo Docker image built successfully!
echo.
echo To run the container:
echo   docker run -p 8080:8080 %IMAGE_NAME%:%IMAGE_TAG%
echo.
echo To run with environment variables:
echo   docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="your-connection-string" %IMAGE_NAME%:%IMAGE_TAG%
echo.
echo To save the image:
echo   docker save %IMAGE_NAME%:%IMAGE_TAG% -o scissorlink-production.tar
echo.
pause

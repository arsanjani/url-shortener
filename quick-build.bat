@echo off
echo ScissorLink Quick Production Build
echo ==================================

REM Quick build without cleanup for faster iterations
echo Building React frontend...
cd client && npm run build && cd ..

echo Copying to wwwroot...
xcopy "client\build\*" "src\wwwroot\" /E /Y /Q

echo Publishing .NET app...
cd src && dotnet publish -c Release -o "..\publish-quick" && cd ..

echo.
echo Quick build complete! Files in 'publish-quick' directory.
echo Run: cd publish-quick && dotnet ScissorLink.dll
pause

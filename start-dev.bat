@echo off
echo Starting ScissorLink Admin Development Environment
echo.

echo Starting .NET API server...
start "ScissorLink API" cmd /c "cd src && dotnet run"

echo Waiting 5 seconds for API to start...
timeout /t 5 /nobreak >nul

echo Starting React development server...
start "ScissorLink Admin UI" cmd /c "cd client && npm start"

echo.
echo Both servers are starting...
echo API: http://localhost:5000
echo Admin UI: http://localhost:3000
echo.
echo Press any key to exit...
pause >nul

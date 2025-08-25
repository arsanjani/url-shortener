Write-Host "Starting ScissorLink Admin Development Environment" -ForegroundColor Green
Write-Host ""

# Start the .NET API server
Write-Host "Starting .NET API server..." -ForegroundColor Yellow
Start-Process -FilePath "powershell" -ArgumentList "-Command", "cd src; dotnet run" -WindowStyle Normal

# Wait for API to start
Write-Host "Waiting 5 seconds for API to start..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

# Start React development server
Write-Host "Starting React development server..." -ForegroundColor Yellow
Start-Process -FilePath "powershell" -ArgumentList "-Command", "cd client; npm start" -WindowStyle Normal

Write-Host ""
Write-Host "Both servers are starting..." -ForegroundColor Green
Write-Host "API: http://localhost:5000" -ForegroundColor Cyan
Write-Host "Admin UI: http://localhost:3000" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

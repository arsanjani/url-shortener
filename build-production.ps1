# ScissorLink Production Build Script (PowerShell)
param(
    [string]$BuildDir = "publish",
    [string]$Configuration = "Release",
    [string]$Framework = "net9.0",
    [switch]$SkipNpmInstall,
    [switch]$Clean = $true
)

Write-Host "==========================================" -ForegroundColor Green
Write-Host "   ScissorLink Production Build Script" -ForegroundColor Green  
Write-Host "==========================================" -ForegroundColor Green
Write-Host ""

# Set variables
$ClientBuildDir = "client\build"
$SrcDir = "src"
$StartTime = Get-Date

Write-Host "[INFO] Starting production build process..." -ForegroundColor Yellow
Write-Host "Build Directory: $BuildDir" -ForegroundColor Cyan
Write-Host "Configuration: $Configuration" -ForegroundColor Cyan
Write-Host "Framework: $Framework" -ForegroundColor Cyan
Write-Host ""

try {
    # Clean previous build
    if ($Clean) {
        Write-Host "[STEP 1/5] Cleaning previous build artifacts..." -ForegroundColor Yellow
        if (Test-Path $BuildDir) {
            Write-Host "Removing existing publish directory..." -ForegroundColor Gray
            Remove-Item -Path $BuildDir -Recurse -Force
        }
        if (Test-Path $ClientBuildDir) {
            Write-Host "Removing existing client build directory..." -ForegroundColor Gray
            Remove-Item -Path $ClientBuildDir -Recurse -Force
        }
        Write-Host "Creating fresh publish directory..." -ForegroundColor Gray
        New-Item -ItemType Directory -Path $BuildDir -Force | Out-Null
        Write-Host ""
    }

    # Build React frontend
    Write-Host "[STEP 2/5] Building React frontend for production..." -ForegroundColor Yellow
    Push-Location "client"
    
    if (-not $SkipNpmInstall) {
        Write-Host "Installing/updating npm dependencies..." -ForegroundColor Gray
        npm install
        if ($LASTEXITCODE -ne 0) {
            throw "Failed to install npm dependencies!"
        }
    }

    Write-Host "Building React application..." -ForegroundColor Gray
    npm run build
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to build React application!"
    }
    Pop-Location
    Write-Host "React build completed successfully." -ForegroundColor Green
    Write-Host ""

    # Copy React build to wwwroot
    Write-Host "[STEP 3/5] Copying React build to backend wwwroot..." -ForegroundColor Yellow
    $WwwRootStatic = Join-Path $SrcDir "wwwroot\static"
    if (Test-Path $WwwRootStatic) {
        Remove-Item -Path $WwwRootStatic -Recurse -Force
    }
    
    Copy-Item -Path "$ClientBuildDir\*" -Destination "$SrcDir\wwwroot" -Recurse -Force
    Write-Host "React files copied to wwwroot successfully." -ForegroundColor Green
    Write-Host ""

    # Build and publish .NET application
    Write-Host "[STEP 4/5] Building and publishing .NET application..." -ForegroundColor Yellow
    Push-Location $SrcDir
    
    Write-Host "Restoring .NET packages..." -ForegroundColor Gray
    dotnet restore
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to restore .NET packages!"
    }

    Write-Host "Building .NET application in $Configuration mode..." -ForegroundColor Gray
    dotnet build --configuration $Configuration --no-restore
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to build .NET application!"
    }

    Write-Host "Publishing .NET application..." -ForegroundColor Gray
    $PublishPath = "..\$BuildDir"
    dotnet publish --configuration $Configuration --framework $Framework --output $PublishPath --no-build --self-contained false
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to publish .NET application!"
    }
    Pop-Location
    Write-Host ".NET application published successfully." -ForegroundColor Green
    Write-Host ""

    # Create additional production files
    Write-Host "[STEP 5/5] Creating production deployment files..." -ForegroundColor Yellow

    # Create web.config for IIS deployment
    Write-Host "Creating web.config for IIS deployment..." -ForegroundColor Gray
    $WebConfigContent = @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" arguments=".\ScissorLink.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess" />
      <security>
        <requestFiltering removeServerHeader="true" />
      </security>
    </system.webServer>
  </location>
</configuration>
"@
    $WebConfigContent | Out-File -FilePath "$BuildDir\web.config" -Encoding UTF8

    # Create deployment README
    Write-Host "Creating deployment instructions..." -ForegroundColor Gray
    $DeploymentReadme = @"
# ScissorLink Production Deployment

This folder contains the production-ready build of ScissorLink.

## Build Information
- Build Date: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')
- Configuration: $Configuration
- Framework: $Framework
- Build Machine: $env:COMPUTERNAME

## Files Included:
- ScissorLink.dll - Main application
- wwwroot/ - Static web assets including React frontend  
- web.config - IIS configuration file
- All necessary .NET runtime dependencies

## Deployment Instructions:

### For IIS:
1. Copy all files to your IIS application directory
2. Ensure .NET 9.0 Runtime is installed on the server
3. Set up the application pool for "No Managed Code"
4. Configure connection strings in appsettings.json if needed

### For Kestrel/Self-hosted:
1. Copy all files to your server
2. Run: ``dotnet ScissorLink.dll``
3. Configure reverse proxy if needed (nginx/Apache)

### For Docker:
1. Use the provided Dockerfile or create your own
2. Copy publish files to container
3. Expose appropriate ports (80/443)

## Configuration:
- Update appsettings.json for production settings
- Configure database connection strings
- Set up HTTPS certificates  
- Configure logging settings
- Review security settings

## Health Check:
After deployment, verify the application is running by accessing:
- Health endpoint: /health (if configured)
- Main application: / 

## Troubleshooting:
- Check application logs in the logs/ directory
- Verify .NET runtime version compatibility
- Ensure database connectivity
- Check file permissions

For more information, see the main project README.md
"@
    $DeploymentReadme | Out-File -FilePath "$BuildDir\DEPLOYMENT-README.md" -Encoding UTF8

    # Create startup scripts
    Write-Host "Creating production startup scripts..." -ForegroundColor Gray
    
    # Batch file for Windows
    $StartupBat = @"
@echo off
echo Starting ScissorLink in Production Mode...
dotnet ScissorLink.dll
pause
"@
    $StartupBat | Out-File -FilePath "$BuildDir\start-production.bat" -Encoding ASCII

    # PowerShell script for advanced startup
    $StartupPs1 = @"
# ScissorLink Production Startup Script
param(
    [string]`$Environment = "Production",
    [int]`$Port = 5000
)

Write-Host "Starting ScissorLink..." -ForegroundColor Green
Write-Host "Environment: `$Environment" -ForegroundColor Cyan
Write-Host "Port: `$Port" -ForegroundColor Cyan

# Set environment variables
`$env:ASPNETCORE_ENVIRONMENT = `$Environment
if (`$Port -ne 5000) {
    `$env:ASPNETCORE_URLS = "http://*:`$Port"
}

# Start the application
dotnet ScissorLink.dll
"@
    $StartupPs1 | Out-File -FilePath "$BuildDir\start-production.ps1" -Encoding UTF8

    # Calculate build time
    $EndTime = Get-Date
    $BuildTime = $EndTime - $StartTime

    Write-Host ""
    Write-Host "==========================================" -ForegroundColor Green
    Write-Host "   BUILD COMPLETED SUCCESSFULLY!" -ForegroundColor Green
    Write-Host "==========================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Build Time: $($BuildTime.ToString('mm\:ss'))" -ForegroundColor Yellow
    Write-Host "Production files are ready in the '$BuildDir' directory:" -ForegroundColor Yellow
    Write-Host ""
    
    Get-ChildItem -Path $BuildDir -Name | ForEach-Object {
        Write-Host "  $_" -ForegroundColor Cyan
    }
    
    Write-Host ""
    Write-Host "Key files:" -ForegroundColor Yellow
    Write-Host "  ScissorLink.dll         : Main application" -ForegroundColor White
    Write-Host "  wwwroot/               : Web assets and React frontend" -ForegroundColor White
    Write-Host "  web.config             : IIS configuration" -ForegroundColor White
    Write-Host "  appsettings.json       : Application configuration" -ForegroundColor White
    Write-Host "  start-production.bat   : Windows startup script" -ForegroundColor White
    Write-Host "  start-production.ps1   : PowerShell startup script" -ForegroundColor White
    Write-Host "  DEPLOYMENT-README.md   : Deployment instructions" -ForegroundColor White
    Write-Host ""
    Write-Host "The application is ready for deployment to production!" -ForegroundColor Green

} catch {
    Write-Host ""
    Write-Host "==========================================" -ForegroundColor Red
    Write-Host "   BUILD FAILED!" -ForegroundColor Red
    Write-Host "==========================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Build failed after $((Get-Date) - $StartTime)" -ForegroundColor Yellow
    exit 1
}

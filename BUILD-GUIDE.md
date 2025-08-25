# ScissorLink Production Build Guide

This guide explains how to build ScissorLink for production deployment using the provided build scripts.

## Quick Start

For a complete production build, simply run:
```bash
# Windows Command Prompt
build-production.bat

# PowerShell (recommended)
.\build-production.ps1
```

## Available Build Scripts

### 1. `build-production.bat` - Complete Production Build
**Best for: Final production deployment**

Features:
- ✅ Builds React frontend with optimizations
- ✅ Copies frontend to .NET wwwroot
- ✅ Builds and publishes .NET application
- ✅ Creates IIS web.config
- ✅ Generates deployment documentation
- ✅ Creates startup scripts
- ✅ Full error handling and validation

Usage:
```cmd
build-production.bat
```

Output: `publish/` directory with complete production-ready application

### 2. `build-production.ps1` - Advanced PowerShell Build
**Best for: CI/CD pipelines and advanced users**

Features:
- 🚀 All features of batch version plus:
- ⚙️ Configurable parameters
- 📊 Build timing and statistics
- 🎨 Colored output and better formatting
- 🔧 Advanced error handling
- 📝 Detailed build information

Usage:
```powershell
# Basic build
.\build-production.ps1

# Custom configuration
.\build-production.ps1 -BuildDir "release" -Configuration "Release" -SkipNpmInstall

# Skip cleanup for faster builds
.\build-production.ps1 -Clean:$false
```

Parameters:
- `-BuildDir`: Output directory (default: "publish")
- `-Configuration`: Build configuration (default: "Release")
- `-Framework`: Target framework (default: "net9.0")
- `-SkipNpmInstall`: Skip npm install step
- `-Clean`: Clean previous builds (default: true)

### 3. `quick-build.bat` - Fast Development Builds
**Best for: Quick testing and development**

Features:
- ⚡ Minimal build time
- 🔄 No cleanup (faster subsequent builds)
- 📦 Essential files only

Usage:
```cmd
quick-build.bat
```

Output: `publish-quick/` directory

### 4. Docker Build Scripts
**Best for: Containerized deployment**

#### `docker-build.bat` - Build Docker Image
```cmd
docker-build.bat
```

#### `Dockerfile` - Multi-stage Production Build
- Optimized production image
- Security best practices
- Health checks included
- Non-root user

## Build Output Structure

After running the production build, you'll get:

```
publish/
├── ScissorLink.dll              # Main application
├── ScissorLink.deps.json        # Dependencies
├── ScissorLink.runtimeconfig.json
├── appsettings.json             # Configuration
├── web.config                   # IIS configuration
├── wwwroot/                     # Static web assets
│   ├── index.html              # React app entry point
│   ├── static/                 # React build files
│   │   ├── css/
│   │   └── js/
│   ├── favicon.ico
│   └── [other static assets]
├── start-production.bat         # Windows startup script
├── start-production.ps1         # PowerShell startup script
└── DEPLOYMENT-README.md         # Deployment instructions
```

## Deployment Options

### 1. IIS Deployment
1. Copy all files from `publish/` to your IIS application directory
2. Ensure .NET 9.0 Runtime is installed
3. Configure application pool for "No Managed Code"
4. Update `appsettings.json` for production settings

### 2. Self-Hosted (Kestrel)
```cmd
cd publish
dotnet ScissorLink.dll
```

Or use the provided startup scripts:
```cmd
# Windows
start-production.bat

# PowerShell (with custom options)
.\start-production.ps1 -Port 8080 -Environment Production
```

### 3. Docker Deployment
```bash
# Build image
docker-build.bat

# Run container
docker run -p 8080:8080 scissorlink:latest
```

### 4. Cloud Deployment
The `publish/` folder can be deployed to:
- Azure App Service
- AWS Elastic Beanstalk
- Google Cloud Run
- Any cloud provider supporting .NET 9

## Prerequisites

### Development Machine
- ✅ .NET 9.0 SDK
- ✅ Node.js 18+ and npm
- ✅ Git (for source control)

### Production Server
- ✅ .NET 9.0 Runtime (ASP.NET Core)
- ✅ SQL Server (if using database)
- ✅ IIS (for IIS deployment) or reverse proxy

## Configuration

### Environment Variables
```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5000
ConnectionStrings__DefaultConnection=your-connection-string
```

### appsettings.json
Update the following for production:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-production-connection-string"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "your-domain.com"
}
```

## Troubleshooting

### Common Issues

**Build fails with npm errors:**
```cmd
cd client
npm install
npm run build
```

**Missing .NET SDK:**
- Download from https://dotnet.microsoft.com/download
- Verify with `dotnet --version`

**Permission errors:**
- Run as Administrator (Windows)
- Check file permissions

**Build artifacts missing:**
- Ensure no antivirus interference
- Check disk space

### Build Logs
- PowerShell script provides detailed error information
- Check individual command outputs
- Verify all prerequisites are installed

## CI/CD Integration

### GitHub Actions Example
```yaml
- name: Build Production
  run: |
    .\build-production.ps1 -SkipNpmInstall:$false
  shell: powershell
```

### Azure DevOps Example
```yaml
- task: PowerShell@2
  inputs:
    filePath: 'build-production.ps1'
    arguments: '-Configuration Release'
```

## Performance Notes

- React build includes optimizations (minification, tree-shaking)
- .NET build uses Release configuration
- Static files are served efficiently from wwwroot
- Consider enabling gzip compression in production
- Use CDN for static assets in high-traffic scenarios

## Security Considerations

- Remove development certificates from production
- Configure HTTPS properly
- Review security headers in web.config
- Update connection strings and secrets
- Enable request validation
- Configure proper CORS settings

## Support

For issues with the build process:
1. Check this guide and troubleshooting section
2. Verify all prerequisites are installed
3. Review build logs for specific errors
4. Check project README for additional information

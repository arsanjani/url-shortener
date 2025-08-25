# ScissorLink - Advanced URL Shortener

A powerful, enterprise-grade URL shortener built with .NET 9 and modern web technologies. Features comprehensive analytics, admin interface, and high-performance architecture.

## 🚀 Latest Updates (2025)

### Major Architecture Overhaul
- **🏗️ Namespace Modernization** - Migrated from legacy `akhr.ir` to meaningful `ScissorLink` namespace
- **📊 Entity Framework Migration** - Completely replaced Dapper with Entity Framework Core for better maintainability
- **⚛️ React Admin Interface** - Brand new Material-UI admin panel for comprehensive link management
- **🔄 Full-Stack Integration** - Seamless API integration between .NET backend and React frontend
- **🎨 Material Design** - Google Material Design standards throughout the admin interface
- **📱 Responsive Design** - Mobile-first approach with modern UI/UX patterns

### Technical Improvements
- **Upgraded to .NET 9** - Latest long-term support version with improved performance
- **Modern Architecture** - Clean separation with dependency injection and SOLID principles
- **Advanced Analytics** - Enhanced click tracking with detailed visitor information
- **Dynamic Testing** - Domain-agnostic test buttons that work regardless of deployment environment
- **Comprehensive Error Handling** - Modern middleware with detailed logging and user-friendly messages

## ✨ Key Features

### Core URL Shortening
- **High-Performance Redirects** - Optimized for speed with memory caching
- **Custom Tokens** - User-defined short codes or auto-generated secure tokens
- **Publish/Draft System** - Control link availability with publish status
- **Bulk Operations** - Efficient handling of multiple links

### Advanced Analytics
- **🌍 Geolocation Tracking** - Country detection via IP with MaxMind GeoIP2
- **💻 Device Intelligence** - Detailed OS and browser identification
- **📊 Click Analytics** - Comprehensive visitor statistics and patterns
- **📅 Time-Series Data** - Historical tracking with timestamp precision
- **📈 Real-time Metrics** - Live click counts and visitor information

### Admin Interface
- **Modern React UI** - TypeScript-based admin panel with Material-UI
- **CRUD Operations** - Complete link management with intuitive interface
- **Dynamic Testing** - Test links directly from admin panel
- **Copy to Clipboard** - One-click URL sharing
- **Responsive Design** - Works seamlessly on desktop, tablet, and mobile
- **Real-time Updates** - Live status changes and analytics

### Enterprise Features
- **Entity Framework Core** - Robust ORM with migrations and relationship management
- **Dependency Injection** - Clean architecture with testable components
- **CORS Support** - Configured for modern SPA development
- **Error Boundaries** - Comprehensive error handling and user feedback
- **Security Ready** - Input validation, SQL injection prevention, XSS protection

## 🛠️ Technology Stack

### Backend (.NET 9)
- **ASP.NET Core WebAPI** - High-performance web framework
- **Entity Framework Core 9.0** - Modern ORM with advanced features
- **Microsoft.Data.SqlClient 5.2.1** - Latest SQL Server data provider
- **MaxMind.GeoIP2 5.2.0** - IP geolocation services
- **System.Text.Json** - High-performance JSON serialization

### Frontend (React 18)
- **React 18** - Latest React with concurrent features
- **TypeScript 4.9** - Type-safe JavaScript development
- **Material-UI 5.15** - Google Material Design components
- **Axios 1.6** - Promise-based HTTP client
- **React Router 6** - Declarative routing

### Database
- **SQL Server 2016+** - Primary database with full schema
- **Entity Framework Migrations** - Database versioning and updates

## 📋 Prerequisites

- **.NET 9 SDK** - Latest LTS version
- **Node.js 18+** and **npm** - For React development
- **SQL Server 2016+** - Database engine
- **Visual Studio 2022** or **VS Code** (recommended)

## 🚀 Quick Start

### Option 1: Development Mode (Recommended)
Use the provided scripts to start both backend and frontend:

**Windows:**
```bash
# Run as Administrator (if needed)
./start-dev.bat
```

**PowerShell:**
```powershell
# Set execution policy if needed: Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
./start-dev.ps1
```

This will start:
- Backend API at `http://localhost:5000`
- React Admin UI at `http://localhost:3000`

### Option 2: Manual Setup

#### 1. Database Setup
Run the [Script.sql](Script.sql) in your SQL Server database to create the required tables:
- `ShortLink` - Main URL data with enhanced schema
- `ShortLinkDetail` - Analytics and visitor tracking

#### 2. Backend Configuration
Edit the connection string in [src/appsettings.json](src/appsettings.json):
```json
{
  "ConnectionStrings": {
    "dbScissorLink": "Server=localhost;Database=dbScissorLink;User Id=myUser;Password=MyPass;Encrypt=false;TrustServerCertificate=true;Connection Timeout=30;"
  }
}
```

#### 3. Start Backend API
```bash
cd src
dotnet restore
dotnet run
```
API will be available at `http://localhost:5000`

#### 4. Start Frontend Admin Interface
```bash
cd client
npm install
npm start
```
Admin interface will be available at `http://localhost:3000`

## 🏗️ Project Architecture

### Namespace Structure
```
ScissorLink/
├── ScissorLink.Controllers/    # API controllers and endpoints
├── ScissorLink.Services/       # Business logic and caching
├── ScissorLink.Repos/          # Data access with Entity Framework
├── ScissorLink.Models/         # Entity Framework models
├── ScissorLink.DTOs/           # Data Transfer Objects for API
├── ScissorLink.Data/           # DbContext and database configuration
├── ScissorLink.Common/         # Shared utilities and middleware
└── ScissorLink.UserAgent/      # Browser and OS detection
```

### Backend Structure
```
src/
├── Controllers/
│   ├── AdminController.cs      # Admin API endpoints (CRUD)
│   ├── ProcessController.cs    # URL redirection and analytics
│   └── ErrorController.cs      # Error handling
├── Data/
│   └── ScissorLinkDbContext.cs # Entity Framework DbContext
├── DTOs/                       # API Data Transfer Objects
│   ├── ShortLinkRequestDto.cs
│   ├── ShortLinkResponseDto.cs
│   └── ShortLinkDetailDto.cs
├── Models/                     # Entity Framework Models
│   ├── DtoShortLink.cs
│   ├── DtoShortLinkDetail.cs
│   └── DtoResult.cs
├── Services/                   # Business Logic
│   ├── ProcessService.cs
│   └── Interface/
│       └── IProcessService.cs
├── Repos/                      # Data Access Layer
│   ├── ProcessRepo.cs
│   ├── BaseRepo.cs
│   └── Interface/
│       └── IProcessRepo.cs
├── Common/                     # Shared Components
│   └── ErrorHandlingMiddleware.cs
├── UserAgent/                  # Device Detection
│   ├── UserAgent.cs
│   ├── ClientBrowser.cs
│   ├── ClientOS.cs
│   └── MatchExpression.cs
└── wwwroot/                    # Static Files
    └── GeoLite2-Country.mmdb
```

### Frontend Structure
```
client/
├── public/
│   └── index.html              # Main HTML template
├── src/
│   ├── components/
│   │   ├── AddShortLinkDialog.tsx    # Add new link dialog
│   │   └── ShortLinksList.tsx        # Links list with actions
│   ├── pages/
│   │   └── ShortLinksPage.tsx        # Main admin page
│   ├── services/
│   │   └── apiService.ts             # API communication layer
│   ├── types/
│   │   └── api.ts                    # TypeScript type definitions
│   ├── App.tsx                       # Main React component
│   └── index.tsx                     # Application entry point
└── package.json
```

## 🔌 API Documentation

### Admin API Endpoints (`/api/admin`)

| Method | Endpoint | Description | Request Body |
|--------|----------|-------------|--------------|
| `GET` | `/shortlinks` | Get all short links | - |
| `GET` | `/shortlinks/{id}` | Get specific short link | - |
| `POST` | `/shortlinks` | Create new short link | `ShortLinkRequestDto` |
| `PUT` | `/shortlinks/{id}` | Update short link | `ShortLinkRequestDto` |
| `DELETE` | `/shortlinks/{id}` | Delete short link | - |
| `POST` | `/shortlinks/{id}/toggle-publish` | Toggle publish status | - |

### URL Redirection API
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/{token}` | Redirect to original URL and track analytics |

### Data Models

#### ShortLinkRequestDto
```typescript
{
  title?: string;        // Optional friendly name
  token?: string;        // Custom token (auto-generated if empty)
  originLink: string;    // Target URL (required)
  isPublish: boolean;    // Publish status
}
```

#### ShortLinkResponseDto
```typescript
{
  id: number;
  title?: string;
  token: string;
  originLink: string;
  isPublish: boolean;
  createAdminDate: string;
  editAdminDate?: string;
  clickCount: number;
  recentClicks: ShortLinkDetailDto[];
}
```

## 📊 Admin Interface Features

### URL Management
- **📋 List View** - Paginated display with search and filtering
- **➕ Add New** - Modal dialog with real-time validation
- **✏️ Edit Links** - In-place editing with conflict resolution
- **🗑️ Delete** - Confirmation dialogs with cascade handling
- **👁️ Publish Toggle** - One-click status changes

### Analytics Dashboard
- **📈 Click Metrics** - Real-time click counts and trends
- **🌍 Geographic Data** - Visitor country distribution
- **💻 Device Stats** - OS and browser analytics
- **📅 Time Analysis** - Visit patterns and peak hours

### User Experience
- **🎨 Material Design** - Consistent Google Material Design
- **📱 Responsive Layout** - Mobile-first responsive design
- **⚡ Fast Loading** - Optimized bundle sizes and lazy loading
- **🔄 Real-time Updates** - Live data without page refreshes
- **📋 Copy to Clipboard** - One-click URL sharing
- **🔗 Dynamic Testing** - Domain-agnostic test buttons

## 🚀 Deployment

### Production Build

#### Backend
```bash
cd src
dotnet publish -c Release -o ../publish
```

#### Frontend
```bash
cd client
npm run build
# Files will be in client/build/ directory
```

### Docker Deployment
```dockerfile
# Example Dockerfile for backend
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY publish/ .
EXPOSE 80
ENTRYPOINT ["dotnet", "ScissorLink.dll"]
```

### Environment Variables
```bash
# Backend
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__dbScissorLink=<production-connection-string>

# Frontend
REACT_APP_API_URL=https://your-api-domain.com/api
```

## 🔒 Security Features

### Input Validation
- **DTO Validation** - Model validation with annotations
- **URL Validation** - Proper URL format checking
- **Token Sanitization** - Safe character validation
- **SQL Injection Prevention** - Parameterized queries with EF Core

### Security Headers
- **CORS Policy** - Configurable cross-origin requests
- **HTTPS Enforcement** - SSL/TLS for production
- **Error Masking** - Safe error messages for users
- **Rate Limiting Ready** - Infrastructure for request throttling

## 📈 Performance Optimizations

### Backend Performance
- **Memory Caching** - 10-minute cache for frequently accessed links
- **Connection Pooling** - Optimized database connections
- **Async Operations** - Non-blocking I/O throughout
- **EF Core Optimization** - Selective loading and compiled queries

### Frontend Performance
- **Code Splitting** - Lazy loading for optimal bundle sizes
- **Component Memoization** - React.memo for expensive renders
- **Virtual Scrolling** - Efficient large list rendering
- **Service Worker Ready** - PWA capabilities

## 🧪 Development & Testing

### Development Workflow
```bash
# Start development environment
./start-dev.ps1  # or start-dev.bat

# Run backend tests
cd src
dotnet test

# Run frontend tests
cd client
npm test

# Build for production
cd src && dotnet publish -c Release
cd client && npm run build
```

### Database Migrations
```bash
# Add new migration
dotnet ef migrations add NewFeatureName

# Update database
dotnet ef database update

# Generate SQL script
dotnet ef migrations script
```

## 🔮 Future Roadmap

### Near Term (Next Release)
- **🔐 Authentication System** - User registration and role-based access
- **📊 Advanced Analytics** - Charts, graphs, and export functionality
- **🔄 Bulk Operations** - Import/export CSV functionality
- **⏰ Link Expiration** - Time-based link expiration

### Medium Term
- **🏷️ Link Categories** - Organizational tags and folders
- **🔗 Custom Domains** - Branded short URLs with custom domains
- **📱 QR Code Generation** - Automatic QR codes for mobile sharing
- **🔒 Password Protection** - Optional password-protected links

### Long Term
- **📧 Email Integration** - Automated reports and notifications
- **🔌 API Extensions** - Webhook support and third-party integrations
- **🌐 Multi-Language Support** - Full internationalization
- **☁️ Cloud Storage** - Alternative storage backends

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. **Fork the repository**
2. **Create a feature branch**: `git checkout -b feature/amazing-feature`
3. **Commit changes**: `git commit -m 'Add amazing feature'`
4. **Push to branch**: `git push origin feature/amazing-feature`
5. **Open a Pull Request**

### Development Guidelines
- Follow C# coding conventions and .NET best practices
- Use TypeScript for all frontend code
- Write unit tests for new features
- Update documentation for API changes
- Ensure responsive design for UI changes

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **MaxMind** - GeoIP2 database for geolocation services
- **Material-UI Team** - Excellent React component library
- **Microsoft** - .NET platform and Entity Framework
- **React Team** - Amazing frontend framework
- **Community Contributors** - All the developers who contributed

## 📞 Support

- **Documentation**: Check this README and code comments
- **Issues**: Use GitHub Issues for bug reports and feature requests
- **Discussions**: Use GitHub Discussions for questions and ideas

---

**Built with ❤️ using .NET 9, React 18, and modern web technologies**

*ScissorLink - Cutting long URLs down to size since 2025*
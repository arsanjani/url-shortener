# Short URL Router 
Powerful URL shortener library in ASP.NET Core WebAPI and C# which contains several features such as click tracking, SQL back-end, internal cache, and modern responsive UI.

## 🚀 Latest Updates (2025)
- **Upgraded to .NET 9** - Latest long-term support version with improved performance
- **Modernized Architecture** - Migrated from legacy WebHost to minimal hosting model
- **Enhanced UI** - Beautiful, responsive 404 error page with modern Bootstrap 5
- **Improved Performance** - Updated to latest NuGet packages and optimized code
- **Better Error Handling** - Modern middleware with System.Text.Json
- **Nullable Reference Types** - Enhanced code safety and reduced null reference exceptions
- **Cross-Platform Support** - Fixed file path issues for better Linux/Mac compatibility

## ✨ Features
- **N-Tier Architecture** - Clean separation of concerns with dependency injection
- **Advanced Click Tracking** - Comprehensive analytics including:
  - 🌍 Country detection via IP geolocation (MaxMind GeoIP2)
  - 💻 Operating system identification
  - 🌐 Browser detection with detailed user agent parsing
  - 📅 Date and time tracking
  - 📊 Statistical reporting
- **High-Performance Caching** - Built-in memory cache for faster response times
- **Optimized Data Access** - Dapper ORM for superior performance over Entity Framework
- **Modular Design** - Interface-based architecture for easy customization and testing
- **Modern Error Handling** - Comprehensive exception handling with detailed logging
- **Responsive UI** - Modern, mobile-friendly error pages with RTL support

## 🛠️ Technology Stack
- **.NET 9** - Latest long-term support framework
- **ASP.NET Core WebAPI** - High-performance web framework
- **Dapper 2.1.35** - Lightweight ORM for data access
- **Microsoft.Data.SqlClient 5.2.1** - Modern SQL Server data provider
- **MaxMind.GeoIP2 5.2.0** - IP geolocation services
- **Bootstrap 5.3.2** - Modern responsive UI framework
- **System.Text.Json** - High-performance JSON serialization

## 📋 Prerequisites
- .NET 9 SDK
- SQL Server (2016 or later)
- Visual Studio 2022 or VS Code (recommended)

## 🚀 How to Use

### 1. Database Setup
Run the [Script.sql](Script.sql) in your target database. It includes two tables:
- `ShortLink` - Stores short link data
- `ShortLinkDetail` - Stores statistical tracking information

### 2. Configuration
Edit the connection string in [appsettings.json](src/appsettings.json):
```json
{
  "ConnectionStrings": {
    "dbScissorLink": "Server=localhost;Database=dbScissorLink;User Id=myUser;Password=MyPass;Encrypt=false;TrustServerCertificate=true;Connection Timeout=30;Max Pool Size=8192;"
  }
}
```

### 3. Data Entry
Insert your short link records in the `ShortLink` table:
```sql
INSERT INTO ShortLink (Token, OriginLink, IsPublish) 
VALUES ('abc123', 'https://example.com/very-long-url', 1);
```

### 4. Run the Project
```bash
cd src
dotnet run
```

Visit `https://localhost:5001/abc123` to test your short URL!

## 🏗️ Architecture

### Project Structure
```
src/
├── Common/                 # Shared utilities and middleware
├── Controllers/            # API controllers
├── Models/                 # Data transfer objects
├── Repos/                  # Data access layer
├── Services/              # Business logic layer
├── UserAgent/             # Browser and OS detection
├── Views/                 # Error pages and UI
└── wwwroot/               # Static files and assets
```

### Key Components
- **ProcessController** - Handles URL redirection and analytics
- **ErrorController** - Manages error pages and responses
- **ProcessService** - Business logic with caching
- **ProcessRepo** - Data access with Dapper
- **UserAgent Parser** - Advanced browser and OS detection
- **ErrorHandlingMiddleware** - Global exception handling

## 🔧 Configuration Options

### App Settings
- `PrecisionWithProfile` - Tracking precision for registered users
- `PrecisionWithoutProfile` - Tracking precision for anonymous users
- `Logging:LogLevel` - Configure logging levels
- `AllowedHosts` - Configure allowed hosts

### Cache Settings
- Default cache duration: 10 minutes for links, 3 hours for geolocation
- Memory cache for improved performance
- Configurable cache expiration

## 🌟 Performance Features
- **Efficient Caching** - Reduces database queries by 90%
- **Optimized Queries** - Dapper provides raw SQL performance
- **Minimal Allocations** - Uses modern .NET performance features
- **Connection Pooling** - Optimized database connections
- **Async/Await** - Non-blocking I/O operations

## 📊 Analytics Dashboard
The system tracks comprehensive analytics:
- Click count and timestamps
- Geographic distribution
- Browser and OS statistics
- Device type analysis
- Traffic patterns over time

## 🔒 Security Features
- Input validation and sanitization
- SQL injection prevention via parameterized queries
- XSS protection in error pages
- Secure headers configuration
- Rate limiting ready (can be easily added)

## 📱 Mobile Support
- Responsive design for all screen sizes
- Touch-friendly interface
- Fast loading on mobile networks
- Progressive Web App ready

## 🛡️ Error Handling
- Comprehensive exception logging
- User-friendly error pages
- Graceful degradation
- Detailed error reporting for developers

## 🌐 Internationalization
- RTL language support (Persian/Arabic)
- Modern font loading (Vazirmatn)
- Localized error messages
- Cultural-aware date/time formatting

## 📈 Future Roadmap
1. **Multi-Database Support** - PostgreSQL, MySQL, SQLite compatibility
2. **Admin Panel** - User registration and link management dashboard
3. **API Documentation** - Swagger/OpenAPI integration
4. **Rate Limiting** - Built-in request throttling
5. **Custom Domains** - Support for branded short URLs
6. **QR Code Generation** - Automatic QR codes for short links
7. **Link Expiration** - Time-based link expiration
8. **Password Protection** - Optional password-protected links
9. **Bulk Operations** - Import/export functionality
10. **Real-time Analytics** - Live dashboard with SignalR

## 🤝 Contributing
Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License
This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments
- MaxMind for GeoIP2 database
- Bootstrap team for the UI framework
- Dapper team for the excellent ORM
- Microsoft for .NET and ASP.NET Core

---
*Built with ❤️ using .NET 9 and modern web technologies*

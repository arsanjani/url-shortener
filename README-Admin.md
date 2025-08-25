<div align="center">

# 🎨 ScissorLink Admin Interface

[![React](https://img.shields.io/badge/React-18-61dafb?style=for-the-badge&logo=react)](https://reactjs.org/)
[![TypeScript](https://img.shields.io/badge/TypeScript-4.9-3178c6?style=for-the-badge&logo=typescript)](https://www.typescriptlang.org/)
[![Material-UI](https://img.shields.io/badge/Material--UI-5.15-0081cb?style=for-the-badge&logo=mui)](https://mui.com/)
[![Entity Framework](https://img.shields.io/badge/Entity_Framework-9.0-512bd4?style=for-the-badge&logo=microsoft)](https://docs.microsoft.com/en-us/ef/)

**🚀 Modern React admin interface for managing the ScissorLink URL shortener**

*Beautiful, responsive, and powerful admin dashboard built with Material-UI*

[🎯 Features](#-features-implemented) • [🏗️ Setup](#-setup-instructions) • [🔌 API](#-api-endpoints) • [🎨 UI Features](#-uiux-features)

---

</div>

![Admin Dashboard Preview](https://via.placeholder.com/800x400/1976d2/ffffff?text=ScissorLink+Admin+Dashboard)

## 🎯 Features Implemented

<table>
<tr>
<td width="50%">

### 🚀 **Backend Excellence (ASP.NET Core)**

- ✅ **ORM Evolution** → Migrated from Dapper to Entity Framework Core
- ✅ **Schema Harmony** → Updated models to match database schema perfectly
- ✅ **RESTful APIs** → Complete admin API endpoints for CRUD operations
- ✅ **Data Transfer** → Implemented proper DTOs for clean API communication
- ✅ **CORS Ready** → Full support for React development workflow

</td>
<td width="50%">

### ⚛️ **Frontend Power (React + TypeScript + Material-UI)**

- ✅ **Modern Stack** → React 18 application with TypeScript
- ✅ **Material Design** → Google Material Design implementation
- ✅ **Smart Lists** → URL display with search and filtering capabilities
- ✅ **Easy Creation** → Intuitive add new URL functionality
- ✅ **Smart Testing** → Dynamic test buttons work on any domain
- ✅ **Responsive UI** → Perfect on desktop, tablet, and mobile
- ✅ **Live Updates** → Real-time status updates (publish/unpublish)
- ✅ **Analytics View** → Comprehensive click analytics display
- ✅ **Quick Actions** → One-click copy to clipboard functionality

</td>
</tr>
</table>

## Project Structure

### Backend Structure
```
src/
├── Controllers/
│   ├── AdminController.cs      # Admin API endpoints
│   └── ProcessController.cs    # URL redirection logic
├── Data/
│   └── ScissorLinkDbContext.cs # Entity Framework DbContext
├── DTOs/                       # Data Transfer Objects
│   ├── ShortLinkRequestDto.cs
│   ├── ShortLinkResponseDto.cs
│   └── ShortLinkDetailDto.cs
├── Models/                     # Entity Framework models
│   ├── DtoShortLink.cs
│   └── DtoShortLinkDetail.cs
└── Services/                   # Business logic
    ├── ProcessService.cs
    └── Interface/
        └── IProcessService.cs
```

### Frontend Structure
```
client/
├── public/
│   └── index.html
├── src/
│   ├── components/
│   │   ├── AddShortLinkDialog.tsx
│   │   └── ShortLinksList.tsx
│   ├── pages/
│   │   └── ShortLinksPage.tsx
│   ├── services/
│   │   └── apiService.ts
│   ├── types/
│   │   └── api.ts
│   ├── App.tsx
│   └── index.tsx
└── package.json
```

## 🏗️ Setup Instructions

<div align="center">

### **⚡ Quick Setup Guide**

*Get your admin interface running in minutes*

</div>

### 📋 **Prerequisites**

<div align="center">

| Technology | Version Required | Status |
|------------|------------------|--------|
| 🔧 **.NET SDK** | 9.0+ | ![Required](https://img.shields.io/badge/Status-Required-red?style=flat-square) |
| 📦 **Node.js** | 18.0+ | ![Required](https://img.shields.io/badge/Status-Required-red?style=flat-square) |
| 🗄️ **SQL Server** | 2016+ | ![Required](https://img.shields.io/badge/Status-Required-red?style=flat-square) |

*All configured in appsettings.json*

</div>

### 🔧 **Backend Setup**

<details>
<summary><strong>Click to expand backend setup steps</strong></summary>

**Step 1: Restore Dependencies**
```bash
cd src
dotnet restore
```

**Step 2: Update Database (if needed)**
```bash
dotnet ef database update
```

**Step 3: Start the API**
```bash
dotnet run
```

🎉 **Backend ready at:** [`http://localhost:5000`](http://localhost:5000)

</details>

### ⚛️ **Frontend Setup**

<details>
<summary><strong>Click to expand frontend setup steps</strong></summary>

**Step 1: Install Dependencies**
```bash
cd client
npm install
```

**Step 2: Environment Configuration**
```bash
# Create .env file with:
REACT_APP_API_URL=http://localhost:5000/api
```

**Step 3: Start Development Server**
```bash
npm start
```

🎉 **Admin Interface ready at:** [`http://localhost:3000`](http://localhost:3000)

</details>

<div align="center">

### **🚀 Pro Tip**

*Use the automated scripts in the root directory for one-click setup!*

```bash
./start-dev.bat  # Windows
./start-dev.ps1  # PowerShell
```

</div>

## API Endpoints

### Admin API (`/api/admin`)
- `GET /shortlinks` - Get all short links
- `GET /shortlinks/{id}` - Get specific short link
- `POST /shortlinks` - Create new short link
- `PUT /shortlinks/{id}` - Update short link
- `DELETE /shortlinks/{id}` - Delete short link
- `POST /shortlinks/{id}/toggle-publish` - Toggle publish status

### URL Redirection API
- `GET /{token}` - Redirect to original URL and track analytics

## Features

### URL Management
- **List View**: Display all shortened URLs with status, click count, and creation date
- **Add New**: Create new short links with optional custom tokens
- **Edit**: Update existing links (title, original URL, publish status)
- **Delete**: Remove links with confirmation dialog
- **Publish/Unpublish**: Toggle link availability

### Dynamic Testing
- **Test Links**: Each URL has a test button that dynamically uses the current domain
- **Copy to Clipboard**: One-click copying of short URLs
- **Real-time Preview**: See the short URL as you type the custom token

### Analytics
- **Click Tracking**: Display click count for each link
- **Visitor Details**: Show recent clicks with country, OS, and browser information
- **Date Tracking**: Creation and last edit timestamps

### UI/UX Features
- **Material Design**: Google Material Design standards throughout
- **Responsive**: Works on desktop, tablet, and mobile
- **Loading States**: Proper loading indicators for async operations
- **Error Handling**: User-friendly error messages
- **Notifications**: Success/error notifications for all actions

## Database Schema

The application uses two main tables:
- `ShortLink`: Main URL data
- `ShortLinkDetail`: Click analytics and visitor information

## Development Notes

### Entity Framework Migration
The project has been fully migrated from Dapper to Entity Framework Core with:
- Proper entity relationships
- Automatic timestamp handling
- Cascade delete for analytics data
- Optimized queries with `Include()` statements

### CORS Configuration
CORS is configured to allow the React development server (`localhost:3000`) to access the API during development.

### Error Handling
Both frontend and backend include comprehensive error handling:
- API validation errors
- Network connectivity issues
- Database constraint violations
- User-friendly error messages

## Production Deployment

### Backend
1. Build the application:
   ```bash
   dotnet publish -c Release
   ```

2. Update connection strings for production
3. Deploy to your preferred hosting platform

### Frontend
1. Build the React application:
   ```bash
   npm run build
   ```

2. Serve the built files from the `build/` directory
3. Update the API URL environment variable for production

## Security Considerations

- Add authentication/authorization as needed
- Implement rate limiting for API endpoints
- Validate and sanitize all user inputs
- Use HTTPS in production
- Consider implementing API keys or JWT tokens

## Future Enhancements

- User authentication and role-based access
- Bulk operations (import/export)
- Advanced analytics dashboard
- Link expiration dates
- Custom domains support
- QR code generation
- Link categories/tags

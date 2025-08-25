# ScissorLink Admin Interface

This project includes a modern React admin interface for managing the ScissorLink URL shortener application.

## Features Implemented

### Backend (ASP.NET Core)
- ✅ Migrated from Dapper to Entity Framework Core
- ✅ Updated models to match database schema
- ✅ Added admin API endpoints for CRUD operations
- ✅ Implemented proper DTOs for API communication
- ✅ Added CORS support for React development

### Frontend (React + TypeScript + Material-UI)
- ✅ Modern React application with TypeScript
- ✅ Google Material Design implementation
- ✅ URL list display with search and filtering
- ✅ Add new URL functionality
- ✅ Dynamic test link buttons that work regardless of domain
- ✅ Responsive design
- ✅ Real-time status updates (publish/unpublish)
- ✅ Click analytics display
- ✅ Copy to clipboard functionality

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

## Setup Instructions

### Prerequisites
- .NET 9.0 SDK
- Node.js 18+ and npm
- SQL Server (connection string configured in appsettings.json)

### Backend Setup
1. Restore NuGet packages:
   ```bash
   cd src
   dotnet restore
   ```

2. Update database schema (if needed):
   ```bash
   dotnet ef database update
   ```

3. Run the API:
   ```bash
   dotnet run
   ```
   The API will be available at `http://localhost:5000`

### Frontend Setup
1. Install dependencies:
   ```bash
   cd client
   npm install
   ```

2. Create environment file:
   ```bash
   # Create .env file with:
   REACT_APP_API_URL=http://localhost:5000/api
   ```

3. Start the development server:
   ```bash
   npm start
   ```
   The admin interface will be available at `http://localhost:3000`

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

# Error Pages Documentation

## Overview

The ScissorLink Admin Panel now includes a comprehensive error handling system built with React and Material-UI components. This replaces the previous Razor-based 404.cshtml with a modern, responsive, and reusable error page system that follows Google Material Design standards.

## Features

- **Material Design Compliance**: All error pages follow Google's Material Design principles
- **Left-to-Right (LTR) Layout**: English-first design with proper LTR text direction
- **Multiple Error Types**: Support for various HTTP error codes and scenarios
- **Responsive Design**: Works seamlessly across desktop, tablet, and mobile devices
- **Smooth Animations**: Subtle animations and transitions for better UX
- **Error Boundary**: Global error catching for unhandled JavaScript errors
- **Automatic Redirects**: API errors automatically redirect to appropriate error pages

## Error Types Supported

### 1. 404 - Not Found (`NotFoundPage`)
- **Route**: `/error/404` or any unmatched route (`*`)
- **Use Case**: Page or resource not found
- **Actions**: Go Home, Admin Panel
- **Color**: Orange (#ff9800)

### 2. 500 - Server Error (`ServerErrorPage`)
- **Route**: `/error/500`
- **Use Case**: Internal server errors
- **Actions**: Try Again, Go Home
- **Color**: Red (#f44336)

### 3. 403 - Forbidden (`ForbiddenPage`)
- **Route**: `/error/403`
- **Use Case**: Access denied/insufficient permissions
- **Actions**: Go Home, Admin Panel
- **Color**: Pink (#e91e63)

### 4. Network Error
- **Use Case**: Connection issues, offline scenarios
- **Actions**: Try Again, Go Home
- **Color**: Purple (#9c27b0)

### 5. Unknown Error
- **Use Case**: Generic errors, unhandled exceptions
- **Actions**: Try Again, Go Home, Admin Panel
- **Color**: Blue Grey (#607d8b)

## File Structure

```
client/src/
├── components/
│   ├── ErrorPage.tsx          # Main error page component
│   ├── ErrorBoundary.tsx      # React error boundary
│   └── ErrorPageDemo.tsx      # Demo component for testing
├── pages/
│   ├── NotFoundPage.tsx       # 404 error page
│   ├── ServerErrorPage.tsx    # 500 error page
│   └── ForbiddenPage.tsx      # 403 error page
├── utils/
│   └── errorUtils.ts          # Error handling utilities
└── services/
    └── apiService.ts          # Updated with error handling
```

## Usage

### Basic Error Page

```tsx
import ErrorPage from '../components/ErrorPage';

const MyErrorPage: React.FC = () => {
  return (
    <ErrorPage
      errorType="not-found"
      showRefresh={false}
      showHome={true}
      showAdmin={true}
    />
  );
};
```

### Custom Error Page

```tsx
import ErrorPage from '../components/ErrorPage';

const CustomErrorPage: React.FC = () => {
  return (
    <ErrorPage
      errorType="unknown"
      title="Custom Error"
      subtitle="Something specific went wrong"
      description="This is a custom error message for a specific scenario."
      showRefresh={true}
      showHome={true}
      showAdmin={false}
      customActions={
        <Button onClick={() => console.log('Custom action')}>
          Custom Action
        </Button>
      }
    />
  );
};
```

### Error Boundary Usage

The `ErrorBoundary` component is automatically applied in `App.tsx` and will catch any unhandled React errors:

```tsx
<ErrorBoundary>
  <YourAppComponents />
</ErrorBoundary>
```

## API Error Handling

The `apiService.ts` has been enhanced with automatic error handling:

```typescript
// Automatically redirects to error pages for 403, 404, 500 status codes
// Other errors are passed to components for manual handling
```

## Error Utilities

### `redirectToErrorPage(statusCode: number)`
Automatically redirects to the appropriate error page based on HTTP status code.

### `getErrorType(statusCode: number)`
Returns the error type string for a given status code.

### `isErrorStatusCode(statusCode: number)`
Checks if a status code represents an error (>= 400).

### `getErrorMessage(statusCode: number)`
Returns a user-friendly error message for a status code.

## Testing

### Error Page Demo
Visit `/error-demo` in the admin panel to preview all error page types in a dialog interface.

### Direct Access
- `/error/404` - 404 error page
- `/error/500` - 500 error page  
- `/error/403` - 403 error page
- Any invalid route - Automatically shows 404

### Programmatic Testing
```typescript
import { redirectToErrorPage } from '../utils/errorUtils';

// Redirect to specific error page
redirectToErrorPage(404);
redirectToErrorPage(500);
redirectToErrorPage(403);
```

## Customization

### Theme Colors
Error page colors are defined in the `errorConfigs` object in `ErrorPage.tsx`:

```typescript
const errorConfigs: Record<string, ErrorConfig> = {
  'not-found': {
    color: '#ff9800', // Orange
    // ... other config
  },
  // ... other error types
};
```

### Adding New Error Types
1. Add configuration to `errorConfigs` in `ErrorPage.tsx`
2. Create a new page component in `pages/`
3. Add route to `App.tsx`
4. Update error utilities if needed

## Migration from 404.cshtml

The new React error pages replace the previous `src/Views/404.cshtml` file:

### What Changed
- ✅ Modern Material Design UI
- ✅ English LTR layout (was RTL Persian)
- ✅ Multiple error types support
- ✅ React/TypeScript implementation
- ✅ Responsive design
- ✅ Better error handling
- ✅ Integrated with admin panel

### What Stayed
- Modern, attractive design
- Gradient backgrounds
- Smooth animations
- Clear error messaging
- Action buttons for navigation

## Browser Support

- Chrome 60+
- Firefox 55+
- Safari 12+
- Edge 79+

## Accessibility

- ARIA labels and roles
- Keyboard navigation support
- Screen reader compatible
- High contrast support
- Focus management

## Performance

- Lazy loading of error pages
- Optimized bundle size
- Minimal re-renders
- Efficient error boundary implementation

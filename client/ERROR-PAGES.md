<div align="center">

# 🛡️ Error Pages Documentation

[![React](https://img.shields.io/badge/React-18-61dafb?style=for-the-badge&logo=react)](https://reactjs.org/)
[![Material-UI](https://img.shields.io/badge/Material--UI-5.15-0081cb?style=for-the-badge&logo=mui)](https://mui.com/)
[![TypeScript](https://img.shields.io/badge/TypeScript-4.9-3178c6?style=for-the-badge&logo=typescript)](https://www.typescriptlang.org/)
[![Error Handling](https://img.shields.io/badge/Error_Handling-Advanced-ff5722?style=for-the-badge&logo=warning)](https://reactjs.org/docs/error-boundaries.html)

**🚀 Comprehensive error handling system for ScissorLink Admin Panel**

*Beautiful, responsive error pages that turn failures into friendly experiences*

[🎯 Features](#-features) • [🔧 Error Types](#-error-types-supported) • [📁 Structure](#-file-structure) • [🧪 Testing](#-testing)

---

</div>

![Error Pages Preview](https://via.placeholder.com/800x400/f44336/ffffff?text=ScissorLink+Error+Pages)

## 🌟 Overview

The ScissorLink Admin Panel features a **comprehensive error handling system** built with React and Material-UI components. This modern solution replaces the previous Razor-based 404.cshtml with a **responsive, reusable error page system** that follows Google Material Design standards.

## 🎯 Features

<div align="center">

### **🛡️ Advanced Error Handling**

*Turning errors into opportunities for better UX*

</div>

<table>
<tr>
<td width="33%" align="center">

### 🎨 **Design Excellence**

🎯 **Material Design** → Google's design principles
📱 **Responsive Layout** → Perfect on all devices  
🌊 **Smooth Animations** → Subtle UX transitions
🌍 **LTR Support** → English-first design

</td>
<td width="33%" align="center">

### 🔧 **Smart Functionality**

🛡️ **Error Boundaries** → Catch unhandled errors
🔄 **Auto Redirects** → API errors handled gracefully
🎮 **Multiple Types** → Support for all HTTP codes
⚡ **Fast Loading** → Optimized error pages

</td>
<td width="33%" align="center">

### 🚀 **User Experience**

😊 **Friendly Messages** → Clear, helpful error text
🔗 **Action Buttons** → Quick navigation options
📊 **Status Awareness** → Context-aware error handling
💡 **Solution Hints** → Guidance for users

</td>
</tr>
</table>

## 🔧 Error Types Supported

<div align="center">

### **🎯 Complete Error Coverage**

*Every error scenario handled with style*

</div>

<table>
<tr>
<td width="50%">

### 🚨 **HTTP Status Errors**

**🔍 404 - Not Found** ![Orange](https://img.shields.io/badge/Color-Orange-ff9800?style=flat-square)
- **Route**: `/error/404` or unmatched routes (`*`)
- **Scenario**: Page or resource not found
- **Actions**: Go Home, Admin Panel

**💥 500 - Server Error** ![Red](https://img.shields.io/badge/Color-Red-f44336?style=flat-square)
- **Route**: `/error/500`
- **Scenario**: Internal server errors
- **Actions**: Try Again, Go Home

**🚫 403 - Forbidden** ![Pink](https://img.shields.io/badge/Color-Pink-e91e63?style=flat-square)
- **Route**: `/error/403`
- **Scenario**: Access denied/insufficient permissions
- **Actions**: Go Home, Admin Panel

</td>
<td width="50%">

### 🌐 **Application Errors**

**📡 Network Error** ![Purple](https://img.shields.io/badge/Color-Purple-9c27b0?style=flat-square)
- **Scenario**: Connection issues, offline scenarios
- **Actions**: Try Again, Go Home
- **Smart Detection**: Automatic offline detection

**❓ Unknown Error** ![Blue Grey](https://img.shields.io/badge/Color-Blue_Grey-607d8b?style=flat-square)
- **Scenario**: Generic errors, unhandled exceptions
- **Actions**: Try Again, Go Home, Admin Panel
- **Fallback**: Catches everything else

</td>
</tr>
</table>

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

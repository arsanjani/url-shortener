import React from 'react';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Outlet,
} from "react-router-dom";
import { Container, AppBar, Toolbar, Typography, Box } from "@mui/material";
import LinkIcon from "@mui/icons-material/Link";
import ShortLinksPage from "./pages/ShortLinksPage";
import NotFoundPage from "./pages/NotFoundPage";
import ServerErrorPage from "./pages/ServerErrorPage";
import ForbiddenPage from "./pages/ForbiddenPage";
import ErrorBoundary from "./components/ErrorBoundary";
import ErrorPageDemo from "./components/ErrorPageDemo";

const theme = createTheme({
  palette: {
    primary: {
      main: "#1976d2",
    },
    secondary: {
      main: "#dc004e",
    },
    background: {
      default: "#f5f5f5",
    },
  },
  typography: {
    h4: {
      fontWeight: 600,
    },
    h6: {
      fontWeight: 500,
    },
  },
  components: {
    MuiPaper: {
      styleOverrides: {
        root: {
          borderRadius: 8,
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 8,
          textTransform: "none",
          fontWeight: 500,
        },
      },
    },
  },
});

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <ErrorBoundary>
        <Router>
          <Routes>
            {/* Admin Routes with Layout */}
            <Route path="/" element={<AdminLayout />}>
              <Route index element={<ShortLinksPage />} />
              <Route path="shortlinks" element={<ShortLinksPage />} />
              <Route path="error-demo" element={<ErrorPageDemo />} />
            </Route>

            {/* Error Routes (Full Screen) */}
            <Route path="/error/404" element={<NotFoundPage />} />
            <Route path="/error/500" element={<ServerErrorPage />} />
            <Route path="/error/403" element={<ForbiddenPage />} />

            {/* Catch-all route for 404 */}
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </Router>
      </ErrorBoundary>
    </ThemeProvider>
  );
}

// Admin Layout Component
function AdminLayout() {
  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static" elevation={2}>
        <Toolbar>
          <LinkIcon sx={{ mr: 2 }} />
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            ScissorLink Admin Panel
          </Typography>
        </Toolbar>
      </AppBar>

      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Outlet />
      </Container>
    </Box>
  );
}

export default App;

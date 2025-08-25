import React from 'react';
import {
  Box,
  Typography,
  Button,
  Container,
  Paper,
  useTheme,
  alpha,
} from '@mui/material';
import {
  ErrorOutline,
  Home,
  Refresh,
  Security,
  CloudOff,
  BugReport,
  Settings,
} from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';

export interface ErrorPageProps {
  errorType?: 'not-found' | 'server-error' | 'forbidden' | 'network' | 'unknown';
  title?: string;
  subtitle?: string;
  description?: string;
  showRefresh?: boolean;
  showHome?: boolean;
  showAdmin?: boolean;
  customActions?: React.ReactNode;
}

interface ErrorConfig {
  icon: React.ReactElement;
  title: string;
  subtitle: string;
  description: string;
  color: string;
}

const errorConfigs: Record<string, ErrorConfig> = {
  'not-found': {
    icon: <ErrorOutline sx={{ fontSize: 120 }} />,
    title: '404',
    subtitle: 'Page Not Found',
    description: 'The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.',
    color: '#ff9800',
  },
  'server-error': {
    icon: <BugReport sx={{ fontSize: 120 }} />,
    title: '500',
    subtitle: 'Internal Server Error',
    description: 'Something went wrong on our end. Please try again later or contact support if the problem persists.',
    color: '#f44336',
  },
  'forbidden': {
    icon: <Security sx={{ fontSize: 120 }} />,
    title: '403',
    subtitle: 'Access Forbidden',
    description: 'You do not have permission to access this resource. Please contact your administrator if you believe this is an error.',
    color: '#e91e63',
  },
  'network': {
    icon: <CloudOff sx={{ fontSize: 120 }} />,
    title: 'Network Error',
    subtitle: 'Connection Failed',
    description: 'Unable to connect to the server. Please check your internet connection and try again.',
    color: '#9c27b0',
  },
  'unknown': {
    icon: <ErrorOutline sx={{ fontSize: 120 }} />,
    title: 'Oops!',
    subtitle: 'Something Went Wrong',
    description: 'An unexpected error occurred. Please try refreshing the page or contact support if the issue continues.',
    color: '#607d8b',
  },
};

const ErrorPage: React.FC<ErrorPageProps> = ({
  errorType = 'unknown',
  title,
  subtitle,
  description,
  showRefresh = true,
  showHome = true,
  showAdmin = true,
  customActions,
}) => {
  const theme = useTheme();
  const navigate = useNavigate();
  
  const config = errorConfigs[errorType];
  const errorColor = config.color;

  const handleRefresh = () => {
    window.location.reload();
  };

  const handleGoHome = () => {
    navigate('/');
  };

  const handleGoToAdmin = () => {
    navigate('/admin');
  };

  return (
    <Box
      sx={{
        minHeight: '100vh',
        background: `linear-gradient(135deg, ${alpha(errorColor, 0.1)} 0%, ${alpha(theme.palette.primary.main, 0.1)} 100%)`,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        padding: 2,
        position: 'relative',
        '&::before': {
          content: '""',
          position: 'absolute',
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          backgroundImage: `radial-gradient(circle at 25px 25px, ${alpha(theme.palette.common.white, 0.1)} 2px, transparent 0)`,
          backgroundSize: '50px 50px',
          pointerEvents: 'none',
        },
      }}
    >
      <Container maxWidth="md">
        <Paper
          elevation={24}
          sx={{
            padding: { xs: 3, sm: 4, md: 6 },
            textAlign: 'center',
            borderRadius: 4,
            background: alpha(theme.palette.background.paper, 0.95),
            backdropFilter: 'blur(10px)',
            border: `1px solid ${alpha(theme.palette.common.white, 0.2)}`,
            position: 'relative',
            overflow: 'hidden',
            '&::before': {
              content: '""',
              position: 'absolute',
              top: 0,
              left: 0,
              right: 0,
              height: 4,
              background: `linear-gradient(90deg, ${errorColor}, ${theme.palette.primary.main})`,
            },
          }}
        >
          {/* Error Code Badge */}
          <Box
            sx={{
              position: 'absolute',
              top: 16,
              right: 16,
              backgroundColor: alpha(errorColor, 0.1),
              color: errorColor,
              padding: '8px 16px',
              borderRadius: 3,
              fontSize: '0.875rem',
              fontWeight: 600,
            }}
          >
            Error {title || config.title}
          </Box>

          {/* Error Icon */}
          <Box
            sx={{
              color: errorColor,
              mb: 3,
              opacity: 0.8,
              animation: 'pulse 2s infinite',
              '@keyframes pulse': {
                '0%': { opacity: 0.8 },
                '50%': { opacity: 1 },
                '100%': { opacity: 0.8 },
              },
            }}
          >
            {config.icon}
          </Box>

          {/* Error Title */}
          <Typography
            variant="h2"
            component="h1"
            sx={{
              fontSize: { xs: '2.5rem', sm: '3rem', md: '3.5rem' },
              fontWeight: 700,
              color: theme.palette.text.primary,
              mb: 2,
              lineHeight: 1.2,
            }}
          >
            {title || config.title}
          </Typography>

          {/* Error Subtitle */}
          <Typography
            variant="h4"
            component="h2"
            sx={{
              fontSize: { xs: '1.5rem', sm: '1.75rem', md: '2rem' },
              fontWeight: 600,
              color: errorColor,
              mb: 2,
            }}
          >
            {subtitle || config.subtitle}
          </Typography>

          {/* Error Description */}
          <Typography
            variant="body1"
            sx={{
              fontSize: '1.125rem',
              color: theme.palette.text.secondary,
              mb: 4,
              lineHeight: 1.6,
              maxWidth: 600,
              mx: 'auto',
            }}
          >
            {description || config.description}
          </Typography>

          {/* Action Buttons */}
          <Box
            sx={{
              display: 'flex',
              gap: 2,
              justifyContent: 'center',
              flexWrap: 'wrap',
              mb: customActions ? 3 : 0,
            }}
          >
            {showHome && (
              <Button
                variant="contained"
                size="large"
                startIcon={<Home />}
                onClick={handleGoHome}
                sx={{
                  minWidth: 160,
                  borderRadius: 3,
                  padding: '12px 24px',
                  background: `linear-gradient(135deg, ${theme.palette.primary.main}, ${theme.palette.primary.dark})`,
                  boxShadow: `0 4px 15px ${alpha(theme.palette.primary.main, 0.3)}`,
                  '&:hover': {
                    transform: 'translateY(-2px)',
                    boxShadow: `0 8px 25px ${alpha(theme.palette.primary.main, 0.4)}`,
                  },
                  transition: 'all 0.3s ease',
                }}
              >
                Go Home
              </Button>
            )}

            {showRefresh && (
              <Button
                variant="outlined"
                size="large"
                startIcon={<Refresh />}
                onClick={handleRefresh}
                sx={{
                  minWidth: 160,
                  borderRadius: 3,
                  padding: '12px 24px',
                  borderWidth: 2,
                  '&:hover': {
                    borderWidth: 2,
                    transform: 'translateY(-2px)',
                    boxShadow: `0 8px 25px ${alpha(theme.palette.primary.main, 0.3)}`,
                  },
                  transition: 'all 0.3s ease',
                }}
              >
                Try Again
              </Button>
            )}

            {showAdmin && (
              <Button
                variant="outlined"
                size="large"
                startIcon={<Settings />}
                onClick={handleGoToAdmin}
                sx={{
                  minWidth: 160,
                  borderRadius: 3,
                  padding: '12px 24px',
                  borderWidth: 2,
                  color: errorColor,
                  borderColor: errorColor,
                  '&:hover': {
                    borderWidth: 2,
                    borderColor: errorColor,
                    backgroundColor: alpha(errorColor, 0.1),
                    transform: 'translateY(-2px)',
                    boxShadow: `0 8px 25px ${alpha(errorColor, 0.3)}`,
                  },
                  transition: 'all 0.3s ease',
                }}
              >
                Admin Panel
              </Button>
            )}
          </Box>

          {/* Custom Actions */}
          {customActions && (
            <Box sx={{ mt: 3 }}>
              {customActions}
            </Box>
          )}

          {/* Floating Animation Elements */}
          <Box
            sx={{
              position: 'absolute',
              width: '100%',
              height: '100%',
              pointerEvents: 'none',
              overflow: 'hidden',
              '&::before, &::after': {
                content: '""',
                position: 'absolute',
                width: 100,
                height: 100,
                background: alpha(errorColor, 0.1),
                borderRadius: '50%',
                animation: 'float 6s ease-in-out infinite',
              },
              '&::before': {
                top: '10%',
                left: '10%',
                animationDelay: '0s',
              },
              '&::after': {
                bottom: '10%',
                right: '10%',
                animationDelay: '3s',
              },
              '@keyframes float': {
                '0%, 100%': { transform: 'translateY(0px)' },
                '50%': { transform: 'translateY(-20px)' },
              },
            }}
          />
        </Paper>
      </Container>
    </Box>
  );
};

export default ErrorPage;

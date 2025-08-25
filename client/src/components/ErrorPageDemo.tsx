import React, { useState } from 'react';
import {
  Box,
  Typography,
  Button,
  Card,
  CardContent,
  Grid,
  Dialog,
  DialogContent,
  DialogTitle,
  IconButton,
} from '@mui/material';
import { Close } from '@mui/icons-material';
import ErrorPage from './ErrorPage';

const ErrorPageDemo: React.FC = () => {
  const [selectedError, setSelectedError] = useState<string | null>(null);

  const errorTypes = [
    {
      type: 'not-found',
      title: '404 - Not Found',
      description: 'Page not found error page',
    },
    {
      type: 'server-error',
      title: '500 - Server Error',
      description: 'Internal server error page',
    },
    {
      type: 'forbidden',
      title: '403 - Forbidden',
      description: 'Access forbidden error page',
    },
    {
      type: 'network',
      title: 'Network Error',
      description: 'Connection failed error page',
    },
    {
      type: 'unknown',
      title: 'Unknown Error',
      description: 'Generic error page',
    },
  ];

  const handleCloseDialog = () => {
    setSelectedError(null);
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" gutterBottom>
        Error Pages Demo
      </Typography>
      <Typography variant="body1" color="text.secondary" sx={{ mb: 4 }}>
        Click on any error type below to preview the Material Design error page.
      </Typography>

      <Grid container spacing={3}>
        {errorTypes.map((error) => (
          <Grid item xs={12} sm={6} md={4} key={error.type}>
            <Card sx={{ height: '100%', cursor: 'pointer' }} onClick={() => setSelectedError(error.type)}>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  {error.title}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  {error.description}
                </Typography>
                <Button
                  variant="outlined"
                  size="small"
                  sx={{ mt: 2 }}
                  onClick={(e) => {
                    e.stopPropagation();
                    setSelectedError(error.type);
                  }}
                >
                  Preview
                </Button>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>

      {/* Error Page Dialog */}
      <Dialog
        open={!!selectedError}
        onClose={handleCloseDialog}
        maxWidth="lg"
        fullWidth
        PaperProps={{
          sx: {
            height: '90vh',
            margin: 1,
          },
        }}
      >
        <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          Error Page Preview
          <IconButton onClick={handleCloseDialog}>
            <Close />
          </IconButton>
        </DialogTitle>
        <DialogContent sx={{ p: 0, overflow: 'hidden' }}>
          {selectedError && (
            <ErrorPage
              errorType={selectedError as any}
              showRefresh={true}
              showHome={true}
              showAdmin={true}
            />
          )}
        </DialogContent>
      </Dialog>
    </Box>
  );
};

export default ErrorPageDemo;

import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Button,
  Paper,
  CircularProgress,
  Alert,
  Fab,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { ShortLinkResponseDto } from '../types/api';
import { ApiService } from '../services/apiService';
import ShortLinksList from '../components/ShortLinksList';
import AddShortLinkDialog from '../components/AddShortLinkDialog';

const ShortLinksPage: React.FC = () => {
  const [shortLinks, setShortLinks] = useState<ShortLinkResponseDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [addDialogOpen, setAddDialogOpen] = useState(false);

  const loadShortLinks = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await ApiService.getAllShortLinks();
      setShortLinks(data);
    } catch (err) {
      setError('Failed to load short links. Please try again.');
      console.error('Error loading short links:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadShortLinks();
  }, []);

  const handleAddSuccess = (newShortLink: ShortLinkResponseDto) => {
    setShortLinks(prev => [newShortLink, ...prev]);
    setAddDialogOpen(false);
  };

  const handleUpdateSuccess = (updatedShortLink: ShortLinkResponseDto) => {
    setShortLinks(prev => 
      prev.map(link => 
        link.id === updatedShortLink.id ? updatedShortLink : link
      )
    );
  };

  const handleDeleteSuccess = (deletedId: number) => {
    setShortLinks(prev => prev.filter(link => link.id !== deletedId));
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="400px">
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Box sx={{ mb: 2 }}>
        <Alert severity="error" action={
          <Button color="inherit" size="small" onClick={loadShortLinks}>
            Retry
          </Button>
        }>
          {error}
        </Alert>
      </Box>
    );
  }

  return (
    <Box>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
        <Typography variant="h4" component="h1">
          Short Links Management
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => setAddDialogOpen(true)}
          size="large"
        >
          Add New Link
        </Button>
      </Box>

      {shortLinks.length === 0 ? (
        <Paper sx={{ p: 4, textAlign: 'center' }}>
          <Typography variant="h6" color="text.secondary" gutterBottom>
            No short links found
          </Typography>
          <Typography variant="body2" color="text.secondary" mb={2}>
            Create your first short link to get started
          </Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => setAddDialogOpen(true)}
          >
            Add First Link
          </Button>
        </Paper>
      ) : (
        <ShortLinksList
          shortLinks={shortLinks}
          onUpdate={handleUpdateSuccess}
          onDelete={handleDeleteSuccess}
        />
      )}

      <AddShortLinkDialog
        open={addDialogOpen}
        onClose={() => setAddDialogOpen(false)}
        onSuccess={handleAddSuccess}
      />

      <Fab
        color="primary"
        aria-label="add"
        sx={{
          position: 'fixed',
          bottom: 16,
          right: 16,
        }}
        onClick={() => setAddDialogOpen(true)}
      >
        <AddIcon />
      </Fab>
    </Box>
  );
};

export default ShortLinksPage;

import React, { useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Box,
  FormControlLabel,
  Switch,
  Typography,
  Alert,
  CircularProgress,
  InputAdornment,
  IconButton,
} from '@mui/material';
import { Refresh as RefreshIcon } from '@mui/icons-material';
import { ShortLinkRequestDto, ShortLinkResponseDto } from '../types/api';
import { ApiService } from '../services/apiService';

interface AddShortLinkDialogProps {
  open: boolean;
  onClose: () => void;
  onSuccess: (newShortLink: ShortLinkResponseDto) => void;
}

const AddShortLinkDialog: React.FC<AddShortLinkDialogProps> = ({
  open,
  onClose,
  onSuccess,
}) => {
  const [formData, setFormData] = useState<ShortLinkRequestDto>({
    title: '',
    token: '',
    originLink: '',
    isPublish: true,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [fieldErrors, setFieldErrors] = useState<{ [key: string]: string }>({});

  const generateRandomToken = () => {
    const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let token = '';
    for (let i = 0; i < 6; i++) {
      token += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    setFormData(prev => ({ ...prev, token }));
  };

  const validateForm = (): boolean => {
    const errors: { [key: string]: string } = {};

    if (!formData.originLink.trim()) {
      errors.originLink = 'Original URL is required';
    } else {
      try {
        new URL(formData.originLink);
      } catch {
        errors.originLink = 'Please enter a valid URL';
      }
    }

    if (formData.token && !/^[a-zA-Z0-9_-]+$/.test(formData.token)) {
      errors.token = 'Token can only contain letters, numbers, hyphens, and underscores';
    }

    if (formData.token && formData.token.length > 50) {
      errors.token = 'Token must be 50 characters or less';
    }

    setFieldErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = async () => {
    if (!validateForm()) {
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const newShortLink = await ApiService.createShortLink(formData);
      onSuccess(newShortLink);
      handleClose();
    } catch (err: any) {
      if (err.response?.status === 400) {
        setError('Invalid data provided. Please check your inputs.');
      } else if (err.response?.status === 409) {
        setError('This token is already in use. Please choose a different one.');
      } else {
        setError('Failed to create short link. Please try again.');
      }
      console.error('Error creating short link:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    setFormData({
      title: '',
      token: '',
      originLink: '',
      isPublish: true,
    });
    setError(null);
    setFieldErrors({});
    onClose();
  };

  const handleInputChange = (field: keyof ShortLinkRequestDto) => 
    (event: React.ChangeEvent<HTMLInputElement>) => {
      const value = event.target.type === 'checkbox' ? event.target.checked : event.target.value;
      setFormData(prev => ({ ...prev, [field]: value }));
      
      // Clear field error when user starts typing
      if (fieldErrors[field]) {
        setFieldErrors(prev => ({ ...prev, [field]: '' }));
      }
    };

  const previewUrl = formData.token ? ApiService.buildShortUrl(formData.token) : '';

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>Add New Short Link</DialogTitle>
      <DialogContent>
        <Box component="form" noValidate sx={{ mt: 1 }}>
          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          <TextField
            fullWidth
            label="Title (Optional)"
            placeholder="My awesome link"
            value={formData.title}
            onChange={handleInputChange('title')}
            margin="normal"
            helperText="A friendly name for your link"
          />

          <TextField
            fullWidth
            label="Custom Token (Optional)"
            placeholder="custom-link"
            value={formData.token}
            onChange={handleInputChange('token')}
            margin="normal"
            error={!!fieldErrors.token}
            helperText={fieldErrors.token || 'Leave blank to auto-generate'}
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <IconButton onClick={generateRandomToken} edge="end">
                    <RefreshIcon />
                  </IconButton>
                </InputAdornment>
              ),
            }}
          />

          {previewUrl && (
            <Box sx={{ mt: 1, mb: 2 }}>
              <Typography variant="body2" color="text.secondary">
                Preview URL:
              </Typography>
              <Typography variant="body2" color="primary" sx={{ wordBreak: 'break-all' }}>
                {previewUrl}
              </Typography>
            </Box>
          )}

          <TextField
            fullWidth
            label="Original URL"
            placeholder="https://example.com/very/long/url"
            value={formData.originLink}
            onChange={handleInputChange('originLink')}
            margin="normal"
            required
            error={!!fieldErrors.originLink}
            helperText={fieldErrors.originLink}
          />

          <FormControlLabel
            control={
              <Switch
                checked={formData.isPublish}
                onChange={handleInputChange('isPublish')}
                color="primary"
              />
            }
            label="Publish immediately"
            sx={{ mt: 2 }}
          />
          <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
            {formData.isPublish 
              ? 'The link will be active immediately' 
              : 'The link will be saved as draft'
            }
          </Typography>
        </Box>
      </DialogContent>
      <DialogActions sx={{ px: 3, pb: 2 }}>
        <Button onClick={handleClose} disabled={loading}>
          Cancel
        </Button>
        <Button
          onClick={handleSubmit}
          variant="contained"
          disabled={loading || !formData.originLink.trim()}
          startIcon={loading ? <CircularProgress size={20} /> : null}
        >
          {loading ? 'Creating...' : 'Create Link'}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default AddShortLinkDialog;

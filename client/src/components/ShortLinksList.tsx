import React, { useState } from 'react';
import { useNavigate } from "react-router-dom";
import {
  Box,
  Card,
  CardContent,
  Typography,
  IconButton,
  Chip,
  Button,
  Tooltip,
  Grid,
  Menu,
  MenuItem,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Snackbar,
  Alert,
} from "@mui/material";
import {
  Launch as LaunchIcon,
  MoreVert as MoreVertIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  Visibility as VisibilityIcon,
  VisibilityOff as VisibilityOffIcon,
  ContentCopy as ContentCopyIcon,
  BarChart as BarChartIcon,
} from "@mui/icons-material";
import { ShortLinkResponseDto } from "../types/api";
import { ApiService } from "../services/apiService";

interface ShortLinksListProps {
  shortLinks: ShortLinkResponseDto[];
  onUpdate: (updatedShortLink: ShortLinkResponseDto) => void;
  onDelete: (deletedId: number) => void;
}

const ShortLinksList: React.FC<ShortLinksListProps> = ({
  shortLinks,
  onUpdate,
  onDelete,
}) => {
  const navigate = useNavigate();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [selectedLink, setSelectedLink] = useState<ShortLinkResponseDto | null>(
    null
  );
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: "",
    severity: "success" as "success" | "error",
  });

  const handleMenuOpen = (
    event: React.MouseEvent<HTMLElement>,
    link: ShortLinkResponseDto
  ) => {
    setAnchorEl(event.currentTarget);
    setSelectedLink(link);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
    setSelectedLink(null);
  };

  const handleCopyToClipboard = async (shortUrl: string) => {
    try {
      await navigator.clipboard.writeText(shortUrl);
      setSnackbar({
        open: true,
        message: "Short URL copied to clipboard!",
        severity: "success",
      });
    } catch (err) {
      setSnackbar({
        open: true,
        message: "Failed to copy URL",
        severity: "error",
      });
    }
    handleMenuClose();
  };

  const handleTogglePublish = async () => {
    if (!selectedLink) return;

    try {
      const updatedLink = await ApiService.togglePublishStatus(selectedLink.id);
      onUpdate(updatedLink);
      setSnackbar({
        open: true,
        message: `Link ${
          updatedLink.isPublish ? "published" : "unpublished"
        } successfully!`,
        severity: "success",
      });
    } catch (err) {
      setSnackbar({
        open: true,
        message: "Failed to update publish status",
        severity: "error",
      });
    }
    handleMenuClose();
  };

  const handleDeleteClick = () => {
    setDeleteDialogOpen(true);
    handleMenuClose();
  };

  const handleDeleteConfirm = async () => {
    if (!selectedLink) return;

    try {
      await ApiService.deleteShortLink(selectedLink.id);
      onDelete(selectedLink.id);
      setSnackbar({
        open: true,
        message: "Link deleted successfully!",
        severity: "success",
      });
    } catch (err) {
      setSnackbar({
        open: true,
        message: "Failed to delete link",
        severity: "error",
      });
    }
    setDeleteDialogOpen(false);
    setSelectedLink(null);
  };

  const handleTestLink = (token: string) => {
    const shortUrl = ApiService.buildShortUrl(token);
    window.open(shortUrl, "_blank");
  };

  const handleViewDashboard = (linkId: number) => {
    navigate(`/dashboard/${linkId}`);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  return (
    <Box>
      <Grid container spacing={3}>
        {shortLinks.map((link) => {
          const shortUrl = ApiService.buildShortUrl(link.token);

          return (
            <Grid item xs={12} md={6} lg={4} key={link.id}>
              <Card
                sx={{
                  height: "100%",
                  display: "flex",
                  flexDirection: "column",
                  transition: "transform 0.2s, box-shadow 0.2s",
                  "&:hover": {
                    transform: "translateY(-2px)",
                    boxShadow: 4,
                  },
                }}
              >
                <CardContent sx={{ flexGrow: 1 }}>
                  <Box
                    display="flex"
                    justifyContent="space-between"
                    alignItems="flex-start"
                    mb={2}
                  >
                    <Box flex={1}>
                      <Typography variant="h6" component="h2" noWrap>
                        {link.title || "Untitled"}
                      </Typography>
                      <Typography variant="body2" color="text.secondary" noWrap>
                        {link.originLink}
                      </Typography>
                    </Box>
                    <IconButton
                      size="small"
                      onClick={(e) => handleMenuOpen(e, link)}
                    >
                      <MoreVertIcon />
                    </IconButton>
                  </Box>

                  <Box mb={2}>
                    <Typography
                      variant="body2"
                      color="primary"
                      sx={{ wordBreak: "break-all" }}
                    >
                      {shortUrl}
                    </Typography>
                  </Box>

                  <Box display="flex" gap={1} mb={2} flexWrap="wrap">
                    <Chip
                      size="small"
                      label={link.isPublish ? "Published" : "Draft"}
                      color={link.isPublish ? "success" : "default"}
                      icon={
                        link.isPublish ? (
                          <VisibilityIcon />
                        ) : (
                          <VisibilityOffIcon />
                        )
                      }
                    />
                    <Chip
                      size="small"
                      label={`${link.clickCount} clicks`}
                      icon={<BarChartIcon />}
                      variant="outlined"
                      clickable
                      onClick={() => handleViewDashboard(link.id)}
                      sx={{
                        cursor: "pointer",
                        "&:hover": {
                          backgroundColor: "action.hover",
                        },
                      }}
                    />
                  </Box>

                  <Typography variant="caption" color="text.secondary">
                    Created: {formatDate(link.createAdminDate)}
                  </Typography>
                  {link.editAdminDate && (
                    <Typography
                      variant="caption"
                      color="text.secondary"
                      display="block"
                    >
                      Updated: {formatDate(link.editAdminDate)}
                    </Typography>
                  )}
                </CardContent>

                <Box p={2} pt={0}>
                  <Button
                    fullWidth
                    variant="contained"
                    startIcon={<LaunchIcon />}
                    onClick={() => handleTestLink(link.token)}
                    disabled={!link.isPublish}
                  >
                    Test Link
                  </Button>
                </Box>
              </Card>
            </Grid>
          );
        })}
      </Grid>

      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleMenuClose}
      >
        <MenuItem
          onClick={() =>
            handleCopyToClipboard(
              ApiService.buildShortUrl(selectedLink?.token || "")
            )
          }
        >
          <ContentCopyIcon sx={{ mr: 1 }} />
          Copy Short URL
        </MenuItem>
        <MenuItem onClick={handleTogglePublish}>
          {selectedLink?.isPublish ? (
            <VisibilityOffIcon sx={{ mr: 1 }} />
          ) : (
            <VisibilityIcon sx={{ mr: 1 }} />
          )}
          {selectedLink?.isPublish ? "Unpublish" : "Publish"}
        </MenuItem>
        <MenuItem onClick={handleDeleteClick} sx={{ color: "error.main" }}>
          <DeleteIcon sx={{ mr: 1 }} />
          Delete
        </MenuItem>
      </Menu>

      <Dialog
        open={deleteDialogOpen}
        onClose={() => setDeleteDialogOpen(false)}
      >
        <DialogTitle>Delete Short Link</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete "
            {selectedLink?.title || "this link"}"? This action cannot be undone.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteDialogOpen(false)}>Cancel</Button>
          <Button onClick={handleDeleteConfirm} color="error" autoFocus>
            Delete
          </Button>
        </DialogActions>
      </Dialog>

      <Snackbar
        open={snackbar.open}
        autoHideDuration={4000}
        onClose={() => setSnackbar((prev) => ({ ...prev, open: false }))}
      >
        <Alert
          severity={snackbar.severity}
          onClose={() => setSnackbar((prev) => ({ ...prev, open: false }))}
        >
          {snackbar.message}
        </Alert>
      </Snackbar>
    </Box>
  );
};

export default ShortLinksList;

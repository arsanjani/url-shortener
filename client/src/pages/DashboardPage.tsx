import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Box,
  Typography,
  Paper,
  Grid,
  Card,
  CardContent,
  CircularProgress,
  Alert,
  Button,
  Chip,
  Divider,
  IconButton,
  Tooltip,
} from '@mui/material';
import {
  ArrowBack as ArrowBackIcon,
  Launch as LaunchIcon,
  ContentCopy as ContentCopyIcon,
  Visibility as VisibilityIcon,
  VisibilityOff as VisibilityOffIcon,
  TrendingUp as TrendingUpIcon,
  Public as PublicIcon,
  Schedule as ScheduleIcon,
  DeviceHub as DeviceHubIcon,
} from '@mui/icons-material';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  LineElement,
  PointElement,
  Title,
  Tooltip as ChartTooltip,
  Legend,
  ArcElement,
} from 'chart.js';
import { Bar, Line, Pie } from 'react-chartjs-2';
import { ApiService } from '../services/apiService';
import { StatisticsResponseDto } from '../types/api';

// Register Chart.js components
ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  LineElement,
  PointElement,
  Title,
  ChartTooltip,
  Legend,
  ArcElement
);

interface DashboardPageProps {}

const DashboardPage: React.FC<DashboardPageProps> = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [statistics, setStatistics] = useState<StatisticsResponseDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (id) {
      loadStatistics(parseInt(id));
    }
  }, [id]);

  const loadStatistics = async (linkId: number) => {
    try {
      setLoading(true);
      setError(null);
      const data = await ApiService.getShortLinkStatistics(linkId);
      setStatistics(data);
    } catch (err) {
      setError('Failed to load statistics. Please try again.');
      console.error('Error loading statistics:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleCopyToClipboard = async () => {
    if (!statistics) return;
    try {
      const shortUrl = ApiService.buildShortUrl(statistics.shortLink.token);
      await navigator.clipboard.writeText(shortUrl);
      // Could add a snackbar here for feedback
    } catch (err) {
      console.error('Failed to copy URL:', err);
    }
  };

  const handleTestLink = () => {
    if (!statistics) return;
    const shortUrl = ApiService.buildShortUrl(statistics.shortLink.token);
    window.open(shortUrl, '_blank');
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
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
        <Alert 
          severity="error" 
          action={
            <Button color="inherit" size="small" onClick={() => id && loadStatistics(parseInt(id))}>
              Retry
            </Button>
          }
        >
          {error}
        </Alert>
      </Box>
    );
  }

  if (!statistics) {
    return (
      <Box sx={{ mb: 2 }}>
        <Alert severity="info">
          No statistics data found.
        </Alert>
      </Box>
    );
  }

  const shortUrl = ApiService.buildShortUrl(statistics.shortLink.token);

  // Chart data preparation
  const dailyChartData = {
    labels: statistics.dailyStats.map(stat => new Date(stat.date).toLocaleDateString()),
    datasets: [
      {
        label: 'Daily Clicks',
        data: statistics.dailyStats.map(stat => stat.clicks),
        backgroundColor: 'rgba(25, 118, 210, 0.2)',
        borderColor: 'rgba(25, 118, 210, 1)',
        borderWidth: 2,
        fill: true,
      },
    ],
  };

  const hourlyChartData = {
    labels: Array.from({ length: 24 }, (_, i) => `${i}:00`),
    datasets: [
      {
        label: 'Clicks by Hour',
        data: Array.from({ length: 24 }, (_, hour) => {
          const stat = statistics.hourlyStats.find(s => s.hour === hour);
          return stat ? stat.clicks : 0;
        }),
        backgroundColor: 'rgba(220, 0, 78, 0.2)',
        borderColor: 'rgba(220, 0, 78, 1)',
        borderWidth: 2,
      },
    ],
  };

  const countryChartData = {
    labels: statistics.countryStats.map(stat => stat.country || 'Unknown'),
    datasets: [
      {
        data: statistics.countryStats.map(stat => stat.clicks),
        backgroundColor: [
          '#FF6384',
          '#36A2EB',
          '#FFCE56',
          '#4BC0C0',
          '#9966FF',
          '#FF9F40',
          '#FF6384',
          '#C9CBCF',
          '#4BC0C0',
          '#FF6384',
        ],
      },
    ],
  };

  const osChartData = {
    labels: statistics.osStats.map(stat => stat.os || 'Unknown'),
    datasets: [
      {
        data: statistics.osStats.map(stat => stat.clicks),
        backgroundColor: [
          '#36A2EB',
          '#FF6384',
          '#FFCE56',
          '#4BC0C0',
          '#9966FF',
          '#FF9F40',
        ],
      },
    ],
  };

  const browserChartData = {
    labels: statistics.browserStats.map(stat => stat.browser || 'Unknown'),
    datasets: [
      {
        data: statistics.browserStats.map(stat => stat.clicks),
        backgroundColor: [
          '#FFCE56',
          '#FF6384',
          '#36A2EB',
          '#4BC0C0',
          '#9966FF',
          '#FF9F40',
        ],
      },
    ],
  };

  const chartOptions = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top' as const,
      },
      title: {
        display: false,
      },
    },
  };

  const pieChartOptions = {
    responsive: true,
    plugins: {
      legend: {
        position: 'bottom' as const,
      },
    },
  };

  return (
    <Box>
      {/* Header */}
      <Box display="flex" alignItems="center" mb={3}>
        <IconButton onClick={() => navigate(-1)} sx={{ mr: 2 }}>
          <ArrowBackIcon />
        </IconButton>
        <Typography variant="h4" component="h1" sx={{ flexGrow: 1 }}>
          Analytics Dashboard
        </Typography>
      </Box>

      {/* Link Information Card */}
      <Paper sx={{ p: 3, mb: 3 }}>
        <Grid container spacing={3} alignItems="center">
          <Grid item xs={12} md={8}>
            <Typography variant="h6" gutterBottom>
              {statistics.shortLink.title || 'Untitled Link'}
            </Typography>
            <Typography variant="body2" color="text.secondary" gutterBottom>
              Target: {statistics.shortLink.originLink}
            </Typography>
            <Typography variant="body1" color="primary" sx={{ wordBreak: 'break-all' }}>
              {shortUrl}
            </Typography>
            <Box mt={2} display="flex" gap={1} flexWrap="wrap">
              <Chip
                size="small"
                label={statistics.shortLink.isPublish ? 'Published' : 'Draft'}
                color={statistics.shortLink.isPublish ? 'success' : 'default'}
                icon={statistics.shortLink.isPublish ? <VisibilityIcon /> : <VisibilityOffIcon />}
              />
              <Chip
                size="small"
                label={`${statistics.totalClicks} total clicks`}
                icon={<TrendingUpIcon />}
                variant="outlined"
              />
              <Chip
                size="small"
                label={`${statistics.uniqueCountries} countries`}
                icon={<PublicIcon />}
                variant="outlined"
              />
            </Box>
          </Grid>
          <Grid item xs={12} md={4}>
            <Box display="flex" gap={1} justifyContent="flex-end">
              <Tooltip title="Copy Short URL">
                <IconButton onClick={handleCopyToClipboard}>
                  <ContentCopyIcon />
                </IconButton>
              </Tooltip>
              <Button
                variant="contained"
                startIcon={<LaunchIcon />}
                onClick={handleTestLink}
                disabled={!statistics.shortLink.isPublish}
              >
                Test Link
              </Button>
            </Box>
          </Grid>
        </Grid>
      </Paper>

      {/* Statistics Cards */}
      <Grid container spacing={3} sx={{ mb: 3 }}>
        <Grid item xs={12} sm={6} md={3}>
          <Card sx={{ height: '100%' }}>
            <CardContent sx={{ height: '100%', display: 'flex', alignItems: 'center' }}>
              <Box display="flex" alignItems="center" width="100%">
                <TrendingUpIcon color="primary" sx={{ mr: 1, flexShrink: 0 }} />
                <Box flex={1}>
                  <Typography variant="h4" sx={{ lineHeight: 1.2 }}>
                    {statistics.totalClicks}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Total Clicks
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <Card sx={{ height: '100%' }}>
            <CardContent sx={{ height: '100%', display: 'flex', alignItems: 'center' }}>
              <Box display="flex" alignItems="center" width="100%">
                <PublicIcon color="primary" sx={{ mr: 1, flexShrink: 0 }} />
                <Box flex={1}>
                  <Typography variant="h4" sx={{ lineHeight: 1.2 }}>
                    {statistics.uniqueCountries}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Countries
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <Card sx={{ height: '100%' }}>
            <CardContent sx={{ height: '100%', display: 'flex', alignItems: 'center' }}>
              <Box display="flex" alignItems="center" width="100%">
                <ScheduleIcon color="primary" sx={{ mr: 1, flexShrink: 0 }} />
                <Box flex={1}>
                  <Typography 
                    variant="body1" 
                    sx={{ 
                      fontSize: '1.1rem', 
                      fontWeight: 600,
                      lineHeight: 1.2,
                      minHeight: '2.4em',
                      display: 'flex',
                      alignItems: 'center'
                    }}
                  >
                    {statistics.firstClick ? formatDate(statistics.firstClick) : 'N/A'}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    First Click
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <Card sx={{ height: '100%' }}>
            <CardContent sx={{ height: '100%', display: 'flex', alignItems: 'center' }}>
              <Box display="flex" alignItems="center" width="100%">
                <DeviceHubIcon color="primary" sx={{ mr: 1, flexShrink: 0 }} />
                <Box flex={1}>
                  <Typography 
                    variant="body1" 
                    sx={{ 
                      fontSize: '1.1rem', 
                      fontWeight: 600,
                      lineHeight: 1.2,
                      minHeight: '2.4em',
                      display: 'flex',
                      alignItems: 'center'
                    }}
                  >
                    {statistics.lastClick ? formatDate(statistics.lastClick) : 'N/A'}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Last Click
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>
      </Grid>

      {/* Charts */}
      <Grid container spacing={3}>
        {/* Daily Clicks Chart */}
        <Grid item xs={12} lg={8}>
          <Paper sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              Daily Clicks
            </Typography>
            <Divider sx={{ mb: 2 }} />
            <Box height={300}>
              <Line data={dailyChartData} options={chartOptions} />
            </Box>
          </Paper>
        </Grid>

        {/* Hourly Distribution */}
        <Grid item xs={12} lg={4}>
          <Paper sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              Hourly Distribution
            </Typography>
            <Divider sx={{ mb: 2 }} />
            <Box height={300}>
              <Bar data={hourlyChartData} options={chartOptions} />
            </Box>
          </Paper>
        </Grid>

        {/* Country Distribution */}
        {statistics.countryStats.length > 0 && (
          <Grid item xs={12} md={4}>
            <Paper sx={{ p: 3 }}>
              <Typography variant="h6" gutterBottom>
                Top Countries
              </Typography>
              <Divider sx={{ mb: 2 }} />
              <Box height={300}>
                <Pie data={countryChartData} options={pieChartOptions} />
              </Box>
            </Paper>
          </Grid>
        )}

        {/* OS Distribution */}
        {statistics.osStats.length > 0 && (
          <Grid item xs={12} md={4}>
            <Paper sx={{ p: 3 }}>
              <Typography variant="h6" gutterBottom>
                Operating Systems
              </Typography>
              <Divider sx={{ mb: 2 }} />
              <Box height={300}>
                <Pie data={osChartData} options={pieChartOptions} />
              </Box>
            </Paper>
          </Grid>
        )}

        {/* Browser Distribution */}
        {statistics.browserStats.length > 0 && (
          <Grid item xs={12} md={4}>
            <Paper sx={{ p: 3 }}>
              <Typography variant="h6" gutterBottom>
                Browsers
              </Typography>
              <Divider sx={{ mb: 2 }} />
              <Box height={300}>
                <Pie data={browserChartData} options={pieChartOptions} />
              </Box>
            </Paper>
          </Grid>
        )}
      </Grid>
    </Box>
  );
};

export default DashboardPage;

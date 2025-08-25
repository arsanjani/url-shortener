// Utility functions for error handling and navigation

/**
 * Redirects to the appropriate error page based on HTTP status code
 * @param statusCode - The HTTP status code
 */
export const redirectToErrorPage = (statusCode: number): void => {
  const errorPaths: Record<number, string> = {
    403: '/error/403',
    404: '/error/404',
    500: '/error/500',
  };

  const path = errorPaths[statusCode] || '/error/500';
  window.location.href = path;
};

/**
 * Determines error type based on HTTP status code
 * @param statusCode - The HTTP status code
 * @returns The error type string
 */
export const getErrorType = (statusCode: number): string => {
  const errorTypes: Record<number, string> = {
    400: 'bad-request',
    401: 'unauthorized',
    403: 'forbidden',
    404: 'not-found',
    500: 'server-error',
    502: 'bad-gateway',
    503: 'service-unavailable',
  };

  return errorTypes[statusCode] || 'unknown';
};

/**
 * Checks if a status code represents an error
 * @param statusCode - The HTTP status code
 * @returns True if the status code represents an error
 */
export const isErrorStatusCode = (statusCode: number): boolean => {
  return statusCode >= 400;
};

/**
 * Gets a user-friendly error message based on status code
 * @param statusCode - The HTTP status code
 * @returns A user-friendly error message
 */
export const getErrorMessage = (statusCode: number): string => {
  const errorMessages: Record<number, string> = {
    400: 'Bad request. Please check your input and try again.',
    401: 'You are not authorized to access this resource.',
    403: 'You do not have permission to access this resource.',
    404: 'The requested resource was not found.',
    500: 'Internal server error. Please try again later.',
    502: 'Bad gateway. The server is temporarily unavailable.',
    503: 'Service unavailable. Please try again later.',
  };

  return errorMessages[statusCode] || 'An unexpected error occurred.';
};

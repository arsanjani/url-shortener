import React, { Component, ErrorInfo, ReactNode } from 'react';
import ErrorPage from './ErrorPage';

interface Props {
  children?: ReactNode;
}

interface State {
  hasError: boolean;
  error?: Error;
}

class ErrorBoundary extends Component<Props, State> {
  public state: State = {
    hasError: false,
  };

  public static getDerivedStateFromError(error: Error): State {
    return { hasError: true, error };
  }

  public componentDidCatch(error: Error, errorInfo: ErrorInfo) {
    console.error('Uncaught error:', error, errorInfo);
  }

  public render() {
    if (this.state.hasError) {
      return (
        <ErrorPage
          errorType="unknown"
          title="Application Error"
          subtitle="Something went wrong"
          description="An unexpected error occurred in the application. Please try refreshing the page or contact support if the problem persists."
          showRefresh={true}
          showHome={true}
          showAdmin={false}
        />
      );
    }

    return this.props.children;
  }
}

export default ErrorBoundary;

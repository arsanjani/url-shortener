import React from 'react';
import ErrorPage from '../components/ErrorPage';

const NotFoundPage: React.FC = () => {
  return (
    <ErrorPage
      errorType="not-found"
      showRefresh={false}
      showHome={true}
      showAdmin={true}
    />
  );
};

export default NotFoundPage;

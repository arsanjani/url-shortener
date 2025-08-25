import React from 'react';
import ErrorPage from '../components/ErrorPage';

const ForbiddenPage: React.FC = () => {
  return (
    <ErrorPage
      errorType="forbidden"
      showRefresh={false}
      showHome={true}
      showAdmin={true}
    />
  );
};

export default ForbiddenPage;

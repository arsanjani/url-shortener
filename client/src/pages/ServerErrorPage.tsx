import React from 'react';
import ErrorPage from '../components/ErrorPage';

const ServerErrorPage: React.FC = () => {
  return (
    <ErrorPage
      errorType="server-error"
      showRefresh={true}
      showHome={true}
      showAdmin={false}
    />
  );
};

export default ServerErrorPage;

import React from 'react';
import OfflineIndicator from '../OfflineIndicator';

/**
 * PWA Wrapper Component
 * 
 * Wraps the application with PWA-related components
 * - Offline indicator
 * - No longer shows auto PWA installation prompt (now available in settings)
 */
const PwaWrapper: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  return (
    <>
      {/* Offline indicator appears at the top when offline */}
      <OfflineIndicator />
      
      {/* Main application content */}
      {children}
    </>
  );
};

export default PwaWrapper; 
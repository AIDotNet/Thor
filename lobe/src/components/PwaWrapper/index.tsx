import React from 'react';
import PwaInstall from '../PwaInstall';
import OfflineIndicator from '../OfflineIndicator';

/**
 * PWA Wrapper Component
 * 
 * Wraps the application with PWA-related components
 * - Offline indicator
 * - PWA installation prompt
 */
const PwaWrapper: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  return (
    <>
      {/* Offline indicator appears at the top when offline */}
      <OfflineIndicator />
      
      {/* Main application content */}
      {children}
      
      {/* PWA install prompt (only shown when eligible) */}
      <PwaInstall />
    </>
  );
};

export default PwaWrapper; 
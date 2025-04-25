import React, { useState, useEffect } from 'react';
import { Alert, Typography } from 'antd';
import {  DisconnectOutlined } from '@ant-design/icons';
import { addConnectivityListeners, isOnline, processQueuedRequests } from '../../utils/pwa';

const { Text } = Typography;

/**
 * Offline Indicator Component
 * 
 * Displays a notification when the user's device goes offline
 * and attempts to sync data when back online
 */
const OfflineIndicator: React.FC = () => {
  const [online, setOnline] = useState<boolean>(isOnline());

  useEffect(() => {
    // Initialize with current status
    setOnline(isOnline());
    
    // Set up listeners for online/offline events
    const cleanup = addConnectivityListeners(
      // Online callback
      async () => {
        setOnline(true);
        // Attempt to process any queued requests when back online
        await processQueuedRequests();
      },
      // Offline callback
      () => {
        setOnline(false);
      }
    );
    
    // Clean up event listeners on unmount
    return cleanup;
  }, []);
  
  // Don't render anything if online
  if (online) {
    return null;
  }
  
  return (
    <Alert
      type="warning"
      banner
      icon={<DisconnectOutlined />}
      message={
        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
          <Text>您当前处于离线状态，部分功能可能不可用</Text>
          <Text type="secondary" style={{ fontSize: '12px' }}>
            您的更改将在网络恢复后自动同步
          </Text>
        </div>
      }
    />
  );
};

export default OfflineIndicator; 
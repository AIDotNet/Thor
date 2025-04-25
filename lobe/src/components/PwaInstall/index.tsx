import React, { useState, useEffect } from 'react';
import { Button, Modal, Typography, Space, Row, Col } from 'antd';
import { DownloadOutlined, CloseOutlined } from '@ant-design/icons';
import { isPwaInstalled, showInstallPrompt } from '../../utils/pwa';
import useThemeStore from '../../store/theme';

const { Title, Paragraph, Text } = Typography;

interface PwaInstallProps {
  /** The logo URL to display in the install prompt */
  logoUrl?: string;
}

/**
 * PWA Installation Prompt Component
 * 
 * This component provides a UI to prompt users to install the PWA
 */
const PwaInstall: React.FC<PwaInstallProps> = ({ logoUrl = '/logo.png' }) => {
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const [deferredPrompt, setDeferredPrompt] = useState<BeforeInstallPromptEvent | null>(null);
  const [isInstalled, setIsInstalled] = useState<boolean>(false);
  const { themeMode } = useThemeStore();
  
  // 根据当前主题模式设置不同的样式
  const isDarkMode = themeMode === 'dark' || 
    (themeMode === 'auto' && window.matchMedia('(prefers-color-scheme: dark)').matches);

  // Check if already installed
  useEffect(() => {
    setIsInstalled(isPwaInstalled());

    // Listen for installation status changes
    const handleDisplayModeChange = () => {
      setIsInstalled(isPwaInstalled());
    };

    window.matchMedia('(display-mode: standalone)').addEventListener('change', handleDisplayModeChange);

    return () => {
      window.matchMedia('(display-mode: standalone)').removeEventListener('change', handleDisplayModeChange);
    };
  }, []);

  // Listen for the beforeinstallprompt event
  useEffect(() => {
    const handleBeforeInstallPrompt = (e: BeforeInstallPromptEvent) => {
      // Prevent Chrome 76+ from automatically showing the prompt
      e.preventDefault();
      // Stash the event so it can be triggered later
      setDeferredPrompt(e);
      // Show the install button
      setIsOpen(true);
    };

    window.addEventListener('beforeinstallprompt', handleBeforeInstallPrompt as EventListener);

    return () => {
      window.removeEventListener('beforeinstallprompt', handleBeforeInstallPrompt as EventListener);
    };
  }, []);

  // Handle the appinstalled event
  useEffect(() => {
    const handleAppInstalled = () => {
      // Hide the prompt
      setIsOpen(false);
      // Clear the saved prompt
      setDeferredPrompt(null);
      // Update installed state
      setIsInstalled(true);
    };

    window.addEventListener('appinstalled', handleAppInstalled);

    return () => {
      window.removeEventListener('appinstalled', handleAppInstalled);
    };
  }, []);

  const handleInstallClick = async () => {
    if (!deferredPrompt) {
      return;
    }

    // Show the installation prompt
    const installed = await showInstallPrompt(deferredPrompt);
    
    if (installed) {
      setDeferredPrompt(null);
      setIsOpen(false);
    }
  };

  const handleClose = () => {
    setIsOpen(false);
  };

  // Don't show anything if already installed or no prompt available
  if (isInstalled || !deferredPrompt) {
    return null;
  }

  // 根据主题设置样式
  const modalStyle = {
    // 暗色模式下设置暗色背景
    ...(isDarkMode ? {
      bodyStyle: { background: '#1f1f1f', color: 'rgba(255, 255, 255, 0.85)' },
    } : {})
  };

  return (
    <Modal
      open={isOpen}
      footer={null}
      closable
      closeIcon={<CloseOutlined />}
      onCancel={handleClose}
      width={400}
      centered
      {...modalStyle}
    >
      <Space direction="vertical" size="large" style={{ width: '100%', textAlign: 'center' }}>
        <img 
          src={logoUrl} 
          alt="Thor AI Platform" 
          style={{ 
            width: 80, 
            height: 80, 
            margin: '0 auto',
            borderRadius: '8px',
            boxShadow: isDarkMode 
              ? '0 2px 8px rgba(0, 0, 0, 0.6)' 
              : '0 2px 8px rgba(0, 0, 0, 0.15)'
          }} 
        />
        
        <Title level={4}>安装 Thor AI 平台</Title>
        
        <Paragraph>
          将 Thor AI 平台添加到您的主屏幕，以便离线使用和更快的访问。
        </Paragraph>
        
        <Row gutter={16}>
          <Col span={12}>
            <Button 
              block
              type="default" 
              onClick={handleClose}
            >
              稍后再说
            </Button>
          </Col>
          <Col span={12}>
            <Button 
              block
              type="primary" 
              icon={<DownloadOutlined />} 
              onClick={handleInstallClick}
            >
              立即安装
            </Button>
          </Col>
        </Row>
        
        <Text type="secondary">您随时可以从浏览器菜单中安装应用</Text>
      </Space>
    </Modal>
  );
};

export default PwaInstall; 
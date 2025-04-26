import React, { useState, useEffect } from 'react';
import { Button, Modal, Typography, Space, Row, Col } from 'antd';
import { DownloadOutlined, CloseOutlined } from '@ant-design/icons';
import { isPwaInstalled, showInstallPrompt } from '../../utils/pwa';
import useThemeStore from '../../store/theme';
import { useTranslation } from 'react-i18next';

const { Title, Paragraph, Text } = Typography;

interface PwaInstallProps {
  /** The logo URL to display in the install prompt */
  logoUrl?: string;
  /** When true, renders as a button instead of auto-displaying modal */
  buttonMode?: boolean;
  /** Button text (only used when buttonMode is true) */
  buttonText?: string;
  /** Button type (only used when buttonMode is true) */
  buttonType?: 'primary' | 'default' | 'dashed' | 'link' | 'text';
  /** Button size (only used when buttonMode is true) */
  buttonSize?: 'large' | 'middle' | 'small';
  /** Additional button style (only used when buttonMode is true) */
  buttonStyle?: React.CSSProperties;
}

/**
 * PWA Installation Prompt Component
 * 
 * This component provides a UI to prompt users to install the PWA.
 * Can be used in two modes:
 * 1. Auto-popup mode (default): Automatically shows installation modal when possible
 * 2. Button mode: Renders as a button that user can click to trigger installation
 */
const PwaInstall: React.FC<PwaInstallProps> = ({ 
  logoUrl = '/logo.png',
  buttonMode = false,
  buttonText = '安装应用',
  buttonType = 'primary',
  buttonSize = 'middle',
  buttonStyle = {}
}) => {
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const [deferredPrompt, setDeferredPrompt] = useState<BeforeInstallPromptEvent | null>(null);
  const [isInstalled, setIsInstalled] = useState<boolean>(false);
  const { themeMode } = useThemeStore();
  const { t } = useTranslation();
  
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
      // Only auto-show the install modal if not in button mode
      if (!buttonMode) {
        setIsOpen(true);
      }
    };

    window.addEventListener('beforeinstallprompt', handleBeforeInstallPrompt as EventListener);

    return () => {
      window.removeEventListener('beforeinstallprompt', handleBeforeInstallPrompt as EventListener);
    };
  }, [buttonMode]);

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
  
  const showInstallModal = () => {
    setIsOpen(true);
  };

  // Don't show anything if already installed
  if (isInstalled) {
    return null;
  }

  // If no prompt is available and we're in button mode,
  // render a disabled button
  if (!deferredPrompt && buttonMode) {
    return (
      <Button
        type={buttonType}
        size={buttonSize}
        icon={<DownloadOutlined />}
        disabled
        style={buttonStyle}
      >
        {buttonText}
      </Button>
    );
  }

  // In button mode, render only the button when prompt is available
  if (buttonMode) {
    return (
      <Button
        type={buttonType}
        size={buttonSize}
        icon={<DownloadOutlined />}
        onClick={showInstallModal}
        style={buttonStyle}
      >
        {buttonText}
      </Button>
    );
  }

  // In auto-popup mode, don't show anything if no prompt or not open
  if (!deferredPrompt || !isOpen) {
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
        
        <Title level={4}>{t('install.title')}</Title>
        
        <Paragraph>
          {t('install.description')}
        </Paragraph>
        
        <Row gutter={16}>
          <Col span={12}>
            <Button 
              block
              type="default" 
              onClick={handleClose}
            >
              {t('install.later')}
            </Button>
          </Col>
          <Col span={12}>
            <Button 
              block
              type="primary" 
              icon={<DownloadOutlined />} 
              onClick={handleInstallClick}
            >
              {t('install.install')}
            </Button>
          </Col>
        </Row>
        
        <Text type="secondary">{t('install.browser')}</Text>
      </Space>
    </Modal>
  );
};

export default PwaInstall; 
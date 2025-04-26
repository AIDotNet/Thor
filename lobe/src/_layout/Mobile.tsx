import { memo, useState } from 'react';
import { LayoutProps } from './type';
import { Outlet, useNavigate } from 'react-router-dom';
import { Layout, ConfigProvider, theme, Button, Drawer, Typography, Avatar, Space, Tooltip } from 'antd';
import styled from 'styled-components';
import { Flexbox } from 'react-layout-kit';
import { MenuOutlined, HomeOutlined } from '@ant-design/icons';
import { ThemeSwitch } from '@lobehub/ui';
import useThemeStore from '../store/theme';
import { motion } from 'framer-motion';
import LanguageSwitcher from '../components/LanguageSwitcher';
import { useTranslation } from 'react-i18next';
import PwaInstall from '../components/PwaInstall';

const { Header, Content, Footer } = Layout;
const { Text, Title } = Typography;

// 样式组件
const StyledHeader = styled(Header)(({ theme }: any) => `
  position: fixed;
  top: 0;
  width: 100%;
  z-index: 100;
  padding: 0 ${theme?.token?.paddingSM || 8}px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: ${theme?.token?.colorBgContainer || '#fff'};
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
  height: 56px;
`);

const StyledContent = styled(Content)(({ theme }: any) => `
  margin-top: 56px;
  padding: ${theme?.token?.paddingSM || 8}px;
  flex: 1;
  overflow: auto;
  display: flex;
  flex-direction: column;
`);

const ContentWrapper = styled(motion.div)(({ theme }: any) => `
  padding: ${theme?.token?.paddingSM || 8}px;
  background: ${theme?.token?.colorBgContainer || '#fff'};
  border-radius: ${theme?.token?.borderRadiusSM || 4}px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
  flex: 1;
  min-height: 0;
  overflow: auto;
`);

const StyledFooter = styled(Footer)(({ theme }: any) => `
  text-align: center;
  padding: ${theme?.token?.paddingXS || 4}px;
  background: transparent;
  height: 50px;
`);

const LogoContainer = styled(Flexbox)`
  align-items: center;
  gap: 8px;
`;

// 页面过渡动画
const pageVariants = {
  initial: { opacity: 0 },
  animate: { opacity: 1 },
  exit: { opacity: 0 }
};

const MobileLayout = memo(({ nav }: LayoutProps) => {
  const [drawerVisible, setDrawerVisible] = useState(false);
  const { themeMode, toggleTheme } = useThemeStore();
  const { token } = theme.useToken();
  const navigate = useNavigate();
  const { t } = useTranslation();

  const toggleDrawer = () => {
    setDrawerVisible(!drawerVisible);
  };

  const handleLogoClick = () => {
    navigate('/');
    if (drawerVisible) setDrawerVisible(false);
  };

  const handleThemeSwitch = (mode: any) => {
    toggleTheme(mode);
  };

  return (
    <ConfigProvider
      theme={{
        components: {
          Layout: {
            headerBg: token.colorBgContainer,
            bodyBg: token.colorBgLayout,
          }
        }
      }}
    >
      <Layout style={{ minHeight: '100vh', height: '100vh', display: 'flex', flexDirection: 'column', overflow: 'hidden' }}>
        <StyledHeader theme={{ token }}>
          <LogoContainer horizontal>
            <Button
              type="text"
              icon={<MenuOutlined />}
              onClick={toggleDrawer}
              size="large"
            />
            <Flexbox gap={8} align="center" horizontal onClick={handleLogoClick} style={{ cursor: 'pointer' }}>
              <Avatar 
                src='/logo.png' 
                size={32} 
                shape="square" 
                style={{ 
                  borderRadius: token.borderRadiusSM,
                  boxShadow: `0 2px 8px ${token.colorPrimaryBg}` 
                }} 
              />
              <Title level={5} style={{ margin: 0 }}>Thor</Title>
            </Flexbox>
          </LogoContainer>
          
          <Space>
            <Tooltip title={t('common.installApp')}>
              <PwaInstall 
                buttonMode={true}
                buttonText=""
                buttonType="text"
                buttonSize="small"
                buttonStyle={{ padding: '4px 8px' }}
                logoUrl='/logo.png'
              />
            </Tooltip>
            <LanguageSwitcher />
            <ThemeSwitch
              onThemeSwitch={handleThemeSwitch}
              themeMode={themeMode}
              size="small"
            />
            <Button
              type="primary"
              ghost
              size="small"
              icon={<HomeOutlined />}
              onClick={() => navigate('/model')}
            >
              {t('nav.model')}
            </Button>
          </Space>
        </StyledHeader>
        
        <StyledContent theme={{ token }}>
          <ContentWrapper
            variants={pageVariants}
            initial="initial"
            animate="animate"
            exit="exit"
            transition={{ duration: 0.3 }}
            theme={{ token }}
          >
            <Outlet />
          </ContentWrapper>
        </StyledContent>

        <StyledFooter theme={{ token }}>
          <Text type="secondary" style={{ fontSize: 12 }}>
            Thor ©{new Date().getFullYear()} Created by Thor
          </Text>
        </StyledFooter>

        {/* 导航抽屉 */}
        <Drawer
          title={
            <Flexbox gap={8} align="center" horizontal>
              <Avatar 
                src='/logo.png' 
                size={24} 
                shape="square" 
                style={{ borderRadius: token.borderRadiusSM }} 
              />
              <Text strong>Thor AI</Text>
            </Flexbox>
          }
          placement="left"
          closable={true}
          onClose={toggleDrawer}
          open={drawerVisible}
          bodyStyle={{ padding: 0 }}
        >
          {nav}
        </Drawer>
      </Layout>
    </ConfigProvider>
  );
});

MobileLayout.displayName = 'MobileMainLayout';

export default MobileLayout;

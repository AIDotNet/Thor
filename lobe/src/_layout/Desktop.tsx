import { memo, useMemo, useCallback } from "react";
import { Outlet } from "react-router-dom";
import { Layout, theme, Button, Tooltip, Typography, ConfigProvider, Space, Divider } from "antd";
import { Avatar, Header, ThemeSwitch } from "@lobehub/ui";
import { Dropdown } from "antd";
import { useNavigate } from "react-router-dom";
import useThemeStore from "../store/theme";
import { Flexbox } from "react-layout-kit";
import LanguageSwitcher from "../components/LanguageSwitcher";
import { useTranslation } from "react-i18next";
import styled from "styled-components";
import { motion } from "framer-motion";
import { LayoutProps } from "./type";
import { HomeOutlined } from "@ant-design/icons";
import PwaInstall from "../components/PwaInstall";
import AnnouncementBanner from "../components/AnnouncementBanner";

const { Content, Footer, Sider } = Layout;
const { Text, Title } = Typography;

// 样式组件
const LogoContainer = styled(Flexbox)`
  padding: 16px 0;
  transition: all 0.3s;
`;

const LogoText = styled(Title)`
  cursor: pointer;
  user-select: none;
  margin: 0 !important;
  transition: all 0.3s;
`;

// 使用函数方式传递theme，避免类型错误
const StyledContent = styled(Content)(({ theme }: any) => `
  margin: ${theme?.token?.marginMD || 16}px;
  transition: all 0.3s;
  flex: 1;
  overflow: auto;
  overflow-x: hidden !important;
  display: flex;
  flex-direction: column;
  
  @media (max-width: 576px) {
    margin: ${theme?.token?.marginSM || 8}px;
  }
`);

const ContentWrapper = styled(motion.div)(({ theme }: any) => `
  padding: ${theme?.token?.paddingLG || 24}px;
  min-height: 0;
  flex: 1;
  background: ${theme?.token?.colorBgContainer || '#fff'};
  border-radius: ${theme?.token?.borderRadiusLG || 8}px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
  transition: all 0.3s;
  overflow: auto;
  
  @media (max-width: 576px) {
    padding: ${theme?.token?.paddingMD || 16}px;
  }
`);

const StyledSider = styled(Sider)`
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
  z-index: 10;
  
  .ant-layout-sider-children {
    display: flex;
    flex-direction: column;
    height: 100%;
  }
`;

const StyledFooter = styled(Footer)(({ theme }: any) => `
  text-align: center;
  background: transparent;
  padding: ${theme?.token?.paddingSM || 8}px;
`);

// 页面过渡动画
const pageVariants = {
  initial: { opacity: 0 },
  animate: { opacity: 1 },
  exit: { opacity: 0 }
};

// 用户下拉菜单Hook
const useUserDropdownItems = () => {
  const { t, i18n } = useTranslation();
  const navigate = useNavigate();
  
  return useMemo(() => ({
    items: [
      {
        key: 'account',
        label: t('nav.setting'),
        onClick: () => navigate('/current')
      },
      {
        key: 'current',
        label: t('nav.current'),
        onClick: () => navigate('/current')
      },
      {
        key: 'divider',
        type: 'divider' as const
      },
      {
        key: 'logout',
        label: t('common.logout'),
        onClick: () => {
          localStorage.removeItem('token');
          navigate('/login');
        },
        danger: true
      }
    ]
  }), [t, i18n.language, navigate]);
};

// 侧边栏组件
const Sidebar = memo(({ nav }: { nav: React.ReactNode }) => {
  const { token } = theme.useToken();
  const navigate = useNavigate();
  
  const handleLogoClick = useCallback(() => {
    navigate('/');
  }, [navigate]);
  
  return (
    <StyledSider 
      width={240} 
      theme="light"
      style={{ background: token.colorBgContainer }}
    >
      <LogoContainer align="center" justify="center">
        <Tooltip title="Thor AI 平台管理系统" placement="right">
          <Flexbox gap={token.marginXS} align="center" justify="center" horizontal onClick={handleLogoClick}>
            <Avatar 
              src='/logo.png' 
              size={40} 
              shape="square" 
              style={{ 
                borderRadius: token.borderRadiusSM,
                boxShadow: `0 2px 8px ${token.colorPrimaryBg}` 
              }} 
            />
            <LogoText level={4}>Thor</LogoText>
          </Flexbox>
        </Tooltip>
      </LogoContainer>
      
      <Divider style={{ margin: `0 ${token.marginSM}px` }} />
      
      <div style={{ flex: 1, overflowY: 'auto', padding: `${token.paddingSM}px 0` }}>
        {nav}
      </div>
    </StyledSider>
  );
});

Sidebar.displayName = "DesktopSidebar";

// 头部操作组件
const HeaderActions = memo(() => {
  const navigate = useNavigate();
  const { t } = useTranslation();
  const { themeMode, toggleTheme } = useThemeStore();
  const dropdownItems = useUserDropdownItems();
  const { token } = theme.useToken();
  
  const handleModelClick = useCallback(() => {
    navigate('/model');
  }, [navigate]);
  
  const handleThemeSwitch = useCallback((model: any) => {
    toggleTheme(model);
  }, [toggleTheme]);
  
  return (
    <Space size={token.marginSM}>
      <Button 
        icon={<HomeOutlined />}
        onClick={handleModelClick}
      >
        {t('nav.model')}
      </Button>
      <LanguageSwitcher />
      <ThemeSwitch 
        onThemeSwitch={handleThemeSwitch}
        themeMode={themeMode} 
      />
      <Dropdown menu={dropdownItems} trigger={['click']}>
        <Avatar 
          src='/logo.png' 
          size={40} 
          style={{ 
            cursor: 'pointer',
            boxShadow: `0 2px 8px ${token.colorPrimaryBg}`
          }} 
        />
      </Dropdown>
    </Space>
  );
});

HeaderActions.displayName = "HeaderActions";

// 主内容区域
const MainContent = memo(() => {
  const { token } = theme.useToken();
  
  return (
    <StyledContent theme={{ token }}>
      <ContentWrapper
        variants={pageVariants}
        initial="initial"
        style={{
          overflow: 'auto',
          overflowX: 'hidden',
          height: '100%',
        }}
        animate="animate"
        exit="exit"
        transition={{ duration: 0.3 }}
        theme={{ token }}
      >
        <Outlet />
      </ContentWrapper>
    </StyledContent>
  );
});

MainContent.displayName = "MainContent";

// 主布局组件
const LayoutPage = memo<LayoutProps>(({ nav }) => {
  const { token } = theme.useToken();
  
  return (
    <ConfigProvider
      theme={{
        components: {
          Layout: {
            siderBg: token.colorBgContainer,
            headerBg: token.colorBgContainer,
            bodyBg: token.colorBgLayout,
          }
        }
      }}
    >
      <div style={{ position: 'relative' }}>
        <AnnouncementBanner />
        <Layout style={{ minHeight: "100vh", height: "100vh", overflow: "hidden" }}>
          <Sidebar nav={nav} />
          <Layout style={{ background: token.colorBgLayout, height: "100%", overflow: "auto" }}>
            <Header 
              actions={<HeaderActions />} 
              style={{
                boxShadow: `0 2px 8px rgba(0, 0, 0, 0.06)`,
                zIndex: 9,
              }}
            />
            <MainContent />
          </Layout>
        </Layout>
      </div>
    </ConfigProvider>
  );
});

LayoutPage.displayName = "DesktopMainLayout";

export default LayoutPage;

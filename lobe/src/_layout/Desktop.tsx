import { memo, useMemo, useCallback } from "react";
import { useLocation } from "react-router-dom";
import { Outlet } from "react-router-dom";
import { Layout, theme, Button, Tooltip, Spin } from "antd";
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

const { Content, Footer, Sider } = Layout;

// 样式组件
const LogoText = styled.span`
  font-size: 24px;
  font-weight: 600;
  font-family: 'PingFang SC';
  cursor: pointer;
  user-select: none;
  margin-top: 8px;
  margin-bottom: 20px;
`;

const StyledContent = styled(Content)`
  margin: 16px;
`;

const ContentWrapper = styled(motion.div)`
  padding: 24px;
  min-height: 360px;
  background: ${props => props.theme.token?.colorBgContainer};
  border-radius: ${props => props.theme.token?.borderRadiusLG}px;
`;

const StyledFooter = styled(Footer)`
  text-align: center;
`;

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
        key: 'logout',
        label: t('common.logout'),
        onClick: () => {
          localStorage.removeItem('token');
          navigate('/login');
        }
      }
    ]
  }), [t, i18n.language, navigate]);
};

// 侧边栏组件
const Sidebar = memo(({ nav }) => {
  const { token: { colorBgContainer } } = theme.useToken();
  const navigate = useNavigate();
  
  const handleLogoClick = useCallback(() => {
    navigate('/');
  }, [navigate]);
  
  return (
    <Sider style={{ background: colorBgContainer, paddingTop: "16px" }}>
      <Tooltip title="Thor AI 平台管理系统">
        <Flexbox gap={8} align="center" justify="center" horizontal>
          <LogoText onClick={handleLogoClick}>Thor</LogoText>
        </Flexbox>
      </Tooltip>
      {nav}
    </Sider>
  );
});

Sidebar.displayName = "DesktopSidebar";

// 头部操作组件
const HeaderActions = memo(() => {
  const navigate = useNavigate();
  const { t } = useTranslation();
  const { themeMode, toggleTheme } = useThemeStore();
  const dropdownItems = useUserDropdownItems();
  
  const handleModelClick = useCallback(() => {
    navigate('/model');
  }, [navigate]);
  
  const handleThemeSwitch = useCallback((model) => {
    toggleTheme(model);
  }, [toggleTheme]);
  
  return (
    <>
      <Button type="text" onClick={handleModelClick}>
        {t('nav.model')}
      </Button>
      <LanguageSwitcher />
      <ThemeSwitch 
        onThemeSwitch={handleThemeSwitch}
        themeMode={themeMode} 
      />
      <Dropdown menu={dropdownItems}>
        <Avatar src='/logo.png' size={48} />
      </Dropdown>
    </>
  );
});

HeaderActions.displayName = "HeaderActions";

// 页脚组件
const FooterContent = memo(() => {
  return <StyledFooter>Thor ©{new Date().getFullYear()} Created by Thor</StyledFooter>;
});

FooterContent.displayName = "FooterContent";

// 主内容区域
const MainContent = memo(() => {
  const { token } = theme.useToken();
  const location = useLocation();
  
  return (
    <StyledContent>
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
  );
});

MainContent.displayName = "MainContent";

// 主布局组件
const LayoutPage = memo<LayoutProps>(({ nav }) => {
  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Sidebar nav={nav} />
      <Layout>
        <Header actions={<HeaderActions />} />
        <MainContent />
        <FooterContent />
      </Layout>
    </Layout>
  );
});

LayoutPage.displayName = "DesktopMainLayout";

export default LayoutPage;

import { memo, useMemo } from "react";

import { LayoutProps } from "./type";
import { Outlet } from "react-router-dom";
import { Layout, theme, Button, Tooltip } from "antd";
import { Avatar, Header, ThemeSwitch } from "@lobehub/ui";
import { Dropdown } from "antd";
import { useNavigate } from "react-router-dom";
import useThemeStore from "../store/theme";
import { Flexbox } from "react-layout-kit";
import LanguageSwitcher from "../components/LanguageSwitcher";
import { useTranslation } from "react-i18next";

const { Content, Footer, Sider } = Layout;
const LayoutPage = memo<LayoutProps>(({ nav }) => {
  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();
  const { themeMode, toggleTheme } = useThemeStore();
  const navigate = useNavigate();
  const { t, i18n } = useTranslation();
  
  // 使用useMemo创建下拉菜单项，这样语言变化时会重新生成
  const dropdownItems = useMemo(() => ({
    items: [
      {
        key: 'account',
        label: t('nav.setting'),
        onClick: () => {
          navigate('/current')
        }
      },
      {
        key: 'current',
        label: t('nav.current'),
        onClick: () => {
          navigate('/current')
        }
      },
      {
        key: 'logout',
        label: t('common.logout'),
        onClick: () => {
          localStorage.removeItem('token')
          navigate('/login')
        }
      }
    ]
  }), [t, i18n.language, navigate]); // 依赖i18n.language以响应语言变化
  
  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Sider
        style={{
          background: colorBgContainer,
          paddingTop: "16px",
        }}
      >
        <Tooltip title="Thor AI 平台管理系统">
          <Flexbox gap={8}
            align={'center'}
            justify={'center'}
            horizontal>
            <span style={{
              fontSize: 24,
              fontWeight: 600,
              fontFamily: 'PingFang SC',
              cursor: 'pointer',
              userSelect: 'none',
              marginTop: '8px',
              marginBottom: '20px',
            }}
              onClick={() => {
                navigate('/')
              }}>
              Thor
            </span>
          </Flexbox>
        </Tooltip>
        {nav}
      </Sider>
      <Layout>
        <Header actions={
          <>
            <Button type="text" onClick={() => {
              navigate('/model')
            }}>
              {t('nav.model')}
            </Button>
            <LanguageSwitcher />
            <ThemeSwitch onThemeSwitch={(model) => toggleTheme(model)}
              themeMode={themeMode} />
            <Dropdown menu={dropdownItems}>
              <Avatar src='/logo.png' size={48} />
            </Dropdown>
          </>
        }>

        </Header>
        <Content style={{ margin: "16px" }}>
          <div
            style={{
              padding: 24,
              minHeight: 360,
              background: colorBgContainer,
              borderRadius: borderRadiusLG,
            }}
          >
            <Outlet />
          </div>
        </Content>
        <Footer style={{ textAlign: "center" }}>
          Thor ©{new Date().getFullYear()} Created by Thor
        </Footer>
      </Layout>
    </Layout>
  );
});

LayoutPage.displayName = "DesktopMainLayout";

export default LayoutPage;

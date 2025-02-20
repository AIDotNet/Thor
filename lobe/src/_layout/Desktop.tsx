import { memo } from "react";

import { LayoutProps } from "./type";
import { Outlet } from "react-router-dom";
import { Layout, theme } from "antd";
import { Avatar, Header, Logo, ThemeSwitch } from "@lobehub/ui";
import { Dropdown } from "antd";
import { useNavigate } from "react-router-dom";
import useThemeStore from "../store/theme";


const { Content, Footer, Sider } = Layout;
const LayoutPage = memo<LayoutProps>(({ nav }) => {
  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();
  const { themeMode, toggleTheme } = useThemeStore();
  const navigate = useNavigate();
  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Sider
        style={{
          background: colorBgContainer,
          paddingTop: "16px",
        }}
      >
        <Logo extra={"Thor"} style={{
          textAlign: 'center',
          marginLeft: '8px',
          marginTop: '8px',
          marginBottom: '8px',
        }} size={48} />
        {nav}
      </Sider>
      <Layout>
        <Header actions={
          <>
            <ThemeSwitch onThemeSwitch={(model) => toggleTheme(model)}
              themeMode={themeMode} />
            <Dropdown menu={{
              items: [
                {
                  key: 'account',
                  label: '账户设置',
                  onClick: () => {
                    navigate('/current')
                  }
                },
                {
                  key: 'logout',
                  label: '退出登录',
                  onClick: () => {
                    localStorage.removeItem('token')
                    navigate('/login')
                  }
                }
              ]
            }}>
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

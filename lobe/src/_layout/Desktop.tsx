import { memo } from "react";

import { LayoutProps } from "./type";
import { Outlet } from "react-router-dom";
import { Layout, theme, Button, Tooltip } from "antd";
import { Avatar, Header, ThemeSwitch } from "@lobehub/ui";
import { Dropdown } from "antd";
import { useNavigate } from "react-router-dom";
import useThemeStore from "../store/theme";
import { Flexbox } from "react-layout-kit";

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
              查看模型价格
            </Button>
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
                  key: 'current',
                  label: '个人信息',
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

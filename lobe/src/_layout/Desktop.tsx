import { memo } from "react";

import { LayoutProps } from "./type";
import { Outlet } from "react-router-dom";
import { Layout, message, theme } from "antd";
import { Avatar, Header, Logo } from "@lobehub/ui";
import { Dropdown } from "antd";
import { useNavigate } from "react-router-dom";
import { useActiveUser } from "../hooks/useActiveTabKey";

const { Content, Footer, Sider } = Layout;
const LayoutPage = memo<LayoutProps>(({ nav }) => {

  const user = useActiveUser()

  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();
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
            <Dropdown menu={{
              items: [
                {
                  key: 'share',
                  label: '获取分享连接',
                  onClick: () => {
                    const shareurl = window.origin + "/register?code=" + user?.id;
                    navigator.clipboard.writeText(shareurl).then(() => {
                      message.success("复制成功")
                    }).catch(err => {
                      console.error('复制失败', err);
                    });
                  }
                },
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

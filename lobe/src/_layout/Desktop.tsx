import { memo } from "react";

import { LayoutProps } from "./type";
import { Outlet } from "react-router-dom";
import { Layout, theme } from "antd";
import { Logo } from "@lobehub/ui";

const { Header, Content, Footer, Sider } = Layout;
const LayoutPage = memo<LayoutProps>(({ nav }) => {
  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();

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
        }} size={40}/>
        {nav}
      </Sider>
      <Layout>
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

import { Layout, Nav,  } from "@douyinfe/semi-ui";
import { Outlet, useNavigate } from 'react-router-dom'

export default function DocLayout() {
    const navigation = useNavigate();

    const { Header, Content } = Layout;
    return (<Layout style={{
        height: '100vh',
    }}>
        <Layout>
            <Header style={{ backgroundColor: 'var(--semi-color-bg-1)' }}>
                <Nav
                    items={[
                        {
                            text: "控制台",
                            onClick: () => {
                                navigation('/panel')
                            }
                        },
                        {
                            text: "接入教程",
                            onClick: () => {
                                navigation('/doc')
                            }
                        },
                        {
                            text: "模型列表",
                            onClick: () => {
                                navigation('/model')
                            }
                        }
                    ]}
                    mode="horizontal"
                ></Nav>
            </Header>
            <Content
                style={{
                    backgroundColor: 'var(--semi-color-bg-0)',
                    overflowY: 'auto',
                    padding: '24px',
                }}
            >
                <Outlet />
            </Content>
        </Layout>
    </Layout>
    )
}
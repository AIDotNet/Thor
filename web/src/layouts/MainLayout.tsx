import { Layout, Nav, Button, Avatar, Switch } from '@douyinfe/semi-ui';
import { IconMoon, IconSun, IconBytedanceLogo, IconArticle, IconUser, IconUserSetting, IconBranch, IconHistogram, IconKey, IconSetting, IconSemiLogo } from '@douyinfe/semi-icons';
import { Outlet, useNavigate } from 'react-router-dom';
import { useState } from 'react';
const body = document.body;

localStorage.getItem('theme-mode') === 'dark' && body.setAttribute('theme-mode', 'dark');

export default function MainLayout() {
    const navigation = useNavigate();
    const [key, setKey] = useState('Home');
    const { Header, Footer, Sider, Content } = Layout;
    const [dark, setDark] = useState(body.hasAttribute('theme-mode'));

    function SwitchTheme(dark: boolean) {
        const rect = document.documentElement.getBoundingClientRect();
        const x = rect.right;
        const y = rect.top;
        document.documentElement.style.setProperty('--x', x + 'px')
        document.documentElement.style.setProperty('--y', y + 'px')
        document.startViewTransition(() => {
            setDark(dark);
            if (!dark) {
                body.removeAttribute('theme-mode');
                localStorage.setItem('theme-mode', 'light');
            } else {
                body.setAttribute('theme-mode', 'dark');
                localStorage.setItem('theme-mode', 'dark');
            }
        });
    }
    return (
        <Layout style={{
            height: '100vh',
        }}>
            <Sider style={{ backgroundColor: 'var(--semi-color-bg-1)' }}>
                <Nav
                    defaultSelectedKeys={[key]}
                    key={key}
                    style={{ maxWidth: 220, height: '100%' }}
                    items={[
                        { itemKey: 'Home', text: '总览', icon: <IconHistogram size="large" />, },
                        { itemKey: 'Channel', text: '渠道', icon: <IconBranch size="large" /> },
                        { itemKey: 'Token', text: '令牌', icon: <IconKey size="large" /> },
                        { itemKey: 'Logger', text: '日志', icon: <IconArticle size="large" /> },
                        { itemKey: 'User', text: '用户', icon: <IconUser size="large" /> },
                        { itemKey: 'Current', text: '我的', icon: <IconUserSetting size="large" /> },
                        { itemKey: 'Setting', text: '设置', icon: <IconSetting size="large" /> },
                    ]}
                    header={{
                        logo: <IconSemiLogo style={{ fontSize: 36 }} />,
                        text: 'AIDotNet API',
                    }}
                    onClick={(data) => {
                        switch (data.itemKey) {
                            case 'Home':
                                navigation('/panel');
                                break;
                            case 'Channel':
                                navigation('/channel');
                                break;
                            case 'Token':
                                navigation('/token');
                                break;
                            case 'Logger':
                                navigation('/logger');
                                break;
                            case 'User':
                                navigation('/user');
                                break;
                            case 'Current':
                                navigation('/current');
                                break;
                            case 'Setting':
                                navigation('/setting');
                                break;
                        }
                        setKey(data.itemKey as string);
                    }}
                    footer={{
                        collapseButton: true,
                    }}
                />
            </Sider>
            <Layout>
                <Header style={{ backgroundColor: 'var(--semi-color-bg-1)' }}>
                    <Nav
                        mode="horizontal"
                        footer={
                            <>
                                <Switch onChange={(v, e) => {
                                    SwitchTheme(v);
                                }} checked={dark} checkedText={<IconSun />} uncheckedText={<IconMoon />} size="large" style={{ marginRight: 5 }} />
                                <Avatar color="orange" size="small">
                                    Token
                                </Avatar>
                            </>
                        }
                    ></Nav>
                </Header>
                <Content
                    style={{
                        padding: '24px',
                        backgroundColor: 'var(--semi-color-bg-0)',
                    }}
                >
                    <Outlet />
                </Content>
                <Footer
                    style={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        padding: '20px',
                        color: 'var(--semi-color-text-2)',
                        backgroundColor: 'rgba(var(--semi-grey-0), 1)',
                    }}
                >
                    <span
                        style={{
                            display: 'flex',
                            alignItems: 'center',
                        }}
                    >
                        <IconBytedanceLogo size="large" style={{ marginRight: '8px' }} />
                        <span>Token © 2024 </span>
                    </span>
                    <span>
                        <span style={{ marginRight: '24px' }}>平台客服</span>
                        <span>反馈建议</span>
                    </span>
                </Footer>
            </Layout>
        </Layout>
    );
};

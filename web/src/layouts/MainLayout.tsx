import { Layout, Nav, Avatar, Switch, Dropdown, Button } from '@douyinfe/semi-ui';
import { IconMoon, IconCreditCard, IconSun, IconCart, IconUserCardVideo, IconGithubLogo, IconArticle, IconUser, IconUserSetting, IconBranch, IconHistogram, IconComment, IconKey, IconSetting, IconSemiLogo } from '@douyinfe/semi-icons';
import { Outlet, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { info } from '../services/UserService';
import { GeneralSetting, InitSetting } from '../services/SettingService';
const body = document.body;

localStorage.getItem('theme-mode') === 'dark' && body.setAttribute('theme-mode', 'dark');

export default function MainLayout() {
    const [user, setUser] = useState({} as any);
    const navigation = useNavigate();
    const [key, setKey] = useState('Home');
    const { Header, Footer, Sider, Content } = Layout;
    const [dark, setDark] = useState(body.hasAttribute('theme-mode'));
    const [items, setItems] = useState([
        {
            itemKey: 'Home',
            text: '总览',
            icon: <IconHistogram size="large" />,
            enable: true,
            role: 'user,admin'
        },
        {
            itemKey: 'Channel',
            text: '渠道',
            icon: <IconBranch size="large" />,
            enable: true,
            role: 'admin'
        },
        {
            itemKey: 'Chat',
            text: '对话',
            icon: <IconComment size="large" />,
            enable: false,
            role: 'user,admin'
        },
        {
            itemKey: 'Vidol',
            text: '数字人',
            icon: <IconUserCardVideo size="large" />,
            enable: false,
            role: 'user,admin'
        },
        {
            itemKey: 'Token',
            text: '令牌',
            icon: <IconKey size="large" />,
            enable: true,
            role: 'user,admin'
        },
        {
            itemKey: 'Product',
            text: '产品管理',
            icon: <IconCart size="large" />,
            enable: true,
            role: 'admin'
        },
        {
            itemKey: 'Logger',
            text: '日志',
            icon: <IconArticle size="large" />,
            enable: true,
            role: 'user,admin'
        },
        {
            itemKey: 'RedeemCode',
            text: '兑换码',
            icon: <IconCreditCard size="large" />,
            enable: true,
            role: 'admin'
        },
        {
            itemKey: 'User',
            text: '用户管理',
            icon: <IconUser size="large" />,
            enable: true,
            role: 'admin'
        },
        {
            itemKey: 'Current',
            text: '钱包/个人信息',
            icon: <IconUserSetting size="large" />,
            enable: true,
            role: 'user,admin'
        },
        {
            itemKey: 'Setting',
            text: '设置',
            icon: <IconSetting size="large" />,
            enable: true,
            role: 'admin'
        },
    ]);

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

    function loadUser() {
        info()
            .then((res) => {
                setUser(res.data);
            });
    }

    useEffect(() => {
        // 获取当前用户token
        const token = localStorage.getItem('token');
        if (!token) {
            navigation('/login');
            return;
        }

        // 解析token
        const role = localStorage.getItem('role') as string;

        const chatLink = InitSetting?.find(x => x.key === GeneralSetting.ChatLink)?.value

        if (chatLink) {
            // 修改 Chat 
            items.forEach(item => {
                if (item.itemKey === 'Chat') {
                    item.enable = true;
                }
            })
        }

        const vidolLink = InitSetting?.find(x => x.key === GeneralSetting.VidolLink)?.value

        if (vidolLink) {
            // 修改 Vidol 
            items.forEach(item => {
                if (item.itemKey === 'Vidol') {
                    item.enable = true;
                }
            })
        }


        setItems(items.filter(item => item.enable && item.role.includes(role)));


        // 获取当前路由
        const path = window.location.pathname;
        // 绑定key
        switch (path) {
            case '/panel':
                setKey('Home');
                break;
            case '/channel':
                setKey('Channel');
                break;
            case '/token':
                setKey('Token');
                break;
            case '/logger':
                setKey('Logger');
                break;
            case '/user':
                setKey('User');
                break;
            case '/current':
                setKey('Current');
                break;
            case '/setting':
                setKey('Setting');
                break;
            case '/redeemCode':
                setKey('RedeemCode');
                break;
            case '/chat':
                setKey('Chat');
                break;
            case '/product':
                setKey('Product');
                break;
            case '/vidol':
                setKey('Vidol');
                break;
        }

        // 获取用户信息
        loadUser();
    }, [])

    return (
        <Layout style={{
            height: '100vh',
        }}>
            <Sider style={{ backgroundColor: 'var(--semi-color-bg-1)' }}>
                <Nav
                    defaultSelectedKeys={[key]}
                    key={key}
                    style={{ maxWidth: 220, height: '100%' }}
                    items={items}

                    header={{
                        logo: <Avatar src='/logo.png' />,
                        text: "AI Gateway",
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
                            case 'RedeemCode':
                                navigation('/redeemCode');
                                break;
                            case 'Chat':
                                navigation('/chat');
                                break;
                            case 'Product':
                                navigation('/product');
                                break;
                            case 'Vidol':
                                navigation('/vidol');
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
                        items={[
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
                        footer={
                            <>
                                <Switch onChange={(v) => {
                                    SwitchTheme(v);
                                }} checked={dark} checkedText={<IconSun />} uncheckedText={<IconMoon />} size="large" style={{ marginRight: 5 }} />

                                <Dropdown
                                    render={
                                        <Dropdown.Menu>
                                            <Dropdown.Item key="1" onClick={() => {
                                                navigation('/current');
                                            }}>个人中心</Dropdown.Item>
                                            <Dropdown.Item key="2" onClick={() => {
                                                localStorage.removeItem('token');
                                                navigation('/login');
                                            }}>退出登录</Dropdown.Item>
                                        </Dropdown.Menu>
                                    }
                                >
                                    <Avatar color="orange" size="small" src={user.avatar ?? '/logo.png'}>
                                    </Avatar>
                                    <span style={{
                                        marginLeft: 10,
                                        marginRight: 10
                                    }}>
                                        {user.userName}
                                    </span>
                                </Dropdown>
                            </>
                        }
                    ></Nav>
                </Header>
                <Content
                    style={{
                        backgroundColor: 'var(--semi-color-bg-0)',
                        overflowY: 'auto'
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

                    </span>
                    <span>
                        <Button
                            theme='borderless'
                            onClick={() => {
                                window.open('https://github.com/AIDotNet/AIDotNet.API', '_blank')
                            }}
                        >
                            <IconGithubLogo size="large" />
                        </Button>
                    </span>
                </Footer>
            </Layout>
        </Layout>
    );
};

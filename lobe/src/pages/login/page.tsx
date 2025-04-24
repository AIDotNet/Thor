import { memo, useState, useEffect } from 'react';
import { message, Input, Button, Card, Typography, Space, Divider, Form, Layout, theme } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone, GithubOutlined, UserOutlined, LockOutlined } from '@ant-design/icons';
import { Tooltip } from '@lobehub/ui';
import { login } from '../../services/AuthorizeService';
import { InitSetting, SystemSetting } from '../../services/SettingService';
import { useNavigate } from 'react-router-dom';
import Gitee from '../../components/Icon/Gitee';
import Casdoor from '../../components/Icon/Casdoor';
import { useTranslation } from 'react-i18next';

const { Title, Paragraph } = Typography;
const { Content } = Layout;

const Login = memo(() => {
    const { t } = useTranslation();
    const { token: themeToken } = theme.useToken();
    const params = new URLSearchParams(location.search);
    const redirect_uri = params.get('redirect_uri');
    const navigate = useNavigate();

    const [loading, setLoading] = useState(false);
    const [form] = Form.useForm();
    const enableCasdoorAuth = InitSetting.find(s => s.key === SystemSetting.EnableCasdoorAuth)?.value;

    const handleAuthRedirect = (url: string) => {
        window.location.href = url;
    };

    const handleGithub = () => {
        const clientId = InitSetting.find(s => s.key === SystemSetting.GithubClientId)?.value;
        if (!clientId) {
            message.error(t('login.configGithubClientId'));
            return;
        }
        handleAuthRedirect(`https://github.com/login/oauth/authorize?client_id=${clientId}&redirect_uri=${location.origin}/auth&response_type=code`);
    };

    useEffect(() => {
        localStorage.removeItem('redirect_uri');
        if (redirect_uri) {
            const url = new URL(redirect_uri);
            localStorage.setItem('redirect_uri', url.toString());
        }
    }, [redirect_uri]);
    
    const handleLogin = async (values: any) => {
        try {
            setLoading(true);
            const token = await login({ account: values.username, pass: values.password });
            if (token.success) {
                localStorage.setItem('token', token.data.token);
                localStorage.setItem('role', token.data.role);
                message.success(t('login.loginSuccess'));
                if (redirect_uri) {
                    const url = new URL(redirect_uri);
                    url.searchParams.append('token', token.data.token);
                    handleAuthRedirect(url.toString());
                    return;
                }
                setTimeout(() => navigate('/panel'), 1000);
            } else {
                message.error(`${t('login.loginFailed')}: ${token.message}`);
            }
        } catch (e) {
            message.error(t('login.loginError'));
        } finally {
            setLoading(false);
        }
    };

    const handlerGitee = () => {
        const enable = InitSetting.find(s => s.key === SystemSetting.EnableGiteeLogin)?.value;
        if (!enable) {
            message.error(t('login.enableGiteeLogin'));
            return;
        }
        const clientId = InitSetting.find(s => s.key === SystemSetting.GiteeClientId)?.value;
        if (!clientId) {
            message.error(t('login.configGiteeClientId'));
            return;
        }
        handleAuthRedirect(`https://gitee.com/oauth/authorize?client_id=${clientId}&redirect_uri=${location.origin}/auth/gitee&response_type=code`);
    };

    const handleCasdoorAuth = () => {
        let casdoorEndipoint = InitSetting.find(s => s.key === SystemSetting.CasdoorEndipoint)?.value as string;
        if (!casdoorEndipoint) {
            message.error(t('login.configCasdoorEndpoint'));
            return;
        }
        const casdoorClientId = InitSetting.find(s => s.key === SystemSetting.CasdoorClientId)?.value;
        if (!casdoorClientId) {
            message.error(t('login.configCasdoorClientId'));
            return;
        }
        if (casdoorEndipoint.endsWith('/')) {
            casdoorEndipoint = casdoorEndipoint.slice(0, -1);
        }
        handleAuthRedirect(`${casdoorEndipoint}/login/oauth/authorize?client_id=${casdoorClientId}&redirect_uri=${location.origin}/auth/casdoor&response_type=code&scope=open email profile`);
    };

    return (
        <Layout style={{ 
            minHeight: '100vh', 
            background: themeToken.colorBgLayout
        }}>
            <Content style={{ 
                display: 'flex', 
                justifyContent: 'center', 
                alignItems: 'center', 
                padding: '50px 20px'
            }}>
                <Card 
                    bordered={false}
                    style={{ 
                        width: '100%', 
                        maxWidth: 420,
                        backgroundColor: themeToken.colorBgContainer,
                        boxShadow: `0 1px 2px -2px ${themeToken.colorPrimary}33, 
                                    0 3px 6px 0 ${themeToken.colorPrimary}23, 
                                    0 5px 12px 4px ${themeToken.colorPrimary}1a`,
                    }}
                >
                    <Space direction="vertical" size="large" style={{ width: '100%' }}>
                        <div style={{ textAlign: 'center' }}>
                            <Title level={2} style={{ margin: '0 0 8px', color: themeToken.colorPrimary }}>{t('login.title')}</Title>
                            <Paragraph type="secondary">{t('login.inputAccountInfo')}</Paragraph>
                        </div>
                        
                        <Form
                            form={form}
                            onFinish={handleLogin}
                            size="large"
                            layout="vertical"
                        >
                            <Form.Item
                                name="username"
                                rules={[{ required: true, message: t('login.accountRequired') }]}
                            >
                                <Input 
                                    prefix={<UserOutlined />} 
                                    placeholder={t('login.accountPlaceholder')} 
                                />
                            </Form.Item>
                            
                            <Form.Item
                                name="password"
                                rules={[{ required: true, message: t('login.passwordRequired') }]}
                            >
                                <Input.Password
                                    prefix={<LockOutlined />}
                                    placeholder={t('login.passwordPlaceholder')}
                                    iconRender={visible => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
                                />
                            </Form.Item>
                            
                            <Form.Item>
                                <Button
                                    type="primary"
                                    htmlType="submit"
                                    loading={loading}
                                    block
                                >
                                    {t('login.loginButton')}
                                </Button>
                            </Form.Item>
                        </Form>
                        <div style={{ textAlign: 'center' }}>
                            <Button 
                                type="link" 
                                onClick={() => window.location.href = '/register'}
                            >
                                {t('login.registerNow')}
                            </Button>
                        </div>
                        <Divider plain>{t('login.thirdPartyLogin')}</Divider>
                        <div style={{ display: 'flex', justifyContent: 'center', gap: 16 }}>
                            <Tooltip title={t('login.githubLogin')}>
                                <Button 
                                    type="default"
                                    shape="circle" 
                                    icon={<GithubOutlined style={{ fontSize: 18 }} />} 
                                    onClick={handleGithub}
                                    size="large"
                                />
                            </Tooltip>
                            <Tooltip title={t('login.giteeLogin')}>
                                <Button 
                                    type="default"
                                    shape="circle" 
                                    icon={<Gitee />} 
                                    onClick={handlerGitee}
                                    size="large"
                                />
                            </Tooltip>
                            {enableCasdoorAuth && (
                                <Tooltip title={t('login.casdoorLogin')}>
                                    <Button 
                                        type="default"
                                        shape="circle" 
                                        icon={<Casdoor />} 
                                        onClick={handleCasdoorAuth}
                                        size="large"
                                    />
                                </Tooltip>
                            )}
                        </div>
                    </Space>
                </Card>
            </Content>
        </Layout>
    );
});

export default Login;

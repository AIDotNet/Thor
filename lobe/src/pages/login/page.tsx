import { memo, useState, useEffect } from 'react';
import { message, Input, Button, Form, Typography,  Divider, theme } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone, GithubOutlined, UserOutlined, LockOutlined } from '@ant-design/icons';
import { Tooltip } from '@lobehub/ui';
import { login } from '../../services/AuthorizeService';
import { InitSetting, SystemSetting } from '../../services/SettingService';
import { useNavigate } from 'react-router-dom';
import Gitee from '../../components/Icon/Gitee';
import Casdoor from '../../components/Icon/Casdoor';
import { useTranslation } from 'react-i18next';
import styled from 'styled-components';

const { Title, Text, Paragraph } = Typography;

// 自定义主题接口
interface CustomTheme {
  backgroundColor: string;
  formBg: string;
  gradientBg: string;
  linkColor: string;
  linkHoverColor: string;
}

const PageContainer = styled.div<{ theme: CustomTheme }>`
  display: flex;
  min-height: 100vh;
  background: ${props => props.theme.backgroundColor};
`;

const BrandSide = styled.div<{ theme: CustomTheme }>`
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  padding: 40px;
  color: white;
  position: relative;
  overflow: hidden;
  @media (max-width: 992px) {
    display: none;
  }
`;

const BrandContent = styled.div`
  max-width: 480px;
  z-index: 2;
  text-align: center;
`;

const FormSide = styled.div<{ theme: CustomTheme }>`
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  padding: 40px 20px;
  background: ${props => props.theme.formBg};
  overflow-y: auto;
  
  @media (max-width: 992px) {
    width: 100%;
  }
`;

const FormContainer = styled.div`
  width: 100%;
  max-width: 420px;
  padding: 0 20px;
`;

const ActionsContainer = styled.div`
  margin-top: 24px;
  display: flex;
  flex-direction: column;
  gap: 16px;
`;

const RegisterLink = styled(Text)<{ theme: CustomTheme }>`
  text-align: center;
  cursor: pointer;
  color: ${props => props.theme.linkColor};
  transition: color 0.3s;
  
  &:hover {
    color: ${props => props.theme.linkHoverColor};
    text-decoration: underline;
  }
`;

const AnimatedShape = styled.div`
  position: absolute;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.1);
  animation: float 15s infinite ease-in-out;
  
  &:nth-child(1) {
    width: 300px;
    height: 300px;
    top: -50px;
    left: -100px;
    animation-delay: 0s;
  }
  
  &:nth-child(2) {
    width: 200px;
    height: 200px;
    bottom: 50px;
    right: 30px;
    animation-delay: 2s;
  }
  
  &:nth-child(3) {
    width: 150px;
    height: 150px;
    bottom: -50px;
    left: 30%;
    animation-delay: 4s;
  }
  
  @keyframes float {
    0% { transform: translateY(0) rotate(0deg); }
    50% { transform: translateY(-20px) rotate(5deg); }
    100% { transform: translateY(0) rotate(0deg); }
  }
`;

const FeatureItem = styled.div`
  display: flex;
  align-items: center;
  margin-bottom: 20px;
  text-align: left;
`;

const FeatureIcon = styled.div`
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.2);
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 16px;
  font-size: 20px;
`;

const SocialButtonsContainer = styled.div`
  display: flex;
  justify-content: center;
  gap: 16px;
  margin-top: 8px;
`;

const SocialButton = styled(Button)`
  transition: all 0.3s ease;
  
  &:hover {
    transform: translateY(-3px);
    box-shadow: 0 6px 12px rgba(0,0,0,0.1);
  }
`;

const Login = memo(() => {
    const { t } = useTranslation();
    const { token: themeToken } = theme.useToken();
    const params = new URLSearchParams(location.search);
    const redirect_uri = params.get('redirect_uri');
    const navigate = useNavigate();

    const [loading, setLoading] = useState(false);
    const [form] = Form.useForm();
    const enableCasdoorAuth = InitSetting.find(s => s.key === SystemSetting.EnableCasdoorAuth)?.value;

    // 设置主题色
    const themeColors = {
      backgroundColor: themeToken.colorBgLayout,
      formBg: themeToken.colorBgContainer,
      gradientBg: `linear-gradient(135deg, ${themeToken.colorPrimary} 0%, ${themeToken.colorPrimaryActive} 100%)`,
      linkColor: themeToken.colorPrimary,
      linkHoverColor: themeToken.colorPrimaryHover
    };

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
        <PageContainer theme={themeColors}>
            {/* 左侧品牌展示区 */}
            <BrandSide theme={themeColors}>
                <AnimatedShape />
                <AnimatedShape />
                <AnimatedShape />

                <BrandContent>
                    <Title level={1} style={{ color: 'white', marginTop: 24 }}>
                        TokenAI 
                    </Title>
                    <Paragraph style={{ color: 'rgba(255,255,255,0.8)', fontSize: 16, marginBottom: 40 }}>
                        {t('login.brandSlogan')}
                    </Paragraph>

                    <Divider style={{ backgroundColor: 'rgba(255,255,255,0.2)', margin: '30px 0' }} />

                    <FeatureItem>
                        <FeatureIcon>🚀</FeatureIcon>
                        <div>
                            <Text style={{ color: 'white', fontWeight: 'bold', fontSize: 16 }}>
                                {t('login.feature1Title')}
                            </Text>
                            <Paragraph style={{ color: 'rgba(255,255,255,0.8)', margin: 0 }}>
                                {t('login.feature1Desc')}
                            </Paragraph>
                        </div>
                    </FeatureItem>

                    <FeatureItem>
                        <FeatureIcon>🔒</FeatureIcon>
                        <div>
                            <Text style={{ color: 'white', fontWeight: 'bold', fontSize: 16 }}>
                                {t('login.feature2Title')}
                            </Text>
                            <Paragraph style={{ color: 'rgba(255,255,255,0.8)', margin: 0 }}>
                                {t('login.feature2Desc')}
                            </Paragraph>
                        </div>
                    </FeatureItem>
                </BrandContent>
            </BrandSide>

            {/* 右侧表单区 */}
            <FormSide theme={themeColors}>
                <FormContainer>
                    <div style={{ textAlign: 'center', marginBottom: 40 }}>
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
                        
                        <ActionsContainer>
                            <Button
                                type="primary"
                                htmlType="submit"
                                loading={loading}
                                block
                                size="large"
                            >
                                {t('login.loginButton')}
                            </Button>
                            
                            <RegisterLink 
                                theme={themeColors}
                                onClick={() => navigate('/register')}
                            >
                                {t('login.registerNow')}
                            </RegisterLink>
                        </ActionsContainer>
                    </Form>

                    <Divider plain>{t('login.thirdPartyLogin')}</Divider>
                    
                    <SocialButtonsContainer>
                        <Tooltip title={t('login.githubLogin')}>
                            <SocialButton 
                                type="default"
                                shape="circle" 
                                icon={<GithubOutlined style={{ fontSize: 18 }} />} 
                                onClick={handleGithub}
                                size="large"
                            />
                        </Tooltip>
                        <Tooltip title={t('login.giteeLogin')}>
                            <SocialButton 
                                type="default"
                                shape="circle" 
                                icon={<Gitee />} 
                                onClick={handlerGitee}
                                size="large"
                            />
                        </Tooltip>
                        {enableCasdoorAuth && (
                            <Tooltip title={t('login.casdoorLogin')}>
                                <SocialButton 
                                    type="default"
                                    shape="circle" 
                                    icon={<Casdoor />} 
                                    onClick={handleCasdoorAuth}
                                    size="large"
                                />
                            </Tooltip>
                        )}
                    </SocialButtonsContainer>
                </FormContainer>
            </FormSide>
        </PageContainer>
    );
});

export default Login;

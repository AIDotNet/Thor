import { memo, useState, useCallback, useEffect, useRef } from 'react';
import { message, Input, Button, Form, Spin, Typography, Steps, Divider, theme } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone, UserOutlined, MailOutlined, LockOutlined, SafetyOutlined, GiftOutlined } from '@ant-design/icons';
import { Avatar } from '@lobehub/ui';
import styled from 'styled-components';
import { create, GetEmailCode } from '../../services/UserService';
import { useNavigate, useLocation } from 'react-router-dom';
import { login } from '../../services/AuthorizeService';
import { IsEnableEmailRegister } from '../../services/SettingService';
import { useTranslation } from 'react-i18next';

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
  max-width: 480px;
  padding: 0 20px;
`;

const LogoContainer = styled.div`
  display: flex;
  align-items: center;
  margin-bottom: 40px;
  
  @media (max-width: 992px) {
    justify-content: center;
  }
`;

const LogoText = styled.div`
  margin-left: 16px;
`;

const StepsContainer = styled.div`
  margin-bottom: 30px;
`;

const ActionsContainer = styled.div`
  margin-top: 30px;
  display: flex;
  flex-direction: column;
  gap: 16px;
`;

const LoginLink = styled(Text) <{ theme: CustomTheme }>`
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

// 彩蛋动画容器
const ConfettiContainer = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  pointer-events: none;
  z-index: 1000;
`;

// 彩蛋粒子样式
const Confetti = styled.div<{ color: string; size: number; left: string; top: string; animationDuration: string; delay: string }>`
  position: fixed;
  width: ${props => props.size}px;
  height: ${props => props.size}px;
  background-color: ${props => props.color};
  left: ${props => props.left};
  top: ${props => props.top};
  opacity: 0;
  border-radius: ${props => Math.random() > 0.5 ? '50%' : props.size / 5 + 'px'};
  transform: scale(0);
  animation: confettiAnim ${props => props.animationDuration} ease-out forwards;
  animation-delay: ${props => props.delay};
  
  @keyframes confettiAnim {
    0% {
      transform: scale(0);
      opacity: 1;
    }
    50% {
      opacity: 1;
    }
    100% {
      transform: scale(1) translate(${() => (Math.random() - 0.5) * 200}px, ${() => (Math.random() - 0.5) * 200}px) rotate(${() => Math.random() * 720}deg);
      opacity: 0;
    }
  }
`;

// 彩蛋爆炸效果
const Explosion = styled.div<{ top: string; left: string }>`
  position: fixed;
  top: ${props => props.top};
  left: ${props => props.left};
  width: 10px;
  height: 10px;
  border-radius: 50%;
  background-color: #ff4d4f;
  animation: explode 0.5s ease-out forwards;
  z-index: 1001;
  
  @keyframes explode {
    0% {
      transform: scale(0);
      opacity: 1;
      box-shadow: 0 0 20px 10px rgba(255, 77, 79, 0.8);
    }
    100% {
      transform: scale(20);
      opacity: 0;
      box-shadow: 0 0 0 0 rgba(255, 77, 79, 0);
    }
  }
`;

const RegisterPage = memo(() => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const location = useLocation();
  const [loading, setLoading] = useState(false);
  const [countDown, setCountDown] = useState(0);
  const [form] = Form.useForm();
  const enableEmailRegister = IsEnableEmailRegister();
  const easterEggTimer = useRef<ReturnType<typeof setTimeout> | null>(null);
  const [showConfetti, setShowConfetti] = useState(false);
  const [explosionPosition, setExplosionPosition] = useState({ top: '0px', left: '0px' });
  const [confettiItems, setConfettiItems] = useState<Array<{
    id: number;
    color: string;
    size: number;
    left: string;
    top: string;
    animationDuration: string;
    delay: string;
  }>>([]);
  const registerButtonRef = useRef<HTMLButtonElement>(null);
  const [currentStep, setCurrentStep] = useState(0);

  // 使用antd的主题系统
  const { token } = theme.useToken();

  // 设置主题色
  const themeColors = {
    backgroundColor: token.colorBgLayout,
    formBg: token.colorBgContainer,
    gradientBg: `linear-gradient(135deg, ${token.colorPrimary} 0%, ${token.colorPrimaryActive} 100%)`,
    linkColor: token.colorPrimary,
    linkHoverColor: token.colorPrimaryHover
  };

  useEffect(() => {
    if (countDown > 0) {
      const timer = setInterval(() => {
        setCountDown((prev) => prev - 1);
      }, 1000);
      return () => clearInterval(timer);
    }
  }, [countDown]);

  useEffect(() => {
    const searchParams = new URLSearchParams(location.search);
    const inviteCode = searchParams.get('inviteCode');
    if (inviteCode) {
      form.setFieldsValue({ inviteCode });
    }
  }, [location, form]);

  // 生成彩蛋粒子
  const generateConfetti = useCallback((buttonElement: HTMLElement) => {
    const rect = buttonElement.getBoundingClientRect();
    const centerX = rect.left + rect.width / 2;
    const centerY = rect.top + rect.height / 2;

    // 设置爆炸中心点
    setExplosionPosition({
      top: `${centerY}px`,
      left: `${centerX}px`
    });

    // 生成彩蛋粒子 - 从按钮中心向四周炸开
    const colors = [token.colorError, token.colorWarning, token.colorInfo, token.colorSuccess, token.colorPrimary, token.colorPrimaryActive];
    const newConfetti = Array.from({ length: 150 }, (_, i) => {
      // 随机角度和距离，确保粒子向四周散开
      const angle = Math.random() * Math.PI * 2; // 0-360度的随机角度
      const distance = Math.random() * 100 + 20; // 随机距离

      // 计算粒子的起始位置（相对于爆炸中心）
      const x = centerX + Math.cos(angle) * distance;
      const y = centerY + Math.sin(angle) * distance;

      return {
        id: i,
        color: colors[Math.floor(Math.random() * colors.length)],
        size: Math.random() * 10 + 5,
        left: `${x}px`,
        top: `${y}px`,
        animationDuration: `${0.5 + Math.random() * 1}s`,
        delay: `${Math.random() * 0.2}s`,
      };
    });

    setConfettiItems(newConfetti);
    setShowConfetti(true);

    // 5秒后清除粒子
    setTimeout(() => {
      setShowConfetti(false);
      setConfettiItems([]);
    }, 5000);
  }, [token]);

  const playEasterEgg = useCallback(() => {
    if (registerButtonRef.current) {
      generateConfetti(registerButtonRef.current);
    }
  }, [generateConfetti]);

  const onFinish = useCallback(async (values: any) => {
    try {
      console.log('Form values:', values);
      setLoading(true);
      const resp = await create({
        userName: values.userName,
        email: values.email,
        password: values.password,
        confirmPassword: values.confirmPassword,
        code: values.code || '',
        inviteCode: values.inviteCode
      });

      if (resp.success) {
        message.success(t('register.registerSuccess'));
        
        // 触发彩蛋动画
        playEasterEgg();
        
        // 自动登录
        easterEggTimer.current = setTimeout(async () => {
          const loginResp = await login({
            account: values.userName,
            pass: values.password
          });
          
          if (loginResp.success) {
            localStorage.setItem('token', loginResp.data.token);
            localStorage.setItem('role', loginResp.data.role);
            setTimeout(() => navigate('/panel'), 1000);
          } else {
            setTimeout(() => navigate('/login'), 1500);
          }
        }, 2000);
      } else {
        message.error(t('register.userCreationFailed') + ': ' + resp.message);
      }
    } catch (error) {
      message.error(t('register.registerError'));
      console.error('Registration error:', error);
    } finally {
      setLoading(false);
    }
  }, [navigate, playEasterEgg, t]);

  const handleGetCode = useCallback(async () => {
    try {
      const email = form.getFieldValue('email');
      if (!email) {
        message.error(t('register.emailRequired'));
        return;
      }
      
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(email)) {
        message.error(t('register.emailInvalid'));
        return;
      }
      
      setCountDown(60);
      const resp = await GetEmailCode(email);
      
      if (resp.success) {
        message.success(t('register.verificationCodeSent'));
      } else {
        // 验证码获取失败时，给用户友好提示
        message.warning(resp.message || t('register.verificationCodeOptional'));
        setCountDown(0);
      }
    } catch (error) {
      console.error('Error sending verification code:', error);
      message.warning(t('register.verificationCodeOptional'));
      setCountDown(0);
    }
  }, [form, t]);

  const nextStep = () => {
    form.validateFields(['userName', 'email']).then((values) => {
      // 单独保存验证通过的值
      console.log('Step 1 validated values:', values);
      
      // 存储第一步表单的值以便于最终提交使用
      const formValues = form.getFieldsValue();
      console.log('Current form values:', formValues);
      
      setCurrentStep(1);
    }).catch(err => {
      console.log('Validation errors:', err);
    });
  };

  const prevStep = () => {
    setCurrentStep(0);
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
            {t('register.brandSlogan')}
          </Paragraph>

          <Divider style={{ backgroundColor: 'rgba(255,255,255,0.2)', margin: '30px 0' }} />

          <FeatureItem>
            <FeatureIcon>🚀</FeatureIcon>
            <div>
              <Text style={{ color: 'white', fontWeight: 'bold', fontSize: 16 }}>
                {t('register.feature1Title')}
              </Text>
              <Paragraph style={{ color: 'rgba(255,255,255,0.8)', margin: 0 }}>
                {t('register.feature1Desc')}
              </Paragraph>
            </div>
          </FeatureItem>

        </BrandContent>
      </BrandSide>

      {/* 右侧表单区 */}
      <FormSide theme={themeColors}>
        <FormContainer>
          <LogoContainer>
            <Avatar size={48} shape="square" src="/logo.png" />
            <LogoText>
              <Title level={4} style={{ margin: 0 }}>
                Thor
              </Title>
              <Text type="secondary">{t('register.subtitle')}</Text>
            </LogoText>
          </LogoContainer>

          <Title level={3} style={{ marginBottom: 24 }}>{t('register.title')}</Title>

          <StepsContainer>
            <Steps
              current={currentStep}
              items={[
                {
                  title: t('register.step1'),
                },
                {
                  title: t('register.step2'),
                }
              ]}
            />
          </StepsContainer>

          <Spin spinning={loading}>
            <Form
              form={form}
              layout="vertical"
              onFinish={onFinish}
              autoComplete="off"
              requiredMark={false}
            >
              {/* 第一步表单内容 - 始终存在但根据步骤控制显示 */}
              <div style={{ display: currentStep === 0 ? 'block' : 'none' }}>
                <Form.Item
                  name="userName"
                  label={t('register.usernameLabel')}
                  rules={[
                    { required: true, message: t('register.usernameRequired') }
                  ]}
                >
                  <Input 
                    prefix={<UserOutlined />} 
                    placeholder={t('register.usernamePlaceholder')} 
                    size="large" 
                  />
                </Form.Item>
                
                <Form.Item
                  name="email"
                  label={t('register.emailLabel')}
                  rules={[
                    { required: true, message: t('register.emailRequired') },
                    { type: 'email', message: t('register.emailInvalid') }
                  ]}
                >
                  <Input 
                    prefix={<MailOutlined />} 
                    placeholder={t('register.emailPlaceholder')} 
                    size="large" 
                  />
                </Form.Item>
                
                {/* 验证码输入字段 - 根据邮箱验证开关控制显示 */}
                {enableEmailRegister && (
                  <Form.Item
                    name="code"
                    label={`${t('register.verificationCodeLabel')} ${!enableEmailRegister ? `(${t('common.optional')})` : ''}`}
                    rules={[
                      { required: false, message: t('register.verificationCodePlaceholder') }
                    ]}
                  >
                    <Input
                      prefix={<SafetyOutlined />}
                      placeholder={t('register.verificationCodePlaceholder')}
                      size="large"
                      addonAfter={
                        !enableEmailRegister ? null : (
                          <Button 
                            type="link" 
                            disabled={countDown > 0}
                            onClick={handleGetCode}
                            style={{ padding: 0, height: 'auto', width: '100%' }}
                          >
                            {countDown > 0 
                              ? t('register.resendCode', { count: countDown }) 
                              : t('register.getVerificationCode')}
                          </Button>
                        )
                      }
                    />
                  </Form.Item>
                )}
                
                <ActionsContainer>
                  <Button 
                    type="primary" 
                    size="large" 
                    onClick={nextStep} 
                    block
                  >
                    {t('register.nextStep')}
                  </Button>
                  
                  <LoginLink onClick={() => navigate('/login')} theme={themeColors}>
                    {t('register.loginLink')}
                  </LoginLink>
                </ActionsContainer>
              </div>
              
              {/* 第二步表单内容 - 始终存在但根据步骤控制显示 */}
              <div style={{ display: currentStep === 1 ? 'block' : 'none' }}>
                <Form.Item
                  name="password"
                  label={t('register.passwordLabel')}
                  rules={[
                    { required: true, message: t('register.passwordRequired') },
                    { min: 6, message: t('register.passwordLength') }
                  ]}
                >
                  <Input.Password
                    prefix={<LockOutlined />}
                    placeholder={t('register.passwordPlaceholder')}
                    size="large"
                    iconRender={visible => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
                  />
                </Form.Item>
                
                <Form.Item
                  name="confirmPassword"
                  label={t('register.confirmPasswordLabel')}
                  dependencies={['password']}
                  rules={[
                    { required: true, message: t('register.confirmPasswordRequired') },
                    ({ getFieldValue }) => ({
                      validator(_, value) {
                        if (!value || getFieldValue('password') === value) {
                          return Promise.resolve();
                        }
                        return Promise.reject(new Error(t('register.passwordMismatch')));
                      },
                    }),
                  ]}
                >
                  <Input.Password
                    prefix={<LockOutlined />}
                    placeholder={t('register.confirmPasswordPlaceholder')}
                    size="large"
                    iconRender={visible => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
                  />
                </Form.Item>
                
                <Form.Item
                  name="inviteCode"
                  label={t('register.inviteCodeLabel')}
                >
                  <Input
                    prefix={<GiftOutlined />}
                    placeholder={t('register.inviteCodePlaceholder')}
                    size="large"
                  />
                </Form.Item>
                
                <ActionsContainer>
                  <Button 
                    type="primary" 
                    size="large" 
                    htmlType="submit" 
                    block
                    loading={loading}
                    ref={registerButtonRef}
                  >
                    {t('register.registerButton')}
                  </Button>
                  
                  <Button 
                    size="large" 
                    onClick={prevStep} 
                    block
                  >
                    {t('register.prevStep')}
                  </Button>
                </ActionsContainer>
              </div>
            </Form>
          </Spin>
        </FormContainer>
      </FormSide>

      {/* 彩蛋效果 */}
      {showConfetti && (
        <ConfettiContainer>
          <Explosion top={explosionPosition.top} left={explosionPosition.left} />
          {confettiItems.map(item => (
            <Confetti
              key={item.id}
              color={item.color}
              size={item.size}
              left={item.left}
              top={item.top}
              animationDuration={item.animationDuration}
              delay={item.delay}
            />
          ))}
        </ConfettiContainer>
      )}
    </PageContainer>
  );
});

export default RegisterPage;
import { memo, useState, useCallback, useEffect, useRef } from 'react';
import { message, Input, Button, Form, Card,  Spin, Typography } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone, UserOutlined, MailOutlined, LockOutlined, SafetyOutlined } from '@ant-design/icons';
import { Avatar,  useCreateStore } from '@lobehub/ui';
import styled from 'styled-components';
import { create, GetEmailCode } from '../../services/UserService';
import { useNavigate, useLocation } from 'react-router-dom';
import { login } from '../../services/AuthorizeService';
import { IsEnableEmailRegister } from '../../services/SettingService';
import { useTranslation } from 'react-i18next';

const { Title, Text } = Typography;

const PageContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 20px;
`;

const StyledCard = styled(Card)`
  width: 100%;
  max-width: 620px;
  min-width: 420px;
  border-radius: 12px;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12);
  
  .ant-card-body {
    padding: 30px;
  }
`;

const LogoContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-bottom: 30px;
`;

const ActionsContainer = styled.div`
  margin-top: 20px;
  display: flex;
  flex-direction: column;
  gap: 16px;
`;

const LoginLink = styled(Text)`
  text-align: center;
  cursor: pointer;
  color: #1890ff;
  transition: color 0.3s;
  
  &:hover {
    color: #40a9ff;
    text-decoration: underline;
  }
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
  const store = useCreateStore();
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
    const colors = ['#ff4d4f', '#ffa940', '#fadb14', '#52c41a', '#1890ff', '#722ed1', '#eb2f96'];
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
  }, []);

  const playEasterEgg = useCallback(() => {
    if (registerButtonRef.current) {
      generateConfetti(registerButtonRef.current);
    }
  }, [generateConfetti]);

  const onFinish = useCallback(async (values: any) => {
    try {
      if (!enableEmailRegister) {
        message.error(t('register.emailNotAllowed'));
        return;
      }

      setLoading(true);
      const resp = await create({
        userName: values.username,
        email: values.email,
        password: values.password,
        confirmPassword: values.confirmPassword,
        code: values.verificationCode,
        inviteCode: values.inviteCode
      });

      if (resp.success) {
        message.success(t('register.registerSuccess'));
        
        // 触发彩蛋动画
        playEasterEgg();
        
        // 自动登录
        easterEggTimer.current = setTimeout(async () => {
          const loginResp = await login({
            account: values.username,
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
  }, [enableEmailRegister, navigate, playEasterEgg, t]);

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
        message.error(resp.message || 'Failed to send verification code');
        setCountDown(0); // 失败时重置倒计时
      }
    } catch (error) {
      console.error('Error sending verification code:', error);
      message.error('Failed to send verification code');
      setCountDown(0);
    }
  }, [form, t]);

  return (
    <PageContainer>
      <StyledCard>
        <LogoContainer>
          <Avatar size={80} shape="square" src="/logo.png" />
          <Title level={2} style={{ marginTop: 16, marginBottom: 4 }}>{t('register.title')}</Title>
          <Text type="secondary">{t('register.subtitle')}</Text>
        </LogoContainer>
        
        <Spin spinning={loading}>
          <Form
            form={form}
            layout="vertical"
            onFinish={onFinish}
            autoComplete="off"
            requiredMark={false}
          >
            <Form.Item
              name="username"
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
              name="verificationCode"
              label={t('register.verificationCodeLabel')}
              rules={[
                { required: true, message: t('register.verificationCodePlaceholder') }
              ]}
            >
              <Input
                prefix={<SafetyOutlined />}
                placeholder={t('register.verificationCodePlaceholder')}
                size="large"
                addonAfter={
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
                }
              />
            </Form.Item>
            
            <Form.Item
              name="inviteCode"
              label={t('register.inviteCodeLabel')}
            >
              <Input
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
              
              <LoginLink onClick={() => navigate('/login')}>
                {t('register.loginLink')}
              </LoginLink>
            </ActionsContainer>
          </Form>
        </Spin>
      </StyledCard>
      
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
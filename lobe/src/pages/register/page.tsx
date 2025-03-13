import { memo, useState, useCallback, useEffect, useRef } from 'react';
import { message, Input, Button, Form, Card, Divider, Spin, Typography } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone, UserOutlined, MailOutlined, LockOutlined, SafetyOutlined } from '@ant-design/icons';
import { Avatar, LogoProps, useControls, useCreateStore } from '@lobehub/ui';
import styled from 'styled-components';
import { create, GetEmailCode } from '../../services/UserService';
import { useNavigate, useLocation } from 'react-router-dom';
import { login } from '../../services/AuthorizeService';
import { IsEnableEmailRegister } from '../../services/SettingService';

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

  const control: LogoProps | any = useControls(
    {
      size: {
        max: 240,
        min: 16,
        step: 4,
        value: 80,
      },
      type: {
        options: ['3d', 'flat', 'high-contrast', 'text', 'combine'],
        value: 'high-contrast',
      },
    },
    { store },
  );

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
      const offsetX = Math.cos(angle) * distance;
      const offsetY = Math.sin(angle) * distance;

      return {
        id: i,
        color: colors[Math.floor(Math.random() * colors.length)],
        size: Math.random() * 10 + 5,
        left: `${centerX + offsetX}px`,
        top: `${centerY + offsetY}px`,
        animationDuration: `${Math.random() * 2 + 1}s`,
        delay: `${Math.random() * 0.3}s`
      };
    });

    setConfettiItems(newConfetti);
    setShowConfetti(true);

    // 5秒后清除彩蛋
    setTimeout(() => {
      setShowConfetti(false);
    }, 2000);
  }, []);

  const handleRegister = useCallback(async (values: any) => {
    const { username, email, password, code, inviteCode } = values;

    if (enableEmailRegister && !code) {
      message.error('请输入验证码');
      return;
    }

    setLoading(true);
    try {
      const res = await create({
        userName: username,
        email,
        password,
        code: code || '',
        inviteCode: inviteCode || ''
      });

      if (res.success) {
        message.success('注册成功');

        // 触发彩蛋效果
        if (registerButtonRef.current) {
          generateConfetti(registerButtonRef.current);
        }
        const loginRes = await login({ account: username, pass: password });
        if (loginRes.success) {
          easterEggTimer.current = setTimeout(async () => {
            localStorage.setItem('token', loginRes.data.token);
            localStorage.setItem('role', loginRes.data.role);
            message.success('登录成功，即将跳转到首页');
            setTimeout(() => navigate('/panel'), 1000);
          }, 2000);
        } else {
          message.error(`登录失败: ${loginRes.message}`);
          setLoading(false);
        }
      } else {
        message.error(`注册失败: ${res.message}`);
        setLoading(false);
      }
    } catch (error) {
      console.error(error);
      message.error('注册过程中发生错误');
      setLoading(false);
    }
  }, [enableEmailRegister, navigate, generateConfetti]);

  const handleGetEmailCode = useCallback(() => {
    form.validateFields(['email']).then(({ email }) => {
      GetEmailCode(email).then((res) => {
        if (res.success) {
          message.success('验证码已发送到您的邮箱');
          setCountDown(60);
        } else {
          message.error(res.message || '获取验证码失败');
        }
      }).catch(() => {
        message.error('网络错误，请稍后重试');
      });
    }).catch(() => {
      message.error('请先输入有效的邮箱地址');
    });
  }, [form]);

  // 清除定时器
  useEffect(() => {
    return () => {
      if (easterEggTimer.current) {
        clearTimeout(easterEggTimer.current);
      }
    };
  }, []);

  return (
    <PageContainer>
      <Spin spinning={loading} tip="处理中...">
        <StyledCard>
          <LogoContainer>
            <Avatar src='/logo.png' {...control} />
            <Title level={2} style={{ marginTop: 16, marginBottom: 0 }}>创建账号</Title>
            <Text type="secondary">加入我们，开始您的旅程</Text>
          </LogoContainer>

          <Form
            form={form}
            layout="vertical"
            requiredMark={false}
            onFinish={handleRegister}
            autoComplete="off"
          >
            <Form.Item
              name="username"
              rules={[{ required: true, message: '请输入用户名' }]}
            >
              <Input
                prefix={<UserOutlined />}
                size="large"
                placeholder="用户名"
              />
            </Form.Item>

            <Form.Item
              name="email"
              rules={[
                { required: true, message: '请输入邮箱' },
                { type: 'email', message: '请输入有效的邮箱地址' }
              ]}
            >
              <Input
                prefix={<MailOutlined />}
                size="large"
                placeholder="邮箱地址"
                suffix={enableEmailRegister ? (
                  <Button
                    type="link"
                    disabled={countDown > 0}
                    onClick={handleGetEmailCode}
                    style={{ padding: '0 8px' }}
                  >
                    {countDown > 0 ? `${countDown}秒` : '获取验证码'}
                  </Button>
                ) : null}
              />
            </Form.Item>

            {enableEmailRegister && (
              <Form.Item
                name="code"
                rules={[{ required: true, message: '请输入验证码' }]}
              >
                <Input
                  prefix={<SafetyOutlined />}
                  size="large"
                  placeholder="验证码"
                />
              </Form.Item>
            )}

            <Form.Item
              name="password"
              rules={[
                { required: true, message: '请输入密码' },
                { min: 6, message: '密码长度不能少于6位' }
              ]}
            >
              <Input.Password
                prefix={<LockOutlined />}
                size="large"
                placeholder="密码"
                iconRender={visible => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
              />
            </Form.Item>

            <Form.Item
              name="inviteCode"
              label="邀请码"
            >
              <Input
                size="large"
                placeholder="邀请码（可选）"
              />
            </Form.Item>

            <ActionsContainer>
              <Button
                type="primary"
                htmlType="submit"
                size="large"
                block
                style={{ height: '46px', borderRadius: '8px' }}
                ref={registerButtonRef}
              >
                注册
              </Button>

              <Divider plain><Text type="secondary">或者</Text></Divider>

              <LoginLink onClick={() => navigate('/login')}>
                已有账号？点击登录
              </LoginLink>
            </ActionsContainer>
          </Form>
        </StyledCard>
      </Spin>

      {/* 彩蛋爆炸效果 */}
      {showConfetti && (
        <>
          <Explosion top={explosionPosition.top} left={explosionPosition.left} />
          <ConfettiContainer>
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
        </>
      )}
    </PageContainer>
  );
});

export default RegisterPage;
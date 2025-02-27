import { memo, useState, useCallback, useEffect } from 'react';
import { message, Input, Button, Form, Card, Divider, Spin, Typography } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone, UserOutlined, MailOutlined, LockOutlined, SafetyOutlined } from '@ant-design/icons';
import { Avatar, LogoProps, useControls, useCreateStore } from '@lobehub/ui';
import styled from 'styled-components';
import { create, GetEmailCode } from '../../services/UserService';
import { useNavigate } from 'react-router-dom';
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

const RegisterPage = memo(() => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [countDown, setCountDown] = useState(0);
  const [form] = Form.useForm();
  const enableEmailRegister = IsEnableEmailRegister();
  const store = useCreateStore();

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

  const handleRegister = useCallback(async (values: any) => {
    const { username, email, password, code } = values;
    
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
        code: code || '' 
      });
      
      if (res.success) {
        message.success('注册成功，正在自动登录');
        setTimeout(async () => {
          const loginRes = await login({ account: username, pass: password });
          if (loginRes.success) {
            localStorage.setItem('token', loginRes.data.token);
            localStorage.setItem('role', loginRes.data.role);
            message.success('登录成功，即将跳转到首页');
            setTimeout(() => navigate('/panel'), 1000);
          } else {
            message.error(`登录失败: ${loginRes.message}`);
            setLoading(false);
          }
        }, 200);
      } else {
        message.error(`注册失败: ${res.message}`);
        setLoading(false);
      }
    } catch (error) {
      console.error(error);
      message.error('注册过程中发生错误');
      setLoading(false);
    }
  }, [enableEmailRegister, navigate]);

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
            
            <ActionsContainer>
              <Button 
                type="primary" 
                htmlType="submit" 
                size="large" 
                block
                style={{ height: '46px', borderRadius: '8px' }}
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
    </PageContainer>
  );
});

export default RegisterPage;
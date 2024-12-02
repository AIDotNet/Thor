import { memo, useState, useCallback } from 'react';
import { message, Input, Button } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone } from '@ant-design/icons';
import { Avatar, LogoProps, useControls, useCreateStore } from '@lobehub/ui';
import styled from 'styled-components';
import { create, GetEmailCode } from '../../services/UserService';
import { useNavigate } from 'react-router-dom';
import { login } from '../../services/AuthorizeService';
import { IsEnableEmailRegister } from '../../services/SettingService';

const Container = styled.div`
    display: flex;
    flex-direction: column;
    align-items: center;
    margin: 0 auto;
    width: 380px;
    margin-bottom: 20px;
`;

const FunctionTools = styled.div`
    display: flex;
    justify-content: center;
    align-items: center;
    text-align: center;
    margin-top: 20px;
    color: #0366d6;
`;

const RegisterPage = memo(() => {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [countDown, setCountDown] = useState(0);
    const [userName, setUserName] = useState('');
    const [code, setCode] = useState('');
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');
    const store = useCreateStore();
    const enableEmailRegister = IsEnableEmailRegister();

    const control: LogoProps | any = useControls(
        {
            size: {
                max: 240,
                min: 16,
                step: 4,
                value: 64,
            },
            type: {
                options: ['3d', 'flat', 'high-contrast', 'text', 'combine'],
                value: 'high-contrast',
            },
        },
        { store },
    );

    const handleLogin = useCallback(async () => {
        if (enableEmailRegister && code.length === 0) {
            message.error('请输入验证码');
            return;
        }
        setLoading(true);
        try {
            const res = await create({ userName, email, password, code });
            if (res.success) {
                message.success('注册成功，正在自动登录');
                setTimeout(async () => {
                    const loginRes = await login({ account: userName, pass: password });
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
        } catch {
            setLoading(false);
        }
    }, [userName, email, password, code, enableEmailRegister, navigate]);

    const handleGetEmailCode = useCallback(() => {
        GetEmailCode(email).then((res) => {
            if (res.success) {
                message.success('发送成功');
                setCountDown(60);
                const timer = setInterval(() => {
                    setCountDown((prev) => {
                        if (prev <= 0) {
                            clearInterval(timer);
                            return 0;
                        }
                        return prev - 1;
                    });
                }, 1000);
            } else {
                message.error(res.message);
            }
        });
    }, [email]);

    return (
        <>
            <Container>
                <div style={{ textAlign: 'center', marginBottom: '20px', marginTop: '20%' }}>
                    <Avatar src='/logo.png' {...control} />
                    <h2>注册账号</h2>
                </div>
                <Input
                    value={userName}
                    onChange={(e) => setUserName(e.target.value)}
                    size='large'
                    placeholder="请输入账号"
                    style={{ marginBottom: '20px', width: '100%' }}
                />
                <Input
                    value={email}
                    suffix={enableEmailRegister ? (
                        <Button
                            disabled={countDown > 0}
                            onClick={handleGetEmailCode}
                        >
                            {countDown > 0 ? `${countDown}秒后重新获取` : '获取验证码'}
                        </Button>
                    ) : null}
                    onChange={(e) => setEmail(e.target.value)}
                    size='large'
                    placeholder="请输入邮箱"
                    style={{ marginBottom: '20px', width: '100%' }}
                />
                {enableEmailRegister && (
                    <Input
                        value={code}
                        onChange={(e) => setCode(e.target.value)}
                        size='large'
                        placeholder="请输入验证码"
                        style={{ marginBottom: '20px', width: '100%' }}
                    />
                )}
                <Input.Password
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    size='large'
                    placeholder="请输入密码"
                    iconRender={visible => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
                />
            </Container>
            <Container>
                <Button
                    loading={loading}
                    onClick={handleLogin}
                    size='large'
                    type="primary"
                    block
                >
                    注册
                </Button>
            </Container>
            <FunctionTools>
                <span onClick={() => navigate('/login')}>
                    已经有账号？去登录
                </span>
            </FunctionTools>
        </>
    );
});

export default RegisterPage;
import { memo, useState } from 'react';
import { message, Input, Button } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone } from '@ant-design/icons';
import { Avatar, LogoProps, useControls, useCreateStore } from '@lobehub/ui';
import styled from 'styled-components';
import { create, GetEmailCode } from '../../services/UserService';
import { useNavigate } from 'react-router-dom';
import { login } from '../../services/AuthorizeService';
import {
    IsEnableEmailRegister
} from '../../services/SettingService';


const FunctionTools = styled.div`
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
    cursor: pointer;
    justify-content: center;
    align-items: center;
    text-align: center;
    margin: 0 auto;
    width: 380px;
    margin-top: 20px;
    color: #0366d6;
`;

const RegisterPage = memo(() => {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);

    // 验证码的倒计时
    const [countDown, setCountDown] = useState(0);

    const [userName, setUserName] = useState('');
    const [code, setCode] = useState('');
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');
    const store = useCreateStore();

    const enableEmailRegister = IsEnableEmailRegister()

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

    function handleLogin() {

        if(enableEmailRegister && code.length === 0){
            message.error('请输入验证码');
            return;
        }
        setLoading(true);
        create({
            userName: userName,
            email: email,
            password,
            code
        })
            .then((res) => {
                if (res.success) {
                    message.success({
                        title: '注册成功',
                        content: '正在自动登录'
                    } as any);

                    // 等待200ms
                    setTimeout(() => {
                        login({
                            account: userName, pass: password
                        })
                            .then((res) => {
                                if (res.success) {
                                    localStorage.setItem('token', res.data.token);
                                    localStorage.setItem('role', res.data.role);
                                    message.success({
                                        title: '登录成功',
                                        content: '即将跳转到首页'
                                    } as any);
                                    setTimeout(() => {
                                        navigate('/panel');
                                    }, 1000);
                                } else {
                                    message.error({
                                        title: '登录失败',
                                        content: res.message
                                    } as any);
                                    setLoading(false);
                                }
                            })
                            .catch(() => {
                                setLoading(false);
                            });
                    }, 200);
                } else {
                    message.error({
                        title: '注册失败',
                        content: res.message
                    } as any);
                    setLoading(false);
                }
            })
            .catch(() => {
                setLoading(false);
            });
    }


    return (
        <>
            <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', margin: '0 auto', width: '380px', marginBottom: '20px' }}>
                <div style={{ textAlign: 'center', marginBottom: '20px', marginTop: '20%' }}>
                    <Avatar src='/logo.png' {...control} />
                    <h2>
                        注册账号
                    </h2>
                </div>
                <div style={{ marginBottom: '20px', width: '100%' }}>
                    <Input
                        value={userName}
                        onChange={(e) => setUserName(e.target.value)}
                        size='large'
                        placeholder="请输入账号" />
                </div>
                <div style={{ marginBottom: '20px', width: '100%' }}>
                    <Input
                        value={email}
                        suffix={enableEmailRegister ? <Button
                            disabled={countDown > 0}
                            onClick={()=>{
                            GetEmailCode(email).then((res) => {
                                if(res.success){
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

                                    

                                }else{
                                    message.error(res.message);
                                }
                            });
                        }}>
                            {countDown > 0 ? `${countDown}秒后重新获取` : '获取验证码'}
                        </Button> : null}
                        onChange={(e) => setEmail(e.target.value)}
                        size='large'
                        placeholder="请输入邮箱" />
                </div>
                {
                    enableEmailRegister &&
                    <div style={{ marginBottom: '20px', width: '100%' }}>
                        <Input
                            value={code}
                            onChange={(e) => setCode(e.target.value)}
                            size='large'
                            placeholder="请输入验证码" />
                    </div>
                }
                <Input.Password
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    size='large'
                    placeholder="请输入密码"
                    iconRender={visible => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
                />
            </div>
            <div style={{ marginBottom: '20px', marginTop: '20px', display: 'flex', flexDirection: 'column', alignItems: 'center', margin: '0 auto', width: '380px' }}>
                <Button
                    loading={loading}
                    onClick={async () => {
                        setLoading(true);
                        await handleLogin();
                        setLoading(false);

                    }}
                    size='large'
                    type="primary"
                    block >
                    注册
                </Button>
            </div>
            <FunctionTools>
                <span onClick={() => {
                    navigate('/login');
                }}>
                    已经有账号？去登录
                </span>
            </FunctionTools>
        </>
    );
});

export default RegisterPage;
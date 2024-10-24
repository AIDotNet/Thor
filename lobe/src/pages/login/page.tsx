import { memo, useState } from 'react';
import { message, Input, Button } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone, GithubOutlined, } from '@ant-design/icons';
import { GridShowcase, Tooltip } from '@lobehub/ui';
import styled from 'styled-components';
import { login } from '../../services/AuthorizeService';
import { InitSetting, SystemSetting } from '../../services/SettingService';
import { useNavigate } from 'react-router-dom';
import Divider from '@lobehub/ui/es/Form/components/FormDivider';
import Gitee from '../../components/Icon/Gitee';

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

const Login = memo(() => {
    const params = new URLSearchParams(location.search);

    const redirect_uri = params.get('redirect_uri');
    const navigate = useNavigate();

    const [loading, setLoading] = useState(false);
    const [user, setUser] = useState('');
    const [password, setPassword] = useState('');

    function handleGithub() {

        const clientId = InitSetting.find(s => s.key === SystemSetting.GithubClientId)?.value;

        if (!clientId) {
            message.error({
                content: '请联系管理员配置 Github ClientId',
            });
            return;
        }

        // 跳转 Github 授权页面
        window.location.href = `https://github.com/login/oauth/authorize?client_id=${clientId}&redirect_uri=${location.origin}/auth&response_type=code`;
    }

    async function handleLogin() {

        try {
            setLoading(true);
            const token = await login({
                account: user,
                pass: password,
            });

            if (token.success) {
                localStorage.setItem('token', token.data.token);
                localStorage.setItem('role', token.data.role);
                message.success({
                    title: '登录成功',
                    content: '即将跳转到首页'
                } as any);

                if (redirect_uri && redirect_uri.startsWith('http')) {
                    const url = new URL(redirect_uri);
                    url.searchParams.append('token', token.data.token);
                    window.location.href = url.toString();
                    return;
                }

                setTimeout(() => {
                    navigate('/panel');
                }, 1000);
            } else {
                message.error({
                    title: '登录失败',
                    content: token.message
                } as any);
                setLoading(false);
            }

        } catch (e) {

        }
        setLoading(false);
    }

    function handlerGitee() {

        // 判断是否开启gitee登录
        const enable = InitSetting.find(s => s.key === SystemSetting.EnableGiteeLogin)?.value;

        if (!enable) {
            message.error({
                content: '请联系管理员开启 Gitee 登录',
            });
            return;
        }

        const clientId = InitSetting.find(s => s.key === SystemSetting.GiteeClientId)?.value;

        if (!clientId) {
            message.error({
                content: '请联系管理员配置 Github ClientId',
            });
            return;
        }

        // 跳转 Gitee 授权页面
        window.location.href = `https://gitee.com/oauth/authorize?client_id=${clientId}&redirect_uri=${location.origin}/auth/gitee&response_type=code`;

    }

    return (
        <GridShowcase>
            <div style={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                margin: '0 auto',
                width: '100%',
                height: '100%',
                justifyContent: 'center',
                textAlign: 'center',

            }}>
                <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', margin: '0 auto', width: '380px', marginBottom: '20px' }}>
                    <div style={{ textAlign: 'center', marginBottom: '20px' }}>
                        <span style={{
                            fontSize: '28px',
                            fontWeight: 'bold',
                            marginBottom: '20px',
                            display: 'block',

                        }}>
                            TokenAI 登录系统
                        </span>
                    </div>
                    <div style={{ marginBottom: '20px', width: '100%' }}>
                        <Input
                            value={user}
                            onChange={(e) => setUser(e.target.value)}
                            size='large'
                            placeholder="请输入账号" />
                    </div>
                    <div style={{ width: '100%' }}></div>
                    <Input.Password
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        size='large'
                        placeholder="请输入密码"
                        // 按下回车键
                        onPressEnter={async () => {
                            await handleLogin();
                        }}
                        iconRender={visible => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
                    />
                </div>
                <div style={{
                    marginBottom: '20px',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    width: '380px',
                    marginTop: '20px',
                }}>
                    <Button
                        loading={loading}
                        onClick={async () => {
                            await handleLogin();
                        }}
                        size='large'
                        type="primary"
                        block >
                        登录
                    </Button>
                </div>
                <FunctionTools>
                    <span onClick={() => {
                        if (typeof window === 'undefined') return;
                        window.location.href = '/register';
                    }}>
                        注册账号
                    </span>
                </FunctionTools>
                <Divider>
                    第三方登录
                </Divider>
                <div style={{
                    // 居中并且一排
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    width: '100%',
                    textAlign: 'center',
                    marginBottom: '20px',
                    marginTop: '20px',
                    // 两个按钮之间的间距
                    gap: '20px',
                }}>
                    <Tooltip title="Github 登录">
                        <Button onClick={() => { handleGithub() }} size='large' icon={<GithubOutlined />} />
                    </Tooltip>
                    <Tooltip title="Gitee 登录">
                        <Button onClick={() => { handlerGitee() }} size='large' icon={<Gitee />} />
                    </Tooltip>
                </div>
            </div>
        </GridShowcase>
    );
});

export default Login;
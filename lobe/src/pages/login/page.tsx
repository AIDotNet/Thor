import { memo, useState ,useEffect} from 'react';
import { message, Input, Button } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone, GithubOutlined, } from '@ant-design/icons';
import { GridShowcase, Tooltip } from '@lobehub/ui';
import styled from 'styled-components';
import { login } from '../../services/AuthorizeService';
import { InitSetting, SystemSetting } from '../../services/SettingService';
import { useNavigate } from 'react-router-dom';
import Divider from '@lobehub/ui/es/Form/components/FormDivider';
import Gitee from '../../components/Icon/Gitee';
import Casdoor from '../../components/Icon/Casdoor';

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
    const enableCasdoorAuth = InitSetting.find(s => s.key === SystemSetting.EnableCasdoorAuth)?.value;

    const handleAuthRedirect = (url: string) => {
        window.location.href = url;
    };

    const handleGithub = () => {
        const clientId = InitSetting.find(s => s.key === SystemSetting.GithubClientId)?.value;
        if (!clientId) {
            message.error({ content: '请联系管理员配置 Github ClientId' });
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
    
    const handleLogin = async () => {
        try {
            setLoading(true);
            const token = await login({ account: user, pass: password });
            if (token.success) {
                localStorage.setItem('token', token.data.token);
                localStorage.setItem('role', token.data.role);
                message.success({ title: '登录成功', content: '即将跳转到首页' } as any);
                if (redirect_uri) {
                    const url = new URL(redirect_uri);
                    url.searchParams.append('token', token.data.token);
                    handleAuthRedirect(url.toString());
                    return;
                }
                setTimeout(() => navigate('/panel'), 1000);
            } else {
                message.error({ title: '登录失败', content: token.message } as any);
            }
        } catch (e) {
            message.error({ content: '登录过程中出现错误，请重试' });
        } finally {
            setLoading(false);
        }
    };

    const handlerGitee = () => {
        const enable = InitSetting.find(s => s.key === SystemSetting.EnableGiteeLogin)?.value;
        if (!enable) {
            message.error({ content: '请联系管理员开启 Gitee 登录' });
            return;
        }
        const clientId = InitSetting.find(s => s.key === SystemSetting.GiteeClientId)?.value;
        if (!clientId) {
            message.error({ content: '请联系管理员配置 Gitee ClientId' });
            return;
        }
        handleAuthRedirect(`https://gitee.com/oauth/authorize?client_id=${clientId}&redirect_uri=${location.origin}/auth/gitee&response_type=code`);
    };

    const handleCasdoorAuth = () => {
        let casdoorEndipoint = InitSetting.find(s => s.key === SystemSetting.CasdoorEndipoint)?.value as string;
        if (!casdoorEndipoint) {
            message.error({ content: '请联系管理员配置 Casdoor Endipoint' });
            return;
        }
        const casdoorClientId = InitSetting.find(s => s.key === SystemSetting.CasdoorClientId)?.value;
        if (!casdoorClientId) {
            message.error({ content: '请联系管理员配置 Casdoor ClientId' });
            return;
        }
        if (casdoorEndipoint.endsWith('/')) {
            casdoorEndipoint = casdoorEndipoint.slice(0, -1);
        }
        handleAuthRedirect(`${casdoorEndipoint}/login/oauth/authorize?client_id=${casdoorClientId}&redirect_uri=${location.origin}/auth/casdoor&response_type=code&scope=open email profile`);
    };

    return (
        <GridShowcase>
            <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', width: '100%', height: '100%' }}>
                <div style={{ textAlign: 'center', marginBottom: '20px' }}>
                    <span style={{ fontSize: '28px', fontWeight: 'bold' }}>TokenAI 登录系统</span>
                </div>
                <Input
                    value={user}
                    onChange={(e) => setUser(e.target.value)}
                    size='large'
                    placeholder="请输入账号"
                    style={{ marginBottom: '20px', width: '380px' }}
                />
                <Input.Password
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    size='large'
                    placeholder="请输入密码"
                    onPressEnter={handleLogin}
                    iconRender={visible => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
                    style={{ marginBottom: '20px', width: '380px' }}
                />
                <Button
                    loading={loading}
                    onClick={handleLogin}
                    size='large'
                    type="primary"
                    block
                    style={{ marginBottom: '20px', width: '380px' }}
                >
                    登录
                </Button>
                <FunctionTools>
                    <span onClick={() => window.location.href = '/register'}>注册账号</span>
                </FunctionTools>
                <Divider>第三方登录</Divider>
                <div style={{ display: 'flex', justifyContent: 'center', gap: '20px', marginTop: '20px' }}>
                    <Tooltip title="Github 登录">
                        <Button onClick={handleGithub} size='large' icon={<GithubOutlined />} />
                    </Tooltip>
                    <Tooltip title="Gitee 登录">
                        <Button onClick={handlerGitee} size='large' icon={<Gitee />} />
                    </Tooltip>
                    {enableCasdoorAuth && (
                        <Tooltip title="Casdoor 登录">
                            <Button onClick={handleCasdoorAuth} size='large' icon={<Casdoor />} />
                        </Tooltip>
                    )}
                </div>
            </div>
        </GridShowcase>
    );
});
export default Login;

import { useEffect, useState } from "react";
import { message, Button } from "antd";
import { getCasdoorToken, getGiteeToken, getGithubToken } from "../../services/AuthorizeService";
import { useNavigate, useLocation } from "react-router-dom";
import { Logo } from "@lobehub/ui";

export default function Auth() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const params = new URLSearchParams(window.location.search);
    const code = params.get('code');
    const location = useLocation();

    const handleTokenResponse = (res: any) => {
        if (res.success) {
            localStorage.setItem('token', res.data.token);
            localStorage.setItem('role', res.data.role);
            const redirect_uri = localStorage.getItem('redirect_uri');
            if (redirect_uri) {
                window.location.href = redirect_uri + '?token=' + res.data.token;
                localStorage.removeItem('redirect_uri');
                return;
            }
            message.success({
                content: "请记住默认密码为Aa123456",
                duration: 5
            });
            setTimeout(() => {
                navigate('/panel');
            }, 800);
        } else {
            message.error({ content: res.message });
            setLoading(false);
        }
    };

    const handleError = (error: any) => {
        message.error({ content: error.message });
        setLoading(false);
    };

    const fetchToken = (fetchFunction: Function, code: string, redirectUri?: string) => {
        setLoading(true);
        fetchFunction(code, redirectUri)
            .then(handleTokenResponse)
            .catch(handleError)
            .finally(() => setLoading(false));
    };

    useEffect(() => {
        if (code) {
            switch (location.pathname) {
                case "/auth":
                    fetchToken(getGithubToken, code);
                    break;
                case "/auth/gitee":
                    fetchToken(getGiteeToken, code, window.location.origin + '/auth/gitee');
                    break;
                case "/auth/casdoor":
                    fetchToken(getCasdoorToken, code);
                    break;
                default:
                    break;
            }
        }
    }, [code, location.pathname]);

    return (
        <div style={{
            height: '100vh',
            width: '100vw',
            display: 'flex',
            flexDirection: 'column',
            justifyContent: 'center',
            alignItems: 'center',
            color: 'white',
            textAlign: 'center',
            backgroundColor: '#282c34',
            padding: '20px',
        }}>
            <Logo size={64} extra="Thor" />
            <h2 style={{
                marginBottom: '20px',
                fontSize: '2rem',
                fontWeight: 'bold',
            }}>第三方登录授权</h2>
            <h3 style={{
                marginBottom: '40px',
                fontSize: '1.5rem',
            }}>{loading ? "正在登录中..." : "登录完成"}</h3>
            <Button style={{
                fontSize: '1.25rem',
                color: 'white',
                background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
                border: 'none',
                padding: '10px 20px',
                borderRadius: '5px',
            }} onClick={() => {
                window.location.href = '/login'
            }}>
                返回登陆
            </Button>
        </div>
    );
}
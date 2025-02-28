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
            background: 'linear-gradient(135deg, #1a1d23 0%, #282c34 100%)',
            padding: '20px',
            fontFamily: '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif',
        }}>
            <div style={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                padding: '40px',
                borderRadius: '16px',
                backdropFilter: 'blur(10px)',
                backgroundColor: 'rgba(40, 44, 52, 0.7)',
                boxShadow: '0 10px 30px rgba(0, 0, 0, 0.3)',
                maxWidth: '500px',
                width: '90%',
            }}>
                <Logo size={80} extra="Thor" />
                <h2 style={{
                    marginTop: '24px',
                    marginBottom: '16px',
                    fontSize: '2rem',
                    fontWeight: 'bold',
                    background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
                    WebkitBackgroundClip: 'text',
                    WebkitTextFillColor: 'transparent',
                }}>第三方登录授权</h2>
                <div style={{
                    marginBottom: '32px',
                    fontSize: '1.5rem',
                    display: 'flex',
                    alignItems: 'center',
                    gap: '10px',
                }}>
                    {loading ? (
                        <>
                            <span className="loading-dot" style={{
                                display: 'inline-block',
                                width: '10px',
                                height: '10px',
                                borderRadius: '50%',
                                backgroundColor: '#FE6B8B',
                                animation: 'pulse 1.5s infinite ease-in-out',
                            }}></span>
                            <span>正在登录中...</span>
                        </>
                    ) : (
                        <span>登录完成</span>
                    )}
                </div>
                <Button style={{
                    fontSize: '1.25rem',
                    color: 'white',
                    background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
                    border: 'none',
                    padding: '10px 30px',
                    borderRadius: '8px',
                    height: 'auto',
                    boxShadow: '0 4px 12px rgba(254, 107, 139, 0.5)',
                    transition: 'all 0.3s ease',
                }} 
                className="login-button"
                onClick={() => {
                    window.location.href = '/login'
                }}>
                    返回登陆
                </Button>
            </div>
            <style>{`
                @keyframes pulse {
                    0%, 100% { transform: scale(1); opacity: 1; }
                    50% { transform: scale(1.5); opacity: 0.7; }
                }
                
                .login-button:hover {
                    transform: translateY(-2px);
                    box-shadow: 0 6px 16px rgba(254, 107, 139, 0.6);
                }
            `}</style>
        </div>
    );
}
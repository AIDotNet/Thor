import { useEffect, useState } from "react";
import { message, Button, Spin } from "antd";
import { getCasdoorToken, getGiteeToken, getGithubToken } from "../../services/AuthorizeService";
import { useNavigate, useLocation } from "react-router-dom";
import { Logo } from "@lobehub/ui";
import { CheckCircleFilled, LoadingOutlined } from '@ant-design/icons';

export default function Auth() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [isSuccess, setIsSuccess] = useState(false);
    const params = new URLSearchParams(window.location.search);
    const code = params.get('code');
    const location = useLocation();

    const handleTokenResponse = (res: any) => {
        if (res.success) {
            setIsSuccess(true);
            localStorage.setItem('token', res.data.token);
            localStorage.setItem('role', res.data.role);
            let redirect_uri = localStorage.getItem('redirect_uri');
            if (redirect_uri) {
                if (redirect_uri.includes('?')) {
                    window.location.href = redirect_uri + '&token=' + res.data.token;
                } else {
                    window.location.href = redirect_uri + '?token=' + res.data.token;
                }
                localStorage.removeItem('redirect_uri');
                return;
            }
            setTimeout(() => {
                navigate('/panel');
            }, 800);
        } else {
            message.error({ 
                content: res.message, 
                style: { borderRadius: '8px', boxShadow: '0 4px 12px rgba(0, 0, 0, 0.15)' } 
            });
            setLoading(false);
        }
    };

    const handleError = (error: any) => {
        message.error({ 
            content: error.message, 
            style: { borderRadius: '8px', boxShadow: '0 4px 12px rgba(0, 0, 0, 0.15)' } 
        });
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

    const antIcon = <LoadingOutlined style={{ fontSize: 24, color: '#FE6B8B' }} spin />;

    return (
        <div className="auth-container">
            <div className="auth-background">
                <div className="auth-bubble"></div>
                <div className="auth-bubble"></div>
                <div className="auth-bubble"></div>
            </div>
            <div className="auth-card">
                <div className="logo-container">
                    <Logo size={80} extra="Thor" />
                </div>
                <h2 className="auth-title">第三方登录授权</h2>
                <div className="auth-status">
                    {loading ? (
                        <div className="loading-container">
                            <Spin indicator={antIcon} />
                            <span className="loading-text">正在处理授权请求...</span>
                        </div>
                    ) : isSuccess ? (
                        <div className="success-container">
                            <CheckCircleFilled style={{ fontSize: 28, color: '#52c41a', marginRight: 10 }} />
                            <span className="success-text">授权成功</span>
                        </div>
                    ) : (
                        <span className="waiting-text">等待处理授权...</span>
                    )}
                </div>
                <Button 
                    className="auth-button"
                    onClick={() => {
                        window.location.href = '/login'
                    }}>
                    返回登录
                </Button>
                <div className="auth-footer">
                    <span>安全可靠的第三方授权服务</span>
                </div>
            </div>
            <style>{`
                .auth-container {
                    height: 100vh;
                    width: 100vw;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    background: radial-gradient(circle at 10% 20%, rgb(26, 29, 35) 0%, rgb(54, 57, 70) 90%);
                    position: relative;
                    overflow: hidden;
                    padding: 20px;
                    font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
                }
                
                .auth-background {
                    position: absolute;
                    top: 0;
                    left: 0;
                    right: 0;
                    bottom: 0;
                    z-index: 0;
                }
                
                .auth-bubble {
                    position: absolute;
                    border-radius: 50%;
                    background: linear-gradient(45deg, rgba(254, 107, 139, 0.2) 30%, rgba(255, 142, 83, 0.2) 90%);
                    filter: blur(30px);
                    animation: float 15s infinite ease-in-out;
                }
                
                .auth-bubble:nth-child(1) {
                    width: 300px;
                    height: 300px;
                    top: -50px;
                    left: 10%;
                    animation-delay: 0s;
                }
                
                .auth-bubble:nth-child(2) {
                    width: 250px;
                    height: 250px;
                    bottom: 10%;
                    right: 15%;
                    animation-delay: 5s;
                }
                
                .auth-bubble:nth-child(3) {
                    width: 200px;
                    height: 200px;
                    bottom: 30%;
                    left: 20%;
                    animation-delay: 10s;
                }
                
                @keyframes float {
                    0%, 100% { transform: translateY(0) translateX(0); }
                    25% { transform: translateY(-20px) translateX(10px); }
                    50% { transform: translateY(0) translateX(20px); }
                    75% { transform: translateY(20px) translateX(10px); }
                }
                
                .auth-card {
                    position: relative;
                    z-index: 1;
                    display: flex;
                    flex-direction: column;
                    align-items: center;
                    padding: 40px;
                    border-radius: 16px;
                    background: rgba(35, 39, 47, 0.8);
                    backdrop-filter: blur(20px);
                    box-shadow: 0 20px 40px rgba(0, 0, 0, 0.3), 
                                0 5px 10px rgba(0, 0, 0, 0.2),
                                inset 0 0 0 1px rgba(255, 255, 255, 0.1);
                    max-width: 400px;
                    width: 90%;
                    transition: all 0.3s ease;
                }
                
                .auth-card:hover {
                    transform: translateY(-5px);
                    box-shadow: 0 25px 50px rgba(0, 0, 0, 0.4), 
                                0 10px 15px rgba(0, 0, 0, 0.25),
                                inset 0 0 0 1px rgba(255, 255, 255, 0.12);
                }
                
                .logo-container {
                    margin-bottom: 10px;
                    padding: 15px;
                    border-radius: 50%;
                    background: rgba(255, 255, 255, 0.05);
                    box-shadow: 0 8px 16px rgba(0, 0, 0, 0.15);
                    backdrop-filter: blur(5px);
                    animation: pulse 3s infinite ease-in-out;
                }
                
                @keyframes pulse {
                    0%, 100% { box-shadow: 0 8px 16px rgba(0, 0, 0, 0.15), 0 0 0 0 rgba(254, 107, 139, 0); }
                    50% { box-shadow: 0 8px 16px rgba(0, 0, 0, 0.15), 0 0 0 10px rgba(254, 107, 139, 0); }
                }
                
                .auth-title {
                    margin-top: 24px;
                    margin-bottom: 24px;
                    font-size: 2rem;
                    font-weight: bold;
                    background: linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%);
                    -webkit-background-clip: text;
                    -webkit-text-fill-color: transparent;
                    text-align: center;
                }
                
                .auth-status {
                    margin-bottom: 32px;
                    height: 60px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    color: white;
                }
                
                .loading-container, .success-container {
                    display: flex;
                    align-items: center;
                    gap: 10px;
                }
                
                .loading-text, .success-text, .waiting-text {
                    font-size: 1.25rem;
                }
                
                .auth-button {
                    font-size: 1.25rem;
                    color: white;
                    background: linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%);
                    border: none;
                    padding: 10px 30px;
                    border-radius: 8px;
                    height: auto;
                    box-shadow: 0 4px 12px rgba(254, 107, 139, 0.5);
                    transition: all 0.3s ease;
                }
                
                .auth-button:hover {
                    transform: translateY(-2px);
                    box-shadow: 0 8px 20px rgba(254, 107, 139, 0.6);
                }
                
                .auth-button:active {
                    transform: translateY(1px);
                    box-shadow: 0 2px 8px rgba(254, 107, 139, 0.5);
                }
                
                .auth-footer {
                    margin-top: 24px;
                    font-size: 0.9rem;
                    color: rgba(255, 255, 255, 0.6);
                    text-align: center;
                }
                
                @media (max-width: 480px) {
                    .auth-card {
                        padding: 30px 20px;
                    }
                    
                    .auth-title {
                        font-size: 1.5rem;
                    }
                    
                    .loading-text, .success-text, .waiting-text {
                        font-size: 1rem;
                    }
                    
                    .auth-button {
                        font-size: 1rem;
                        padding: 8px 24px;
                    }
                }
            `}</style>
        </div>
    );
}
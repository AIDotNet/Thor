import { useEffect, useState } from "react";
import { message, Button } from "antd";
import { getGiteeToken, getGithubToken } from "../../services/AuthorizeService";
import { useNavigate, useLocation } from "react-router-dom";
import { Logo } from "@lobehub/ui";

export default function Auth() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const params = new URLSearchParams(window.location.search);
    const code = params.get('code');
    const location = useLocation();
    console.log(location.pathname, code);

    function GithubToken(code: string) {
        setLoading(true);
        getGithubToken(code)
            .then((res) => {
                if (res.success) {
                    localStorage.setItem('token', res.data.token);
                    localStorage.setItem('role', res.data.role);

                    message.success({
                        content: "请记住默认密码为Aa123456",
                        duration: 5
                    });

                    setTimeout(() => {
                        navigate('/panel');
                    }, 800);
                } else {
                    message.error({
                        content: res.message
                    } as any);
                    setLoading(false);
                }
            }).catch((error) => {
                message.error({
                    content: error.message
                });
            }).finally(() => {
                setLoading(false);
            });
    }

    function GiteeToken(code: string) {
        setLoading(true);
        const redirectUri = window.location.origin + '/auth/gitee';

        getGiteeToken(code, redirectUri)
            .then((res) => {
                if (res.success) {
                    localStorage.setItem('token', res.data.token);
                    localStorage.setItem('role', res.data.role);

                    message.success({
                        content: "请记住默认密码为Aa123456",
                        duration: 5
                    });

                    setTimeout(() => {
                        navigate('/panel');
                    }, 800);
                } else {
                    message.error({
                        content: res.message
                    } as any);
                    setLoading(false);
                }
            }).catch((error) => {
                message.error({
                    content: error.message
                });
            }).finally(() => {
                setLoading(false);
            });
    }

    useEffect(() => {
        if (code) {
            if (location.pathname === "/auth") {
                GithubToken(code);
            } else if (location.pathname === "/auth/gitee") {
                GiteeToken(code);
            }
        }
    }, []);

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
        }}>
            <Logo size={64} extra="Thor"/>
            <h2 style={{
                marginBottom: '20px',
                fontSize: '2rem',
                fontWeight: 'bold',
            }}>第三方登录授权</h2>
            <div>
                <h3 style={{
                    marginBottom: '40px',
                    fontSize: '1.5rem',
                }}>{loading ? "正在登录中..." : "登录完成"}</h3>
            </div>
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
    )
}


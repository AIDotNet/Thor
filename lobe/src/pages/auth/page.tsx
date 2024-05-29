import { useLocation } from "react-router-dom";
import { useEffect, useState } from "react";
import { message, Button, } from "antd";
import { getGithubToken } from "../../services/AuthorizeService";
import { useNavigate } from "react-router-dom";

export default function Auth() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    // 获取code参数
    const params = new URLSearchParams(window.location.search);
    const code = params.get('code');
    // 获取当前页面路径
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
                    }, 1000);
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

    // 首次进入调用
    useEffect(() => {
        if (code) {
            if (location.pathname === "/auth") {
                GithubToken(code);
            } else {
                message.error({
                    content: '未知的第三方登录'
                });
            }
        }
    }, [])
    return (<div style={{
        background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
        height: '100vh',
        width: '100vw',
        position: 'absolute',
        top: 0,
        left: 0,
        paddingTop: '100px',

    }}>
        <h2 style={{
            textAlign: 'center',
            marginTop: '100px',
            margin: 'auto',
            // 好看的背景
        }}>第三方登录授权</h2>
        <div>
            <h3 style={{
                textAlign: 'center',
                marginTop: '100px',
            }}>{!loading ? "正在登录中..." : "登录完成"}</h3>
        </div>

        <Button style={{
            margin: 'auto',
            marginTop: '100px',
            display: 'block',
            fontSize: '20px',
            color: 'white',
            background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',

        }} onClick={() => {
            window.location.href = '/login'
        }} >
            返回登陆
        </Button>
    </div>)
}


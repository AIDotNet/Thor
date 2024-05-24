import { Avatar, Form, Button, Notification, Divider } from '@douyinfe/semi-ui';
import styles from './index.module.scss';
import { useState } from 'react';
import { login } from '../../services/AuthorizeService';
import { useNavigate } from 'react-router-dom';
import { IconGithubLogo } from '@douyinfe/semi-icons'
import { InitSetting, SystemSetting } from '../../services/SettingService';

export default function Login() {
    // 接收参数 redirect_uri
    const params = new URLSearchParams(location.search);

    const redirect_uri = params.get('redirect_uri');

    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();
    function handleLogin(value: any) {
        setLoading(true);
        login({
            account: value.account, pass: value.password
        })
            .then((res) => {
                if (res.success) {
                    localStorage.setItem('token', res.data.token);
                    localStorage.setItem('role', res.data.role);
                    Notification.success({
                        title: '登录成功',
                        content: '即将跳转到首页'
                    } as any);
                    
                    if(redirect_uri && redirect_uri.startsWith('http')) {
                        const url = new URL(redirect_uri);
                        url.searchParams.append('token', res.data.token);
                        window.location.href = url.toString();
                        return;
                    }


                    setTimeout(() => {
                        navigate('/panel');
                    }, 1000);
                } else {
                    Notification.error({
                        title: '登录失败',
                        content: res.message
                    } as any);
                    setLoading(false);
                }
            })
            .catch(() => {
                setLoading(false);
            });
    }

    function handleGithub() {

        const clientId = InitSetting.find(s => s.key === SystemSetting.GithubClientId)?.value;

        if(!clientId){
            Notification.error({
                title: '错误',
                content: '未配置 Github ClientId'
            });
            return;
        }

        // 跳转 Github 授权页面
        window.location.href = `https://github.com/login/oauth/authorize?client_id=${clientId}&redirect_uri=${location.origin}/auth&response_type=code`;
    }

    return (
        <div className={styles.main}>
            <div className={styles.login}>
                <div className={styles.component66}>
                    <Avatar
                        src='logo.png'
                        className={styles.logo}
                    />
                    <div className={styles.header}>
                        <p className={styles.text}>
                            <span className={styles.text}>登录</span>
                            <span className={styles.text1}> AIDotNet API </span>
                            <span className={styles.text2}>账户</span>
                        </p>
                    </div>
                </div>
                <div className={styles.form}>
                    <Form className={styles.inputs} onSubmit={handleLogin}>
                        <Form.Input
                            label={{ text: "用户名" }}
                            field="account"
                            size='large'
                            placeholder="输入用户名"
                            style={{ width: "100%" }}
                            fieldStyle={{ alignSelf: "stretch", padding: 0 }}
                        />
                        <Form.Input
                            label={{ text: "密码" }}
                            field="password"
                            size='large'
                            type='password'
                            placeholder="输入密码"
                            style={{ width: "100%" }}
                            fieldStyle={{ alignSelf: "stretch", padding: 0 }}
                        />
                        <Button loading={loading} htmlType='submit' theme="solid" className={styles.button}>
                            登录
                        </Button>
                    </Form>
                    <Divider></Divider>
                    <div className={styles.footer}>
                        <a onClick={() => {
                            navigate('/register');
                        }} className={styles.link}>注册</a>
                        <a onClick={() => {
                            navigate('/forget');
                        }} className={styles.link}>忘记密码</a>
                        <Divider layout="vertical" margin='12px' />
                        <Button size='large' className={styles.github} theme='borderless' icon={<IconGithubLogo size='large' />} onClick={() => handleGithub()} >
                        </Button>
                    </div>
                </div>
            </div>
        </div>
    )
}
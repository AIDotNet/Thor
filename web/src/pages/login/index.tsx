import { Avatar, Form, Button, Notification, Divider } from '@douyinfe/semi-ui';
import styles from './index.module.scss';
import { useState } from 'react';
import { login } from '../../services/AuthorizeService';
import { useNavigate } from 'react-router-dom';

export default function Login() {
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
                    </div>
                </div>
            </div>
        </div>
    )
}
import { Avatar, Form, Button, Notification, Divider } from '@douyinfe/semi-ui';
import styles from './index.module.scss';
import { useState } from 'react';
import { login } from '../../services/AuthorizeService';
import { useNavigate } from 'react-router-dom';
import { create } from '../../services/UserService';

export default function Register() {
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();
    function handleLogin(value: any) {
        setLoading(true);

        // 判断两次密码是否一致
        if (value.password !== value.password1) {
            Notification.error({
                title: '注册失败',
                content: '两次密码不一致'
            } as any);
            setLoading(false);
            return;
        }

        create(value)
            .then((res) => {
                if (res.success) {
                    Notification.success({
                        title: '注册成功',
                        content: '正在自动登录'
                    } as any);

                    // 等待200ms
                    setTimeout(() => {
                        login(value.userName, value.password)
                            .then((res) => {
                                if (res.success) {
                                    localStorage.setItem('token', res.data);
                                    Notification.success({
                                        title: '登录成功',
                                        content: '即将跳转到首页'
                                    } as any);
                                    setTimeout(() => {
                                        navigate('/');
                                    }, 1000);
                                } else {
                                    Notification.error({
                                        title: '登录失败',
                                        content: res.message
                                    } as any);
                                    setLoading(false);
                                }
                            })
                            .catch((err) => {
                                setLoading(false);
                            });
                    }, 200);
                } else {
                    Notification.error({
                        title: '注册失败',
                        content: res.message
                    } as any);
                    setLoading(false);
                }
            })
            .catch((err) => {
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
                            <span className={styles.text}>注册</span>
                            <span className={styles.text1}> AIDotNet API </span>
                            <span className={styles.text2}>账户</span>
                        </p>
                    </div>
                </div>
                <div className={styles.form}>
                    <Form className={styles.inputs} onSubmit={handleLogin}>
                        <Form.Input
                            label={{ text: "用户名" }}
                            field="userName"
                            size='large'
                            required
                            placeholder="输入用户名"
                            style={{ width: "100%" }}
                            fieldStyle={{ alignSelf: "stretch", padding: 0 }}
                        />
                        <Form.Input
                            label={{ text: "邮箱" }}
                            field="email"
                            size='large'
                            placeholder="输入邮箱"
                            required
                            style={{ width: "100%" }}
                            fieldStyle={{ alignSelf: "stretch", padding: 0 }}
                        />
                        <Form.Input
                            label={{ text: "密码" }}
                            field="password"
                            size='large'
                            type='password'
                            required
                            placeholder="输入密码"
                            style={{ width: "100%" }}
                            fieldStyle={{ alignSelf: "stretch", padding: 0 }}
                        />
                        <Form.Input
                            label={{ text: "密码" }}
                            field="password1"
                            size='large'
                            required
                            type='password1'
                            placeholder="输入密码"
                            style={{ width: "100%" }}
                            fieldStyle={{ alignSelf: "stretch", padding: 0 }}
                        />
                        <Button loading={loading} htmlType='submit' theme="solid" className={styles.button}>
                            注册
                        </Button>
                    </Form>
                    <Divider></Divider>
                    <div className={styles.footer}>
                        <a onClick={() => {
                            navigate('/login');
                        }} className={styles.link}>登录</a>
                        <a onClick={() => {
                            navigate('/forget');
                        }} className={styles.link}>忘记密码</a>
                    </div>
                </div>
            </div>
        </div>
    )
}
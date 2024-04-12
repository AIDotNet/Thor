import { useEffect, useState } from 'react';
import { Form, Input, Button, Avatar } from '@douyinfe/semi-ui';
import { info } from '../../services/UserService';

export default function ProfileForm() {
    const [user, setUser] = useState({ 
        email: '',
        avatar: ''
     });

    function loadUser() {
        info()
            .then((res) => {
                setUser({
                    email: res.data.email,
                    avatar: res.data.avatar
                });
                console.log(user, res.data);
                
            });
    }

    useEffect(() => {
        loadUser();
    }, []);

    const handleSubmit = () => {
        
    };

    return (
        <div style={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center'
        }}>
            <Avatar style={{
                marginTop: 8
            }}  src={user.avatar??"/logo.png"}/>
            <Input placeholder={'输入您的头像地址'} style={{
                marginTop: 8
            }}  value={user.avatar} onChange={value => setUser({
                ...user,
                avatar: value
            })} >
            </Input>
            <Input placeholder={'输入您的新的邮箱地址'} style={{
                marginTop: 8
            }} value={user.email} onChange={value => setUser({
                ...user,
                email: value
            })} >
            </Input>
            <Button style={{
                marginTop: 8
            }} block  type="primary" htmlType="submit">
                保存修改
            </Button>
        </div>
    );
}
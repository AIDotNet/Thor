import { useEffect, useState } from 'react';
import { info, update, updatePassword } from '../../services/UserService';
import { message, Avatar, Input, Button, Tabs } from 'antd';
import Pay from '../../components/pay';

export default function ProfileForm() {
  const [user, setUser] = useState({} as any);
  const [password, setPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');

  function loadUser() {
    info()
      .then((res) => {
        setUser(res.data);
      });
  }

  useEffect(() => {
    loadUser();
  }, []);

  /**
   * 提交修改
   */
  const handleSubmit = () => {
    update(user)
      .then((res) => {
        res.success ? message.success({
          content: '修改成功',
        }) : message.error({
          content: '修改失败',
        });
      });
  };

  /**
   * 修改密码
   */
  function onUpdatePassword() {
    updatePassword({
      oldPassword: password,
      newPassword: newPassword
    })
      .then((res) => {
        res.success ? message.success({
          content: '修改成功',
        }) : message.error({
          content: res.message
        });
      });
  }

  return (
    <div style={{
      margin: 20,
      height: '100%',
      width: '100%',
    }}>
      <Tabs
        style={{
          width: '100%',
        }}
        tabPosition="top"
        items={[
          {
            key: '1',
            label: '账号余额',
            children: <div style={{ padding: '0 24px' }}>
              <Pay user={user} />
            </div>
          },
          {
            key: '2',
            label: '修改个人信息',
            children: <div style={{
              padding: '0 24px',
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center'
            }}>
              <Avatar style={{
                marginTop: 8
              }} src={(!user.avatar || user.avatar === '') ? "/logo.png" : user.avatar} />
              <Input placeholder={'输入您的头像地址'} style={{
                marginTop: 8
              }} value={user.avatar} onChange={value => setUser({
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
              }} onClick={() => handleSubmit()} block type="primary" htmlType="submit">
                保存修改
              </Button>
            </div>
          },
          {
            key: '3',
            label: '修改登录密码',
            children: <div style={{
              padding: '0 24px',
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center'
            }}>
              <Input value={password}
                type='password'
                onChange={(value) => {
                  setPassword(value.target.value);
                }}
                placeholder={'输入您原有密码'} style={{
                  marginTop: 8
                }} >
              </Input>
              <Input value={newPassword}
                type='password'
                onChange={(value) => {
                  setNewPassword(value.target.value);
                }}
                placeholder={'输入您的新密码'} style={{
                  marginTop: 8
                }} >
              </Input>
              <Button style={{
                marginTop: 8
              }} onClick={() => onUpdatePassword()} block type="primary" htmlType="submit">
                保存修改
              </Button>
            </div>
          }
        ]}
        type='line'>
      </Tabs>
    </div>
  );
}
import { useEffect, useState } from 'react';
import { info,  updatePassword } from '../../services/UserService';
import { message,  Input, Button, Tabs } from 'antd';
import Pay from '../../components/pay';
import UserInfo from '../../components/User/UserInfo';
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
            label: '用户信息',
            children: <div style={{ padding: '0 24px' }}>
              <UserInfo user={user} />
            </div>
          },
          {
            key: '2',
            label: '充值余额',
            children: <div style={{ padding: '0 24px' }}>
              <Pay user={user} />
            </div>
          },
          {
            key: '3',
            label: '修改密码',
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
              <div style={{
                marginTop: 8
              }}>
                <Button style={{
                  marginTop: 8
                }} onClick={() => onUpdatePassword()} block type="primary" htmlType="submit">
                  保存修改
                </Button>
              </div>
            </div>
          }
        ]}
        type='line'>
      </Tabs>
    </div>
  );
}
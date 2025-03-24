import { useEffect, useState } from 'react';
import { info } from '../../services/UserService';
import { Tabs } from 'antd';
import Pay from '../../components/pay';
import UserInfo from '../../components/User/UserInfo';
export default function ProfileForm() {
  const [user, setUser] = useState({} as any);

  function loadUser() {
    info()
      .then((res) => {
        setUser(res.data);
      });
  }

  useEffect(() => {
    loadUser();
  }, []);

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
              <UserInfo 
                onUpdate={loadUser}
                user={user} />
            </div>
          },
          {
            key: '2',
            label: '充值余额',
            children: <div style={{ padding: '0 24px' }}>
              <Pay user={user} />
            </div>
          }
        ]}
        type='line'>
      </Tabs>
    </div>
  );
}
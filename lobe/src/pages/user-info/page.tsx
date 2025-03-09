import { useEffect, useState } from 'react';
import { Card, message } from 'antd';
import { info } from '../../services/UserService';
import UserInfo from '../../components/User/UserInfo';

export default function UserInfoPage() {
  const [user, setUser] = useState({} as any);
  const [loading, setLoading] = useState(true);

  function loadUser() {
    setLoading(true);
    info()
      .then((res) => {
        setUser(res.data);
      })
      .catch((err) => {
        message.error('获取用户信息失败');
        console.error(err);
      })
      .finally(() => {
        setLoading(false);
      });
  }

  useEffect(() => {
    loadUser();
  }, []);

  return (
    <div style={{ maxWidth: 800, margin: '20px auto' }}>
      <Card
        title="用户信息"
        loading={loading}
      >
        <UserInfo user={user} onUpdate={loadUser} />
      </Card>
    </div>
  );
} 
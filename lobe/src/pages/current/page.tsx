import { useEffect, useState } from 'react';
import { info } from '../../services/UserService';
import { ConfigProvider, Card, Tabs, Empty, Spin, theme } from 'antd';
import Pay from '../../components/pay';
import UserInfo from '../../components/User/UserInfo';
import { useTranslation } from 'react-i18next';
import { UserOutlined, WalletOutlined } from '@ant-design/icons';

export default function ProfileForm() {
  const { t } = useTranslation();
  const { token } = theme.useToken();
  const [user, setUser] = useState({} as any);
  const [loading, setLoading] = useState(true);

  function loadUser() {
    setLoading(true);
    info()
      .then((res) => {
        setUser(res.data);
      })
      .finally(() => {
        setLoading(false);
      });
  }

  useEffect(() => {
    loadUser();
  }, []);

  // Modern card styles with Ant Design token system
  const containerStyle = {
    margin: token.marginLG,
    height: '100%',
    width: '100%',
    maxWidth: '1200px',
    padding: token.paddingMD,
  };

  const cardStyle = {
    borderRadius: token.borderRadiusLG,
    boxShadow: `0 4px 20px ${token.colorBgElevated}`,
    overflow: 'hidden',
  };

  const tabBarStyle = {
    marginBottom: token.marginMD,
    display: 'flex',
    justifyContent: 'center',
  };

  return (
    <div style={containerStyle}>
      <ConfigProvider
        theme={{
          components: {
            Tabs: {
              cardGutter: token.marginXS,
            },
          },
        }}
      >
        <Card 
          style={cardStyle}
          bodyStyle={{ padding: 0 }}
        >
          {loading ? (
            <div style={{ padding: token.paddingLG, textAlign: 'center' }}>
              <Spin size="large" />
            </div>
          ) : Object.keys(user).length === 0 ? (
            <Empty 
              description={t('common.noData')}
              style={{ padding: token.paddingLG }}
            />
          ) : (
            <Tabs
              style={{ width: '100%' }}
              tabBarStyle={tabBarStyle}
              tabPosition="top"
              items={[
                {
                  key: '1',
                  label: (
                    <span>
                      <UserOutlined style={{ marginRight: token.marginXS }} />
                      {t('userProfile.userInfo')}
                    </span>
                  ),
                  children: (
                    <div style={{ padding: `0 ${token.paddingLG}px` }}>
                      <UserInfo 
                        onUpdate={loadUser}
                        user={user} 
                      />
                    </div>
                  ),
                },
                {
                  key: '2',
                  label: (
                    <span>
                      <WalletOutlined style={{ marginRight: token.marginXS }} />
                      {t('userProfile.balance')}
                    </span>
                  ),
                  children: (
                    <div style={{ padding: `0 ${token.paddingLG}px` }}>
                      <Pay user={user} />
                    </div>
                  ),
                }
              ]}
              type='card'
            />
          )}
        </Card>
      </ConfigProvider>
    </div>
  );
}
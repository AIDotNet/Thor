
import { memo } from 'react';
import { Flexbox } from 'react-layout-kit';
import UserAvatar from '../UserAvatar';
import { Button } from 'antd';
import { Tag } from '@lobehub/ui';
import { Github } from '@lobehub/icons';
import { config } from '../../../config';
import { useActiveUser } from '../../../hooks/useActiveTabKey';
import { renderQuota } from '../../../utils/render';


const PanelContent = memo<{ closePopover: () => void }>(({ }) => {
  const user = useActiveUser();


  return (
    <Flexbox gap={2} style={{ minWidth: 200 }}>
      <Flexbox
        align={'center'}
        horizontal
        justify={'space-between'}
        style={{ padding: '6px 6px 6px 6px' }}
      >
        <Flexbox align={'center'} flex={'none'} gap={6} horizontal>
          <UserAvatar user={user} size={64} clickable />
          <div>
            <span>
              {user?.userName || "游客"}
            </span>
            <div>
              <span>账号余额：</span>
              <Tag color='red'>{renderQuota(user?.residualCredit ?? 0, 2)}</Tag>
            </div>
          </div>
        </Flexbox>
      </Flexbox>
      <Flexbox
        align={'center'}
        horizontal
        justify={'space-between'}
        style={{ padding: '6px 6px 6px 6px' }}
      >
        <Button block onClick={() => {
          localStorage.removeItem('token');
          window.location.href = '/login';
        }}>
          退出登录
        </Button>
      </Flexbox>
    </Flexbox>
  );
});

export default PanelContent;

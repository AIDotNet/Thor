import { ActionIcon, Tooltip } from '@lobehub/ui';
import { Book, Github } from 'lucide-react';
import { memo } from 'react';
import { Link } from 'react-router-dom';


const BottomActions = memo(() => {

  return (
    <div style={{
      // 排底部
      position: 'fixed',
      bottom: 0,
      display: 'flex',
    }}>
      <Tooltip title='Thor开源地址'>

        <Link style={{
          marginLeft: '50%',
        }} to={'https://github.com/AIDotNet/Thor'} target={'_blank'}>
          <ActionIcon icon={Github} placement={'right'} title={'GitHub'} />
        </Link>
      </Tooltip>
      <Tooltip title='Thor文档'>
        <Link style={{
          marginLeft: '50%',
        }} to={'/doc'} target={'_blank'}>
          <ActionIcon icon={Book} placement={'right'} title={'文档'} />
        </Link>
      </Tooltip>
    </div>
  );
});

export default BottomActions;

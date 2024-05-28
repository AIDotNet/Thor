import { ActionIcon } from '@lobehub/ui';
import { Book, Github } from 'lucide-react';
import { memo } from 'react';
import { Link } from 'react-router-dom';


const BottomActions = memo(() => {

  return (
    <>
      <Link to={'https://github.com/AIDotNet/AIDotNet.API'} target={'_blank'}>
        <ActionIcon icon={Github} placement={'right'} title={'GitHub'} />
      </Link>
      <Link to={'https://docs.token-ai.cn'} target={'_blank'}>
        <ActionIcon icon={Book} placement={'right'} title={'文档'} />
      </Link>
    </>
  );
});

export default BottomActions;

import { Icon } from '@lobehub/ui';
import { Loader2 } from 'lucide-react';
import { memo } from 'react';
import { Center, Flexbox } from 'react-layout-kit';


const FullscreenLoading = memo<{ title?: string }>(({ title }) => {
    return (
      <Flexbox height={'100%'} style={{ userSelect: 'none' }} width={'100%'}>
        <Center flex={1} gap={12} width={'100%'}>
          <span style={{
            fontSize: '24px',
            fontWeight: 'bold',
            fontFamily: 'Arial, sans-serif',
            userSelect: 'none',
            color: 'var(--leva-colors-highlight3)',
          }}>
            Thor 雷神托尔
          </span>
          <Center gap={16} horizontal>
            <Icon icon={Loader2} spin />
            {title}
          </Center>
        </Center>
      </Flexbox>
    );
  });
  
  export default FullscreenLoading;
  
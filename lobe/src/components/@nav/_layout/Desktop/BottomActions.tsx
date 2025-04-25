import { ActionIcon, Tooltip } from '@lobehub/ui';
import { Book, Github, HelpCircle } from 'lucide-react';
import { memo, useState } from 'react';
import { Link } from 'react-router-dom';

const iconStyle = {
  borderRadius: '50%',
  transition: 'transform 0.2s, background 0.2s',
};

const BottomActions = memo(() => {
  const [hoveredIcon, setHoveredIcon] = useState<string | null>(null);
  
  const getIconStyle = (iconName: string) => {
    return {
      ...iconStyle,
      transform: hoveredIcon === iconName ? 'scale(1.1)' : 'scale(1)',
    };
  };

  return (
    <div 
      style={{
        display: 'flex',
        justifyContent: 'space-evenly',
        alignItems: 'center',
        padding: '8px 0',
        width: '100%',
        marginBottom: '8px'
      }}
    >
      <Tooltip title='Thor开源地址' placement="top">
        <Link 
          to={'https://github.com/AIDotNet/Thor'} 
          target={'_blank'}
          style={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
          }}
        >
          <ActionIcon 
            icon={Github} 
            size={'large'}
            style={getIconStyle('github')}
            onMouseEnter={() => setHoveredIcon('github')}
            onMouseLeave={() => setHoveredIcon(null)}
          />
        </Link>
      </Tooltip>
      
      <Tooltip title='Thor文档' placement="top">
        <Link 
          to={'/doc'} 
          target={'_blank'}
          style={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
          }}
        >
          <ActionIcon 
            icon={Book} 
            size={'large'}
            style={getIconStyle('doc')}
            onMouseEnter={() => setHoveredIcon('doc')}
            onMouseLeave={() => setHoveredIcon(null)}
          />
        </Link>
      </Tooltip>
      
      <Tooltip title='帮助中心' placement="top">
        <Link 
          to={'/help'} 
          target={'_blank'}
          style={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
          }}
        >
          <ActionIcon 
            icon={HelpCircle} 
            size={'large'}
            style={getIconStyle('help')}
            onMouseEnter={() => setHoveredIcon('help')}
            onMouseLeave={() => setHoveredIcon(null)}
          />
        </Link>
      </Tooltip>
    </div>
  );
});

export default BottomActions;

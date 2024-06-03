import { ActionIcon } from '@lobehub/ui';
import { Tooltip } from 'antd';
import { LucideX } from 'lucide-react';
import { memo, useState } from 'react';
import { Flexbox } from 'react-layout-kit';
import UserPanel from '../../../User/UserPanel';
import UserAvatar from '../../../User/UserAvatar';
import { useActiveUser } from '../../../../hooks/useActiveTabKey';

const Avatar = memo(() => {
    const [hideSettingsMoveGuide] = useState(true);
    const user = useActiveUser();
    const content = (
        <UserPanel>
            <UserAvatar user={user} clickable />
        </UserPanel>
    );

    return hideSettingsMoveGuide ? (
        content
    ) : (
        <Tooltip
            color={'blue'}
            open
            placement={'right'}
            prefixCls={'guide'}
            title={
                <Flexbox align={'center'} gap={8} horizontal>
                    <ActionIcon
                        icon={LucideX}
                        onClick={() => {
                            
                        }}
                        role={'close-guide'}
                        size={'small'}
                        style={{ color: 'inherit' }}
                    />
                </Flexbox>
            }
        >
            {content}
        </Tooltip>
    );
});

Avatar.displayName = 'Avatar';

export default Avatar;

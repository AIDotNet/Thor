/* eslint-disable sort-keys-fix/sort-keys-fix , typescript-sort-keys/interface */
import { ActionIcon } from '@lobehub/ui';
import { AppWindowMac } from 'lucide-react';

import { memo } from 'react';
import { SidebarTabKey } from '../../../../store/global/initialState';
import { Link } from 'react-router-dom';

export interface TopActionProps {
    tab?: SidebarTabKey;
}

const TopActions = memo<TopActionProps>(({ tab }) => {

    const items = [
        {
            href: '/panel',
            icon: AppWindowMac,
            // @ts-ignore
            title: "面板",
            key: SidebarTabKey.Panel,
            onClick: () => {

            }
        }
    ];


    return (
        <>
            {items.map((item: any) => {
                return (
                    <Link
                        to={item.href}
                        onClick={(e) => {
                            e.preventDefault();
                            item.onClick();
                        }}
                    >
                        <ActionIcon
                            active={tab === item?.key}
                            icon={item.icon}
                            placement={'right'}
                            size="large"
                            title={item.title}
                        />
                    </Link>)
            })}
        </>
    );
});

export default TopActions;

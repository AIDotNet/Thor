/* eslint-disable sort-keys-fix/sort-keys-fix , typescript-sort-keys/interface */
'use client';

import { Icon, MobileTabBar, type MobileTabBarProps } from '@lobehub/ui';
import { createStyles } from 'antd-style';
import { AppWindowMac, MessageSquare, SquareFunction, Album, User } from 'lucide-react';
import { rgba } from 'polished';
import { useNavigate } from 'react-router-dom';
import { memo, useEffect, useMemo, useState } from 'react';

import { useActiveTabKey } from '../../../hooks/useActiveTabKey';
import { SidebarTabKey } from '../../../store/global/initialState';

const useStyles = createStyles(({ css, token }) => ({
    active: css`
    svg {
      fill: ${rgba(token.colorPrimary, 0.33)};
    }
  `,
    container: css`
    position: fixed;
    z-index: 100;
    right: 0;
    bottom: 0;
    left: 0;
  `,
}));

const Nav = memo(() => {
    const { styles } = useStyles();
    const activeKey = useActiveTabKey();
    const navigate = useNavigate();
    const items = [
        {
            icon: (active: any) => (
                <Icon className={active ? styles.active : undefined} icon={AppWindowMac} />
            ),
            key: SidebarTabKey.Panel,
            onClick: () => {
                navigate('/panel');
            },
            title: "面板",
        },
    ];
    return <MobileTabBar activeKey={activeKey} className={styles.container} items={items} />;
});

Nav.displayName = 'MobileNav';

export default Nav;

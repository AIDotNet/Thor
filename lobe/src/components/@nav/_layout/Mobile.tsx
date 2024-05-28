import { BarChart3, BarChart, KeyRound, ShipWheel, FileText, Code, User, CircleUserRound, Settings } from 'lucide-react';
import { Icon, MobileTabBar, MobileTabBarItemProps, type MobileTabBarProps } from '@lobehub/ui';
import { createStyles } from 'antd-style';
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
        <Icon className={active ? styles.active : undefined} icon={BarChart3} />
      ),
      title: "面板",
      key: SidebarTabKey.Panel,
      onClick: () => {
        navigate('/panel')
      }
    },
    {
      icon: (active: any) => (
        <Icon className={active ? styles.active : undefined} icon={KeyRound} />
      ),
      title: "令牌",
      key: SidebarTabKey.Token,
      onClick: () => {
        navigate('/token')
      }
    },
    {
      icon: (active: any) => (
        <Icon className={active ? styles.active : undefined} icon={FileText} />
      ),
      title: "日志",
      key: SidebarTabKey.Logger,
      onClick: () => {
        navigate('/logger')
      }
    },
    {
      icon: (active: any) => (
        <Icon className={active ? styles.active : undefined} icon={CircleUserRound} />
      ),
      title: "钱包/个人",
      key: SidebarTabKey.Current,
      onClick: () => {
        navigate('/current')
      }
    }
  ] as MobileTabBarItemProps[];

  return <MobileTabBar activeKey={activeKey} className={styles.container} items={items} />;
});

Nav.displayName = 'MobileNav';

export default Nav;

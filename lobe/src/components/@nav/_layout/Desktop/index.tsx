import { SideNav } from '@lobehub/ui';
import { memo, useEffect, useState } from 'react';

import { useActiveTabKey } from '../../../../hooks/useActiveTabKey';

import BottomActions from './BottomActions';
import TopActions from './TopActions';
import { useLocation } from 'react-router-dom';


const Nav = memo(() => {
  const [sidebarKey, setSidebarKey] = useState(useActiveTabKey());
  const location = useLocation();

  useEffect(() => {
    setSidebarKey(useActiveTabKey())
  }, [location]);

  return (
    <SideNav
      bottomActions={<BottomActions />}
      topActions={<TopActions tab={sidebarKey} />}
    />
  );
});

Nav.displayName = 'DesktopNav';

export default Nav;

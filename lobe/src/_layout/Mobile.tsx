import { memo } from 'react';
import { LayoutProps } from './type';
import { Outlet } from 'react-router-dom';

const Layout = memo(({ nav }: LayoutProps) => {
  return (
    <>
      <Outlet />
      {nav}
    </>
  );
});

Layout.displayName = 'MobileMainLayout';

export default Layout;

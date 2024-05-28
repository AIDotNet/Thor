import { memo } from 'react';
import { Flexbox } from 'react-layout-kit';


import { LayoutProps } from './type';
import { Outlet } from 'react-router-dom';

const Layout = memo<LayoutProps>(({ nav }) => {
    return (
        <Flexbox
            height={'100%'}
            horizontal
            style={{
                position: 'relative',
            }}
            width={'100%'}
        >
            {nav}
            <Outlet />
        </Flexbox>
    );
});

Layout.displayName = 'DesktopMainLayout';

export default Layout;

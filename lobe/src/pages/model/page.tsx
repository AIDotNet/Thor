
import { LayoutProps } from '../../_layout/type';
import ServerLayout from '../../components/server/ServerLayout';

import Desktop from './(desktop)/page';
import Mobile from './(moblie)/page';

const MainLayout = ServerLayout<LayoutProps>({ Desktop, Mobile });

MainLayout.displayName = 'MainLayout';

export default MainLayout;

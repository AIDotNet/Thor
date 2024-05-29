import ServerLayout from '../components/server/ServerLayout';

import Desktop from './Desktop';
import Mobile from './Mobile';
import { LayoutProps } from './type';

const MainLayout = ServerLayout<LayoutProps>({ Desktop, Mobile });

MainLayout.displayName = 'MainLayout';

export default MainLayout;

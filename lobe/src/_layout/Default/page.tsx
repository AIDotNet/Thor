import ServerLayout from '../../components/server/ServerLayout';
import Desktop from './Desktop';
import Mobile from './Mobile';

const DefaultLayout = ServerLayout({ Desktop, Mobile });

DefaultLayout.displayName = 'MainLayout';

export default DefaultLayout;

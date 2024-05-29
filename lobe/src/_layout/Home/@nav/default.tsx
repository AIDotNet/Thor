import ServerLayout from '../../../components/server/ServerLayout';

import Desktop from './_layout/Desktop';
import Mobile from './_layout/Moblie';

const DocMainLayout = ServerLayout({ Desktop, Mobile });

DocMainLayout.displayName = 'MainLayout';

export default DocMainLayout;

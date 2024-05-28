
import { SidebarTabKey } from '../store/global/initialState';

/**
 * Returns the active tab key (chat/market/settings/...)
 */
export const useActiveTabKey = () => {
    const pathname = window.location.pathname;
    return pathname.split('/').find(Boolean)! as SidebarTabKey;
};

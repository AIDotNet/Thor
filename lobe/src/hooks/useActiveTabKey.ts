
import { SidebarTabKey } from '../store/global/initialState';

/**
 * Returns the active tab key 
 */
export const useActiveTabKey = () => {
    const pathname = window.location.pathname;
    return pathname.split('/').find(Boolean)! as SidebarTabKey;
};

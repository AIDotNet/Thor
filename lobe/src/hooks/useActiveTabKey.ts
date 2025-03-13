
import { useEffect, useState } from 'react';
import { SidebarTabKey } from '../store/global/initialState';
import { info } from '../services/UserService';
import { ResultDto, User } from '..';

/**
 * Returns the active tab key 
 */
export const useActiveTabKey = () => {
    const pathname = window.location.pathname;
    var key = pathname.split('/').find(Boolean)! as SidebarTabKey;
    return key;
};

export const useActiveUser = () => {
    const [user, setUser] = useState<User | null>(null);

    useEffect(() => {
        info()
            .then((data: ResultDto<User>) => {
                setUser(data.data)
            });
    }, []);

    return user;
}
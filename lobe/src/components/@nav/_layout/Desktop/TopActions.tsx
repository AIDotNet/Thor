/* eslint-disable sort-keys-fix/sort-keys-fix , typescript-sort-keys/interface */
import { ActionIcon } from '@lobehub/ui';
import { BarChart3, BarChart, KeyRound, ShipWheel, FileText, Code, User, CircleUserRound, Settings } from 'lucide-react';

import { memo } from 'react';
import { SidebarTabKey } from '../../../../store/global/initialState';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';

export interface TopActionProps {
    tab?: SidebarTabKey;
}

const TopActions = memo<TopActionProps>(({ tab }) => {

    const navigate = useNavigate();

    const items = [
        {
            href: '/panel',
            icon: BarChart3,
            title: "面板",
            key: SidebarTabKey.Panel,
            onClick: () => {
                navigate('/panel')
            }
        },
        {
            href: '/channel',
            icon: BarChart,
            title: "渠道",
            key: SidebarTabKey.Channel,
            onClick: () => {
                navigate('/channel')
            }
        },
        {
            href: '/token',
            icon: KeyRound,
            title: "令牌",
            key: SidebarTabKey.Token,
            onClick: () => {
                navigate('/token')
            }
        },
        {
            href: '/product',
            icon: ShipWheel,
            title: "产品",
            key: SidebarTabKey.Product,
            onClick: () => {
                navigate('/product')
            }
        },
        {
            href: '/logger',
            icon: FileText,
            title: "日志",
            key: SidebarTabKey.Logger,
            onClick: () => {
                navigate('/logger')
            }
        },
        {
            href: '/redeem-code',
            icon: Code,
            title: "兑换码",
            key: SidebarTabKey.RedeemCode,
            onClick: () => {
                navigate('/redeem-code')
            }
        },
        {
            href: '/user',
            icon: User,
            title: "用户管理",
            key: SidebarTabKey.User,
            onClick: () => {
                navigate('/user')
            }
        },
        {
            href: '/current',
            icon: CircleUserRound,
            title: "钱包/个人",
            key: SidebarTabKey.Current,
            onClick: () => {
                navigate('/current')
            }
        },
        {
            href: '/setting',
            icon: Settings,
            title: "系统设置",
            key: SidebarTabKey.Setting,
            onClick: () => {
                navigate('/setting')
            }
        },
    ];


    return (
        <>
            {items.map((item: any) => {
                return (
                    <Link
                        to={item.href}
                        onClick={(e) => {
                            e.preventDefault();
                            item.onClick();
                        }}
                    >
                        <ActionIcon
                            active={tab === item?.key}
                            icon={item.icon}
                            placement={'right'}
                            size="large"
                            title={item.title}
                        />
                    </Link>)
            })}
        </>
    );
});

export default TopActions;

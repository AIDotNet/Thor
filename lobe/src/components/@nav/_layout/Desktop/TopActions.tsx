import { ActionIcon } from '@lobehub/ui';
import { BarChart3, BarChart, KeyRound, ShipWheel, Ghost, FileText, BotMessageSquare, Code, User, CircleUserRound, Settings } from 'lucide-react';
import { memo } from 'react';
import { SidebarTabKey } from '../../../../store/global/initialState';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { GeneralSetting, InitSetting } from '../../../../services/SettingService';

export interface TopActionProps {
    tab?: SidebarTabKey;
}

const TopActions = memo<TopActionProps>(({ tab }) => {

    const navigate = useNavigate();
    const chatDisabled = InitSetting.find((item: any) => item.key === GeneralSetting.ChatLink)
    const vidolDisabled = InitSetting.find((item: any) => item.key === GeneralSetting.VidolLink)

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
            disabled: chatDisabled.value === undefined || chatDisabled.value === '',
            href: InitSetting[GeneralSetting.RechargeAddress as any]?.value,
            icon: BotMessageSquare,
            title: "对话",
            key: SidebarTabKey.Chat,
            onClick: () => {
                // 给chatDisabled.value url添加query
                const url = new URL(chatDisabled.value);
                url.searchParams.append('token', localStorage.getItem('token') || '');
                window.open(url.href, '_blank');
            }
        },
        {
            disabled: (vidolDisabled.value === undefined || vidolDisabled.value === ''),
            href: InitSetting[GeneralSetting.VidolLink as any]?.value,
            icon: Ghost,
            title: "数字人",
            key: SidebarTabKey.Vidol,
            onClick: () => {
                const url = new URL(vidolDisabled.value);
                url.searchParams.append('token', localStorage.getItem('token') || '');
                window.open(url.href, '_blank');
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
            {items
                .filter((item: any) => !item.disabled)
                .map((item: any) => {
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

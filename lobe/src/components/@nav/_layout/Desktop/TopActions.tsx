import { ActionIcon } from '@lobehub/ui';
import { BarChart3, BarChart, KeyRound, ShipWheel, Ghost, FileText, BotMessageSquare, Code, User, CircleUserRound, Settings } from 'lucide-react';
import { memo, useEffect, useState } from 'react';
import { SidebarTabKey } from '../../../../store/global/initialState';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { GeneralSetting, InitSetting } from '../../../../services/SettingService';
import { info } from '../../../../services/UserService';
import { SlidersOutlined } from '@ant-design/icons';

export interface TopActionProps {
    tab?: SidebarTabKey;
}

const TopActions = memo<TopActionProps>(({ tab }) => {

    const navigate = useNavigate();
    const [setUser] = useState<any>({})
    const chatDisabled = InitSetting.find((item: any) => item.key === GeneralSetting.ChatLink)
    const vidolDisabled = InitSetting.find((item: any) => item.key === GeneralSetting.VidolLink)

    const [items, setItems] = useState([
        {
            href: '/panel',
            icon: BarChart3,
            title: "面板",
            enable: true,
            key: SidebarTabKey.Panel,
            role: 'user,admin',
            onClick: () => {
                navigate('/panel')
            }
        },
        {
            href: '/channel',
            icon: BarChart,
            title: "渠道",
            enable: true,
            key: SidebarTabKey.Channel,
            onClick: () => {
                navigate('/channel')
            },
            role: 'admin'
        },
        {
            disabled: chatDisabled.value === undefined || chatDisabled.value === '',
            href: InitSetting[GeneralSetting.RechargeAddress as any]?.value,
            icon: BotMessageSquare,
            title: "对话",
            enable: false,
            key: SidebarTabKey.Chat,
            onClick: () => {
                // 给chatDisabled.value url添加query
                const url = new URL(chatDisabled.value);
                url.searchParams.append('token', localStorage.getItem('token') || '');
                window.open(url.href, '_blank');
            },
            role: 'user,admin'
        },
        {
            disabled: (vidolDisabled.value === undefined || vidolDisabled.value === ''),
            href: InitSetting[GeneralSetting.VidolLink as any]?.value,
            icon: Ghost,
            title: "数字人",
            enable: false,
            key: SidebarTabKey.Vidol,
            onClick: () => {
                const url = new URL(vidolDisabled.value);
                url.searchParams.append('token', localStorage.getItem('token') || '');
                window.open(url.href, '_blank');
            },
            role: 'user,admin'
        },
        {
            href: '/token',
            icon: KeyRound,
            enable: true,
            title: "令牌",
            key: SidebarTabKey.Token,
            onClick: () => {
                navigate('/token')
            },
            role: 'user,admin'
        },
        {
            href: '/product',
            icon: ShipWheel,
            title: "产品",
            enable: true,
            key: SidebarTabKey.Product,
            onClick: () => {
                navigate('/product')
            },
            role: 'admin'
        },
        {
            href: '/logger',
            icon: FileText,
            title: "日志",
            enable: true,
            key: SidebarTabKey.Logger,
            onClick: () => {
                navigate('/logger')
            },
            role: 'user,admin'
        },
        {
            href: '/redeem-code',
            icon: Code,
            enable: true,
            title: "兑换码",
            key: SidebarTabKey.RedeemCode,
            onClick: () => {
                navigate('/redeem-code')
            },
            role: 'admin'
        },
        {
            href: '/rate-limit',
            icon: SlidersOutlined,
            enable: true,
            title: "限流",
            key: SidebarTabKey.RateLimit,
            onClick: () => {
                navigate('/rate-limit')
            },
            role: 'admin'
        },
        {
            href: '/user',
            icon: User,
            title: "用户管理",
            enable: true,
            key: SidebarTabKey.User,
            onClick: () => {
                navigate('/user')
            },
            role: 'admin'
        },
        {
            href: '/current',
            icon: CircleUserRound,
            title: "钱包/个人",
            enable: true,
            key: SidebarTabKey.Current,
            onClick: () => {
                navigate('/current')
            },
            role: 'user,admin'
        },
        {
            href: '/setting',
            icon: Settings,
            title: "系统设置",
            enable: true,
            key: SidebarTabKey.Setting,
            onClick: () => {
                navigate('/setting')
            },
            role: 'admin'
        },
    ]);

    function loadUser() {
        info()
            .then((res) => {
                setUser(res.data);
            });
    }

    useEffect(() => {
        // 获取当前用户token
        const token = localStorage.getItem('token');
        if (!token) {
            navigate('/login');
            return;
        }

        // 解析token
        const role = localStorage.getItem('role') as string;

        const chatLink = InitSetting?.find(x => x.key === GeneralSetting.ChatLink)?.value

        if (chatLink) {
            // 修改 Chat 
            items.forEach(item => {
                if (item.key === SidebarTabKey.Chat) {
                    item.enable = true;
                }
            })
        }

        const vidolLink = InitSetting?.find(x => x.key === GeneralSetting.VidolLink)?.value

        if (vidolLink) {
            // 修改 Vidol 
            items.forEach(item => {
                if (item.key === SidebarTabKey.Vidol) {
                    item.enable = true;
                }
            })
        }


        setItems(items.filter(item => item.enable && item.role.includes(role)));


        // 获取用户信息
        loadUser();
    }, [])

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

import { memo, useEffect, useState, useMemo } from "react";

import { useActiveTabKey } from "../../../../hooks/useActiveTabKey";
import {
  BarChart,
  KeyRound,
  ShipWheel,
  Brain,
  FileText,
  BotMessageSquare,
  Code,
  User,
  CircleUserRound,
  Settings,
  Shuffle,
  Handshake,
  BrainCog,
  UsersRound,
  Home,
  ChevronRight,
  Bug,
  PieChart,
  Megaphone
} from "lucide-react";
import './index.css'
import { SidebarTabKey } from "../../../../store/global/initialState";
import { useLocation, useNavigate } from "react-router-dom";
import { Menu, Typography, Badge, Divider } from "antd";
import {
  GeneralSetting,
  InitSetting,
} from "../../../../services/SettingService";
import { SlidersOutlined } from "@ant-design/icons";
import { info } from "../../../../services/UserService";
import { useTranslation } from "react-i18next";

const { Text } = Typography;

// Define type for menu items to match expected structure
interface MenuItem {
  icon: JSX.Element;
  label: React.ReactNode;
  enable: boolean;
  key: string;
  role?: string;
  onClick?: () => void;
  disabled?: boolean;
  children?: MenuItem[];
}

const Nav = memo(() => {
  const { t, i18n } = useTranslation();
  const [sidebarKey, setSidebarKey] = useState<SidebarTabKey>(useActiveTabKey());
  const location = useLocation();
  const navigate = useNavigate();
  const chatDisabled = InitSetting.find(
    (item: any) => item.key === GeneralSetting.ChatLink
  );
  const [userRole, setUserRole] = useState<string | null>(null);
  // 管理导航菜单展开状态
  const [openKeys, setOpenKeys] = useState<string[]>([]);

  // 使用 useMemo 并依赖 i18n.language，这样语言变化时菜单会重新生成
  const getMenuItems = useMemo((): MenuItem[] => {
    const items: MenuItem[] = [
      {
        icon: <Home />,
        label: <Badge dot={false}>{t('sidebar.panel')}</Badge>,
        enable: true,
        key: SidebarTabKey.Panel,
        role: "user,admin",
        onClick: () => {
          navigate("/panel");
        },
      },
      {
        key: SidebarTabKey.AI,
        label: <Text strong>{t('sidebar.ai')}</Text>,
        enable: true,
        icon: <Brain />,
        role: "user,admin",
        children: [
          {
            icon: <BarChart />,
            label: t('sidebar.channel'),
            enable: true,
            key: SidebarTabKey.Channel,
            onClick: () => {
              navigate("/channel");
            },
            role: "admin",
          },
          {
            disabled: chatDisabled.value === undefined || chatDisabled.value === "",
            icon: <BotMessageSquare />,
            label: <Badge dot={true} color="blue">{t('sidebar.chat')}</Badge>,
            enable: false,
            key: SidebarTabKey.Chat,
            onClick: () => {
              // 给chatDisabled.value url添加query
              const url = new URL(chatDisabled.value);
              url.searchParams.append("token", localStorage.getItem("token") || "");
              window.open(url.href, "_blank");
            },
            role: "user,admin",
          },
          {
            icon: <BrainCog />,
            enable: true,
            label: t('sidebar.modelManager'),
            key: SidebarTabKey.ModelManager,
            onClick: () => {
              navigate("/model-manager");
            },
            role: "admin",
          },
          {
            icon: <Shuffle />,
            enable: true,
            label: t('sidebar.modelMap'),
            key: SidebarTabKey.ModelMap,
            onClick: () => {
              navigate("/model-map");
            },
            role: "admin",
          }
        ]
      },
      {
        icon: <Bug />,
        enable: true,
        label: t('sidebar.playground'),
        key: SidebarTabKey.Playground,
        onClick: () => {
          navigate("/playground");
        },
      },
      {
        icon: <KeyRound />,
        enable: true,
        label: t('sidebar.token'),
        key: SidebarTabKey.Token,
        onClick: () => {
          navigate("/token");
        },
        role: "user,admin",
      },
      {
        icon: <PieChart />,
        enable: true,
        label: t('sidebar.usage'),
        key: SidebarTabKey.Usage,
        onClick: () => {
          navigate("/usage");
        },
        role: "user,admin",
      },
      
      // Business section
      {
        key: SidebarTabKey.Business,
        label: <Text strong>{t('sidebar.business')}</Text>,
        icon: <Handshake />,
        enable: true,
        role: "admin",
        children: [
          {
            icon: <Code />,
            enable: true,
            label: t('sidebar.redeemCode'),
            key: SidebarTabKey.RedeemCode,
            onClick: () => {
              navigate("/redeem-code");
            },
            role: "admin",
          },
          {
            icon: <ShipWheel />,
            label: t('sidebar.product'),
            enable: true,
            key: SidebarTabKey.Product,
            onClick: () => {
              navigate("/product");
            },
            role: "admin",
          },
        ]
      },
      
      // User section
      {
        icon: <CircleUserRound />,
        label: t('sidebar.current'),
        enable: true,
        key: SidebarTabKey.Current,
        onClick: () => {
          navigate("/current");
        },
        role: "user,admin",
      },
      
      // System settings
      {
        icon: <Settings />,
        label: <Text strong>{t('sidebar.setting')}</Text>,
        enable: true,
        key: SidebarTabKey.Setting,
        children: [
          {
            icon: <Megaphone />,
            label: t('sidebar.announcement'),
            enable: true,
            key: SidebarTabKey.Announcement,
            onClick: () => {
              navigate("/announcement");
            },
            role: "admin",
          },
          {
            icon: <FileText />,
            label: t('sidebar.logger'),
            enable: true,
            key: SidebarTabKey.Logger,
            onClick: () => {
              navigate("/logger");
            },
            role: "user,admin",
          },
          {
            icon: <SlidersOutlined />,
            enable: true,
            label: t('sidebar.rateLimit'),
            key: SidebarTabKey.RateLimit,
            onClick: () => {
              navigate("/rate-limit");
            },
            role: "admin",
          },
          {
            icon: <User />,
            label: t('sidebar.user'),
            enable: true,
            key: SidebarTabKey.User,
            onClick: () => {
              navigate("/user");
            },
            role: "admin",
          },
          {
            icon: <UsersRound />,
            label: t('sidebar.userGroup'),
            enable: true,
            key: SidebarTabKey.UserGroup,
            onClick: () => {
              navigate("/user-group");
            },
            role: "admin",
          },
          {
            icon: <Settings />,
            label: t('nav.setting'),
            enable: true,
            key: 'system-setting',
            onClick: () => {
              navigate("/setting");
            },
            role: "admin",
          },
        ],
      },
    ];
    if (userRole) {
      return items.filter((item) => {
        if (item.children) {
          item.children = item.children.filter((child) => {
            if (!child.role) return true;
            return child.role.split(",").includes(userRole);
          });
          // Only return items with children
          return item.children.length > 0 && (item.role ? item.role.split(",").includes(userRole) : true);
        }
        if (!item.role) return true;
        return item.role.split(",").includes(userRole);
      });
    }
    
    return items;
  }, [t, i18n.language, navigate, chatDisabled, userRole]); // 依赖 i18n.language 确保语言变化时重新计算

  const [items, setItems] = useState<MenuItem[]>(getMenuItems);

  useEffect(() => {
    loadUser();
  }, []);

  // 当语言或菜单项定义变化时更新菜单
  useEffect(() => {
    setItems(getMenuItems);
  }, [getMenuItems]);

  function loadUser() {
    info().then((res) => {
      if (!res.success) return;
      const role = res.data.role;
      if (!role) return;
      setUserRole(role);
    });
  }

  useEffect(() => {
    const path = location.pathname;
    for (let index = 0; index < items.length; index++) {
      const element = items[index];
      if (element.onClick && path.includes(element.key)) {
        setSidebarKey(element.key as SidebarTabKey);
        return;
      }
      if (element.children) {
        for (let j = 0; j < element.children.length; j++) {
          const child = element.children[j];
          // @ts-ignore
          if (child.onClick && path.includes(child.key)) {
            setSidebarKey(child.key as SidebarTabKey);
            // 确保父菜单项展开
            if (!openKeys.includes(element.key)) {
              setOpenKeys([...openKeys, element.key]);
            }
            return;
          }
        }
      }
    }
  }, [location.pathname, items, openKeys]);

  // 处理菜单展开/收起
  const handleOpenChange = (keys: string[]) => {
    setOpenKeys(keys);
  };

  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        height: "100%",
        boxShadow: "0 2px 8px rgba(0, 0, 0, 0.15)",
      }}
    >
      <Divider style={{ margin: "0 0 8px 0" }} />
      <div
        style={{
          flex: 1,
          overflow: "auto",
        }}
      >
        <Menu
          mode="inline"
          style={{
            border: "none",
            padding: "0 4px",
            backgroundColor: 'transparent',
          }}
          items={items}
          selectedKeys={[sidebarKey]}
          openKeys={openKeys}
          onOpenChange={handleOpenChange}
          expandIcon={({ isOpen }) => <ChevronRight 
          size={16} style={{
            transform: isOpen ? 'rotate(90deg)' : 'none',
            transition: 'transform 0.2s',
            color: 'inherit',
          }} />}
        />
      </div>
      <Divider style={{ margin: "8px 0 0 0" }} />
    </div>
  );
});

export default Nav;

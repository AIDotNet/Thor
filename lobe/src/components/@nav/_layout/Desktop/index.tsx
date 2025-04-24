import { memo, useEffect, useState, useMemo } from "react";

import { useActiveTabKey } from "../../../../hooks/useActiveTabKey";
import {
  BarChart3,
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
  UsersRound
} from "lucide-react";
import './index.css'
import { SidebarTabKey } from "../../../../store/global/initialState";
import BottomActions from "./BottomActions";
import { useLocation, useNavigate } from "react-router-dom";
import { Menu } from "antd";
import {
  GeneralSetting,
  InitSetting,
} from "../../../../services/SettingService";
import { SlidersOutlined } from "@ant-design/icons";
import { info } from "../../../../services/UserService";
import { useTranslation } from "react-i18next";

const Nav = memo(() => {
  const { t, i18n } = useTranslation();
  const [sidebarKey, setSidebarKey] = useState(useActiveTabKey());
  const location = useLocation();
  const navigate = useNavigate();
  const chatDisabled = InitSetting.find(
    (item: any) => item.key === GeneralSetting.ChatLink
  );
  const [userRole, setUserRole] = useState<string | null>(null);
  // 管理导航菜单展开状态
  const [openKeys, setOpenKeys] = useState<string[]>([]);

  // 使用 useMemo 并依赖 i18n.language，这样语言变化时菜单会重新生成
  const getMenuItems = useMemo(() => {
    const items = [
      {
        icon: <BarChart3 />,
        label: t('sidebar.panel'),
        enable: true,
        key: SidebarTabKey.Panel,
        role: "user,admin",
        onClick: () => {
          navigate("/panel");
        },
      }, {
        key: SidebarTabKey.AI,
        label: t('sidebar.ai'),
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
            label: t('sidebar.chat'),
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
        key: SidebarTabKey.Business,
        label: t('sidebar.business'),
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
      {
        icon: <Settings />,
        label: t('sidebar.setting'),
        enable: true,
        key: SidebarTabKey.Setting,
        children: [
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

    // 如果已经获取了用户角色，则按照角色过滤菜单项
    if (userRole) {
      return items.filter((item) => {
        if (item.children) {
          item.children = item.children.filter((child: any) => {
            if (!child.role) return true;
            return child.role.split(",").includes(userRole);
          });
        }
        if (!item.role) return true;
        return item.role.split(",").includes(userRole);
      });
    }
    
    return items;
  }, [t, i18n.language, navigate, chatDisabled, userRole]); // 依赖 i18n.language 确保语言变化时重新计算

  const [items, setItems] = useState(getMenuItems);

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
        setSidebarKey(element.key);
        return;
      }
      if (element.children) {
        for (let j = 0; j < element.children.length; j++) {
          const child = element.children[j];
          // @ts-ignore
          if (child.onClick && path.includes(child.key)) {
            setSidebarKey(element.key);
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
      }}
    >
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
          }}
          items={items}
          selectedKeys={[sidebarKey]}
          openKeys={openKeys}
          onOpenChange={handleOpenChange}
        />
      </div>
      <BottomActions />
    </div>
  );
});

export default Nav;

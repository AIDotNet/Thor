import { memo, useEffect, useState } from "react";

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

const Nav = memo(() => {
  const [sidebarKey, setSidebarKey] = useState(useActiveTabKey());
  const location = useLocation();
  const navigate = useNavigate();
  const chatDisabled = InitSetting.find(
    (item: any) => item.key === GeneralSetting.ChatLink
  );

  const [items, setItems] = useState<any[]>([
    {
      icon: <BarChart3 />,
      label: "面板",
      enable: true,
      key: SidebarTabKey.Panel,
      role: "user,admin",
      onClick: () => {
        navigate("/panel");
      },
    }, {
      key: SidebarTabKey.AI,
      label: "AI服务",
      enable: true,
      icon: <Brain />,
      role: "user,admin",
      children: [
        {
          icon: <BarChart />,
          label: "渠道",
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
          label: "对话",
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
          label: "模型管理",
          key: SidebarTabKey.ModelManager,
          onClick: () => {
            navigate("/model-manager");
          },
          role: "admin",
        },
        {
          icon: <Shuffle />,
          enable: true,
          label: "模型映射",
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
      label: "API Key 管理",
      key: SidebarTabKey.Token,
      onClick: () => {
        navigate("/token");
      },
      role: "user,admin",
    },
    {
      key: SidebarTabKey.Business,
      label: "运营服务",
      icon: <Handshake />,
      enable: true,
      role: "admin",
      children: [
        {
          icon: <Code />,
          enable: true,
          label: "兑换码",
          key: SidebarTabKey.RedeemCode,
          onClick: () => {
            navigate("/redeem-code");
          },
          role: "admin",
        },
        {
          icon: <ShipWheel />,
          label: "产品",
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
      label: "钱包/个人",
      enable: true,
      key: SidebarTabKey.Current,
      onClick: () => {
        navigate("/current");
      },
      role: "user,admin",
    },
    {
      icon: <Settings />,
      label: "系统服务",
      enable: true,
      key: SidebarTabKey.Setting,
      children: [
        {
          icon: <FileText />,
          label: "日志",
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
          label: "限流",
          key: SidebarTabKey.RateLimit,
          onClick: () => {
            navigate("/rate-limit");
          },
          role: "admin",
        },
        {
          icon: <User />,
          label: "用户管理",
          enable: true,
          key: SidebarTabKey.User,
          onClick: () => {
            navigate("/user");
          },
          role: "admin",
        },
        {
          icon: <UsersRound />,
          label: "用户分组",
          enable: true,
          key: SidebarTabKey.UserGroup,
          onClick: () => {
            navigate("/user-group");
          },
          role: "admin",
        },
        {
          icon: <Settings />,
          label: "系统设置",
          enable: true,
          key: SidebarTabKey.Setting,
          onClick: () => {
            navigate("/setting");
          },
          role: "admin",
        }
      ],
      role: "user,admin",
    }
  ]);

  useEffect(() => {
    // 获取当前用户token
    const token = localStorage.getItem("token");
    if (!token) {
      navigate("/login");
      return;
    }

    // 解析token
    const role = localStorage.getItem("role") as string;

    const chatLink = InitSetting?.find(
      (x) => x.key === GeneralSetting.ChatLink
    )?.value;

    if (chatLink) {
      items.forEach((item) => {
        if (item.children) {
          item.children.forEach((child: any) => {
            if (child.key === SidebarTabKey.Chat) {
              child.enable = chatLink !== "";
            }
          });
        }
      });
    }

    // 过滤items
    items.forEach((item) => {
      if (item.children) {
        item.children = item.children.filter((child: any) =>
          child.role.includes(role) && child.enable);
      }
    });


    setItems(items.filter((item) => item.enable && item.role.includes(role)));

    // 获取用户信息
    loadUser();
  }, []);

  function loadUser() {
    info().then(() => {
    });
  }
  useEffect(() => {
    setSidebarKey(useActiveTabKey());
  }, [location]);

  return (
    <>
      <Menu
        selectable
        style={{
          height: '100%'
        }}
        className="desktop-layout"
        selectedKeys={[sidebarKey]}
        onClick={() => {
          setSidebarKey(useActiveTabKey());
        }}
        mode="inline"
        items={items}
      />
      <BottomActions />
    </>
  );
});

Nav.displayName = "DesktopNav";

export default Nav;

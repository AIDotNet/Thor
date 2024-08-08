import { memo, useEffect, useState } from "react";

import { useActiveTabKey } from "../../../../hooks/useActiveTabKey";
import {
  BarChart3,
  BarChart,
  KeyRound,
  ShipWheel,
  Ghost,
  FileText,
  BotMessageSquare,
  Code,
  User,
  CircleUserRound,
  Settings,
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
  const vidolDisabled = InitSetting.find(
    (item: any) => item.key === GeneralSetting.VidolLink
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
    },
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
      disabled: vidolDisabled.value === undefined || vidolDisabled.value === "",
      icon: <Ghost />,
      label: "数字人",
      enable: false,
      key: SidebarTabKey.Vidol,
      onClick: () => {
        const url = new URL(vidolDisabled.value);
        url.searchParams.append("token", localStorage.getItem("token") || "");
        window.open(url.href, "_blank");
      },
      role: "user,admin",
    },
    {
      icon: <KeyRound />,
      enable: true,
      label: "令牌",
      key: SidebarTabKey.Token,
      onClick: () => {
        navigate("/token");
      },
      role: "user,admin",
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
      label: "系统设置",
      enable: true,
      key: SidebarTabKey.Setting,
      onClick: () => {
        navigate("/setting");
      },
      role: "admin",
    },
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
      // 修改 Chat
      items.forEach((item) => {
        if (item.key === SidebarTabKey.Chat) {
          item.enable = true;
        }
      });
    }

    const vidolLink = InitSetting?.find(
      (x) => x.key === GeneralSetting.VidolLink
    )?.value;

    if (vidolLink) {
      // 修改 Vidol
      items.forEach((item) => {
        if (item.key === SidebarTabKey.Vidol) {
          item.enable = true;
        }
      });
    }

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

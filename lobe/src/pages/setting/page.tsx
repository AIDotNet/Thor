
import {  GetSetting } from "../../services/SettingService";
import { useEffect, useState } from "react";
import { Tabs } from "antd";
import OtherSettings from "./features/OtherSettings";
import ServiceSetup from "./features/ServiceSetup";
import SystemSetup from "./features/SystemSetup";

export default function Channel() {
  const [settings, setSettings] = useState([] as any[]);

  function loadSettings() {
    GetSetting()
      .then((res) => {
        setSettings(res.data);
      });
  }

  useEffect(() => {
    loadSettings();
  }, []);

  return (
    <Tabs type="line"
      items={[
        {
          label: '业务设置',
          key: '1',
          children: <ServiceSetup settings={settings} setSettings={setSettings} />
        },
        {
          label: '系统设置',
          key: '2',
          children: <SystemSetup settings={settings} setSettings={setSettings} />
        },
        {
          label: '其他设置',
          key: '3',
          children: <OtherSettings settings={settings} setSettings={setSettings} />
        },
      ]}
      style={{
        margin: '20px',
        height: 'calc(100vh - 200px)',
        width: '100%',
        overflow: 'auto',

      }}>
    </Tabs>
  )
}
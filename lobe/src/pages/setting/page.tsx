
import { GeneralSetting, GetSetting } from "../../services/SettingService";
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

        for (let i = 0; i < res.data.length; i++) {
          // 格式化json
          if (res.data[i].key === GeneralSetting.ModelPromptRate || res.data[i].key === GeneralSetting.ModelCompletionRate) {
            res.data[i].value = JSON.stringify(JSON.parse(res.data[i].value), null, 2);
          }
        }

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
        height: '100%',
        width: '100%',
        overflow: 'auto',

      }}>
    </Tabs>
  )
}
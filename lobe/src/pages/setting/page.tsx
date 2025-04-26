import { GetSetting } from "../../services/SettingService";
import { useEffect, useState } from "react";
import { Tabs } from "antd";
import OtherSettings from "./features/OtherSettings";
import ServiceSetup from "./features/ServiceSetup";
import SystemSetup from "./features/SystemSetup";
import { useTranslation } from "react-i18next";

export default function Setting() {
  const [settings, setSettings] = useState([] as any[]);
  const { t } = useTranslation();

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
          label: t('settingPage.tabs.serviceSetup'),
          key: '1',
          children: <ServiceSetup settings={settings} setSettings={setSettings} />
        },
        {
          label: t('settingPage.tabs.systemSetup'),
          key: '2',
          children: <SystemSetup settings={settings} setSettings={setSettings} />
        },
        {
          label: t('settingPage.tabs.otherSettings'),
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
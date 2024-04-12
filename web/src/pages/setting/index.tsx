import { TabPane, Tabs } from "@douyinfe/semi-ui";
import ServiceSetup from "./features/ServiceSetup";
import SystemSetup from "./features/SystemSetup";
import OtherSettings from "./features/OtherSettings";
import { GetSetting } from "../../services/SettingService";
import { useEffect, useState } from "react";
import { Setting } from "../..";

export default function Channel() {
    const [settings, setSettings] = useState([] as Setting[]);

    function loadSettings(){
        GetSetting()
            .then((res) => {
                setSettings(res.data);
            });
    }

    useEffect(() => {
        loadSettings();
    }, []);

    return (
        <Tabs type="line">
            <TabPane tab="业务设置" itemKey="1">
                <ServiceSetup setSettings={(v:any)=>{
                    setSettings(v);
                }} settings={settings}/>
            </TabPane>
            <TabPane tab="系统设置" itemKey="2">
                <SystemSetup setSettings={(v:any)=>{
                    setSettings(v);
                }}  settings={settings}/>
            </TabPane>
            <TabPane tab="其他设置" itemKey="3">
                <OtherSettings setSettings={(v:any)=>{
                    setSettings(v);
                }}  settings={settings}/>
            </TabPane>
        </Tabs>
    )
}
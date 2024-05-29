import { useEffect, useState } from 'react';
import { Button, Card, Collapse, Form, Input, notification } from 'antd';
import { OtherSetting, UpdateSetting } from '../../../services/SettingService';

interface OtherSettingsProps {
    settings: any[];
    setSettings?: any;
}

export default function OtherSettings({
    settings,
    setSettings
}: OtherSettingsProps) {
    const [input, setInput] = useState<any>([]);

    useEffect(() => {
        const initialValues = settings.reduce((acc, setting) => {
            acc[setting.key] = setting.value;
            return acc;
        }, {});
        setInput(initialValues);
    }, [settings]);

    function handleSubmit() {
        UpdateSetting(settings)
            .then((res) => {
                if (res.success) {
                    notification.success({
                        message: '修改成功',
                    });
                } else {
                    notification.error({
                        message: '修改失败',
                    });
                }
            });
    }

    const handleInputChange = (key: string, value: any) => {
        setInput((prevInput: any) => ({
            ...prevInput,
            [key]: value
        }));
        const setting = settings.find(s => s.key === key);
        if (setting) {
            setting.value = value;
        }

        setSettings(settings);
    };
    return (
        <Card
            title="通用设置"
            style={{ maxWidth: '100%' }}
        >
            <Form
            >
                <Collapse>
                    <Collapse.Panel
                        header="站点设置"
                        key="1">
                        <Form.Item
                            label="网站标题"
                            rules={[{ required: true, message: '请输入网站标题' }]}
                        >
                            <Input
                                value={input[OtherSetting.WebTitle]}
                                onChange={(e) => {
                                    handleInputChange(OtherSetting.WebTitle, e.target.value);
                                }}
                            />
                        </Form.Item>
                        <Form.Item
                            label="网站Logo地址"
                            rules={[{ required: true, message: '请输入网站Logo地址' }]}
                        >
                            <Input
                                value={input[OtherSetting.WebLogo]}
                                onChange={(e) => {
                                    handleInputChange(OtherSetting.WebLogo, e.target.value);
                                }}
                            />
                        </Form.Item>
                        <Form.Item
                            label="首页内容"
                            rules={[{ required: true, message: '请输入首页内容' }]}
                        >
                            <Input.TextArea
                                value={input[OtherSetting.IndexContent]}
                                onChange={(e) => {
                                    handleInputChange(OtherSetting.IndexContent, e.target.value);
                                }}
                            />
                        </Form.Item>
                        <Button block type="primary" onClick={()=>handleSubmit()}>保存设置</Button>
                    </Collapse.Panel>
                </Collapse>
            </Form>
        </Card>
    );
}
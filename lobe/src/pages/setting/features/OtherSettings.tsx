import { useState } from 'react';
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
    const [input, setInput] = useState({
        webTitle: settings?.find(s => s.key === OtherSetting.WebTitle)?.value || '',
        webLogo: settings?.find(s => s.key === OtherSetting.WebLogo)?.value || '',
        indexContent: settings?.find(s => s.key === OtherSetting.IndexContent)?.value || '',
    });

    function handleSubmit() {
        const updatedSettings = [
            { key: OtherSetting.WebTitle, value: input.webTitle },
            { key: OtherSetting.WebLogo, value: input.webLogo },
            { key: OtherSetting.IndexContent, value: input.indexContent },
        ];

        UpdateSetting(updatedSettings)
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

    return (
        <>
            <Card
                title="通用设置"
                style={{ maxWidth: '100%' }}
            >
                <Form
                    onFinish={handleSubmit}
                >
                    <Collapse>
                        <Collapse.Panel
                            header="站点设置"
                            key="1">
                            <Form.Item
                                label="网站标题"
                                name="webTitle"
                                rules={[{ required: true, message: '请输入网站标题' }]}
                            >
                                <Input
                                    value={input.webTitle}
                                    onChange={(e) => {
                                        setInput({ ...input, webTitle: e.target.value });
                                    }}
                                />
                            </Form.Item>
                            <Form.Item
                                label="网站Logo地址"
                                name="webLogo"
                                rules={[{ required: true, message: '请输入网站Logo地址' }]}
                            >
                                <Input
                                    value={input.webLogo}
                                    onChange={(e) => {
                                        setInput({ ...input, webLogo: e.target.value });
                                    }}
                                />
                            </Form.Item>
                            <Form.Item
                                label="首页内容"
                                name="indexContent"
                                rules={[{ required: true, message: '请输入首页内容' }]}
                            >
                                <Input.TextArea
                                    value={input.indexContent}
                                    onChange={(e) => {
                                        setInput({ ...input, indexContent: e.target.value });
                                    }}
                                />
                            </Form.Item>
                            <Button block type="primary" htmlType="submit">保存设置</Button>
                        </Collapse.Panel>
                    </Collapse>
                </Form>
            </Card>
        </>
    );
}
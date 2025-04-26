import { useEffect, useState } from 'react';
import { Button, Card, Collapse, Form, Input, notification } from 'antd';
import { OtherSetting, UpdateSetting } from '../../../services/SettingService';
import { useTranslation } from 'react-i18next';

interface OtherSettingsProps {
    settings: any[];
    setSettings?: any;
}

export default function OtherSettings({
    settings,
    setSettings
}: OtherSettingsProps) {
    const [input, setInput] = useState<any>([]);
    const { t } = useTranslation();

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
                        message: t('settingPage.general.saveSuccess'),
                    });
                } else {
                    notification.error({
                        message: t('settingPage.general.saveFailed'),
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
            title={t('settingPage.general.title')}
            style={{ maxWidth: '100%' }}
        >
            <Form>
                <Collapse>
                    <Collapse.Panel
                        header={t('settingPage.other.siteSettings')}
                        key="1">
                        <Form.Item
                            label={t('settingPage.other.webTitle')}
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
                            label={t('settingPage.other.webLogo')}
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
                            label={t('settingPage.other.indexContent')}
                            rules={[{ required: true, message: '请输入首页内容' }]}
                        >
                            <Input.TextArea
                                value={input[OtherSetting.IndexContent]}
                                onChange={(e) => {
                                    handleInputChange(OtherSetting.IndexContent, e.target.value);
                                }}
                            />
                        </Form.Item>
                        <Button block type="primary" onClick={()=>handleSubmit()}>{t('settingPage.general.save')}</Button>
                    </Collapse.Panel>
                </Collapse>
            </Form>
        </Card>
    );
}
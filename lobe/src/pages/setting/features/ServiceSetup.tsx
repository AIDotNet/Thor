import { Button, Card, Collapse, Form, Input, InputNumber, Switch, notification } from "antd";
import { GeneralSetting, UpdateSetting } from "../../../services/SettingService";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";

interface ServiceSetupProps {
    settings: any[];
    setSettings?: any;
}

export default function ServiceSetup({
    settings,
    setSettings
}: ServiceSetupProps) {
    const [input, setInput] = useState<any>([]);
    const { t } = useTranslation();

    useEffect(() => {
        const initialValues = settings.reduce((acc, setting) => {
            acc[setting.key] = setting.value;
            return acc;
        }, {});
        setInput(initialValues);
    }, [settings])

    function handleSubmit() {
        UpdateSetting(settings)
            .then((res) => {
                res.success ? notification.success({
                    message: t('settingPage.general.saveSuccess'),
                }) : notification.error({
                    message: t('settingPage.general.saveFailed'),
                });
            });
    }

    const handleInputChange = (key: string, value: any) => {
        setInput((prevInput: any) => ({
            ...prevInput,
            [key]: value.toString()
        }));
        const setting = settings.find(s => s.key === key);
        if (setting) {
            setting.value = value.toString();
        }

        setSettings(settings);
    };

    return (
        <Card title={t('settingPage.general.title')} style={{ maxWidth: '100%' }}>
            <Form>
                <Collapse>
                    <Collapse.Panel key={1} header={t('settingPage.general.title')}>
                        <Form.Item label={t('settingPage.service.rechargeAddress')}>
                            <Input
                                value={input[GeneralSetting.RechargeAddress]}
                                onChange={(e) => handleInputChange(GeneralSetting.RechargeAddress, e.target.value)}
                                placeholder={t('settingPage.service.rechargeAddress')}
                            />
                        </Form.Item>
                        <Form.Item label={t('settingPage.service.chatLink')}>
                            <Input
                                value={input[GeneralSetting.ChatLink]}
                                onChange={(e) => handleInputChange(GeneralSetting.ChatLink, e.target.value)}
                                placeholder={t('settingPage.service.chatLink')}
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>

                <Collapse accordion>
                    <Collapse.Panel key={1} header={t('settingPage.service.quotaSettings')}>
                        <Form.Item label={t('settingPage.service.newUserQuota')}>
                            <InputNumber
                                value={input[GeneralSetting.NewUserQuota]}
                                onChange={(value) => handleInputChange(GeneralSetting.NewUserQuota, value)}
                                placeholder={t('settingPage.service.newUserQuota')}
                            />
                        </Form.Item>
                        <Form.Item label={t('settingPage.service.requestQuota')}>
                            <InputNumber
                                value={input[GeneralSetting.RequestQuota]}
                                onChange={(value) => handleInputChange(GeneralSetting.RequestQuota, value)}
                                placeholder={t('settingPage.service.requestQuota')}
                            />
                        </Form.Item>
                        <Form.Item label={t('settingPage.service.inviteQuota')}>
                            <InputNumber
                                value={input[GeneralSetting.InviteQuota]}
                                onChange={(value) => handleInputChange(GeneralSetting.InviteQuota, value)}
                                placeholder={t('settingPage.service.inviteQuota')}
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>

                <Collapse>
                    <Collapse.Panel key={1} header={t('settingPage.service.logSettings')}>
                        <Form.Item label={t('settingPage.service.enableClearLog')}>
                            <Switch
                                checked={input[GeneralSetting.EnableClearLog] === 'true'}
                                onChange={(value) => handleInputChange(GeneralSetting.EnableClearLog, value ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item label={t('settingPage.service.intervalDays')}>
                            <InputNumber
                                value={input[GeneralSetting.IntervalDays]}
                                onChange={(value) => handleInputChange(GeneralSetting.IntervalDays, value)}
                                placeholder={t('settingPage.service.intervalDays')}
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>

                <Collapse>
                    <Collapse.Panel key={1} header={t('settingPage.service.channelMonitoring')}>
                        <Form.Item label={t('settingPage.service.enableAutoCheckChannel')}>
                            <Switch
                                checked={input[GeneralSetting.EnableAutoCheckChannel] === 'true'}
                                onChange={(value) => handleInputChange(GeneralSetting.EnableAutoCheckChannel, value ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item label={t('settingPage.service.checkInterval')}>
                            <InputNumber
                                value={input[GeneralSetting.CheckInterval]}
                                onChange={(value) => handleInputChange(GeneralSetting.CheckInterval, value)}
                                placeholder={t('settingPage.service.checkInterval')}
                            />
                        </Form.Item>
                        <Form.Item label={t('settingPage.service.autoDisableChannel')}>
                            <Switch
                                checked={input[GeneralSetting.AutoDisableChannel] === 'true'}
                                onChange={(value) => handleInputChange(GeneralSetting.AutoDisableChannel, value ? 'true' : 'false')}
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>

                <Collapse>
                    <Collapse.Panel key={1} header={t('settingPage.service.alipaySettings')}>
                        <Form.Item label={t('settingPage.service.alipayNotifyUrl')}>
                            <Input
                                value={input[GeneralSetting.AlipayNotifyUrl]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayNotifyUrl, e.target.value)}
                                placeholder={t('settingPage.service.alipayNotifyUrl')}
                            />
                        </Form.Item>
                        <Form.Item label={t('settingPage.service.alipayAppId')}>
                            <Input
                                value={input[GeneralSetting.AlipayAppId]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayAppId, e.target.value)}
                                placeholder={t('settingPage.service.alipayAppId')}
                            />
                        </Form.Item>
                        <Form.Item label={t('settingPage.service.alipayPrivateKey')}>
                            <Input
                                value={input[GeneralSetting.AlipayPrivateKey]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayPrivateKey, e.target.value)}
                                placeholder={t('settingPage.service.alipayPrivateKey')}
                            />
                        </Form.Item>
                        <Form.Item label={t('settingPage.service.alipayPublicKey')}>
                            <Input
                                value={input[GeneralSetting.AlipayPublicKey]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayPublicKey, e.target.value)}
                                placeholder={t('settingPage.service.alipayPublicKey')}
                            />
                        </Form.Item>
                        <Form.Item label={t('settingPage.service.alipayAppCertPath')}>
                            <Input
                                value={input[GeneralSetting.AlipayAppCertPath]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayAppCertPath, e.target.value)}
                                placeholder={t('settingPage.service.alipayAppCertPath')}
                            />
                        </Form.Item>
                        <Form.Item label={t('settingPage.service.alipayRootCertPath')}>
                            <Input
                                value={input[GeneralSetting.AlipayRootCertPath]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayRootCertPath, e.target.value)}
                                placeholder={t('settingPage.service.alipayRootCertPath')}
                            />
                        </Form.Item>
                        <Form.Item label={t('settingPage.service.alipayPublicCertPath')}>
                            <Input
                                value={input[GeneralSetting.AlipayPublicCertPath]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayPublicCertPath, e.target.value)}
                                placeholder={t('settingPage.service.alipayPublicCertPath')}
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>

                <Button block onClick={handleSubmit} type="primary">
                    {t('settingPage.general.save')}
                </Button>
            </Form>
        </Card>
    );
}
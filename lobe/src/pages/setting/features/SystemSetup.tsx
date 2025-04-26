import { useState, useEffect } from 'react';
import { Button, Card, Collapse, Divider, Form, Input, Switch, notification } from 'antd';
import { SystemSetting, UpdateSetting } from '../../../services/SettingService';
import { useTranslation } from 'react-i18next';

interface SystemSetupProps {
    settings: any[];
    setSettings?: any;
}

export default function SystemSetup({
    settings,
    setSettings
}: SystemSetupProps) {
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
            <Form
                initialValues={input}
            >
                <Collapse>
                    <Collapse.Panel key='1' header={t('settingPage.general.title')}>
                        <Form.Item
                            label={t('settingPage.system.serverAddress')}
                            rules={[{ required: true, message: '服务器地址不能为空' }]}
                        >
                            <Input
                                value={input[SystemSetting.ServerAddress]}
                                onChange={(e) => handleInputChange(SystemSetting.ServerAddress, e.target.value)}
                                placeholder={t('settingPage.system.serverAddress')}
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>
                <Collapse>
                    <Collapse.Panel key='2' header={t('settingPage.system.accountSettings')}>
                        <Form.Item
                            label={t('settingPage.system.enableRegister')}
                            valuePropName="checked"
                        >
                            <Switch
                                checked={input[SystemSetting.EnableRegister] === 'true'}
                                onChange={(checked) => handleInputChange(SystemSetting.EnableRegister, checked ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Divider>{t('settingPage.system.gitHubSettings')}</Divider>
                        <Form.Item
                            label={t('settingPage.system.enableGithubLogin')}
                            valuePropName="checked"
                        >
                            <Switch
                                checked={input[SystemSetting.EnableGithubLogin] === 'true'}
                                onChange={(checked) => handleInputChange(SystemSetting.EnableGithubLogin, checked ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item
                            label={t('settingPage.system.githubClientId')}
                        >
                            <Input
                                value={input[SystemSetting.GithubClientId]}
                                onChange={(e) => handleInputChange(SystemSetting.GithubClientId, e.target.value)}
                                placeholder={t('settingPage.system.githubClientId')}
                            />
                        </Form.Item>
                        <Form.Item
                            label={t('settingPage.system.githubClientSecret')}
                        >
                            <Input
                                value={input[SystemSetting.GithubClientSecret]}
                                onChange={(e) => handleInputChange(SystemSetting.GithubClientSecret, e.target.value)}
                                placeholder={t('settingPage.system.githubClientSecret')}
                            />
                        </Form.Item>
                        <Divider>{t('settingPage.system.giteeSettings')}</Divider>
                        <Form.Item
                            label={t('settingPage.system.enableGiteeLogin')}
                            valuePropName="checked"
                        >
                            <Switch
                                checked={input[SystemSetting.EnableGiteeLogin] === 'true'}
                                onChange={(checked) => handleInputChange(SystemSetting.EnableGiteeLogin, checked ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item
                            label={t('settingPage.system.giteeClientId')}
                        >
                            <Input
                                value={input[SystemSetting.GiteeClientId]}
                                onChange={(e) => handleInputChange(SystemSetting.GiteeClientId, e.target.value)}
                                placeholder={t('settingPage.system.giteeClientId')}
                            />
                        </Form.Item>
                        <Form.Item
                            label={t('settingPage.system.giteeClientSecret')}
                        >
                            <Input
                                value={input[SystemSetting.GiteeClientSecret]}
                                onChange={(e) => handleInputChange(SystemSetting.GiteeClientSecret, e.target.value)}
                                placeholder={t('settingPage.system.giteeClientSecret')}
                            />
                        </Form.Item>
                        <Divider>{t('settingPage.system.emailSettings')}</Divider>
                        <Form.Item
                            label={t('settingPage.system.enableEmailRegister')}
                            valuePropName="checked"
                        >
                            <Switch
                                checked={input[SystemSetting.EnableEmailRegister] === 'true'}
                                onChange={(checked) => handleInputChange(SystemSetting.EnableEmailRegister, checked ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item
                            label={t('settingPage.system.emailAddress')}
                        >
                            <Input
                                value={input[SystemSetting.EmailAddress]}
                                onChange={(e) => handleInputChange(SystemSetting.EmailAddress, e.target.value)}
                                placeholder={t('settingPage.system.emailAddress')}
                            />
                        </Form.Item>
                        <Form.Item
                            label={t('settingPage.system.emailPassword')}
                        >
                            <Input
                                value={input[SystemSetting.EmailPassword]}
                                onChange={(e) => handleInputChange(SystemSetting.EmailPassword, e.target.value)}
                                placeholder={t('settingPage.system.emailPassword')}
                            />
                        </Form.Item>
                        <Form.Item
                            label={t('settingPage.system.smtpAddress')}
                        >
                            <Input
                                value={input[SystemSetting.SmtpAddress]}
                                onChange={(e) => handleInputChange(SystemSetting.SmtpAddress, e.target.value)}
                                placeholder={t('settingPage.system.smtpAddress')}
                            />
                        </Form.Item>

                        <Divider>{t('settingPage.system.casdoorSettings')}</Divider>
                        
                        <Form.Item
                            label={t('settingPage.system.enableCasdoorAuth')}
                            valuePropName="checked"
                        >
                            <Switch
                                checked={input[SystemSetting.EnableCasdoorAuth] === 'true'}
                                onChange={(checked) => handleInputChange(SystemSetting.EnableCasdoorAuth, checked ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item
                            label={t('settingPage.system.casdoorEndpoint')}
                        >
                            <Input
                                value={input[SystemSetting.CasdoorEndipoint]}
                                onChange={(e) => handleInputChange(SystemSetting.CasdoorEndipoint, e.target.value)}
                                placeholder={t('settingPage.system.casdoorEndpoint')}
                            />
                        </Form.Item>
                        <Form.Item
                            label={t('settingPage.system.casdoorClientId')}
                        >
                            <Input
                                value={input[SystemSetting.CasdoorClientId]}
                                onChange={(e) => handleInputChange(SystemSetting.CasdoorClientId, e.target.value)}
                                placeholder={t('settingPage.system.casdoorClientId')}
                            />
                        </Form.Item>
                        <Form.Item
                            label={t('settingPage.system.casdoorClientSecret')}
                        >
                            <Input
                                value={input[SystemSetting.CasdoorClientSecret]}
                                onChange={(e) => handleInputChange(SystemSetting.CasdoorClientSecret, e.target.value)}
                                placeholder={t('settingPage.system.casdoorClientSecret')}
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>
                <Form.Item>
                    <Button type="primary" onClick={()=>{
                        handleSubmit()
                    }} block>{t('settingPage.general.save')}</Button>
                </Form.Item>
            </Form>
        </Card>
    );
}
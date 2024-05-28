import { useState, useEffect } from 'react';
import { Button, Card, Collapse, Form, Input, Switch, notification } from 'antd';
import { SystemSetting, UpdateSetting } from '../../../services/SettingService';

interface SystemSetupProps {
    settings: any[];
    setSettings?: any;
}

export default function SystemSetup({
    settings,
    setSettings
}: SystemSetupProps) {
    const [input, setInput] = useState<any>({});

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
                    message: '修改成功',
                }) : notification.error({
                    message: '修改失败',
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
        setSettings([...settings]);
    };

    return (
        <Card
            title='通用设置'
            style={{ maxWidth: '100%' }}
            extra={
                <Button type='primary'>立即使用</Button>
            }
        >
            <Form
                onFinish={handleSubmit}
                initialValues={input}
            >
                <Collapse>
                    <Collapse.Panel key='1' header='通用设置'>
                        <Form.Item
                            name={SystemSetting.ServerAddress}
                            label='服务器地址'
                            rules={[{ required: true, message: '服务器地址不能为空' }]}
                        >
                            <Input
                                value={input[SystemSetting.ServerAddress]}
                                onChange={(e) => handleInputChange(SystemSetting.ServerAddress, e.target.value)}
                                placeholder='请输入服务器地址'
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>
                <Collapse>
                    <Collapse.Panel key='2' header='账号设置'>
                        <Form.Item
                            name={SystemSetting.EnableRegister}
                            label='启用账号注册'
                            valuePropName="checked"
                        >
                            <Switch
                                checked={input[SystemSetting.EnableRegister] === 'true'}
                                onChange={(checked) => handleInputChange(SystemSetting.EnableRegister, checked ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item
                            name={SystemSetting.EnableGithubLogin}
                            label='允许Github登录'
                            valuePropName="checked"
                        >
                            <Switch
                                checked={input[SystemSetting.EnableGithubLogin] === 'true'}
                                onChange={(checked) => handleInputChange(SystemSetting.EnableGithubLogin, checked ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item
                            name={SystemSetting.GithubClientId}
                            label='Github Client Id'
                        >
                            <Input
                                value={input[SystemSetting.GithubClientId]}
                                onChange={(e) => handleInputChange(SystemSetting.GithubClientId, e.target.value)}
                                placeholder='请输入Github Client Id'
                            />
                        </Form.Item>
                        <Form.Item
                            name={SystemSetting.GithubClientSecret}
                            label='Github Client Secret'
                        >
                            <Input
                                value={input[SystemSetting.GithubClientSecret]}
                                onChange={(e) => handleInputChange(SystemSetting.GithubClientSecret, e.target.value)}
                                placeholder='请输入Github Client Secret'
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>
                <Form.Item>
                    <Button type="primary" htmlType="submit" block>保存设置</Button>
                </Form.Item>
            </Form>
        </Card>
    );
}
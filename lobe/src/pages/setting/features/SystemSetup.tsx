import { useState, useEffect } from 'react';
import { Button, Card, Collapse, Divider, Form, Input, Switch, notification } from 'antd';
import { SystemSetting, UpdateSetting } from '../../../services/SettingService';

interface SystemSetupProps {
    settings: any[];
    setSettings?: any;
}

export default function SystemSetup({
    settings,
    setSettings
}: SystemSetupProps) {
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

        setSettings(settings);
    };

    return (
        <Card
            title='通用设置'
            style={{ maxWidth: '100%' }}
        >
            <Form
                initialValues={input}
            >
                <Collapse>
                    <Collapse.Panel key='1' header='通用设置'>
                        <Form.Item
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
                            label='启用账号注册'
                            valuePropName="checked"
                        >
                            <Switch
                                checked={input[SystemSetting.EnableRegister] === 'true'}
                                onChange={(checked) => handleInputChange(SystemSetting.EnableRegister, checked ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Divider>如果允许Github登录，请填写以下信息</Divider>
                        <Form.Item
                            label='允许Github登录'
                            valuePropName="checked"
                        >
                            <Switch
                                checked={input[SystemSetting.EnableGithubLogin] === 'true'}
                                onChange={(checked) => handleInputChange(SystemSetting.EnableGithubLogin, checked ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item
                            label='Github Client Id'
                        >
                            <Input
                                value={input[SystemSetting.GithubClientId]}
                                onChange={(e) => handleInputChange(SystemSetting.GithubClientId, e.target.value)}
                                placeholder='请输入Github Client Id'
                            />
                        </Form.Item>
                        <Form.Item
                            label='Github Client Secret'
                        >
                            <Input
                                value={input[SystemSetting.GithubClientSecret]}
                                onChange={(e) => handleInputChange(SystemSetting.GithubClientSecret, e.target.value)}
                                placeholder='请输入Github Client Secret'
                            />
                        </Form.Item>
                        <Divider>如果允许Gitee登录，请填写以下信息</Divider>
                        <Form.Item
                            label='允许Gitee登录'
                            valuePropName="checked"
                        >
                            <Switch
                                checked={input[SystemSetting.EnableGiteeLogin] === 'true'}
                                onChange={(checked) => handleInputChange(SystemSetting.EnableGiteeLogin, checked ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item
                            label='Gitee Client Id'
                        >
                            <Input
                                value={input[SystemSetting.GiteeClientId]}
                                onChange={(e) => handleInputChange(SystemSetting.GiteeClientId, e.target.value)}
                                placeholder='请输入Gitee Client Id'
                            />
                        </Form.Item>
                        <Form.Item
                            label='Gitee Client Secret'
                        >
                            <Input
                                value={input[SystemSetting.GiteeClientSecret]}
                                onChange={(e) => handleInputChange(SystemSetting.GiteeClientSecret, e.target.value)}
                                placeholder='请输入Gitee Client Secret'
                            />
                        </Form.Item>
                        <Divider>如果启用邮箱注册验证，请填写以下信息</Divider>
                        <Form.Item
                            label='是否启用邮箱注册验证'
                            valuePropName="checked"
                        >
                            <Switch
                                checked={input[SystemSetting.EnableEmailRegister] === 'true'}
                                onChange={(checked) => handleInputChange(SystemSetting.EnableEmailRegister, checked ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item
                            label='发送邮箱'
                        >
                            <Input
                                value={input[SystemSetting.EmailAddress]}
                                onChange={(e) => handleInputChange(SystemSetting.EmailAddress, e.target.value)}
                                placeholder='请输入发送邮箱'
                            />
                        </Form.Item>
                        <Form.Item
                            label='发送邮箱密码'
                        >
                            <Input
                                value={input[SystemSetting.EmailPassword]}
                                onChange={(e) => handleInputChange(SystemSetting.EmailPassword, e.target.value)}
                                placeholder='请输入发送邮箱密码'
                            />
                        </Form.Item>
                        <Form.Item
                            label='SMTP地址'
                        >
                            <Input
                                value={input[SystemSetting.SmtpAddress]}
                                onChange={(e) => handleInputChange(SystemSetting.SmtpAddress, e.target.value)}
                                placeholder='SMTP地址'
                            />
                        </Form.Item>

                        <Divider>如果启用Casdoor授权，请填写以下信息</Divider>
                        
                        <Form.Item
                            label='是否启用Casdoor授权'
                            valuePropName="checked"
                        >
                            <Switch
                                checked={input[SystemSetting.EnableCasdoorAuth] === 'true'}
                                onChange={(checked) => handleInputChange(SystemSetting.EnableCasdoorAuth, checked ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item
                            label='Casdoor 自定义端点'
                        >
                            <Input
                                value={input[SystemSetting.CasdoorEndipoint]}
                                onChange={(e) => handleInputChange(SystemSetting.CasdoorEndipoint, e.target.value)}
                                placeholder='请输入Casdoor 自定义端点'
                            />
                        </Form.Item>
                        <Form.Item
                            label='Casdoor Client Id'
                        >
                            <Input
                                value={input[SystemSetting.CasdoorClientId]}
                                onChange={(e) => handleInputChange(SystemSetting.CasdoorClientId, e.target.value)}
                                placeholder='请输入Casdoor Client Id'
                            />
                        </Form.Item>
                        <Form.Item
                            label='Casdoor Client Secret'
                        >
                            <Input
                                value={input[SystemSetting.CasdoorClientSecret]}
                                onChange={(e) => handleInputChange(SystemSetting.CasdoorClientSecret, e.target.value)}
                                placeholder='Casdoor Client Secret'
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>
                <Form.Item>
                    <Button type="primary" onClick={()=>{
                        handleSubmit()
                    }} block>保存设置</Button>
                </Form.Item>
            </Form>
        </Card>
    );
}
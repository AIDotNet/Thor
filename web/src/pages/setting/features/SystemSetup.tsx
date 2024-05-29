import { Button, Card, Collapse, Form, Notification } from '@douyinfe/semi-ui';
import { SystemSetting, UpdateSetting } from '../../../services/SettingService';
import { Setting } from '../../..';


interface SystemSetupProps {
    settings: Setting[];
    setSettings?: any;
}

export default function SystemSetup({
    settings,
    setSettings
}: SystemSetupProps) {

    function handleSubmit() {
        UpdateSetting(settings)
            .then((res) => {
                res.success ? Notification.success({
                    title: '修改成功',
                }) : Notification.error({
                    title: '修改失败',
                });
            });
    }

    return (
        <>
            <Card
                title='通用设置'
                style={{ maxWidth: '100%' }}
                headerExtraContent={
                    <Button type='primary'>立即使用</Button>
                }
            >
                <Form
                >
                    <Collapse>
                        <Collapse.Panel itemKey='1' header='通用设置'>
                            <Form.Input
                                field={SystemSetting.ServerAddress}
                                initValue={settings?.find(s => s.key === SystemSetting.ServerAddress)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let serviceAddress = settings?.find(s => s.key === SystemSetting.ServerAddress);
                                    if (serviceAddress) {
                                        serviceAddress.value = value;
                                    }
                                    setSettings(settings);
                                }}
                                label='服务器地址'
                                placeholder='请输入服务器地址'
                                rules={[{ required: true, message: '服务器地址不能为空' }]}
                            />
                            <Form.Input
                                field={SystemSetting.Theme}
                                initValue={settings?.find(s => s.key === SystemSetting.Theme)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let serviceAddress = settings?.find(s => s.key === SystemSetting.Theme);
                                    if (serviceAddress) {
                                        serviceAddress.value = value;
                                    }
                                    setSettings(settings);
                                }}
                                label='服务器主题'
                                placeholder='请输入服务器主题（重启服务生效）'
                                rules={[{ required: true, message: '请输入服务器主题' }]}
                            />
                        </Collapse.Panel>
                    </Collapse>
                    <Collapse>
                        <Collapse.Panel itemKey='1' header='账号设置'>
                            <Form.Switch
                                field={SystemSetting.EnableRegister}
                                initValue={settings?.find(s => s.key === SystemSetting.EnableRegister)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let enableRegister = settings?.find(s => s.key === SystemSetting.EnableRegister);
                                    if (enableRegister) {
                                        enableRegister.value = value === true ? 'true' : 'false';
                                    }
                                    setSettings(settings);
                                }}
                                label='启用账号注册'
                            />
                            <Form.Switch
                                field={SystemSetting.EnableGithubLogin}
                                initValue={settings?.find(s => s.key === SystemSetting.EnableGithubLogin)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let enableGithubLogin = settings?.find(s => s.key === SystemSetting.EnableGithubLogin);
                                    if (enableGithubLogin) {
                                        enableGithubLogin.value = value === true ? 'true' : 'false';
                                    }
                                    setSettings(settings);
                                }}
                                label='允许Github登录'
                            />
                            <Form.Input
                                field={SystemSetting.GithubClientId}
                                initValue={settings?.find(s => s.key === SystemSetting.GithubClientId)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let githubClientId = settings?.find(s => s.key === SystemSetting.GithubClientId);
                                    if (githubClientId) {
                                        githubClientId.value = value;
                                    }
                                    setSettings(settings);
                                }}

                                label='Github Client Id'
                                placeholder='请输入Github Client Id'
                            />
                            <Form.Input
                                field={SystemSetting.GithubClientSecret}
                                initValue={settings?.find(s => s.key === SystemSetting.GithubClientSecret)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let githubClientSecret = settings?.find(s => s.key === SystemSetting.GithubClientSecret);
                                    if (githubClientSecret) {
                                        githubClientSecret.value = value;
                                    }
                                    setSettings(settings);
                                }}
                                label='Github Client Secret'
                                placeholder='请输入Github Client Secret'
                            />
                        </Collapse.Panel>
                    </Collapse>
                    <Button block onClick={() => handleSubmit()} type="primary">保存设置</Button>
                </Form>
            </Card>
        </>
    );
}
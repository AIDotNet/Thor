import { Button, Card, Collapse, Form, Notification } from '@douyinfe/semi-ui';
import { OtherSetting, UpdateSetting } from '../../../services/SettingService';
import { Setting } from '../../..';

interface OtherSettingsProps {
    settings: Setting[];
    setSettings?: any;
}

export default function OtherSettings({
    settings,
    setSettings
}: OtherSettingsProps) {

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
                title="通用设置"
                style={{ maxWidth: '100%' }}
            >
                <Form
                >
                    <Collapse>
                        <Collapse.Panel
                            header="站点设置"
                            itemKey="1">
                            <Form.Input
                                field={OtherSetting.WebTitle}
                                defaultValue={settings?.find(s => s.key === OtherSetting.WebTitle)?.value}
                                initValue={settings?.find(s => s.key === OtherSetting.WebTitle)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let webTitle = settings?.find(s => s.key === OtherSetting.WebTitle);
                                    if (webTitle) {
                                        webTitle.value = value;
                                    }
                                    setSettings(settings);
                                }}
                                label="网站标题"
                                rules={[{ required: true, message: '请输入网站标题' }]}
                            />
                            <Form.Input
                                field={OtherSetting.WebLogo}
                                initValue={settings?.find(s => s.key === OtherSetting.WebLogo)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let webLogo = settings?.find(s => s.key === OtherSetting.WebLogo);
                                    if (webLogo) {
                                        webLogo.value = value;
                                    }
                                    setSettings(settings);
                                }}
                                label="网站Logo地址"
                                rules={[{ required: true, message: '请输入网站Logo地址' }]}
                            />
                            <Form.TextArea
                                field={OtherSetting.IndexContent}
                                initValue={settings?.find(s => s.key === OtherSetting.IndexContent)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let indexContent = settings?.find(s => s.key === OtherSetting.IndexContent);
                                    if (indexContent) {
                                        indexContent.value = value;
                                    }
                                    setSettings(settings);
                                }}
                                label="首页内容"
                                rules={[{ required: true, message: '请输入首页内容' }]}
                            />
                            <Button block onClick={() => handleSubmit()} type="primary">保存设置</Button>
                        </Collapse.Panel>
                    </Collapse>
                </Form>
            </Card>
        </>
    );
}
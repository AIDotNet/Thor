import { Button, Card, Collapse, Form, Notification } from "@douyinfe/semi-ui";
import { GeneralSetting, UpdateSetting } from "../../../services/SettingService";
import { Setting } from "../../..";


interface ServiceSetupProps {
    settings: Setting[];
    setSettings?: any;
}

export default function ServiceSetup({
    settings,
    setSettings
}: ServiceSetupProps) {

    function handleSubmit() {
        // 校验 模型倍率Prompt 和 模型倍率Completion
        let modelPromptRate = settings?.find(s => s.key === GeneralSetting.ModelPromptRate);
        let modelCompletionRate = settings?.find(s => s.key === GeneralSetting.ModelCompletionRate);

        if (modelPromptRate) {
            try {
                JSON.parse(modelPromptRate.value);
            } catch (e) {
                return Notification.error({
                    title: '模型倍率Prompt 格式错误',
                });
            }
        }

        if (modelCompletionRate) {
            try {
                JSON.parse(modelCompletionRate.value);
            } catch (e) {
                return Notification.error({
                    title: '模型倍率Completion 格式错误'
                });
            }
        }


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
            >
                <Form >
                    <Collapse>
                        <Collapse.Panel itemKey="1" header="通用设置">
                            <Form.Input
                                field={GeneralSetting.RechargeAddress}
                                initValue={settings?.find(s => s.key === GeneralSetting.RechargeAddress)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let rechargeAddress = settings?.find(s => s.key === GeneralSetting.RechargeAddress);
                                    if (rechargeAddress) {
                                        rechargeAddress.value = value;
                                    }
                                    setSettings(settings);
                                }}
                                label='充值地址'
                                style={{
                                    marginBottom: 16,
                                    width: '100%',
                                }}
                                placeholder='请输入充值地址'
                            />
                            <Form.Input
                                field={GeneralSetting.ChatLink}
                                label='对话链接'
                                initValue={settings?.find(s => s.key === GeneralSetting.ChatLink)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let chatLink = settings?.find(s => s.key === GeneralSetting.ChatLink);
                                    if (chatLink) {
                                        chatLink.value = value;
                                    }
                                    setSettings(settings);
                                }}
                                style={{
                                    marginBottom: 16,
                                    width: '100%',
                                }}
                                placeholder='请输入对话链接'
                            />
                        </Collapse.Panel>
                    </Collapse>

                    <Collapse
                        accordion
                    >
                        <Collapse.Panel
                            header="额度设置"
                            itemKey="1">
                            <Form.InputNumber
                                field={GeneralSetting.NewUserQuota}
                                initValue={settings?.find(s => s.key === GeneralSetting.NewUserQuota)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let newUserQuota = settings?.find(s => s.key === GeneralSetting.NewUserQuota);
                                    if (newUserQuota) {
                                        newUserQuota.value = value.toString();
                                    }
                                    setSettings(settings);
                                }}
                                label='新用户初始额度'
                                style={{
                                    marginBottom: 16,
                                    width: '100%',
                                }}
                                placeholder='请输入新用户初始额度'
                            />
                            <Form.InputNumber
                                field={GeneralSetting.RequestQuota}
                                initValue={settings?.find(s => s.key === GeneralSetting.RequestQuota)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let requestQuota = settings?.find(s => s.key === GeneralSetting.RequestQuota);
                                    if (requestQuota) {
                                        requestQuota.value = value.toString();
                                    }
                                    setSettings(settings);
                                }}
                                label='请求预扣额度'
                                style={{
                                    marginBottom: 16,
                                    width: '100%',
                                }}
                                placeholder='请输入请求预扣额度'
                            />
                            <Form.InputNumber
                                field={GeneralSetting.InviteQuota}
                                initValue={settings?.find(s => s.key === GeneralSetting.InviteQuota)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let inviteQuota = settings?.find(s => s.key === GeneralSetting.InviteQuota);
                                    if (inviteQuota) {
                                        inviteQuota.value = value.toString();
                                    }
                                    setSettings(settings);
                                }}
                                label='邀请奖励额度'
                                style={{
                                    marginBottom: 16,
                                    width: '100%',
                                }}
                                placeholder='请输入邀请奖励额度'
                            />
                        </Collapse.Panel>
                    </Collapse>

                    <Collapse>
                        <Collapse.Panel itemKey="1" header="日志设置">
                            <Form.Switch
                                field={GeneralSetting.EnableClearLog}
                                initValue={settings?.find(s => s.key === GeneralSetting.EnableClearLog)?.value === 'true'}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let enableClearLog = settings?.find(s => s.key === GeneralSetting.EnableClearLog);
                                    if (enableClearLog) {
                                        enableClearLog.value = value === true ? 'true' : 'false';
                                    }
                                    setSettings(settings);
                                }}
                                label='启用定时清理日志'
                            />
                            <Form.InputNumber
                                field={GeneralSetting.IntervalDays}
                                label='间隔天数'
                                initValue={settings?.find(s => s.key === GeneralSetting.IntervalDays)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let intervalDays = settings?.find(s => s.key === GeneralSetting.IntervalDays);
                                    if (intervalDays) {
                                        intervalDays.value = value.toString();
                                    }
                                    setSettings(settings);
                                }}
                                style={{
                                    marginBottom: 16,
                                    width: '100%',
                                }}
                                placeholder='请输入间隔天数'
                            />
                        </Collapse.Panel>
                    </Collapse>
                    <Collapse>
                        <Collapse.Panel itemKey="1" header="渠道监控">
                            <Form.Switch
                                field={GeneralSetting.EnableAutoCheckChannel}
                                initValue={settings?.find(s => s.key === GeneralSetting.EnableAutoCheckChannel)?.value === 'true'}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let enableAutoCheckChannel = settings?.find(s => s.key === GeneralSetting.EnableAutoCheckChannel);
                                    if (enableAutoCheckChannel) {
                                        enableAutoCheckChannel.value = value === true ? 'true' : 'false';
                                    }
                                    setSettings(settings);
                                }}
                                label='启用自动检测渠道策略'
                            />
                            <Form.InputNumber
                                field={GeneralSetting.CheckInterval}
                                label='检测间隔 (分钟)'
                                initValue={settings?.find(s => s.key === GeneralSetting.CheckInterval)?.value}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let checkInterval = settings?.find(s => s.key === GeneralSetting.CheckInterval);
                                    if (checkInterval) {
                                        checkInterval.value = value.toString();
                                    }
                                    setSettings(settings);
                                }}
                                style={{
                                    marginBottom: 16,
                                    width: '100%',
                                }}
                                placeholder='请输入检测间隔'
                            />
                            <Form.Switch
                                field={GeneralSetting.AutoDisableChannel}
                                initValue={settings?.find(s => s.key === GeneralSetting.AutoDisableChannel)?.value === 'true'}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let autoDisableChannel = settings?.find(s => s.key === GeneralSetting.AutoDisableChannel);
                                    if (autoDisableChannel) {
                                        autoDisableChannel.value = value === true ? 'true' : 'false';
                                    }
                                    setSettings(settings);
                                }}
                                label='自动禁用异常渠道'
                            />
                        </Collapse.Panel>
                    </Collapse>
                    <Collapse>
                        <Collapse.Panel itemKey="1" header="倍率设置">
                            <Form.TextArea
                                rows={30}
                                field={GeneralSetting.ModelPromptRate}
                                initValue={() => {
                                    var v = settings?.find(s => s.key === GeneralSetting.ModelPromptRate)?.value;
                                    if (v) {
                                        return JSON.stringify(JSON.parse(v), null, 4);
                                    }
                                }}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let modelPromptRate = settings?.find(s => s.key === GeneralSetting.ModelPromptRate);
                                    if (modelPromptRate) {
                                        modelPromptRate.value = value;
                                    }
                                    setSettings(settings);
                                }}
                                label='模型倍率Prompt'
                                style={{
                                    marginBottom: 16,
                                    width: '100%',
                                }}
                                placeholder='请输入模型倍率Prompt'
                            />
                            <Form.TextArea
                                field={GeneralSetting.ModelCompletionRate}
                                rows={30}
                                initValue={() => {
                                    var v = settings?.find(s => s.key === GeneralSetting.ModelCompletionRate)?.value;
                                    if (v) {
                                        return JSON.stringify(JSON.parse(v), null, 4);
                                    }
                                }}
                                onChange={(value) => {
                                    // 更新指定的设置
                                    let modelCompletionRate = settings?.find(s => s.key === GeneralSetting.ModelCompletionRate);
                                    if (modelCompletionRate) {
                                        modelCompletionRate.value = value;
                                    }
                                    setSettings(settings);
                                }}
                                label='模型倍率Completion'
                                style={{
                                    marginBottom: 16,
                                    width: '100%',
                                }}
                                placeholder='请输入模型倍率Completion'
                            />
                        </Collapse.Panel>
                    </Collapse>
                    <Button block onClick={() => { handleSubmit() }} type="primary">保存设置</Button>
                </Form>
            </Card>
        </>
    );
}
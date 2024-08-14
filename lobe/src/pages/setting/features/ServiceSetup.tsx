import { Button, Card, Collapse, Form, Input, InputNumber, Switch, notification } from "antd";
import { GeneralSetting, UpdateSetting } from "../../../services/SettingService";
import { useEffect, useState } from "react";

interface ServiceSetupProps {
    settings: any[];
    setSettings?: any;
}

export default function ServiceSetup({
    settings,
    setSettings
}: ServiceSetupProps) {
    const [input, setInput] = useState<any>([]);

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
                    message: '修改成功',
                }) : notification.error({
                    message: '修改失败',
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
        <Card title='通用设置' style={{ maxWidth: '100%' }}>
            <Form>
                <Collapse>
                    <Collapse.Panel key={1} header="通用设置">
                        <Form.Item label='充值地址'>
                            <Input
                                value={input[GeneralSetting.RechargeAddress]}
                                onChange={(e) => handleInputChange(GeneralSetting.RechargeAddress, e.target.value)}
                                placeholder='请输入充值地址'
                            />
                        </Form.Item>
                        <Form.Item label='对话链接'>
                            <Input
                                value={input[GeneralSetting.ChatLink]}
                                onChange={(e) => handleInputChange(GeneralSetting.ChatLink, e.target.value)}
                                placeholder='请输入对话链接'
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>

                <Collapse accordion>
                    <Collapse.Panel key={1} header="额度设置">
                        <Form.Item label='新用户初始额度'>
                            <InputNumber
                                value={input[GeneralSetting.NewUserQuota]}
                                onChange={(value) => handleInputChange(GeneralSetting.NewUserQuota, value)}
                                placeholder='请输入新用户初始额度'
                            />
                        </Form.Item>
                        <Form.Item label='请求预扣额度'>
                            <InputNumber
                                value={input[GeneralSetting.RequestQuota]}
                                onChange={(value) => handleInputChange(GeneralSetting.RequestQuota, value)}
                                placeholder='请输入请求预扣额度'
                            />
                        </Form.Item>
                        <Form.Item label='邀请奖励额度'>
                            <InputNumber
                                value={input[GeneralSetting.InviteQuota]}
                                onChange={(value) => handleInputChange(GeneralSetting.InviteQuota, value)}
                                placeholder='请输入邀请奖励额度'
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>

                <Collapse>
                    <Collapse.Panel key={1} header="日志设置">
                        <Form.Item label='启用定时清理日志'>
                            <Switch
                                checked={input[GeneralSetting.EnableClearLog] === 'true'}
                                onChange={(value) => handleInputChange(GeneralSetting.EnableClearLog, value ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item label='间隔天数'>
                            <InputNumber
                                value={input[GeneralSetting.IntervalDays]}
                                onChange={(value) => handleInputChange(GeneralSetting.IntervalDays, value)}
                                placeholder='请输入间隔天数'
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>

                <Collapse>
                    <Collapse.Panel key={1} header="渠道监控">
                        <Form.Item label='启用自动检测渠道策略'>
                            <Switch
                                checked={input[GeneralSetting.EnableAutoCheckChannel] === 'true'}
                                onChange={(value) => handleInputChange(GeneralSetting.EnableAutoCheckChannel, value ? 'true' : 'false')}
                            />
                        </Form.Item>
                        <Form.Item label='检测间隔 (分钟)'>
                            <InputNumber
                                value={input[GeneralSetting.CheckInterval]}
                                onChange={(value) => handleInputChange(GeneralSetting.CheckInterval, value)}
                                placeholder='请输入检测间隔'
                            />
                        </Form.Item>
                        <Form.Item label='自动禁用异常渠道'>
                            <Switch
                                checked={input[GeneralSetting.AutoDisableChannel] === 'true'}
                                onChange={(value) => handleInputChange(GeneralSetting.AutoDisableChannel, value ? 'true' : 'false')}
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>

                <Collapse>
                    <Collapse.Panel key={1} header="支付宝设置">
                        <Form.Item label='支付宝回调地址'>
                            <Input
                                value={input[GeneralSetting.AlipayNotifyUrl]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayNotifyUrl, e.target.value)}
                                placeholder='请输入支付宝回调地址'
                            />
                        </Form.Item>
                        <Form.Item label='支付宝AppId'>
                            <Input
                                value={input[GeneralSetting.AlipayAppId]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayAppId, e.target.value)}
                                placeholder='请输入支付宝AppId'
                            />
                        </Form.Item>
                        <Form.Item label='支付宝应用私钥'>
                            <Input
                                value={input[GeneralSetting.AlipayPrivateKey]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayPrivateKey, e.target.value)}
                                placeholder='请输入支付宝应用私钥'
                            />
                        </Form.Item>
                        <Form.Item label='支付宝公钥'>
                            <Input
                                value={input[GeneralSetting.AlipayPublicKey]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayPublicKey, e.target.value)}
                                placeholder='请输入支付宝公钥'
                            />
                        </Form.Item>
                        <Form.Item label='支付宝应用公钥证书路径'>
                            <Input
                                value={input[GeneralSetting.AlipayAppCertPath]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayAppCertPath, e.target.value)}
                                placeholder='请输入支付宝应用公钥证书路径'
                            />
                        </Form.Item>
                        <Form.Item label='支付宝根证书路径'>
                            <Input
                                value={input[GeneralSetting.AlipayRootCertPath]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayRootCertPath, e.target.value)}
                                placeholder='请输入支付宝根证书路径'
                            />
                        </Form.Item>
                        <Form.Item label='支付宝公钥证书路径'>
                            <Input
                                value={input[GeneralSetting.AlipayPublicCertPath]}
                                onChange={(e) => handleInputChange(GeneralSetting.AlipayPublicCertPath, e.target.value)}
                                placeholder='请输入支付宝公钥证书路径'
                            />
                        </Form.Item>
                    </Collapse.Panel>
                </Collapse>

                <Button block onClick={handleSubmit} type="primary">
                    保存设置
                </Button>
            </Form>
        </Card>
    );
}
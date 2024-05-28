import { Drawer, Divider, Form, Button, Switch, message, Input, DatePicker, InputNumber } from 'antd';
import { Add } from '../../../services/TokenService'
import { useState } from 'react';
import { renderQuota } from '../../../utils/render';

interface CreateTokenProps {
    onSuccess: () => void;
    visible: boolean;
    onCancel: () => void;
}

export default function CreateToken({
    onSuccess,
    visible,
    onCancel
}: CreateTokenProps) {

    type FieldType = {
        name?: string;
        unlimitedQuota: boolean;
        remainQuota?: number;
        unlimitedExpired: boolean;
        expiredTime?: Date;
    };

    const [input, setInput] = useState<FieldType>({
        name: '',
        unlimitedQuota: false,
        remainQuota: 0,
        unlimitedExpired: false,
        expiredTime: new Date(Date.now() + 15 * 24 * 60 * 60 * 1000)
    });

    function handleSubmit(values: any) {
        Add(values)
            .then((item) => {
                if (item.success) {
                    message.success({
                        content: '创建成功',
                    });
                    onSuccess();
                } else {
                    message.error({
                        content: item.message
                    });

                }
            });
    }

    return <Drawer
        width={500}
        
        title="创建Token" open={visible} onClose={onCancel}>
        <Form
         
        onFinish={values => handleSubmit(values)} style={{ width: 400 }}>
            <Form.Item<FieldType>
                label="Token名称"
                name="name"
                rules={[{ required: true, message: '请输入Token名称' }]}
            >
                <Input value={input.name} onChange={(v) => {
                    setInput({ ...input, name: v.target.value })
                }} />
            </Form.Item>
            {
                !input.unlimitedQuota && <Form.Item<FieldType>
                    label="额度"
                    name="remainQuota"
                    rules={[]}
                >
                    <InputNumber
                        addonAfter={renderQuota(input.remainQuota ?? 0, 6)} value={input.remainQuota} onChange={(v) => {
                            setInput({ ...input, remainQuota: v as any })
                        }} />
                </Form.Item>
            }
            <Form.Item<FieldType>
                label="无限额度"
                name="unlimitedQuota"
            >
                <Switch value={input.unlimitedQuota} onChange={(v) => {
                    setInput({ ...input, unlimitedQuota: v })
                }} />
            </Form.Item>
            {
                input.unlimitedExpired === false && <Form.Item<FieldType> label='过期时间' name={'expiredTime'}>
                    <DatePicker style={{
                        width: '100%'
                    }} value={input.expiredTime} onChange={(v) => {
                        setInput({ ...input, expiredTime: v as any })
                    }} />
                </Form.Item>
            }
            <Form.Item<FieldType>
                label="永不过期"
                name="unlimitedExpired"
            >
                <Switch value={input.unlimitedExpired} onChange={(v) => {
                    setInput({ ...input, unlimitedExpired: v })
                }} />
            </Form.Item>
            <Button type='primary' block htmlType='submit'>提交</Button>
        </Form>
    </Drawer>
}
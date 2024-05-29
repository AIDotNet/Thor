import { useState } from "react";
import { Button, Divider, Form, Input, Drawer, message, Tag } from 'antd';
import { Add } from "../../../services/RedeemCodeService";
import { renderQuota } from "../../../utils/render";

interface CreateRedeemCodeProps {
    onSuccess: () => void;
    visible: boolean;
    onCancel: () => void;
}

export default function CreateRedeemCode({
    onSuccess,
    visible,
    onCancel
}: CreateRedeemCodeProps) {

    const [input, setInput] = useState<any>({
        name: '',
        quota: 0,
        count: 1
    })

    function handleSubmit() {
        Add({
            name: input.name,
            quota: input.quota,
            count: input.count
        })
            .then((item) => {
                if (item.success) {
                    message.success('操作成功');

                    if (item.data.length > 1) {
                        // 将内容下载到本地
                        const blob = new Blob([item.data.join('\n')], { type: 'text/plain' });
                        const url = URL.createObjectURL(blob);
                        const a = document.createElement('a');
                        a.href = url;
                        a.download = '兑换码.txt';
                        a.click();
                        URL.revokeObjectURL(url);
                    }

                    onSuccess();
                } else {
                    message.error('操作失败');
                }
            })
            .catch(() => {
                message.error('操作失败');
            });
    }

    return (
        <Drawer
            title="创建Token"
            open={visible}
            onClose={onCancel}
            width={500}
        >
            <Divider />
            <Form onFinish={handleSubmit} style={{ width: 400 }}>

                <Form.Item
                    label="名称"
                    rules={[{ required: true, message: '名称不能为空' }]}
                >
                    <Input value={input.name}
                        onChange={(v) => {
                            setInput({ ...input, name: v.target.value })
                        }}
                        placeholder="请输入名称" />
                </Form.Item>
                <Form.Item
                    label="额度"
                    rules={[
                        { required: true, message: '额度不能为空' },
                        { type: 'number', min: 0, message: '额度不能小于0' }
                    ]}
                >
                    <Input
                        type="number"
                        placeholder="请输入额度"
                        value={input.quota}
                        onChange={(v) => {
                            setInput({ ...input, quota: v.target.value });
                        }}
                        suffix={<Tag color="green">{renderQuota(input.quota ?? 0, 6)}</Tag>}
                    />
                </Form.Item>
                <Form.Item
                    label="生成数量"
                    rules={[{ required: true, message: '生成数量不能为空' }]}
                    initialValue={1}
                >
                    <Input
                        value={input.count}
                        onChange={(v) => {
                            setInput({ ...input, count: v.target.value })
                        }}
                        type="number" />
                </Form.Item>
                <Button type="primary" block htmlType="submit">
                    提交
                </Button>
            </Form>
        </Drawer>
    );
}
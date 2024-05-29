import { useState, useEffect } from "react";
import { Button, Divider, Form, Input, Checkbox, DatePicker, Drawer } from "antd";

interface UpdateRedeemCodeProps {
    onSuccess: () => void;
    visible: boolean;
    onCancel: () => void;
    value?: any;
}

export default function UpdateRedeemCode({
    visible,
    onCancel,
    value
}: UpdateRedeemCodeProps) {
    const [input, setInput] = useState<any>({
        name: '',
        remainQuota: '',
        unlimitedQuota: false,
        expiredTime: null,
        unlimitedExpired: false,
    });

    useEffect(() => {
        setInput({
            name: value?.name,
            remainQuota: value?.remainQuota,
            unlimitedQuota: value?.unlimitedQuota,
            expiredTime: value?.expiredTime,
            unlimitedExpired: value?.unlimitedExpired,
        })
    }, [value]);

    function handleSubmit() {
        // Update(input)
        //     .then(() => {
        //         notification.success({
        //             message: '修改成功',
        //         });
        //         onSuccess();
        //     });
    }

    return (
        <Drawer
            title="修改Token"
            open={visible}
            onClose={onCancel}
            width={500}
        >
            <Divider />
            <Form
                initialValues={input}
                onFinish={handleSubmit}
                style={{ width: 400 }}
            >
                <Form.Item
                    label="Token名称"
                    rules={[
                        { required: true, message: 'Token名称不能为空' },
                        { min: 3, message: 'Token名称长度不能小于3' },
                    ]}
                >
                    <Input
                        value={input.name}
                        onChange={(e) => setInput({ ...input, name: e.target.value })}
                        placeholder="请输入token名称"
                    />
                </Form.Item>
                {
                    !input.unlimitedQuota && (
                        <Form.Item
                            label="额度"
                        >
                            <Input
                                type="number"
                                value={input.remainQuota}
                                onChange={(e) => setInput({ ...input, remainQuota: e.target.value })}
                                placeholder="请输入token额度"
                            />
                        </Form.Item>
                    )
                }
                <Form.Item
                    valuePropName="checked"
                >
                    <Checkbox
                        checked={input.unlimitedQuota}
                        onChange={(e) => setInput({ ...input, unlimitedQuota: e.target.checked })}
                    >
                        无限额度
                    </Checkbox>
                </Form.Item>
                {
                    !input.unlimitedExpired && (
                        <Form.Item
                            label="过期时间"
                        >
                            <DatePicker
                                value={input.expiredTime}
                                onChange={(date) => setInput({ ...input, expiredTime: date })}
                                style={{ width: '100%' }}
                            />
                        </Form.Item>
                    )
                }
                <Form.Item
                    valuePropName="checked"
                >
                    <Checkbox
                        checked={input.unlimitedExpired}
                        onChange={(e) => setInput({ ...input, unlimitedExpired: e.target.checked })}
                    >
                        永不过期
                    </Checkbox>
                </Form.Item>
                <Button type="primary" block htmlType="submit">
                    提交
                </Button>
            </Form>
        </Drawer>
    );
}
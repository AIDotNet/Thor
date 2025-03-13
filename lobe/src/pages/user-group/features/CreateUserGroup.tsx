import { Form, Input, InputNumber, Modal, Switch, message } from "antd";
import { useEffect } from "react";
import { create } from "../../../services/UserGroupService";

interface CreateUserGroupPageProps {
    open: boolean;
    onClose: () => void;
    onOk: () => void;
}

export default function CreateUserGroupPage(props: CreateUserGroupPageProps) {
    const [form] = Form.useForm();

    useEffect(() => {
        if (props.open) {
            form.resetFields();
        }
    }, [props.open]);

    const handleOk = async () => {
        try {
            const values = await form.validateFields();
            const res = await create(values);
            if (res.success) {
                message.success('创建成功');
                props.onOk();
            } else {
                message.error(res.message || '创建失败');
            }
        } catch (error) {
            console.error('表单验证失败:', error);
        }
    };

    return (
        <Modal
            title="创建用户分组"
            open={props.open}
            onCancel={props.onClose}
            onOk={handleOk}
            destroyOnClose
        >
            <Form
                form={form}
                layout="vertical"
                initialValues={{
                    enable: true,
                    rate: 1.0,
                    order: 0
                }}
            >
                <Form.Item
                    name="name"
                    label="分组名称"
                    rules={[{ required: true, message: '请输入分组名称' }]}
                >
                    <Input placeholder="请输入分组名称" />
                </Form.Item>

                <Form.Item
                    name="code"
                    label="唯一编码"
                    rules={[{ required: true, message: '请输入唯一编码' }]}
                >
                    <Input placeholder="请输入唯一编码" />
                </Form.Item>

                <Form.Item
                    name="description"
                    label="描述"
                    rules={[{ required: true, message: '请输入描述' }]}
                >
                    <Input.TextArea rows={3} placeholder="请输入描述" />
                </Form.Item>

                <Form.Item
                    name="rate"
                    label="分组倍率"
                    rules={[{ required: true, message: '请输入分组倍率' }]}
                >
                    <InputNumber min={0} step={0.1} style={{ width: '100%' }} />
                </Form.Item>

                <Form.Item
                    name="order"
                    label="排序"
                    rules={[{ required: true, message: '请输入排序' }]}
                >
                    <InputNumber min={0} style={{ width: '100%' }} />
                </Form.Item>

                <Form.Item
                    name="enable"
                    label="是否启用"
                    valuePropName="checked"
                >
                    <Switch />
                </Form.Item>
            </Form>
        </Modal>
    );
} 
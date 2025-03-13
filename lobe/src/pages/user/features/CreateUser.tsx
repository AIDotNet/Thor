import { useState, useEffect } from "react";
import { Button, Drawer, Form, Input, Select, message } from 'antd';
import { create } from "../../../services/UserService";
import { getList } from "../../../services/UserGroupService";

interface CreateUserProps {
    visible: boolean;
    onCancel: () => void;
    onSuccess: () => void;
}

export default function CreateUser({
    visible,
    onCancel,
    onSuccess
}: CreateUserProps) {
    const [input, setInput] = useState({
        userName: '',
        email: '',
        password: '',
        role: 'user',
        groups: []
    });
    const [groupOptions, setGroupOptions] = useState<any[]>([]);

    // 获取用户分组列表
    useEffect(() => {
        if (visible) {
            getList().then(res => {
                if (res.success) {
                    setGroupOptions(res.data || []);
                } else {
                    message.error('获取用户分组失败');
                }
            });
        }
    }, [visible]);

    function handleSubmit() {
        create(input)
            .then((res) => {
                if (res.success) {
                    message.success('创建成功');
                    onSuccess();
                } else {
                    message.error({
                        content: res.message
                    } as any);
                }
            })
    }

    useEffect(() => {
        if (!visible) {
            setInput({
                userName: '',
                email: '',
                password: '',
                role: 'user',
                groups: []
            });
        }
    }, [visible]);

    return (
        <Drawer
            open={visible}
            width={500}
            title="创建用户"
            onClose={onCancel}
        >
            <Form onFinish={handleSubmit} style={{ width: 400 }}>

                <Form.Item
                    label="用户名"
                    name="userName"
                    rules={[{ required: true, message: '请输入用户名' }]}
                >
                    <Input value={input.userName} onChange={(e) => setInput({ ...input, userName: e.target.value })} />
                </Form.Item>

                <Form.Item
                    label="邮箱"
                    name="email"
                    rules={[{ required: true, message: '请输入邮箱' }]}
                >
                    <Input value={input.email} onChange={(e) => setInput({ ...input, email: e.target.value })} />
                </Form.Item>

                <Form.Item
                    label="密码"
                    name="password"
                    rules={[{ required: true, message: '请输入密码' }]}
                >
                    <Input.Password value={input.password} onChange={(e) => setInput({ ...input, password: e.target.value })} />
                </Form.Item>

                <Form.Item
                    label="角色"
                    name="role"
                    rules={[{ required: true, message: '请选择角色' }]}
                >
                    <Select
                        value={input.role}
                        onChange={(value) => setInput({ ...input, role: value })}
                    >
                        <Select.Option value="user">用户</Select.Option>
                        <Select.Option value="admin">管理员</Select.Option>
                    </Select>
                </Form.Item>

                <Form.Item
                    label="组"
                    name="groups"
                    rules={[{ required: true, message: '请选择组' }]}
                >
                    <Select
                        mode="multiple"
                        value={input.groups}
                        onChange={(value) => setInput({ ...input, groups: value })}
                        placeholder="请选择用户组"
                        options={groupOptions.map(group => ({
                            label: group.name,
                            value: group.code
                        }))}
                    />
                </Form.Item>
                <Button type='primary' block htmlType='submit'>提交</Button>
            </Form>
        </Drawer>
    )
}
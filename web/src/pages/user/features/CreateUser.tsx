import { Button, Form, Modal, Notification } from "@douyinfe/semi-ui";
import { create } from "../../../services/UserService";

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

    function handleSubmit(value: any) {
        create(value)
            .then((res) => {
                if (res.success) {
                    onSuccess();
                } else {
                    Notification.error({
                        title: '创建失败',
                        content: res.message
                    } as any);
                }
            })
    }

    return (
        <Modal
            footer={[]}
            title='创建用户'
            visible={visible}
            onCancel={onCancel}
            onOk={onSuccess}
        >
            <Form
                onSubmit={handleSubmit}
            >
                <Form.Input
                    rules={[
                        {
                            required: true,
                            message: '请输入用户名'
                        }
                    ]}
                    field="userName"
                    label='用户名'>
                </Form.Input>
                <Form.Input
                    rules={[
                        {
                            required: true,
                            message: '请输入邮箱'
                        }
                    ]}
                    field="email"
                    label='邮箱'>
                </Form.Input>
                <Form.Input
                    rules={[
                        {
                            required: true,
                            message: '请输入密码'
                        }
                    ]}
                    field="password"
                    label='密码'>
                </Form.Input>
                <Form.Select
                    rules={[
                        {
                            required: true,
                            message: '请选择角色'
                        }
                    ]}
                    field="role" style={{
                        width: '100%'
                    }} initValue={'user'} label='角色'>
                    <Form.Select.Option value='user'>用户</Form.Select.Option>
                    <Form.Select.Option value='admin'>管理员</Form.Select.Option>
                </Form.Select>
                <Button block type='primary' htmlType='submit'>提交</Button>
            </Form>
        </Modal>
    )
}
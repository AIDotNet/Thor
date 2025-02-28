import { useState, useEffect } from "react";
import { Button, Drawer, Input, Select, message } from 'antd';
import { update } from "../../../services/UserService";

interface EditUserProps {
    visible: boolean;
    onCancel: () => void;
    onSuccess: () => void;
    user: any; // 要编辑的用户
}

export default function EditUser({
    visible,
    onCancel,
    onSuccess,
    user
}: EditUserProps) {
    const [input, setInput] = useState({
        id: '',
        email: '',
        groups: []
    });
    const [errors, setErrors] = useState({
        email: '',
        groups: ''
    });

    // 当用户数据变化时更新输入
    useEffect(() => {
        if (user && visible) {
            setInput({
                id: user.id || '',
                email: user.email || '',
                groups: user.groups
            });
            console.log(user);
        }
    }, [user, visible]);

    function validate() {
        let valid = true;
        const newErrors = { email: '', groups: '' };
        
        if (!input.email) {
            newErrors.email = '请输入邮箱';
            valid = false;
        }
        
        if (!input.groups || input.groups.length === 0) {
            newErrors.groups = '请选择组';
            valid = false;
        }
        
        setErrors(newErrors);
        return valid;
    }

    function handleSubmit() {
        if (!validate()) return;
        
        // 只有当密码被修改时才包含密码字段
        const userData = {
            ...input,
        };

        update(userData)
            .then((res) => {
                if (res.success) {
                    message.success('更新成功');
                    onSuccess();
                } else {
                    message.error({
                        content: res.message
                    } as any);
                }
            })
    }

    // 当抽屉关闭时重置数据
    useEffect(() => {
        if (!visible) {
            setInput({
                id: '',
                email: '',
                groups: []
            });
            setErrors({
                email: '',
                groups: ''
            });
        }
    }, [visible]);

    return (
        <Drawer
            open={visible}
            width={500}
            title="编辑用户"
            onClose={onCancel}
        >
            <div style={{ width: 400 }}>
                <div style={{ marginBottom: 24 }}>
                    <div style={{ marginBottom: 8 }}>
                        <label style={{ display: 'block', marginBottom: 8 }}>邮箱</label>
                        <Input 
                            value={input.email} 
                            onChange={(e) => setInput({ ...input, email: e.target.value })}
                            status={errors.email ? 'error' : ''}
                        />
                        {errors.email && <div style={{ color: 'red', fontSize: 12 }}>{errors.email}</div>}
                    </div>
                </div>
                
                <div style={{ marginBottom: 24 }}>
                    <div style={{ marginBottom: 8 }}>
                        <label style={{ display: 'block', marginBottom: 8 }}>组</label>
                        <Select
                            mode="tags"
                            style={{ width: '100%' }}
                            value={input.groups}
                            // 提供默认的选项
                            options={user?.groups?.map((group: string) => ({
                                label: group,
                                value: group
                            }))}
                            onChange={(value) => setInput({ ...input, groups: value })}
                            status={errors.groups ? 'error' : ''}
                        />
                        {errors.groups && <div style={{ color: 'red', fontSize: 12 }}>{errors.groups}</div>}
                    </div>
                </div>
                
                <Button type='primary' block onClick={handleSubmit}>
                    提交
                </Button>
            </div>
        </Drawer>
    )
} 
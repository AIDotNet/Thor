import { createProduct } from "../../../services/ProductService";
import { Form, Button, Drawer, Input, InputNumber, Tag, message } from 'antd';
import { renderQuota } from "../../../utils/render";
import { useState } from "react";

interface CreateProductProps {
    visible: boolean;
    onCancel: () => void;
    onSuccess: () => void;
}

export default function CreateProduct({
    visible,
    onCancel,
    onSuccess
}: CreateProductProps) {

    const [input,setInput] = useState<any>({
        name: '',
        description: '',
        price: 0,
        remainQuota: 0,
        stock: 0
    })

    function handleSubmit(values: any) {
        createProduct({
            name: input.name,
            description: input.description,
            price: input.price,
            remainQuota: input.remainQuota,
            stock: input.stock
        })
            .then((res) => {
                if (res.success) {
                    message.success('创建成功');
                    onSuccess();
                } else {
                    message.error(res.message);
                }
            })
            .catch((err) => {
                message.error('创建失败');
            });
    }

    return (
        <Drawer
            title="创建产品"
            footer={null}
            open={visible}
            onClose={onCancel}
        >
            <Form
                onFinish={handleSubmit}
                layout="vertical"
            >
                <Form.Item
                    label="产品名称"
                    rules={[{ required: true, message: '产品名称不能为空' }]}
                >
                    <Input value={input.name} onChange={(v)=>{
                        setInput({...input, name: v.target.value})
                    }}/>
                </Form.Item>
                <Form.Item
                    label="产品描述"
                    rules={[{ required: true, message: '产品描述不能为空' }]}
                >
                    <Input value={input.description} onChange={(v)=>{
                        setInput({...input, description: v.target.value})
                    }}/>
                </Form.Item>
                <Form.Item
                    label="价格"
                    rules={[{ required: true, message: '价格不能为空' }]}
                >
                    <InputNumber value={input.price} onChange={(v)=>{
                        setInput({...input, price: v as any})
                    }} style={{ width: '100%' }} />
                </Form.Item>
                <Form.Item
                    name="remainQuota"
                    label="额度"
                    rules={[{ required: true, message: '额度不能为空' }]}
                >
                    <InputNumber
                        value={input.remainQuota}
                        onChange={(v) => {
                            setInput({ ...input, remainQuota: v as any })
                        }}
                        style={{ width: '100%' }}
                        addonAfter={<Tag>{renderQuota(input.remainQuota ?? 0, 6)}</Tag>}
                    />
                </Form.Item>
                <Form.Item
                    name="stock"
                    label="库存"
                    rules={[{ required: true, message: '库存不能为空' }]}
                >
                    <InputNumber 
                        value={input.stock} 
                        onChange={(v)=>{
                            setInput({...input, stock: v as any})
                        }}
                        style={{ width: '100%' }} min={-1} />
                </Form.Item>
                <Button type="primary" block htmlType="submit">提交</Button>
            </Form>
        </Drawer>
    );
}
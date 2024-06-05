import { useEffect, useState } from "react";
import { getModels } from "../../../services/ModelService";
import { message, Drawer } from 'antd';
import { Button, Select, Form, Input } from 'antd'
import { createRateLimitModel } from "../../../services/RateLimitModelService";

interface ICreateChannelProps {
    onSuccess: () => void;
    visible: boolean;
    onCancel: () => void;
}

export default function CreateChannel({
    onSuccess,
    visible,
    onCancel
}: ICreateChannelProps) {
    const [models, setModels] = useState<any>();
    const [input, setInput] = useState<FieldType>({
        name: '',
        description: '',
        whiteList: [],
        blackList: [],
        enable: false,
        model: [],
        strategy: '',
        limit: 0,
        value: 0,
    });


    type FieldType = {
        name?: string;
        description?: string;
        whiteList: string[];
        blackList: string[];
        enable: boolean;
        model: string[];
        strategy: string;
        limit: number;
        value: number;
    };

    function loading() {

        getModels()
            .then(res => {
                if (res.success) {
                    setModels(res.data);
                } else {
                    message.error({
                        content: res.message
                    });
                }
            })

    }

    useEffect(() => {
        if (visible) {
            loading();
        }
    }, [visible]);

    function handleSubmit(values: any) {

        // 判断是否选择了模型
        if (!values.model || values.model.length === 0) {
            message.error({
                content: '请选择模型'
            });
            return;
        }

        createRateLimitModel(values)
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
        open={visible}
        width={500}
        title="创建限流策略"
        onClose={() => onCancel()}
    >
        <Form onFinish={(values: any) => handleSubmit(values)} style={{ width: 400 }}>

            <Form.Item<FieldType>
                label="限流策略名称"
                name="name"
                rules={[{ required: true, message: '请输入限流策略名称' }]}
            >
                <Input value={input.name} onChange={(v) => {
                    setInput({ ...input, name: v.target.value })
                }} />
            </Form.Item>

            <Form.Item<FieldType>
                label="描述"
                name="description"
            >
                <Input value={input.description}
                    onChange={(v) => setInput({ ...input, description: v.target.value })} />
            </Form.Item>
            <Form.Item<FieldType>
                label="限流策略"
                name="strategy"
                rules={[{ required: true, message: '请选择限流策略' }]}
            >
                <Select style={{ width: '100%' }}
                    value={input.strategy}
                    onChange={(v) => setInput({ ...input, strategy: v })}
                    options={[
                        {
                            label: '秒',
                            value: 's'
                        },
                        {
                            label: '分钟',
                            value: 'm'
                        },
                        {
                            label: '小时',
                            value: 'h'
                        },
                        {
                            label: '天',
                            value: 'd'
                        }
                    ]}
                    placeholder="限流策略">
                </Select>
            </Form.Item>
            <Form.Item<FieldType>
                label="策略数量"
                name="limit"
                rules={[
                    { required: true, message: '请输入策略数量' },
                ]}
            >
                <Input
                    type='number'
                    value={input.limit}
                    max={99999}
                    min={1}
                    onChange={(v) => {
                        setInput({ ...input, limit: parseInt(v.target.value) })
                    }} />
            </Form.Item>
            <Form.Item<FieldType>
                label="限流数量"
                name="value"
                rules={[
                    { required: true, message: '请输入限流数量' },
                ]}
            >
                <Input
                    type='number'
                    value={input.value}
                    max={99999}
                    min={1}
                    onChange={(v) => {
                        setInput({ ...input, value: parseInt(v.target.value) })
                    }} />
            </Form.Item>

            <Form.Item<FieldType>
                label="白名单"
                name="whiteList"
                // 正则表达式验证是否是ip地址
                rules={[{ pattern: /^(\d{1,3}\.){3}\d{1,3}$/, message: '请输入正确的ip地址' }]}
            >
                <Select mode="tags" style={{ width: '100%' }}
                    defaultActiveFirstOption={true}
                    allowClear
                    value={input.whiteList}
                    onChange={(v) => setInput({ ...input, whiteList: v })}
                    placeholder="白名单">
                </Select>
            </Form.Item>
            <Form.Item<FieldType>
                label="黑名单"
                name="blackList"
                // 正则表达式验证是否是ip地址
                rules={[{ pattern: /^(\d{1,3}\.){3}\d{1,3}$/, message: '请输入正确的ip地址' }]}
            >
                <Select mode="tags" style={{ width: '100%' }}
                    defaultActiveFirstOption={true}
                    value={input.blackList}
                    allowClear
                    onChange={(v) => setInput({ ...input, blackList: v })}
                    placeholder="黑名单">
                </Select>
            </Form.Item>
            <Form.Item<FieldType>
                label="模型"
                name="model"
                rules={[{ required: true, message: '请选择模型' }]}
            >
                <Select mode="multiple" style={{ width: '100%' }}
                    defaultActiveFirstOption={true}
                    allowClear
                    value={input.model}
                    onChange={(v) => setInput({ ...input, model: v })}
                    options={models && models.map((model: any) => {
                        return { label: model, value: model }
                    })}
                    placeholder="模型">
                </Select>
            </Form.Item>
            <Button type='primary' block htmlType='submit'>提交</Button>
        </Form>
    </Drawer>;
}
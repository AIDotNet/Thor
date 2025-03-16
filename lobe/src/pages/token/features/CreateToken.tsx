import { Drawer, Form, Button, Switch, message, Input, DatePicker, InputNumber, Select } from 'antd';
import { Add } from '../../../services/TokenService'
import { useEffect, useState } from 'react';
import { renderQuota } from '../../../utils/render';
import { getModels } from '../../../services/ModelService';
import { getCurrentList } from '../../../services/UserGroupService';
import { Flexbox } from 'react-layout-kit';

const { Option } = Select;


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
    const [models, setModels] = useState<any>();

    const [groups, setGroups] = useState<any[]>([]);

    useEffect(() => {
        getCurrentList()
            .then((res) => {
                setGroups(res.data);
            })
    }, []);

    type FieldType = {
        name?: string;
        unlimitedQuota: boolean;
        remainQuota?: number;
        unlimitedExpired: boolean;
        expiredTime?: Date;
        limitModels: string[];
        whiteIpList: string[];
        groups: string[];
    };

    function loadModel() {

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
            loadModel();
        }
    }, [visible]);

    const [input, setInput] = useState<FieldType>({
        name: '',
        unlimitedQuota: false,
        remainQuota: 0,
        unlimitedExpired: false,
        groups: [],
        limitModels: [],
        expiredTime: new Date(Date.now() + 15 * 24 * 60 * 60 * 1000),
        whiteIpList: []
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
            <Form.Item<FieldType> name='limitModels' label='限制使用模型（不填则不限制）' style={{ width: '100%' }}>
                <Select
                    placeholder="请选择可用模型"
                    defaultActiveFirstOption={true}
                    mode="tags"
                    value={input.limitModels}
                    onChange={(v) => {
                        setInput({ ...input, limitModels: v });
                    }}
                    allowClear
                >
                    {

                        models && models.map((model: any) => {
                            return <Option key={model} value={model}>{model}</Option>
                        })
                    }
                </Select>
            </Form.Item>
            <Form.Item
                name="groups"
                label="组"
                rules={[{ required: true, message: '请选择组' }]}
                style={{ width: "100%" }}
            >
                <Select
                    placeholder="请选择组"
                    mode="multiple"
                    maxTagCount={1}
                    maxCount={1}
                    allowClear
                    options={groups?.map((group: any) => {
                        return {
                            label: <Flexbox gap={8} horizontal>
                                <span>{group.name}</span>
                                <span style={{ fontSize: 12, color: '#999' }}>{group.description}</span>
                                <span style={{ fontSize: 12, color: '#999' }}>
                                    <span>倍率：</span>
                                    {group.rate}
                                </span>
                            </Flexbox>,
                            value: group.code
                        }
                    })}
                    value={input.groups}
                    onChange={(v) => {
                        setInput({ ...input, groups: v });
                    }}
                />
            </Form.Item>
            <Form.Item<FieldType> name='whiteIpList' label='IP白名单' style={{ width: '100%' }}>
                <Select
                    placeholder="请选择IP白名单"
                    defaultActiveFirstOption={true}
                    mode="tags"
                    value={input.whiteIpList}
                    onChange={(v) => {
                        setInput({ ...input, whiteIpList: v });
                    }}
                    allowClear
                >
                </Select>
            </Form.Item>
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
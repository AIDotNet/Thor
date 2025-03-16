import { Drawer, Form, Button, Switch, message, Input, DatePicker, InputNumber, Select } from 'antd';
import { Update } from '../../../services/TokenService'
import { useEffect, useState } from 'react';
import { renderQuota } from '../../../utils/render';
import { getModels } from '../../../services/ModelService';
import { getCurrentList } from '../../../services/UserGroupService';
import { Flexbox } from 'react-layout-kit';
const { Option } = Select;

interface UpdateTokenProps {
    onSuccess: () => void;
    visible: boolean;
    onCancel: () => void;
    value?: any;
}

export default function UpdateToken({
    onSuccess,
    visible,
    onCancel,
    value
}: UpdateTokenProps) {

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
    const [models, setModels] = useState<any>();

    const [input, setInput] = useState<any>({
        name: '',
        unlimitedQuota: false,
        remainQuota: 0,
        unlimitedExpired: false,
        expiredTime: new Date(Date.now() + 15 * 24 * 60 * 60 * 1000),
        limitModels: [],
        whiteIpList: [],
        groups: [],
    });

    function handleSubmit(values: any) {
        values.id = value.id;
        Update({
            id: value.id,
            ...input
        })
            .then((item) => {
                if (item.success) {
                    message.success({
                        content: '修改成功',
                    });
                    onSuccess();
                } else {
                    message.error({
                        content: item.message
                    });

                }
            });
    }

    useEffect(() => {
        setInput({
            name: value?.name,
            unlimitedQuota: value?.unlimitedQuota,
            remainQuota: value?.remainQuota,
            unlimitedExpired: value?.unlimitedExpired,
            expiredTime: value?.expiredTime,
            limitModels: value?.limitModels,
            whiteIpList: value?.whiteIpList,
            groups: value?.groups
        })

    }, [value])

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

    if (!visible) {
        return null;
    }

    return <Drawer
        width={500}
        title="修改Token" open={visible} onClose={onCancel}>
        <Form
            initialValues={value}
            onFinish={values => handleSubmit(values)}
            style={{ width: 400 }}>
            <Form.Item<FieldType>
                label="Token名称"
                rules={[{ required: true, message: '请输入Token名称' }]}
            >
                <Input value={input?.name} onChange={(v) => {
                    setInput({ ...input, name: v.target.value })
                }} />
            </Form.Item>
            {
                !input?.unlimitedQuota && <Form.Item<FieldType>
                    label="额度"
                    rules={[]}
                >
                    <InputNumber
                        addonAfter={renderQuota(input?.remainQuota ?? 0, 6)} value={input?.remainQuota} onChange={(v) => {
                            setInput({ ...input, remainQuota: v as any })
                        }} />
                </Form.Item>
            }
            <Form.Item<FieldType>
                label="无限额度"
            >
                <Switch value={input?.unlimitedQuota} onChange={(v) => {
                    setInput({ ...input, unlimitedQuota: v })
                }} />
            </Form.Item>
            {
                input?.unlimitedExpired === false && <Form.Item<FieldType> label='过期时间'>
                    <DatePicker style={{
                        width: '100%'
                    }} value={input?.expiredTime} onChange={(v) => {
                        setInput({ ...input, expiredTime: v as any })
                    }} />
                </Form.Item>
            }
            <Form.Item<FieldType>
                name='limitModels' label='限制使用模型（不填则不限制）' style={{ width: '100%' }}>
                <Select
                    placeholder="请选择限制使用模型"
                    defaultActiveFirstOption={true}
                    mode="tags"
                    defaultValue={value?.limitModels}
                    onChange={(v) => {
                        setInput({ ...input, limitModels: v });
                    }}
                    value={input.limitModels}
                    allowClear
                >
                    {

                        models && models.map((model: any) => {
                            return <Option key={model} value={model}>{model}</Option>
                        })
                    }
                </Select>
            </Form.Item>

            <Form.Item<FieldType>
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
                    onChange={(v) => {
                        setInput({ ...input, groups: v });
                    }}
                    value={input.groups}
                    defaultValue={value?.groups}
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
                    defaultValue={value?.whiteIpList}
                    allowClear
                >
                </Select>
            </Form.Item>
            <Form.Item<FieldType>
                label="永不过期"
            >
                <Switch value={input?.unlimitedExpired} onChange={(v) => {
                    setInput({ ...input, unlimitedExpired: v })
                }} />
            </Form.Item>
            <Button type='primary' block htmlType='submit'>提交</Button>
        </Form>
    </Drawer>
}
import { Modal, Tag } from "@lobehub/ui";
import { Button, Form, Input, InputNumber, message, Select } from "antd";
import { UpdateModelManager } from "../../../services/ModelManagerService";
import { getIconByNames } from "../../../utils/iconutils";
import { useState } from "react";
import { renderQuota } from "../../../utils/render";


interface UpdateModelManagerProps {
    open: boolean;
    onClose: () => void;
    onOk: () => void;
    value: any;
}

export default function UpdateModelManagerPage({
    open,
    onClose,
    onOk,
    value
}: UpdateModelManagerProps) {

    const [promptRate, setPromptRate] = useState(0);

    function save(v: any) {
        v.id = value.id;
        UpdateModelManager(v)
            .then((res) => {
                if (res.success) {
                    onOk();
                } else {
                    message.error(res.message);
                }
            });
    }

    return (open && <Modal open={open} onCancel={() => onClose()} onClose={() => onClose()} footer={[]} title="创建模型倍率">
        <Form onFinish={save} initialValues={value}>
            <Form.Item rules={[
                {
                    required: true,
                    message: '请输入模型名称'
                }
            ]} name={'model'} label='模型名称'>
                <Input placeholder="请输入模型名称"></Input>
            </Form.Item>
            <Form.Item rules={[
                {
                    required: true,
                    message: '请输入模型计费类型'
                }
            ]} name='quotaType' label='模型计费类型'>
                <Select>
                    <Select.Option value={1}>按量计费</Select.Option>
                    <Select.Option value={2}>按次计费</Select.Option>
                </Select>
            </Form.Item>
            <Form.Item style={{
                padding: 0,
                margin: 0
            }} shouldUpdate={(prevValues, currentValues) => prevValues.quotaType !== currentValues.quotaType}>
                {({ getFieldValue }) => {
                    const quotaType = getFieldValue('quotaType');
                    return quotaType === 1 ? (
                        <Form.Item
                            rules={[
                                {
                                    required: true,
                                    message: '请输入提示倍率'
                                },
                                {
                                    validator: (_, value) => {
                                        return value > 0 ? Promise.resolve() : Promise.reject('提示倍率必须大于0');
                                    }
                                }
                            ]}
                            name={"promptRate"} label="提示倍率">
                            <InputNumber style={{ width: '100%' }} placeholder="请输入提示倍率"></InputNumber>
                        </Form.Item>
                    ) : quotaType === 2 ? (
                        <Form.Item
                            rules={[
                                {
                                    required: true,
                                    message: '请输入提示倍率'
                                },
                                {
                                    validator: (_, value) => {
                                        return value > 0 ? Promise.resolve() : Promise.reject('提示倍率必须大于0');
                                    }
                                },
                            ]} style={{
                                padding:0,
                                margin:0
                            }} 
                            name={"promptRate"} label="每次费用">
                            <InputNumber
                                onChange={(value) => setPromptRate(value ?? 0)}
                                value={promptRate}
                                suffix={<Tag>{renderQuota(promptRate, 6)}</Tag>}
                                style={{ width: '100%' }} placeholder="请输入每次费用"></InputNumber>
                        </Form.Item>
                    ) : null;
                }}
            </Form.Item>
            <Form.Item style={{
                padding: 0,
                margin: 0
            }} shouldUpdate={(prevValues, currentValues) => prevValues.quotaType !== currentValues.quotaType}>
                {({ getFieldValue }) => {
                    return getFieldValue('quotaType') === 1 ? (
                        <Form.Item name={"completionRate"} label="完成倍率">
                            <InputNumber style={{
                                width: '100%'
                            }} placeholder="请输入完成倍率"></InputNumber>
                        </Form.Item>
                    ) : null;
                }}
            </Form.Item>
            <Form.Item name='isVersion2' label='是否实时模型'>
                <Select defaultValue={false}>
                    <Select.Option value={true}>是</Select.Option>
                    <Select.Option value={false}>否</Select.Option>
                </Select>
            </Form.Item>
            <Form.Item name='audioPromptRate' label='音频提示倍率'>
                <Input placeholder="请输入音频提示倍率"></Input>
            </Form.Item>
            <Form.Item name='AudioOutputRate' label='音频完成倍率'>
                <Input placeholder="请输入音频完成倍率"></Input>
            </Form.Item>

            <Form.Item rules={[
                {
                    required: true,
                    message: '请输入描述'
                }
            ]} name='description' label='描述'>
                <Input placeholder="请输入描述"></Input>
            </Form.Item>
            <Form.Item name='quotaMax' label='最大上文'>
                <Input placeholder="请输入最大上文"></Input>
            </Form.Item>
            <Form.Item rules={[
                {
                    required: true,
                    message: '模型显示图标'
                }
            ]} name='icon' label='模型显示图标'>
                <Select options={getIconByNames(25)}>
                </Select>
            </Form.Item>
            <Form.Item name='tags' label='模型标签'>
                <Select
                    mode="tags"
                    options={[
                        {
                            label: '文本',
                            value: '文本'
                        },
                        {
                            label: "视觉",
                            value: "视觉"
                        },
                        {
                            label: "多模态",
                            value: "多模态"
                        },
                        {
                            label: "图像分析",
                            value: "图像分析"
                        },
                        {
                            label: "文件分析",
                            value: "文件分析"
                        }
                    ]}
                    placeholder="请输入标签"
                >
                </Select>
            </Form.Item>
            <Form.Item>
                <Button block htmlType="submit" >更新</Button>
            </Form.Item>
        </Form>
    </Modal>)
}
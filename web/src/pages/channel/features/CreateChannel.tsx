import { Divider, Modal, SideSheet, Form, Button, Notification } from "@douyinfe/semi-ui";
import { useEffect, useState } from "react";
import { getModels, getTypes } from "../../../services/ModelService";
import { Add } from "../../../services/ChannelService";
import { getModelPrompt } from "../../../uitls/render";

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
    // 字典models key, value类型
    const [types, setTypes] = useState<any>();
    const [models, setModels] = useState<any>();

    function loading() {
        getTypes()
            .then(res => {
                if (res.success) {
                    setTypes(res.data);
                } else {
                    Modal.error({
                        title: '获取模型失败',
                        content: res.message
                    });
                }
            })

        getModels()
            .then(res => {
                if (res.success) {
                    setModels(res.data);
                } else {
                    Modal.error({
                        title: '获取模型失败',
                        content: res.message
                    });
                }
            })

    }

    useEffect(() => {
        if (visible) {
            loading();
        }    }, [visible]);

    function handleSubmit(values: any) {

        // 判断是否选择了模型
        if (!values.models || values.models.length === 0) {
            Notification.error({
                title: '创建失败',
                content: '请选择模型'
            });
            return;
        }

        Add(values)
            .then((item) => {
                if (item.success) {
                    Notification.success({
                        title: '创建成功',
                    });
                    onSuccess();
                } else {
                    Notification.error({
                        title: '创建失败',
                        content: item.message
                    });
                }
            });
    }

    return <SideSheet
        visible={visible}
        title="创建Channel"
        onCancel={onCancel}
    >
        <Divider></Divider>
        <Form onSubmit={values => handleSubmit(values)} style={{ width: 400 }}>
            {({ values }) => (
                <>
                    <Form.Input rules={[{
                        required: true,
                        message: '渠道名称不能为空'
                    }, {
                        min: 3,
                        message: '渠道名称长度不能小于3'
                    }]} field='name' label='渠道名称' style={{ width: '100%' }} placeholder='请输入渠道名称'></Form.Input>
                    <Form.Select
                        rules={[{
                            required: true,
                            message: '请选择模型'
                        }]}
                        field='type' label='渠道类型' style={{ width: '100%' }} placeholder='请选择渠道类型'>
                        {
                            types && Object.keys(types).map((key) => {
                                return <Form.Select.Option key={key} value={types[key]}>{key}</Form.Select.Option>
                            })
                        }
                    </Form.Select>
                    <Form.Input field='address' label='代理地址' style={{ width: '100%' }} placeholder='请输入代理地址'></Form.Input>
                    {
                        values.type === "AzureOpenAI" && <Form.Select
                            field='other'
                            label='版本'
                            optionList={[
                                {
                                    label: "2022-12-01",
                                    value: "2022-12-01"
                                }, {
                                    label: "2023-05-15",
                                    value: "2023-05-15"
                                }, {
                                    label: "2023-06-01-preview",
                                    value: "2023-06-01-preview"
                                }, {
                                    label: "2023-07-01-preview",
                                    value: "2023-07-01-preview"
                                }, {
                                    label: "2024-02-15-preview",
                                    value: "2024-02-15-preview"
                                }, {
                                    label: "2024-03-01-preview",
                                    value: "2024-03-01-preview"
                                }, {
                                    label: "2024-04-01-preview",
                                    value: "2024-04-01-preview"
                                }
                            ]}
                            allowCreate={true}
                            multiple={false}
                            filter={true}
                            defaultActiveFirstOption
                            style={{ width: '100%' }} placeholder='请输入版本'>

                        </Form.Select>
                    }
                    {
                        values.type === "Hunyuan" && <Form.Select
                            field='other'
                            label='资源地域'
                            optionList={[
                                {
                                    label: "北京（ap-beijing）",
                                    value: "ap-beijing"
                                }, {
                                    label: "广州 （ap-guangzhou）",
                                    value: "ap-guangzhou"
                                }
                            ]}
                            allowCreate={true}
                            multiple={false}
                            filter={true}
                            defaultActiveFirstOption
                            style={{ width: '100%' }} placeholder='请输入资源地域'>
                            </Form.Select>
                    }
                    <Form.Input field='key' label='密钥' style={{ width: '100%' }} placeholder={getModelPrompt(values.type)}></Form.Input>
                    <Form.Select
                        field="models"
                        label="模型"
                        allowCreate={true}
                        multiple={true}
                        filter={true}
                        defaultActiveFirstOption
                        style={{ width: '100%' }}
                        placeholder="请选择模型"
                    >
                        {
                            models && models.map((model: any) => {
                                return <Form.Select.Option key={model} value={model}>{model}</Form.Select.Option>
                            })
                        }
                    </Form.Select>

                    <Button type='primary' block htmlType='submit'>提交</Button>
                </>
            )}
        </Form>
    </SideSheet>;
}
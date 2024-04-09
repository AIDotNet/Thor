import { Divider, Modal, SideSheet, Form, Button, Notification } from "@douyinfe/semi-ui";
import { useEffect, useState } from "react";
import { getModels, getTypes } from "../../../services/ModelService";
import { Add } from "../../../services/ChannelService";

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
        }
    }, [visible]);

    function handleSubmit(values: any) {
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
            {({ }: any) => (
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
                    <Form.Input field='key' label='密钥' style={{ width: '100%' }} placeholder='请输入密钥'></Form.Input>
                    <Form.Select
                        field="models"
                        label="模型"
                        multiple
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
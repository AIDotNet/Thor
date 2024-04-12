import { Button, Divider, Form, Notification, SideSheet, Tag } from "@douyinfe/semi-ui";
import { renderQuota } from "../../../uitls/render";
import { Add } from "../../../services/RedeemCodeService";

interface CreateRedeemCodeProps {
    onSuccess: () => void;
    visible: boolean;
    onCancel: () => void;
}

export default function CreateRedeemCode({
    onSuccess,
    visible,
    onCancel
}: CreateRedeemCodeProps) {

    function handleSubmit(values: any) {
        Add(values)
            .then((item) => {
                if(item.success) {
                    Notification.success({
                        title: '操作成功',
                    });

                    if(item.data.length > 1) {
                        // 将内容下载到本地
                        const blob = new Blob([item.data.join('\n')], { type: 'text/plain' });
                        const url = URL.createObjectURL(blob);
                        const a = document.createElement('a');
                        a.href = url;
                        a.download = '兑换码.txt';
                        a.click();
                        URL.revokeObjectURL(url);

                    }

                    onSuccess();
                
                }else{
                    Notification.error({
                        title: '操作失败',
                    });
                }
                
            }), () => Notification.error({
                title: '操作失败',
            });
    }

    return <SideSheet title="创建Token" visible={visible} onCancel={onCancel}>
        <Divider></Divider>
        <Form onSubmit={values => handleSubmit(values)} style={{ width: 400 }}>
            {({ formState, values, formApi }: any) => (
                <>
                    <Form.Input rules={[{
                        required: true,
                        message: '名称不能为空'
                    }]} field='name' label='名称' style={{ width: '100%' }} placeholder='请输入名称'></Form.Input>
                    <Form.Input type="number" rules={[{
                        min: 0,
                        message: '额度不能小于0'
                    }, {
                        required: true,
                        message: '额度不能为空'
                    }]} field='quota'
                        label='额度' style={{ width: '100%' }} suffix={<Tag color='green'>{renderQuota(values.quota ?? 0, 6)}</Tag>} placeholder='请输入额度'></Form.Input>
                    <Form.Input field="count" label="生成数量"
                        defaultValue={1}
                        rules={[{
                            required: true,
                            message: '生成数量不能为空'
                        }]}
                    >

                    </Form.Input>
                    <Button type='primary' block htmlType='submit'>提交</Button>
                </>
            )}
        </Form>
    </SideSheet>
}
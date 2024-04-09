import { Button, Divider, Form, Notification, SideSheet } from "@douyinfe/semi-ui";
import { Update } from '../../../services/TokenService'

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

    function handleSubmit(values: any) {
        Update(values)
            .then(() => {
                Notification.success({
                    title: '修改成功',
                });
                onSuccess();
            });
    }

    return <SideSheet title="创建Token" visible={visible} onCancel={onCancel}>
        <Divider></Divider>
        <Form initValues={value} onSubmit={values => handleSubmit(values)} style={{ width: 400 }}>
            {({ formState, values, formApi }: any) => (
                <>
                    <Form.Input rules={[{
                        required: true,
                        message: 'Token名称不能为空'
                    },{
                        min: 3,
                        message: 'Token名称长度不能小于3'
                    }]} field='name' label='Token名称' style={{ width: '100%' }} placeholder='请输入token名称'></Form.Input>
                    {
                        values.unlimitedQuota ? null : <Form.Input  type="number" field='remainQuota' label='额度' style={{ width: '100%' }} placeholder='请输入token额度'></Form.Input>
                    }
                    <Form.Checkbox field='unlimitedQuota' label='无限额度'></Form.Checkbox>
                    {
                        values.unlimitedExpired ? null : <Form.DatePicker field='expiredTime' label='过期时间' style={{ width: '100%' }}></Form.DatePicker>
                    }
                    <Form.Checkbox field='unlimitedExpired' label='永不过期'></Form.Checkbox>
                    <Button type='primary' block htmlType='submit'>提交</Button>
                </>
            )}
        </Form>
    </SideSheet>
}
import { Button, Divider, Form, SideSheet } from "@douyinfe/semi-ui";


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

    function handleSubmit(values: any) {
        console.log(values);
        onSuccess();
    }

    return <SideSheet title="创建Token" visible={visible} onCancel={onCancel}>
        <Divider></Divider>

        <Form onSubmit={values => handleSubmit(values)} style={{ width: 400 }}>
            {({ formState, values, formApi }) => (
                <>
                    <Form.Input field='name' label='PhoneNumber' style={{ width: '100%' }} placeholder='Enter your phone number'></Form.Input>
                    <Form.Input field='password' label='Password' style={{ width: '100%' }} placeholder='Enter your password'></Form.Input>
                    <Form.Checkbox field='agree' noLabel>I have read and agree to the terms of service</Form.Checkbox>
                    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                        <p>
                            <span>Or</span><Button theme='borderless' style={{ color: 'var(--semi-color-primary)', marginLeft: 10, cursor: 'pointer' }}>Sign up</Button>
                        </p>
                        <Button disabled={!values.agree} htmlType='submit' type="tertiary">Log in</Button>
                    </div>
                </>
            )}
        </Form>
    </SideSheet>
}
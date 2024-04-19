import { Button, Form, Modal, Tag } from "@douyinfe/semi-ui";
import { renderQuota } from "../../../uitls/render";
import { putProduct } from "../../../services/ProductService";

interface UpdateProductProps {
    visible: boolean;
    onCancel: () => void;
    onSuccess: () => void;
    value?: any;
}

export default function UpdateProduct({
    visible,
    onCancel,
    onSuccess,
    value
}: UpdateProductProps) {
    function handleSubmit(values: any) {
        values.id = value.id;
        putProduct(values)
            .then((res) => {
                if (res.success) {
                    onSuccess();
                }
            });
    }

    return (
        <Modal
            title="编辑产品"
            footer={null}
            visible={visible}
            onCancel={onCancel}
            style={{
                height: '550px',
            }}
            onOk={onSuccess}
        >
            <Form
                initValues={value}
                onSubmit={(values) => {
                    handleSubmit(values)
                }}>
                {({ values }) => {
                    return (
                        <>
                            <Form.Input
                                rules={[
                                    {
                                        required: true,
                                        message: '产品名称不能为空'
                                    }
                                ]}
                                field="name" label="产品名称" name="name" required />
                            <Form.Input
                                rules={[{
                                    required: true,
                                    message: '产品描述不能为空'
                                }]}
                                field="description" label="产品描述" name="description" required />
                            <Form.InputNumber
                                style={{
                                    width: '100%'
                                }}
                                initValue={1}
                                rules={[{
                                    required: true,
                                    message: '价格不能为空'
                                }]}
                                field="price" label="价格" name="price" required />
                            <Form.InputNumber
                                style={{
                                    width: '100%'
                                }}
                                initValue={values.remainQuota ?? 0}
                                rules={[{
                                    required: true,
                                    message: '额度不能为空'
                                }]}
                                field="remainQuota" label="额度" name="remainQuota" required suffix={
                                    <Tag>{renderQuota(values.remainQuota ?? 0, 6)}</Tag>
                                } />
                            <Form.InputNumber
                                min={-1}
                                style={{
                                    width: '100%'
                                }}
                                initValue={-1}
                                rules={[{
                                    required: true,
                                    message: '库存不能为空'
                                }]}
                                field="stock" label="库存" name="stock" required />
                            <Button block type="primary" htmlType="submit">提交</Button>
                        </>
                    )
                }}
            </Form>
        </Modal>
    )
}
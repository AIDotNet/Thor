import { useState } from 'react';
import { Modal, Form, Button, message, Select, InputNumber } from 'antd';
import { createModelMap, ModelMap } from "../../../services/ModelMapService";
import { MinusCircleOutlined, PlusOutlined } from '@ant-design/icons';
import { getModelList } from '../../../services/ModelService';
import { useEffect } from 'react';

interface CreateModelMapProps {
  visible: boolean;
  onSuccess: () => void;
  onCancel: () => void;
}

export default function CreateModelMap({ visible, onSuccess, onCancel }: CreateModelMapProps) {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [models, setModels] = useState<any[]>([]);

  useEffect(() => {
    // 加载模型列表
    getModelList().then((res: any) => {
      if (res.success) {
        setModels(res.data);
      } else {
        message.error('加载模型列表失败');
      }
    });
  }, []);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);

      values.modelId = values.modelId[0];
      
      values.modelMapItems = values.modelMapItems.map((item: any) => ({
        modelId: item.modelId[0],
        order: item.order
      }));

      // 清空values.modelMapItems中的空字符串
      values.modelMapItems = values.modelMapItems.filter((item: any) => item.modelId !== '');

      const data: ModelMap = {
        modelId: values.modelId,
        group: values.group || [],
        modelMapItems: values.modelMapItems || []
      };

      const response = await createModelMap(data);

      if (response.success) {
        message.success('创建成功');
        form.resetFields();
        onSuccess();
      } else {
        message.error(response.message || '创建失败');
      }
    } catch (error) {
      console.error('Validate Failed:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal
      title="创建模型映射"
      open={visible}
      onCancel={onCancel}
      footer={[
        <Button key="back" onClick={onCancel}>
          取消
        </Button>,
        <Button key="submit" type="primary" loading={loading} onClick={handleSubmit}>
          创建
        </Button>
      ]}
      width={700}
    >
      <Form
        form={form}
        layout="vertical"
        initialValues={{ modelMapItems: [] }}
      >
        <Form.Item
          name="modelId"
          label="源模型ID"
          rules={[{ required: true, message: '请选择源模型ID' }]}
        >
          <Select
            placeholder="请选择模型ID"
            showSearch
            defaultActiveFirstOption={true}
            mode="tags"
            maxCount={1}
            allowClear
            optionFilterProp="children"
          >
            {models.map((model: any) => (
              <Select.Option key={model} value={model}>
                {model}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>

        <Form.Item
          name="group"
          label="分组"
          rules={[{ required: true, message: '请输入分组' }]}
        >
          <Select
            mode="tags"
            placeholder="请输入分组，回车分隔"
            options={[
              {
                label: '默认',
                value: 'default'
              },
              {
                label: 'VIP',
                value: 'vip'
              }
            ]}
            style={{ width: '100%' }}
          />
        </Form.Item>

        <Form.List name="modelMapItems">
          {(fields, { add, remove }) => (
            <>
              {fields.map(({ key, name, ...restField }) => (
                <div key={key} style={{ display: 'flex', marginBottom: 8, alignItems: 'baseline' }}>
                  <Form.Item
                    {...restField}
                    name={[name, 'modelId']}
                    rules={[{ required: true, message: '请选择目标模型ID' }]}
                    style={{ flex: 1, marginRight: 8 }}
                  >
                    <Select
                      placeholder="请选择目标模型ID"
                      showSearch
                      optionFilterProp="children"
                      defaultActiveFirstOption={true}
                      mode="tags"
                      maxCount={1}
                      allowClear
                    >
                      {models.map((model: any) => (
                        <Select.Option key={model} value={model}>
                          {model}
                        </Select.Option>
                      ))}
                    </Select>
                  </Form.Item>
                  <Form.Item
                    {...restField}
                    name={[name, 'order']}
                    rules={[{ required: true, message: '请输入权重' }]}
                    style={{ width: 120 }}
                  >
                    <InputNumber placeholder="权重" style={{ width: '100%' }} />
                  </Form.Item>
                  {fields.length > 1 ? (
                    <MinusCircleOutlined
                      onClick={() => remove(name)}
                      style={{ marginLeft: 8 }}
                    />
                  ) : null}
                </div>
              ))}
              <Form.Item>
                <Button
                  type="dashed"
                  onClick={() => add()}
                  block
                  icon={<PlusOutlined />}
                >
                  添加映射项
                </Button>
              </Form.Item>
            </>
          )}
        </Form.List>
      </Form>
    </Modal>
  );
} 
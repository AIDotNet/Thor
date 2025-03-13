import { useState, useEffect } from 'react';
import { Modal, Form,  Button, message, Select, InputNumber } from 'antd';
import { updateModelMap, ModelMap } from "../../../services/ModelMapService";
import { MinusCircleOutlined, PlusOutlined } from '@ant-design/icons';
import { getModelList } from '../../../services/ModelService';
import { getList } from "../../../services/UserGroupService";
import { Flexbox } from "react-layout-kit";

interface UpdateModelMapProps {
  visible: boolean;
  value: ModelMap;
  onSuccess: () => void;
  onCancel: () => void;
}

export default function UpdateModelMap({ visible, value, onSuccess, onCancel }: UpdateModelMapProps) {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [models, setModels] = useState<any[]>([]);
  const [groups, setGroups] = useState<any[]>([]);

  useEffect(() => {
    // 加载模型列表
    getModelList().then((res: any) => {
      if (res.success) {
        setModels(res.data);
      } else {
        message.error('加载模型列表失败');
      }
    });
    
    // 加载用户组列表
    getList().then((res) => {
      if (res.success) {
        setGroups(res.data);
      } else {
        message.error('加载用户组列表失败');
      }
    });
  }, []);

  useEffect(() => {
    // 表单回填
    if (visible && value) {
      form.setFieldsValue({
        modelId: value.modelId,
        group: value.group,
        modelMapItems: value.modelMapItems.map((item: any) => ({
          modelId: [item.modelId],
          order: item.order
        }))
      });
    }
  }, [visible, value, form]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);

      values.modelMapItems = values.modelMapItems.map((item: any) => ({
        modelId: item.modelId[0],
        order: item.order
      }));

      const data: ModelMap = {
        id: value.id,
        modelId: value.modelId,
        group: values.group || [],
        modelMapItems: values.modelMapItems || []
      };

      const response = await updateModelMap(data);

      if (response.success) {
        message.success('更新成功');
        onSuccess();
      } else {
        message.error(response.message || '更新失败');
      }
    } catch (error) {
      console.error('Validate Failed:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal
      title="编辑模型映射"
      open={visible}
      onCancel={onCancel}
      footer={[
        <Button key="back" onClick={onCancel}>
          取消
        </Button>,
        <Button key="submit" type="primary" loading={loading} onClick={handleSubmit}>
          更新
        </Button>
      ]}
      width={700}
    >
      <Form
        form={form}
        layout="vertical"
      >
        <Form.Item
          name="modelId"
          label="源模型ID"
          rules={[{ required: true, message: '请选择源模型ID' }]}
        >
          <Select
            placeholder="请选择模型ID"
            showSearch
            optionFilterProp="children"
            disabled
          >
            {models.map((model: any) => (
              <Select.Option key={model.id} value={model.id}>
                {model.name || model.id}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>

        <Form.Item
          name="group"
          label="分组"
          rules={[{ required: true, message: '请选择分组' }]}
        >
          <Select
            mode="tags"
            placeholder="请选择分组"
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
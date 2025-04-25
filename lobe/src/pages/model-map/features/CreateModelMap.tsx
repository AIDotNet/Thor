import { useState, useEffect } from 'react';
import { Modal, Form, Button, message, Select, InputNumber, Typography, } from 'antd';
import { createModelMap, ModelMap } from "../../../services/ModelMapService";
import { MinusCircleOutlined, PlusOutlined } from '@ant-design/icons';
import { getModelList } from '../../../services/ModelService';
import { getList } from "../../../services/UserGroupService";
import { Flexbox } from "react-layout-kit";
import { useTranslation } from 'react-i18next';
import { useTheme } from 'antd-style';

interface CreateModelMapProps {
  visible: boolean;
  onSuccess: () => void;
  onCancel: () => void;
}

export default function CreateModelMap({ visible, onSuccess, onCancel }: CreateModelMapProps) {
  const { t } = useTranslation();
  const theme = useTheme();
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [models, setModels] = useState<any[]>([]);
  const [groups, setGroups] = useState<any[]>([]);

  useEffect(() => {
    if (visible) {
      // 加载模型列表
      getModelList().then((res: any) => {
        if (res.success) {
          setModels(res.data);
        } else {
          message.error(t('modelMap.loadError'));
        }
      });
      
      // 加载用户组列表
      getList().then((res) => {
        if (res.success) {
          setGroups(res.data);
        } else {
          message.error(t('modelMap.loadError'));
        }
      });
    }
  }, [visible, t]);

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
        message.success(t('modelMap.createSuccess'));
        form.resetFields();
        onSuccess();
      } else {
        message.error(response.message || t('common.operateFailed'));
      }
    } catch (error) {
      console.error('Validate Failed:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal
      title={<Typography.Title level={5} style={{ margin: 0 }}>{t('modelMap.createMap')}</Typography.Title>}
      open={visible}
      onCancel={onCancel}
      footer={[
        <Button key="back" onClick={onCancel}>
          {t('common.cancel')}
        </Button>,
        <Button key="submit" type="primary" loading={loading} onClick={handleSubmit}>
          {t('common.create')}
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
          label={t('modelMap.sourceModelId')}
          rules={[{ required: true, message: t('modelMap.pleaseSelectSourceModel') }]}
        >
          <Select
            placeholder={t('modelMap.sourceModelId')}
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
          label={t('modelMap.group')}
          rules={[{ required: true, message: t('modelMap.pleaseSelectGroup') }]}
        >
          <Select
            mode="tags"
            placeholder={t('modelMap.group')}
            options={groups?.map((group: any) => {
              return {
                label: <Flexbox gap={8} horizontal>
                  <span>{group.name}</span>
                  <span style={{ fontSize: 12, color: theme.colorTextSecondary }}>{group.description}</span>
                  <span style={{ fontSize: 12, color: theme.colorTextSecondary }}>
                    <span>{t('rate')}：</span>
                    {group.rate}
                  </span>
                </Flexbox>,
                value: group.code
              }
            })}
            style={{ width: '100%' }}
          />
        </Form.Item>

        <Typography.Title level={5} style={{ marginTop: theme.marginLG }}>
          {t('modelMap.addMappingItem')}
        </Typography.Title>

        <Form.List name="modelMapItems">
          {(fields, { add, remove }) => (
            <>
              {fields.map(({ key, name, ...restField }) => (
                <Flexbox key={key} horizontal align="center" gap={8} style={{ marginBottom: theme.marginMD }}>
                  <Form.Item
                    {...restField}
                    name={[name, 'modelId']}
                    rules={[{ required: true, message: t('modelMap.pleaseSelectTargetModel') }]}
                    style={{ flex: 1, marginBottom: 0 }}
                  >
                    <Select
                      placeholder={t('modelMap.targetModelId')}
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
                    rules={[{ required: true, message: t('modelMap.pleaseEnterWeight') }]}
                    style={{ width: 120, marginBottom: 0 }}
                  >
                    <InputNumber placeholder={t('modelMap.weight')} style={{ width: '100%' }} />
                  </Form.Item>
                  {fields.length > 1 ? (
                    <Button 
                      type="text" 
                      danger
                      icon={<MinusCircleOutlined />}
                      onClick={() => remove(name)}
                    />
                  ) : null}
                </Flexbox>
              ))}
              <Form.Item style={{ marginTop: theme.marginSM }}>
                <Button
                  type="dashed"
                  onClick={() => add()}
                  block
                  icon={<PlusOutlined />}
                >
                  {t('modelMap.addMappingItem')}
                </Button>
              </Form.Item>
            </>
          )}
        </Form.List>
      </Form>
    </Modal>
  );
} 
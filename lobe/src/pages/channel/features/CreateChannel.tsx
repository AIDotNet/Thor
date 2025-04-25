import { useEffect, useState } from "react";
import { getModels, getTypes } from "../../../services/ModelService";
import { Add } from "../../../services/ChannelService";
import { message, Drawer, Checkbox, Space } from "antd";
import { Button, Select, Form, Input, Typography } from "antd";
import { getModelPrompt } from "../../../utils/render";
import { getList } from "../../../services/UserGroupService";
import { Flexbox } from "react-layout-kit";
import { useTranslation } from "react-i18next";
import { theme } from "antd";

const { Option } = Select;
const { Text } = Typography;

interface ICreateChannelProps {
  onSuccess: () => void;
  visible: boolean;
  onCancel: () => void;
}

export default function CreateChannel({
  onSuccess,
  visible,
  onCancel,
}: ICreateChannelProps) {
  const { t } = useTranslation();
  const { token } = theme.useToken();
  const [groups, setGroups] = useState<any[]>([]);

  useEffect(() => {
    getList()
      .then((res) => {
        if (res.success) {
          setGroups(res.data);
        }
      });
  }, []);

  // 字典models key, value类型
  const [types, setTypes] = useState<any>();
  const [models, setModels] = useState<any>();
  const [input, setInput] = useState<any>({
    name: "",
    type: "",
    address: "",
    other: "",
    key: "",
    models: [],
    groups: [],
    cache: false
  });

  type FieldType = {
    name?: string;
    type?: string;
    address?: string;
    other?: string;
    key?: string;
    models: string[];
    groups: string[];
    cache?: boolean;
  };

  function loading() {
    getTypes().then((res) => {
      if (res.success) {
        setTypes(res.data);
      } else {
        message.error(res.message || t('common.operateFailed'));
      }
    });

    getModels().then((res) => {
      if (res.success) {
        setModels(res.data);
      } else {
        message.error(res.message || t('common.operateFailed'));
      }
    });
  }

  useEffect(() => {
    if (visible) {
      loading();
    }
  }, [visible]);

  function handleSubmit(values: any) {
    if (input.type === "Claude" && input.cache) {
      values.other = "true";
    } else if (input.type === "Claude" && !input.cache) {
      values.other = "false";
    }

    // 判断是否选择了模型
    if (!values.models || values.models.length === 0) {
      message.error(t('channel.modelsRequired'));
      return;
    }

    Add(values).then((response) => {
      if (response.success) {
        message.success(t('common.createSuccess'));
        onSuccess();
      } else {
        message.error(response.message || t('common.operateFailed'));
      }
    });
  }

  return (
    <Drawer
      open={visible}
      width={500}
      title={t('channel.createChannel')}
      onClose={onCancel}
      styles={{
        body: {
          paddingBottom: 80,
        },
      }}
      footer={
        <Space style={{ 
          width: '100%', 
          justifyContent: 'flex-end',
          marginBottom: token.marginLG
        }}>
          <Button onClick={onCancel}>{t('common.cancel')}</Button>
          <Button 
            type="primary" 
            form="createChannelForm" 
            htmlType="submit"
          >
            {t('common.submit')}
          </Button>
        </Space>
      }
    >
      <Form
        id="createChannelForm"
        onFinish={handleSubmit}
        layout="vertical"
        initialValues={{
          cache: false
        }}
      >
        <Form.Item<FieldType>
          label={t('channel.channelName')}
          name="name"
          rules={[
            {
              required: true,
              message: t('channel.channelNameRequired'),
            },
            {
              min: 3,
              message: t('channel.channelNameMinLength'),
            },
          ]}
        >
          <Input
            value={input.name}
            onChange={(v) => {
              setInput({ ...input, name: v.target.value });
            }}
            placeholder={t('channel.enterChannelName')}
          />
        </Form.Item>

        <Form.Item<FieldType>
          rules={[
            {
              required: true,
              message: t('channel.platformTypeRequired'),
            },
          ]}
          name="type"
          label={t('channel.channelType')}
        >
          <Select
            placeholder={t('channel.selectPlatformType')}
            value={input.type}
            onChange={(v) => {
              setInput({ ...input, type: v });
            }}
            allowClear
          >
            {types &&
              Object.keys(types).map((key) => {
                return (
                  <Option key={key} value={types[key]}>
                    {key}
                  </Option>
                );
              })}
          </Select>
        </Form.Item>

        <Form.Item<FieldType> 
          label={t('channel.proxyAddress')} 
          name="address"
        >
          <Input 
            placeholder={t('channel.enterProxyAddress')} 
          />
        </Form.Item>
        
        {input.type === "AzureOpenAI" && (
          <Form.Item<FieldType>
            name="other"
            label={t('channel.version')}
          >
            <Select
              placeholder={t('channel.version')}
              value={input.other}
              defaultActiveFirstOption={true}
              defaultValue={"2024-10-01-preview"}
              onChange={(v) => {
                setInput({ ...input, other: v });
              }}
              allowClear
            >
              <Option key={"2024-05-01-preview"} value={"2024-05-01-preview"}>
                2024-05-01-preview
              </Option>
              <Option key={"2024-04-01-preview"} value={"2024-04-01-preview"}>
                2024-04-01-preview
              </Option>
              <Option key={"2024-06-01"} value={"2024-06-01"}>
                2024-06-01
              </Option>
              <Option key={"2024-10-01-preview"} value={"2024-10-01-preview"}>
                2024-10-01-preview
              </Option>
              <Option key={"2024-10-21"} value={"2024-10-21"}>
                2024-10-21
              </Option>
              <Option key={"2025-01-01-preview"} value={"2025-01-01-preview"}>
                2025-01-01-preview
              </Option>
            </Select>
          </Form.Item>
        )}
        
        {input.type === "AWSClaude" && (
          <Form.Item<FieldType>
            name="other"
            label={t('channel.region')}
            rules={[
              {
                required: true,
                message: t('channel.region'),
              },
            ]}
          >
            <Input
              value={input.other}
              onChange={(v) => {
                setInput({ ...input, other: v.target.value });
              }}
              placeholder={t('channel.region')}
            />
          </Form.Item>
        )}
        
        {input.type === "Claude" && (
          <Form.Item<FieldType>
            name="cache"
            label={t('channel.cacheEnabled')}
            valuePropName="checked"
          >
            <Checkbox
              checked={input.cache}
              onChange={(v) => {
                setInput({ ...input, cache: v.target.checked });
              }}
            />
          </Form.Item>
        )}
        
        {input.type === "Hunyuan" && (
          <Form.Item<FieldType>
            name="other"
            label={t('channel.region')}
          >
            <Select
              placeholder={t('channel.region')}
              value={input.other}
              onChange={(v) => {
                setInput({ ...input, other: v });
              }}
              allowClear
            >
              <Option key={"ap-beijing"} value={"ap-beijing"}>
                北京（ap-beijing）
              </Option>
              <Option key={"ap-guangzhou"} value={"ap-guangzhou"}>
                广州 （ap-guangzhou）
              </Option>
            </Select>
          </Form.Item>
        )}
        
        {input.type === "ErnieBot" && (
          <Form.Item<FieldType>
            name="other"
            label={t('channel.appId')}
            rules={[
              {
                required: true,
                message: t('channel.appId'),
              },
            ]}
          >
            <Input
              value={input.other}
              onChange={(v) => {
                setInput({ ...input, other: v.target.value });
              }}
              placeholder={t('channel.appId')}
            />
          </Form.Item>
        )}
        
        <Form.Item<FieldType> 
          label={t('channel.key')} 
          name="key"
        >
          <Input.Password
            placeholder={getModelPrompt(input.type)}
            autoComplete="token"
          />
        </Form.Item>
        
        <Form.Item<FieldType>
          name="groups"
          label={t('channel.groups')}
          rules={[{ required: true, message: t('channel.groupsRequired') }]}
        >
          <Select
            placeholder={t('channel.selectGroups')}
            mode="tags"
            options={groups?.map((group: any) => {
              return {
                label: <Flexbox gap={8} horizontal>
                  <span>{group.name}</span>
                  <span style={{ fontSize: 12, color: token.colorTextSecondary }}>{group.description}</span>
                  <span style={{ fontSize: 12, color: token.colorTextSecondary }}>
                    <Text type="secondary">{t('channel.rate')}：</Text>
                    {group.rate}
                  </span>
                </Flexbox>,
                value: group.code
              }
            })}
            value={input.groups}
            onChange={(v) => {
              setInput({ ...input, groups: v });
            }}
          />
        </Form.Item>
        
        <Form.Item
          name="models"
          label={t('channel.models')}
          rules={[
            {
              required: true,
              message: t('channel.modelsRequired'),
            },
          ]}
        >
          <Select
            placeholder={t('channel.selectModels')}
            defaultActiveFirstOption={true}
            mode="tags"
            value={input.models}
            onChange={(v) => {
              setInput({ ...input, models: v });
            }}
            allowClear
            optionFilterProp="children"
            showSearch
          >
            {models &&
              models.map((model: any) => {
                return (
                  <Option key={model} value={model}>
                    {model}
                  </Option>
                );
              })}
          </Select>
        </Form.Item>
      </Form>
    </Drawer>
  );
}

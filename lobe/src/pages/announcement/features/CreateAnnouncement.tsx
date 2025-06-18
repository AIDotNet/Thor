import React, { useState } from 'react';
import { Modal, Form, Input, Select, Switch, InputNumber, DatePicker, message, Tabs, Card, Typography } from 'antd';
import { createAnnouncement } from '../../../services/AnnouncementService';
import ReactMarkdown from 'react-markdown';
import styled from 'styled-components';

const { TextArea } = Input;
const { Option } = Select;
const { Text } = Typography;

const MarkdownPreview = styled.div`
  .markdown-content {
    font-size: 14px;
    line-height: 1.6;
    color: #666;
    padding: 12px;
    border: 1px solid #d9d9d9;
    border-radius: 6px;
    background: #fafafa;
    min-height: 120px;
    
    h1, h2, h3, h4, h5, h6 {
      margin-top: 16px;
      margin-bottom: 8px;
      font-weight: 600;
      color: #262626;
    }
    
    h1 { font-size: 20px; }
    h2 { font-size: 18px; }
    h3 { font-size: 16px; }
    h4 { font-size: 14px; }
    h5 { font-size: 12px; }
    h6 { font-size: 12px; }
    
    p {
      margin-bottom: 12px;
    }
    
    ul, ol {
      margin-bottom: 12px;
      padding-left: 20px;
    }
    
    li {
      margin-bottom: 4px;
    }
    
    code {
      background: #f5f5f5;
      padding: 2px 6px;
      border-radius: 3px;
      font-family: 'Consolas', 'Monaco', monospace;
      font-size: 12px;
    }
    
    pre {
      background: #f5f5f5;
      padding: 12px;
      border-radius: 6px;
      overflow-x: auto;
      margin-bottom: 12px;
      
      code {
        background: none;
        padding: 0;
      }
    }
    
    blockquote {
      border-left: 4px solid #1890ff;
      padding-left: 12px;
      margin: 12px 0;
      color: #595959;
      font-style: italic;
    }
    
    a {
      color: #1890ff;
      text-decoration: none;
      
      &:hover {
        text-decoration: underline;
      }
    }
    
    img {
      max-width: 100%;
      height: auto;
      border-radius: 4px;
      margin: 8px 0;
    }
    
    table {
      width: 100%;
      border-collapse: collapse;
      margin: 12px 0;
      
      th, td {
        border: 1px solid #f0f0f0;
        padding: 8px 12px;
        text-align: left;
      }
      
      th {
        background: #fafafa;
        font-weight: 600;
      }
    }
  }
`;

interface CreateAnnouncementProps {
  visible: boolean;
  onCancel: () => void;
  onSuccess: () => void;
}

const CreateAnnouncement: React.FC<CreateAnnouncementProps> = ({ visible, onCancel, onSuccess }) => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [content, setContent] = useState('');

  // 当模态框关闭时重置内容
  React.useEffect(() => {
    if (!visible) {
      setContent('');
    }
  }, [visible]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      
      // 确保内容值被正确获取
      const submitData = {
        ...values,
        content: content || values.content, // 优先使用 state 中的 content
        expireTime: values.expireTime ? values.expireTime.toISOString() : null
      };

      const result = await createAnnouncement(submitData);
      
      if (result.success) {
        message.success('公告创建成功');
        form.resetFields();
        setContent('');
        onSuccess();
      } else {
        message.error(result.message || '创建失败');
      }
    } catch (error) {
      console.error('创建公告失败:', error);
      message.error('创建失败');
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = () => {
    form.resetFields();
    setContent('');
    onCancel();
  };

  const handleContentChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    const value = e.target.value;
    setContent(value);
    form.setFieldsValue({ content: value });
  };

  const tabItems = [
    {
      key: 'edit',
      label: '编辑',
      children: (
        <TextArea
          rows={8}
          placeholder="请输入公告内容，支持 Markdown 格式&#10;&#10;示例：&#10;# 一级标题&#10;## 二级标题&#10;**粗体文字**&#10;*斜体文字*&#10;`代码`&#10;- 列表项&#10;> 引用&#10;[链接文字](http://example.com)"
          showCount
          maxLength={2000}
          value={content}
          onChange={handleContentChange}
        />
      )
    },
    {
      key: 'preview',
      label: '预览',
      children: (
        <MarkdownPreview>
          <div className="markdown-content">
            {content ? (
              <ReactMarkdown>{content}</ReactMarkdown>
            ) : (
              <Text type="secondary">请在编辑区域输入内容以查看预览效果</Text>
            )}
          </div>
        </MarkdownPreview>
      )
    }
  ];

  return (
    <Modal
      title="创建公告"
      open={visible}
      onOk={handleSubmit}
      onCancel={handleCancel}
      confirmLoading={loading}
      width={700}
      destroyOnClose
    >
      <Form
        form={form}
        layout="vertical"
        initialValues={{
          type: 'info',
          enabled: true,
          pinned: false,
          order: 0
        }}
      >
        <Form.Item
          name="title"
          label="公告标题"
          rules={[{ required: true, message: '请输入公告标题' }]}
        >
          <Input placeholder="请输入公告标题" />
        </Form.Item>

        <Form.Item
          name="content"
          label={
            <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
              公告内容
              <Text type="secondary" style={{ fontSize: '12px' }}>
                支持 Markdown 格式
              </Text>
            </div>
          }
          rules={[{ required: true, message: '请输入公告内容' }]}
        >
          <div>
            <Tabs items={tabItems} />
          </div>
        </Form.Item>

        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
          <Form.Item
            name="type"
            label="公告类型"
            rules={[{ required: true, message: '请选择公告类型' }]}
          >
            <Select placeholder="请选择公告类型">
              <Option value="info">信息</Option>
              <Option value="success">成功</Option>
              <Option value="warning">警告</Option>
              <Option value="error">错误</Option>
            </Select>
          </Form.Item>

          <Form.Item
            name="order"
            label="排序权重"
            tooltip="数值越大越靠前"
          >
            <InputNumber min={0} placeholder="排序权重" style={{ width: '100%' }} />
          </Form.Item>
        </div>

        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
          <Form.Item
            name="enabled"
            label="是否启用"
            valuePropName="checked"
          >
            <Switch checkedChildren="启用" unCheckedChildren="禁用" />
          </Form.Item>

          <Form.Item
            name="pinned"
            label="是否置顶"
            valuePropName="checked"
          >
            <Switch checkedChildren="置顶" unCheckedChildren="普通" />
          </Form.Item>
        </div>

        <Form.Item
          name="expireTime"
          label="过期时间"
          tooltip="不设置则永不过期"
        >
          <DatePicker
            showTime
            placeholder="选择过期时间（可选）"
            style={{ width: '100%' }}
          />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default CreateAnnouncement; 
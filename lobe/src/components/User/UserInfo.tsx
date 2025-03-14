import { useState, useEffect } from 'react';
import { Form, Input, Button, message, Avatar, Card, Row, Col, Divider, Tag, Spin } from 'antd';
import { UserOutlined, CheckCircleOutlined, CloseCircleOutlined } from '@ant-design/icons';
import { updateInfo } from '../../services/UserService';
import { renderNumber, renderQuota } from '../../utils/render';
import { InviteInfo } from '../../services/SystemService';

interface UserInfoProps {
  user: any;
  onUpdate?: () => void;
}

const UserInfo = ({ user, onUpdate }: UserInfoProps) => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [inviteInfo, setInviteInfo] = useState({
    credit: 0,
    count: 0,
    limit: 0
  });

  useEffect(() => {
    // 获取邀请信息
    const fetchInviteInfo = async () => {
      try {
        const response = await InviteInfo();
        if (response.success) {
          setInviteInfo({
            credit: response.data.credit || 0,
            count: response.data.count || 0,
            limit: response.data.limit || 0
          });
        }
      } catch (error) {
        console.error('获取邀请信息失败', error);
      }
    };
    
    fetchInviteInfo();
  }, []);

  useEffect(() => {
    if (user) {
      form.setFieldsValue({
        userName: user.userName,
        email: user.email,
      });
    }
  }, [user, form]);

  const handleSubmit = async (values: any) => {
    setLoading(true);
    try {
      const response = await updateInfo({
        id: user.id,
        ...values
      });
      
      if (response.success) {
        message.success('信息更新成功');
        setIsEditing(false);
        if (onUpdate) onUpdate();
      } else {
        message.error(response.message || '更新失败');
      }
    } catch (error) {
      message.error('更新过程中出错');
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const formatDate = (dateString: string) => {
    if (!dateString) return '未知';
    return dateString;
  };

  const renderUserInfo = () => (
    <div style={{
      height:'calc(100vh - 190px)',
    }}>
      <Row justify="center" style={{ marginBottom: 24 }}>
        <Avatar size={80} icon={<UserOutlined />} src={user.avatar} />
      </Row>
      
      <Row gutter={[16, 16]}>
        <Col span={12}>
          <Card size="small" title="基本信息">
            <p><strong>用户名：</strong> {user.userName}</p>
            <p><strong>电子邮箱：</strong> {user.email}</p>
            <p><strong>角色：</strong> {user.role}</p>
            <p>
              <strong>用户组：</strong> 
              {user.groups && user.groups.map((group: any) => (
                <Tag color="blue" key={group} style={{ marginRight: 5 }}>
                  {group}
                </Tag>
              ))}
            </p>
            <p><strong>注册时间：</strong> {formatDate(user.createdAt)}</p>
            <p><strong>最后更新：</strong> {formatDate(user.updatedAt)}</p>
          </Card>
        </Col>
        
        <Col span={12}>
          <Card size="small" title="账户信息">
            <p>
              <strong>账户状态：</strong> 
              {user.isDisabled ? 
                <Tag icon={<CloseCircleOutlined />} color="error">已禁用</Tag> : 
                <Tag icon={<CheckCircleOutlined />} color="success">正常</Tag>
              }
            </p>
            <p><strong>剩余额度：</strong> {renderQuota(user.residualCredit || 0)}</p>
            <p><strong>Token消耗：</strong> {renderNumber(user.consumeToken || 0)}</p>
            <p><strong>请求次数：</strong> {renderNumber(user.requestCount || 0)}</p>
          </Card>
        </Col>
      </Row>
      
      <Divider />
      <Row justify="center" style={{ marginBottom: 24 }}>
        <Card size="small" title="邀请信息" style={{ width: '100%' }}>
          <p><strong>邀请奖励：</strong> {renderQuota(inviteInfo.credit || 0)}</p>
          <p><strong>已邀请人数：</strong> {inviteInfo.count || 0}</p>
          <p><strong>剩余可邀请：</strong> {inviteInfo.limit || 0}</p>
          <Button 
            type="primary"
            block
            onClick={() => {
              const url = window.location.origin + '/register?inviteCode=' + user.id;
              navigator.clipboard.writeText(url);
              message.success('邀请码已复制到剪贴板');
            }}>
            复制邀请码，分享给好友，您将获得{renderQuota(inviteInfo.credit || 0)}奖励
          </Button>
        </Card>
      </Row>

      <Button 
        type="primary" 
        onClick={() => setIsEditing(true)}
        block
      >
        编辑基本信息
      </Button>
    </div>
  );

  const renderEditForm = () => (
    <Form
      form={form}
      layout="vertical"
      onFinish={handleSubmit}
    >
      <Form.Item
        label="用户名"
        name="userName"
        rules={[{ required: true, message: '请输入用户名' }]}
      >
        <Input />
      </Form.Item>
      
      <Form.Item
        label="电子邮箱"
        name="email"
        rules={[
          { required: true, message: '请输入电子邮箱' },
          { type: 'email', message: '请输入有效的电子邮箱地址' }
        ]}
      >
        <Input />
      </Form.Item>
      
      <Form.Item>
        <Button.Group style={{ width: '100%' }}>
          <Button 
            style={{ width: '50%' }}
            onClick={() => setIsEditing(false)}
          >
            取消
          </Button>
          <Button 
            type="primary" 
            htmlType="submit" 
            loading={loading}
            style={{ width: '50%' }}
          >
            保存
          </Button>
        </Button.Group>
      </Form.Item>
    </Form>
  );

  if (!user || Object.keys(user).length === 0) {
    return <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}><Spin /></div>;
  }

  return (
    <div style={{ maxWidth: 800, margin: '0 auto' }}>
      {isEditing ? renderEditForm() : renderUserInfo()}
    </div>
  );
};

export default UserInfo; 
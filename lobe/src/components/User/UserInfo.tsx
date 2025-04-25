import { useState, useEffect } from 'react';
import { Form, theme, Input, Button, message, Card, Row, Col, Divider, Tag, Spin, Avatar, Modal, Tabs, QRCode, Typography, Upload } from 'antd';
import { CheckCircleOutlined, CloseCircleOutlined, UserOutlined, LockOutlined, CopyOutlined, UploadOutlined, MailOutlined } from '@ant-design/icons';
import { updateInfo, updatePassword } from '../../services/UserService';
import { renderNumber, renderQuota } from '../../utils/render';
import { InviteInfo } from '../../services/SystemService';
import type { UploadProps } from 'antd';
import '../../styles/userInfo.css'; // 导入样式文件
import { useTranslation } from 'react-i18next';

const { TabPane } = Tabs;
const { useToken } = theme;

interface UserInfoProps {
  user: any;
  onUpdate?: () => void;
}

const UserInfo = ({ user, onUpdate }: UserInfoProps) => {
  const { t } = useTranslation();
  const { token } = useToken();
  const [form] = Form.useForm();
  const [passwordForm] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [passwordModalVisible, setPasswordModalVisible] = useState(false);
  const [inviteInfo, setInviteInfo] = useState({
    credit: 0,
    count: 0,
    limit: 0
  });
  const [activeTab, setActiveTab] = useState('1');

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
        console.error(t('common.failed'), error);
      }
    };
    
    fetchInviteInfo();
  }, [t]);

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
        message.success(t('userProfile.updateSuccess'));
        setIsEditing(false);
        if (onUpdate) onUpdate();
      } else {
        message.error(response.message || t('userProfile.updateFailed'));
      }
    } catch (error) {
      message.error(t('userProfile.updateFailed'));
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const handlePasswordChange = async (values: any) => {
    setLoading(true);
    try {
      const response = await updatePassword({
        id: user.id,
        oldPassword: values.oldPassword,
        newPassword: values.newPassword
      });
      
      if (response.success) {
        message.success(t('userProfile.passwordSuccess'));
        setPasswordModalVisible(false);
        passwordForm.resetFields();
      } else {
        message.error(response.message || t('userProfile.passwordFailed'));
      }
    } catch (error) {
      console.error(t('userProfile.passwordFailed'), error);
      message.error(t('userProfile.passwordFailed'));
    } finally {
      setLoading(false);
    }
  };

  const formatDate = (dateString: string) => {
    if (!dateString) return '未知';
    try {
      const date = new Date(dateString);
      return date.toLocaleString('zh-CN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
      });
    } catch (e) {
      return dateString;
    }
  };

  // 头像上传配置
  const uploadProps: UploadProps = {
    name: 'avatar',
    action: '/api/v1/user/upload-avatar', // 替换为实际的上传API
    showUploadList: false,
    headers: {
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
    beforeUpload: (file) => {
      const isImage = file.type.startsWith('image/');
      if (!isImage) {
        message.error('只能上传图片文件!');
      }
      const isLt2M = file.size / 1024 / 1024 < 2;
      if (!isLt2M) {
        message.error('图片必须小于2MB!');
      }
      return isImage && isLt2M;
    },
    onChange(info) {
      if (info.file.status === 'done') {
        message.success('头像上传成功');
        // 这里应该更新用户头像
        if (onUpdate) onUpdate();
      } else if (info.file.status === 'error') {
        message.error('头像上传失败');
      }
    },
  };

  const getInviteUrl = () => {
    return window.location.origin + '/register?inviteCode=' + user.id;
  };

  const copyInviteUrl = () => {
    const url = getInviteUrl();
    navigator.clipboard.writeText(url);
    message.success(t('userProfile.inviteCopied'));
  };

  const renderUserInfo = () => (
    <div style={{
      height:'calc(100vh - 290px)',
      overflowY:'auto', 
      padding:'20px 10px',
      overflowX:'hidden',
    }}>
      <Tabs 
        activeKey={activeTab} 
        onChange={setActiveTab} 
        centered
        type="card"
        className="user-profile-tabs"
      >
        <TabPane tab="个人资料" key="1">
          <Row justify="center" style={{ marginBottom: 24 }}>
            <Col>
              <div style={{ textAlign: 'center' }}>
                <div style={{ position: 'relative', display: 'inline-block' }}>
                  <Avatar 
                    size={120} 
                    icon={<UserOutlined />} 
                    src={user.avatar}
                    style={{ 
                      marginBottom: 16,
                      boxShadow: '0 2px 10px rgba(0,0,0,0.1)',
                      border: '4px solid #fff'
                    }}
                  />
                  <Upload {...uploadProps}>
                    <Button 
                      icon={<UploadOutlined />} 
                      size="small"
                      style={{ 
                        position: 'absolute', 
                        bottom: 0, 
                        right: 0,
                        borderRadius: '50%',
                        boxShadow: '0 2px 8px rgba(0,0,0,0.15)',
                        background: '#1890ff',
                        color: '#fff',
                        border: '2px solid #fff'
                      }}
                    />
                  </Upload>
                </div>
                <Typography.Title level={3} style={{ marginBottom: 4 }}>{user.userName}</Typography.Title>
                <Typography.Text type="secondary">{user.email}</Typography.Text>
              </div>
            </Col>
          </Row>
          
          <Row gutter={[24, 24]}>
            <Col xs={24} md={12}>
              <Card 
                size="small" 
                title={<><UserOutlined style={{ marginRight: 8 }} />基本信息</>} 
                bordered={true} 
                className="info-card"
                style={{ 
                  borderRadius: '8px',
                  boxShadow: '0 1px 5px rgba(0,0,0,0.05)'
                }}
              >
                <div className="info-item">
                  <span className="info-label">用户名：</span>
                  <span className="info-value">{user.userName}</span>
                </div>
                <div className="info-item">
                  <span className="info-label">电子邮箱：</span>
                  <span className="info-value">{user.email}</span>
                </div>
                <div className="info-item">
                  <span className="info-label">角色：</span>
                  <span className="info-value">{user.role}</span>
                </div>
                <div className="info-item">
                  <span className="info-label">用户组：</span>
                  <span className="info-value">
                    {user.groups && user.groups.map((group: any) => (
                      <Tag color="blue" key={group} style={{ marginRight: 5 }}>
                        {group}
                      </Tag>
                    ))}
                  </span>
                </div>
                <div className="info-item">
                  <span className="info-label">注册时间：</span>
                  <span className="info-value">{formatDate(user.createdAt)}</span>
                </div>
                <div className="info-item">
                  <span className="info-label">最后更新：</span>
                  <span className="info-value">{formatDate(user.updatedAt)}</span>
                </div>
              </Card>
            </Col>
            
            <Col xs={24} md={12}>
              <Card 
                size="small" 
                title={<><LockOutlined style={{ marginRight: 8 }} />账户信息</>} 
                bordered={true} 
                className="info-card"
                style={{ 
                  borderRadius: '8px',
                  boxShadow: '0 1px 5px rgba(0,0,0,0.05)'
                }}
              >
                <div className="info-item">
                  <span className="info-label">账户状态：</span>
                  <span className="info-value">
                    {user.isDisabled ? 
                      <Tag icon={<CloseCircleOutlined />} color="error">已禁用</Tag> : 
                      <Tag icon={<CheckCircleOutlined />} color="success">正常</Tag>
                    }
                  </span>
                </div>
                <div className="info-item">
                  <span className="info-label">剩余额度：</span>
                  <span className="info-value highlight">{renderQuota(user.residualCredit || 0)}</span>
                </div>
                <div className="info-item">
                  <span className="info-label">Token消耗：</span>
                  <span className="info-value">{renderNumber(user.consumeToken || 0)}</span>
                </div>
                <div className="info-item">
                  <span className="info-label">请求次数：</span>
                  <span className="info-value">{renderNumber(user.requestCount || 0)}</span>
                </div>
              </Card>
            </Col>
          </Row>
          
          <Row justify="center" style={{ marginTop: 24 }}>
            <Col xs={24} sm={16} md={12}>
              <Button.Group style={{ width: '100%', boxShadow: '0 2px 8px rgba(0,0,0,0.1)', borderRadius: '8px', overflow: 'hidden' }}>
                <Button 
                  type="primary" 
                  onClick={() => setIsEditing(true)}
                  icon={<UserOutlined />}
                  style={{ width: '50%', height: '40px' }}
                >
                  编辑基本信息
                </Button>
                <Button 
                  type="default" 
                  onClick={() => setPasswordModalVisible(true)}
                  icon={<LockOutlined />}
                  style={{ width: '50%', height: '40px' }}
                >
                  修改密码
                </Button>
              </Button.Group>
            </Col>
          </Row>
        </TabPane>
        
        <TabPane tab="邀请奖励" key="2">
          <Card 
            bordered={true}
            className="invite-card"
            style={{ 
              maxWidth: 600, 
              margin: '0 auto',
              borderRadius: '12px',
              boxShadow: '0 2px 12px rgba(0,0,0,0.08)'
            }}
          >
            <div style={{ textAlign: 'center', marginBottom: 20 }}>
              <Typography.Title level={4}>邀请好友，双方共享奖励</Typography.Title>
              <Typography.Text type="secondary">每成功邀请一位好友注册，您将获得额外奖励</Typography.Text>
            </div>
            
            <Row gutter={[16, 24]} style={{ marginBottom: 24 }}>
              <Col xs={24} sm={8}>
                <div className="stat-box">
                  <div className="stat-icon" style={{ color: '#1890ff' }}>
                    <svg viewBox="64 64 896 896" focusable="false" data-icon="gift" width="36px" height="36px" fill="currentColor" aria-hidden="true"><path d="M880 310H732.4c13.6-21.4 21.6-46.8 21.6-74 0-76.1-61.9-138-138-138-41.4 0-78.7 18.4-104 47.4-25.3-29-62.6-47.4-104-47.4-76.1 0-138 61.9-138 138 0 27.2 7.9 52.6 21.6 74H144c-17.7 0-32 14.3-32 32v200c0 4.4 3.6 8 8 8h40v344c0 17.7 14.3 32 32 32h640c17.7 0 32-14.3 32-32V550h40c4.4 0 8-3.6 8-8V342c0-17.7-14.3-32-32-32zm-334-74c0-38.6 31.4-70 70-70s70 31.4 70 70c0 38.6-31.4 70-70 70h-70v-70zm-138-70c38.6 0 70 31.4 70 70v70h-70c-38.6 0-70-31.4-70-70s31.4-70 70-70zM180 482V378h298v104H180zm48 68h250v308H228V550zm568 308H546V550h250v308zm48-376H546V378h298v104z"></path></svg>
                  </div>
                  <div className="stat-value" style={{ color: '#1890ff' }}>
                    {renderQuota(inviteInfo.credit || 0)}
                  </div>
                  <div className="stat-label">邀请奖励</div>
                </div>
              </Col>
              <Col xs={24} sm={8}>
                <div className="stat-box">
                  <div className="stat-icon" style={{ color: '#52c41a' }}>
                    <svg viewBox="64 64 896 896" focusable="false" data-icon="team" width="36px" height="36px" fill="currentColor" aria-hidden="true"><path d="M824.2 699.9a301.55 301.55 0 00-86.4-60.4C783.1 602.8 812 546.8 812 484c0-110.8-92.4-201.7-203.2-200-109.1 1.7-197 90.6-197 200 0 62.8 29 118.8 74.2 155.5a300.95 300.95 0 00-86.4 60.4C345 754.6 314 826.8 312 903.8a8 8 0 008 8.2h56c4.3 0 7.9-3.4 8-7.7 1.9-58 25.4-112.3 66.7-153.5A226.62 226.62 0 01612 684c60.9 0 118.2 23.7 161.3 66.8C814.5 792 838 846.3 840 904.3c.1 4.3 3.7 7.7 8 7.7h56a8 8 0 008-8.2c-2-77-33-149.2-87.8-203.9zM612 612c-34.2 0-66.4-13.3-90.5-37.5a126.86 126.86 0 01-37.5-91.8c.3-32.8 13.4-64.5 36.3-88 24-24.6 56.1-38.3 90.4-38.7 33.9-.3 66.8 12.9 91 36.6 24.8 24.3 38.4 56.8 38.4 91.4 0 34.2-13.3 66.3-37.5 90.5A127.3 127.3 0 01612 612zM361.5 510.4c-.9-8.7-1.4-17.5-1.4-26.4 0-15.9 1.5-31.4 4.3-46.5.7-3.6-1.2-7.3-4.5-8.8-13.6-6.1-26.1-14.5-36.9-25.1a127.54 127.54 0 01-38.7-95.4c.9-32.1 13.8-62.6 36.3-85.6 24.7-25.3 57.9-39.1 93.2-38.7 31.9.3 62.7 12.6 86 34.4 7.9 7.4 14.7 15.6 20.4 24.4 2 3.1 5.9 4.4 9.3 3.2 17.6-6.1 36.2-10.4 55.3-12.4 5.6-.6 8.8-6.6 6.3-11.6-32.5-64.3-98.9-108.7-175.7-109.9-110.9-1.7-203.3 89.2-203.3 199.9 0 62.8 28.9 118.8 74.2 155.5-31.8 14.7-61.1 35-86.5 60.4-54.8 54.7-85.8 126.9-87.8 204a8 8 0 008 8.2h56.1c4.3 0 7.9-3.4 8-7.7 1.9-58 25.4-112.3 66.7-153.5 29.4-29.4 65.4-49.8 104.7-59.7 3.9-1 6.5-4.7 6-8.7z"></path></svg>
                  </div>
                  <div className="stat-value" style={{ color: '#52c41a' }}>
                    {inviteInfo.count || 0}
                  </div>
                  <div className="stat-label">已邀请人数</div>
                </div>
              </Col>
              <Col xs={24} sm={8}>
                <div className="stat-box">
                  <div className="stat-icon" style={{ color: '#fa8c16' }}>
                    <svg viewBox="64 64 896 896" focusable="false" data-icon="user-add" width="36px" height="36px" fill="currentColor" aria-hidden="true"><path d="M678.3 642.4c24.2-13 51.9-20.4 81.4-20.4h.1c3 0 4.4-3.6 2.2-5.6a371.67 371.67 0 00-103.7-65.8c-.4-.2-.8-.3-1.2-.5C719.2 505 759.6 431.7 759.6 349c0-137-110.8-248-247.5-248S264.7 212 264.7 349c0 82.7 40.4 156 102.6 201.1-.4.2-.8.3-1.2.5-44.7 18.9-84.8 46-119.3 80.6a373.42 373.42 0 00-80.4 119.5A373.6 373.6 0 00137 888.8a8 8 0 008 8.2h59.9c4.3 0 7.9-3.5 8-7.8 2-77.2 32.9-149.5 87.6-204.3C357 628.2 432.2 597 512.2 597c56.7 0 111.1 15.7 158 45.1a8.1 8.1 0 008.1.3zM512.2 521c-45.8 0-88.9-17.9-121.4-50.4A171.2 171.2 0 01340.5 349c0-45.9 17.9-89.1 50.3-121.6S466.3 177 512.2 177s88.9 17.9 121.4 50.4A171.2 171.2 0 01683.9 349c0 45.9-17.9 89.1-50.3 121.6C601.1 503.1 558 521 512.2 521zM880 759h-84v-84c0-4.4-3.6-8-8-8h-56c-4.4 0-8 3.6-8 8v84h-84c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h84v84c0 4.4 3.6 8 8 8h56c4.4 0 8-3.6 8-8v-84h84c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8z"></path></svg>
                  </div>
                  <div className="stat-value" style={{ color: '#fa8c16' }}>
                    {inviteInfo.limit || 0}
                  </div>
                  <div className="stat-label">剩余可邀请</div>
                </div>
              </Col>
            </Row>
            
            <Divider style={{ margin: '12px 0 24px' }} />
            
            <Row gutter={24} align="middle">
              <Col xs={24} md={12} style={{ textAlign: 'center', marginBottom: 24 }}>
                <div className="qrcode-container">
                  <QRCode
                    value={getInviteUrl()}
                    size={180}
                    style={{ 
                      margin: '0 auto 16px',
                      padding: '8px',
                      background: token.colorBgContainer,
                      boxShadow: '0 2px 8px rgba(0,0,0,0.1)',
                      borderRadius: '8px'
                    }}
                  />
                  <Typography.Text type="secondary">扫描二维码邀请好友</Typography.Text>
                </div>
              </Col>
              
              <Col xs={24} md={12} style={{ marginBottom: 0 }}>
                <div className="invite-link-container">
                  <div style={{ 
                    padding: '16px', 
                    borderRadius: '8px',
                    marginBottom: '16px'
                  }}>
                    <Typography.Paragraph 
                      copyable={{ 
                        text: getInviteUrl(),
                        icon: [
                          <CopyOutlined key="copy-icon" />,
                          <CheckCircleOutlined key="copied-icon" style={{ color: '#52c41a' }} />
                        ]
                      }}
                      style={{ 
                        marginBottom: 0,
                        wordBreak: 'break-all'
                      }}
                    >
                      {getInviteUrl()}
                    </Typography.Paragraph>
                  </div>
                  
                  <Button 
                    type="primary"
                    icon={<CopyOutlined />}
                    onClick={copyInviteUrl}
                    block
                    size="large"
                    style={{ 
                      height: '46px',
                      borderRadius: '8px',
                      boxShadow: '0 2px 8px rgba(24,144,255,0.2)'
                    }}
                  >
                    复制邀请链接
                  </Button>
                  
                  <div style={{ textAlign: 'center', marginTop: '16px' }}>
                    <Typography.Text type="secondary">
                      每邀请一位好友，您将获得 <Typography.Text strong style={{ color: '#1890ff' }}>{renderQuota(inviteInfo.credit || 0)}</Typography.Text> 奖励
                    </Typography.Text>
                  </div>
                </div>
              </Col>
            </Row>
          </Card>
        </TabPane>
      </Tabs>
    </div>
  );

  const renderEditForm = () => (
    <div style={{ maxWidth: 500, margin: '0 auto', padding: '20px 0' }}>
      <Card 
        title="编辑个人信息" 
        bordered={true}
        style={{ 
          borderRadius: '12px',
          boxShadow: '0 2px 12px rgba(0,0,0,0.08)'
        }}
      >
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
            <Input prefix={<UserOutlined />} size="large" />
          </Form.Item>
          
          <Form.Item
            label="电子邮箱"
            name="email"
            rules={[
              { required: true, message: '请输入电子邮箱' },
              { type: 'email', message: '请输入有效的电子邮箱地址' }
            ]}
          >
            <Input prefix={<MailOutlined />} size="large" />
          </Form.Item>
          
          <Form.Item>
            <Button.Group style={{ width: '100%', borderRadius: '8px', overflow: 'hidden' }}>
              <Button 
                style={{ width: '50%', height: '40px' }}
                onClick={() => setIsEditing(false)}
              >
                取消
              </Button>
              <Button 
                type="primary" 
                htmlType="submit" 
                loading={loading}
                style={{ width: '50%', height: '40px' }}
              >
                保存
              </Button>
            </Button.Group>
          </Form.Item>
        </Form>
      </Card>
    </div>
  );

  if (!user || Object.keys(user).length === 0) {
    return (
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
        <Spin size="large" tip="加载中..." />
      </div>
    );
  }

  return (
    <div style={{ maxWidth: 900, margin: '0 auto' }}>
      {isEditing ? renderEditForm() : renderUserInfo()}
      
      <Modal
        title="修改密码"
        open={passwordModalVisible}
        footer={null}
        onCancel={() => setPasswordModalVisible(false)}
        width={400}
        style={{ borderRadius: '12px', overflow: 'hidden' }}
        bodyStyle={{ padding: '24px' }}
      >
        <Form
          form={passwordForm}
          layout="vertical"
          onFinish={handlePasswordChange}
        >
          <Form.Item
            label="当前密码"
            name="oldPassword"
            rules={[{ required: true, message: '请输入当前密码' }]}
          >
            <Input.Password prefix={<LockOutlined />} size="large" />
          </Form.Item>
          
          <Form.Item
            label="新密码"
            name="newPassword"
            rules={[
              { required: true, message: '请输入新密码' },
              { min: 8, message: '密码长度不能少于8个字符' }
            ]}
          >
            <Input.Password prefix={<LockOutlined />} size="large" />
          </Form.Item>
          
          <Form.Item
            label="确认新密码"
            name="confirmPassword"
            dependencies={['newPassword']}
            rules={[
              { required: true, message: '请确认新密码' },
              ({ getFieldValue }) => ({
                validator(_, value) {
                  if (!value || getFieldValue('newPassword') === value) {
                    return Promise.resolve();
                  }
                  return Promise.reject(new Error('两次输入的密码不一致'));
                },
              }),
            ]}
          >
            <Input.Password prefix={<LockOutlined />} size="large" />
          </Form.Item>
          
          <Form.Item>
            <Button.Group style={{ width: '100%', borderRadius: '8px', overflow: 'hidden' }}>
              <Button 
                style={{ width: '50%', height: '40px' }}
                onClick={() => setPasswordModalVisible(false)}
              >
                取消
              </Button>
              <Button 
                type="primary" 
                htmlType="submit" 
                loading={loading}
                style={{ width: '50%', height: '40px' }}
              >
                确认修改
              </Button>
            </Button.Group>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default UserInfo; 
import React, { useState, useEffect } from 'react';
import { Modal, Button, Tag, Space, Typography, Avatar, Divider, theme, Card, Badge, Timeline } from 'antd';
import {
  CloseOutlined,
  InfoCircleOutlined,
  CheckCircleOutlined,
  ExclamationCircleOutlined,
  CloseCircleOutlined,
  CalendarOutlined,
  StarFilled,
  BellOutlined,
  SoundOutlined,
  ClockCircleOutlined,
  PushpinOutlined,
  EyeOutlined,
  CheckOutlined
} from '@ant-design/icons';
import { getActiveAnnouncements } from '../../services/AnnouncementService';
import ReactMarkdown from 'react-markdown';
import styled from 'styled-components';
import { t } from '../../utils/i18n';

const { Title, Text, Paragraph } = Typography;

// ChatGPT-inspired design system
const ChatGPTContainer = styled.div<{ $token: any }>`
  border-radius: 12px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.12);
  border: 1px solid ${props => props.$token.colorBorderSecondary};
  overflow: hidden;
  max-width: 800px;
  margin: 0 auto;
  
  @media (max-width: 768px) {
    margin: 8px;
    max-width: calc(100vw - 16px);
    border-radius: 8px;
  }
  
  @media (max-width: 480px) {
    margin: 4px;
    max-width: calc(100vw - 8px);
    border-radius: 4px;
  }
`;

const Header = styled.div<{ $token: any }>`
  padding: 24px 32px;
  border-bottom: 1px solid ${props => props.$token.colorBorderSecondary};
  display: flex;
  align-items: center;
  justify-content: space-between;
  
  @media (max-width: 768px) {
    padding: 12px 16px;
    flex-direction: column;
    align-items: flex-start;
    gap: 8px;
  }
  
  @media (max-width: 480px) {
    padding: 8px 12px;
  }
`;

const HeaderTitle = styled.div`
  display: flex;
  align-items: center;
  gap: 12px;
`;

const HeaderActions = styled.div`
  display: flex;
  align-items: center;
  gap: 8px;
`;

const AnnouncementCard = styled(Card)<{ $type?: string; $isPinned?: boolean; $token: any }>`
  margin: 16px 0;
  border-radius: 8px;
  border: 1px solid ${props => {
    switch (props.$type) {
      case 'success': return props.$token.colorSuccessBorder;
      case 'warning': return props.$token.colorWarningBorder;
      case 'error': return props.$token.colorErrorBorder;
      default: return props.$token.colorBorderSecondary;
    }
  }};
  
  ${props => props.$isPinned && `
    border-left: 4px solid ${props.$token.colorWarning};
  `}
  
  .ant-card-body {
    padding: 24px;
  }
  
  @media (max-width: 768px) {
    margin: 12px 0;
    .ant-card-body {
      padding: 16px;
    }
  }
  
  @media (max-width: 480px) {
    margin: 8px 0;
    .ant-card-body {
      padding: 12px;
    }
  }
  
  &:hover {
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.08);
    transform: translateY(-1px);
    transition: all 0.2s ease;
  }
`;

const TypeIcon = styled.div<{ $type?: string; $token: any }>`
  width: 32px;
  height: 32px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  background: ${props => {
    switch (props.$type) {
      case 'success': return props.$token.colorSuccessBg;
      case 'warning': return props.$token.colorWarningBg;
      case 'error': return props.$token.colorErrorBg;
      default: return props.$token.colorPrimaryBg;
    }
  }};
  color: ${props => {
    switch (props.$type) {
      case 'success': return props.$token.colorSuccess;
      case 'warning': return props.$token.colorWarning;
      case 'error': return props.$token.colorError;
      default: return props.$token.colorPrimary;
    }
  }};
  font-size: 16px;
`;

const AnnouncementMeta = styled.div`
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 16px;
  flex-wrap: wrap;
`;

const TimeInfo = styled.div<{ $token: any }>`
  display: flex;
  align-items: center;
  gap: 6px;
  color: ${props => props.$token.colorTextTertiary};
  font-size: 13px;
`;

const ContentWrapper = styled.div<{ $token: any }>`
  .markdown-content {
    font-size: 15px;
    line-height: 1.7;
    color: ${props => props.$token.colorText};
    
    h1, h2, h3, h4, h5, h6 {
      color: ${props => props.$token.colorTextHeading};
      margin: 20px 0 12px 0;
      font-weight: 600;
    }
    
    h1 { font-size: 24px; }
    h2 { font-size: 20px; }
    h3 { font-size: 18px; }
    h4 { font-size: 16px; }
    h5 { font-size: 14px; }
    h6 { font-size: 13px; }
    
    p {
      margin-bottom: 12px;
      color: ${props => props.$token.colorText};
    }
    
    strong {
      color: ${props => props.$token.colorTextHeading};
      font-weight: 600;
    }
    
    code {
      padding: 3px 6px;
      border-radius: 4px;
      font-family: 'SF Mono', Monaco, Consolas, monospace;
      font-size: 13px;
      color: ${props => props.$token.colorError};
    }
    
    pre {
      padding: 16px;
      border-radius: 8px;
      border-left: 3px solid ${props => props.$token.colorPrimary};
      margin: 16px 0;
      overflow-x: auto;
      
      code {
        background: none;
        padding: 0;
        color: ${props => props.$token.colorTextHeading};
      }
    }
    
    blockquote {
      margin: 16px 0;
      padding: 16px 20px;
      border-left: 4px solid ${props => props.$token.colorPrimary};
      border-radius: 0 8px 8px 0;
      font-style: italic;
    }
    
    ul, ol {
      margin: 12px 0;
      padding-left: 24px;
    }
    
    li {
      margin-bottom: 6px;
      line-height: 1.6;
    }
    
    table {
      width: 100%;
      border-collapse: collapse;
      margin: 16px 0;
      border-radius: 8px;
      overflow: hidden;
      border: 1px solid ${props => props.$token.colorBorderSecondary};
    }
    
    th, td {
      padding: 12px 16px;
      text-align: left;
      border-bottom: 1px solid ${props => props.$token.colorBorderSecondary};
    }
    
    th {
      background: ${props => props.$token.colorFillQuaternary};
      font-weight: 600;
      color: ${props => props.$token.colorTextHeading};
    }
  }
`;

const FloatingButton = styled(Button)<{ $token: any }>`
  position: fixed;
  bottom: 24px;
  right: 24px;
  width: 56px;
  height: 56px;
  border-radius: 28px;
  background: linear-gradient(135deg, ${props => props.$token.colorPrimary}, ${props => props.$token.colorPrimaryHover});
  border: 2px solid ${props => props.$token.colorPrimaryBorder};
  box-shadow: 
    0 4px 12px ${props => props.$token.colorPrimary}30,
    0 2px 4px ${props => props.$token.colorPrimary}20,
    inset 0 1px 0 rgba(255, 255, 255, 0.1);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  
  &:hover {
    background: linear-gradient(135deg, ${props => props.$token.colorPrimaryHover}, ${props => props.$token.colorPrimaryActive});
    transform: translateY(-2px) scale(1.05);
    box-shadow: 
      0 8px 25px ${props => props.$token.colorPrimary}40,
      0 4px 8px ${props => props.$token.colorPrimary}30,
      inset 0 1px 0 rgba(255, 255, 255, 0.15);
  }
  
  &:active {
    transform: translateY(0) scale(0.98);
    box-shadow: 
      0 2px 8px ${props => props.$token.colorPrimary}30,
      0 1px 3px ${props => props.$token.colorPrimary}20,
      inset 0 1px 0 rgba(255, 255, 255, 0.05);
  }
  
  .ant-badge {
    & .ant-badge-count {
      background: ${props => props.$token.colorError};
      color: ${props => props.$token.colorWhite};
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
      border: 2px solid ${props => props.$token.colorBgContainer};
      font-weight: 600;
      font-size: 11px;
      min-width: 18px;
      height: 18px;
      line-height: 14px;
    }
  }
  
  .anticon {
    color: ${props => props.$token.colorWhite};
    font-size: 20px;
    filter: drop-shadow(0 1px 2px rgba(0, 0, 0, 0.1));
  }
  
  @media (max-width: 768px) {
    bottom: 16px;
    right: 16px;
    width: 48px;
    height: 48px;
    border-radius: 24px;
    box-shadow: 
      0 3px 10px ${props => props.$token.colorPrimary}25,
      0 1px 3px ${props => props.$token.colorPrimary}15;
    
    .anticon {
      font-size: 18px;
    }
  }
  
  @media (max-width: 480px) {
    bottom: 12px;
    right: 12px;
    width: 44px;
    height: 44px;
    border-radius: 22px;
    box-shadow: 
      0 2px 8px ${props => props.$token.colorPrimary}20,
      0 1px 2px ${props => props.$token.colorPrimary}10;
    
    .anticon {
      font-size: 16px;
    }
    
    .ant-badge .ant-badge-count {
      min-width: 16px;
      height: 16px;
      font-size: 10px;
      line-height: 12px;
    }
  }
`;

const StatsCard = styled(Card)<{ $token: any }>`
  background: ${props => props.$token.colorFillQuaternary};
  border: 1px solid ${props => props.$token.colorBorderSecondary};
  border-radius: 8px;
  margin-bottom: 16px;
  
  .ant-card-body {
    padding: 16px 20px;
  }
  
  @media (max-width: 768px) {
    .ant-card-body {
      padding: 12px 16px;
    }
  }
`;

const ResponsiveContent = styled.div<{ $token: any }>`
  padding: 24px;
  max-height: calc(100vh - 200px);
  overflow-y: auto;
  
  @media (max-width: 768px) {
    padding: 16px;
    max-height: calc(100vh - 160px);
  }
  
  @media (max-width: 480px) {
    padding: 12px;
    max-height: calc(100vh - 140px);
  }
`;

interface Announcement {
  id: string;
  title: string;
  content: string;
  type: 'info' | 'success' | 'warning' | 'error';
  enabled: boolean;
  pinned: boolean;
  order: number;
  expireTime?: string;
  createdAt: string;
  updatedAt: string;
}

const EnhancedAnnouncement: React.FC = () => {
  const { token } = theme.useToken();
  const [visible, setVisible] = useState(false);
  const [announcements, setAnnouncements] = useState<Announcement[]>([]);
  const [loading, setLoading] = useState(false);
  const [visibleAnnouncements, setVisibleAnnouncements] = useState<Announcement[]>([]);
  const [readAnnouncements, setReadAnnouncements] = useState<string[]>([]);

  // Get dismissed announcements
  const getDismissedAnnouncements = (): string[] => {
    try {
      const dismissed = localStorage.getItem('dismissedAnnouncements');
      return dismissed ? JSON.parse(dismissed) : [];
    } catch {
      return [];
    }
  };

  // Add dismissed announcement
  const addDismissedAnnouncement = (id: string) => {
    try {
      const dismissed = getDismissedAnnouncements();
      if (!dismissed.includes(id)) {
        dismissed.push(id);
        localStorage.setItem('dismissedAnnouncements', JSON.stringify(dismissed));
      }
    } catch {
      // Silent error handling
    }
  };

  // Mark as read
  const markAsRead = (id: string) => {
    setReadAnnouncements(prev => [...prev, id]);
    addDismissedAnnouncement(id);
  };

  // Load announcements
  const loadAnnouncements = async () => {
    try {
      setLoading(true);
      const { data } = await getActiveAnnouncements();
      setAnnouncements(data);

      const dismissed = getDismissedAnnouncements();
      const activeAnnouncements = data.filter(announcement => !dismissed.includes(announcement.id));
      setVisibleAnnouncements(activeAnnouncements);
    } catch (error) {
      console.error('Failed to load announcements:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadAnnouncements();
  }, []);

  const getTypeIcon = (type: string) => {
    switch (type) {
      case 'success': return <CheckCircleOutlined />;
      case 'warning': return <ExclamationCircleOutlined />;
      case 'error': return <CloseCircleOutlined />;
      default: return <InfoCircleOutlined />;
    }
  };

  const getTypeText = (type: string) => {
    switch (type) {
      case 'success': return '成功';
      case 'warning': return '警告';
      case 'error': return '错误';
      default: return '信息';
    }
  };

  const getTypeColor = (type: string) => {
    switch (type) {
      case 'success': return 'success';
      case 'warning': return 'warning';
      case 'error': return 'error';
      default: return 'blue';
    }
  };

  const formatTime = (timeString: string) => {
    try {
      const date = new Date(timeString);
      const now = new Date();
      const diffInMinutes = Math.floor((now.getTime() - date.getTime()) / (1000 * 60));

      if (diffInMinutes < 1) return '刚刚';
      if (diffInMinutes < 60) return `${diffInMinutes}分钟前`;
      if (diffInMinutes < 1440) return `${Math.floor(diffInMinutes / 60)}小时前`;
      if (diffInMinutes < 43200) return `${Math.floor(diffInMinutes / 1440)}天前`;

      return date.toLocaleDateString('zh-CN');
    } catch {
      return '未知时间';
    }
  };

  const handleDismissAll = () => {
    visibleAnnouncements.forEach(announcement => {
      addDismissedAnnouncement(announcement.id);
    });
    setVisibleAnnouncements([]);
    setVisible(false);
  };

  const unreadCount = visibleAnnouncements.length;

  if (unreadCount === 0) {
    return null;
  }

  return (
    <>
      <FloatingButton
        $token={token}
        type="primary"
        icon={
          <Badge count={unreadCount} size="small" offset={[-4, 4]}>
            <SoundOutlined />
          </Badge>
        }
        onClick={() => setVisible(true)}
      />

      <Modal
        open={visible}
        onCancel={() => setVisible(false)}
        footer={null}
        width={800}
        centered
        style={{ 
          top: 20,
          maxWidth: 'calc(100vw - 16px)',
          margin: '8px',
          padding: 0
        }}
        bodyStyle={{ 
          padding: 0,
          maxHeight: 'calc(100vh - 100px)',
          overflow: 'hidden'
        }}
        destroyOnClose
        className="chatgpt-announcement-modal"
        styles={{
          body: {
            padding: 0,
            borderRadius: '12px',
            backgroundColor: 'transparent'
          },
          mask: {
            backgroundColor: 'rgba(0, 0, 0, 0.45)',
            backdropFilter: 'blur(4px)'
          },
          content: {
            boxShadow: '0 8px 32px rgba(0, 0, 0, 0.12)',
            borderRadius: '12px',
            overflow: 'hidden'
          }
        }}
      >
        <ChatGPTContainer $token={token}>
          <Header $token={token}>
            <HeaderTitle>
              <BellOutlined style={{ fontSize: 20, color: token.colorPrimary }} />
              <div>
                <Title level={3} style={{ margin: 0, color: token.colorTextHeading }}>
                  系统公告
                </Title>
                <Text type="secondary">
                  {unreadCount} 条未读公告
                </Text>
              </div>
            </HeaderTitle>
            <HeaderActions>
              <Button
                type="text"
                icon={<CloseOutlined />}
                onClick={() => setVisible(false)}
                size="large"
              />
            </HeaderActions>
          </Header>

          <ResponsiveContent $token={token}>
            {visibleAnnouncements.length > 0 && (
              <StatsCard $token={token}>
                <Space>
                  <EyeOutlined />
                  <Text>您有 {unreadCount} 条未读公告，请及时查看</Text>
                </Space>
              </StatsCard>
            )}

            <Timeline
              items={visibleAnnouncements.map((announcement, index) => ({
                dot: <TypeIcon $type={announcement.type} $token={token}>{getTypeIcon(announcement.type)}</TypeIcon>,
                color: getTypeColor(announcement.type),
                children: (
                  <AnnouncementCard
                    key={announcement.id}
                    $type={announcement.type}
                    $isPinned={announcement.pinned}
                    $token={token}
                    title={
                      <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                        <Title level={4} style={{ margin: 0, color: token.colorTextHeading }}>
                          {announcement.title}
                        </Title>
                        {announcement.pinned && (
                          <Tag icon={<PushpinOutlined />} color="gold">
                            置顶
                          </Tag>
                        )}
                      </div>
                    }
                    extra={
                      <Button
                        type="text"
                        icon={<CheckOutlined />}
                        onClick={() => markAsRead(announcement.id)}
                        size="small"
                      >
                        已读
                      </Button>
                    }
                  >
                    <AnnouncementMeta>
                      <Tag color={getTypeColor(announcement.type)}>
                        {getTypeText(announcement.type)}
                      </Tag>
                      <TimeInfo $token={token}>
                        <ClockCircleOutlined />
                        {formatTime(announcement.createdAt)}
                      </TimeInfo>
                      {announcement.expireTime && (
                        <TimeInfo $token={token}>
                          <CalendarOutlined />
                          过期: {new Date(announcement.expireTime).toLocaleDateString('zh-CN')}
                        </TimeInfo>
                      )}
                    </AnnouncementMeta>
                    
                    <ContentWrapper $token={token}>
                      <div className="markdown-content">
                        <ReactMarkdown>
                          {announcement.content}
                        </ReactMarkdown>
                      </div>
                    </ContentWrapper>
                  </AnnouncementCard>
                )
              }))}
            />

            {visibleAnnouncements.length > 1 && (
              <div style={{ textAlign: 'center', marginTop: 24 }}>
                <Button
                  type="primary"
                  icon={<CheckOutlined />}
                  onClick={handleDismissAll}
                  size="large"
                >
                  全部标记为已读
                </Button>
              </div>
            )}
          </ResponsiveContent>
        </ChatGPTContainer>
      </Modal>
    </>
  );
};

export default EnhancedAnnouncement;
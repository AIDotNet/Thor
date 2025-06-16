import React, { useState, useEffect } from 'react';
import { Modal, Button, Tag, Space, Typography, Avatar, Divider, theme } from 'antd';
import {
  CloseOutlined,
  InfoCircleOutlined,
  CheckCircleOutlined,
  ExclamationCircleOutlined,
  CloseCircleOutlined,
  CalendarOutlined,
  StarFilled,
  BellOutlined
} from '@ant-design/icons';
import { getActiveAnnouncements } from '../../services/AnnouncementService';
import ReactMarkdown from 'react-markdown';
import styled from 'styled-components';
import { t } from '../../utils/i18n';

const { Title, Text, Paragraph } = Typography;

// 主容器 - 自适应主题
const StyledModal = styled(Modal)<{ $token: any }>`
  .ant-modal-content {
    background: ${props => props.$token.colorBgContainer};
    border: 1px solid ${props => props.$token.colorBorderSecondary};
    border-radius: ${props => props.$token.borderRadius}px;
    box-shadow: ${props => props.$token.boxShadowSecondary};
    padding: 0;
    overflow: hidden;
    max-width: 600px;
  }
  
  .ant-modal-header {
    background: ${props => props.$token.colorFillQuaternary};
    padding: 20px 24px;
    border: none;
    border-radius: ${props => props.$token.borderRadius}px ${props => props.$token.borderRadius}px 0 0;
    border-bottom: 1px solid ${props => props.$token.colorBorderSecondary};
  }
  
  .ant-modal-title {
    color: ${props => props.$token.colorTextHeading};
    font-size: 16px;
    font-weight: 600;
    display: flex;
    align-items: center;
    gap: 8px;
    margin: 0;
  }
  
  .ant-modal-close {
    position: absolute;
    top: 16px;
    right: 20px;
    width: 28px;
    height: 28px;
    border-radius: ${props => props.$token.borderRadiusSM}px;
    background: ${props => props.$token.colorFillSecondary};
    border: 1px solid ${props => props.$token.colorBorderSecondary};
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all 0.2s;
    
    &:hover {
      background: ${props => props.$token.colorFillTertiary};
      border-color: ${props => props.$token.colorBorder};
    }
    
    .ant-modal-close-x {
      color: ${props => props.$token.colorTextSecondary};
      font-size: 12px;
      line-height: 1;
    }
  }
  
  .ant-modal-body {
    padding: 0;
    max-height: 60vh;
    overflow-y: auto;
    
    &::-webkit-scrollbar {
      width: 4px;
    }
    
    &::-webkit-scrollbar-track {
      background: ${props => props.$token.colorFillQuaternary};
    }
    
    &::-webkit-scrollbar-thumb {
      background: ${props => props.$token.colorFillTertiary};
      border-radius: 2px;
    }
  }
  
  .ant-modal-footer {
    background: ${props => props.$token.colorFillQuaternary};
    padding: 16px 24px;
    border-top: 1px solid ${props => props.$token.colorBorderSecondary};
    border-radius: 0 0 ${props => props.$token.borderRadius}px ${props => props.$token.borderRadius}px;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  @media (max-width: 768px) {
    .ant-modal-content {
      margin: 16px;
      max-width: calc(100vw - 32px);
    }
  }
`;

// 公告项容器 - 自适应主题
const AnnouncementItem = styled.div<{ $isPinned?: boolean; $type?: string; $token: any }>`
  padding: 24px;
  position: relative;
  background: ${props =>
    props.$isPinned
      ? props.$token.colorWarningBg
      : props.$token.colorBgContainer
  };
  
  &:not(:last-child) {
    border-bottom: 1px solid ${props => props.$token.colorBorderSecondary};
  }
  
  &::before {
    content: '';
    position: absolute;
    left: 0;
    top: 0;
    bottom: 0;
    width: 3px;
    background: ${props => {
    switch (props.$type) {
      case 'success': return props.$token.colorSuccess;
      case 'warning': return props.$token.colorWarning;
      case 'error': return props.$token.colorError;
      default: return props.$token.colorPrimary;
    }
  }};
  }
`;

// 公告头部
const AnnouncementHeader = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 12px;
`;

// 公告元信息
const AnnouncementMeta = styled.div`
  display: flex;
  align-items: center;
  gap: 8px;
  flex-wrap: wrap;
`;

// 公告标题 - 自适应主题
const AnnouncementTitle = styled(Title)<{ $token: any }>`
  margin: 0 0 12px 0 !important;
  font-size: 16px !important;
  font-weight: 600 !important;
  line-height: 1.4 !important;
  color: ${props => props.$token.colorTextHeading} !important;
  display: flex;
  align-items: center;
  gap: 8px;
`;

// 类型图标容器 - 自适应主题
const TypeIconWrapper = styled.div<{ $type?: string; $token: any }>`
  width: 20px;
  height: 20px;
  border-radius: ${props => props.$token.borderRadiusSM}px;
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
  font-size: 12px;
`;

// Markdown内容容器 - 自适应主题
const MarkdownContent = styled.div<{ $token: any }>`
  .markdown-content {
    font-size: 14px;
    line-height: 1.6;
    color: ${props => props.$token.colorText};
    
    h1, h2, h3, h4, h5, h6 {
      color: ${props => props.$token.colorTextHeading};
      margin-top: 16px;
      margin-bottom: 8px;
      font-weight: 600;
    }
    
    h1 { font-size: 20px; }
    h2 { font-size: 18px; }
    h3 { font-size: 16px; }
    h4 { font-size: 14px; }
    h5, h6 { font-size: 12px; }
    
    p {
      margin-bottom: 12px;
      color: ${props => props.$token.colorText};
    }
    
    strong {
      color: ${props => props.$token.colorTextHeading};
      font-weight: 600;
    }
    
    em {
      font-style: italic;
      color: ${props => props.$token.colorTextSecondary};
    }
    
    code {
      background: ${props => props.$token.colorFillSecondary};
      padding: 2px 4px;
      border-radius: ${props => props.$token.borderRadiusXS}px;
      font-family: 'SF Mono', Monaco, Consolas, 'Liberation Mono', 'Courier New', monospace;
      font-size: 12px;
      color: ${props => props.$token.colorError};
    }
    
    pre {
      background: ${props => props.$token.colorFillQuaternary};
      padding: 12px;
      border-radius: ${props => props.$token.borderRadiusSM}px;
      border-left: 3px solid ${props => props.$token.colorPrimary};
      margin: 12px 0;
      overflow-x: auto;
      
      code {
        background: none;
        padding: 0;
        color: ${props => props.$token.colorTextHeading};
      }
    }
    
    blockquote {
      margin: 12px 0;
      padding: 12px 16px;
      background: ${props => props.$token.colorPrimaryBg};
      border-left: 3px solid ${props => props.$token.colorPrimary};
      border-radius: 0 ${props => props.$token.borderRadiusSM}px ${props => props.$token.borderRadiusSM}px 0;
      
      p {
        margin: 0;
        color: ${props => props.$token.colorText};
        font-style: italic;
      }
    }
    
    ul, ol {
      margin: 12px 0;
      padding-left: 20px;
      
      li {
        margin-bottom: 4px;
        color: ${props => props.$token.colorText};
        line-height: 1.5;
      }
    }
    
    a {
      color: ${props => props.$token.colorLink};
      text-decoration: none;
      
      &:hover {
        color: ${props => props.$token.colorLinkHover};
        text-decoration: underline;
      }
    }
    
    img {
      max-width: 100%;
      height: auto;
      border-radius: ${props => props.$token.borderRadiusSM}px;
      margin: 12px 0;
    }
    
    table {
      width: 100%;
      border-collapse: collapse;
      margin: 12px 0;
      border-radius: ${props => props.$token.borderRadiusSM}px;
      overflow: hidden;
      border: 1px solid ${props => props.$token.colorBorderSecondary};
      
      th, td {
        padding: 8px 12px;
        text-align: left;
        border-bottom: 1px solid ${props => props.$token.colorBorderSecondary};
      }
      
      th {
        background: ${props => props.$token.colorFillQuaternary};
        font-weight: 600;
        color: ${props => props.$token.colorTextHeading};
      }
    }
    
    hr {
      border: none;
      height: 1px;
      background: ${props => props.$token.colorBorderSecondary};
      margin: 16px 0;
    }
  }
`;

// 时间和关闭按钮容器 - 自适应主题
const AnnouncementActions = styled.div<{ $token: any }>`
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-top: 16px;
  padding-top: 12px;
  border-top: 1px solid ${props => props.$token.colorBorderSecondary};
`;

// 时间显示 - 自适应主题
const TimeDisplay = styled.div<{ $token: any }>`
  display: flex;
  align-items: center;
  gap: 4px;
  color: ${props => props.$token.colorTextTertiary};
  font-size: 12px;
`;

// 关闭按钮 - 自适应主题
const DismissButton = styled(Button)<{ $token: any }>`
  border: 1px solid ${props => props.$token.colorErrorBorder};
  background: ${props => props.$token.colorErrorBg};
  color: ${props => props.$token.colorError};
  border-radius: ${props => props.$token.borderRadiusSM}px;
  padding: 4px 8px;
  height: auto;
  font-size: 12px;
  display: flex;
  align-items: center;
  gap: 4px;
  transition: all 0.2s;
  
  &:hover {
    background: ${props => props.$token.colorErrorBgHover} !important;
    border-color: ${props => props.$token.colorErrorBorderHover} !important;
    color: ${props => props.$token.colorError} !important;
  }
`;

// 底部操作按钮 - 自适应主题
const ActionButton = styled(Button)<{ $token: any }>`
  border-radius: ${props => props.$token.borderRadiusSM}px;
  height: 32px;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 6px;
  transition: all 0.2s;
  
  &.primary {
    background: ${props => props.$token.colorPrimary};
    border: none;
    
    &:hover {
      background: ${props => props.$token.colorPrimaryHover};
    }
  }
  
  &.secondary {
    background: ${props => props.$token.colorFillSecondary};
    border: 1px solid ${props => props.$token.colorBorder};
    color: ${props => props.$token.colorTextSecondary};
    
    &:hover {
      background: ${props => props.$token.colorFillTertiary};
      border-color: ${props => props.$token.colorBorderHover};
      color: ${props => props.$token.colorText};
    }
  }
`;

// 通知数量徽章 - 自适应主题
const NotificationBadge = styled.div<{ $token: any }>`
  background: ${props => props.$token.colorError};
  color: ${props => props.$token.colorWhite};
  border-radius: 10px;
  padding: 2px 6px;
  font-size: 11px;
  font-weight: 600;
  min-width: 18px;
  height: 18px;
  display: flex;
  align-items: center;
  justify-content: center;
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

const AnnouncementBanner: React.FC = () => {
  const { token } = theme.useToken();
  const [visible, setVisible] = useState(false);
  const [announcements, setAnnouncements] = useState<Announcement[]>([]);
  const [loading, setLoading] = useState(false);
  const [visibleAnnouncements, setVisibleAnnouncements] = useState<Announcement[]>([]);

  // 获取已关闭的公告ID列表
  const getDismissedAnnouncements = (): string[] => {
    try {
      const dismissed = localStorage.getItem('dismissedAnnouncements');
      return dismissed ? JSON.parse(dismissed) : [];
    } catch {
      return [];
    }
  };

  // 添加已关闭的公告ID
  const addDismissedAnnouncement = (id: string) => {
    try {
      const dismissed = getDismissedAnnouncements();
      if (!dismissed.includes(id)) {
        dismissed.push(id);
        localStorage.setItem('dismissedAnnouncements', JSON.stringify(dismissed));
      }
    } catch {
      // 静默处理错误
    }
  };

  // 加载公告数据
  const loadAnnouncements = async () => {
    try {
      setLoading(true);
      const { data } = await getActiveAnnouncements();
      setAnnouncements(data);

      // 过滤掉已关闭的公告
      const dismissed = getDismissedAnnouncements();
      const activeAnnouncements = data.filter(announcement => !dismissed.includes(announcement.id));
      setVisibleAnnouncements(activeAnnouncements);

      // 如果有未关闭的公告，显示弹窗
      if (activeAnnouncements.length > 0) {
        setVisible(true);
      }
    } catch (error) {
      console.error('Failed to load announcements:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadAnnouncements();
  }, []);

  // 获取类型对应的图标
  const getTypeIcon = (type: string) => {
    switch (type) {
      case 'success':
        return <CheckCircleOutlined />;
      case 'warning':
        return <ExclamationCircleOutlined />;
      case 'error':
        return <CloseCircleOutlined />;
      default:
        return <InfoCircleOutlined />;
    }
  };

  // 获取类型对应的标签文本
  const getTypeText = (type: string) => {
    switch (type) {
      case 'success':
        return '成功';
      case 'warning':
        return '警告';
      case 'error':
        return '错误';
      default:
        return '信息';
    }
  };

  // 获取类型对应的颜色
  const getTypeColor = (type: string) => {
    switch (type) {
      case 'success':
        return 'success';
      case 'warning':
        return 'warning';
      case 'error':
        return 'error';
      default:
        return 'blue';
    }
  };

  // 关闭单个公告
  const handleDismissAnnouncement = (id: string) => {
    addDismissedAnnouncement(id);
    const newVisibleAnnouncements = visibleAnnouncements.filter(announcement => announcement.id !== id);
    setVisibleAnnouncements(newVisibleAnnouncements);

    // 如果没有更多公告，关闭弹窗
    if (newVisibleAnnouncements.length === 0) {
      setVisible(false);
    }
  };

  // 全部标记为已读
  const handleMarkAllAsRead = () => {
    visibleAnnouncements.forEach(announcement => {
      addDismissedAnnouncement(announcement.id);
    });
    setVisibleAnnouncements([]);
    setVisible(false);
  };

  // 格式化时间
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

  // 如果没有可显示的公告，不渲染组件
  if (visibleAnnouncements.length === 0) {
    return null;
  }

  return (
    <StyledModal
      $token={token}
      title={
        <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
          <BellOutlined style={{ fontSize: 16 }} />
          <span>系统公告</span>
          <NotificationBadge $token={token}>
            {visibleAnnouncements.length}
          </NotificationBadge>
        </div>
      }
      open={visible}
      onCancel={() => setVisible(false)}
      footer={
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Text type="secondary" style={{ fontSize: 12 }}>
            共 {visibleAnnouncements.length} 条公告
          </Text>
          <Space size={8}>
            <ActionButton
              $token={token}
              className="secondary"
              onClick={() => setVisible(false)}
            >
              稍后查看
            </ActionButton>
            <ActionButton
              $token={token}
              className="primary"
              type="primary"
              onClick={handleMarkAllAsRead}
            >
              <CheckCircleOutlined />
              全部已读
            </ActionButton>
          </Space>
        </div>
      }
      width={600}
      centered
      maskClosable={false}
      destroyOnClose
    >
      {visibleAnnouncements.map((announcement, index) => (
        <AnnouncementItem
          key={announcement.id}
          $isPinned={announcement.pinned}
          $type={announcement.type}
          $token={token}
        >
          <AnnouncementHeader>
            <AnnouncementMeta>
              <Tag color={getTypeColor(announcement.type)} style={{ fontSize: '11px' }}>
                {getTypeText(announcement.type)}
              </Tag>
              {announcement.pinned && (
                <Tag color="gold" icon={<StarFilled />} style={{ fontSize: '11px' }}>
                  置顶
                </Tag>
              )}
            </AnnouncementMeta>
          </AnnouncementHeader>

          <AnnouncementTitle level={4} $token={token}>
            <TypeIconWrapper $type={announcement.type} $token={token}>
              {getTypeIcon(announcement.type)}
            </TypeIconWrapper>
            {announcement.title}
          </AnnouncementTitle>

          <MarkdownContent $token={token}>
            <div className="markdown-content">
              <ReactMarkdown>
                {announcement.content}
              </ReactMarkdown>
            </div>
          </MarkdownContent>

          <AnnouncementActions $token={token}>
            <TimeDisplay $token={token}>
              <CalendarOutlined />
              {formatTime(announcement.createdAt)}
            </TimeDisplay>
            <DismissButton
              $token={token}
              size="small"
              onClick={() => handleDismissAnnouncement(announcement.id)}
              icon={<CloseOutlined />}
            >
              关闭
            </DismissButton>
          </AnnouncementActions>
        </AnnouncementItem>
      ))}
    </StyledModal>
  );
};

export default AnnouncementBanner; 
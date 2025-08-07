import React, { useState, useEffect } from "react";
import {
  Card,
  Statistic,
  Row,
  Col,
  Button,
  Space,
  message,
  Modal,
  DatePicker,
  Input,
  Form,
  Divider,
  Typography,
  Alert,
  theme,
  Spin,
  Tooltip,
  Badge,
} from "antd";
import {
  DeleteOutlined,
  ReloadOutlined,
  WarningOutlined,
  CalendarOutlined,
  DatabaseOutlined,
  CleaningServicesOutlined,
  InfoCircleOutlined,
} from "@ant-design/icons";
import {
  getTracingStatistics,
  cleanupTracings,
  deleteTracingsByDate,
  deleteTracingsByChatLoggerId,
  deleteTracingsByTraceId,
  TracingStatistics,
} from "../../services/TracingService";
import dayjs from "dayjs";

const { Title, Text } = Typography;
const { useToken } = theme;

export default function TracingPage() {
  const { token } = useToken();
  const [loading, setLoading] = useState(false);
  const [statistics, setStatistics] = useState<TracingStatistics>({
    totalCount: 0,
    todayCount: 0,
    yesterdayCount: 0,
    weekCount: 0,
  });

  // 弹窗状态
  const [cleanupModalVisible, setCleanupModalVisible] = useState(false);
  const [dateDeleteModalVisible, setDateDeleteModalVisible] = useState(false);
  const [idDeleteModalVisible, setIdDeleteModalVisible] = useState(false);

  // 表单数据
  const [cleanupDays, setCleanupDays] = useState(30);
  const [deleteDate, setDeleteDate] = useState<dayjs.Dayjs | null>(null);
  const [deleteType, setDeleteType] = useState<'chatLoggerId' | 'traceId'>('chatLoggerId');
  const [deleteId, setDeleteId] = useState('');

  // 操作加载状态
  const [cleanupLoading, setCleanupLoading] = useState(false);
  const [dateDeleteLoading, setDateDeleteLoading] = useState(false);
  const [idDeleteLoading, setIdDeleteLoading] = useState(false);

  const styles = {
    pageContainer: {
      padding: 24,
      maxWidth: 1200,
      margin: '0 auto',
      background: token.colorBgContainer,
      minHeight: 'calc(100vh - 48px)',
    },
    pageHeader: {
      marginBottom: 24,
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center',
      flexWrap: 'wrap' as const,
      gap: 16,
    },
    pageTitle: {
      margin: 0,
      fontSize: 24,
      fontWeight: 600,
      color: token.colorTextHeading,
    },
    statsRow: {
      marginBottom: 32,
    },
    statsCard: {
      height: '100%',
      borderRadius: 12,
      border: `1px solid ${token.colorBorderSecondary}`,
      boxShadow: '0 2px 8px rgba(0,0,0,0.06)',
      transition: 'all 0.3s ease',
      '&:hover': {
        transform: 'translateY(-2px)',
        boxShadow: '0 4px 16px rgba(0,0,0,0.12)',
      },
    },
    operationsSection: {
      marginBottom: 24,
    },
    operationCard: {
      marginBottom: 16,
      borderRadius: 8,
      border: `1px solid ${token.colorBorderSecondary}`,
      background: token.colorBgContainer,
      transition: 'all 0.2s ease',
    },
    operationHeader: {
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center',
      marginBottom: 12,
    },
    operationTitle: {
      fontSize: 16,
      fontWeight: 500,
      color: token.colorTextHeading,
      display: 'flex',
      alignItems: 'center',
      gap: 8,
    },
    operationDescription: {
      color: token.colorTextSecondary,
      marginBottom: 16,
      lineHeight: 1.6,
    },
    dangerOperation: {
      border: `1px solid ${token.colorErrorBorder}`,
      background: `${token.colorErrorBg}`,
    },
    warningOperation: {
      border: `1px solid ${token.colorWarningBorder}`,
      background: `${token.colorWarningBg}`,
    },
    infoAlert: {
      marginBottom: 24,
      borderRadius: 8,
    },
    modalContent: {
      padding: '8px 0',
    },
    formItem: {
      marginBottom: 16,
    },
    statisticIcon: {
      fontSize: 24,
      color: token.colorPrimary,
    },
  };

  // 加载统计数据
  const loadStatistics = async () => {
    setLoading(true);
    try {
      const response = await getTracingStatistics();
      if (response.success) {
        setStatistics(response.data);
      } else {
        message.error(response.message || '加载统计数据失败');
      }
    } catch (error) {
      message.error('加载统计数据时发生错误');
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  // 清理过期记录
  const handleCleanup = async () => {
    if (cleanupDays <= 0) {
      message.error('保留天数必须大于0');
      return;
    }

    setCleanupLoading(true);
    try {
      const response = await cleanupTracings(cleanupDays);
      if (response.success) {
        message.success(`清理完成！${response.data.message}`);
        setCleanupModalVisible(false);
        loadStatistics(); // 重新加载统计数据
      } else {
        message.error(response.message || '清理失败');
      }
    } catch (error) {
      message.error('清理操作发生错误');
      console.error(error);
    } finally {
      setCleanupLoading(false);
    }
  };

  // 按日期删除
  const handleDateDelete = async () => {
    if (!deleteDate) {
      message.error('请选择删除日期');
      return;
    }

    setDateDeleteLoading(true);
    try {
      const dateStr = deleteDate.format('YYYY-MM-DDTHH:mm:ss') + 'Z';
      const response = await deleteTracingsByDate(dateStr);
      if (response.success) {
        message.success(`删除完成！${response.data.message}`);
        setDateDeleteModalVisible(false);
        setDeleteDate(null);
        loadStatistics(); // 重新加载统计数据
      } else {
        message.error(response.message || '删除失败');
      }
    } catch (error) {
      message.error('删除操作发生错误');
      console.error(error);
    } finally {
      setDateDeleteLoading(false);
    }
  };

  // 按ID删除
  const handleIdDelete = async () => {
    if (!deleteId.trim()) {
      message.error(`请输入${deleteType === 'chatLoggerId' ? 'ChatLoggerId' : 'TraceId'}`);
      return;
    }

    setIdDeleteLoading(true);
    try {
      const response = deleteType === 'chatLoggerId' 
        ? await deleteTracingsByChatLoggerId(deleteId.trim())
        : await deleteTracingsByTraceId(deleteId.trim());
        
      if (response.success) {
        message.success(`删除完成！${response.data.message}`);
        setIdDeleteModalVisible(false);
        setDeleteId('');
        loadStatistics(); // 重新加载统计数据
      } else {
        message.error(response.message || '删除失败');
      }
    } catch (error) {
      message.error('删除操作发生错误');
      console.error(error);
    } finally {
      setIdDeleteLoading(false);
    }
  };

  useEffect(() => {
    loadStatistics();
  }, []);

  return (
    <div style={styles.pageContainer}>
      {/* 页面标题 */}
      <div style={styles.pageHeader}>
        <Title level={3} style={styles.pageTitle}>
          <DatabaseOutlined style={{ marginRight: 12, color: token.colorPrimary }} />
          Tracing 链路跟踪管理
        </Title>
        <Button
          icon={<ReloadOutlined />}
          onClick={loadStatistics}
          loading={loading}
        >
          刷新数据
        </Button>
      </div>

      {/* 系统提示 */}
      <Alert
        message="重要提示"
        description="删除操作不可恢复，请谨慎操作。建议在执行大批量删除前先备份重要数据。"
        type="info"
        icon={<InfoCircleOutlined />}
        style={styles.infoAlert}
        showIcon
      />

      {/* 统计卡片 */}
      <Row gutter={[16, 16]} style={styles.statsRow}>
        <Col xs={24} sm={12} lg={6}>
          <Card style={styles.statsCard}>
            <Statistic
              title="总记录数"
              value={statistics.totalCount}
              prefix={<DatabaseOutlined style={styles.statisticIcon} />}
              loading={loading}
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card style={styles.statsCard}>
            <Statistic
              title="今日记录"
              value={statistics.todayCount}
              prefix={<CalendarOutlined style={styles.statisticIcon} />}
              loading={loading}
              valueStyle={{ color: token.colorSuccess }}
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card style={styles.statsCard}>
            <Statistic
              title="昨日记录"
              value={statistics.yesterdayCount}
              prefix={<CalendarOutlined style={styles.statisticIcon} />}
              loading={loading}
              valueStyle={{ color: token.colorWarning }}
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card style={styles.statsCard}>
            <Statistic
              title="本周记录"
              value={statistics.weekCount}
              prefix={<CalendarOutlined style={styles.statisticIcon} />}
              loading={loading}
              valueStyle={{ color: token.colorInfo }}
            />
          </Card>
        </Col>
      </Row>

      {/* 记录时间范围 */}
      {(statistics.oldestRecord || statistics.newestRecord) && (
        <Row gutter={16} style={{ marginBottom: 24 }}>
          <Col span={24}>
            <Card size="small">
              <Text type="secondary">
                记录时间范围：
                {statistics.oldestRecord && (
                  <span style={{ marginLeft: 8 }}>
                    最早 {dayjs(statistics.oldestRecord).format('YYYY-MM-DD HH:mm:ss')}
                  </span>
                )}
                {statistics.oldestRecord && statistics.newestRecord && <span style={{ margin: '0 8px' }}>至</span>}
                {statistics.newestRecord && (
                  <span>
                    最新 {dayjs(statistics.newestRecord).format('YYYY-MM-DD HH:mm:ss')}
                  </span>
                )}
              </Text>
            </Card>
          </Col>
        </Row>
      )}

      {/* 操作区域 */}
      <div style={styles.operationsSection}>
        <Title level={4}>删除操作</Title>
        
        {/* 清理过期记录 */}
        <Card style={{ ...styles.operationCard, ...styles.warningOperation }}>
          <div style={styles.operationHeader}>
            <div style={styles.operationTitle}>
              <CleaningServicesOutlined />
              清理过期记录
            </div>
            <Badge count={statistics.totalCount > 10000 ? '推荐' : 0} />
          </div>
          <div style={styles.operationDescription}>
            删除指定天数之前的所有Tracing记录，这是推荐的清理方式，可以有效控制数据库存储空间。
          </div>
          <Button
            type="primary"
            icon={<CleaningServicesOutlined />}
            onClick={() => setCleanupModalVisible(true)}
            disabled={statistics.totalCount === 0}
          >
            批量清理
          </Button>
        </Card>

        {/* 按日期删除 */}
        <Card style={{ ...styles.operationCard, ...styles.dangerOperation }}>
          <div style={styles.operationHeader}>
            <div style={styles.operationTitle}>
              <CalendarOutlined />
              按日期删除
            </div>
          </div>
          <div style={styles.operationDescription}>
            删除指定日期之前的所有Tracing记录。请谨慎选择日期，此操作不可恢复。
          </div>
          <Button
            danger
            icon={<DeleteOutlined />}
            onClick={() => setDateDeleteModalVisible(true)}
            disabled={statistics.totalCount === 0}
          >
            按日期删除
          </Button>
        </Card>

        {/* 按ID删除 */}
        <Card style={styles.operationCard}>
          <div style={styles.operationHeader}>
            <div style={styles.operationTitle}>
              <DatabaseOutlined />
              按ID删除
            </div>
          </div>
          <div style={styles.operationDescription}>
            删除指定ChatLoggerId或TraceId相关的所有Tracing记录，适用于删除特定会话或链路的跟踪数据。
          </div>
          <Button
            icon={<DeleteOutlined />}
            onClick={() => setIdDeleteModalVisible(true)}
          >
            按ID删除
          </Button>
        </Card>
      </div>

      {/* 清理过期记录弹窗 */}
      <Modal
        title={
          <Space>
            <CleaningServicesOutlined />
            清理过期记录
          </Space>
        }
        open={cleanupModalVisible}
        onCancel={() => setCleanupModalVisible(false)}
        footer={[
          <Button key="cancel" onClick={() => setCleanupModalVisible(false)}>
            取消
          </Button>,
          <Button
            key="confirm"
            type="primary"
            loading={cleanupLoading}
            onClick={handleCleanup}
            icon={<CleaningServicesOutlined />}
          >
            确认清理
          </Button>,
        ]}
      >
        <div style={styles.modalContent}>
          <Form layout="vertical">
            <Form.Item 
              label="保留天数" 
              style={styles.formItem}
              extra="将删除超过指定天数的所有记录"
            >
              <Input
                type="number"
                value={cleanupDays}
                onChange={(e) => setCleanupDays(Number(e.target.value))}
                min={1}
                max={365}
                suffix="天"
                placeholder="输入保留天数"
              />
            </Form.Item>
          </Form>
          <Alert
            message={`将删除 ${cleanupDays} 天前的所有记录`}
            type="warning"
            showIcon
            icon={<WarningOutlined />}
          />
        </div>
      </Modal>

      {/* 按日期删除弹窗 */}
      <Modal
        title={
          <Space>
            <CalendarOutlined />
            按日期删除记录
          </Space>
        }
        open={dateDeleteModalVisible}
        onCancel={() => {
          setDateDeleteModalVisible(false);
          setDeleteDate(null);
        }}
        footer={[
          <Button key="cancel" onClick={() => {
            setDateDeleteModalVisible(false);
            setDeleteDate(null);
          }}>
            取消
          </Button>,
          <Button
            key="confirm"
            type="primary"
            danger
            loading={dateDeleteLoading}
            onClick={handleDateDelete}
            icon={<DeleteOutlined />}
          >
            确认删除
          </Button>,
        ]}
      >
        <div style={styles.modalContent}>
          <Form layout="vertical">
            <Form.Item 
              label="删除截止日期" 
              style={styles.formItem}
              extra="将删除此日期之前的所有记录"
            >
              <DatePicker
                value={deleteDate}
                onChange={setDeleteDate}
                showTime
                format="YYYY-MM-DD HH:mm:ss"
                placeholder="选择删除截止日期"
                style={{ width: '100%' }}
              />
            </Form.Item>
          </Form>
          {deleteDate && (
            <Alert
              message={`将删除 ${deleteDate.format('YYYY-MM-DD HH:mm:ss')} 之前的所有记录`}
              type="error"
              showIcon
              icon={<WarningOutlined />}
            />
          )}
        </div>
      </Modal>

      {/* 按ID删除弹窗 */}
      <Modal
        title={
          <Space>
            <DatabaseOutlined />
            按ID删除记录
          </Space>
        }
        open={idDeleteModalVisible}
        onCancel={() => {
          setIdDeleteModalVisible(false);
          setDeleteId('');
        }}
        footer={[
          <Button key="cancel" onClick={() => {
            setIdDeleteModalVisible(false);
            setDeleteId('');
          }}>
            取消
          </Button>,
          <Button
            key="confirm"
            type="primary"
            loading={idDeleteLoading}
            onClick={handleIdDelete}
            icon={<DeleteOutlined />}
          >
            确认删除
          </Button>,
        ]}
      >
        <div style={styles.modalContent}>
          <Form layout="vertical">
            <Form.Item label="删除类型" style={styles.formItem}>
              <Button.Group>
                <Button
                  type={deleteType === 'chatLoggerId' ? 'primary' : 'default'}
                  onClick={() => setDeleteType('chatLoggerId')}
                >
                  ChatLoggerId
                </Button>
                <Button
                  type={deleteType === 'traceId' ? 'primary' : 'default'}
                  onClick={() => setDeleteType('traceId')}
                >
                  TraceId
                </Button>
              </Button.Group>
            </Form.Item>
            <Form.Item 
              label={deleteType === 'chatLoggerId' ? 'ChatLoggerId' : 'TraceId'}
              style={styles.formItem}
              extra={`输入要删除的${deleteType === 'chatLoggerId' ? '聊天日志ID' : '链路跟踪ID'}`}
            >
              <Input
                value={deleteId}
                onChange={(e) => setDeleteId(e.target.value)}
                placeholder={`请输入${deleteType === 'chatLoggerId' ? 'ChatLoggerId' : 'TraceId'}`}
              />
            </Form.Item>
          </Form>
          {deleteId && (
            <Alert
              message={`将删除${deleteType === 'chatLoggerId' ? 'ChatLoggerId' : 'TraceId'}为 "${deleteId}" 的所有相关记录`}
              type="warning"
              showIcon
              icon={<WarningOutlined />}
            />
          )}
        </div>
      </Modal>
    </div>
  );
}
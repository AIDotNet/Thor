import { useEffect, useState, useMemo, useRef } from "react";
import {
  message,
  Button,
  Input,
  Select,
  DatePicker,
  Table,
  Card,
  Skeleton,
  Space,
  Divider,
  Typography,
  Row,
  Col,
  Collapse,
  theme,
  ConfigProvider,
  Timeline,
  Empty,
  Checkbox,
  Tooltip as AntTooltip,
  Badge,
  Modal
} from "antd";
import {
  SearchOutlined,
  FilterOutlined,
  ReloadOutlined,
  DownloadOutlined,
  BarChartOutlined,
  UserOutlined,
  CheckCircleOutlined,
  CaretRightOutlined,
  CalendarOutlined,
  SettingOutlined,
  MenuOutlined,
} from "@ant-design/icons";
import { getLoggers, viewConsumption, Export } from "../../services/LoggerService";
import { GetServerLoad } from "../../services/TrackerService";
import { getSimpleList } from "../../services/UserService";
import { Tag, Tooltip } from "@lobehub/ui";
import { renderQuota } from "../../utils/render";
import dayjs from "dayjs";
import { DndContext, PointerSensor, useSensor, useSensors } from '@dnd-kit/core';
import type { DragEndEvent } from '@dnd-kit/core';
import { SortableContext, arrayMove, useSortable, verticalListSortingStrategy } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';

const { Title, Text } = Typography;
const { Panel } = Collapse;
const { useToken } = theme;

interface ColumnConfigItem {
  key: string;
  title: string;
  dataIndex: string;
  width?: number;
  fixed?: boolean | 'left' | 'right';
  disable?: boolean;
  render?: (...args: any[]) => JSX.Element | null;
  sorter?: (a: any, b: any) => number;
  filters?: { text: string; value: string }[];
  onFilter?: (value: any, record: any) => boolean;
  ellipsis?: boolean | { showTitle?: boolean };
}

// 拖拽表头
const SortableItem = (props: {
  id: string;
  style?: React.CSSProperties;
  children: React.ReactNode;
}) => {
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({
    id: props.id,
  });
  
  const style: React.CSSProperties = {
    ...props.style,
    transform: CSS.Transform.toString(transform),
    transition,
    cursor: 'move',
    ...(isDragging ? { position: 'relative', zIndex: 9999 } : {}),
  };

  return (
    <div ref={setNodeRef} style={style} {...attributes} {...listeners}>
      {props.children}
    </div>
  );
};

export default function LoggerPage() {
  const { token } = useToken();
  const [data, setData] = useState<any[]>([]);
  const [total, setTotal] = useState<number>(0);
  const [loading, setLoading] = useState<boolean>(false);
  const [consume, setConsume] = useState<number>(0);
  const [consumeLoading, setConsumeLoading] = useState<boolean>(false);
  const [exportLoading, setExportLoading] = useState<boolean>(false);
  const [filterVisible, setFilterVisible] = useState<boolean>(true);
  const [expandedRowKeys, setExpandedRowKeys] = useState<string[]>([]);
  const [traceDataMap, setTraceDataMap] = useState<Record<string, any>>({});
  const [loadingTraceMap, setLoadingTraceMap] = useState<Record<string, boolean>>({});
  const [columnSettingsVisible, setColumnSettingsVisible] = useState<boolean>(false);
  const [columnsConfig, setColumnsConfig] = useState<ColumnConfigItem[]>([]);
  const [selectedColumns, setSelectedColumns] = useState<string[]>([]);
  // 临时状态，用于编辑中的列配置
  const [tempColumnsConfig, setTempColumnsConfig] = useState<ColumnConfigItem[]>([]);
  const [tempSelectedColumns, setTempSelectedColumns] = useState<string[]>([]);
  // 用户列表相关状态
  const [userList, setUserList] = useState<any[]>([]);
  const [userListLoading, setUserListLoading] = useState<boolean>(false);
  const [isAdmin, setIsAdmin] = useState<boolean>(localStorage.getItem("role") === "admin");
  
  const [input, setInput] = useState({
    page: 1,
    pageSize: 10,
    type: -1,
    model: "",
    startTime: dayjs().subtract(15, 'day').format("YYYY-MM-DD HH:mm:ss"),
    endTime: null,
    keyword: "",
    userId: "",
  } as {
    page: number;
    pageSize: number;
    type: -1 | 1 | 2 | 3 | 4;
    model: string;
    startTime: string | null;
    endTime: string | null;
    keyword: string;
    organizationId?: string;
    userId?: string;
  });

  // 拖拽传感器
  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 5,
      },
    })
  );

  // Style objects using theme tokens
  const styles = {
    pageContainer: {
      padding: 24,
      maxWidth: 1600,
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
      fontSize: 20,
      fontWeight: 500,
      color: token.colorTextHeading,
    },
    statsRow: {
      marginBottom: 24,
    },
    statsCard: {
      height: '100%',
      borderRadius: 8,
      border: `1px solid ${token.colorBorderSecondary}`,
      boxShadow: 'none',
    },
    statsValue: {
      fontSize: 24,
      fontWeight: 600,
      display: 'block',
      color: token.colorTextHeading,
      marginBottom: 4,
    },
    statsLabel: {
      color: token.colorTextSecondary,
      fontSize: 14,
    },
    filterSection: {
      marginBottom: 24,
    },
    filterCollapse: {
      borderRadius: 8,
      overflow: 'hidden',
      border: `1px solid ${token.colorBorderSecondary}`,
      background: token.colorBgElevated,
    },
    filtersGrid: {
      display: 'grid',
      gridTemplateColumns: 'repeat(auto-fill, minmax(240px, 1fr))',
      gap: 16,
    },
    filterItem: {
      display: 'flex',
      flexDirection: 'column' as const,
      gap: 8,
    },
    filterLabel: {
      fontSize: 14,
      color: token.colorTextSecondary,
      marginBottom: 4,
    },
    actionBar: {
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center',
      marginBottom: 16,
      flexWrap: 'wrap' as const,
      gap: 8,
    },
    tableCard: {
      borderRadius: 8,
      border: `1px solid ${token.colorBorderSecondary}`,
      boxShadow: 'none',
      overflow: 'hidden',
      background: token.colorBgContainer,
    },
    tableCardBody: {
      padding: 0,
    },
    tableRowLight: {
      backgroundColor: token.colorBgContainer,
      transition: 'background-color 0.3s',
      '&:hover': {
        backgroundColor: `${token.colorPrimaryBg} !important`,
      },
    },
    tableRowDark: {
      backgroundColor: token.colorFillAlter,
      transition: 'background-color 0.3s',
      '&:hover': {
        backgroundColor: `${token.colorPrimaryBg} !important`,
      },
    },
    tag: {
      margin: 2,
    },
    // 展开区域样式
    expandedRow: {
      padding: '16px 24px',
      background: token.colorBgElevated,
      borderRadius: 8,
    },
    metadataCard: {
      marginBottom: 16,
      background: token.colorBgContainer,
      padding: 16,
      borderRadius: 8,
      border: `1px solid ${token.colorBorderSecondary}`,
    },
    metadataLabel: {
      fontSize: 12,
      color: token.colorTextSecondary,
    },
    metadataValue: {
      fontSize: 14,
      color: token.colorText,
      whiteSpace: 'nowrap' as const,
      overflow: 'hidden' as const,
      textOverflow: 'ellipsis' as const,
    },
    timelineContainer: {
      marginTop: 16,
    },
    timelineTitle: {
      fontSize: 16,
      marginBottom: 16,
      fontWeight: 500,
    },
    timelineItem: {
      background: token.colorBgContainer,
      padding: '12px 16px',
      borderRadius: 8,
      border: `1px solid ${token.colorBorderSecondary}`,
      marginBottom: 8,
      boxShadow: '0 1px 2px rgba(0,0,0,0.02)',
    },
    timelineHeader: {
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center',
      marginBottom: 4,
    },
    timelineDetails: {
      fontSize: 12,
      color: token.colorTextSecondary,
    },
    emptyData: {
      padding: '40px 0',
    },
    columnSettingsModal: {
      padding: 0,
    },
    columnSettingsHeader: {
      padding: '12px 16px',
      borderBottom: `1px solid ${token.colorBorderSecondary}`,
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center',
    },
    columnSettingsList: {
      padding: '8px 0',
      maxHeight: '400px',
      overflow: 'auto',
    },
    columnSettingsItem: {
      padding: '8px 16px',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'space-between',
      borderBottom: `1px solid ${token.colorBorderSecondary}`,
      transition: 'background-color 0.2s',
      '&:hover': {
        backgroundColor: token.colorBgTextHover,
      },
    },
    columnSettingsItemTitle: {
      flex: 1,
      marginLeft: 8,
    },
    columnSettingsFooter: {
      padding: '12px 16px',
      borderTop: `1px solid ${token.colorBorderSecondary}`,
      display: 'flex',
      justifyContent: 'space-between',
    },
    columnSettingsHandle: {
      cursor: 'grab',
      color: token.colorTextSecondary,
      display: 'flex',
      alignItems: 'center',
    },
    tableHeader: {
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center',
      padding: '12px 16px',
      borderBottom: `1px solid ${token.colorBorderSecondary}`,
      backgroundColor: token.colorBgContainer,
    },
    tableHeaderTitle: {
      margin: 0,
      fontSize: 16,
      fontWeight: 500,
    },
    tableSettingsButton: {
      display: 'flex',
      alignItems: 'center',
    },
  };

  // 使用ref来跟踪配置是否已初始化
  const configInitialized = useRef<boolean>(false);

  // 获取链路追踪数据
  const fetchTraceData = async (loggerId: string) => {
    if (traceDataMap[loggerId]) return;
    
    setLoadingTraceMap(prev => ({ ...prev, [loggerId]: true }));
    try {
      const response = await GetServerLoad(loggerId);
      if (response.success) {
        setTraceDataMap(prev => ({ ...prev, [loggerId]: response.data }));
      } else {
        message.error(response.message || '获取链路追踪数据失败');
      }
    } catch (error) {
      message.error('获取链路追踪数据时发生错误');
      console.error(error);
    } finally {
      setLoadingTraceMap(prev => ({ ...prev, [loggerId]: false }));
    }
  };

  // 展开/折叠行
  const handleExpandRow = (expanded: boolean, record: any) => {
    if (expanded) {
      setExpandedRowKeys([record.id]);
      fetchTraceData(record.id);
    } else {
      setExpandedRowKeys([]);
    }
  };

  // 渲染链路追踪时间线
  const renderTraceTimeline = (data: any) => {
    if (!data) return null;
    
    const renderTimelineItem = (item: any, index: number) => {
      // 计算执行时间
      const duration = item.duration ? `${item.duration}ms` : '0ms';
      
      // 根据操作类型或服务名称设置不同颜色
      let itemColor = 'blue';
      if (item.serviceName.includes('System.Net.Http')) {
        itemColor = 'purple';
      } else if (item.name.includes('错误') || item.status === 2) {
        itemColor = 'red';
      } else if (item.name.includes('消费') || item.name.includes('扣款')) {
        itemColor = 'green';
      } else if (item.name.includes('OpenAI') || item.name.includes('模型')) {
        itemColor = 'cyan';
      }
      
      // 状态图标
      let dotIcon = <CheckCircleOutlined style={{ fontSize: '16px' }} />;
      
      return (
        <Timeline.Item 
          key={item.id || index} 
          color={itemColor}
          dot={dotIcon}
          style={{ paddingBottom: 5 }}
        >
          <div style={styles.timelineItem}>
            <div style={styles.timelineHeader}>
              <span>
                <Tag color={itemColor} style={{ marginRight: 8 }}>
                  {item.name}
                </Tag>
              </span>
              <span style={{ color: token.colorTextSecondary, fontSize: '12px' }}>
                {item.startTime ? item.startTime.split(' ')[1] : ''}
              </span>
            </div>
            <div style={styles.timelineDetails}>
              <Space size={12}>
                <span>服务: {item.serviceName}</span>
                {item.duration > 0 && <span>耗时: {duration}</span>}
                {Object.keys(item.attributes).length > 0 && (
                  <Tooltip 
                    title={
                      <div>
                        {Object.entries(item.attributes).map(([key, value]) => (
                          <div key={key}>{key}: {String(value)}</div>
                        ))}
                      </div>
                    }
                  >
                    <span style={{ cursor: 'pointer', textDecoration: 'underline' }}>查看属性</span>
                  </Tooltip>
                )}
              </Space>
            </div>
          </div>
          
          {/* 如果有子节点，递归渲染 */}
          {item.children && item.children.length > 0 && (
            <div style={{ marginLeft: 20 }}>
              <Timeline>
                {item.children.map((child: any, childIndex: number) => renderTimelineItem(child, childIndex))}
              </Timeline>
            </div>
          )}
        </Timeline.Item>
      );
    };
    
    return (
      <Timeline>
        {renderTimelineItem(data, 0)}
      </Timeline>
    );
  };

  // 渲染展开行内容
  const expandedRowRender = (record: any) => {
    const loggerId = record.id;
    const isLoading = loadingTraceMap[loggerId];
    const traceData = traceDataMap[loggerId];
    
    if (isLoading) {
      return (
        <div style={{ padding: '20px 0', textAlign: 'center' }}>
          <Skeleton active paragraph={{ rows: 3 }} />
        </div>
      );
    }
    
    if (!traceData) {
      return (
        <div style={styles.emptyData}>
          <Empty description="暂无链路追踪数据" />
        </div>
      );
    }
    
    return (
      <div style={styles.expandedRow}>
        <div style={styles.metadataCard}>
          <Row gutter={[16, 16]}>
            <Col span={8}>
              <div style={styles.metadataLabel}>跟踪ID</div>
              <div style={styles.metadataValue} title={traceData.traceId || '-'}>
                {traceData.traceId || '-'}
              </div>
            </Col>
            <Col span={8}>
              <div style={styles.metadataLabel}>日志ID</div>
              <div style={styles.metadataValue} title={traceData.chatLoggerId || '-'}>
                {traceData.chatLoggerId || '-'}
              </div>
            </Col>
            <Col span={8}>
              <div style={styles.metadataLabel}>开始时间</div>
              <div style={styles.metadataValue}>{traceData.startTime || '-'}</div>
            </Col>
            <Col span={8}>
              <div style={styles.metadataLabel}>服务名称</div>
              <div style={styles.metadataValue}>{traceData.serviceName || '-'}</div>
            </Col>
            <Col span={8}>
              <div style={styles.metadataLabel}>总耗时</div>
              <div style={styles.metadataValue}>{traceData.duration ? `${traceData.duration}ms` : '-'}</div>
            </Col>
            <Col span={8}>
              <div style={styles.metadataLabel}>创建时间</div>
              <div style={styles.metadataValue}>{traceData.createdAt || '-'}</div>
            </Col>
          </Row>
        </div>
        
        <div style={styles.timelineContainer}>
          <Typography.Title level={5} style={styles.timelineTitle}>调用链路</Typography.Title>
          {renderTraceTimeline(traceData)}
        </div>
      </div>
    );
  };

  function timeString(totalTime: number) {
    let s = totalTime / 1000;
    let m = Math.floor(s / 60);
    s = Math.floor(s % 60);
    return `${m}分${s}秒`;
  }

  // 原始列配置
  const originalColumns = useMemo<ColumnConfigItem[]>(() => [
    {
      title: "时间",
      fixed: "left" as const,
      dataIndex: "createdAt",
      key: "createdAt",
      width: 180,
      sorter: (a: any, b: any) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime(),
    },
    {
      title: "消费",
      dataIndex: "quota",
      key: "quota",
      width: 120,
      render: (value: any) => value && (
        <Tag 
          color="green" 
          style={styles.tag}
        >
          {renderQuota(value, 6)}
        </Tag>
      ),
      sorter: (a: any, b: any) => a.quota - b.quota,
    },
    {
      disable: !(localStorage.getItem("role") === "admin"),
      title: "渠道",
      dataIndex: "channelName",
      width: 190,
      key: "channelName",
      render: (value: any) => value && (
        <Tag 
          color="blue" 
          style={styles.tag}
        >
          {value}
        </Tag>
      ),
    },
    {
      title: "用户",
      dataIndex: "userName",
      width: 90,
      key: "userName",
      render: (value: any) => value && (
        <Tag 
          color="blue" 
          style={styles.tag}
        >
          {value}
        </Tag>
      ),
    },
    {
      title: "令牌名称",
      dataIndex: "tokenName",
      width: 190,
      key: "tokenName",
      ellipsis: true,
    },
    {
      title: '组织id',
      dataIndex: 'organizationId',
      key: 'organizationId',
      width: 120,
      ellipsis: true,
    },
    {
      title: "模型",
      dataIndex: "modelName",
      width: 180,
      key: "modelName",
      render: (value: any) => {
        return value && (
          <Tag
            style={styles.tag}
            onClick={() => {
              navigator.clipboard
                .writeText(value)
                .then(() => {
                  message.success({ content: "复制成功" });
                })
                .catch(() => {
                  message.error({ content: "复制失败" });
                });
            }}
          >
            {value}
          </Tag>
        );
      }
    },
    {
      title: "用时",
      dataIndex: "duration",
      key: "duration",
      width: 160,
      render: (_: any, item: any) => {
        return (
          <Space size={4}>
            <Tag 
              color="pink" 
              style={styles.tag}
            >
              {timeString(item.totalTime)}
            </Tag>
            <Tag 
              color="gold" 
              style={styles.tag}
            >
              {item.stream ? "流式" : "非流式"}
            </Tag>
          </Space>
        );
      },
      sorter: (a: any, b: any) => a.totalTime - b.totalTime,
    },
    {
      title: "提示tokens",
      dataIndex: "promptTokens",
      key: "promptTokens",
      width: 120,
      sorter: (a: any, b: any) => a.promptTokens - b.promptTokens,
    },
    {
      title: "完成tokens",
      dataIndex: "completionTokens",
      key: "completionTokens",
      width: 120,
      sorter: (a: any, b: any) => a.completionTokens - b.completionTokens,
    },
    {
      title: "请求IP地址",
      dataIndex: "ip",
      key: "ip",
      width: 120,
    },
    {
      title: "详情",
      dataIndex: "content",
      key: "content",
      width: 250,
      ellipsis: {
        showTitle: false,
      },
      render: (value: any) => (
        <Tooltip placement="topLeft" title={value}>
          {value}
        </Tooltip>
      ),
    },
    {
      title: "客户端信息",
      dataIndex: "userAgent",
      key: "userAgent",
      width: 150,
      ellipsis: {
        showTitle: false,
      },
      render: (value: any) => (
        <Tooltip placement="topLeft" title={value}>
          {value}
        </Tooltip>
      ),
    },
  ], [styles.tag]);

  // 初始化列配置
  useEffect(() => {
    // 防止重复初始化
    if (configInitialized.current) {
      return;
    }
    
    try {
      // 尝试从localStorage获取已保存的配置
      const savedConfig = localStorage.getItem('logger_columns_config');
      const savedSelected = localStorage.getItem('logger_selected_columns');
      
      if (savedConfig && savedSelected) {
        try {
          const parsedConfig = JSON.parse(savedConfig);
          const parsedSelected = JSON.parse(savedSelected);
          
          // 验证数据有效性
          if (Array.isArray(parsedConfig) && parsedConfig.length > 0 && 
              Array.isArray(parsedSelected) && parsedSelected.length > 0) {
            setColumnsConfig(parsedConfig);
            setSelectedColumns(parsedSelected);
            setTempColumnsConfig(parsedConfig);
            setTempSelectedColumns(parsedSelected);
            console.log('从本地存储加载列配置', parsedConfig, parsedSelected);
            configInitialized.current = true;
            return;
          }
        } catch (error) {
          console.error('解析列配置出错', error);
        }
      }
      
      // 默认配置
      const defaultSelected = originalColumns
        .filter(col => !col.disable)
        .map(col => col.key);
      setColumnsConfig(originalColumns);
      setSelectedColumns(defaultSelected);
      setTempColumnsConfig(originalColumns);
      setTempSelectedColumns(defaultSelected);
      console.log('使用默认列配置');
      configInitialized.current = true;
    } catch (error) {
      console.error('初始化列配置出错', error);
      // 失败时的默认配置
      const defaultSelected = originalColumns
        .filter(col => !col.disable)
        .map(col => col.key);
      setColumnsConfig(originalColumns);
      setSelectedColumns(defaultSelected);
      setTempColumnsConfig(originalColumns);
      setTempSelectedColumns(defaultSelected);
      configInitialized.current = true;
    }
  }, [originalColumns]);

  // 打开列设置弹窗时初始化临时状态
  const openColumnSettings = () => {
    setTempColumnsConfig([...columnsConfig]);
    setTempSelectedColumns([...selectedColumns]);
    setColumnSettingsVisible(true);
  };
  
  // 关闭列设置弹窗时不保存更改
  const closeColumnSettings = () => {
    setColumnSettingsVisible(false);
    // 丢弃临时更改
    setTempColumnsConfig([...columnsConfig]);
    setTempSelectedColumns([...selectedColumns]);
  };

  // 保存列配置
  const saveColumnSettings = () => {
    if (tempSelectedColumns.length === 0) {
      message.warning('请至少选择一列');
      return;
    }
    
    try {
      // 应用临时状态到实际状态
      setColumnsConfig([...tempColumnsConfig]);
      setSelectedColumns([...tempSelectedColumns]);
      
      // 保存到本地存储
      const configToSave = JSON.stringify(tempColumnsConfig);
      const selectedToSave = JSON.stringify(tempSelectedColumns);
      
      localStorage.setItem('logger_columns_config', configToSave);
      localStorage.setItem('logger_selected_columns', selectedToSave);
      
      console.log('保存列配置', {
        columnsConfig: tempColumnsConfig,
        selectedColumns: tempSelectedColumns
      });
      
      setColumnSettingsVisible(false);
      message.success('列配置已保存');
    } catch (error) {
      console.error('保存列配置出错', error);
      message.error('保存列配置失败');
    }
  };

  // 重置列配置
  const resetColumnSettings = () => {
    // 只重置临时状态
    setTempColumnsConfig(originalColumns);
    setTempSelectedColumns(originalColumns
      .filter(col => !col.disable)
      .map(col => col.key)
    );
  };

  // 切换列显示状态
  const toggleColumnVisibility = (key: string) => {
    if (tempSelectedColumns.includes(key)) {
      // 至少保留一列
      if (tempSelectedColumns.length > 1) {
        setTempSelectedColumns(tempSelectedColumns.filter(k => k !== key));
      } else {
        message.warning('至少需要保留一列');
      }
    } else {
      setTempSelectedColumns([...tempSelectedColumns, key]);
    }
  };

  // 处理列排序
  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;
    
    if (active.id !== over?.id) {
      setTempColumnsConfig((columns) => {
        const oldIndex = columns.findIndex((col) => col.key === active.id);
        const newIndex = columns.findIndex((col) => col.key === over?.id);
        
        return arrayMove(columns, oldIndex, newIndex);
      });
    }
  };

  // 获取当前使用的列配置
  const activeColumns = useMemo<ColumnConfigItem[]>(() => {
    // 确保选择的列不为空，如果为空使用默认显示
    if (selectedColumns.length === 0) {
      const defaultColumns = columnsConfig
        .filter(col => col.disable === undefined || col.disable === false)
        .slice(0, 3); // 至少显示前三列
      
      return defaultColumns.map((col, index) => ({
        ...col,
        ...(index === 0 ? { fixed: 'left' as const } : {}),
      }));
    }
    
    // 正常情况
    const filtered = columnsConfig
      .filter(col => selectedColumns.includes(col.key) && (col.disable === undefined || col.disable === false));
    
    // 如果筛选后没有列，则显示第一列
    if (filtered.length === 0 && columnsConfig.length > 0) {
      const firstCol = columnsConfig[0];
      return [{ ...firstCol, fixed: 'left' as const }];
    }
    
    return filtered.map((col, index) => ({
      ...col,
      // 为第一列添加固定属性
      ...(index === 0 ? { fixed: 'left' as const } : {}),
    }));
  }, [columnsConfig, selectedColumns]);

  function loadData() {
    setLoading(true);

    getLoggers(input)
      .then((res) => {
        if (res.success) {
          setData(res.data.items);
          setTotal(res.data.total);
        } else {
          message.error({
            content: res.message,
          });
        }
      })
      .finally(() => {
        setLoading(false);
      });
  }

  function loadViewConsumption() {
    setConsumeLoading(true);
    viewConsumption(input)
      .then((res) => {
        if (res.success) {
          setConsume(res.data);
        } else {
          message.error({
            content: res.message,
          });
        }
      })
      .finally(() => {
        setConsumeLoading(false);
      });
  }

  // 加载用户列表
  const loadUserList = async () => {
    if (!isAdmin) return;
    
    setUserListLoading(true);
    try {
      const response = await getSimpleList();
      if (response.success) {
        setUserList(response.data);
      } else {
        message.error(response.message || '获取用户列表失败');
      }
    } catch (error) {
      message.error('获取用户列表时发生错误');
      console.error(error);
    } finally {
      setUserListLoading(false);
    }
  };

  useEffect(() => {
    loadData();
    loadViewConsumption();
  }, [input.page, input.pageSize]);

  // 初始化加载用户列表
  useEffect(() => {
    if (isAdmin) {
      loadUserList();
    }
  }, [isAdmin]);

  // 数值显示组件
  const ConsumptionNumber = ({ value }: { value: number }) => {
    return (
      <span style={styles.statsValue}>
        {renderQuota(value, 4)}
      </span>
    );
  };

  const handleSearch = () => {
    setInput({
      ...input,
      page: 1,
    });
    loadData();
    loadViewConsumption();
  };

  const handleReset = () => {
    setInput({
      page: 1,
      pageSize: 10,
      type: -1,
      model: "",
      startTime: dayjs().subtract(15, 'day').format("YYYY-MM-DD HH:mm:ss"),
      endTime: null,
      keyword: "",
      userId: "",
    });
    setTimeout(() => {
      loadData();
      loadViewConsumption();
    }, 0);
  };

  const handleExport = async () => {
    try {
      setExportLoading(true);
      message.loading({ content: '正在导出数据...', key: 'export' });
      
      // 准备导出参数，不包括分页参数
      const exportParams = {
        type: input.type,
        model: input.model,
        startTime: input.startTime,
        endTime: input.endTime,
        keyword: input.keyword,
        organizationId: input.organizationId,
        userId: input.userId,
      };

      const response = await Export(exportParams);
      
      // 创建下载链接
      const blob = new Blob([response], { 
        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' 
      });
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      
      // 生成文件名
      const timestamp = dayjs().format('YYYY-MM-DD_HH-mm-ss');
      link.download = `日志数据_${timestamp}.xlsx`;
      
      // 触发下载
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
      
      message.success({ content: '数据导出成功！', key: 'export' });
    } catch (error) {
      console.error('导出失败:', error);
      message.error({ content: '导出失败，请重试', key: 'export' });
    } finally {
      setExportLoading(false);
    }
  };

  return (
    <ConfigProvider
      theme={{
        token: {
          borderRadius: 8,
          colorBgContainer: token.colorBgContainer,
          colorBorderSecondary: token.colorBorderSecondary,
          colorPrimary: token.colorPrimary,
          colorTextHeading: token.colorTextHeading,
          colorTextSecondary: token.colorTextSecondary,
          colorFillAlter: token.colorFillAlter,
        }
      }}
    >
      <div style={styles.pageContainer}>
        <div style={styles.pageHeader}>
          <Title level={4} style={styles.pageTitle}>系统日志</Title>
          <Space wrap>
            <Button 
              icon={<ReloadOutlined />} 
              onClick={handleReset}
            >
              重置
            </Button>
            <Button
              type="primary"
              icon={filterVisible ? <FilterOutlined /> : <FilterOutlined />}
              onClick={() => setFilterVisible(!filterVisible)}
            >
              {filterVisible ? "隐藏筛选" : "显示筛选"}
            </Button>
          </Space>
        </div>

        <Row gutter={[16, 16]} style={styles.statsRow}>
          <Col xs={24} sm={12} md={8} lg={6}>
            <Card
              style={styles.statsCard}
              bodyStyle={{ padding: 16 }}
            >
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                <div>
                  {consumeLoading ? (
                    <Skeleton.Button active size="small" style={{ width: "120px", height: "32px" }} />
                  ) : (
                    <ConsumptionNumber value={consume} />
                  )}
                  <Text style={styles.statsLabel}>区间总消费</Text>
                </div>
                <BarChartOutlined style={{ fontSize: 20, color: token.colorPrimary }} />
              </div>
            </Card>
          </Col>
        </Row>

        {filterVisible && (
          <div style={styles.filterSection}>
            <Collapse 
              defaultActiveKey={['1']}
              style={styles.filterCollapse}
              ghost
            >
              <Panel 
                header={<Text strong>搜索筛选</Text>} 
                key="1"
              >
                <div style={styles.filtersGrid}>
                  <div style={styles.filterItem}>
                    <Text style={styles.filterLabel}>模型名称</Text>
                    <Input
                      placeholder="输入模型名称"
                      value={input.model}
                      onChange={(e) => {
                        setInput({
                          ...input,
                          model: e.target.value,
                        });
                      }}
                      prefix={<SearchOutlined style={{ color: token.colorTextSecondary }} />}
                      allowClear
                    />
                  </div>
                  
                  <div style={styles.filterItem}>
                    <Text style={styles.filterLabel}>类型</Text>
                    <Select
                      style={{ width: '100%' }}
                      value={input.type}
                      onChange={(e: any) => {
                        setInput({
                          ...input,
                          type: e,
                        });
                      }}
                      placeholder="选择类型"
                    >
                      <Select.Option value={-1}>全部</Select.Option>
                      <Select.Option value={1}>消费</Select.Option>
                      <Select.Option value={2}>充值</Select.Option>
                      <Select.Option value={3}>系统</Select.Option>
                      <Select.Option value={4}>新增用户</Select.Option>
                    </Select>
                  </div>
                  
                  <div style={styles.filterItem}>
                    <Text style={styles.filterLabel}>关键字</Text>
                    <Input
                      placeholder="搜索关键字"
                      value={input.keyword}
                      onChange={(e) => {
                        setInput({
                          ...input,
                          keyword: e.target.value,
                        });
                      }}
                      prefix={<SearchOutlined style={{ color: token.colorTextSecondary }} />}
                      allowClear
                    />
                  </div>
                  
                  <div style={styles.filterItem}>
                    <Text style={styles.filterLabel}>组织ID</Text>
                    <Input
                      placeholder="输入组织ID"
                      value={input.organizationId}
                      onChange={(e) => {
                        setInput({
                          ...input,
                          organizationId: e.target.value,
                        });
                      }}
                      prefix={<UserOutlined style={{ color: token.colorTextSecondary }} />}
                      allowClear
                    />
                  </div>

                  {isAdmin && (
                    <div style={styles.filterItem}>
                      <Text style={styles.filterLabel}>指定用户</Text>
                      <Select
                        style={{ width: '100%' }}
                        placeholder="选择用户（管理员专用）"
                        value={input.userId}
                        onChange={(value: string) => {
                          setInput({
                            ...input,
                            userId: value,
                          });
                        }}
                        allowClear
                        loading={userListLoading}
                        showSearch
                        filterOption={(inputValue, option) =>
                          option?.children?.toString().toLowerCase().includes(inputValue.toLowerCase()) || false
                        }
                      >
                        {userList.map((user: any) => (
                          <Select.Option key={user.id} value={user.id}>
                            {user.userName}
                          </Select.Option>
                        ))}
                      </Select>
                    </div>
                  )}
                  
                  <div style={styles.filterItem}>
                    <Text style={styles.filterLabel}>开始时间</Text>
                    <DatePicker
                      style={{ width: '100%' }}
                      value={input.startTime ? dayjs(input.startTime) : null}
                      onChange={(e: any) => {
                        setInput({
                          ...input,
                          startTime: e ? e.format("YYYY-MM-DD HH:mm:ss") : null,
                        });
                      }}
                      placeholder="选择开始时间"
                      allowClear
                      showTime
                      format="YYYY-MM-DD HH:mm:ss"
                      suffixIcon={<CalendarOutlined style={{ color: token.colorTextSecondary }} />}
                    />
                  </div>
                  
                  <div style={styles.filterItem}>
                    <Text style={styles.filterLabel}>结束时间</Text>
                    <DatePicker
                      style={{ width: '100%' }}
                      value={input.endTime ? dayjs(input.endTime) : null}
                      onChange={(e: any) => {
                        setInput({
                          ...input,
                          endTime: e ? e.format("YYYY-MM-DD HH:mm:ss") : null,
                        });
                      }}
                      placeholder="选择结束时间"
                      allowClear
                      showTime
                      format="YYYY-MM-DD HH:mm:ss"
                      suffixIcon={<CalendarOutlined style={{ color: token.colorTextSecondary }} />}
                    />
                  </div>
                </div>
                
                <Divider style={{ margin: '16px 0' }} />
                
                <div style={{ display: 'flex', justifyContent: 'flex-end' }}>
                  <Space>
                    <Button onClick={handleReset}>重置</Button>
                    <Button type="primary" onClick={handleSearch}>
                      搜索
                    </Button>
                  </Space>
                </div>
              </Panel>
            </Collapse>
          </div>
        )}

        <div style={styles.actionBar}>
          <Space>
            <Button
              icon={<ReloadOutlined />}
              onClick={loadData}
              loading={loading}
            >
              刷新
            </Button>
            
            <Button
              icon={<DownloadOutlined />}
              onClick={handleExport}
              loading={exportLoading}
            >
              导出数据
            </Button>
          </Space>
          
          <Text type="secondary">
            共 {total} 条记录
          </Text>
        </div>

        <Card 
          style={styles.tableCard} 
          bodyStyle={styles.tableCardBody}
          title={
            <div style={styles.tableHeader}>
              <Typography.Title level={5} style={styles.tableHeaderTitle}>
                日志记录
              </Typography.Title>
              <AntTooltip title="列配置">
                <Button
                  icon={<SettingOutlined />}
                  type="text"
                  onClick={openColumnSettings}
                  style={styles.tableSettingsButton}
                >
                  <Badge 
                    count={columnsConfig.filter(c => !c.disable).length - selectedColumns.length} 
                    size="small" 
                    offset={[5, -5]}
                    showZero={false}
                  >
                    <span>列配置</span>
                  </Badge>
                </Button>
              </AntTooltip>
            </div>
          }
        >
          <Table
            scroll={{
              x: "max-content",
              y: "calc(100vh - 420px)",
            }}
            loading={loading}
            columns={activeColumns as any}
            dataSource={data}
            rowKey={(record) => record.id || Math.random().toString()}
            pagination={{
              total: total,
              pageSize: input.pageSize,
              current: input.page,
              showSizeChanger: true,
              showQuickJumper: true,
              showTotal: (total) => `共 ${total} 条记录`,
              onChange: (page, pageSize) => {
                setInput({
                  ...input,
                  page,
                  pageSize,
                });
              },
              style: { padding: '16px 24px' }
            }}
            expandable={{
              expandedRowRender,
              onExpand: handleExpandRow,
              expandedRowKeys,
              expandIcon: ({ expanded, onExpand, record }) => (
                <CaretRightOutlined
                  rotate={expanded ? 90 : 0}
                  onClick={(e) => onExpand(record, e)}
                  style={{ transition: '0.3s', cursor: 'pointer', color: token.colorPrimary }}
                />
              )
            }}
            rowClassName={(_, index) => (index % 2 === 0 ? "table-row-light" : "table-row-dark")}
            size="middle"
          />
        </Card>

        {/* 列设置弹窗 */}
        <Modal
          title="表格列配置"
          open={columnSettingsVisible}
          onCancel={closeColumnSettings}
          footer={null}
          width={500}
          bodyStyle={{ padding: 0 }}
        >
          <div style={styles.columnSettingsHeader}>
            <Text>共 {tempColumnsConfig.length} 列（已选择 {tempSelectedColumns.length} 列）</Text>
            <Button type="link" onClick={resetColumnSettings}>
              恢复默认
            </Button>
          </div>

          <div style={styles.columnSettingsList}>
            <DndContext sensors={sensors} onDragEnd={handleDragEnd}>
              <SortableContext items={tempColumnsConfig.map(c => c.key)} strategy={verticalListSortingStrategy}>
                {tempColumnsConfig.map((column) => (
                  <SortableItem key={column.key} id={column.key} style={styles.columnSettingsItem}>
                    <Checkbox
                      checked={tempSelectedColumns.includes(column.key)}
                      onChange={() => toggleColumnVisibility(column.key)}
                      disabled={column.fixed === 'left' || column.disable}
                    />
                    <span style={styles.columnSettingsItemTitle}>{column.title}</span>
                    <div style={styles.columnSettingsHandle}>
                      <MenuOutlined />
                    </div>
                  </SortableItem>
                ))}
              </SortableContext>
            </DndContext>
          </div>

          <div style={styles.columnSettingsFooter}>
            <Button onClick={closeColumnSettings}>
              取消
            </Button>
            <Button type="primary" onClick={saveColumnSettings}>
              保存设置
            </Button>
          </div>
        </Modal>
      </div>
    </ConfigProvider>
  );
}
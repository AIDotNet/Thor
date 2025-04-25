import { useEffect, useState, useMemo } from "react";
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
  ConfigProvider
} from "antd";
import {
  SearchOutlined,
  FilterOutlined,
  ReloadOutlined,
  DownloadOutlined,
  BarChartOutlined,
  UserOutlined
} from "@ant-design/icons";
import { getLoggers, viewConsumption } from "../../services/LoggerService";
import { Tag, Tooltip } from "@lobehub/ui";
import { renderQuota } from "../../utils/render";
import dayjs from "dayjs";

const { Title, Text } = Typography;
const { Panel } = Collapse;
const { useToken } = theme;

export default function LoggerPage() {
  const { token } = useToken();
  const [data, setData] = useState<any[]>([]);
  const [total, setTotal] = useState<number>(0);
  const [loading, setLoading] = useState<boolean>(false);
  const [consume, setConsume] = useState<number>(0);
  const [consumeLoading, setConsumeLoading] = useState<boolean>(false);
  const [filterVisible, setFilterVisible] = useState<boolean>(true);
  const [input, setInput] = useState({
    page: 1,
    pageSize: 10,
    type: -1,
    model: "",
    startTime: null,
    endTime: null,
    keyword: "",
  } as {
    page: number;
    pageSize: number;
    type: -1 | 1 | 2 | 3 | 4;
    model: string;
    startTime: string | null;
    endTime: string | null;
    keyword: string;
    organizationId?: string;
  });

  // Style objects using theme tokens
  const styles = {
    dashboardContainer: {
      padding: 16,
      width: '100%',
      maxWidth: 1800,
      margin: '0 auto',
    },
    dashboardHeader: {
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'space-between',
      marginBottom: 24,
      flexWrap: 'wrap' as const,
      gap: 16,
    },
    headerTitle: {
      margin: 0,
      fontSize: '1.6rem',
    },
    statsCard: {
      height: '100%',
      borderRadius: token.borderRadius,
      boxShadow: `0 1px 4px ${token.colorBorderSecondary}`,
    },
    statsValue: {
      fontSize: '1.8rem',
      fontWeight: 600,
      display: 'block',
      color: token.colorTextHeading,
    },
    statsLabel: {
      color: token.colorTextSecondary,
      fontSize: '0.9rem',
    },
    filterSection: {
      marginBottom: 24,
    },
    filterCollapse: {
      borderRadius: token.borderRadius,
      overflow: 'hidden',
      background: token.colorBgContainer,
      border: `1px solid ${token.colorBorderSecondary}`,
    },
    filtersContainer: {
      display: 'grid',
      gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))',
      gap: 16,
    },
    filterItem: {
      display: 'flex',
      flexDirection: 'column' as const,
      gap: 8,
    },
    filterLabel: {
      fontSize: '0.85rem',
      color: token.colorTextSecondary,
      marginBottom: 4,
    },
    actionBar: {
      display: 'flex',
      gap: 8,
      marginBottom: 16,
      flexWrap: 'wrap' as const,
    },
    tableCard: {
      borderRadius: token.borderRadius,
      overflow: 'hidden',
      boxShadow: `0 1px 4px ${token.colorBorderSecondary}`,
    },
    tableCardBody: {
      padding: 0,
    },
    tableRowLight: {
      backgroundColor: token.colorBgContainer,
    },
    tableRowDark: {
      backgroundColor: token.colorFillAlter,
    },
    tag: {
      margin: 2,
    }
  };

  // Media query styles
  function timeString(totalTime: number) {
    let s = totalTime / 1000;
    let m = Math.floor(s / 60);
    s = Math.floor(s % 60);
    return `${m}分${s}秒`;
  }

  const columns = useMemo(() => [
    {
      title: "时间",
      fixed: "left",
      dataIndex: "createdAt",
      key: "createdAt",
      width: 180,
      sorter: (a: any, b: any) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime(),
    },
    {
      title: "消费",
      dataIndex: "quota",
      fixed: "left",
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
      filters: [
        { text: '渠道A', value: '渠道A' },
        { text: '渠道B', value: '渠道B' },
      ],
      onFilter: (value: any, record: any) => record.channelName === value,
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
      },
      filters: [
        { text: 'GPT-3.5', value: 'gpt-3.5-turbo' },
        { text: 'GPT-4', value: 'gpt-4' },
      ],
      onFilter: (value: any, record: any) => record.modelName === value,
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

  useEffect(() => {
    loadData();
    loadViewConsumption();
  }, [input.page, input.pageSize]);

  // Number display component
  const ConsumptionNumber = ({ value }: { value: number }) => {
    return (
      <span style={{ fontWeight: "bold", display: "inline-block" }}>
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
      startTime: null,
      endTime: null,
      keyword: "",
    });
    setTimeout(() => {
      loadData();
      loadViewConsumption();
    }, 0);
  };

  return (
    <ConfigProvider
      theme={{
        token: {
          borderRadius: token.borderRadius,
          colorBgContainer: token.colorBgContainer,
          colorBorderSecondary: token.colorBorderSecondary,
          colorPrimary: token.colorPrimary,
          colorTextHeading: token.colorTextHeading,
          colorTextSecondary: token.colorTextSecondary,
          colorFillAlter: token.colorFillAlter,
        }
      }}
    >
      <div style={styles.dashboardContainer}>
        <div style={styles.dashboardHeader}>
          <Title level={3} style={styles.headerTitle}>日志查询与消费统计</Title>
          <Space wrap>
            <Button 
              icon={<ReloadOutlined />} 
              onClick={handleReset}
            >
              <span className="button-text">重置</span>
            </Button>
            <Button
              type="primary"
              icon={<FilterOutlined />}
              onClick={() => setFilterVisible(!filterVisible)}
            >
              <span className="button-text">
                {filterVisible ? "隐藏筛选" : "显示筛选"}
              </span>
            </Button>
          </Space>
        </div>

        <Row gutter={[16, 16]} style={{ marginBottom: '24px' }}>
          <Col xs={24} sm={12} md={8} lg={6}>
            <Card
              style={styles.statsCard}
              bodyStyle={{ padding: 16 }}
            >
              <Text style={styles.statsValue}>
                {consumeLoading ? (
                  <Skeleton.Button active size="small" style={{ width: "120px", height: "32px" }} />
                ) : (
                  <ConsumptionNumber value={consume} />
                )}
              </Text>
              <Text style={styles.statsLabel}>区间总消费</Text>
              <div style={{ position: 'absolute', right: '16px', top: '16px' }}>
                <Button 
                  type="text" 
                  icon={<BarChartOutlined />} 
                />
              </div>
            </Card>
          </Col>
        </Row>

        {filterVisible && (
          <div style={{ marginBottom: '24px' }}>
            <div style={styles.filterSection}>
              <Collapse 
                defaultActiveKey={['1']}
                style={styles.filterCollapse}
              >
                <Panel header="搜索筛选" key="1">
                  <div style={styles.filtersContainer}>
                    <div style={styles.filterItem}>
                      <Text style={styles.filterLabel}>模型名称</Text>
                      <Input
                        prefix={<SearchOutlined />}
                        value={input.model}
                        onChange={(e) => {
                          setInput({
                            ...input,
                            model: e.target.value,
                          });
                        }}
                        placeholder="模型名称"
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
                        prefix={<SearchOutlined />}
                        value={input.keyword}
                        onChange={(e) => {
                          setInput({
                            ...input,
                            keyword: e.target.value,
                          });
                        }}
                        placeholder="关键字"
                        allowClear
                      />
                    </div>
                    
                    <div style={styles.filterItem}>
                      <Text style={styles.filterLabel}>组织ID</Text>
                      <Input
                        prefix={<UserOutlined />}
                        value={input.organizationId}
                        onChange={(e) => {
                          setInput({
                            ...input,
                            organizationId: e.target.value,
                          });
                        }}
                        placeholder="组织ID"
                        allowClear
                      />
                    </div>
                    
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
                        placeholder="开始时间"
                        allowClear
                        showTime
                        format="YYYY-MM-DD HH:mm:ss"
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
                        placeholder="结束时间"
                        allowClear
                        showTime
                        format="YYYY-MM-DD HH:mm:ss"
                      />
                    </div>
                  </div>
                  
                  <Divider style={{ margin: '16px 0' }} />
                  
                  <div style={{ display: 'flex', justifyContent: 'flex-end' }}>
                    <Space>
                      <Button onClick={handleReset}>重置</Button>
                      <Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>
                        搜索
                      </Button>
                    </Space>
                  </div>
                </Panel>
              </Collapse>
            </div>
          </div>
        )}

        <div style={styles.actionBar}>
          <Button
            icon={<ReloadOutlined />}
            onClick={loadData}
            loading={loading}
          >
            <span className="button-text">刷新</span>
          </Button>
          
          <Button
            icon={<DownloadOutlined />}
          >
            <span className="button-text">导出数据</span>
          </Button>
        </div>

        <div>
          <Card style={styles.tableCard} bodyStyle={styles.tableCardBody}>
            <Table
              scroll={{
                x: "max-content",
                y: "calc(100vh - 420px)",
              }}
              loading={loading}
              columns={columns.filter(
                (item) => item.disable == false || item.disable == undefined
              ) as any}
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
              }}
              rowClassName={(_, index) => (index % 2 === 0 ? "table-row-light" : "table-row-dark")}
              size="middle"
            />
          </Card>
        </div>
      </div>
    </ConfigProvider>
  );
}
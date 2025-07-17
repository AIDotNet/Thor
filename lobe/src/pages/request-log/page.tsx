import { useEffect, useState } from "react";
import {
  message,
  Button,
  Input,
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
  Empty,
  Tag,
  Tooltip,
  Modal
} from "antd";
import {
  SearchOutlined,
  FilterOutlined,
  ReloadOutlined,
  CalendarOutlined,
  CheckCircleOutlined,
  CloseCircleOutlined,
  ClockCircleOutlined,
  EyeOutlined,
  CopyOutlined
} from "@ant-design/icons";
import { getRequestLogs } from "../../services/RequestLogService";
import dayjs from "dayjs";
import JsonView from '@uiw/react-json-view';
import { Highlighter } from "@lobehub/ui";

const { Title, Text } = Typography;
const { Panel } = Collapse;
const { useToken } = theme;

export default function RequestLogPage() {
  const { token } = useToken();
  const [data, setData] = useState<any[]>([]);
  const [total, setTotal] = useState<number>(0);
  const [loading, setLoading] = useState<boolean>(false);
  const [filterVisible, setFilterVisible] = useState<boolean>(true);
  const [detailModalVisible, setDetailModalVisible] = useState<boolean>(false);
  const [selectedRecord, setSelectedRecord] = useState<any>(null);

  const [input, setInput] = useState({
    page: 1,
    pageSize: 10,
    model: "",
    userName: "",
    startTime: dayjs().subtract(7, 'day').format("YYYY-MM-DD HH:mm:ss"),
    endTime: null,
  } as {
    page: number;
    pageSize: number;
    model: string;
    userName: string;
    startTime: string | null;
    endTime: string | null;
  });

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
    tag: {
      margin: 2,
    },
  };

  const columns = [
    {
      title: "请求时间",
      dataIndex: "requestTime",
      key: "requestTime",
      width: 180,
      render: (value: string) => dayjs(value).format("YYYY-MM-DD HH:mm:ss"),
      sorter: (a: any, b: any) => new Date(a.requestTime).getTime() - new Date(b.requestTime).getTime(),
    },
    {
      title: "路由路径",
      dataIndex: "routePath",
      key: "routePath",
      width: 200,
      ellipsis: true,
    },
    {
      title: "状态",
      dataIndex: "isSuccess",
      key: "isSuccess",
      width: 100,
      render: (value: boolean, record: any) => (
        <Tag
          color={value ? "success" : "error"}
          icon={value ? <CheckCircleOutlined /> : <CloseCircleOutlined />}
          style={styles.tag}
        >
          {record.httpStatusCode}
        </Tag>
      ),
    },
    {
      title: "耗时",
      dataIndex: "durationMs",
      key: "durationMs",
      width: 100,
      render: (value: number) => (
        <Tag color="blue" icon={<ClockCircleOutlined />} style={styles.tag}>
          {value}ms
        </Tag>
      ),
      sorter: (a: any, b: any) => a.durationMs - b.durationMs,
    },
    {
      title: "客户端IP",
      dataIndex: "clientIp",
      key: "clientIp",
      width: 120,
    },
    {
      title: "用户代理",
      dataIndex: "userAgent",
      key: "userAgent",
      width: 200,
      ellipsis: {
        showTitle: false,
      },
      render: (value: string) => (
        <Tooltip placement="topLeft" title={value}>
          {value}
        </Tooltip>
      ),
    },
    {
      title: "操作",
      key: "action",
      width: 100,
      render: (_: any, record: any) => (
        <Button
          type="link"
          size="small"
          icon={<EyeOutlined />}
          onClick={() => {
            setSelectedRecord(record);
            setDetailModalVisible(true);
          }}
        >
          详情
        </Button>
      ),
    },
  ];

  function loadData() {
    setLoading(true);

    getRequestLogs(
      input.page,
      input.pageSize,
      input.model,
      input.userName,
      input.startTime || "",
      input.endTime || ""
    )
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

  useEffect(() => {
    loadData();
  }, [input.page, input.pageSize]);

  const handleSearch = () => {
    setInput({
      ...input,
      page: 1,
    });
    loadData();
  };

  const handleReset = () => {
    setInput({
      page: 1,
      pageSize: 10,
      model: "",
      userName: "",
      startTime: dayjs().subtract(7, 'day').format("YYYY-MM-DD HH:mm:ss"),
      endTime: null,
    });
    setTimeout(() => {
      loadData();
    }, 0);
  };

  const copyToClipboard = (text: string) => {
    navigator.clipboard.writeText(text).then(() => {
      message.success('复制成功');
    }).catch(() => {
      message.error('复制失败');
    });
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
          <Title level={4} style={styles.pageTitle}>请求日志</Title>
          <Space wrap>
            <Button
              icon={<ReloadOutlined />}
              onClick={handleReset}
            >
              重置
            </Button>
            <Button
              type="primary"
              icon={<FilterOutlined />}
              onClick={() => setFilterVisible(!filterVisible)}
            >
              {filterVisible ? "隐藏筛选" : "显示筛选"}
            </Button>
          </Space>
        </div>

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
                    <Text style={styles.filterLabel}>用户名</Text>
                    <Input
                      placeholder="输入用户名"
                      value={input.userName}
                      onChange={(e) => {
                        setInput({
                          ...input,
                          userName: e.target.value,
                        });
                      }}
                      prefix={<SearchOutlined style={{ color: token.colorTextSecondary }} />}
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
          </Space>

          <Text type="secondary">
            共 {total} 条记录
          </Text>
        </div>

        <Card
          style={styles.tableCard}
          bodyStyle={styles.tableCardBody}
          title="请求日志记录"
        >
          <Table
            scroll={{
              x: "max-content",
              y: "calc(100vh - 420px)",
            }}
            loading={loading}
            columns={columns}
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
            size="middle"
          />
        </Card>
        <Modal
          title="请求详情"
          open={detailModalVisible}
          onCancel={() => setDetailModalVisible(false)}
          footer={null}
          height={'95%'}
          width={"100%"}
          style={{ top: 20 }}
        >
          {selectedRecord && (
            <div>
              <Row gutter={[16, 16]} style={{ marginBottom: 16 }}>
                <Col span={8}>
                  <Text strong>请求时间：</Text>
                  <br />
                  <Text>{dayjs(selectedRecord.requestTime).format("YYYY-MM-DD HH:mm:ss")}</Text>
                </Col>
                <Col span={8}>
                  <Text strong>响应时间：</Text>
                  <br />
                  <Text>{dayjs(selectedRecord.responseTime).format("YYYY-MM-DD HH:mm:ss")}</Text>
                </Col>
                <Col span={8}>
                  <Text strong>处理耗时：</Text>
                  <br />
                  <Text>{selectedRecord.durationMs}ms</Text>
                </Col>
                <Col span={8}>
                  <Text strong>路由路径：</Text>
                  <br />
                  <Text>{selectedRecord.routePath}</Text>
                </Col>
                <Col span={8}>
                  <Text strong>HTTP状态码：</Text>
                  <br />
                  <Text>{selectedRecord.httpStatusCode}</Text>
                </Col>
                <Col span={8}>
                  <Text strong>是否成功：</Text>
                  <br />
                  <Text>{selectedRecord.isSuccess ? "是" : "否"}</Text>
                </Col>
              </Row>

              <Row style={{
              }} gutter={[16, 16]}>
                <Col  span={12}>
                  {selectedRecord.requestBody && (
                    <>
                      <Divider />
                      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 }}>
                        <Text strong style={{ fontSize: 16 }}>请求体：</Text>
                        <Button
                          type="text"
                          size="small"
                          icon={<CopyOutlined />}
                          onClick={() => copyToClipboard(selectedRecord.requestBody)}
                        >
                          复制
                        </Button>
                      </div>
                      <div style={{
                        background: token.colorBgElevated,
                        border: `1px solid ${token.colorBorderSecondary}`,
                        borderRadius: 8,
                        maxHeight: 600,
                        overflow: 'auto'
                      }}>
                        <Highlighter
                          language='json'
                        >
                          {JSON.stringify(
                            JSON.parse(selectedRecord.requestBody),
                            null,
                            2
                          )}
                        </Highlighter>
                      </div>
                    </>
                  )}
                </Col>

                <Col span={12}>
                  {selectedRecord.responseBody && (
                    <>
                      <Divider />
                      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 }}>
                        <Text strong style={{ fontSize: 16 }}>响应体：</Text>
                        <Button
                          type="text"
                          size="small"
                          icon={<CopyOutlined />}
                          onClick={() => copyToClipboard(selectedRecord.responseBody)}
                        >
                          复制
                        </Button>
                      </div>
                      <div style={{
                        background: token.colorBgElevated,
                        border: `1px solid ${token.colorBorderSecondary}`,
                        borderRadius: 8,
                        maxHeight: 600,
                        overflow: 'auto'
                      }}>
                        <Highlighter
                          language='json'
                        >
                          {selectedRecord.responseBody}
                        </Highlighter>
                      </div>
                    </>
                  )}
                </Col>
              </Row>

              {selectedRecord.errorMessage && (
                <>
                  <Divider />
                  <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 }}>
                    <Text strong style={{ fontSize: 16 }}>错误信息：</Text>
                    <Button
                      type="text"
                      size="small"
                      icon={<CopyOutlined />}
                      onClick={() => copyToClipboard(selectedRecord.errorMessage)}
                    >
                      复制
                    </Button>
                  </div>
                  <pre style={{
                    background: token.colorErrorBg,
                    border: `1px solid ${token.colorErrorBorder}`,
                    padding: 16,
                    borderRadius: 8,
                    maxHeight: 600,
                    overflow: 'auto',
                    color: token.colorError,
                    fontSize: 13,
                    fontFamily: 'Monaco, Menlo, "Ubuntu Mono", monospace',
                    margin: 0
                  }}>
                    {selectedRecord.errorMessage}
                  </pre>
                </>
              )}
            </div>
          )}
        </Modal>

      </div>
    </ConfigProvider>
  );
}




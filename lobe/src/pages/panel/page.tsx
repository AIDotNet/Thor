import { renderNumber, renderQuota } from '../../utils/render';
import { useEffect, useState } from 'react';
import { GetStatistics } from '../../services/StatisticsService';
import { BarList, DonutChart, FunnelChart, Heatmaps, LineChart, LineChartProps, Tracker } from '@lobehub/charts';
import { useTheme } from 'antd-style';
import { getIconByName } from '../../utils/iconutils';
import { GetServerLoad, GetUserRequest } from '../../services/TrackerService';
import { Flexbox } from 'react-layout-kit';
import { Typography, Card, Row, Col, Space, Statistic, Skeleton, Grid, ConfigProvider } from 'antd';
import { 
  RiseOutlined, 
  FallOutlined, 
  DashboardOutlined, 
  LineChartOutlined, 
  BarChartOutlined, 
  DollarOutlined,
  ApiOutlined,
  ThunderboltOutlined
} from '@ant-design/icons';

const { Title, Text } = Typography;
const { useBreakpoint } = Grid;

export default function PanelPage() {
  const [data, setData] = useState<any>(undefined);
  const [consumeChart, setConsumeChart] = useState<any[]>([]);
  const [requestChart, setRequestChart] = useState<any[]>([]);
  const [tokenChart, setTokenChart] = useState<any[]>([]);
  const [modelsChart, setModelsChart] = useState<any[]>([]);
  const [modelsName, setModelsName] = useState<any[]>([]);
  const [userNewData, setUserNewData] = useState<any[] | null>(null);
  const [rechargeData, setRechargeData] = useState<any[] | null>(null);
  const [trackerData, setTrackerData] = useState<any[]>([]);
  const [userRequest, setUserRequest] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  const theme = useTheme();
  const screens = useBreakpoint();
  const colors = [theme.colorSuccess, theme.colorError];

  function loadTrackerData() {
    GetServerLoad().then((res) => {
      setTrackerData(res.data);
    });
  }

  function loadUserRequest() {
    GetUserRequest().then((res) => {
      setUserRequest(res.data);
    });
  }

  function loadStatistics() {
    const consumeChart: any[] = [];
    const requestChart: any[] = [];
    const tokenChart: any[] = [];
    const modelsChart: any[] = [];
    const modelsName: any[] = [];

    setLoading(true);
    
    GetStatistics()
      .then((res) => {
        const { modelDate, consumes, requests, tokens, models, userNewData, rechargeData } = res.data;

        if (userNewData) {
          setUserNewData(userNewData);
        } else {
          setUserNewData(null);
        }

        if (rechargeData) {
          setRechargeData(rechargeData);
        } else {
          setRechargeData(null);
        }

        modelDate.forEach((item: any, i: number) => {
          const consume = consumes?.find((x: any) => x.dateTime === item);
          const request = requests.find((x: any) => x.dateTime === item);
          const token = tokens.find((x: any) => x.dateTime === item);
          consumeChart.push({
            date: item,
            消费: consume?.value,
          });
          requestChart.push({
            date: item,
            请求数: request?.value,
          });
          tokenChart.push({
            date: item,
            令牌数: token?.value
          });

          const model = {} as any;
          models.forEach((item: any) => {
            model[item.name] = item.data[i];
          });
          model.date = item;
          modelsChart.push(model);
        });

        models.forEach((item: any) => {
          modelsName.push(item.name);
        });

        setConsumeChart(consumeChart);
        setRequestChart(requestChart);
        setTokenChart(tokenChart);
        setModelsChart(modelsChart);
        setModelsName(modelsName);

        res.data?.modelRanking?.forEach((item: any) => {
          item.icon = getIconByName(item.icon).icon;
        });

        setData(res.data);
      })
      .finally(() => {
        setLoading(false);
      });
  }

  useEffect(() => {
    loadStatistics();
    loadTrackerData();
    loadUserRequest();
  }, []);

  const cunsumeValueFormatter: LineChartProps['valueFormatter'] = (number: any) => {
    return `${renderQuota(number, 6)}`;
  };

  const requestValueFormatter: LineChartProps['valueFormatter'] = (number: any) => {
    return `${renderNumber(number)}`;
  };

  const tokenValueFormatter: LineChartProps['valueFormatter'] = (number: any) => {
    return `${renderNumber(number)}`;
  };

  const labelFormatter = (value: any) => {
    return `${value}` + '日';
  };

  const modelsValueFormatter: LineChartProps['valueFormatter'] = (value: any) => {
    return `${renderQuota(value)}`;
  };

  // 统一卡片样式
  const cardStyle = { 
    borderRadius: 8,
    height: '100%'
  };

  return (
    <ConfigProvider
      theme={{
        components: {
          Card: {
            headerBg: 'transparent',
            colorBgContainer: theme.colorBgElevated,
          },
          Statistic: {
            contentFontSize: 24,
          }
        },
      }}
    >
      <div style={{
        overflow: 'auto',
        overflowX: 'hidden',
      }}>
        <Skeleton loading={loading} active>
          <Row gutter={[16, 16]}>
            {/* 顶部数据概览部分 */}
            <Col xs={24} lg={8} xl={6}>
              <Card 
                bordered={false}
                style={cardStyle}
              >
                <Flexbox gap={8}>
                  <Text type="secondary">当前剩余额度</Text>
                  <Flexbox horizontal align="center" justify="space-between">
                    <Title level={2} style={{ margin: 0 }}>
                      {renderQuota(data?.currentResidualCredit, 2)}
                    </Title>
                    <DollarOutlined style={{ fontSize: 24, opacity: 0.5 }} />
                  </Flexbox>
                  <DonutChart 
                    colors={colors}
                    showAnimation={true}
                    style={{
                      height: screens.xs ? 160 : 220,
                      width: '100%',
                      padding: 10,
                    }}
                    valueFormatter={(v) => `${renderQuota(v, 2)}`}
                    data={[
                      {
                        value: data?.currentConsumedCredit,
                        name: '消费额度',
                      }, 
                      {
                        value: data?.currentResidualCredit,
                        name: '剩余额度',
                      }
                    ]} 
                  />
                </Flexbox>
              </Card>
            </Col>

            <Col xs={24} lg={16} xl={18}>
              <Row gutter={[16, 16]}>
                <Col xs={24} md={8}>
                  <Card bordered={false} style={cardStyle}>
                    <Statistic
                      title="最近消费总额"
                      value={renderQuota(data?.consumes?.reduce((a: number, b: any) => a + b.value, 0), 2)}
                      prefix={<RiseOutlined />}
                    />
                    <LineChart
                      categories={['消费']}
                      data={consumeChart}
                      valueFormatter={cunsumeValueFormatter}
                      xAxisLabelFormatter={labelFormatter}
                      index="date"
                      style={{ marginTop: 16, height: screens.xs ? 100 : screens.md ? 80 : 120 }}
                    />
                  </Card>
                </Col>
                <Col xs={24} md={8}>
                  <Card bordered={false} style={cardStyle}>
                    <Statistic
                      title="最近请求总数"
                      value={renderNumber(data?.requests?.reduce((a: number, b: any) => a + b.value, 0))}
                      prefix={<ApiOutlined />}
                    />
                    <LineChart
                      categories={['请求数']}
                      data={requestChart}
                      valueFormatter={requestValueFormatter}
                      xAxisLabelFormatter={labelFormatter}
                      index="date"
                      style={{ marginTop: 16, height: screens.xs ? 100 : screens.md ? 80 : 120 }}
                    />
                  </Card>
                </Col>
                <Col xs={24} md={8}>
                  <Card bordered={false} style={cardStyle}>
                    <Statistic
                      title="最近消耗token总数"
                      value={renderNumber(data?.tokens?.reduce((a: number, b: any) => a + b.value, 0))}
                      prefix={<ThunderboltOutlined />}
                    />
                    <LineChart
                      categories={['令牌数']}
                      data={tokenChart}
                      valueFormatter={tokenValueFormatter}
                      xAxisLabelFormatter={labelFormatter}
                      index="date"
                      style={{ marginTop: 16, height: screens.xs ? 100 : screens.md ? 80 : 120 }}
                    />
                  </Card>
                </Col>
              </Row>

              <Row gutter={[16, 16]} style={{ marginTop: 16 }}>
                <Col xs={24}>
                  <Card 
                    title={<Space><DashboardOutlined /> 服务器负载</Space>}
                    bordered={false}
                    style={cardStyle}
                  >
                    <Tracker 
                      data={trackerData} 
                      style={{ height: screens.xs ? 100 : screens.md ? 120 : 140 }}
                    />
                  </Card>
                </Col>
              </Row>
            </Col>

            {/* 用户活动热力图 */}
            <Col xs={24}>
              <Card 
                title={<Space><LineChartOutlined /> 用户活动热力图</Space>}
                bordered={false}
                style={cardStyle}
              >
                <Heatmaps
                  data={userRequest}
                  fontSize={12}
                  blockMargin={4} 
                  blockSize={10}
                  labels={{
                    legend: {
                      less: '少',
                      more: '多',
                    },
                    months: [
                      '一月',
                      '二月',
                      '三月',
                      '四月',
                      '五月',
                      '六月',
                      '七月',
                      '八月',
                      '九月',
                      '十月',
                      '十一月',
                      '十二月',
                    ],
                    tooltip: '{{count}} 项活动于 {{date}}',
                    totalCount: '{{year}} 年共有 {{count}} 项活动',
                    weekdays: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
                  }}
                />
              </Card>
            </Col>

            {/* 模型数据分析部分 */}
            <Col xs={24} xl={16}>
              <Card 
                title={<Space><LineChartOutlined /> 模型消耗分布（最近七天）</Space>}
                bordered={false}
                style={cardStyle}
              >
                <LineChart
                  style={{
                    height: screens.xs ? 300 : 350,
                  }}
                  categories={modelsName}
                  data={modelsChart}
                  index="date"
                  valueFormatter={modelsValueFormatter}
                />
              </Card>
            </Col>
            <Col xs={24} xl={8}>
              <Card 
                title={<Space><BarChartOutlined /> 模型费用排名（最近七天）</Space>}
                bordered={false}
                style={cardStyle}
              >
                <BarList 
                  style={{
                    height: screens.xs ? 300 : 350,
                  }}
                  data={data?.modelRanking}
                  showAnimation
                  valueFormatter={modelsValueFormatter}
                  sortOrder='descending' 
                />
              </Card>
            </Col>

            {/* 条件性显示的部分 */}
            {userNewData && (
              <Col xs={24} lg={12}>
                <Card 
                  title={<Space><RiseOutlined /> 新用户注册（最近七天）</Space>}
                  bordered={false}
                  style={cardStyle}
                >
                  <FunnelChart
                    style={{
                      height: 350,
                    }}
                    data={userNewData}
                    barGap='20%'
                    enableLegendSlider={false}
                    evolutionGradient={true}
                    gradient={false}
                    showArrow={true}
                    showGridLines={true}
                    showLegend={true}
                    showTooltip={true}
                    showXAxis={true}
                    showYAxis={true}
                    variant='base'
                    xAxisLabel=''
                    yAxisAlign='left'
                    yAxisLabel=''
                  />
                </Card>
              </Col>
            )}
            
            {rechargeData && (
              <Col xs={24} lg={userNewData ? 12 : 24}>
                <Card 
                  title={<Space><FallOutlined /> 最近充值数据</Space>}
                  bordered={false}
                  style={cardStyle}
                >
                  <FunnelChart
                    style={{
                      height: 350,
                    }}
                    data={rechargeData}
                    barGap='20%'
                    enableLegendSlider={false}
                    evolutionGradient={true}
                    gradient={false}
                    showArrow={true}
                    showGridLines={true}
                    showLegend={true}
                    valueFormatter={modelsValueFormatter}
                    showTooltip={true}
                    showXAxis={true}
                    showYAxis={true}
                    variant='base'
                    xAxisLabel=''
                    yAxisAlign='left'
                    yAxisLabel=''
                  />
                </Card>
              </Col>
            )}
          </Row>
        </Skeleton>
      </div>
    </ConfigProvider>
  );
}
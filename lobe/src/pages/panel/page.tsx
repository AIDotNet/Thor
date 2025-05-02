import { renderNumber, renderQuota } from '../../utils/render';
import { useEffect, useState, useRef } from 'react';
import { GetStatistics } from '../../services/StatisticsService';
import * as echarts from 'echarts';
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
  DollarOutlined,
  ApiOutlined,
  ThunderboltOutlined
} from '@ant-design/icons';

const { Title, Text } = Typography;
const { useBreakpoint } = Grid;

// 定义LineChart接口以兼容原有代码
interface LineChartProps {
  valueFormatter?: (value: any) => string;
}

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
  
  // 为echarts图表添加DOM引用
  const donutChartRef = useRef<HTMLDivElement>(null);
  const consumeChartRef = useRef<HTMLDivElement>(null);
  const requestChartRef = useRef<HTMLDivElement>(null);
  const tokenChartRef = useRef<HTMLDivElement>(null);
  const trackerChartRef = useRef<HTMLDivElement>(null);
  const heatmapChartRef = useRef<HTMLDivElement>(null);
  const modelsChartRef = useRef<HTMLDivElement>(null);
  const userNewDataChartRef = useRef<HTMLDivElement>(null);
  const rechargeDataChartRef = useRef<HTMLDivElement>(null);

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

  // 初始化和渲染 echarts 图表
  useEffect(() => {
    // 仅在数据加载完成且组件挂载时渲染图表
    if (loading || !data) return;

    // 渲染环形图 (Donut Chart)
    if (donutChartRef.current) {
      const donutChart = echarts.init(donutChartRef.current);
      donutChart.setOption({
        tooltip: {
          trigger: 'item',
          formatter: (params: any) => {
            return `${params.name}: ${renderQuota(params.value, 2)} (${params.percent}%)`;
          }
        },
        legend: {
          bottom: '0%',
          left: 'center'
        },
        series: [
          {
            name: '额度分布',
            type: 'pie',
            radius: ['40%', '70%'],
            avoidLabelOverlap: false,
            itemStyle: {
              borderRadius: 10,
              borderColor: '#fff',
              borderWidth: 2
            },
            label: {
              show: false,
              position: 'center'
            },
            emphasis: {
              label: {
                show: true,
                fontSize: 20,
                fontWeight: 'bold'
              }
            },
            labelLine: {
              show: false
            },
            data: [
              { value: data.currentConsumedCredit, name: '消费额度', itemStyle: { color: colors[1] } },
              { value: data.currentResidualCredit, name: '剩余额度', itemStyle: { color: colors[0] } },
            ],
            animationType: 'scale',
            animationEasing: 'elasticOut',
            animationDelay: function () {
              return Math.random() * 200;
            }
          }
        ]
      });
      
      // 响应式处理
      const resizeDonut = () => {
        donutChart.resize();
      };
      
      window.addEventListener('resize', resizeDonut);
      
      return () => {
        window.removeEventListener('resize', resizeDonut);
        donutChart.dispose();
      };
    }
  }, [loading, data, colors]);

  // 渲染折线图
  const renderLineChart = (
    ref: React.RefObject<HTMLDivElement>, 
    data: any[], 
    category: string, 
    valueFormatter: (value: any) => string
  ) => {
    if (!ref.current || data.length === 0) return;
    
    const chart = echarts.init(ref.current);
    
    const xAxisData = data.map(item => item.date);
    const seriesData = data.map(item => item[category]);
    
    chart.setOption({
      tooltip: {
        trigger: 'axis',
        formatter: (params: any) => {
          const value = params[0].value;
          return `${params[0].name}日: ${valueFormatter(value)}`;
        }
      },
      xAxis: {
        type: 'category',
        data: xAxisData,
        axisLabel: {
          formatter: labelFormatter
        }
      },
      yAxis: {
        type: 'value',
        axisLabel: {
          formatter: valueFormatter
        }
      },
      grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
      },
      series: [
        {
          name: category,
          type: 'line',
          data: seriesData,
          smooth: true,
          showSymbol: false,
          areaStyle: {
            opacity: 0.2
          },
          itemStyle: {
            color: theme.colorPrimary
          }
        }
      ]
    });
    
    return chart;
  };

  // 渲染服务器负载图
  const renderTrackerChart = () => {
    if (!trackerChartRef.current || !trackerData.length) return;
    
    const chart = echarts.init(trackerChartRef.current);
    
    // 提取x轴数据和series数据
    const xAxisData = trackerData.map(item => item.time);
    const cpuData = trackerData.map(item => item.cpu);
    const memoryData = trackerData.map(item => item.memory);
    
    chart.setOption({
      tooltip: {
        trigger: 'axis',
        axisPointer: {
          type: 'cross',
          label: {
            backgroundColor: '#6a7985'
          }
        }
      },
      legend: {
        data: ['CPU', '内存']
      },
      grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
      },
      xAxis: {
        type: 'category',
        boundaryGap: false,
        data: xAxisData
      },
      yAxis: {
        type: 'value',
        max: 100,
        axisLabel: {
          formatter: '{value}%'
        }
      },
      series: [
        {
          name: 'CPU',
          type: 'line',
          stack: '总量',
          areaStyle: { opacity: 0.3 },
          emphasis: {
            focus: 'series'
          },
          data: cpuData,
          itemStyle: { color: '#1890ff' }
        },
        {
          name: '内存',
          type: 'line',
          stack: '总量',
          areaStyle: { opacity: 0.3 },
          emphasis: {
            focus: 'series'
          },
          data: memoryData,
          itemStyle: { color: '#52c41a' }
        }
      ]
    });
    
    return chart;
  };

  // 渲染热力图
  const renderHeatmap = () => {
    if (!heatmapChartRef.current || !userRequest.length) return;
    
    const chart = echarts.init(heatmapChartRef.current);
    
    // 热力图数据处理
    // 假设userRequest数据格式为 [{date: '2023-01-01', count: 10}, ...]
    const heatmapData = userRequest.map(item => [item.date, item.count]);
    const today = new Date();
    const startDate = new Date();
    startDate.setFullYear(today.getFullYear() - 1);
    
    const calendarData = {
      range: [startDate.toISOString().split('T')[0], today.toISOString().split('T')[0]],
      cellSize: [10, 10],
      splitLine: {
        show: false
      },
      itemStyle: {
        borderWidth: 2,
        borderColor: '#fff'
      },
      monthLabel: {
        formatter: function(param: {year: number, month: number}) {
          const monthNames = ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'];
          return monthNames[param.month - 1];
        }
      },
      dayLabel: {
        firstDay: 1,
        nameMap: ['日', '一', '二', '三', '四', '五', '六']
      },
      yearLabel: { show: false }
    };
    
    chart.setOption({
      tooltip: {
        formatter: function(params: any) {
          return `${params.value[0]}: ${params.value[1]} 项活动`;
        }
      },
      visualMap: {
        min: 0,
        max: 10,
        calculable: true,
        orient: 'horizontal',
        left: 'center',
        bottom: 20,
        inRange: {
          color: ['#ebedf0', '#c6e48b', '#7bc96f', '#239a3b', '#196127']
        },
        text: ['多', '少']
      },
      calendar: calendarData,
      series: {
        type: 'heatmap',
        coordinateSystem: 'calendar',
        data: heatmapData
      }
    });
    
    return chart;
  };

  // 渲染模型折线图
  const renderModelsLineChart = () => {
    if (!modelsChartRef.current || !modelsChart.length) return;
    
    const chart = echarts.init(modelsChartRef.current);
    
    // 提取x轴数据
    const xAxisData = modelsChart.map(item => item.date);
    
    // 准备各个模型的数据
    const series = modelsName.map(name => ({
      name: name,
      type: 'line',
      data: modelsChart.map(item => item[name]),
      smooth: true,
      showSymbol: false
    }));
    
    chart.setOption({
      tooltip: {
        trigger: 'axis',
        formatter: function(params: any) {
          let result = `${params[0].axisValue}日<br/>`;
          params.forEach((param: any) => {
            result += `${param.marker}${param.seriesName}: ${modelsValueFormatter(param.value)}<br/>`;
          });
          return result;
        }
      },
      legend: {
        data: modelsName,
        type: 'scroll',
        bottom: 0
      },
      grid: {
        left: '3%',
        right: '4%',
        bottom: '10%',
        containLabel: true
      },
      xAxis: {
        type: 'category',
        boundaryGap: false,
        data: xAxisData,
        axisLabel: {
          formatter: labelFormatter
        }
      },
      yAxis: {
        type: 'value',
        axisLabel: {
          formatter: (value: any) => modelsValueFormatter(value)
        }
      },
      series: series
    });
    
    return chart;
  };

  // 渲染漏斗图
  const renderFunnelChart = (
    ref: React.RefObject<HTMLDivElement>, 
    data: any[] | null, 
    valueFormatter?: (value: any) => string
  ) => {
    if (!ref.current || !data || data.length === 0) return;
    
    const chart = echarts.init(ref.current);
    
    const seriesData = data.map(item => ({
      value: item.value,
      name: item.name
    }));
    
    chart.setOption({
      tooltip: {
        trigger: 'item',
        formatter: function(params: any) {
          if (valueFormatter) {
            return `${params.name}: ${valueFormatter(params.value)}`;
          }
          return `${params.name}: ${params.value}`;
        }
      },
      legend: {
        data: data.map(item => item.name),
        bottom: 0
      },
      series: [
        {
          name: '数据',
          type: 'funnel',
          left: '10%',
          top: 60,
          bottom: 60,
          width: '80%',
          min: 0,
          max: Math.max(...data.map(item => item.value)) * 1.2,
          minSize: '0%',
          maxSize: '100%',
          sort: 'descending',
          gap: 2,
          label: {
            show: true,
            position: 'inside'
          },
          emphasis: {
            label: {
              fontSize: 20
            }
          },
          data: seriesData
        }
      ]
    });
    
    return chart;
  };

  // 当数据变化或组件挂载时重新渲染所有图表
  useEffect(() => {
    if (loading || !data) return;
    
    // 消费折线图
    const consumeLineChart = renderLineChart(consumeChartRef, consumeChart, '消费', cunsumeValueFormatter);
    
    // 请求折线图
    const requestLineChart = renderLineChart(requestChartRef, requestChart, '请求数', requestValueFormatter);
    
    // Token折线图
    const tokenLineChart = renderLineChart(tokenChartRef, tokenChart, '令牌数', tokenValueFormatter);
    
    // 服务器负载图
    const trackerChart = renderTrackerChart();
    
    // 热力图
    const heatmapChart = renderHeatmap();
    
    // 模型使用情况折线图
    const modelsChart = renderModelsLineChart();
    
    // 新用户漏斗图
    const userNewChart = renderFunnelChart(userNewDataChartRef, userNewData);
    
    // 充值数据漏斗图
    const rechargeChart = renderFunnelChart(rechargeDataChartRef, rechargeData, modelsValueFormatter);
    
    // 为了响应窗口大小变化，添加resize事件监听
    const resizeHandler = () => {
      consumeLineChart?.resize();
      requestLineChart?.resize();
      tokenLineChart?.resize();
      trackerChart?.resize();
      heatmapChart?.resize();
      modelsChart?.resize();
      userNewChart?.resize();
      rechargeChart?.resize();
    };
    
    window.addEventListener('resize', resizeHandler);
    
    return () => {
      window.removeEventListener('resize', resizeHandler);
      
      // 释放图表实例
      consumeLineChart?.dispose();
      requestLineChart?.dispose();
      tokenLineChart?.dispose();
      trackerChart?.dispose();
      heatmapChart?.dispose();
      modelsChart?.dispose();
      userNewChart?.dispose();
      rechargeChart?.dispose();
    };
  }, [loading, data, consumeChart, requestChart, tokenChart, modelsChart, userNewData, rechargeData, trackerData, userRequest]);

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
                  <div ref={donutChartRef} style={{
                    height: screens.xs ? 160 : 220,
                    width: '100%',
                    padding: 10,
                  }} />
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
                    <div ref={consumeChartRef} style={{ marginTop: 16, height: screens.xs ? 100 : screens.md ? 80 : 120 }} />
                  </Card>
                </Col>
                <Col xs={24} md={8}>
                  <Card bordered={false} style={cardStyle}>
                    <Statistic
                      title="最近请求总数"
                      value={renderNumber(data?.requests?.reduce((a: number, b: any) => a + b.value, 0))}
                      prefix={<ApiOutlined />}
                    />
                    <div ref={requestChartRef} style={{ marginTop: 16, height: screens.xs ? 100 : screens.md ? 80 : 120 }} />
                  </Card>
                </Col>
                <Col xs={24} md={8}>
                  <Card bordered={false} style={cardStyle}>
                    <Statistic
                      title="最近消耗token总数"
                      value={renderNumber(data?.tokens?.reduce((a: number, b: any) => a + b.value, 0))}
                      prefix={<ThunderboltOutlined />}
                    />
                    <div ref={tokenChartRef} style={{ marginTop: 16, height: screens.xs ? 100 : screens.md ? 80 : 120 }} />
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
                    <div ref={trackerChartRef} style={{ height: screens.xs ? 100 : screens.md ? 120 : 140 }} />
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
                <div ref={heatmapChartRef} style={{ height: screens.xs ? 300 : 350 }} />
              </Card>
            </Col>

            {/* 模型数据分析部分 */}
            <Col xs={24} xl={16}>
              <Card 
                title={<Space><LineChartOutlined /> 模型消耗分布（最近七天）</Space>}
                bordered={false}
                style={cardStyle}
              >
                <div ref={modelsChartRef} style={{ height: screens.xs ? 300 : 350 }} />
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
                  <div ref={userNewDataChartRef} style={{ height: 350 }} />
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
                  <div ref={rechargeDataChartRef} style={{ height: 350 }} />
                </Card>
              </Col>
            )}
          </Row>
        </Skeleton>
      </div>
    </ConfigProvider>
  );
}

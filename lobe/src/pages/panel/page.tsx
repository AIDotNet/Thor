import { renderNumber, renderQuota } from '../../utils/render';
import { useEffect, useState, useRef } from 'react';
import { GetStatistics } from '../../services/StatisticsService';
import * as echarts from 'echarts';
import { useTheme } from 'antd-style';
import { getIconByName } from '../../utils/iconutils';
import { GetUserRequest } from '../../services/TrackerService';
import { Flexbox } from 'react-layout-kit';
import { Typography, Card, Row, Col, Space, Statistic, Skeleton, Grid, ConfigProvider } from 'antd';
import {
  RiseOutlined,
  LineChartOutlined,
  DollarOutlined,
  ApiOutlined,
  ThunderboltOutlined,
  UsergroupAddOutlined,
  TransactionOutlined
} from '@ant-design/icons';
import { useTranslation } from 'react-i18next';

const { Title, Text } = Typography;
const { useBreakpoint } = Grid;

// 定义LineChart接口以兼容原有代码
interface LineChartProps {
  valueFormatter?: (value: any) => string;
}

export default function PanelPage() {
  const { t } = useTranslation();
  const [data, setData] = useState<any>(undefined);
  const [consumeChart, setConsumeChart] = useState<any[]>([]);
  const [requestChart, setRequestChart] = useState<any[]>([]);
  const [tokenChart, setTokenChart] = useState<any[]>([]);
  const [modelsChart, setModelsChart] = useState<any[]>([]);
  const [modelsName, setModelsName] = useState<any[]>([]);
  const [userNewData, setUserNewData] = useState<any[] | null>(null);
  const [rechargeData, setRechargeData] = useState<any[] | null>(null);
  const [userRequest, setUserRequest] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  // 为echarts图表添加DOM引用
  const donutChartRef = useRef<HTMLDivElement>(null);
  const consumeChartRef = useRef<HTMLDivElement>(null);
  const requestChartRef = useRef<HTMLDivElement>(null);
  const tokenChartRef = useRef<HTMLDivElement>(null);
  const modelsChartRef = useRef<HTMLDivElement>(null);
  const userNewDataChartRef = useRef<HTMLDivElement>(null);
  const rechargeDataChartRef = useRef<HTMLDivElement>(null);

  const theme = useTheme();
  const screens = useBreakpoint();
  const colors = [theme.colorSuccess, theme.colorError];

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
            consumption: consume?.value,
          });
          requestChart.push({
            date: item,
            requests: request?.value,
          });
          tokenChart.push({
            date: item,
            tokens: token?.value
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
    return `${value}` + t('panel.chartLabels.day');
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
            name: t('panel.statistics.quotaDistribution'),
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
              { value: data.currentConsumedCredit, name: t('panel.statistics.consumedQuota'), itemStyle: { color: colors[1] } },
              { value: data.currentResidualCredit, name: t('panel.statistics.remainingQuota'), itemStyle: { color: colors[0] } },
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
    valueFormatter: (value: any) => string,
    translatedLabel?: string
  ) => {
    if (!ref.current || data.length === 0) return;

    const chart = echarts.init(ref.current);

    const xAxisData = data.map(item => item.date);
    const seriesData = data.map(item => item[category]);
    const displayLabel = translatedLabel || category;

    chart.setOption({
      tooltip: {
          trigger: 'axis',
          confine: true,
          backgroundColor: theme.colorBgElevated,
          borderColor: theme.colorBorder,
          textStyle: {
              color: theme.colorText
          },
          formatter: (params: any) => {
              const dateStr = params[0].value;
              let result = `<div style="font-weight:bold;margin-bottom:4px">${dateStr}${t('panel.chartLabels.day')}</div>`;

              // 将params按value排序
              params.sort((a: any, b: any) => b.value - a.value);

              params.forEach((param: any) => {
                if (param.value > 0) {
                  result += `<div style="display:flex;justify-content:space-between;margin:3px 0">
                    <span>${param.marker}${param.seriesName}</span>
                    <span style="margin-left:15px;font-weight:bold">${valueFormatter(param.value)}</span>
                  </div>`;
                }
              });

              return result;
          },
          axisPointer: {
              type: 'shadow',
              lineStyle: {
                  color: 'transparent'
              },
              crossStyle: {
                  color: 'transparent'
              }
          }
      },
      xAxis: {
        type: 'category',
        data: xAxisData,
        axisLabel: {
          formatter: labelFormatter,
          color: theme.colorTextSecondary,
          fontSize: 12
        },
        axisLine: {
          lineStyle: {
            color: theme.colorBorderSecondary
          }
        },
        axisTick: {
          show: false
        }
      },
      yAxis: {
        type: 'value',
        axisLabel: {
          formatter: valueFormatter,
          color: theme.colorTextSecondary,
          fontSize: 12
        },
        splitLine: {
          lineStyle: {
            color: theme.colorSplit,
            type: 'dashed',
            opacity: 0.5
          }
        },
        axisLine: {
          show: false
        },
        axisTick: {
          show: false
        }
      },
      grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        top: '8%',
        containLabel: true
      },
      series: [
        {
          name: displayLabel,
          type: 'line',
          data: seriesData,
          smooth: true,
          symbol: 'circle',
          symbolSize: 6,
          emphasis: {
            itemStyle: {
              shadowBlur: 10,
              shadowColor: theme.colorPrimary + '40'
            },
            scale: true
          },
          showSymbol: data.length <= 10,
          lineStyle: {
            width: 3,
            color: theme.colorPrimary,
            shadowColor: theme.colorPrimary + '20',
            shadowBlur: 10,
            cap: 'round'
          },
          areaStyle: {
            color: {
              type: 'linear',
              x: 0,
              y: 0,
              x2: 0,
              y2: 1,
              colorStops: [{
                offset: 0,
                color: theme.colorPrimary + '30' // 透明度20%
              }, {
                offset: 1,
                color: theme.colorPrimary + '05' // 透明度3%
              }]
            },
            origin: 'start'
          },
          itemStyle: {
            color: theme.colorPrimary,
            borderWidth: 2,
            borderColor: theme.colorBgContainer
          }
        }
      ],
      animation: true,
      animationDuration: 1000,
      animationEasing: 'cubicOut'
    });

    return chart;
  };


  // 渲染模型折线图
  const renderModelsLineChart = () => {
    if (!modelsChartRef.current || !modelsChart.length) return;

    const chart = echarts.init(modelsChartRef.current);

    // 提取x轴数据
    const xAxisData = modelsChart.map(item => item.date);

    // 准备各个模型的数据，只保留大于0的数值
    const series = modelsName.map(name => ({
      name: name,
      type: 'bar',
      data: modelsChart.map(item => item[name] > 0 ? item[name] : '-'),
      stack: 'total',
      emphasis: {
        focus: 'series',
        itemStyle: {
          shadowBlur: 15,
          shadowOffsetX: 0,
          shadowOffsetY: 5,
          shadowColor: 'rgba(0,0,0,0.3)'
        }
      },
      barMaxWidth: '50%',
      // 只显示非0数据
      label: {
        show: false
      }
    }));

    chart.setOption({
      tooltip: {
          trigger: 'axis',
          confine: true,
          backgroundColor: theme.colorBgElevated,
          borderColor: theme.colorBorder,
          textStyle: {
              color: theme.colorText
          },
          formatter: (params: any) => {
              const dateStr = params[0].axisValue;
              let result = `<div style="font-weight:bold;margin-bottom:4px">${dateStr}${t('panel.chartLabels.day')}</div>`;
              params.forEach((param: any) => {
                if (param.value > 0) {
                  result += `<div style="display:flex;justify-content:space-between;margin:3px 0">
                    <span>${param.marker}${param.seriesName}</span>
                    <span style="margin-left:15px;font-weight:bold">${modelsValueFormatter(param.value)}</span>
                  </div>`;
                }
              });

              return result;
          },
          axisPointer: {
              type: 'shadow',
              lineStyle: {
                  color: 'transparent'
              },
              crossStyle: {
                  color: 'transparent'
              }
          }
      },
      legend: {
        data: modelsName,
        type: 'scroll',
        bottom: 0,
        icon: 'rect',
        itemWidth: 12,
        itemHeight: 12,
        textStyle: {
          fontSize: 12
        }
      },
      grid: {
        left: '3%',
        right: '4%',
        bottom: '10%',
        top: '5%',
        containLabel: true
      },
      xAxis: {
        type: 'category',
        data: xAxisData,
        axisLabel: {
          formatter: labelFormatter,
          fontSize: 12
        },
        axisLine: {
          lineStyle: {
            color: theme.colorBorderSecondary
          }
        }
      },
      yAxis: {
        type: 'value',
        axisLabel: {
          formatter: (value: any) => modelsValueFormatter(value),
          fontSize: 12
        },
        splitLine: {
          lineStyle: {
            type: 'dashed',
            color: theme.colorSplit
          }
        }
      },
      series: series
    });

    return chart;
  };

  // 渲染漏斗图 - 替换为柱状图
  const renderBarChart = (
    ref: React.RefObject<HTMLDivElement>,
    data: any[] | null,
    valueFormatter?: (value: any) => string
  ) => {
    if (!ref.current || !data || data.length === 0) return;

    const chart = echarts.init(ref.current);

    // 确保数据字段名正确，并处理可能的对象格式
    const categories = data.map(item => typeof item.name === 'object' ? JSON.stringify(item.name) : item.name);
    const values = data.map(item => item.value);

    chart.setOption({
      tooltip: {
        trigger: 'axis',
        axisPointer: {
          type: 'shadow'
        },
        formatter: function (params: any) {
          const value = params[0].value;
          if (valueFormatter) {
            return `${params[0].name}: ${valueFormatter(value)}`;
          }
          return `${params[0].name}: ${value}`;
        }
      },
      grid: {
        left: '5%',
        right: '5%',
        bottom: '15%',
        top: '8%',
        containLabel: true
      },
      xAxis: {
        type: 'category',
        data: categories,
        axisLabel: {
          interval: 0,
          rotate: data.length > 5 ? 30 : 0,
          fontSize: 12,
          color: theme.colorTextSecondary,
          formatter: function (value: any) {
            // 如果是对象字符串，尝试提取更有意义的值
            if (value.startsWith('{') && value.endsWith('}')) {
              try {
                return t('panel.chartLabels.userCount');
              } catch (e) {
                return value;
              }
            }
            return value;
          }
        },
        axisTick: {
          alignWithLabel: true
        },
        axisLine: {
          lineStyle: {
            color: theme.colorBorderSecondary
          }
        }
      },
      yAxis: {
        type: 'value',
        name: t('panel.chartLabels.userCount'),
        nameTextStyle: {
          color: theme.colorTextSecondary,
          fontSize: 12
        },
        minInterval: 1, // 强制Y轴以整数显示
        axisLabel: {
          formatter: (value: any) => value, // 显示整数
          color: theme.colorTextSecondary,
          fontSize: 12
        },
        splitLine: {
          lineStyle: {
            color: theme.colorSplit,
            type: 'dashed'
          }
        }
      },
      series: [
        {
          name: t('panel.chartLabels.userCount'),
          type: 'bar',
          data: values,
          itemStyle: {
            color: {
              type: 'linear',
              x: 0,
              y: 0,
              x2: 0,
              y2: 1,
              colorStops: [{
                offset: 0,
                color: theme.colorPrimary
              }, {
                offset: 1,
                color: theme.colorPrimaryHover
              }]
            },
            borderRadius: [4, 4, 0, 0]
          },
          emphasis: {
            itemStyle: {
              color: {
                type: 'linear',
                x: 0,
                y: 0,
                x2: 0,
                y2: 1,
                colorStops: [{
                  offset: 0,
                  color: theme.colorPrimaryActive
                }, {
                  offset: 1,
                  color: theme.colorPrimary
                }]
              }
            }
          },
          label: {
            show: true,
            position: 'top',
            formatter: '{c}',
            fontSize: 12,
            color: theme.colorTextSecondary
          },
          barWidth: '60%',
          animationDelay: (idx: number) => idx * 100
        }
      ],
      animationEasing: 'elasticOut',
      animationDelayUpdate: (idx: number) => idx * 5
    });

    return chart;
  };

  // 渲染饼图
  const renderPieChart = (
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
        formatter: function (params: any) {
          if (valueFormatter) {
            return `${params.name}: ${valueFormatter(params.value)} (${params.percent}%)`;
          }
          return `${params.name}: ${params.value} (${params.percent}%)`;
        }
      },
      legend: {
        type: 'scroll',
        orient: 'horizontal',
        bottom: 10,
        data: data.map(item => item.name)
      },
      series: [
        {
          name: t('panel.chartLabels.value'),
          type: 'pie',
          radius: ['40%', '70%'],
          center: ['50%', '45%'],
          avoidLabelOverlap: true,
          itemStyle: {
            borderRadius: 6,
            borderColor: theme.colorBgContainer,
            borderWidth: 2
          },
          label: {
            show: true,
            formatter: '{b}: {d}%'
          },
          emphasis: {
            label: {
              show: true,
              fontWeight: 'bold',
              fontSize: 16
            },
            itemStyle: {
              shadowBlur: 10,
              shadowOffsetX: 0,
              shadowColor: 'rgba(0, 0, 0, 0.3)'
            }
          },
          data: seriesData
        }
      ],
      animationType: 'scale',
      animationEasing: 'elasticOut'
    });

    return chart;
  };

  // 当数据变化或组件挂载时重新渲染所有图表
  useEffect(() => {
    if (loading || !data) return;

    // 消费折线图
    const consumeLineChart = renderLineChart(consumeChartRef, consumeChart, 'consumption', cunsumeValueFormatter, t('panel.chartLabels.consumption'));

    // 请求折线图
    const requestLineChart = renderLineChart(requestChartRef, requestChart, 'requests', requestValueFormatter, t('panel.chartLabels.requests'));

    // Token折线图
    const tokenLineChart = renderLineChart(tokenChartRef, tokenChart, 'tokens', tokenValueFormatter, t('panel.chartLabels.tokens'));
    // 模型使用情况折线图
    const modelsChart = renderModelsLineChart();

    // 新用户柱状图
    const userNewChart = renderBarChart(userNewDataChartRef, userNewData);

    // 充值数据饼图
    const rechargeChart = renderPieChart(rechargeDataChartRef, rechargeData, modelsValueFormatter);

    // 为了响应窗口大小变化，添加resize事件监听
    const resizeHandler = () => {
      consumeLineChart?.resize();
      requestLineChart?.resize();
      tokenLineChart?.resize();
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
      modelsChart?.dispose();
      userNewChart?.dispose();
      rechargeChart?.dispose();
    };
  }, [loading, data, consumeChart, requestChart, tokenChart, modelsChart, userNewData, rechargeData, userRequest]);

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
                  <Text type="secondary">{t('panel.statistics.currentResidualCredit')}</Text>
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
                      title={t('panel.statistics.recentConsumption')}
                      value={renderQuota(data?.consumes?.reduce((a: number, b: any) => a + b.value, 0), 2)}
                      prefix={<RiseOutlined />}
                    />
                    <div ref={consumeChartRef} style={{ height: 200 }} />
                  </Card>
                </Col>
                <Col xs={24} md={8}>
                  <Card bordered={false} style={cardStyle}>
                    <Statistic
                      title={t('panel.statistics.recentRequests')}
                      value={renderNumber(data?.requests?.reduce((a: number, b: any) => a + b.value, 0))}
                      prefix={<ApiOutlined />}
                    />
                    <div ref={requestChartRef} style={{ height: 200 }} />
                  </Card>
                </Col>
                <Col xs={24} md={8}>
                  <Card bordered={false} style={cardStyle}>
                    <Statistic
                      title={t('panel.statistics.recentTokens')}
                      value={renderNumber(data?.tokens?.reduce((a: number, b: any) => a + b.value, 0))}
                      prefix={<ThunderboltOutlined />}
                    />
                    <div ref={tokenChartRef} style={{ height: 200 }} />
                  </Card>
                </Col>
              </Row>

              <Row gutter={[16, 16]} style={{ marginTop: 16 }}>
                <Col xs={24}>

                </Col>
              </Row>
            </Col>
            {/* 模型数据分析部分 */}
            <Col xs={24} xl={24}>
              <Card
                title={<Space><LineChartOutlined /> {t('panel.charts.modelDistribution')}</Space>}
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
                  title={<Space><UsergroupAddOutlined /> {t('panel.charts.newUserRegistration')}</Space>}
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
                  title={<Space><TransactionOutlined /> {t('panel.charts.recentRechargeData')}</Space>}
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

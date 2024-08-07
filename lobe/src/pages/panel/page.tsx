import * as echarts from 'echarts';
import { renderNumber, renderQuota } from '../../utils/render';
import { useEffect, useState } from 'react';
import { GetStatistics } from '../../services/StatisticsService';
import styled from "styled-components"
import { Row, Col, Card, Divider, Tabs,Tag } from 'antd'

const Dashboard = styled.div` 
  width: 100%;
  overflow: auto;
`;

export default function PanelPage() {
  const [data, setData] = useState<any>({});
  const [currentDayConsume, setCurrentDayConsume] = useState<number>(0);
  const [currentDayRequest, setCurrentDayRequest] = useState<number>(0);
  const [currentDayToken, setCurrentDayToken] = useState<number>(0);

  function loadStatistics() {
    GetStatistics()
      .then((res) => {
        setData(res.data);
      });
  }

  useEffect(() => {
    loadStatistics();
  }, []);

  useEffect(() => {
    const consumesChart = echarts.init(document.getElementById('consumes') as HTMLDivElement);
    var consumesOption = {
      xAxis: {
        type: 'category',
        data: data?.consumes?.map((item: any) => item.dateTime),
        axisTick: {
          show: false
        },
        axisLabel: {
          show: false
        },
        splitLine: {
          show: false
        }
      },
      yAxis: {
        type: 'value',
        axisTick: {
          show: false
        },
        axisLabel: {
          show: false
        },
        splitLine: {
          show: false
        },
      },
      tooltip: {
        show: true,
        trigger: 'axis',
        axisPointer: {
          type: "shadow"
        },
        formatter: function (params: any) {
          return renderQuota(params[0].value, 6); // Format the value to two decimal places
        }
      },
      series: [
        {
          data: data?.consumes?.map((item: any) => ({
            value: item.value,
          })),
          type: 'line',
          smooth: true,
          showSymbol: false
        }
      ],
      grid: {
        left: 5,
        top: 5,
        right: 5,
        bottom: 5
      },
    };

    // 获取data.data.consumes中的最后一个元素
    const lastConsume = data?.consumes?.slice(-1)[0];

    setCurrentDayConsume(lastConsume?.value);

    consumesOption && consumesChart.setOption(consumesOption);


    const requestsChart = echarts.init(document.getElementById('requests') as HTMLDivElement);
    var requestOption = {
      xAxis: {
        type: 'category',
        data: data?.requests?.map((item: any) => item.dateTime),
        axisTick: {
          show: false
        },
        axisLabel: {
          show: false
        },
        splitLine: {
          show: false
        }
      },
      yAxis: {
        type: 'value',
        axisTick: {
          show: false
        },
        axisLabel: {
          show: false
        },
        splitLine: {
          show: false
        }
      },
      tooltip: {
        show: true,
        trigger: 'axis',
        axisPointer: {
          type: "shadow"
        },
      },
      series: [
        {
          data: data?.requests?.map((item: any) => ({
            value: item.value,
          })),
          type: 'line',
          smooth: true,
          showSymbol: false
        }
      ],
      grid: {
        left: 5,
        top: 5,
        right: 5,
        bottom: 5
      },
    };

    // 获取data.data.consumes中的最后一个元素
    const lastRequest = data?.requests?.slice(-1)[0];
    setCurrentDayRequest(lastRequest?.value);

    requestOption && requestsChart.setOption(requestOption);


    const tokensChart = echarts.init(document.getElementById('tokens') as HTMLDivElement);
    var tokensOption = {
      xAxis: {
        type: 'category',
        data: data?.tokens?.map((item: any) => item.dateTime),
        axisTick: {
          show: false
        },
        axisLabel: {
          show: false
        },
        splitLine: {
          show: false
        }
      },
      yAxis: {
        type: 'value',
        axisTick: {
          show: false
        },
        axisLabel: {
          show: false
        },
        splitLine: {
          show: false
        }
      },
      tooltip: {
        show: true,
        trigger: 'axis',
        axisPointer: {
          type: "shadow"
        },
      },
      series: [
        {
          data: data?.tokens?.map((item: any) => ({
            value: item.value,
          })),
          type: 'line',
          smooth: true,
          barMinHeight: 5,
          showSymbol: false
        }
      ],
      grid: {
        left: 5,
        top: 5,
        right: 5,
        bottom: 5
      },
    };

    const lastTokens = data?.tokens?.slice(-1)[0];
    setCurrentDayToken(lastTokens?.value);

    tokensOption && tokensChart.setOption(tokensOption);

    const modelConsumptionDistributionChart = echarts.init(document.getElementById('model-consumption-distribution') as HTMLDivElement);
    var modelConsumptionDistributionOption = {
      tooltip: {
        show: true,
        trigger: 'axis',
        axisPointer: {
          type: "shadow"
        },
        formatter: function (params: any) {
          let res = `${params[0].name}<br/>`;

          const total = params.map((x: any) => x.value).reduce((a: number, b: number) => a + b, 0);

          res += `${params[0].marker} 总计：${renderQuota(total, 6)}<br/>`;

          for (let i = 0; i < params.length; i++) {
            if (params[i].value === 0) {
              continue;
            }
            res += `${params[i].marker} ${params[i].seriesName}：${renderQuota(params[i].value, 6)}<br/>`;
          }
          
          return res;
        },
      },
      legend: {
        show: true,
        bottom: 0,
        orient: 'horizontal',
        data: data?.models?.map((item: any) => item.name)
      },
      grid: {
        bottom: '10%',
        containLabel: true
      },
      xAxis: [
        {
          type: 'category',
          data: data?.modelDate,
          axisTick: {
            alignWithLabel: true
          }
        }
      ],
      yAxis: [
        {
          type: 'value',
          axisLabel: {
            formatter: function (value: number) {
              return renderQuota(value, 6);
            }
          }
        }
      ],
      series: data?.models?.map((item: any) => {
        return ({
          name: item.name,
          type: 'bar',
          data: item.data,
          stack: 'account',
        })
      })
    };

    modelConsumptionDistributionOption && modelConsumptionDistributionChart.setOption(modelConsumptionDistributionOption);

    const proportionOfModelCallsChart = echarts.init(document.getElementById('proportion-of-model-calls') as HTMLDivElement);

    const proportionOfModel = data?.models?.map((item: any) => {
      return ({
        value: item.tokenUsed,
        name: item.name
      })
    })

    let option = {
      tooltip: {
        trigger: 'item',
        formatter: (params: any) => {
          return `${params.name}：消耗${params.value}token(${params.percent}%)`
        }
      },
      legend: {
        show: true,
        // 靠左边居中
        top: 'middle',
        left: 0,
        orient: 'vertical',
        data: data?.models?.map((item: any) => item.name)
      },
      series: [
        {
          name: '模型Token消耗占比',
          type: 'pie',
          radius: ['35%', '55%'],
          avoidLabelOverlap: true,
          itemStyle: {
            borderRadius: 10,
            borderColor: '#fff',
            borderWidth: 2,
          },
          emphasis: {
            label: {
              show: true,
              fontSize: 20,
            }
          },
          data: proportionOfModel
        }
      ]
    };

    option && proportionOfModelCallsChart.setOption(option);

    // 监听窗口变化
    window.addEventListener('resize', () => {
      consumesChart.resize();
      requestsChart.resize();
      tokensChart.resize();
      modelConsumptionDistributionChart.resize();
      proportionOfModelCallsChart.resize();
    });

    return () => {
      window.removeEventListener('resize', () => {
        consumesChart.resize();
        requestsChart.resize();
        tokensChart.resize();
        modelConsumptionDistributionChart.resize();
        proportionOfModelCallsChart.resize();
      });
    }

  }, [data]);

  return (
    <Dashboard>
      <Row>
        <Col style={{
          marginBottom: 10,
        }} span={6} xs={24} sm={12} md={8} lg={6}>
          <Card
            style={{
              maxWidth: 360,
              height: 220,
              backgroundColor: 'var(--semi-color-fill-0)',
              userSelect: 'none',
            }}
          >
            <div style={{
              fontSize: '18px',
              marginBottom: 10
            }}>
              当前余额：<Tag color="#108ee9">
              {renderQuota(data?.currentResidualCredit, 6)}
              </Tag>
            </div>
            <div style={{
              fontSize: '18px',
              marginBottom: 10,
              marginTop: 10
            }}>
              消费累计：<Tag color="#108ee9">{renderQuota(data?.currentConsumedCredit, 2)}</Tag>
            </div>
            <div style={{
              fontSize: '18px',
              marginBottom: 10,
              marginTop: 10
            }}>
              总请求数：<Tag color="#108ee9">{data?.totalRequestCount}</Tag>
            </div>
            <div style={{
              fontSize: '18px',
              marginBottom: 10,
              marginTop: 10
            }}>
              token消耗数：<Tag color="#108ee9">{renderNumber(data?.totalTokenCount)}</Tag>
            </div>
          </Card>
        </Col>
        <Col style={{
          marginBottom: 10,
        }} span={6} xs={24} sm={12} md={8} lg={6}>
          <Card
            style={{
              maxWidth: 360,
              backgroundColor: 'var(--semi-color-fill-0)'
            }}
          >
            <div style={{
              marginBottom: 16
            }}>
              今日消费：{renderQuota(currentDayConsume, 2)}
            </div>
            <Divider />
            <div style={{
              height: '100%',
              width: '100%',
            }} id='consumes'>

            </div>
          </Card>
        </Col>
        <Col style={{
          marginBottom: 10,
        }} span={6} xs={24} sm={12} md={8} lg={6}>
          <Card
            style={{
              maxWidth: 360,
              backgroundColor: 'var(--semi-color-fill-0)'
            }}
          >
            <div style={{
              marginBottom: 16
            }}>
              今日请求数：{currentDayRequest}
            </div>
            <Divider />
            <div style={{
              height: '100%',
              width: '100%',
            }} id='requests'>

            </div>
          </Card>
        </Col>
        <Col style={{
          marginBottom: 10,
        }} span={6} xs={24} sm={12} md={8} lg={6}>

          <Card
            style={{
              maxWidth: 360,
              backgroundColor: 'var(--semi-color-fill-0)'
            }}
          >
            <div style={{
              marginBottom: 16
            }}>
              今日消耗token数：{renderNumber(currentDayToken)}
            </div>
            <Divider />
            <div style={{
              height: '100%',
              width: '100%',
            }} id='tokens'>

            </div>
          </Card>
        </Col>
      </Row>
      <Card style={{
        height: 600,
      }}>

        <Tabs type="line"
          items={[
            {
              key: '1',
              label: '模型消耗分布',
              forceRender: true,
              children: <div style={{
                height: '500px',
                width: '100%',
              }} id='model-consumption-distribution'>

              </div>
            },
            {
              key: '2',
              label: '模型Token消耗占比',
              forceRender: true,
              children: <div style={{
                height: 'calc(100vh - 400px)',
                width: 'calc(100vw - 100px)',
              }} id='proportion-of-model-calls'>
              </div>
            }
          ]}
          style={{
            height: '100%',
            width: '100%',
          }}>
        </Tabs>
      </Card>
    </Dashboard>
  );
}
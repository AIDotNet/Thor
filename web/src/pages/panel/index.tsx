
import { useEffect, useState } from 'react';
import './index.css'
import { GetStatistics } from '../../services/StatisticsService';
import * as echarts from 'echarts';
import { Card, Col, Divider, Row, TabPane, Tabs } from '@douyinfe/semi-ui';
import { renderQuota } from '../../uitls/render';

export default function Panel() {
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
                }
            },
            tooltip: {
                trigger: 'axis',
                axisPointer: {
                    type: 'line',
                    lineStyle: {
                        color: '#999999',
                        type: 'dashed'
                    }
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
                left: 0,
                top: 5,
                right: 0,
                bottom: 0
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
                trigger: 'axis',
                axisPointer: {
                    type: 'line',
                    lineStyle: {
                        color: '#999999',
                        type: 'dashed'
                    }
                }
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
                left: 0,
                top: 5,
                right: 0,
                bottom: 0
            },
        };

        // 获取data.data.consumes中的最后一个元素
        const lastRequest = data?.consumes?.slice(-1)[0];
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
                trigger: 'axis',
                axisPointer: {
                    type: 'line',
                    lineStyle: {
                        color: '#999999',
                        type: 'dashed'
                    }
                }
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
                left: 0,
                top: 5,
                right: 0,
                bottom: 0
            },
        };

        const lastTokens = data?.tokens?.slice(-1)[0];
        setCurrentDayToken(lastTokens?.value);

        tokensOption && tokensChart.setOption(tokensOption);

        const modelConsumptionDistributionChart = echarts.init(document.getElementById('model-consumption-distribution') as HTMLDivElement);
        var modelConsumptionDistributionOption = {
            tooltip: {
                trigger: 'axis',
                axisPointer: {
                    type: "cross",
                    label: {
                        formatter: function (params: any) {
                            if (params.seriesData.length === 0) {
                                // @ts-ignore
                                window.mouseCurValue = params.value;
                            }
                        }
                    }
                },
                formatter: function (params: any) {
                    let res = "", sum = 0;
                    for (let i = 0; i < params.length; i++) {
                        let series = params[i];
                        sum += Number(series.data);
                        // @ts-ignore
                        if (sum >= window.mouseCurValue) {
                            res = series.axisValue + "<br/>" + series.marker + series.seriesName + ":" + renderQuota(series.data, 6) + "<br/>";
                            break;
                        }
                    }
                    return res;
                },
            },
            legend: {
                orient: 'vertical',
                left: 10,
                data: data?.models?.map((item: any) => item.name)
            },
            toolbox: {
                trigger: 'axis',
                axisPointer: {
                    type: 'line',
                    lineStyle: {
                        color: '#999999',
                        type: 'dashed'
                    }
                },
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            xAxis: [
                {
                    type: 'category',
                    data: data.modelDate,
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
                    stack: 'Ad',
                    emphasis: {
                        focus: 'series'
                    },
                    label: {
                        show: true,
                        position: 'top',
                        formatter: function (params: any) {
                            return renderQuota(params.value, 6);
                        }
                    }
                })
            })
        };

        modelConsumptionDistributionOption && modelConsumptionDistributionChart.setOption(modelConsumptionDistributionOption);

        // 获取proportion-of-model-calls 显示一个饼图，data.models中的moduleName是模型的名字，value是消耗的值
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
                    return `${params.name}：${renderQuota(params.value, 6)}(${params.percent}%)`
                }
            },
            legend: {
                top: '90%',
                left: 'center'
            },
            series: [
                {
                    name: 'Access From',
                    type: 'pie',
                    radius: ['30%', '50%'],
                    avoidLabelOverlap: false,
                    itemStyle: {
                        borderRadius: 10,
                        borderColor: '#fff',
                        borderWidth: 2
                    },
                    label: {
                        show: true,
                        formatter: '{b}：{d}%', // 用来换行
                    },
                    emphasis: {
                        label: {
                            show: true,
                            fontSize: 40,
                            fontWeight: 'bold'
                        }
                    },
                    labelLine: {
                        show: false
                    },
                    data: proportionOfModel
                }
            ]
        };

        option && proportionOfModelCallsChart.setOption(option);

    }, [data]);

    return (
        <div className="dashboard">

            <Row>
                <Col style={{
                    padding: '0 16px'
                }} span={8}>
                    <Card
                        style={{
                            maxWidth: 360,
                            backgroundColor: 'var(--semi-color-fill-0)'
                        }}
                    >
                        <div style={{
                            marginBottom: 16
                        }}>
                            今日消费：{renderQuota(currentDayConsume, 6)}
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
                    padding: '0 16px'
                }} span={8}>
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
                    padding: '0 16px'
                }} span={8}>

                    <Card
                        style={{
                            maxWidth: 360,
                            backgroundColor: 'var(--semi-color-fill-0)'
                        }}
                    >

                        <div style={{
                            marginBottom: 16
                        }}>
                            今日消耗token数：{currentDayToken}
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

                <Tabs type="line" style={{
                    height: '100%',
                    width: '100%',
                }}>
                    <TabPane tab="模型消耗分布" itemKey="1">
                        <div style={{
                            height: '500px',
                            width: '100%',
                        }} id='model-consumption-distribution'>

                        </div>
                    </TabPane>
                    <TabPane tab="模型调用占比" style={{
                        height: '100%',
                        width: '100%',
                    }} itemKey="2">
                        <div style={{
                            height: 'calc(100vh - 400px)',
                            width: 'calc(100vw - 100px)',
                        }} id='proportion-of-model-calls'>
                        </div>
                    </TabPane>
                </Tabs>
            </Card>
        </div>
    )
}

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

        // 获取data.data.consumes中的最后一个元素
        const lastTokens = data?.tokens?.slice(-1)[0];
        setCurrentDayToken(lastTokens?.value);

        tokensOption && tokensChart.setOption(tokensOption);

        // 获取model-consumption-distribution 显示一个柱状的图表，并且每一天有多个模型的消耗，data.models中的moduleName是模型的名字，value是消耗的值, 每一天的数据是一个对象，对象中有一个数组，数组中有多个对象，每个对象是一个模型的消耗
        const modelConsumptionDistributionChart = echarts.init(document.getElementById('model-consumption-distribution') as HTMLDivElement);

        data.models = data?.models?.map((item: any) => {
            return ({
                name: item.name,
                type: 'bar',
                data: item.data,
                stack: 'Ad',
                emphasis: {
                    focus: 'series'
                },
            })
        });

        var modelConsumptionDistributionOption = {
            tooltip: {
                trigger: 'axis',
                axisPointer: {
                    type: 'shadow'
                }
            },
            legend: {},
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            xAxis: [
                {
                    type: 'category',
                    data: data.modelDate
                }
            ],
            yAxis: [
                {
                    type: 'value'
                }
            ],
            series: data.models
        };

        modelConsumptionDistributionOption && modelConsumptionDistributionChart.setOption(modelConsumptionDistributionOption);

        // 获取proportion-of-model-calls 显示一个饼图，data.models中的moduleName是模型的名字，value是消耗的值
        const proportionOfModelCallsChart = echarts.init(document.getElementById('proportion-of-model-calls') as HTMLDivElement);
        var proportionOfModelCallsOption = {
            tooltip: {
                trigger: 'item'
            },
            legend: {
                top: '5%',
                left: 'center'
            },
            series: [
                {
                    name: 'Access From',
                    type: 'pie',
                    radius: ['40%', '70%'],
                    avoidLabelOverlap: false,
                    padAngle: 5,
                    itemStyle: {
                        borderRadius: 10
                    },
                    label: {
                        show: false,
                        position: 'center'
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
                    data: [
                        { value: 1048, name: 'Search Engine' },
                        { value: 735, name: 'Direct' },
                        { value: 580, name: 'Email' },
                        { value: 484, name: 'Union Ads' },
                        { value: 300, name: 'Video Ads' }
                    ]
                }
            ]
        };

        proportionOfModelCallsOption && proportionOfModelCallsChart.setOption(proportionOfModelCallsOption);

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

                <Tabs type="line">
                    <TabPane tab="模型消耗分布" itemKey="1">
                        <div style={{
                            height: '500px',
                            width: '100%',
                        }} id='model-consumption-distribution'>

                        </div>
                    </TabPane>
                    <TabPane tab="模型调用占比" itemKey="2">
                        <div style={{
                            height: '500px',
                            width: '100%',
                        }} id='proportion-of-model-calls'>

                        </div>
                    </TabPane>
                </Tabs>
            </Card>
        </div>
    )
}
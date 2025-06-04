import { useEffect, useState, useRef } from 'react';
import { Typography, Card, Row, Col, Space, ConfigProvider, Divider, DatePicker, Button, Select } from 'antd';
import { Flexbox } from 'react-layout-kit';
import { useTheme } from 'antd-style';
import {
    MessageOutlined,
    FileImageOutlined,
    AudioOutlined,
    BuildOutlined,
    DownloadOutlined,
    SoundOutlined,
    TranslationOutlined
} from '@ant-design/icons';
import { renderNumber, renderQuota } from '../../utils/render';
import * as echarts from 'echarts';
import { getTokens } from '../../services/TokenService';
import { GetUsage } from '../../services/UsageService';
import dayjs, { Dayjs } from 'dayjs';
import { useTranslation } from 'react-i18next';

const { Text } = Typography;
const { RangePicker } = DatePicker;

export default function UsagePage() {
    const theme = useTheme();
    const [loading, setLoading] = useState(false);
    const [dateRange, setDateRange] = useState<[dayjs.Dayjs, dayjs.Dayjs] | null>(null);
    const [selectedApiKey, setSelectedApiKey] = useState<string | undefined>(undefined);
    const [tokenOptions, setTokenOptions] = useState<{ label: string; value: string }[]>([]);
    const { t } = useTranslation();

    // 使用数据
    const [usageData, setUsageData] = useState<any>({
        totalCost: 0,
        totalRequestCount: 0,
        totalTokenCount: 0,
        dailyUsage: [],
        serviceRequests: []
    });

    // 图表容器引用
    const chatCompletionChartRef = useRef<HTMLDivElement>(null);
    const imagesChartRef = useRef<HTMLDivElement>(null);
    const embeddingsChartRef = useRef<HTMLDivElement>(null);
    const audioSpeechChartRef = useRef<HTMLDivElement>(null);
    const audioTranscriptionChartRef = useRef<HTMLDivElement>(null);
    const audioTranslationChartRef = useRef<HTMLDivElement>(null);
    const dailyUsageChartRef = useRef<HTMLDivElement>(null);  // 消费图表
    const requestsChartRef = useRef<HTMLDivElement>(null);    // 请求数图表
    const tokensChartRef = useRef<HTMLDivElement>(null);      // 令牌数图表

    // 图表实例引用
    const chartInstancesRef = useRef<{ [key: string]: echarts.ECharts }>({});

    // 统一卡片样式
    const cardStyle = {
        borderRadius: 8,
        height: '100%',
        marginBottom: 16
    };

    const categoryCardStyle = {
        ...cardStyle,
        marginBottom: 0
    };

    // 加载Token列表
    const loadTokenList = async () => {
        setLoading(true);
        try {
            const response = await getTokens(1, 1000); // 获取足够多的token
            if (response.success && response.data) {
                const options = [
                    { label: t('usage.allApiKeys'), value: '' },
                    ...(response.data.items || []).map((item: any) => ({
                        label: item.name || item.key, // 使用名称，如果没有则使用Key
                        value: item.key
                    }))
                ];
                setTokenOptions(options);
            }
        } catch (error) {
            console.error('加载Token失败', error);
        } finally {
            setLoading(false);
        }
    };

    // 初始加载Token列表
    useEffect(() => {
        loadTokenList();

        // 默认设置日期范围为最近7天
        const endDate = dayjs();
        const startDate = dayjs().subtract(7, 'day');
        setDateRange([startDate, endDate]);
    }, []);

    // 根据选择的Token和日期范围获取使用数据
    const fetchUsageData = async () => {
        setLoading(true);
        try {
            // 如果未选择或选择的是空字符串，则查询所有Token的数据
            const tokenKey = !selectedApiKey || selectedApiKey === '' ? '' : selectedApiKey;

            const startDateStr = dateRange?.[0]?.format('YYYY-MM-DD');
            const endDateStr = dateRange?.[1]?.format('YYYY-MM-DD');

            const response = await GetUsage(tokenKey, startDateStr, endDateStr);

            if (response.success && response.data) {
                setUsageData(response.data);

                // 确保DOM已经渲染后再更新图表
                setTimeout(() => {
                    updateCharts(response.data);

                    // 手动触发resize确保图表正确渲染
                    Object.values(chartInstancesRef.current).forEach(chart => {
                        chart?.resize();
                    });
                }, 100);
            }
        } catch (error) {
            console.error('获取使用数据失败', error);
        } finally {
            setLoading(false);
        }
    };

    // 当选择的Token或日期范围变化时，重新获取数据
    useEffect(() => {
        if (dateRange) {
            fetchUsageData();
        }
    }, [selectedApiKey, dateRange]);

    // 处理每个API类别的数据
    const getCategoryData = (categoryEndpoint: string) => {
        // 筛选特定API端点的请求
        const filteredRequests = usageData.serviceRequests.filter(
            (req: any) => {
                // 对于聊天完成类别，同时包含/v1/chat/completions和/v1/completions
                if (categoryEndpoint === '/v1/chat/completions') {
                    return req.apiEndpoint.startsWith('/v1/chat/completions') ||
                        req.apiEndpoint.startsWith('/v1/completions');
                }
                // 其他类别正常匹配
                return req.apiEndpoint.startsWith(categoryEndpoint);
            }
        );

        // 汇总数据
        const summary = {
            requestCount: 0,
            tokenCount: 0,
            imageCount: 0,
            cost: 0,
            audioSeconds: 0
        };

        filteredRequests.forEach((req: any) => {
            summary.requestCount += req.requestCount || 0;
            summary.tokenCount += req.tokenCount || 0;
            summary.imageCount += req.imageCount || 0;
            summary.cost += req.cost || 0;
            // 音频API可能会有秒数计量，这里假设有AudioSeconds字段
            summary.audioSeconds += req.audioSeconds || 0;
        });

        return summary;
    };

    // 更新图表数据
    const updateCharts = (data: any) => {
        // 如果没有数据或者没有日期数据，则不进行图表更新
        if (!data || !data.dailyUsage || data.dailyUsage.length === 0) {
            return;
        }

        // 提取日期和按服务分类的数据
        const dates = data.dailyUsage.map((item: any) => {
            const date = new Date(item.date);
            return `${date.getMonth() + 1}/${date.getDate()}`;
        }).reverse(); // 从旧到新

        // 为每日使用柱状图准备数据
        const dailyUsageData = data.dailyUsage.map((item: any) => ({
            date: new Date(item.date),
            cost: item.cost,
            requestCount: item.requestCount,
            tokenCount: item.tokenCount,
            modelUsage: item.modelUsage || []
        })).sort((a: any, b: any) => a.date.getTime() - b.date.getTime()); // 按日期升序排序

        const dailyDates = dailyUsageData.map((item: any) => `${item.date.getMonth() + 1}/${item.date.getDate()}`);
        const dailyCosts = dailyUsageData.map((item: any) => item.cost);
        const dailyRequests = dailyUsageData.map((item: any) => item.requestCount);
        const dailyTokens = dailyUsageData.map((item: any) => item.tokenCount);

        // 更新三个独立图表，传入模型使用数据
        updateCostChart(dailyDates, dailyCosts, dailyUsageData);
        updateRequestsChart(dailyDates, dailyRequests, dailyUsageData);
        updateTokensChart(dailyDates, dailyTokens, dailyUsageData);

        // 提取各服务的数据
        const chatServiceData = prepareChartDataForService('/v1/chat/completions', data);
        const imageServiceData = prepareChartDataForService('/v1/images/generations', data);
        const embeddingsServiceData = prepareChartDataForService('/v1/embeddings', data);
        const audioSpeechServiceData = prepareChartDataForService('/v1/audio/speech', data);
        const audioTranscriptionServiceData = prepareChartDataForService('/v1/audio/transcriptions', data);
        const audioTranslationServiceData = prepareChartDataForService('/v1/audio/translations', data);

        // 更新图表
        updateChart(chatCompletionChartRef, t('usage.chatCompletion'), dates, chatServiceData, true);
        updateChart(imagesChartRef, t('usage.images'), dates, imageServiceData, false);
        updateChart(embeddingsChartRef, t('usage.embeddings'), dates, embeddingsServiceData, true);
        updateChart(audioSpeechChartRef, t('usage.audioSpeech'), dates, audioSpeechServiceData, false);
        updateChart(audioTranscriptionChartRef, t('usage.audioTranscription'), dates, audioTranscriptionServiceData, false);
        updateChart(audioTranslationChartRef, t('usage.audioTranslation'), dates, audioTranslationServiceData, false);
    };

    // 更新消费图表
    const updateCostChart = (dates: string[], costs: number[], dailyUsageData: any[]) => {
        if (!dailyUsageChartRef.current) return;

        // 销毁之前的实例（如果存在）
        if (chartInstancesRef.current['dailyUsage']) {
            chartInstancesRef.current['dailyUsage'].dispose();
        }

        // 创建新的图表实例
        const chart = echarts.init(dailyUsageChartRef.current);
        chartInstancesRef.current['dailyUsage'] = chart;

        // 计算总消费
        const totalCost = costs.reduce((sum, current) => sum + current, 0);

        // 检查是否有模型使用数据
        const hasModelUsage = dailyUsageData.some(item => item.modelUsage && item.modelUsage.length > 0);

        let series: any[] = [];

        if (hasModelUsage) {
            // 收集所有模型名称
            const allModels = new Set<string>();
            dailyUsageData.forEach(item => {
                if (item.modelUsage) {
                    item.modelUsage.forEach((model: any) => {
                        allModels.add(model.modelName);
                    });
                }
            });

            // 为每个模型创建系列数据
            const modelColors = [
                theme.colorPrimary,
                theme.colorSuccess,
                theme.colorWarning,
                theme.colorError,
                theme.colorInfo,
                '#722ed1', // 紫色
                '#eb2f96', // 粉色
                '#52c41a', // 绿色
                '#fa8c16', // 橙色
                '#13c2c2'  // 青色
            ];

            Array.from(allModels).forEach((modelName, index) => {
                const modelData = dailyUsageData.map(item => {
                    const modelUsage = item.modelUsage?.find((m: any) => m.modelName === modelName);
                    return modelUsage ? modelUsage.cost : 0;
                });

                series.push({
                    name: modelName,
                    type: 'bar',
                    stack: 'cost',
                    barWidth: '60%',
                    itemStyle: {
                        color: modelColors[index % modelColors.length]
                    },
                    emphasis: {
                        itemStyle: {
                            opacity: 0.8
                        }
                    },
                    data: modelData
                });
            });
        } else {
            // 没有模型数据时使用原来的逻辑
            series = [{
                name: t('usage.cost'),
                type: 'bar',
                barWidth: '60%',
                itemStyle: {
                    color: theme.colorPrimary
                },
                emphasis: {
                    itemStyle: {
                        color: theme.colorPrimaryActive
                    }
                },
                data: costs
            }];
        }

        // 消费图表配置
        const option = {
            backgroundColor: 'transparent',
            title: {
                text: `${t('usage.totalSpend')}: ${renderQuota(totalCost)}`,
                left: 10,
                top: 10,
                textStyle: {
                    fontSize: 14,
                    fontWeight: 'bold',
                    color: theme.colorPrimary
                }
            },
            grid: {
                top: hasModelUsage ? 80 : 60,
                right: 20,
                bottom: 50,
                left: 0,
                containLabel: false
            },
            legend: hasModelUsage ? {
                top: 40,
                left: 10,
                orient: 'horizontal',
                itemGap: 10,
                textStyle: {
                    fontSize: 12,
                    color: theme.colorText
                }
            } : undefined,
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
                    let result = `<div style="font-weight:bold;margin-bottom:5px">${t('usage.date')}: ${dateStr}</div>`;

                    params.forEach((param: any) => {
                        if (param.value > 0) {
                            let valueText = renderQuota(param.value);

                            result += `<div style="display:flex;justify-content:space-between;margin:3px 0">
                  <span style="margin-right:15px">
                    <span style="display:inline-block;width:10px;height:10px;border-radius:50%;background-color:${param.color};margin-right:5px"></span>
                    ${param.seriesName}:
                  </span>
                  <span style="font-weight:bold">${valueText}</span>
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
                data: dates,
                boundaryGap: true,
                axisLine: {
                    lineStyle: { color: theme.colorBorderSecondary }
                },
                axisTick: { alignWithLabel: true },
                axisLabel: {
                    interval: 0,
                    rotate: dates.length > 5 ? 30 : 0,
                    fontSize: 11,
                    margin: 10,
                    formatter: (value: string) => {
                        const date = new Date(value);
                        return `${date.getMonth() + 1}/${date.getDate()}`;
                    },
                    color: theme.colorTextSecondary,
                    hideOverlap: false
                }
            },
            yAxis: {
                type: 'value',
                name: '',
                axisLine: { show: false },
                axisTick: { show: false },
                axisLabel: { show: false },
                splitLine: { show: false }
            },
            series: series
        };

        // 设置配置
        chart.setOption(option);
    };

    // 更新请求数图表
    const updateRequestsChart = (dates: string[], requests: number[], dailyUsageData: any[]) => {
        if (!requestsChartRef.current) return;

        // 销毁之前的实例（如果存在）
        if (chartInstancesRef.current['requests']) {
            chartInstancesRef.current['requests'].dispose();
        }

        // 创建新的图表实例
        const chart = echarts.init(requestsChartRef.current);
        chartInstancesRef.current['requests'] = chart;

        // 计算总请求数
        const totalRequests = requests.reduce((sum, current) => sum + current, 0);

        // 检查是否有模型使用数据
        const hasModelUsage = dailyUsageData.some(item => item.modelUsage && item.modelUsage.length > 0);

        let series: any[] = [];

        if (hasModelUsage) {
            // 收集所有模型名称
            const allModels = new Set<string>();
            dailyUsageData.forEach(item => {
                if (item.modelUsage) {
                    item.modelUsage.forEach((model: any) => {
                        allModels.add(model.modelName);
                    });
                }
            });

            // 为每个模型创建系列数据
            const modelColors = [
                theme.colorSuccess,
                theme.colorPrimary,
                theme.colorWarning,
                theme.colorError,
                theme.colorInfo,
                '#722ed1',
                '#eb2f96',
                '#52c41a',
                '#fa8c16',
                '#13c2c2'
            ];

            Array.from(allModels).forEach((modelName, index) => {
                const modelData = dailyUsageData.map(item => {
                    const modelUsage = item.modelUsage?.find((m: any) => m.modelName === modelName);
                    return modelUsage ? modelUsage.requestCount : 0;
                });

                series.push({
                    name: modelName,
                    type: 'bar',
                    stack: 'requests',
                    barWidth: '60%',
                    itemStyle: {
                        color: modelColors[index % modelColors.length]
                    },
                    emphasis: {
                        itemStyle: {
                            opacity: 0.8
                        }
                    },
                    data: modelData
                });
            });
        } else {
            // 没有模型数据时使用原来的逻辑
            series = [{
                name: t('usage.requests'),
                type: 'bar',
                barWidth: '60%',
                itemStyle: {
                    color: theme.colorSuccess
                },
                emphasis: {
                    itemStyle: {
                        color: theme.colorSuccessActive
                    }
                },
                data: requests
            }];
        }

        // 请求数图表配置
        const option = {
            backgroundColor: 'transparent',
            title: {
                text: `${t('usage.totalRequests')}: ${renderNumber(totalRequests)}`,
                left: 10,
                top: 10,
                textStyle: {
                    fontSize: 14,
                    fontWeight: 'bold',
                    color: theme.colorSuccess
                }
            },
            grid: {
                top: hasModelUsage ? 80 : 60,
                right: 20,
                bottom: 50,
                left: 0,
                containLabel: false
            },
            legend: hasModelUsage ? {
                top: 40,
                left: 10,
                orient: 'horizontal',
                itemGap: 10,
                textStyle: {
                    fontSize: 12,
                    color: theme.colorText
                }
            } : undefined,
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
                    let result = `<div style="font-weight:bold;margin-bottom:5px">${t('usage.date')}: ${dateStr}</div>`;

                    params.forEach((param: any) => {
                        let valueText = param.value;
                        if (param.value > 0) {
                            result += `<div style="display:flex;justify-content:space-between;margin:3px 0">
              <span style="margin-right:15px">
                <span style="display:inline-block;width:10px;height:10px;border-radius:50%;background-color:${param.color};margin-right:5px"></span>
                ${param.seriesName}:
              </span>
              <span style="font-weight:bold">${valueText}</span>
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
                data: dates,
                boundaryGap: true,
                axisLine: {
                    lineStyle: { color: theme.colorBorderSecondary }
                },
                axisTick: { alignWithLabel: true },
                axisLabel: {
                    interval: 0,
                    rotate: dates.length > 5 ? 30 : 0,
                    fontSize: 11,
                    margin: 10,
                    formatter: (value: string) => {
                        const date = new Date(value);
                        return `${date.getMonth() + 1}/${date.getDate()}`;
                    },
                    color: theme.colorTextSecondary,
                    hideOverlap: false
                }
            },
            yAxis: {
                type: 'value',
                name: '',
                axisLine: { show: false },
                axisTick: { show: false },
                axisLabel: { show: false },
                splitLine: { show: false }
            },
            series: series
        };

        // 设置配置
        chart.setOption(option);
    };

    // 更新令牌数图表
    const updateTokensChart = (dates: string[], tokens: number[], dailyUsageData: any[]) => {
        if (!tokensChartRef.current) return;

        // 销毁之前的实例（如果存在）
        if (chartInstancesRef.current['tokens']) {
            chartInstancesRef.current['tokens'].dispose();
        }

        // 创建新的图表实例
        const chart = echarts.init(tokensChartRef.current);
        chartInstancesRef.current['tokens'] = chart;

        // 计算总令牌数
        const totalTokens = tokens.reduce((sum, current) => sum + current, 0);

        // 检查是否有模型使用数据
        const hasModelUsage = dailyUsageData.some(item => item.modelUsage && item.modelUsage.length > 0);

        let series: any[] = [];

        if (hasModelUsage) {
            // 收集所有模型名称
            const allModels = new Set<string>();
            dailyUsageData.forEach(item => {
                if (item.modelUsage) {
                    item.modelUsage.forEach((model: any) => {
                        allModels.add(model.modelName);
                    });
                }
            });

            // 为每个模型创建系列数据
            const modelColors = [
                theme.colorWarning,
                theme.colorPrimary,
                theme.colorSuccess,
                theme.colorError,
                theme.colorInfo,
                '#722ed1',
                '#eb2f96',
                '#52c41a',
                '#fa8c16',
                '#13c2c2'
            ];

            Array.from(allModels).forEach((modelName, index) => {
                const modelData = dailyUsageData.map(item => {
                    const modelUsage = item.modelUsage?.find((m: any) => m.modelName === modelName);
                    return modelUsage ? modelUsage.tokenCount : 0;
                });

                series.push({
                    name: modelName,
                    type: 'bar',
                    stack: 'tokens',
                    barWidth: '60%',
                    itemStyle: {
                        color: modelColors[index % modelColors.length]
                    },
                    emphasis: {
                        itemStyle: {
                            opacity: 0.8
                        }
                    },
                    data: modelData
                });
            });
        } else {
            // 没有模型数据时使用原来的逻辑
            series = [{
                name: t('usage.tokenCount'),
                type: 'bar',
                barWidth: '60%',
                itemStyle: {
                    color: theme.colorWarning
                },
                emphasis: {
                    itemStyle: {
                        color: theme.colorWarningActive
                    }
                },
                data: tokens
            }];
        }

        // 令牌数图表配置
        const option = {
            backgroundColor: 'transparent',
            title: {
                text: `${t('usage.totalTokens')}: ${renderNumber(totalTokens)}`,
                left: 10,
                top: 10,
                textStyle: {
                    fontSize: 14,
                    fontWeight: 'bold',
                    color: theme.colorWarning
                }
            },
            grid: {
                top: hasModelUsage ? 80 : 60,
                right: 20,
                bottom: 50,
                left: 0,
                containLabel: false
            },
            legend: hasModelUsage ? {
                top: 40,
                left: 10,
                orient: 'horizontal',
                itemGap: 10,
                textStyle: {
                    fontSize: 12,
                    color: theme.colorText
                }
            } : undefined,
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
                    let result = `<div style="font-weight:bold;margin-bottom:5px">${t('usage.date')}: ${dateStr}</div>`;

                    params.forEach((param: any) => {
                        if (param.value > 0) {
                            let valueText = renderNumber(param.value);

                        result += `<div style="display:flex;justify-content:space-between;margin:3px 0">
              <span style="margin-right:15px">
                <span style="display:inline-block;width:10px;height:10px;border-radius:50%;background-color:${param.color};margin-right:5px"></span>
                ${param.seriesName}:
              </span>
              <span style="font-weight:bold">${valueText}</span>
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
                data: dates,
                boundaryGap: true,
                axisLine: {
                    lineStyle: { color: theme.colorBorderSecondary }
                },
                axisTick: { alignWithLabel: true },
                axisLabel: {
                    interval: 0,
                    rotate: dates.length > 5 ? 30 : 0,
                    fontSize: 11,
                    margin: 10,
                    formatter: (value: string) => {
                        const date = new Date(value);
                        return `${date.getMonth() + 1}/${date.getDate()}`;
                    },
                    color: theme.colorTextSecondary,
                    hideOverlap: false
                }
            },
            yAxis: {
                type: 'value',
                name: '',
                axisLine: { show: false },
                axisTick: { show: false },
                axisLabel: { show: false },
                splitLine: { show: false }
            },
            series: series
        };

        // 设置配置
        chart.setOption(option);
    };

    // 准备特定服务的图表数据
    const prepareChartDataForService = (apiEndpoint: string, data: any) => {
        const resultData = {
            requestCount: new Array(data.dailyUsage.length).fill(0),
            tokenCount: new Array(data.dailyUsage.length).fill(0)
        };

        // 按照日期倒序排列
        const sortedDailyUsage = [...data.dailyUsage].sort((a: any, b: any) => {
            return new Date(a.date).getTime() - new Date(b.date).getTime();
        });

        sortedDailyUsage.forEach((daily, index) => {
            const requests = data.serviceRequests.filter(
                (req: any) => {
                    // 聊天完成类别特殊处理
                    if (apiEndpoint === '/v1/chat/completions') {
                        return (req.apiEndpoint.startsWith('/v1/chat/completions') ||
                            req.apiEndpoint.startsWith('/v1/completions')) &&
                            req.date === daily.date;
                    }
                    // 其他类别正常匹配
                    return req.apiEndpoint.startsWith(apiEndpoint) && req.date === daily.date;
                }
            );

            const totalRequests = requests.reduce((sum: number, req: any) => sum + (req.requestCount || 0), 0);
            const totalTokens = requests.reduce((sum: number, req: any) => sum + (req.tokenCount || 0), 0);

            resultData.requestCount[index] = totalRequests;
            resultData.tokenCount[index] = totalTokens;
        });

        return resultData;
    };

    // 更新单个图表
    const updateChart = (chartRef: any, title: string, dates: any, seriesData: any, hasTokens: boolean) => {
        if (!chartRef.current) return;

        // 销毁之前的实例（如果存在）
        if (chartInstancesRef.current[title]) {
            chartInstancesRef.current[title].dispose();
        }

        // 创建新的图表实例
        const chart = echarts.init(chartRef.current);
        chartInstancesRef.current[title] = chart;

        // 基本配置 - 使用简化配置，专注于解决日期标签显示问题
        const option = {
            backgroundColor: 'transparent',
            grid: {
                top: 30,        // 使用固定像素值
                right: 20,      // 不需要右侧空间，因为不再使用双Y轴
                bottom: 50,     // 确保底部有足够空间
                left: 50,       // 为Y轴留出足够空间
                containLabel: true
            },
            tooltip: {
                trigger: 'axis',
                confine: true, // 确保提示框在图表区域内
                backgroundColor: theme.colorBgElevated,
                borderColor: theme.colorBorder,
                textStyle: {
                    color: theme.colorText
                },
                axisPointer: {
                    type: 'shadow',
                    lineStyle: {
                        color: 'transparent'
                    },
                    crossStyle: {
                        color: 'transparent'
                    }
                },
                formatter: (params: any) => {
                    const dateStr = params[0].axisValue;
                    let result = `<div style="font-weight:bold;margin-bottom:5px">${t('usage.date')}: ${dateStr}</div>`;

                    params.forEach((param: any) => {
                        let valueText = param.seriesName === t('usage.spend')
                            ? renderQuota(param.value)
                            : renderNumber(param.value);

                        result += `<div style="display:flex;justify-content:space-between;margin:3px 0">
              <span style="margin-right:15px">
                <span style="display:inline-block;width:10px;height:10px;border-radius:50%;background-color:${param.color};margin-right:5px"></span>
                ${param.seriesName}:
              </span>
              <span style="font-weight:bold">${valueText}</span>
            </div>`;
                    });

                    return result;
                }
            },
            legend: hasTokens ? {
                data: [t('usage.requests'), t('usage.tokenCount')],
                right: 0,
                top: 0
            } : {
                data: [t('usage.requests')],
                right: 0,
                top: 0
            },
            xAxis: {
                type: 'category',
                data: dates,
                boundaryGap: true,
                axisLine: {
                    lineStyle: { color: theme.colorBorderSecondary }
                },
                axisTick: { alignWithLabel: true },
                axisLabel: {
                    interval: 0,                  // 显示所有标签
                    rotate: dates.length > 5 ? 30 : 0,
                    fontSize: 11,
                    margin: 10,
                    formatter: (value: any) => value,
                    color: theme.colorTextSecondary,
                    hideOverlap: false
                }
            },
            yAxis: {
                type: 'value',
                name: hasTokens ? t('usage.quantity') : t('usage.requests'),
                axisLine: {
                    show: true,
                    lineStyle: { color: theme.colorPrimary }
                },
                axisTick: { show: true },
                axisLabel: {
                    color: theme.colorTextSecondary
                },
                splitLine: {
                    lineStyle: {
                        color: theme.colorSplit,
                        type: 'dashed'
                    }
                }
            },
            series: hasTokens ? [
                {
                    name: t('usage.requests'),
                    type: 'bar',
                    stack: '总量',
                    barWidth: '40%',
                    itemStyle: {
                        color: theme.colorPrimary
                    },
                    emphasis: {
                        itemStyle: {
                            color: theme.colorPrimaryActive
                        }
                    },
                    data: seriesData.requestCount
                },
                {
                    name: t('usage.tokenCount'),
                    type: 'bar',
                    stack: '总量',
                    barWidth: '40%',
                    itemStyle: {
                        color: theme.colorSuccess
                    },
                    emphasis: {
                        itemStyle: {
                            color: theme.colorSuccessActive
                        }
                    },
                    data: seriesData.tokenCount
                }
            ] : [
                {
                    name: t('usage.requests'),
                    type: 'bar',
                    barWidth: '40%',
                    itemStyle: {
                        color: theme.colorPrimary
                    },
                    emphasis: {
                        itemStyle: {
                            color: theme.colorPrimaryActive
                        }
                    },
                    data: seriesData.requestCount
                }
            ]
        };

        // 设置配置
        chart.setOption(option);
    };

    // 导出数据
    const handleExport = () => {
        // 如果未选择或选择的是空字符串，则导出所有Token的数据
        const tokenKey = !selectedApiKey || selectedApiKey === '' ? undefined : selectedApiKey;

        console.log('导出数据', {
            tokenKey,
            startDate: dateRange?.[0]?.format('YYYY-MM-DD'),
            endDate: dateRange?.[1]?.format('YYYY-MM-DD')
        });

        // 这里实现导出逻辑，例如创建CSV文件
        const csvContent = generateCSV(usageData);
        const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.setAttribute('href', url);
        link.setAttribute('download', `usage-${dateRange?.[0]?.format('YYYY-MM-DD')}-to-${dateRange?.[1]?.format('YYYY-MM-DD')}.csv`);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    };

    // 生成CSV文件内容
    const generateCSV = (data: any) => {
        // CSV头部
        let csvContent = `${t('usage.date')},${t('usage.apiEndpoint')},${t('usage.apiName')},${t('usage.model')},${t('usage.requests')},${t('usage.tokenCount')},${t('usage.imageCount')},${t('usage.cost')}\n`;

        // 添加行数据
        if (data.serviceRequests && data.serviceRequests.length > 0) {
            data.serviceRequests.forEach((req: any) => {
                const date = new Date(req.date).toISOString().split('T')[0];
                const cost = renderQuota(req.cost);
                const row = `${date},${req.apiEndpoint},${req.apiName},${req.modelName},${req.requestCount},${req.tokenCount},${req.imageCount},${cost}\n`;
                csvContent += row;
            });
        }

        return csvContent;
    };

    // 初始化并监听窗口大小变化
    useEffect(() => {
        const handleResize = () => {
            // 确保所有图表实例都调整大小
            Object.values(chartInstancesRef.current).forEach(chart => {
                chart?.resize();
            });
        };

        window.addEventListener('resize', handleResize);

        // 当组件完全挂载后，手动触发一次resize
        setTimeout(handleResize, 300);

        return () => {
            window.removeEventListener('resize', handleResize);
            // 销毁所有图表实例
            Object.values(chartInstancesRef.current).forEach(chart => {
                chart?.dispose();
            });
            chartInstancesRef.current = {};
        };
    }, []);

    // 获取各类别的统计数据
    const chatCompletionsData = getCategoryData('/v1/chat/completions');
    const imagesData = getCategoryData('/v1/images/generations');
    const embeddingsData = getCategoryData('/v1/embeddings');
    const audioSpeechData = getCategoryData('/v1/audio/speech');
    const audioTranscriptionData = getCategoryData('/v1/audio/transcriptions');
    const audioTranslationData = getCategoryData('/v1/audio/translations');

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
            <div style={{ padding: '0 16px' }}>
                <Row gutter={[16, 16]} style={{ marginBottom: 16 }}>
                    <Col flex="auto">
                        <Select
                            placeholder={t('usage.selectApiKey')}
                            style={{ width: 300 }}
                            options={tokenOptions}
                            onChange={(value) => setSelectedApiKey(value)}
                            loading={loading}
                            showSearch
                            optionFilterProp="label"
                            allowClear
                            value={selectedApiKey}
                        />
                    </Col>
                    <Col>
                        <Space>
                            <RangePicker
                                value={dateRange}
                                onChange={(dates) => {
                                    if (dates) {
                                        setDateRange(dates as [Dayjs, Dayjs]);
                                    } else {
                                        setDateRange(null);
                                    }
                                }}
                            />
                            <Button icon={<DownloadOutlined />} onClick={handleExport} disabled={loading}>
                                {t('usage.exportData')}
                            </Button>
                        </Space>
                    </Col>
                </Row>

                <Row gutter={[16, 16]}>
                    <Col span={24}>
                        <Card style={cardStyle} loading={loading}>
                            <Row>
                                <Col span={24}>
                                    <Row gutter={[16, 16]}>
                                        <Col xs={24} md={12}>
                                            <div style={{ position: 'relative' }}>
                                                <div
                                                    style={{
                                                        position: 'absolute',
                                                        top: 0,
                                                        left: 0,
                                                        right: 0,
                                                        height: '4px',
                                                        borderRadius: '8px 8px 0 0'
                                                    }}
                                                />
                                                <div
                                                    style={{
                                                        position: 'absolute',
                                                        top: 12,
                                                        left: 12,
                                                        fontWeight: 'bold',
                                                        fontSize: '16px',
                                                        color: theme.colorPrimary
                                                    }}
                                                >
                                                    {t('usage.spendStatistics')}
                                                </div>
                                                <div
                                                    ref={dailyUsageChartRef}
                                                    style={{
                                                        height: 380,
                                                        background: theme.colorBgContainer,
                                                        borderRadius: 8,
                                                        paddingTop: 40
                                                    }}
                                                />
                                            </div>
                                        </Col>
                                        <Col xs={24} md={12}>
                                            <Row>
                                                <Col span={24}>
                                                    <div style={{ position: 'relative' }}>
                                                        <div
                                                            style={{
                                                                position: 'absolute',
                                                                top: 0,
                                                                left: 0,
                                                                right: 0,
                                                                height: '4px',
                                                                borderRadius: '8px 8px 0 0'
                                                            }}
                                                        />
                                                        <div
                                                            ref={requestsChartRef}
                                                            style={{
                                                                height: 180,
                                                                background: theme.colorBgContainer,
                                                                borderRadius: 8,
                                                                boxShadow: '0 3px 10px rgba(0, 0, 0, 0.1)',
                                                            }}
                                                        />
                                                    </div>
                                                </Col>
                                            </Row>
                                            <Row style={{ marginTop: 20 }}>
                                                <Col span={24}>
                                                    <div style={{ position: 'relative' }}>
                                                        <div
                                                            style={{
                                                                position: 'absolute',
                                                                top: 0,
                                                                left: 0,
                                                                right: 0,
                                                                height: '4px',
                                                                borderRadius: '8px 8px 0 0'
                                                            }}
                                                        />
                                                        <div
                                                            style={{
                                                                position: 'absolute',
                                                                top: 12,
                                                                left: 12,
                                                                fontWeight: 'bold',
                                                                fontSize: '16px',
                                                                color: theme.colorWarning
                                                            }}
                                                        >
                                                            {t('usage.tokensStatistics')}
                                                        </div>
                                                        <div
                                                            ref={tokensChartRef}
                                                            style={{
                                                                height: 180,
                                                                background: theme.colorBgContainer,
                                                                borderRadius: 8,
                                                            }}
                                                        />
                                                    </div>
                                                </Col>
                                            </Row>
                                        </Col>
                                    </Row>
                                </Col>
                            </Row>
                        </Card>
                    </Col>

                    <Col span={24}>
                        <Divider orientation="left">{t('usage.apiCapabilities')}</Divider>
                        <Row gutter={[16, 16]}>
                            <Col xs={24} md={12} lg={8}>
                                <Card
                                    title={<Space>{<MessageOutlined />} {t('usage.chatCompletions')}</Space>}
                                    style={categoryCardStyle}
                                    loading={loading}
                                >
                                    <>
                                        <Space direction="vertical" style={{ width: '100%' }}>
                                            <Flexbox horizontal justify="space-between">
                                                <Text>{t('usage.requests')}</Text>
                                                <Text>{chatCompletionsData.requestCount}</Text>
                                            </Flexbox>
                                            <Flexbox horizontal justify="space-between">
                                                <Text>{t('usage.tokenCount')}</Text>
                                                <Text>{renderNumber(chatCompletionsData.tokenCount)}</Text>
                                            </Flexbox>
                                            <Flexbox horizontal justify="space-between">
                                                <Text>{t('usage.cost')}</Text>
                                                <Text>{renderQuota(chatCompletionsData.cost)}</Text>
                                            </Flexbox>
                                        </Space>
                                        <div ref={chatCompletionChartRef} style={{ height: 320, marginTop: 16 }} />
                                    </>
                                </Card>
                            </Col>

                            <Col xs={24} md={12} lg={8}>
                                <Card
                                    title={<Space>{<FileImageOutlined />} {t('usage.images')}</Space>}
                                    style={categoryCardStyle}
                                    loading={loading}
                                >
                                    <>
                                        <Space direction="vertical" style={{ width: '100%' }}>
                                            <Flexbox horizontal justify="space-between">
                                                <Text>{t('usage.requests')}</Text>
                                                <Text>{imagesData.requestCount}</Text>
                                            </Flexbox>
                                            <Flexbox horizontal justify="space-between">
                                                <Text>{t('usage.imageCount')}</Text>
                                                <Text>{imagesData.imageCount}</Text>
                                            </Flexbox>
                                            <Flexbox horizontal justify="space-between">
                                                <Text>{t('usage.cost')}</Text>
                                                <Text>{renderQuota(imagesData.cost)}</Text>
                                            </Flexbox>
                                        </Space>
                                        <div ref={imagesChartRef} style={{ height: 320, marginTop: 16 }} />
                                    </>
                                </Card>
                            </Col>

                            <Col xs={24} md={12} lg={8}>
                                <Card
                                    title={<Space>{<BuildOutlined />} {t('usage.embeddings')}</Space>}
                                    style={categoryCardStyle}
                                    loading={loading}
                                >
                                    <Space direction="vertical" style={{ width: '100%' }}>
                                        <Flexbox horizontal justify="space-between">
                                            <Text>{t('usage.requests')}</Text>
                                            <Text>{embeddingsData.requestCount}</Text>
                                        </Flexbox>
                                        <Flexbox horizontal justify="space-between">
                                            <Text>{t('usage.inputTokens')}</Text>
                                            <Text>{embeddingsData.tokenCount}</Text>
                                        </Flexbox>
                                        <Flexbox horizontal justify="space-between">
                                            <Text>{t('usage.cost')}</Text>
                                            <Text>{renderQuota(embeddingsData.cost)}</Text>
                                        </Flexbox>
                                    </Space>
                                    <div ref={embeddingsChartRef} style={{ height: 320, marginTop: 16 }} />
                                </Card>
                            </Col>

                            <Col xs={24} md={12} lg={8}>
                                <Card
                                    title={<Space>{<SoundOutlined />} {t('usage.audioSpeech')}</Space>}
                                    style={categoryCardStyle}
                                    loading={loading}
                                >
                                    <>
                                        <Space direction="vertical" style={{ width: '100%' }}>
                                            <Flexbox horizontal justify="space-between">
                                                <Text>{t('usage.requests')}</Text>
                                                <Text>{audioSpeechData.requestCount}</Text>
                                            </Flexbox>
                                            <Flexbox horizontal justify="space-between">
                                                <Text>{t('usage.cost')}</Text>
                                                <Text>{renderQuota(audioSpeechData.cost)}</Text>
                                            </Flexbox>
                                        </Space>
                                        <div ref={audioSpeechChartRef} style={{ height: 320, marginTop: 16 }} />
                                    </>
                                </Card>
                            </Col>
                            <Col xs={24} md={12} lg={8}>
                                <Card
                                    title={<Space>{<AudioOutlined />} {t('usage.audioTranscription')}</Space>}
                                    style={categoryCardStyle}
                                    loading={loading}
                                >
                                    <>
                                        <Space direction="vertical" style={{ width: '100%' }}>
                                            <Flexbox horizontal justify="space-between">
                                                <Text>{t('usage.requests')}</Text>
                                                <Text>{audioTranscriptionData.requestCount}</Text>
                                            </Flexbox>
                                            <Flexbox horizontal justify="space-between">
                                                <Text>{t('usage.cost')}</Text>
                                                <Text>{renderQuota(audioTranscriptionData.cost)}</Text>
                                            </Flexbox>
                                        </Space>
                                        <div ref={audioTranscriptionChartRef} style={{ height: 320, marginTop: 16 }} />
                                    </>
                                </Card>
                            </Col>

                            {/* 音频翻译 */}
                            <Col xs={24} md={12} lg={8}>
                                <Card
                                    title={<Space>{<TranslationOutlined />} {t('usage.audioTranslation')}</Space>}
                                    style={categoryCardStyle}
                                    loading={loading}
                                >
                                    <>
                                        <Space direction="vertical" style={{ width: '100%' }}>
                                            <Flexbox horizontal justify="space-between">
                                                <Text>{t('usage.requests')}</Text>
                                                <Text>{audioTranslationData.requestCount}</Text>
                                            </Flexbox>
                                            <Flexbox horizontal justify="space-between">
                                                <Text>{t('usage.cost')}</Text>
                                                <Text>{renderQuota(audioTranslationData.cost)}</Text>
                                            </Flexbox>
                                        </Space>
                                        <div ref={audioTranslationChartRef} style={{ height: 320, marginTop: 16 }} />
                                    </>
                                </Card>
                            </Col>
                        </Row>
                    </Col>
                </Row>
            </div>
        </ConfigProvider>
    );
} 
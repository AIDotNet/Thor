import { renderNumber, renderQuota } from '../../utils/render';
import { useEffect, useState } from 'react';
import { GetStatistics } from '../../services/StatisticsService';
import { Flexbox } from 'react-layout-kit';
import { BarList, DonutChart, FunnelChart, Heatmaps, LineChart, LineChartProps, Tracker } from '@lobehub/charts';
import { useTheme } from 'antd-style';
import { getIconByName } from '../../utils/iconutils';
import { GetServerLoad, GetUserRequest } from '../../services/TrackerService';

export default function PanelPage() {
  const [data, setData] = useState<any>(undefined);
  const [consumeChart, setConsumeChart] = useState<any[]>([]);
  const [requestChart, setRequestChart] = useState<any[]>([]);
  const [tokenChart, setTokenChart] = useState<any[]>([]);
  const [modelsChart, setModelsChart] = useState<any[]>([]);
  const [modelsName, setModelsName] = useState<any[]>([]);
  const [userNewData, setUserNewData] = useState<any[] | null>(null)
  const [rechargeData, setRechargeData] = useState<any[] | null>(null)
  const [trackerData, setTrackerData] = useState<any[]>([])
  const [userRequest, setUserRequest] = useState<any[]>([])

  const theme = useTheme();
  const colors = [theme.colorSuccess, theme.colorError];

  const commonFlexboxStyle = {
    width: '25%',
    marginLeft: 5,
  };

  function loadTrackerData() {
    GetServerLoad().then((res) => {
      setTrackerData(res.data)
    })
  }

  function loadUserRequest() {
    GetUserRequest().then((res) => {
      setUserRequest(res.data)
    })
  }

  function loadStatistics() {
    const consumeChart: any[] = [];
    const requestChart: any[] = [];
    const tokenChart: any[] = [];
    const modelsChart: any[] = [];
    const modelsName: any[] = [];

    GetStatistics()
      .then((res) => {
        const { modelDate, consumes, requests, tokens, models, userNewData, rechargeData } = res.data;

        if (userNewData) {
          setUserNewData(userNewData);
        } else {
          setUserNewData(null);
        }

        if (rechargeData) {
          setRechargeData(rechargeData)
        } else {
          setRechargeData(null)
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
  }

  const labelFormatter = (value: any) => {
    return `${value}` + '日';
  }

  const modelsValueFormatter: LineChartProps['valueFormatter'] = (value: any) => {
    return `${renderQuota(value)}`;
  }

  return (
    <Flexbox style={{
      padding: 20,
      height: 'calc(100vh - 150px)',
      overflow: 'auto',
      gap: 20,
    }}>
      <Flexbox horizontal>
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
        <Flexbox style={{
          width: '75%',
          marginLeft: 20,
        }}>
          <h4 style={{
            marginTop: 0,
          }}>
            服务器负载
          </h4>
          <Tracker data={trackerData} />
        </Flexbox>
      </Flexbox>
      <Flexbox style={{
        height: 250,
      }} horizontal gap={20}>
        <Flexbox style={{
          width: '25%',
          marginRight: 20,
        }}>
          <Flexbox horizontal gap={10}>
            <Flexbox>
              <div style={{ opacity: 0.5 }}>
                最近消费总额
              </div>
              <h2 style={{ marginTop: 4 }}>{renderQuota(data?.consumes?.reduce((a: number, b: any) => a + b.value, 0), 2)}</h2>
            </Flexbox>
            <Flexbox>
              <div style={{ opacity: 0.5 }}>
                令牌消耗总数
              </div>
              <h2 style={{ marginTop: 4 }}>{renderNumber(data?.totalTokenCount)}</h2>
            </Flexbox>
            <Flexbox>
              <div style={{ opacity: 0.5 }}>
                请求总数
              </div>
              <h2 style={{ marginTop: 4 }}>{renderNumber(data?.totalRequestCount)}</h2>
            </Flexbox>
          </Flexbox>

          <Flexbox gap={8} width={'100%'}>
            <div style={{ color: theme.colorTextSecondary }}>
              当前剩余额度
            </div>
            <h2 style={{ marginBlock: 0 }}>
              {renderQuota(data?.currentResidualCredit, 2)}
            </h2>
            <div />
            <DonutChart colors={colors}
              showAnimation={true}
              style={{
                width: '100%',
                padding: 10,
              }}
              valueFormatter={(v) => `${renderQuota(v, 2)}`}
              data={[{
                value: data?.currentConsumedCredit,
                name: '消费额度',
              }, {
                value: data?.currentResidualCredit,
                name: '剩余额度',
              }]} />
          </Flexbox>
        </Flexbox>
        <Flexbox style={commonFlexboxStyle}>
          <div style={{ opacity: 0.5 }}>
            最近消费总额（最近七天）
          </div>
          <h2 style={{ marginTop: 4 }}>{renderQuota(data?.consumes?.reduce((a: number, b: any) => a + b.value, 0), 6)}</h2>
          <LineChart
            categories={['消费']}
            data={consumeChart}
            valueFormatter={cunsumeValueFormatter}
            xAxisLabelFormatter={labelFormatter}
            index="date"
          />
        </Flexbox>
        <Flexbox style={commonFlexboxStyle}>
          <div style={{ opacity: 0.5 }}>
            最近请求总数（最近七天）
          </div>
          <h2 style={{ marginTop: 4 }}>{renderNumber(data?.requests?.reduce((a: number, b: any) => a + b.value, 0))}</h2>
          <LineChart
            categories={['请求数']}
            data={requestChart}
            valueFormatter={requestValueFormatter}
            xAxisLabelFormatter={labelFormatter}
            index="date"
          />
        </Flexbox>
        <Flexbox style={commonFlexboxStyle}>
          <div style={{ opacity: 0.5 }}>
            最近消耗token总数（最近七天）
          </div>
          <h2 style={{ marginTop: 4 }}>{renderNumber(data?.tokens?.reduce((a: number, b: any) => a + b.value, 0))}</h2>
          <LineChart
            categories={['令牌数']}
            data={tokenChart}
            valueFormatter={tokenValueFormatter}
            xAxisLabelFormatter={labelFormatter}
            index="date"
          />
        </Flexbox>
      </Flexbox>
      <Flexbox style={{
        marginTop: 50,
        height: '400px',
      }} horizontal gap={20}>
        <Flexbox style={{
          width: '60%',
          height: '100%',
        }}>
          <h4>
            模型消耗分布（最近七天）
          </h4>
          <LineChart
            style={{
              height: '100%',
            }}
            categories={modelsName}
            data={modelsChart}
            index="date"
            valueFormatter={modelsValueFormatter}
          />
        </Flexbox>
        <Flexbox style={{
          width: '40%',
          height: '100%',
          paddingLeft: 20,
        }}>
          <h4>
            模型费用排名（最近七天）
          </h4>
          <BarList style={{
            height: '100%',
          }}
            data={data?.modelRanking}
            showAnimation
            valueFormatter={modelsValueFormatter}
            sortOrder='descending' />
        </Flexbox>
      </Flexbox>
      {userNewData && (
        <Flexbox style={{
          height: '400px',
          width: '100%',
        }}>
          <h4>
            新用户注册（最近七天）
          </h4>
          <FunnelChart
            style={{
              height: '100%',
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
        </Flexbox>
      )}
      {rechargeData && (
        <Flexbox style={{
          height: '400px',
          width: '100%',
        }}>
          <h4>
            最近充值数据
          </h4>
          <FunnelChart
            style={{
              height: '100%',
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
        </Flexbox>
      )}
    </Flexbox>
  );
}
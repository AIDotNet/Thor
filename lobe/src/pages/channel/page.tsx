import { useEffect, useState } from "react";
import { Button, Switch, message, Tag, Dropdown, InputNumber, Table, Space, Card, Typography, Select, Row, Col, Upload } from 'antd';
import { LoadingOutlined, ReloadOutlined, PlusOutlined, SearchOutlined, DownloadOutlined, UploadOutlined } from '@ant-design/icons';
import { Remove, UpdateOrder, controlAutomatically, disable, getChannels, test, downloadImportTemplate, importChannel } from "../../services/ChannelService";
import { renderQuota } from "../../utils/render";
import { Input } from "@lobehub/ui";
import { getTypes } from "../../services/ModelService";
import CreateChannel from "./features/CreateChannel";
import UpdateChannel from "./features/UpdateChannel";
import { useTranslation } from "react-i18next";
import { getList } from "../../services/UserGroupService";
import { ConfigProvider, theme } from 'antd';
import { Flexbox } from "react-layout-kit";

const { Text, Title } = Typography;
const { Option } = Select;

export default function ChannelPage() {
  const { t } = useTranslation();
  const { token } = theme.useToken();

  const [columns, setColumns] = useState<any[]>([]);
  const [createVisible, setCreateVisible] = useState(false);
  const [updateVisible, setUpdateVisible] = useState(false);
  const [loading, setLoading] = useState(false);
  const [updateValue, setUpdateValue] = useState({} as any);
  const [data, setData] = useState<any[]>([]);
  const [total, setTotal] = useState(0);
  const [input, setInput] = useState({
    page: 1,
    pageSize: 10,
    keyword: '',
    group: '',
  });
  const [testingChannels, setTestingChannels] = useState<string[]>([]);
  const [groups, setGroups] = useState<any[]>([]);
  const [importing, setImporting] = useState(false);

  useEffect(() => {
    getList().then((res) => {
      if (res.success) {
        setGroups(res.data);
      }
    });
  }, []);

  useEffect(() => {
    setColumns([
      {
        title: t('channel.channelName'),
        dataIndex: 'name',
        fixed: 'left',
        width: 150,
      },
      {
        title: t('channel.status'),
        width: 120,
        dataIndex: 'disable',
        render: (value: any, item: any) => {
          return <Switch
            checkedChildren={<Text type="danger">{t('channel.disable')}</Text>}
            unCheckedChildren={<Text type="success">{t('channel.enable')}</Text>}
            checked={value}
            onChange={() => {
              disable(item.id)
                .then((response) => {
                  response.success 
                    ? message.success(t('channel.operationSuccess'))
                    : message.error(t('channel.operationFailed'));
                  loadingData();
                }, () => message.error(t('channel.operationFailed')));
            }}
            style={{ width: '50px' }}
          />
        }
      },
      {
        title: t('channel.autoCheck'),
        width: 120,
        dataIndex: 'controlAutomatically',
        render: (value: any, item: any) => {
          return <Switch
            onChange={() => {
              controlAutomatically(item.id)
                .then((response) => {
                  response.success 
                    ? message.success(t('channel.operationSuccess'))
                    : message.error(t('channel.operationFailed'));
                  loadingData();
                }, () => message.error(t('channel.operationFailed')));
            }}
            checkedChildren={<Text type="danger">{t('channel.disable')}</Text>}
            unCheckedChildren={<Text type="success">{t('channel.enable')}</Text>}
            checked={!value}
            style={{ width: '50px' }}
          />
        }
      },
      {
        width: 120,
        title: t('channel.channelType'),
        dataIndex: 'typeName',
        render: (value: any) => {
          return <Tag color={token.colorBgBlur}>{value}</Tag>
        }
      },
      {
        title: t('channel.responseTime'),
        width: 120,
        dataIndex: 'responseTime',
        render: (value: any, item: any) => {
          const isLoading = testingChannels.includes(item.id);
          
          if (value) {
            // Color based on response time
            let color = token.colorSuccess;
            if (value < 3000) {
              color = token.colorSuccess;
            } else if (value < 5000) {
              color = token.colorWarning;
            } else {
              color = token.colorError;
            }

            return <Tag
              color={color}
              onClick={() => !isLoading && testToken(item.id)}
              icon={isLoading ? <LoadingOutlined /> : null}
              style={{ cursor: 'pointer' }}
            >
              {isLoading ? t('channel.testing') : `${(value / 1000).toFixed(1)} ${t('channel.seconds')}`}
            </Tag>
          } else {
            return <Tag 
              onClick={() => !isLoading && testToken(item.id)}
              icon={isLoading ? <LoadingOutlined /> : null}
              style={{ cursor: 'pointer' }}
            >
              {isLoading ? t('channel.testing') : t('channel.notTested')}
            </Tag>
          }
        }
      },
      {
        title: t('channel.createdAt'),
        width: 150,
        dataIndex: 'createdAt',
      },
      {
        title: t('channel.totalConsumption'),
        dataIndex: 'quota',
        width: 120,
        render: (value: any) => {
          return <Tag color={token.colorPrimary}>{renderQuota(value, 2)}</Tag>
        }
      },
      {
        title: t('channel.quota'),
        dataIndex: 'remainQuota',
        width: 100,
        render: (value: any) => {
          return <Text
            style={{
              color: token.colorPrimary,
            }}
          >{value}</Text>
        }
      },
      {
        title: t('channel.groups'),
        dataIndex: 'groups',
        width: 150,
        render: (value: any) => {
          return (
            <Space wrap>
              {value.map((item: any, index: number) => (
                <Tag key={index} color={token.colorPrimaryBg}>{item}</Tag>
              ))}
            </Space>
          );
        }
      },
      {
        title: t('channel.channelWeight'),
        width: 120,
        dataIndex: 'order',
        render: (value: any, item: any) => {
          return <InputNumber
            onChange={(v) => {
              if (v !== null) {
                item.order = v;
                data.forEach((x: any) => {
                  if (x.id === item.id) {
                    x.order = v;
                  }
                })
                setData([...data]);
              }
            }}
            onBlur={() => {
              UpdateOrder(item.id, item.order)
                .then((response) => {
                  response.success 
                    ? message.success(t('channel.operationSuccess'))
                    : message.error(t('channel.operationFailed'));
                  loadingData();
                }, () => message.error(t('channel.operationFailed')));
            }}
            value={value}
            style={{ width: '80px' }}
            min={0}
          />
        }
      },
      {
        title: t('channel.operations'),
        fixed: 'right',
        width: 100,
        dataIndex: 'operate',
        render: (_v: any, item: any) => {
          return (
            <Dropdown
              menu={{
                items: [
                  {
                    key: 1,
                    label: t('common.edit'),
                    onClick: () => {
                      setUpdateValue(item);
                      setUpdateVisible(true);
                    }
                  },
                  {
                    key: 2,
                    label: item.disable ? t('channel.enable') : t('channel.disable'),
                    onClick: () => {
                      disable(item.id)
                        .then((response) => {
                          response.success 
                            ? message.success(t('channel.operationSuccess'))
                            : message.error(t('channel.operationFailed'));
                          loadingData();
                        }, () => message.error(t('channel.operationFailed')));
                    }
                  },
                  {
                    key: 3,
                    label: item.controlAutomatically ? t('channel.enableAutoCheck') : t('channel.disableAutoCheck'),
                    onClick: () => {
                      controlAutomatically(item.id)
                        .then((response) => {
                          response.success 
                            ? message.success(t('channel.operationSuccess'))
                            : message.error(t('channel.operationFailed'));
                          loadingData();
                        }, () => message.error(t('channel.operationFailed')));
                    }
                  },
                  {
                    key: 4,
                    label: t('common.delete'),
                    danger: true,
                    onClick: () => removeToken(item.id)
                  }
                ]
              }}
            >
              <Button type="primary">{t('common.operate')}</Button>
            </Dropdown>
          );
        },
      },
    ]);
  }, [t, testingChannels, token, data]);

  function removeToken(id: string) {
    Remove(id)
      .then((response) => {
        if (response.success) {
          message.success(t('common.deleteSuccess'));
          loadingData();
        } else {
          message.error(t('common.deleteFailed'));
        }
      });
  }

  function testToken(id: string) {
    setTestingChannels(prev => [...prev, id]);
    
    test(id)
      .then((response) => {
        if (response.success) {
          message.success(t('channel.connectionSuccess'));
          loadingData();
        } else {
          message.error(response.message || t('channel.connectionFailed'));
        }
      })
      .finally(() => {
        setTestingChannels(prev => prev.filter(channelId => channelId !== id));
      });
  }

  function loadingData() {
    setLoading(true);
    getChannels(input.page, input.pageSize, input.keyword, input.group ? [input.group] : undefined)
      .then((response: any) => {
        if (response.success) {
          const values = response.data.items as any[];
          getTypes()
            .then(res => {
              if (res.success) {
                const entries = Object.entries(res.data);
                values.forEach(x => {
                  for (const [key, value] of entries) {
                    if (value == x.type) {
                      x.typeName = key;
                      break;
                    }
                  }
                });
                setData([...values]);
                setTotal(response.data.total);
              } else {
                message.error(res.message || t('common.operateFailed'));
              }
            });
        } else {
          message.error(response.message || t('common.operateFailed'));
        }
      })
      .finally(() => {
        setLoading(false);
      });
  }

  useEffect(() => {
    loadingData();
  }, [input]);

  // 下载导入模板
  function handleDownloadTemplate() {
    downloadImportTemplate()
      .then((response) => {
        console.log(response);
        if (response.success || response instanceof Blob) {
          // 如果响应直接是 Blob 或者包含数据
          const blob = response instanceof Blob ? response : new Blob([response.data || response], { 
            type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' 
          });
          const url = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = url;
          link.download = 'channel_import_template.xlsx';
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
          window.URL.revokeObjectURL(url);
          message.success(t('channel.downloadSuccess'));
        } else {
          message.error(response.message || t('channel.downloadFailed'));
        }
      })
      .catch((error) => {
        console.log(error);
        message.error(t('channel.downloadFailed'));
      });
  }

  // 处理文件导入
  function handleImport(file: File) {
    setImporting(true);
    importChannel(file)
      .then((response) => {
        if (response.success) {
          message.success(t('channel.importSuccess'));
          loadingData(); // 重新加载数据
        } else {
          message.error(response.message || t('channel.importFailed'));
        }
      })
      .catch(() => {
        message.error(t('channel.importFailed'));
      })
      .finally(() => {
        setImporting(false);
      });
    
    return false; // 阻止默认上传行为
  }

  return (
    <ConfigProvider theme={{
      components: {
        Card: {
          headerBg: token.colorBgContainer,
          headerFontSize: 16,
          headerFontSizeSM: 14,
        },
        Table: {
          headerBg: token.colorBgContainer,
        }
      }
    }}>
      <Card 
        bordered={false}
        style={{ marginBottom: token.marginMD }}
      >
        <Flexbox gap={token.marginLG}>
          <Row gutter={[16, 16]} align="middle" style={{ width: '100%' }}>
            <Col xs={24} md={12}>
              <Title level={4} style={{ margin: 0 }}>{t('channel.title')}</Title>
            </Col>
            <Col xs={24} md={12}>
              <Space style={{ width: '100%', justifyContent: 'flex-end' }}>
                <Input 
                  value={input.keyword} 
                  onChange={(e) => {
                    setInput({
                      ...input,
                      keyword: e.target.value,
                    });
                  }} 
                  style={{ width: 200 }} 
                  placeholder={t('common.search')}
                  prefix={<SearchOutlined />}
                />
                
                <Select
                  placeholder={t('channel.selectGroups')}
                  allowClear
                  style={{ width: 200 }}
                  value={input.group || undefined}
                  onChange={(value) => {
                    setInput({
                      ...input,
                      group: value,
                    });
                  }}
                >
                  {groups.map((group) => (
                    <Option key={group.code} value={group.code}>
                      <Flexbox gap={8} horizontal>
                        <span>{group.name}</span>
                        <span style={{ fontSize: 12, color: token.colorTextSecondary }}>{group.description}</span>
                        <span style={{ fontSize: 12, color: token.colorTextSecondary }}>
                          <span>{t('channel.rate')}：</span>
                          {group.rate}
                        </span>
                      </Flexbox>
                    </Option>
                  ))}
                </Select>
                
                <Button 
                  icon={<DownloadOutlined />}
                  onClick={handleDownloadTemplate}
                >
                  {t('channel.downloadTemplate')}
                </Button>
                
                <Upload
                  accept=".xlsx,.xls"
                  showUploadList={false}
                  beforeUpload={handleImport}
                >
                  <Button 
                    icon={<UploadOutlined />}
                    loading={importing}
                  >
                    {t('channel.importTemplate')}
                  </Button>
                </Upload>
                
                <Button 
                  icon={<ReloadOutlined />}
                  onClick={() => loadingData()}
                >
                  {t('common.refresh')}
                </Button>
                
                <Button 
                  type="primary"
                  icon={<PlusOutlined />}
                  onClick={() => setCreateVisible(true)}
                >
                  {t('channel.createChannel')}
                </Button>
              </Space>
            </Col>
          </Row>
        </Flexbox>
      </Card>

      <Card bordered={false}>
        <Table
          columns={columns}
          dataSource={data}
          scroll={{
            y: 'calc(100vh - 350px)',
            x: 'max-content'
          }}
          loading={loading}
          rowKey={row => row.id}
          pagination={{
            total: total,
            pageSize: input.pageSize,
            current: input.page,
            showSizeChanger: true,
            showQuickJumper: true,
            showTotal: (total) => `${t('common.total')}: ${total}`,
            onChange: (page, pageSize) => {
              setInput({
                ...input,
                page,
                pageSize,
              });
            },
          }}
        />
      </Card>

      <CreateChannel visible={createVisible} onSuccess={() => {
        setCreateVisible(false);
        loadingData();
      }} onCancel={() => {
        setCreateVisible(false);
      }} />

      <UpdateChannel visible={updateVisible} value={updateValue} onSuccess={() => {
        setUpdateVisible(false);
        loadingData();
        setUpdateValue({} as any);
      }} onCancel={() => {
        setUpdateVisible(false);
        setUpdateValue({} as any);
      }} />
    </ConfigProvider>
  );
}
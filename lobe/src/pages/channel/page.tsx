import { useEffect, useState } from "react";
import { Button, Switch, message, Tag, Dropdown, InputNumber, Table } from 'antd';
import { Remove, UpdateOrder, controlAutomatically, disable, getChannels, test } from "../../services/ChannelService";
import { renderQuota } from "../../utils/render";
import styled from "styled-components";
import { Input } from "@lobehub/ui";
import CreateChannel from "./features/CreateChannel";
import UpdateChannel from "./features/UpdateChannel";
import { getTypes } from "../../services/ModelService";

const Header = styled.header`

`
export default function ChannelPage() {

  const columns = [
    {
      title: '名称',
      dataIndex: 'name',
      fixed: 'left',
    },
    {
      title: '是否禁用',
      width: '100px',
      dataIndex: 'disable',
      render: (value: any, item: any) => {
        return <Switch
          checkedChildren={<span style={{
            color: "red"
          }}>
            禁
          </span>}
          unCheckedChildren={<span style={{
            color: "green"
          }}>
            启
          </span>}
          value={value} onChange={() => {
            disable(item.id)
              .then((item) => {
                item.success ? message.success({
                  content: '操作成功'
                }) : message.error({
                  content: '操作失败'
                });
                loadingData();
              }), () => message.error({
                content: '操作失败'
              });
          }} style={{
            width: '50px',
          }} aria-label="a switch for semi demo"></Switch>
      }
    },
    {
      title: '自动检测',
      width: '100px',
      dataIndex: 'controlAutomatically',
      render: (value: any, item: any) => {
        return <Switch
          onChange={() => {
            controlAutomatically(item.id)
              .then((item) => {
                item.success ? message.success({
                  content: '操作成功'
                }) : message.error({
                  content: '操作失败'
                });
                loadingData();
              }), () => message.error({
                content: '操作失败'
              });
          }}
          checkedChildren={<span style={{
            color: "red"
          }}>
            禁
          </span>}
          unCheckedChildren={'启'}
          value={!value} style={{
            width: '50px',
          }} aria-label="a switch for semi demo"></Switch>
      }
    },
    {
      width: '100px',
      title: '平台类型',
      dataIndex: 'typeName',
      render: (value: any) => {
        return <Tag>{value}</Tag>
      }
    },
    {
      title: '响应时间',
      width: '100px',
      dataIndex: 'responseTime',
      render: (value: any, item: any) => {
        if (value) {
          // 小于3000毫秒显示绿色
          let color;
          if (value < 3000) {
            color = 'green';
          } else if (value < 5000) {
            color = 'yellow';
          } else {
            color = 'red';
          }

          return <Tag
            color={color as any} onClick={() => testToken(item.id)}>{value / 1000}秒</Tag>
        } else {
          return <Tag onClick={() => testToken(item.id)}>未测试</Tag>
        }
      }
    },
    {
      title: '创建时间',
      width: '150px',
      dataIndex: 'createdAt',
    },
    {
      title: '消耗总额',
      dataIndex: 'quota',
      render: (value: any) => {
        return <Tag>{renderQuota(value, 2)}</Tag>
      }
    },
    {
      title: '额度',
      dataIndex: 'remainQuota',
      render: (value: any) => {
        return <span>{value}</span>
      }
    },
    {
      title: '渠道权重',
      
      dataIndex: 'order',
      render: (value: any, item: any) => {
        return <InputNumber
          onChange={(v) => {
            item.order = v;
            data.forEach((x: any) => {
              if (x.id === item.id) {
                x.order = v;
              }
            })
            setData([...data]);
          }}
          onBlur={() => {
            UpdateOrder(item.id, item.order)
              .then((i) => {
                i.success ? message.success({
                  content: '操作成功',
                }) : message.error({
                  content: '操作失败',
                });
                loadingData();
              }), () => message.error({
                content: '操作失败',
              });
          }}

          value={value} style={{
            width: '80px',
          }}></InputNumber>
      }
    },
    {
      title: '操作',
      fixed: 'right',
      dataIndex: 'operate',
      render: (_v: any, item: any) => {
        return <>
          <Dropdown
            menu={{
              items: [
                {
                  key: 1,
                  label: '编辑',
                  onClick: () => {
                    setUpdateValue(item);
                    setUpdateVisible(true);
                  }
                },
                {
                  key: 2,
                  label: item.disable ? '启用渠道' : '禁用渠道',
                  onClick: () => {
                    disable(item.id)
                      .then((item) => {
                        item.success ? message.success({
                          content: '操作成功',
                        }) : message.error({
                          content: '操作失败',
                        });
                        loadingData();
                      }), () => message.error({
                        content: '操作失败',
                      });
                  }
                },
                {
                  key: 3,
                  label: item.controlAutomatically ? '禁用自动检测' : '启用自动检测',
                  onClick: () => {
                    controlAutomatically(item.id)
                      .then((item) => {
                        item.success ? message.success({
                          content: '操作成功',
                        }) : message.error({
                          content: '操作失败',
                        });
                        loadingData();
                      }), () => message.error({
                        content: '操作失败',
                      });
                  }
                },
                {
                  key: 4,
                  label: '删除',
                  onClick: () => removeToken(item.id)
                }
              ] as any[]
            }
            }
          >
            <Button >操作</Button>
          </Dropdown>
        </>;
      },
    },
  ];
  const [createVisible, setCreateVisible] = useState(false);
  const [updateVisible, setUpdateVisible] = useState(false);
  const [updateValue, setUpdateValue] = useState({} as any);
  const [data, setData] = useState<any[]>([]);
  const [total, setTotal] = useState(0);
  const [input, setInput] = useState({
    page: 1,
    pageSize: 10,
    keyword: '',
  });

  function removeToken(id: string) {
    Remove(id)
      .then((v) => {
        if (v.success) {
          loadingData();
        } else {
          message.error({
            content: '删除失败',
          })
        }
      })
  }

  function testToken(id: string) {
    test(id)
      .then((v) => {
        if (v.success) {
          message.success({
            content: '测试成功',
          })
          loadingData();
        } else {
          message.error({
            content: v.message,
          })
        }
      })
  }

  function loadingData() {
    getChannels(input.page, input.pageSize)
      .then((v: any) => {
        if (v.success) {
          const values = v.data.items as any[];
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
                })
                setData([...values]);
                setTotal(v.data.total);
              } else {
                message.error({
                  content: res.message
                });
              }
            })
        } else {
          message.error({
            content: v.message,
          });
        }
      })
  }

  useEffect(() => {
    loadingData();
  }, [input]);


  return (
    <>

      <Header>
        <span style={{
          fontSize: '1.5rem',
          fontWeight: 'bold',
        }}>
          渠道管理
        </span>

        <Dropdown
          menu={{
            items: [
              {
                key: 1,
                label: "创建渠道",
                onClick: () => setCreateVisible(true)
              }
            ]
          }}
        >
          <Button style={{
            float: 'right',
          }}>操作</Button>
        </Dropdown>
        <Button style={{
          marginRight: '0.5rem',
          float: 'right',
        }}>搜索</Button>
        <Input value={input.keyword} onChange={(v) => {
          setInput({
            ...input,
            keyword: v.target.value,
          });
        }} style={{
          width: '150px',
          float: 'right',
          marginRight: '1rem',
        }} placeholder='搜索关键字'></Input>
      </Header>
      <Table
        style={{
          marginTop: '1rem',
        }}
        columns={columns as any}
        dataSource={data}
        scroll={{
          y: 'calc(100vh - 350px)',
          x: 'max-content'
        }}
        rowKey={row => row.id}
        pagination={{
          total: total,
          pageSize: input.pageSize,
          defaultPageSize: input.page,
          onChange: (page, pageSize) => {
            setInput({
              ...input,
              page,
              pageSize,
            });
          },

        }} />
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
    </>
  );
}
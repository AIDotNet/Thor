import { useMemo, useState } from "react";
import styled from "styled-components"
import { disable, getTokens, Remove } from '../../services/TokenService'
import { Switch, Dropdown, message, Button, Table } from 'antd'
import { renderQuota } from "../../utils/render";
import { Input, Tag } from "@lobehub/ui";
import CreateToken from "./features/CreateToken";
import UpdateToken from "./features/UpdateToken";

const Header = styled.header`

`

export default function TokenPage() {
  const columns = [
    {
      title: '名称',
      dataIndex: 'name',
      fixed: 'left',
    },
    {
      title: '是否禁用',
      dataIndex: 'disabled',
      render: (value: any, item: any) => {
        return <Switch
          unCheckedChildren={<span style={{
            color: "red"
          }}>
            禁
          </span>}
          checkedChildren={<span style={{
            color: "green"
          }}>
            启
          </span>}
          value={!value} onChange={() => {
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
          }} style={{
            width: '50px',
          }} ></Switch>
      }
    },
    {
      title: '已用额度',
      dataIndex: 'usedQuota',
      render: (value: any) => {
        return (renderQuota(value, 6))
      }
    },
    {
      title: '剩余额度',
      dataIndex: 'remainingundrawn',
      render: (_v: any, item: any) => {
        if (item.unlimitedQuota) {
          return <span>不限制</span>
        } else {
          return <span>{renderQuota(item.remainQuota - item.usedQuota, 6)}</span>
        }
      }
    },
    {
      title: '创建时间',
      dataIndex: 'createdAt',
    },
    {
      title: '最近使用时间',
      dataIndex: 'accessedTime',
    },
    {
      title: '过期时间',
      dataIndex: 'expiredTime',
      render: (value: any, item: any) => {
        if (item.unlimitedExpired) {
          return (<span>不过期</span>)
        } else {
          return (<span>{value}</span>)
        }
      }
    },
    {
      title: '操作',
      dataIndex: 'operate',
      fixed: 'right',
      render: (_v: any, item: any) => {
        return <>
          <Dropdown
            menu={{
              items: [
                {
                  key: 'lobeChat',
                  label: <Tag>绑定LobeChat</Tag>,
                  onClick: () => bingLobeChat(item.key)
                },
                {
                  key: 'edit',
                  label: '编辑',
                  onClick: () => {
                    setUpdateTokenVisible(true);
                    setUpdateTokenValue(item);
                  }
                },
                {
                  key: 'copy',
                  label: '复制Key',
                  onClick: () => copyKey(item.key)
                },
                {
                  key: 'disable',
                  label: item.disabled ? '启用' : '禁用',
                  onClick: () => {
                    disable(item.id).then((item) => {
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
                  key: 'show',
                  label: '查看',
                  onClick: () => {
                    message.info({
                      content: item.key,
                    })
                  }
                },
                {
                  key: 'delete',
                  label: '删除',
                  onClick: () => removeToken(item.id)
                }
              ]
            }}
          >
            <Button >操作</Button>
          </Dropdown>
        </>;
      },
    },
  ];
  const [createTokenVisible, setCreateTokenVisible] = useState(false);
  const [updateTokenVisible, setUpdateTokenVisible] = useState(false);
  const [updateTokenValue, setUpdateTokenValue] = useState({} as any);
  const [data, setData] = useState([]);
  const [total, setTotal] = useState(0);
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);
  const [input, setInput] = useState({
    page: 1,
    pageSize: 10,
    token: '',
    keyword: '',
  });

  function copyKey(key: string) {
    try {

      navigator.clipboard.writeText(key).then(() => {
        message.success({
          content: '复制成功',
        })
      }).catch(() => {
        message.error({
          content: '复制失败 token:' + key,
        })
      });
    } catch (e) {
      message.error({
        content: '复制失败 token:' + key,
      })
    }
  }

  function bingLobeChat(token: string) {
    const json = JSON.stringify({
      keyVaults: {
        openai: {
          apiKey: token,
          baseURL: window.location.origin + '/v1',
        }
      }
    });
    window.open(`https://lobe-chat.ai-v1.cn?settings=${json}`);
  }

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

  function loadingData() {
    getTokens(input.page, input.pageSize).then((res) => {
      if (res.success) {
        setData(res.data.items);
        setTotal(res.data.total);
      }
    });
  }

  useMemo(() => {
    loadingData();
  }, [input]);

  const rowSelection = {
    selectedRowKeys,
    onChange: (selectedRowKeys: any) => {
      setSelectedRowKeys(selectedRowKeys);
    },
  };

  return (
    <div style={{
      height: '80vh',
      overflow: 'auto',
      width: '100%',
    }}>
      <Header>
        <span style={{
          fontSize: '1.5rem',
          fontWeight: 'bold',
        }}>
          Token管理
        </span>

        <Dropdown
          menu={{
            items: [
              {
                key: 1,
                label: '创建Token',
                onClick: () => setCreateTokenVisible(true)
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
        <Input value={input.token} onChange={(e) => {
          setInput({
            ...input,
            token: e.target.value,
          });
        }} style={{
          width: '200px',
          float: 'right',
          marginRight: '1rem',
        }} placeholder='搜索Token'></Input>
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
      <Table style={{
        marginTop: '1rem',
      }}
        scroll={{
          x: 'max-content',
          y: 'calc(100vh - 350px)',
        }}
        columns={columns as any[]}
        dataSource={data}
        rowKey={(row: any) => row.id}
        rowSelection={rowSelection}
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
      <CreateToken visible={createTokenVisible}
        onSuccess={() => {
          setCreateTokenVisible(false);
          loadingData();
        }}
        onCancel={() => {
          setCreateTokenVisible(false);
        }} />
      <UpdateToken visible={updateTokenVisible}
        value={updateTokenValue}
        onSuccess={() => {
          setUpdateTokenVisible(false);
          loadingData();
          setUpdateTokenValue({});
        }}
        onCancel={() => {
          setUpdateTokenVisible(false);
          setUpdateTokenValue({});
        }} />
    </div>
  )
}
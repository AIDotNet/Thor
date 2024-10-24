import { useMemo, useState } from "react";
import styled from "styled-components"
import { Button, Dropdown, Input, message, Switch, Table, Tag } from 'antd';
import { Enable, Remove, getRedeemCodes } from "../../services/RedeemCodeService";
import { renderQuota } from "../../utils/render";
import CreateRedeemCode from "./features/CreateRedeemCode";
import UpdateRedeemCode from "./features/UpdateRedeemCode";

const Header = styled.header`

`

export default function RedeemCode() {
  const columns = [
    {
      title: '名称',
      dataIndex: 'name',
    },
    {
      title: '是否禁用',
      dataIndex: 'disabled',
      render: (value: any, item: any) => {
        return <Switch
          defaultChecked={!value} onChange={() => {
            Enable(item.id)
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
          }} aria-label="a switch for semi demo"></Switch>
      }
    },
    {
      title: '额度',
      dataIndex: 'quota',
      render: (value: any) => {
        return <span>{renderQuota(value, 2)}</span>
      }
    },
    {
      title: '创建时间',
      dataIndex: 'createdAt',
    },
    {
      title: '兑换人',
      dataIndex: 'redeemedUserName',
      render: (value: any) => {
        if (value) {
          return <Tag color='green'>{value}</Tag>;
        } else {
          return '暂无';
        }
      }
    },
    {
      title: '兑换时间',
      dataIndex: 'redeemedTime',
      render: (value: any) => {
        if (value) {
          return value;
        } else {
          return '未兑换';
        }
      }
    },
    {
      title: '操作',
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
                    setUpdateTokenVisible(true);
                    setUpdateTokenValue(item);
                  }
                },
                {
                  key: 2,
                  label: '复制',
                  onClick: () => {
                    copyKey(item.code);
                  }
                },
                {
                  key: 3,
                  label: '删除',
                  onClick: () => removeRedeemCode(item.id)
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
          content: '复制失败:' + key,
          duration: 10,
        })
      });
    } catch (e) {
      message.error({
        content: '复制失败:' + key,
        duration: 10,
      })
    }
  }

  function removeRedeemCode(id: number) {
    Remove(id)
      .then((v) => {
        if (v.success) {
          loadingData();
          message.success({
            content: '删除成功',
          })
        } else {
          message.error({
            content: '删除失败',
          })
        }
      })
  }

  function loadingData() {
    getRedeemCodes(input.page, input.pageSize, input.keyword).then((res) => {
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
      margin: '20px',
      width: '100%',
      height: '100%',
    }}>
      <Header>
        <span style={{
          fontSize: '1.5rem',
          fontWeight: 'bold',
        }}>
          兑换码管理
        </span>

        <Dropdown
          menu={{
            items: [
              {
                key: 1,
                label: '创建令牌',
                onClick: () => {
                  setCreateTokenVisible(true)
                }
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
      <Table style={{
        marginTop: '1rem',
      }} columns={columns} dataSource={data} rowSelection={rowSelection} pagination={{
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
      <CreateRedeemCode visible={createTokenVisible} onCancel={() =>
        setCreateTokenVisible(false)} onSuccess={() => {
          setCreateTokenVisible(false);
          loadingData();
        }} />
      <UpdateRedeemCode value={updateTokenValue} visible={updateTokenVisible} onCancel={() => {
        setUpdateTokenValue({} as any);
        setUpdateTokenVisible(false)
      }} onSuccess={() => {
        setUpdateTokenVisible(false);
        setUpdateTokenValue({} as any);
        loadingData();
      }} />
    </div>
  )
}
import { useEffect, useState } from "react";
import styled from "styled-components";
import { enable, getUsers, Remove } from "../../services/UserService";
import { Switch, Tag, Tooltip, Button, Dropdown, Input, Table, message } from 'antd';
import { renderQuota } from "../../utils/render";
import CreateUser from "./features/CreateUser";
import EditUser from "./features/EditUser";


const Header = styled.header`

`
export default function Channel() {
  const [data, setData] = useState<any[]>([]);
  const [total, setTotal] = useState<number>(0);
  const [loading, setLoading] = useState<boolean>(false);
  const [createVisible, setCreateVisible] = useState<boolean>(false);
  const [editVisible, setEditVisible] = useState<boolean>(false);
  const [currentUser, setCurrentUser] = useState<any>(null);
  const [input, setInput] = useState({
    page: 1,
    pageSize: 10,
    keyword: ''
  });

  const columns = [
    {
      title: '名称',
      dataIndex: 'userName',
      key: 'userName'
    },
    {
      title: '邮箱',
      dataIndex: 'email',
      key: 'email'
    },
    {
      title: '角色',
      dataIndex: 'role',
      key: 'role'
    },
    {
      title: '分组',
      dataIndex: 'groups',
      key: 'groups',
      render: (value: string[]) => {
        return value.map((item: string) => {
          return <Tag key={item}>{item}</Tag>
        })
      }
    },
    {
      title: '状态',
      dataIndex: 'isDisabled',
      key: 'isDisabled',
      render: (value: any, item: any) => {
        return <Switch
          value={value} 
          unCheckedChildren={'启'}
          checkedChildren={<span style={{
            color: "red"
          }}>
            禁
          </span>}
          onChange={() => {
            enable(item.id)
              .then((item) => {
                item.success ? message.success({
                  content: '操作成功',
                }) : message.error({
                  content: '操作失败',
                });
                loadData();
              }), () => message.error({
                content: '操作失败',
              });
          }} style={{
            width: '50px',
          }} aria-label="a switch for semi demo"></Switch>
      }
    },
    {
      title: '统计',
      dataIndex: 'statics',
      key: 'statics',
      render: (_value: any, item: any) => {
        return <>
          <Tooltip title="消耗的token">
            <Tag color='blue' style={{
              marginRight: '0.5rem'
            }}>{item.consumeToken}</Tag>
          </Tooltip>
          <Tooltip title="请求总数">
            <Tag color='blue' style={{
              marginRight: '0.5rem'
            }}>{item.requestCount}</Tag>
          </Tooltip>
          <Tooltip title="剩余额度">
            <Tag color='blue' style={{
              marginRight: '0.5rem'
            }}>{renderQuota(item.residualCredit, 6)}</Tag>
          </Tooltip>
        </>;
      }
    },
    {
      title: '创建时间',
      dataIndex: 'createdAt',
      key: 'createdAt'
    },
    {
      title: '操作',
      key: 'action',
      render: (_text: any, item: any) => (
        <Dropdown
          menu={{
            items: [
              {
                key: 1,
                label: '编辑',
                onClick: () => openEditUser(item)
              },
              {
                key: 2,
                label: '删除',
                onClick: () => removeUser(item.id)
              }
            ]
          }}
        >
          <Button>操作</Button>
        </Dropdown>
      ),
    }
  ]

  function openEditUser(user: any) {
    setCurrentUser(user);
    setEditVisible(true);
  }

  function removeUser(id: string) {
    Remove(id).then((res) => {
      if (res.success) {
        loadData();
      } else {
        message.error({
          content: res.message
        });
      }
    }
    )
  }

  function loadData() {
    setLoading(true);
    getUsers(input.page, input.pageSize, input.keyword)
      .then((res) => {
        if (res.success) {
          setData(res.data.items);
          setTotal(res.data.total);
        }
      })
      .finally(() => setLoading(false));

    setLoading(false);
  }

  useEffect(() => {
    loadData();
  }, [input]);

  return (
    <div style={{
      margin: '10px',
      height: '80vh',
      overflow: 'auto',
      width: '100%',
    }}>
      <Header>
        <span style={{
          fontSize: '1.5rem',
          fontWeight: 'bold',
        }}>
          用户管理
        </span>

        <Dropdown
          menu={{
            items: [
              {
                key: 1,
                label: '创建用户',
                onClick: () => {
                  setCreateVisible(true);
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
      <Table loading={loading} style={{
        marginTop: '1rem',
      }} columns={columns} dataSource={data} pagination={{
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
      <CreateUser onSuccess={() => {
        setCreateVisible(false);
        loadData();
      }} visible={createVisible} onCancel={() => {
        setCreateVisible(false);
      }
      } />
      <EditUser 
        visible={editVisible} 
        user={currentUser}
        onCancel={() => {
          setEditVisible(false);
        }} 
        onSuccess={() => {
          setEditVisible(false);
          loadData();
        }} 
      />
    </div>
  )
}
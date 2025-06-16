import { useEffect, useState } from "react";
import styled from "styled-components";
import { getAnnouncements, deleteAnnouncement, toggleAnnouncement } from "../../services/AnnouncementService";
import { Switch, Tag, Button, Dropdown, Input, Table, message, Space } from 'antd';
import CreateAnnouncement from "./features/CreateAnnouncement";
import EditAnnouncement from "./features/EditAnnouncement";

const Header = styled.header`
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16px;
`;

const { Search } = Input;

export default function Announcement() {
  const [data, setData] = useState<any[]>([]);
  const [total, setTotal] = useState<number>(0);
  const [loading, setLoading] = useState<boolean>(false);
  const [createVisible, setCreateVisible] = useState<boolean>(false);
  const [editVisible, setEditVisible] = useState<boolean>(false);
  const [currentAnnouncement, setCurrentAnnouncement] = useState<any>(null);
  const [input, setInput] = useState({
    page: 1,
    pageSize: 10,
    keyword: ''
  });

  const columns = [
    {
      title: '标题',
      dataIndex: 'title',
      key: 'title',
      ellipsis: true,
    },
    {
      title: '类型',
      dataIndex: 'type',
      key: 'type',
      render: (type: string) => {
        const colorMap: { [key: string]: string } = {
          'info': 'blue',
          'success': 'green',
          'warning': 'orange',
          'error': 'red'
        };
        return <Tag color={colorMap[type] || 'blue'}>{type}</Tag>;
      }
    },
    {
      title: '状态',
      dataIndex: 'enabled',
      key: 'enabled',
      render: (enabled: boolean, item: any) => {
        return (
          <Switch
            checked={enabled}
            checkedChildren="启用"
            unCheckedChildren="禁用"
            onChange={(checked) => handleToggle(item.id, checked)}
          />
        );
      }
    },
    {
      title: '置顶',
      dataIndex: 'pinned',
      key: 'pinned',
      render: (pinned: boolean) => {
        return pinned ? <Tag color="red">置顶</Tag> : <Tag>普通</Tag>;
      }
    },
    {
      title: '排序权重',
      dataIndex: 'order',
      key: 'order',
    },
    {
      title: '过期时间',
      dataIndex: 'expireTime',
      key: 'expireTime',
      render: (expireTime: string) => {
        if (!expireTime) return '永不过期';
        return new Date(expireTime).toLocaleString();
      }
    },
    {
      title: '创建时间',
      dataIndex: 'createdAt',
      key: 'createdAt',
      render: (createdAt: string) => {
        return new Date(createdAt).toLocaleString();
      }
    },
    {
      title: '操作',
      key: 'action',
      render: (_text: any, item: any) => (
        <Space>
          <Button size="small" onClick={() => openEditAnnouncement(item)}>
            编辑
          </Button>
          <Button size="small" danger onClick={() => handleDelete(item.id)}>
            删除
          </Button>
        </Space>
      ),
    }
  ];

  function openEditAnnouncement(announcement: any) {
    setCurrentAnnouncement(announcement);
    setEditVisible(true);
  }

  function handleDelete(id: string) {
    deleteAnnouncement(id).then((res) => {
      if (res.success) {
        message.success('删除成功');
        loadData();
      } else {
        message.error(res.message || '删除失败');
      }
    });
  }

  function handleToggle(id: string, enabled: boolean) {
    toggleAnnouncement(id, enabled).then((res) => {
      if (res.success) {
        message.success('操作成功');
        loadData();
      } else {
        message.error(res.message || '操作失败');
      }
    });
  }

  function loadData() {
    setLoading(true);
    getAnnouncements(input.page, input.pageSize, input.keyword)
      .then((res) => {
        if (res.success) {
          setData(res.data.items);
          setTotal(res.data.total);
        } else {
          message.error(res.message || '加载失败');
        }
      })
      .finally(() => setLoading(false));
  }

  function handleSearch(value: string) {
    setInput({
      ...input,
      keyword: value,
      page: 1
    });
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
          公告管理
        </span>
        <Space>
          <Search
            placeholder="搜索公告标题或内容"
            allowClear
            enterButton="搜索"
            size="middle"
            onSearch={handleSearch}
          />
          <Button type="primary" onClick={() => setCreateVisible(true)}>
            创建公告
          </Button>
        </Space>
      </Header>

      <Table
        columns={columns}
        dataSource={data}
        loading={loading}
        rowKey="id"
        pagination={{
          current: input.page,
          pageSize: input.pageSize,
          total: total,
          showSizeChanger: true,
          showQuickJumper: true,
          showTotal: (total, range) => `第 ${range[0]}-${range[1]} 条/共 ${total} 条`,
          onChange: (page, pageSize) => {
            setInput({
              ...input,
              page,
              pageSize: pageSize || input.pageSize
            });
          }
        }}
      />

      <CreateAnnouncement
        visible={createVisible}
        onCancel={() => setCreateVisible(false)}
        onSuccess={() => {
          setCreateVisible(false);
          loadData();
        }}
      />

      <EditAnnouncement
        visible={editVisible}
        announcement={currentAnnouncement}
        onCancel={() => {
          setEditVisible(false);
          setCurrentAnnouncement(null);
        }}
        onSuccess={() => {
          setEditVisible(false);
          setCurrentAnnouncement(null);
          loadData();
        }}
      />
    </div>
  );
} 
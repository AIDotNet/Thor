import { useEffect, useState } from "react";
import { Button, Table, message, Tag, Dropdown, Space, List } from 'antd';
import { deleteModelMap, getModelMapList, ModelMap } from "../../services/ModelMapService";
import { Input } from "@lobehub/ui";
import CreateModelMap from "./features/CreateModelMap";
import UpdateModelMap from "./features/UpdateModelMap";

export default function ModelMapPage() {
  const [createVisible, setCreateVisible] = useState(false);
  const [updateVisible, setUpdateVisible] = useState(false);
  const [loading, setLoading] = useState(false);
  const [updateValue, setUpdateValue] = useState<ModelMap | null>(null);
  const [data, setData] = useState<ModelMap[]>([]);
  const [keyword, setKeyword] = useState('');

  const columns = [
    {
      title: '模型ID',
      dataIndex: 'modelId',
    },
    {
      title: '分组',
      dataIndex: 'group',
      render: (groups: string[]) => (
        <Space>
          {groups.map(group => (
            <Tag key={group}>{group}</Tag>
          ))}
        </Space>
      )
    },
    {
      title: '创建时间',
      dataIndex: 'createdAt',
      width: '180px',
    },
    {
      title: '更新时间',
      dataIndex: 'updatedAt',
      width: '180px',
    },
    {
      title: '映射项数量',
      dataIndex: 'modelMapItems',
      width: '100px',
      render: (items: any[]) => items.length
    },
    {
      title: '操作',
      fixed: 'right' as const,
      width: '120px',
      render: (_: any, record: ModelMap) => (
        <Dropdown
          menu={{
            items: [
              {
                key: '1',
                label: '编辑',
                onClick: () => {
                  setUpdateValue(record);
                  setUpdateVisible(true);
                }
              },
              {
                key: '2',
                label: '删除',
                onClick: () => handleDelete(record.id as string)
              }
            ]
          }}
        >
          <Button>操作</Button>
        </Dropdown>
      ),
    },
  ];

  const expandedRowRender = (record: ModelMap) => {
    return (
      <List
        size="small"
        bordered
        dataSource={record.modelMapItems}
        renderItem={(item: any) => (
          <List.Item>
            <div style={{ display: 'flex', width: '100%', justifyContent: 'space-between' }}>
              <div><strong style={{ marginRight: '8px' }}>模型:</strong>
                <Tag color="blue"
                  onClick={() => {
                    // 复制
                    navigator.clipboard.writeText(item.modelId)
                      .then(() => {
                        message.success('复制成功');
                      })
                      .catch(() => {
                        const input = document.createElement('input');
                        input.value = item.modelId;
                        document.body.appendChild(input);
                        input.select();
                        document.execCommand('copy');
                        document.body.removeChild(input);
                        message.success('复制成功');
                      });
                  }}
                  style={{ cursor: 'pointer' }}
                >{item.modelId}</Tag>
              </div>
              <div><strong style={{ marginRight: '8px' }}>排序:</strong>
                <Tag color="green">{item.order}</Tag>
              </div>
            </div>
          </List.Item>
        )}
      />
    );
  };

  const handleDelete = (id: string) => {
    deleteModelMap(id)
      .then(response => {
        if (response.success) {
          message.success('删除成功');
          loadData();
        } else {
          message.error(response.message || '删除失败');
        }
      })
      .catch(() => {
        message.error('操作失败');
      });
  };

  const loadData = () => {
    setLoading(true);
    getModelMapList()
      .then((response: any) => {
        if (response.success) {
          setData(response.data);
        } else {
          message.error(response.message || '加载数据失败');
        }
      })
      .catch(() => {
        message.error('加载数据失败');
      })
      .finally(() => {
        setLoading(false);
      });
  };

  useEffect(() => {
    loadData();
  }, []);

  const filteredData = data.filter(item => {
    if (!keyword) return true;
    return (
      item.id?.toLowerCase().includes(keyword.toLowerCase()) ||
      item.modelId?.toLowerCase().includes(keyword.toLowerCase()) ||
      item.group?.some(g => g.toLowerCase().includes(keyword.toLowerCase()))
    );
  });

  return (
    <>
      <div style={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        marginBottom: '16px'
      }}>
        <span style={{
          fontSize: '1.5rem',
          fontWeight: 'bold',
        }}>
          模型映射管理
        </span>

        <div>
          <Input
            value={keyword}
            onChange={(e) => setKeyword(e.target.value)}
            placeholder="搜索关键字"
            style={{ width: '200px', marginRight: '16px' }}
          />

          <Button
            onClick={loadData}
            style={{ marginRight: '16px' }}
          >
            刷新
          </Button>

          <Button
            type="primary"
            onClick={() => setCreateVisible(true)}
          >
            创建映射
          </Button>
        </div>
      </div>

      <Table
        columns={columns}
        dataSource={filteredData}
        rowKey="id"
        loading={loading}
        scroll={{ x: 'max-content', y: 'calc(100vh - 350px)' }}
        pagination={{ pageSize: 10 }}
        expandable={{
          expandedRowRender,
          rowExpandable: record => record.modelMapItems && record.modelMapItems.length > 0,
        }}
      />

      <CreateModelMap
        visible={createVisible}
        onSuccess={() => {
          setCreateVisible(false);
          loadData();
        }}
        onCancel={() => setCreateVisible(false)}
      />

      {updateValue && (
        <UpdateModelMap
          visible={updateVisible}
          value={updateValue}
          onSuccess={() => {
            setUpdateVisible(false);
            setUpdateValue(null);
            loadData();
          }}
          onCancel={() => {
            setUpdateVisible(false);
            setUpdateValue(null);
          }}
        />
      )}
    </>
  );
} 
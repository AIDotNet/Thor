import { useEffect, useState } from "react";
import { 
  Button, 
  Table, 
  message, 
  Tag, 
  Space, 
  Card, 
  Typography, 
  Modal, 
  Flex, 
  ConfigProvider
} from 'antd';
import { DeleteOutlined, EditOutlined, PlusOutlined, ReloadOutlined, SearchOutlined } from '@ant-design/icons';
import { deleteModelMap, getModelMapList, ModelMap } from "../../services/ModelMapService";
import { Input } from "@lobehub/ui";
import CreateModelMap from "./features/CreateModelMap";
import UpdateModelMap from "./features/UpdateModelMap";
import { useTranslation } from "react-i18next";
import { useTheme } from "antd-style";

export default function ModelMapPage() {
  const { t } = useTranslation();
  const theme = useTheme();
  const [createVisible, setCreateVisible] = useState(false);
  const [updateVisible, setUpdateVisible] = useState(false);
  const [loading, setLoading] = useState(false);
  const [updateValue, setUpdateValue] = useState<ModelMap | null>(null);
  const [data, setData] = useState<ModelMap[]>([]);
  const [keyword, setKeyword] = useState('');
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [selectedId, setSelectedId] = useState<string>('');

  const columns = [
    {
      title: t('modelMap.modelId'),
      dataIndex: 'modelId',
      key: 'modelId',
    },
    {
      title: t('modelMap.group'),
      dataIndex: 'group',
      key: 'group',
      render: (groups: string[]) => (
        <Space wrap>
          {groups.map(group => (
            <Tag key={group} color={theme.colorPrimary}>
              {group}
            </Tag>
          ))}
        </Space>
      )
    },
    {
      title: t('modelMap.createdAt'),
      dataIndex: 'createdAt',
      key: 'createdAt',
      width: '180px',
    },
    {
      title: t('modelMap.updatedAt'),
      dataIndex: 'updatedAt',
      key: 'updatedAt',
      width: '180px',
    },
    {
      title: t('modelMap.itemCount'),
      dataIndex: 'modelMapItems',
      key: 'modelMapItems',
      width: '120px',
      render: (items: any[]) => items.length
    },
    {
      title: t('modelMap.actions'),
      key: 'actions',
      fixed: 'right' as const,
      width: '120px',
      render: (_: any, record: ModelMap) => (
        <Space>
          <Button 
            type="text" 
            icon={<EditOutlined />} 
            onClick={() => {
              setUpdateValue(record);
              setUpdateVisible(true);
            }}
          />
          <Button 
            type="text" 
            danger 
            icon={<DeleteOutlined />} 
            onClick={() => {
              setSelectedId(record.id as string);
              setDeleteModalOpen(true);
            }}
          />
        </Space>
      ),
    },
  ];

  const handleDelete = () => {
    setLoading(true);
    deleteModelMap(selectedId)
      .then(response => {
        if (response.success) {
          message.success(t('modelMap.deleteSuccess'));
          loadData();
        } else {
          message.error(response.message || t('modelMap.deleteFailed'));
        }
      })
      .catch(() => {
        message.error(t('common.operateFailed'));
      })
      .finally(() => {
        setLoading(false);
        setDeleteModalOpen(false);
      });
  };

  const loadData = () => {
    setLoading(true);
    getModelMapList()
      .then((response: any) => {
        if (response.success) {
          setData(response.data);
        } else {
          message.error(response.message || t('modelMap.loadError'));
        }
      })
      .catch(() => {
        message.error(t('modelMap.loadError'));
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
    <ConfigProvider
      theme={{
        components: {
          Table: {
            headerBg: theme.colorBgContainer,
            headerColor: theme.colorTextLabel,
            borderColor: theme.colorBorderSecondary,
            headerSplitColor: theme.colorBorderSecondary,
          }
        }
      }}
    >
      <Card bordered={false}>
        <Flex justify="space-between" align="center" style={{ marginBottom: theme.marginLG }}>
          <Typography.Title level={4} style={{ margin: 0 }}>
            {t('modelMap.title')}
          </Typography.Title>

          <Space>
            <Input 
              value={keyword} 
              onChange={(e) => setKeyword(e.target.value)} 
              placeholder={t('modelMap.searchPlaceholder')}
              size="middle"
              prefix={<SearchOutlined style={{ color: theme.colorTextSecondary }} />}
              style={{ width: '240px' }}
            />

            <Button 
              icon={<ReloadOutlined />}
              onClick={loadData}
            >
              {t('modelMap.refresh')}
            </Button>

            <Button 
              type="primary"
              icon={<PlusOutlined />}
              onClick={() => setCreateVisible(true)}
            >
              {t('modelMap.createMap')}
            </Button>
          </Space>
        </Flex>

        <Table
          columns={columns}
          dataSource={filteredData}
          rowKey="id"
          loading={loading}
          scroll={{ x: 'max-content' }}
          pagination={{ 
            pageSize: 10,
            showSizeChanger: true,
            showTotal: (total) => `${total} items`
          }}
        />

        <Modal
          title={t('modelMap.deleteMap')}
          open={deleteModalOpen}
          onOk={handleDelete}
          onCancel={() => setDeleteModalOpen(false)}
          okText={t('common.confirm')}
          cancelText={t('common.cancel')}
          okButtonProps={{ danger: true, loading: loading }}
        >
          <Typography.Text>
            {t('modelMap.deleteConfirm')}
          </Typography.Text>
        </Modal>

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
      </Card>
    </ConfigProvider>
  );
} 
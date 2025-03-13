import { useEffect, useState } from "react";
import { Button, Dropdown, message, Table, Space, Input as AntInput } from "antd";
import { Header, Tag } from "@lobehub/ui";
import { enableUserGroup, getList, remove } from "../../services/UserGroupService";
import CreateUserGroupPage from "./features/CreateUserGroup";
import UpdateUserGroupPage from "./features/UpdateUserGroup";

export default function UserGroupPage() {
    const [createOpen, setCreateOpen] = useState<boolean>(false);
    const [updateValue, setUpdateValue] = useState<any>({
        value: {},
        open: false
    });
    const [data, setData] = useState<any[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [input, setInput] = useState({
        keyword: '',
    });

    function loadData() {
        setLoading(true);
        getList()
            .then((res) => {
                if (res.success) {
                    let filteredData = res.data;
                    if (input.keyword) {
                        filteredData = filteredData.filter((item: any) => 
                            item.name.includes(input.keyword) || 
                            item.code.includes(input.keyword) || 
                            item.description.includes(input.keyword)
                        );
                    }
                    setData(filteredData);
                } else {
                    message.error(res.message || '获取用户分组列表失败');
                }
            }).finally(() => {
                setLoading(false);
            });
    }

    useEffect(() => {
        loadData();
    }, []);

    return (
        <div>
            <Header
                nav={'用户分组管理'}
                actions={
                    <Space>
                        <AntInput.Search
                            placeholder="请输入名称或编码搜索"
                            value={input.keyword}
                            onChange={(e) => setInput({ ...input, keyword: e.target.value })}
                            onSearch={() => loadData()}
                            style={{ width: 200 }}
                        />
                        <Button type="primary" onClick={() => setCreateOpen(true)}>
                            新增分组
                        </Button>
                    </Space>
                }
            />
            <Table
                rowKey={row => row.id}
                loading={loading}
                dataSource={data}
                scroll={{
                    y: 'calc(100vh - 340px)',
                    x: 'max-content'
                }}
                columns={[
                    {
                        key: 'name',
                        title: '名称',
                        fixed: 'left',
                        dataIndex: 'name'
                    },
                    {
                        key: 'code',
                        title: '唯一编码',
                        dataIndex: 'code'
                    },
                    {
                        key: 'description',
                        title: '描述',
                        dataIndex: 'description'
                    },
                    {
                        key: 'rate',
                        title: '分组倍率',
                        dataIndex: 'rate',
                        render: (value: number) => <Tag color='blue'>{value}x</Tag>
                    },
                    {
                        key: 'order',
                        title: '排序',
                        dataIndex: 'order'
                    },
                    {
                        key: 'enable',
                        title: '状态',
                        dataIndex: 'enable',
                        render: (value: boolean) => (
                            <Tag color={value ? 'green' : 'red'}>
                                {value ? '启用' : '禁用'}
                            </Tag>
                        )
                    },
                    {
                        key: 'actions',
                        title: '操作',
                        fixed: 'right',
                        render: (_: any, item: any) => (
                            <Dropdown
                                menu={{
                                    items: [
                                        {
                                            key: 'edit',
                                            label: '编辑',
                                            onClick: () => setUpdateValue({ value: item, open: true })
                                        },
                                        {
                                            key: "enable",
                                            label: item.enable ? '禁用' : '启用',
                                            onClick: () => {
                                                enableUserGroup(item.id, !item.enable)
                                                    .then((res) => {
                                                        if (res.success) {
                                                            message.success('操作成功');
                                                            loadData();
                                                        } else {
                                                            message.error(res.message || '操作失败');
                                                        }
                                                    });
                                            }
                                        },
                                        {
                                            key: 'delete',
                                            label: '删除',
                                            style: { color: 'red' },
                                            onClick: () => {
                                                remove(item.id)
                                                    .then((res) => {
                                                        if (res.success) {
                                                            message.success('删除成功');
                                                            loadData();
                                                        } else {
                                                            message.error(res.message || '删除失败');
                                                        }
                                                    });
                                            }
                                        }
                                    ] as any[]
                                }}
                            >
                                <Button>操作</Button>
                            </Dropdown>
                        )
                    }
                ]}
            />
            <CreateUserGroupPage open={createOpen} onClose={() => setCreateOpen(false)} onOk={() => {
                loadData();
                setCreateOpen(false);
            }} />
            <UpdateUserGroupPage open={updateValue.open} onClose={() => setUpdateValue({ ...updateValue, open: false })} onOk={() => {
                loadData();
                setUpdateValue({ ...updateValue, open: false });
            }} value={updateValue.value} />
        </div>
    );
} 
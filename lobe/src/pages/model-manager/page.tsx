import { useEffect, useState } from "react";
import { DeleteModelManager, EnableModelManager, GetModelManagerList } from "../../services/ModelManagerService";
import { Button, Dropdown, message, Table } from "antd";
import { Header, Input, Tag } from "@lobehub/ui";
import { getCompletionRatio, renderQuota } from "../../utils/render";
import CreateModelManagerPage from "./features/CreateModelManager";
import { getIconByName } from "../../utils/iconutils";
import { IconAvatar, OpenAI } from "@lobehub/icons";
import UpdateModelManagerPage from "./features/UpdateModelManager";

export default function ModelManager() {
    const [createOpen, setCreateOpen] = useState<boolean>(false);
    const [updateValue, setUpdateValue] = useState<any>({
        value: {},
        open: false
    });
    const [data, setData] = useState<any[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [total, setTotal] = useState<number>(0);
    const [input, setInput] = useState({
        page: 1,
        pageSize: 10,
        model: '',
    });
    function loadData() {
        setLoading(true);
        GetModelManagerList(input.model, input.page, input.pageSize)
            .then((res) => {
                setData(res.data.items);
                setTotal(res.data.total);
            }).finally(() => {
                setLoading(false);
            });
    }

    useEffect(() => {
        loadData();
    }, [input]);

    return (
        <div>
            <Header
                nav={'模型倍率管理'}
                actions={<>
                    <Input placeholder="请输入需要搜索的模型" value={input.model} onChange={(v) => {
                        setInput({
                            ...input,
                            model: v.target.value
                        })
                    }} >
                    </Input>
                    <Button onClick={() => {
                        setCreateOpen(true);
                    }}>
                        新增模型
                    </Button>
                </>
                }
            >
            </Header>
            <Table
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
                    }
                }}
                loading={loading}
                dataSource={data}
                scroll={{
                    y: 800,
                    x: 800,
                }}
                columns={[
                    {
                        key: 'icon',
                        title: '图标',
                        dataIndex: 'icon',
                        width: 60,
                        render: (value: any) => {
                            const icon = getIconByName(value);
                            return icon?.icon ?? <IconAvatar size={36} Icon={OpenAI} />;
                        }
                    },
                    {
                        key: 'model',
                        title: '模型',
                        dataIndex: 'model'
                    },
                    {
                        key: 'description',
                        title: '描述',
                        dataIndex: 'description'
                    },
                    {
                        key: 'price',
                        title: '模型价格',
                        dataIndex: 'price',
                        width: 180,
                        render: (_: any, item: any) => {
                            return (<div>
                                <Tag color='cyan'>提示{renderQuota(item.promptRate * 1000)}/1k tokens</Tag>
                                {item.completionRate ? <><Tag style={{
                                    marginTop: 8
                                }} color='geekblue'>完成{renderQuota(item.completionRate * 1000)}/1M tokens</Tag></> : <><Tag style={{
                                    marginTop: 8
                                }} color='geekblue'>完成{renderQuota(getCompletionRatio(item.model) * 1000)}/1M tokens</Tag></>}

                                {item.isVersion2 && <div>
                                    <Tag color='cyan'>音频输入{renderQuota(item.audioPromptRate * 1000000)}/1M tokens</Tag>
                                    {item.completionRate ? <><Tag style={{
                                        marginTop: 8
                                    }} color='geekblue'>音频完成{renderQuota(item.audioOutputRate * 1000000)}/1M tokens</Tag></> : <><Tag style={{
                                        marginTop: 8
                                    }} color='geekblue'>音频完成{renderQuota(getCompletionRatio(item.model) * 1000000)}/1M tokens</Tag></>}
                                </div>}
                            </div>)
                        }
                    },
                    {
                        key: 'quotaType',
                        title: '模型类型',
                        dataIndex: 'quotaType',
                        render: (value: any) => {
                            return value === 1 ? '按量计费' : '按次计费';
                        }
                    },
                    {
                        key: 'quotaMax',
                        title: '模型额度最大上文',
                        dataIndex: 'quotaMax'
                    },
                    {
                        key: "tags",
                        title: '标签',
                        dataIndex: 'tags',
                        render: (value: any) => {
                            return value.map((tag: any) => {
                                return <Tag key={tag} color='blue'>{tag}</Tag>;
                            });
                        }
                    },
                    {
                        key: 'enable',
                        title: '状态',
                        dataIndex: 'enable',
                        render: (value: any) => {
                            return value ? '启用' : '禁用';
                        }
                    },
                    {
                        key: 'actions',
                        title: '操作',
                        render: (_: any, item: any) => {
                            return (
                                <Dropdown
                                    menu={{
                                        items: [
                                            {
                                                key: 'edit',
                                                label: '编辑',
                                                onClick: () => {
                                                    setUpdateValue({
                                                        value: item,
                                                        open: true
                                                    });
                                                }
                                            },
                                            {
                                                key: "enable",
                                                label: item.enable ? '禁用' : '启用',
                                                onClick: () => {
                                                    EnableModelManager(item.id)
                                                        .then((res) => {
                                                            if (res.success) {
                                                                message.success('操作成功');
                                                                loadData();
                                                            } else {
                                                                message.error(res.message);
                                                            }
                                                        });
                                                }
                                            },
                                            {
                                                key: 'delete',
                                                label: '删除',
                                                style: {
                                                    color: 'red'
                                                },
                                                onClick: () => {
                                                    DeleteModelManager(item.id)
                                                        .then((res) => {
                                                            if (res.success) {
                                                                message.success('删除成功');
                                                                loadData();
                                                            } else {
                                                                message.error(res.message);
                                                            }
                                                        });

                                                }
                                            }
                                        ] as any[]
                                    }
                                    }
                                >
                                    <Button >操作</Button>
                                </Dropdown>
                            );
                        }
                    }
                ]}
            />
            <CreateModelManagerPage open={createOpen} onClose={() => {
                setCreateOpen(false);
            }} onOk={() => {
                loadData();
                setCreateOpen(false);
            }} />

            <UpdateModelManagerPage open={updateValue.open} onClose={() => {
                setUpdateValue({
                    ...updateValue,
                    open: false
                });
            }
            } onOk={() => {
                loadData();
                setUpdateValue({
                    ...updateValue,
                    open: false
                });
            }} value={updateValue.value} />

        </div>
    )
}
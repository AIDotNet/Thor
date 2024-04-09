import { Button, Dropdown, Input, Notification, Table, Tag } from "@douyinfe/semi-ui"
import { useMemo, useState } from "react";
import styled from "styled-components"
import { getChannels,disable,Remove } from "../../services/ChannelService";
import CreateChannel from "./features/CreateChannel";
import UpdateChannel from "./features/UpdateChannel";

const Header = styled.header`

`

export default function Channel() {
    const columns = [
        {
            title: '名称',
            dataIndex: 'name',
        },
        {
            title: '是否禁用',
            dataIndex: 'disable',
            render: (value: any) => {
                if (value) {
                    return <span>禁用</span>
                } else {
                    return <span>正常</span>
                }
            }
        },
        {
            title: 'AI类型',
            dataIndex: 'type',
            render: (value: any) => {
                return <Tag>{value}</Tag>
            }
        },
        {
            title: '响应时间',
            dataIndex: 'responseTime'
        },
        {
            title: '创建时间',
            dataIndex: 'createdAt',
        },
        {
            title: '消费Token',
            dataIndex: 'quota',
            render: (value: any) => {
                return <span>{value}</span>
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
            title: '操作',
            dataIndex: 'operate',
            render: (_v: any, item: any) => {
                return <>
                    <Dropdown
                        render={
                            <Dropdown.Menu>
                                <Dropdown.Item onClick={() => {
                                    setUpdateValue(item);
                                    setUpdateVisible(true);
                                }}>编辑</Dropdown.Item>
                                <Dropdown.Item onClick={() => {
                                    disable(item.id).then((item) => {
                                        item.success ? Notification.success({
                                            title: '操作成功',
                                        }) : Notification.error({
                                            title: '操作失败',
                                        });
                                        loadingData();
                                    }), () => Notification.error({
                                        title: '操作失败',
                                    });
                                }}>
                                    {
                                        item.disable ? '启用' : '禁用'
                                    }
                                </Dropdown.Item>
                                <Dropdown.Item style={{
                                    color: 'red',
                                }} onClick={() => removeToken(item.id)}>删除</Dropdown.Item>
                            </Dropdown.Menu>
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
    const [data, setData] = useState([]);
    const [total, setTotal] = useState(0);
    const [selectedRowKeys, setSelectedRowKeys] = useState([]);
    const [input, setInput] = useState({
        page: 1,
        pageSize: 10,
        keyword: '',
    });

    function copyKey(key: string) {
        navigator.clipboard.writeText(key).then(() => {
            Notification.success({
                title: '复制成功',
            })
        }).catch(() => {
            Notification.error({
                title: '复制失败',
            })
        });
    }

    function removeToken(id: string) {
        Remove(id)
            .then((v) => {
                if (v.success) {
                    loadingData();
                } else {
                    Notification.error({
                        title: '删除失败',
                    })
                }
            })
    }

    function loadingData() {
        getChannels(input.page, input.pageSize)
            .then((v) => {
                if (v.success) {
                    setData(v.data.items);
                    setTotal(v.data.total);
                } else {
                    Notification.error({
                        title: '获取数据失败',
                    });
                }
            })
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
        <>
            <Header>
                <span style={{
                    fontSize: '1.5rem',
                    fontWeight: 'bold',
                }}>
                    渠道管理
                </span>

                <Dropdown
                    render={
                        <Dropdown.Menu>
                            <Dropdown.Item onClick={() => setCreateVisible(true)}>创建渠道</Dropdown.Item>
                        </Dropdown.Menu>
                    }
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
                        keyword: v,
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
                currentPage: input.page,
                onChange: (page, pageSize) => {
                    setInput({
                        ...input,
                        page,
                        pageSize,
                    });
                },

            }} />
            <CreateChannel onSuccess={() => {
                setCreateVisible(false);
                loadingData();
            }}
                visible={createVisible} onCancel={() => setCreateVisible(false)} />

            <UpdateChannel onSuccess={() => {
                setUpdateVisible(false);
                loadingData();
            }
            }
                value={updateValue}
                visible={updateVisible}
                onCancel={() => setUpdateVisible(false)} />

        </>
    )
}
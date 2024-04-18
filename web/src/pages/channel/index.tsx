import { Button, Switch, Dropdown, Input, Notification, Table, Tag, InputNumber } from "@douyinfe/semi-ui"
import { useMemo, useState } from "react";
import styled from "styled-components"
import { getChannels, disable, Remove, test, controlAutomatically, UpdateOrder } from "../../services/ChannelService";
import CreateChannel from "./features/CreateChannel";
import UpdateChannel from "./features/UpdateChannel";
import { IconTick, IconClose } from "@douyinfe/semi-icons";

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
                return <Switch size='large'
                    defaultChecked={!value} onChange={() => {
                        disable(value.id)
                            .then((item) => {
                                item.success ? Notification.success({
                                    title: '操作成功',
                                }) : Notification.error({
                                    title: '操作失败',
                                });
                                loadingData();
                            }), () => Notification.error({
                                title: '操作失败',
                            });
                    }} checkedText={<IconTick />} uncheckedText={<IconClose />} style={{
                        width: '50px',
                    }} aria-label="a switch for semi demo"></Switch>
            }
        },
        {
            title: '是否自动监控',
            dataIndex: 'controlAutomatically',
            render: (value: any,i:any) => {
                return <Switch size='large'
                    defaultChecked={value} onChange={() => {
                        controlAutomatically(i.id)
                            .then((item) => {
                                item.success ? Notification.success({
                                    title: '操作成功',
                                }) : Notification.error({
                                    title: '操作失败',
                                });
                                loadingData();
                            }), () => Notification.error({
                                title: '操作失败',
                            });
                    }} checkedText={<IconTick />} uncheckedText={<IconClose />} style={{
                        width: '50px',
                    }} aria-label="a switch for semi demo"></Switch>
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
                    size='large'
                    shape='circle'
                    type='solid'
                    color={color as any} onClick={() => testToken(item.id)}>{value / 1000}秒</Tag>
                } else {
                    return <Tag onClick={() => testToken(item.id)}>未测试</Tag>
                }
            }
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
            title: '权重',
            dataIndex: 'order',
            render: (value: any,item:any) => {
                return <InputNumber 
                hideButtons
                onChange={(v) => {
                    item.order = v;
                    setData([...data]);
                }}
                onBlur={() => {
                    UpdateOrder(item.id, item.order)
                        .then((i) => {
                            i.success ? Notification.success({
                                title: '操作成功',
                            }) : Notification.error({
                                title: '操作失败',
                            });
                            loadingData();
                        }), () => Notification.error({
                            title: '操作失败',
                        });
                }}
                
                value={value} style={{
                    width: '80px',
                }}></InputNumber>
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

    function testToken(id: string) {
        test(id)
            .then((v) => {
                if (v.success) {
                    Notification.success({
                        title: '测试成功',
                    })
                    loadingData();
                } else {
                    Notification.error({
                        title: '测试失败',
                    })
                }
            })
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
        <div  style={{
            margin: '20px'
        }}>
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

        </div>
    )
}
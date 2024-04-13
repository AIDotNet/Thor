import { Button, Dropdown, Input, Notification, Switch, Table } from "@douyinfe/semi-ui"
import { useMemo, useState } from "react";
import styled from "styled-components"
import CreateToken from "./features/CreateToken";
import { disable, getTokens, Remove } from '../../services/TokenService'
import UpdateToken from "./features/UpdateToken";
import { IconClose, IconTick } from "@douyinfe/semi-icons";
import { renderQuota } from "../../uitls/render";

const Header = styled.header`

`

export default function Token() {
    const columns = [
        {
            title: '名称',
            dataIndex: 'name',
        },
        {
            title: '是否禁用',
            dataIndex: 'disabled',
            render: (value: any, item: any) => {
                return <Switch size='large'
                    defaultChecked={!value} onChange={(v) => {
                        disable(item.id)
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
                // unlimitedExpired
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
            render: (_v: any, item: any) => {
                return <>
                    <Dropdown
                        render={
                            <Dropdown.Menu>
                                <Dropdown.Item onClick={() => {
                                    setUpdateTokenVisible(true);
                                    setUpdateTokenValue(item);
                                }}>编辑</Dropdown.Item>
                                <Dropdown.Item onClick={() => copyKey(item.key)}>复制Key</Dropdown.Item>
                                <Dropdown.Item onClick={() => {
                                    disable(item.id).then((item) => {
                                        item.success ? Notification.success({
                                            title: '操作成功',
                                        }) : Notification.error({
                                            title: '操作失败',
                                        });
                                    }), () => Notification.error({
                                        title: '操作失败',
                                    });
                                }}>
                                    {
                                        item.disabled ? '启用' : '禁用'
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
        <>
            <Header>
                <span style={{
                    fontSize: '1.5rem',
                    fontWeight: 'bold',
                }}>
                    Token管理
                </span>

                <Dropdown
                    render={
                        <Dropdown.Menu>
                            <Dropdown.Item onClick={() => setCreateTokenVisible(true)}>创建令牌</Dropdown.Item>
                            <Dropdown.Item>删除选中令牌</Dropdown.Item>
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
                <Input value={input.token} onChange={(e) => {
                    setInput({
                        ...input,
                        token: e,
                    });
                }} style={{
                    width: '200px',
                    float: 'right',
                    marginRight: '1rem',
                }} placeholder='搜索Token'></Input>
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
            <CreateToken visible={createTokenVisible} onCancel={() =>
                setCreateTokenVisible(false)} onSuccess={() => {
                    setCreateTokenVisible(false);
                    loadingData();
                }} />
            <UpdateToken value={updateTokenValue} visible={updateTokenVisible} onCancel={() => {
                setUpdateTokenValue({} as any);
                setUpdateTokenVisible(false)
            }} onSuccess={() => {
                setUpdateTokenVisible(false);
                setUpdateTokenValue({} as any);
                loadingData();
            }} />
        </>
    )
}
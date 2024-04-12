import { Button, Dropdown, Input, Notification, Switch, Table, Tag } from "@douyinfe/semi-ui"
import { useMemo, useState } from "react";
import styled from "styled-components"
import CreateRedeemCode from "./features/CreateRedeemCode";
import UpdateRedeemCode from "./features/UpdateRedeemCode";
import { IconClose, IconTick } from "@douyinfe/semi-icons";
import { renderQuota } from "../../uitls/render";
import { Enable, Remove, getRedeemCodes } from "../../services/RedeemCodeService";

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
                return <Switch size='large'
                    defaultChecked={!value} onChange={(v) => {
                        Enable(item.id)
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
            title: '额度',
            dataIndex: 'quota',
            render: (value: any, item: any) => {
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
                        render={
                            <Dropdown.Menu>
                                <Dropdown.Item onClick={() => {
                                    setUpdateTokenVisible(true);
                                    setUpdateTokenValue(item);
                                }}>编辑</Dropdown.Item>
                                <Dropdown.Item onClick={() => copyKey(item.code)}>复制</Dropdown.Item>
                                <Dropdown.Item style={{
                                    color: 'red',
                                }} onClick={() => removeRedeemCode(item.id)}>删除</Dropdown.Item>
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

    function removeRedeemCode(id: number) {
        Remove(id)
            .then((v) => {
                if (v.success) {
                    loadingData();
                    Notification.success({
                        title: '删除成功',
                    })
                } else {
                    Notification.error({
                        title: '删除失败',
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
        </>
    )
}
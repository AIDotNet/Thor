import { useEffect, useState } from "react";
import { Button, Switch, message, Tag, Dropdown, InputNumber, Table } from 'antd';
import { renderQuota } from "../../utils/render";
import styled from "styled-components";
import { Input } from "@lobehub/ui";
import { disableRateLimitModel, getRateLimitModel, removeRateLimitModel } from "../../services/RateLimitModelService";
import CreateRateLimit from "./features/CreateRateLimit";
import UpdateRateLimit from "./features/UpdateRateLimit";

const Header = styled.header`

`
export default function RateLimitPage() {

    const columns = [
        {
            title: '名称',
            dataIndex: 'name',
        },
        {
            title: '是否启用',
            dataIndex: 'enable',
            render: (value: any, item: any) => {
                return <Switch
                    checkedChildren={<span style={{
                        color: "green"
                    }}>
                        启
                    </span>}
                    unCheckedChildren={<span style={{
                        color: "red"
                    }}>
                        禁
                    </span>}
                    value={value} onChange={() => {
                        disableRateLimitModel(item.id)
                            .then((item) => {
                                item.success ? message.success({
                                    content: '操作成功'
                                }) : message.error({
                                    content: '操作失败'
                                });
                                loadingData();
                            }), () => message.error({
                                content: '操作失败'
                            });
                    }} style={{
                        width: '50px',
                    }} aria-label="a switch for semi demo"></Switch>
            }
        },
        {
            title: '描述',
            dataIndex: 'description'
        },
        {
            title: '限流策略',
            dataIndex: 'strategy',
            render: (value: any) => {
                switch (value) {
                    case 's':
                        return '秒';
                    case 'm':
                        return '分';
                    case 'h':
                        return '时';
                    case 'd':
                        return '天';
                    default:
                        return '未知';
                }
            }
        },
        {
            title: '创建时间',
            dataIndex: 'createdAt',
        },
        {
            title: '限流策略数量',
            dataIndex: 'limit',
        },
        {
            title: '限流数量',
            dataIndex: 'value',
        },
        {
            title: '操作',
            dataIndex: 'operate',
            render: (_v: any, item: any) => {
                return <>
                    <Dropdown
                        menu={{
                            items: [
                                {
                                    key: 1,
                                    label: '编辑',
                                    onClick: () => {
                                        setUpdateValue(item);
                                        setUpdateVisible(true);
                                    }
                                },
                                {
                                    key: 4,
                                    label: '删除',
                                    onClick: () => remove(item.id)
                                }
                            ] as any[]
                        }
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
    const [data, setData] = useState<any[]>([]);
    const [total, setTotal] = useState(0);
    const [input, setInput] = useState({
        page: 1,
        pageSize: 10,
        keyword: '',
    });

    function remove(id: string) {
        removeRateLimitModel(id)
            .then((v) => {
                if (v.success) {
                    loadingData();
                } else {
                    message.error({
                        content: '删除失败',
                    })
                }
            })
    }


    function loadingData() {
        getRateLimitModel(input.page, input.pageSize)
            .then((v: any) => {
                if (v.success) {
                    const values = v.data.items as any[];
                    setData([...values]);
                    setTotal(v.data.total);
                } else {
                    message.error({
                        content: v.message,
                    });
                }
            })
    }

    useEffect(() => {
        loadingData();
    }, [input]);


    return (
        <div style={{
            margin: '20px',
            height: '100%',
            width: '100%',
        }}>
            <Header>
                <span style={{
                    fontSize: '1.5rem',
                    fontWeight: 'bold',
                }}>
                    限流策略管理
                </span>

                <Dropdown
                    menu={{
                        items: [
                            {
                                key: 1,
                                label: "新增策略渠道",
                                onClick: () => setCreateVisible(true)
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
            <Table style={{
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
            <CreateRateLimit visible={createVisible} onSuccess={() => {
                setCreateVisible(false);
                loadingData();
            }} onCancel={() => {
                setCreateVisible(false);
            }} />

            <UpdateRateLimit visible={updateVisible} value={updateValue} onSuccess={() => {
                setUpdateVisible(false);
                loadingData();
            }
            } onCancel={() => {
                setUpdateVisible(false);
            }} />
            
        </div>
    );
}
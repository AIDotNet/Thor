import { useEffect, useState } from "react";
import { ChatLogger } from "../..";
import styled from "styled-components"
import { Button, DatePicker, Dropdown, Input, Notification, Table, Tag } from "@douyinfe/semi-ui";
import { getLoggers } from "../../services/LoggerService";
import { renderQuota } from "../../uitls/render";


const Header = styled.header`

`

export default function Channel() {
    const [data, setData] = useState<ChatLogger[]>([]);
    const [total, setTotal] = useState<number>(0);
    const [loading, setLoading] = useState<boolean>(false);
    const [input, setInput] = useState({
        page: 1,
        pageSize: 10,
        type: '',
        model: '',
        startTime: '',
        endTime: '',
        keyword: ''
    });

    const columns = [
        {
            title: '时间',
            dataIndex: 'createdAt',
            key: 'createdAt'
        },
        {
            title: '渠道',
            dataIndex: 'channelName',
            key: 'channelName',
            render: (value: any) => {
                return <Tag size='large' type='ghost' color='blue'>{value}</Tag>
            }
        },
        {
            title: '用户',
            dataIndex: 'userName',
            key: 'userName',
            render: (value: any) => {
                return <Tag size='large' type='ghost' color='blue'>{value}</Tag>
            }
        },
        {
            title: '模型',
            dataIndex: 'modelName',
            key: 'modelName',
            render: (value: any) => {
                return <Tag size='large' >{value}</Tag>
            }
        },
        {
            title: '提示tokens',
            dataIndex: 'promptTokens',
            key: 'promptTokens'
        },
        {
            title: '完成tokens',
            dataIndex: 'completionTokens',
            key: 'completionTokens'
        },
        {
            title: '额度',
            dataIndex: 'quota',
            key: 'quota',
            render: (value: any) => {
                return <Tag size='large' type='ghost' color='green'>{renderQuota(value, 6)}</Tag>
            }
        },
        {
            title: '详情',
            dataIndex: 'content',
            key: 'content'
        }
    ]

    function loadData() {
        setLoading(true);

        getLoggers(input)
            .then(res => {
                if (res.success) {
                    setData(res.data.items);
                    setTotal(res.data.total);
                } else {
                    Notification.error({
                        title: '获取数据失败',
                        content: res.message
                    });
                }
            })
            .finally(() => {
                setLoading(false);
            })
    }

    useEffect(() => {
        loadData()
    }, [input.page, input.pageSize]);

    return (
        <div style={{
            margin: '20px'
        }}>
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
                            <Dropdown.Item>删除选中令牌</Dropdown.Item>
                        </Dropdown.Menu>
                    }
                >
                    <Button style={{
                        float: 'right',
                    }}>操作</Button>
                </Dropdown>
                <Button onClick={() => loadData()} style={{
                    marginRight: '0.5rem',
                    float: 'right',
                }}>搜索</Button>
                <Input
                    value={input.model}
                    onChange={(e) => {
                        setInput({
                            ...input,
                            model: e,
                        });
                    }}
                    style={{
                        marginRight: '0.5rem',
                        float: 'right',
                        width: '10rem',
                    }} placeholder='Token名称' />
                <Input
                    value={input.keyword}
                    onChange={(e) => {
                        setInput({
                            ...input,
                            keyword: e,
                        });
                    }}
                    style={{
                        marginRight: '0.5rem',
                        float: 'right',
                        width: '10rem',
                    }} placeholder='关键字' />

                <DatePicker
                    value={input.startTime}
                    onChange={(e: any) => {
                        setInput({
                            ...input,
                            startTime: e,
                        });
                    }}

                    style={{
                        marginRight: '0.5rem',
                        float: 'right',
                    }} placeholder='开始时间' />
                <DatePicker
                    value={input.endTime}
                    onChange={(e: any) => {
                        setInput({
                            ...input,
                            endTime: e,
                        });
                    }}
                    style={{
                        marginRight: '0.5rem',
                        float: 'right',
                    }} placeholder='结束时间' />

            </Header>
            <Table loading={loading} style={{
                marginTop: '1rem',
            }} columns={columns} dataSource={data} pagination={{
                total: total,
                pageSize: input.pageSize,
                currentPage: input.page,
                onChange: (page, pageSize) => {
                    // 修改input以后获取数据
                    setInput({
                        ...input,
                        page,
                        pageSize,
                    });
                },

            }} />
        </div>
    )
}
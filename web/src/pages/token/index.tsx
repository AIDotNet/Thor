import { Button, Dropdown, Input, Table } from "@douyinfe/semi-ui"
import { useMemo, useState } from "react";
import styled from "styled-components"
import CreateToken from "./features/CreateToken";

const Header = styled.header`

`

export default function Channel() {
    const columns = [
        {
            title: '名称',
            dataIndex: 'name',
        },
        {
            title: '状态',
            dataIndex: 'status',
        },
        {
            title: '已用额度',
            dataIndex: 'usedQuota',
        },
        {
            title: '剩余额度',
            dataIndex: 'remainingundrawn',
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
        },
        {
            title: '操作',
            dataIndex: 'operate',
            render: () => {
                return <>
                    <Button>编辑</Button>
                </>;
            },
        },
    ];
    const [createTokenVisible, setCreateTokenVisible] = useState(false);
    const [data, setData] = useState([]);
    const [selectedRowKeys, setSelectedRowKeys] = useState([]);
    const pagination = useMemo(() => ({
        pageSize: 10,

    }), [])
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
                <Input style={{
                    width: '200px',
                    float: 'right',
                    marginRight: '1rem',
                }} placeholder='Token'></Input>
                <Input style={{
                    width: '150px',
                    float: 'right',
                    marginRight: '1rem',
                }} placeholder='请输入关键字'></Input>
            </Header>
            <Table style={{
                marginTop: '1rem',
            }} columns={columns} dataSource={data} rowSelection={rowSelection} pagination={pagination} />
            <CreateToken visible={createTokenVisible} onCancel={() => setCreateTokenVisible(false)} onSuccess={() => { }} />
        </>
    )
}
import { Button, Dropdown, Input, Notification, Switch, Table } from "@douyinfe/semi-ui"
import { useMemo, useState } from "react";
import styled from "styled-components"
import { renderQuota } from "../../uitls/render";
import { getProduct, removeProduct } from "../../services/ProductService";
import CreateProduct from "./features/CreateProduct";
import UpdateProduct from "./features/UpdateProduct";

const Header = styled.header`

`

export default function Product() {
    const columns = [
        {
            title: '名称',
            dataIndex: 'name',
        },
        {
            title: '描述',
            dataIndex: 'description'
        },
        {
            title: '价格',
            dataIndex: 'price'
        },
        {
            title: '额度',
            dataIndex: 'remainQuota',
            render: (_v: any, item: any) => {
                return <span>{renderQuota(item.remainQuota, 6)}</span>
            }
        },
        {
            title: '产品库存',
            dataIndex: 'stock',
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
                                    setUpdateVisible(true);
                                    setUpdateValue(item);
                                }}>编辑</Dropdown.Item>
                                <Dropdown.Item style={{
                                    color: 'red',
                                }} onClick={() => remove(item.id)}>删除</Dropdown.Item>
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

    function remove(id: string) {
        removeProduct(id)
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
        getProduct()
            .then((res) => {
                setData(res.data);
            })
    }

    useMemo(() => {
        loadingData();
    }, []);

    return (
        <div style={{
            margin: '20px'
        }}>
            <Header>
                <span style={{
                    fontSize: '1.5rem',
                    fontWeight: 'bold',
                }}>
                    产品管理
                </span>
                <Button onClick={()=>{
                    setCreateVisible(true);
                }} style={{
                    float: 'right',
                }}>创建产品</Button>
            </Header>
            <Table style={{
                marginTop: '1rem',
            }} columns={columns} dataSource={data} />
            <CreateProduct 
                visible={createVisible} 
                onCancel={()=>setCreateVisible(false)}
                onSuccess={()=>{
                    setCreateVisible(false);
                    loadingData();
                }}
             />
             <UpdateProduct 
                value={updateValue}
                visible={updateVisible} 
                onCancel={()=>setUpdateVisible(false)}
                onSuccess={()=>{
                    setUpdateVisible(false);
                    loadingData();
                }}
                />
        </div>
    )
}
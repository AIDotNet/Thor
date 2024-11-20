import { useMemo, useState } from "react";
import styled from "styled-components"
import { getProduct, removeProduct } from "../../services/ProductService";
import { renderQuota } from "../../utils/render";
import { Dropdown, Button, Table, message } from 'antd';
import CreateProduct from "./features/CreateProduct";
import UpdateProduct from "./features/UpdateProduct";

const Header = styled.header`

`

export default function ProductPage() {
  const columns = [
    {
      title: '名称',
      fixed: 'left',
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
      fixed: 'right',
      render: (_v: any, item: any) => {
        return <>
          <Dropdown
            menu={{
              items: [
                {
                  key: 1,
                  label: '编辑',
                  onClick: () => {
                    setUpdateVisible(true);
                    setUpdateValue(item);
                  }
                },
                {
                  key: 2,
                  label: '删除',
                  onClick: () => remove(item.id)
                }
              ]
            }}
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
          message.error({
            content: '删除失败',
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
      margin: '20px',
      height: '100%',
      width: '100%',
    }}>
      <Header>
        <span style={{
          fontSize: '1.5rem',
          fontWeight: 'bold',
        }}>
          产品管理
        </span>
        <Button onClick={() => {
          setCreateVisible(true);
        }} style={{
          float: 'right',
        }}>创建产品</Button>
      </Header>
      <Table style={{
        marginTop: '1rem',
      }} columns={columns as any}
      scroll={{
        x: 'max-content',
        y: 'calc(100vh - 350px)',
      }}
      dataSource={data} />
      <CreateProduct visible={createVisible} onCancel={() => setCreateVisible(false)} onSuccess={() => {
        setCreateVisible(false);
        loadingData();
      }} />
      <UpdateProduct visible={updateVisible} value={updateValue} onCancel={() => setUpdateVisible(false)} onSuccess={() => {
        setUpdateVisible(false);
        loadingData();
      }} />
      
    </div>
  )
}
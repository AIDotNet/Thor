import { useMemo, useState } from "react";
import styled from "styled-components"
import { Button, Dropdown, Input, message, Switch, Table, Tag } from 'antd';
import { Enable, Remove, getRedeemCodes } from "../../services/RedeemCodeService";
import { renderQuota } from "../../utils/render";
import CreateRedeemCode from "./features/CreateRedeemCode";
import UpdateRedeemCode from "./features/UpdateRedeemCode";
import { useTranslation } from "react-i18next";

const Header = styled.header`

`

export default function RedeemCode() {
  const { t } = useTranslation();
  
  const columns = [
    {
      title: t('redeemCode.name'),
      dataIndex: 'name',
    },
    {
      title: t('redeemCode.disabled'),
      dataIndex: 'disabled',
      render: (value: any, item: any) => {
        return <Switch
          defaultChecked={!value} onChange={() => {
            Enable(item.id)
              .then((item) => {
                item.success ? message.success({
                  content: t('common.operateSuccess'),
                }) : message.error({
                  content: t('common.operateFailed'),
                });
                loadingData();
              }), () => message.error({
                content: t('common.operateFailed'),
              });
          }} style={{
            width: '50px',
          }} aria-label="a switch for semi demo"></Switch>
      }
    },
    {
      title: t('redeemCode.quota'),
      dataIndex: 'quota',
      render: (value: any) => {
        return <span>{renderQuota(value, 2)}</span>
      }
    },
    {
      title: t('redeemCode.createdAt'),
      dataIndex: 'createdAt',
    },
    {
      title: t('redeemCode.redeemedUser'),
      dataIndex: 'redeemedUserName',
      render: (value: any) => {
        if (value) {
          return <Tag color='green'>{value}</Tag>;
        } else {
          return t('common.noData');
        }
      }
    },
    {
      title: t('redeemCode.redeemedTime'),
      dataIndex: 'redeemedTime',
      render: (value: any) => {
        if (value) {
          return value;
        } else {
          return t('redeemCode.notRedeemed');
        }
      }
    },
    {
      title: t('common.operate'),
      dataIndex: 'operate',
      render: (_v: any, item: any) => {
        return <>
          <Dropdown
            menu={{
              items: [
                {
                  key: 1,
                  label: t('common.edit'),
                  onClick: () => {
                    setUpdateTokenVisible(true);
                    setUpdateTokenValue(item);
                  }
                },
                {
                  key: 2,
                  label: t('common.copy'),
                  onClick: () => {
                    copyKey(item.code);
                  }
                },
                {
                  key: 3,
                  label: t('common.delete'),
                  onClick: () => removeRedeemCode(item.id)
                }
              ]
            }}
          >
            <Button>{t('common.operate')}</Button>
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
    try {
      navigator.clipboard.writeText(key).then(() => {
        message.success({
          content: t('common.copySuccess'),
        })
      }).catch(() => {
        message.error({
          content: `${t('common.copyFailed')}: ${key}`,
          duration: 10,
        })
      });
    } catch (e) {
      message.error({
        content: `${t('common.copyFailed')}: ${key}`,
        duration: 10,
      })
    }
  }

  function removeRedeemCode(id: number) {
    Remove(id)
      .then((v) => {
        if (v.success) {
          loadingData();
          message.success({
            content: t('common.deleteSuccess'),
          })
        } else {
          message.error({
            content: t('common.deleteFailed'),
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
    <div style={{
      margin: '20px',
      width: '100%',
      height: '100%',
    }}>
      <Header>
        <span style={{
          fontSize: '1.5rem',
          fontWeight: 'bold',
        }}>
          {t('redeemCode.title')}
        </span>

        <Dropdown
          menu={{
            items: [
              {
                key: 1,
                label: t('redeemCode.createRedeemCode'),
                onClick: () => {
                  setCreateTokenVisible(true)
                }
              }
            ]
          }}
        >
          <Button style={{
            float: 'right',
          }}>{t('common.operate')}</Button>
        </Dropdown>
        <Button style={{
          marginRight: '0.5rem',
          float: 'right',
        }}>{t('common.search')}</Button>
        <Input value={input.keyword} onChange={(v) => {
          setInput({
            ...input,
            keyword: v.target.value,
          });
        }} style={{
          width: '150px',
          float: 'right',
          marginRight: '1rem',
        }} placeholder={t('common.search')}></Input>
      </Header>
      <Table style={{
        marginTop: '1rem',
        height: '100%',
      }} 
      scroll={{
        x: 'max-content',
        y: 'calc(100vh - 350px)',
      }}
      columns={columns} dataSource={data} rowSelection={rowSelection} pagination={{
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
      <CreateRedeemCode visible={createTokenVisible} onCancel={() =>
        setCreateTokenVisible(false)} onSuccess={() => {
          setCreateTokenVisible(false);
          loadingData();
      }} />
      <UpdateRedeemCode visible={updateTokenVisible} value={updateTokenValue} onCancel={() =>
        setUpdateTokenVisible(false)} onSuccess={() => {
          setUpdateTokenVisible(false);
          loadingData();
      }} />
    </div>
  )
}
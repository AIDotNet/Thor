import { useEffect, useState } from "react";
import { DeleteModelManager, EnableModelManager, GetModelManagerList } from "../../services/ModelManagerService";
import { Button, Dropdown, message, Table, Space, Input as AntInput, Switch, Typography, ConfigProvider, theme, Tag as AntTag } from "antd";
import { Header, Tag } from "@lobehub/ui";
import { getCompletionRatio, renderQuota } from "../../utils/render";
import CreateModelManagerPage from "./features/CreateModelManager";
import { getIconByName } from "../../utils/iconutils";
import { IconAvatar, OpenAI } from "@lobehub/icons";
import { PlusOutlined, SearchOutlined } from "@ant-design/icons";
import UpdateModelManagerPage from "./features/UpdateModelManager";
import { useTranslation } from "react-i18next";
import { useResponsive } from "antd-style";
import type { ColumnsType } from 'antd/es/table';

export default function ModelManager() {
    const { t } = useTranslation();
    const { mobile } = useResponsive();
    const { token } = theme.useToken();

    
    const [createOpen, setCreateOpen] = useState<boolean>(false);
    const [updateValue, setUpdateValue] = useState<any>({
        value: {},
        open: false
    });
    const [data, setData] = useState<any[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [total, setTotal] = useState<number>(0);
    const [isK, setIsK] = useState<boolean>(false);
    const [input, setInput] = useState({
        page: 1,
        pageSize: 10,
        model: '',
    });


    function loadData() {
        setLoading(true);
        GetModelManagerList(input.model, input.page, input.pageSize)
            .then((res) => {
                setData(res.data.items);
                setTotal(res.data.total);
            }).finally(() => {
                setLoading(false);
            });
    }

    useEffect(() => {
        loadData();
    }, [input]);

    const renderPrice = (item: any) => {
        if (isK) {
            if (item.quotaType === 1) {
                return (<>
                    <div>
                        <Tag color='cyan'>{t('modelManager.perPrompt')}{renderQuota(item.promptRate * 1000, 6)}/1K tokens</Tag>
                        {item.completionRate ? <><Tag style={{
                            marginTop: 8
                        }} color='geekblue'>{t('modelManager.perCompletion')}{renderQuota(item.completionRate * 1000, 6)}/1K tokens</Tag></> : <><Tag style={{
                            marginTop: 8
                        }} color='geekblue'>{t('modelManager.perCompletion')}{renderQuota(getCompletionRatio(item.model) * 1000, 6)}/1K tokens</Tag></>}
                    </div>
                    {item.isVersion2 && <div>
                        <Tag color='cyan'>{t('modelManager.audioInput')}{renderQuota(item.audioPromptRate * 1000)}/1K tokens</Tag>
                        {item.completionRate ? <><Tag style={{
                            marginTop: 8
                        }} color='geekblue'>{t('modelManager.audioOutput')}{renderQuota(item.audioOutputRate * 1000)}/1K tokens</Tag></> : <><Tag style={{
                            marginTop: 8
                        }} color='geekblue'>{t('modelManager.audioOutput')}{renderQuota(getCompletionRatio(item.model) * 1000)}/1K tokens</Tag></>}
                    </div>}
                </>)
            } else {
                return (
                    <Tag style={{ marginTop: 8 }} color='geekblue'>
                        {t('modelManager.perUsage')}{renderQuota(item.promptRate, 6)}
                    </Tag>)
            }
        } else {
            if (item.quotaType === 1) {
                return (<>
                    <div>
                        <Tag color='cyan'>{t('modelManager.perPrompt')}{renderQuota(item.promptRate * 1000000)}/1M tokens</Tag>
                        {item.completionRate ? <><Tag style={{
                            marginTop: 8
                        }} color='geekblue'>{t('modelManager.perCompletion')}{renderQuota(item.completionRate * 1000000)}/1M tokens</Tag></> : <><Tag style={{
                            marginTop: 8
                        }} color='geekblue'>{t('modelManager.perCompletion')}{renderQuota(getCompletionRatio(item.model) * 1000000)}/1M tokens</Tag></>}
                    </div>
                    {item.isVersion2 && <div>
                        <Tag color='cyan'>{t('modelManager.audioInput')}{renderQuota(item.audioPromptRate * 1000000)}/1M tokens</Tag>
                        {item.completionRate ? <><Tag style={{
                            marginTop: 8
                        }} color='geekblue'>{t('modelManager.audioOutput')}{renderQuota(item.audioOutputRate * 1000000)}/1M tokens</Tag></> : <><Tag style={{
                            marginTop: 8
                        }} color='geekblue'>{t('modelManager.audioOutput')}{renderQuota(getCompletionRatio(item.model) * 1000000)}/1M tokens</Tag></>}
                    </div>}
                </>)
            } else {
                return (
                    <Tag style={{ marginTop: 8 }} color='geekblue'>
                        {t('modelManager.perUsage')}{renderQuota(item.promptRate, 6)}
                    </Tag>)
            }
        }
    };

    const renderModelType = (value: number) => {
        switch (value) {
            case 1:
                return <AntTag color="blue">{t('modelManager.volumeBilling')}</AntTag>;
            case 2:
                return <AntTag color="green">{t('modelManager.perUseBilling')}</AntTag>;
            default:
                return null;
        }
    };

    const columns: ColumnsType<any> = [
        {
            key: 'icon',
            title: t('modelManager.modelIcon'),
            fixed: 'left' as const,
            dataIndex: 'icon',
            width: 60,
            render: (value: any) => {
                const icon = getIconByName(value);
                return icon?.icon ?? <IconAvatar size={36} Icon={OpenAI} />;
            }
        },
        {
            key: 'model',
            title: t('modelManager.modelName'),
            fixed: 'left' as const,
            dataIndex: 'model'
        },
        {
            key: 'description',
            title: t('modelManager.modelDescription'),
            dataIndex: 'description',
            responsive: ['md']
        },
        {
            key: 'price',
            title: t('modelManager.modelPrice'),
            dataIndex: 'price',
            width: mobile ? 120 : 180,
            render: (_: any, item: any) => renderPrice(item)
        },
        {
            key: 'quotaType',
            title: t('modelManager.modelType'),
            dataIndex: 'quotaType',
            responsive: ['lg'],
            render: (value: any) => renderModelType(value)
        },
        {
            key: 'type',
            title: t('modelManager.modelCategory'),
            dataIndex: 'type',
            responsive: ['lg'],
            render: (value: any) => {
                switch (value) {
                    case 'chat':
                        return <AntTag color="processing">{t('modelManager.typeChat')}</AntTag>;
                    case 'audio':
                        return <AntTag color="orange">{t('modelManager.typeAudio')}</AntTag>;
                    case 'image':
                        return <AntTag color="gold">{t('modelManager.typeImage')}</AntTag>;
                    case 'stt':
                        return <AntTag color="purple">{t('modelManager.typeSTT')}</AntTag>;
                    case 'tts':
                        return <AntTag color="magenta">{t('modelManager.typeTTS')}</AntTag>;
                    case 'embedding':
                        return <AntTag color="cyan">{t('modelManager.typeEmbedding')}</AntTag>;
                    default:
                        return value;
                }
            }
        },
        {
            key: 'quotaMax',
            title: t('modelManager.modelMaxContext'),
            dataIndex: 'quotaMax',
            responsive: ['xl']
        },
        {
            key: "tags",
            title: t('modelManager.modelTags'),
            dataIndex: 'tags',
            responsive: ['lg'],
            render: (value: any) => value.map((tag: any) => <Tag key={tag} color='blue'>{tag}</Tag>)
        },
        {
            key: 'enable',
            title: t('modelManager.modelStatus'),
            dataIndex: 'enable',
            responsive: ['md'],
            render: (value: any) => (
                value ? 
                <AntTag color="success">{t('modelManager.modelEnabled')}</AntTag> : 
                <AntTag color="error">{t('modelManager.modelDisabled')}</AntTag>
            )
        },
        {
            key: 'actions',
            title: t('common.action'),
            fixed: mobile ? undefined : 'right' as const,
            render: (_: any, item: any) => (
                <Dropdown
                    menu={{
                        items: [
                            {
                                key: 'edit',
                                label: t('common.edit'),
                                onClick: () => setUpdateValue({ value: item, open: true })
                            },
                            {
                                key: "enable",
                                label: item.enable ? t('common.disable') : t('common.enable'),
                                onClick: () => {
                                    EnableModelManager(item.id)
                                        .then((res) => {
                                            if (res.success) {
                                                message.success(t('common.operateSuccess'));
                                                loadData();
                                            } else {
                                                message.error(res.message);
                                            }
                                        });
                                }
                            },
                            {
                                key: 'delete',
                                label: t('common.delete'),
                                danger: true,
                                onClick: () => {
                                    DeleteModelManager(item.id)
                                        .then((res) => {
                                            if (res.success) {
                                                message.success(t('common.deleteSuccess'));
                                                loadData();
                                            } else {
                                                message.error(res.message);
                                            }
                                        });
                                }
                            }
                        ] as any[]
                    }}
                    placement="bottomRight"
                    trigger={['click']}
                >
                    <Button size={mobile ? "small" : "middle"}>{t('common.operate')}</Button>
                </Dropdown>
            )
        }
    ];

    return (
        <ConfigProvider theme={{
            components: {
                Table: {
                    headerBg: token.colorBgContainer,
                    headerSplitColor: token.colorBorderSecondary,
                    rowHoverBg: token.colorFillAlter,
                }
            }
        }}>
            <div style={{ height: '100%', display: 'flex', flexDirection: 'column'}}>
                <Typography.Title level={4} style={{ margin: 0, marginBottom: 16, display: mobile ? 'none' : 'block' }}>
                    {t('modelManager.title')}
                </Typography.Title>
                
                <Header
                    nav={mobile ? t('modelManager.title') : ''}
                    style={{ 
                        marginBottom: 16, 
                        padding: mobile ? '12px 16px' : '16px 24px', 
                        backgroundColor: token.colorBgContainer,
                        borderRadius: token.borderRadiusLG 
                    }}
                    actions={
                        <Space wrap={mobile}>
                            <Switch 
                                value={isK} 
                                checkedChildren={<Tag>{t('modelManager.modelPricePerK')}</Tag>} 
                                unCheckedChildren={<Tag>{t('modelManager.modelPricePerM')}</Tag>} 
                                defaultChecked 
                                onChange={(checked) => setIsK(checked)} 
                            />
                            <AntInput.Search
                                placeholder={t('modelManager.searchModel')}
                                value={input.model}
                                onChange={(e) => setInput({ ...input, model: e.target.value })}
                                onSearch={() => loadData()}
                                style={{ width: mobile ? '100%' : 240 }}
                                prefix={<SearchOutlined style={{ color: token.colorTextDisabled }} />}
                                allowClear
                            />
                            <Button 
                                type="primary" 
                                icon={<PlusOutlined />} 
                                onClick={() => setCreateOpen(true)}
                            >
                                {mobile ? '' : t('modelManager.createModel')}
                            </Button>
                        </Space>
                    }
                />
                
                <div style={{ 
                    flex: 1, 
                    backgroundColor: token.colorBgContainer, 
                    borderRadius: token.borderRadiusLG,
                    overflow: 'auto',
                    display: 'flex',
                    flexDirection: 'column'
                }}>
                    <Table
                        rowKey={row => row.id}
                        pagination={{
                            total: total,
                            pageSize: input.pageSize,
                            current: input.page,
                            onChange: (page, pageSize) => {
                                setInput({
                                    ...input,
                                    page,
                                    pageSize,
                                });
                            },
                            showSizeChanger: !mobile,
                            showTotal: (total) => `${total} ${t('common.items')}`,
                            style: { margin: 0, padding: mobile ? '8px' : '12px' },
                            position: ['bottomCenter']
                        }}
                        loading={loading}
                        dataSource={data}
                        scroll={{
                            x: 'max-content',
                            scrollToFirstRowOnChange: true
                        }}
                        columns={columns}
                        size={mobile ? "small" : "middle"}
                        style={{ height: '100%' }}
                    />
                </div>
                
                <CreateModelManagerPage open={createOpen} onClose={() => setCreateOpen(false)} onOk={() => {
                    loadData();
                    setCreateOpen(false);
                }} />
                
                <UpdateModelManagerPage open={updateValue.open} onClose={() => setUpdateValue({ ...updateValue, open: false })} onOk={() => {
                    loadData();
                    setUpdateValue({ ...updateValue, open: false });
                }} value={updateValue.value} />
            </div>
        </ConfigProvider>
    );
}
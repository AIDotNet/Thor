import { useEffect, useState } from "react";
import { Header, Input, Tag, Tooltip, } from "@lobehub/ui";
import { IconAvatar, OpenAI } from "@lobehub/icons";
import { Button, Switch, Table, message, theme } from "antd";
import { getCompletionRatio, renderQuota } from "../../utils/render";
import { getIconByName } from "../../utils/iconutils";
import { GetModelManagerList } from "../../services/ModelManagerService";
import { Search } from "lucide-react";
import { useLocation, useNavigate } from "react-router-dom";
import { Flexbox } from "react-layout-kit";

export default function DesktopLayout() {
    const navigate = useNavigate();
    const location = useLocation();
    const [data, setData] = useState<any[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [isK, setIsK] = useState<boolean>(false);
    const [total, setTotal] = useState<number>(0);
    const [input, setInput] = useState({
        page: 1,
        pageSize: 10,
        model: '',
        isFirst: true,
        type: ''
    });
    const [icons, setIcons] = useState<string[]>([]);

    // 使用 Ant Design 的主题 token
    const { token } = theme.useToken();

    function loadData() {
        setLoading(true);
        GetModelManagerList(input.model, input.page, input.pageSize, true, input.type)
            .then((res) => {
                setData(res.data.items);
                setTotal(res.data.total);
            }).finally(() => {
                setLoading(false);
            });
    }

    useEffect(() => {
        // 更新url地址的query
        const searchParams = new URLSearchParams(location.search);
        searchParams.set('model', input.model);

        navigate({
            search: searchParams.toString(),
        });

    }, [input.model]);

    useEffect(() => {
        // 根据query更新input
        const searchParams = new URLSearchParams(location.search);
        const model = searchParams.get('model');
        if (model) {
            setInput({
                ...input,
                model,
                isFirst: false
            });
        }
    }, [location.search]);

    useEffect(() => {
        loadData();
    }, [input.page, input.pageSize, input.isFirst, input.type, input.model])

    useEffect(() => {
        if (data.length > 0 && icons.length === 0) {
            const uniqueIcons = Array.from(new Set(data.map(item => item.icon)));
            // OpenAI排在最前面
            const openAIIndex = uniqueIcons.indexOf('OpenAI');
            if (openAIIndex > -1) {
                uniqueIcons.splice(openAIIndex, 1);
                uniqueIcons.unshift('OpenAI');
            }

            uniqueIcons.forEach((icon, index) => {
                if (icon === '' || icon === undefined) {
                    uniqueIcons[index] = '其他';
                }
            });

            console.log(uniqueIcons);

            setIcons(uniqueIcons);
        }
    }, [data]);

    return (
        <div className="model-manager-container">
            <Header
                nav={'模型列表'}
                actions={
                    <div className="header-actions">
                        <div className="unit-switch">
                            <span className="switch-label">单位：</span>
                            <Switch
                                value={isK}
                                checkedChildren={<Tag color="blue">K</Tag>}
                                unCheckedChildren={<Tag color="green">M</Tag>}
                                defaultChecked
                                onChange={(checked) => setIsK(checked)}
                            />
                        </div>
                        <Input
                            placeholder="请输入需要搜索的模型"
                            value={input.model}
                            className="search-input"
                            suffix={
                                <Button
                                    type='primary'
                                    size="small"
                                    icon={<Search size={16} />}
                                    onClick={() => loadData()}
                                />
                            }
                            onChange={(v) => {
                                setInput({
                                    ...input,
                                    model: v.target.value
                                })
                            }}
                        />
                    </div>
                }
            />
            <div className="icon-filter" style={{ backgroundColor: token.colorBgContainer }}>
                {icons.map(icon => (
                    <Flexbox
                        key={icon}
                        horizontal
                        gap={8}
                        className={`icon-item ${input.type === icon ? 'selected' : ''}`}
                        onClick={() => {
                            setInput({
                                ...input,
                                type: icon ?? "其他"
                            })
                        }}
                    >
                        {getIconByName(icon, 24)?.icon ?? <IconAvatar size={24} Icon={OpenAI} />}
                        <span style={{
                            overflow: 'hidden',
                            textOverflow: 'ellipsis',
                            display: '-webkit-box',
                            WebkitLineClamp: 1,
                            WebkitBoxOrient: 'vertical',
                            color: input.type === icon ? token.colorTextLightSolid : token.colorText
                        }}>{icon || '其他'}</span>
                    </Flexbox>
                ))}
            </div>
            <div className="table-container">
                <Table
                    rowKey={row => row.id}
                    pagination={{
                        total: total,
                        pageSize: input.pageSize,
                        defaultPageSize: input.page,
                        showSizeChanger: true,
                        showQuickJumper: true,
                        showTotal: (total) => `共 ${total} 条记录`,
                        onChange: (page, pageSize) => {
                            setInput({
                                ...input,
                                page,
                                pageSize,
                            });
                        }
                    }}
                    loading={loading}
                    dataSource={data}
                    className="model-table"
                    columns={[
                        {
                            key: 'icon',
                            title: '图标',
                            dataIndex: 'icon',
                            width: 70,
                            align: 'center',
                            render: (value: any) => {
                                const icon = getIconByName(value);
                                return <div className="model-icon">
                                    {icon?.icon ?? <IconAvatar size={36} Icon={OpenAI} />}
                                </div>;
                            }
                        },
                        {
                            key: 'model',
                            title: '模型',
                            dataIndex: 'model',
                            width: 260,
                            render: (mode: string) => {
                                return <span onClick={() => {
                                    navigator.clipboard.writeText(mode)
                                        .then(() => {
                                            message.success('复制成功');
                                        })
                                        .catch(() => {
                                            const input = document.createElement('input');
                                            input.value = mode;
                                            document.body.appendChild(input);
                                            input.select();
                                            document.execCommand('copy');
                                            document.body.removeChild(input);

                                            message.success('复制成功');
                                        })
                                }}>
                                    {mode}
                                </span>
                            }
                        },
                        {
                            key: 'isRealTime',
                            title: '实时接口',
                            dataIndex: 'isVersion2',
                            width: 90,
                            render: (value: boolean) => {
                                return value ? '是' : '否';
                            }
                        },
                        {
                            key: 'description',
                            title: '描述',
                            dataIndex: 'description',
                            render: (value: any) => {
                                return <Tooltip title={value}>
                                    <span style={{
                                        overflow: 'hidden',
                                        textOverflow: 'ellipsis',
                                        display: '-webkit-box',
                                        WebkitLineClamp: 1,
                                        WebkitBoxOrient: 'vertical',
                                    }}>
                                        {value}
                                    </span>
                                </Tooltip>
                            }
                        },
                        {
                            key: 'price',
                            title: '模型价格',
                            dataIndex: 'price',
                            render: (_: any, item: any) => {
                                if (isK) {
                                    if (item.quotaType === 1) {
                                        return (<>
                                            <div>
                                                <Tag color='cyan'>提示{renderQuota(item.promptRate * 1000, 6)}/1K tokens</Tag>
                                                {item.completionRate ? <><Tag style={{
                                                    marginTop: 8
                                                }} color='geekblue'>完成{renderQuota(item.promptRate * 1000 * item.completionRate, 6)}/1K tokens</Tag></> : <><Tag style={{
                                                    marginTop: 8
                                                }} color='geekblue'>完成{renderQuota(getCompletionRatio(item.model) * 1000, 6)}/1K tokens</Tag></>}
                                            </div>
                                            {item.isVersion2 && <div>
                                                <Tag color='cyan'>音频输入{renderQuota(item.audioPromptRate * 1000)}/1M tokens</Tag>
                                                {item.completionRate ? <><Tag style={{
                                                    marginTop: 8
                                                }} color='geekblue'>音频完成{renderQuota(item.audioPromptRate * 1000 * item.audioOutputRate)}/1M tokens</Tag></> : <><Tag style={{
                                                    marginTop: 8
                                                }} color='geekblue'>音频完成{renderQuota(getCompletionRatio(item.model) * 1000)}/1M tokens</Tag></>}
                                            </div>}
                                        </>)
                                    } else {
                                        return (
                                            <Tag style={{ marginTop: 8 }} color='geekblue'>
                                                每次{renderQuota(item.promptRate, 6)}
                                            </Tag>)
                                    }
                                } else {
                                    if (item.quotaType === 1) {

                                        return (<>
                                            <div>
                                                <Tag color='cyan'>提示{renderQuota(item.promptRate * 1000000)}/1M tokens</Tag>
                                                {item.completionRate ? <><Tag style={{
                                                    marginTop: 8
                                                }} color='geekblue'>完成{renderQuota(item.promptRate * 1000000 * item.completionRate)}/1M tokens</Tag></> : <><Tag style={{
                                                    marginTop: 8
                                                }} color='geekblue'>完成{renderQuota(getCompletionRatio(item.model) * 1000000)}/1M tokens</Tag></>}
                                            </div>
                                            {item.isVersion2 && <div>
                                                <Tag color='cyan'>音频输入{renderQuota(item.audioPromptRate * 1000000)}/1M tokens</Tag>
                                                {item.completionRate ? <><Tag style={{
                                                    marginTop: 8
                                                }} color='geekblue'>音频完成{renderQuota(item.audioPromptRate * 1000000 * item.audioOutputRate)}/1M tokens</Tag></> : <><Tag style={{
                                                    marginTop: 8
                                                }} color='geekblue'>音频完成{renderQuota(getCompletionRatio(item.model) * 1000000)}/1M tokens</Tag></>}
                                            </div>}
                                        </>)
                                    } else {

                                        return (
                                            <Tag style={{ marginTop: 8 }} color='geekblue'>
                                                每次{renderQuota(item.promptRate, 6)}
                                            </Tag>)
                                    }
                                }
                            }
                        },
                        {
                            key: 'quotaType',
                            title: '模型类型',
                            dataIndex: 'quotaType',
                            render: (value: any) => {
                                return value === 1 ? '按量计费' : '按次计费';
                            }
                        },
                        {
                            key: 'quotaMax',
                            title: '模型额度最大上文',
                            dataIndex: 'quotaMax'
                        },
                        {
                            key: "tags",
                            title: '标签',
                            dataIndex: 'tags',
                            render: (value: any) => {
                                return value.map((tag: any) => {
                                    return <Tag key={tag} color='blue'>{tag}</Tag>;
                                });
                            }
                        },
                        {
                            key: 'enable',
                            title: '状态',
                            dataIndex: 'enable',
                            render: (value: any) => {
                                if (value) {
                                    return <Tag color='green'>可用</Tag>
                                }
                                else {
                                    return <Tag color='red'>
                                        不可用
                                    </Tag>
                                }
                            }
                        }
                    ]}
                />
            </div>
            <style >{`
                .model-manager-container {
                    display: flex;
                    flex-direction: column;
                }
                
                .header-actions {
                    display: flex;
                    align-items: center;
                    gap: 16px;
                }
                
                .unit-switch {
                    display: flex;
                    align-items: center;
                    gap: 8px;
                }
                
                .switch-label {
                    font-size: 14px;
                    color: rgba(0, 0, 0, 0.65);
                }
                
                .search-input {
                    width: 240px;
                }
                
                .table-container {
                    margin: 16px;
                    border-radius: 8px;
                    box-shadow: 0 1px 2px rgba(0, 0, 0, 0.03);
                    padding: 16px;
                    flex: 1;
                    overflow: auto;
                }
                
                .model-table {
                    margin: 0;
                }
                
                .model-icon {
                    display: flex;
                    justify-content: center;
                    align-items: center;
                }

                .icon-filter {
                    display: flex;
                    flex-wrap: wrap;
                    gap: 16px;
                    margin: 16px;
                    padding: 16px;
                    border-radius: ${token.borderRadius}px;
                    box-shadow: ${token.boxShadow};
                }

                .icon-item {
                    cursor: pointer;
                    padding: 8px;
                    border-radius: ${token.borderRadiusSM}px;
                    transition: background-color 0.3s;
                }

                .icon-item:hover {
                    background-color: ${token.colorBgBlur};
                }

                .icon-item.selected {
                    background-color: ${token.colorBgTextHover};
                    color: ${token.colorTextLightSolid};
                }

                .icon-item.selected svg {
                    color: ${token.colorTextLightSolid};
                }
            `}</style>
        </div>
    );
}
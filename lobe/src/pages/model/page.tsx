import { useEffect, useMemo, useState } from "react";
import { Header, Input, Tag, Tooltip } from "@lobehub/ui";
import { IconAvatar, OpenAI } from "@lobehub/icons";
import { Button, Empty, Switch, Table, message, theme } from "antd";
import type { AlignType } from "rc-table/lib/interface";
import { getCompletionRatio, renderQuota } from "../../utils/render";
import { getIconByName } from "../../utils/iconutils";
import { GetModelManagerList } from "../../services/ModelManagerService";
import { Search } from "lucide-react";
import { useLocation, useNavigate } from "react-router-dom";
import { Flexbox } from "react-layout-kit";
import { getProvider } from "../../services/ModelService";
import { motion } from "framer-motion";
import { createStyles } from "antd-style";

const MotionTag = motion(Tag);
const MotionButton = motion(Button);
const MotionDiv = motion.div;

const useStyles = createStyles(({ token, css }) => ({
    modelContainer: css`
        display: flex;
        flex-direction: column;
        height: 100%;
        background: linear-gradient(145deg, ${token.colorBgContainer} 0%, ${token.colorBgElevated} 100%);
        position: relative;
        overflow: hidden;
    `,
    searchInput: css`
        width: 240px;
        transition: all 0.3s;
        &:focus, &:hover {
            width: 280px;
            box-shadow: 0 0 8px ${token.colorPrimaryBg};
        }
    `,
    iconFilter: css`
        display: flex;
        flex-wrap: wrap;
        gap: 12px;
        margin: 16px 16px 0 16px;
        padding: 16px;
        border-radius: ${token.borderRadius}px;
        background-color: ${token.colorBgContainer};
        box-shadow: ${token.boxShadowSecondary};
        position: relative;
        overflow: hidden;
        z-index: 1;
    `,
    iconItem: css`
        cursor: pointer;
        padding: 8px 16px;
        border-radius: ${token.borderRadiusSM}px;
        transition: all 0.4s cubic-bezier(0.2, 0.8, 0.2, 1);
        border: 1px solid transparent;
        position: relative;
        overflow: hidden;
        
        &:hover {
            background-color: ${token.colorBgTextHover};
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
        }
        
        &.selected {
            background: linear-gradient(90deg, ${token.colorPrimary} 0%, ${token.colorPrimaryActive} 100%);
            color: ${token.colorTextLightSolid};
            transform: translateY(-2px);
            box-shadow: 0 8px 20px ${token.colorPrimaryBgHover};
        }
        
        &.selected svg {
            color: ${token.colorTextLightSolid};
        }
    `,
    tableContainer: css`
        margin: 16px;
        border-radius: ${token.borderRadius}px;
        box-shadow: ${token.boxShadowSecondary};
        padding: 16px;
        flex: 1;
        overflow: auto;
        background-color: ${token.colorBgContainer};
        position: relative;
        z-index: 1;
        backdrop-filter: blur(10px);
    `,
    modelName: css`
        cursor: pointer;
        color: ${token.colorPrimary};
        transition: color 0.3s;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        position: relative;
        padding-bottom: 2px;
        
        &:hover {
            color: ${token.colorPrimaryHover};
            &:after {
                width: 100%;
            }
        }
        
        &:after {
            content: '';
            position: absolute;
            bottom: 0;
            left: 0;
            width: 0;
            height: 1px;
            background: ${token.colorPrimary};
            transition: width 0.3s ease;
        }
    `,
    modelTags: css`
        display: flex;
        flex-wrap: wrap;
        gap: 6px;
    `,
    primaryButton: css`
        background: linear-gradient(90deg, ${token.colorPrimary} 0%, ${token.colorPrimaryActive} 100%);
        border: none;
        box-shadow: 0 4px 10px ${token.colorPrimaryBg};
        transition: all 0.3s cubic-bezier(0.2, 0.8, 0.2, 1);
        
        &:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 15px ${token.colorPrimaryBgHover};
        }
    `
}));

export default function DesktopLayout() {
    const navigate = useNavigate();
    const location = useLocation();
    const [data, setData] = useState<any[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [isK, setIsK] = useState<boolean>(false);
    const [provider, setProvider] = useState<any>({});    
    const [total, setTotal] = useState<number>(0);
    const [input, setInput] = useState({
        page: 1,
        pageSize: 10,
        model: '',
        isFirst: true,
        type: ''
    });

    const { styles } = useStyles();
    
    const { token } = theme.useToken();

    const loadProvider = () => {
        getProvider().then((res) => {
            setProvider(res.data);
        })
    }

    const loadData = () => {
        setLoading(true);
        GetModelManagerList(input.model, input.page, input.pageSize, true, input.type)
            .then((res) => {
                setData(res.data.items);
                setTotal(res.data.total);
            })
            .catch(() => {
                message.error('加载模型数据失败');
            })
            .finally(() => {
                setLoading(false);
            });
    }

    const copyModelName = (modelName: string) => {
        try {
            navigator.clipboard.writeText(modelName)
                .then(() => {
                    message.success('复制成功');
                })
                .catch(() => {
                    fallbackCopy(modelName);
                });
        } catch (error) {
            fallbackCopy(modelName);
        }
    };

    const fallbackCopy = (text: string) => {
        const input = document.createElement('input');
        input.value = text;
        document.body.appendChild(input);
        input.select();
        document.execCommand('copy');
        document.body.removeChild(input);
        message.success('复制成功');
    };

    const handleProviderFilter = (key: string) => {
        setInput({
            ...input,
            type: input.type === key ? '' : key,
            page: 1
        });
    };

    const columns = useMemo(() => [
        {
            key: 'icon',
            title: '图标',
            dataIndex: 'icon',
            width: 70,
            align: 'center' as AlignType,
            render: (value: any) => {
                const icon = getIconByName(value);
                return (
                    <motion.div 
                        className="model-icon"
                        whileHover={{ scale: 1.1, rotate: 5 }}
                        transition={{ type: "spring", stiffness: 400, damping: 10 }}
                    >
                        {icon?.icon ?? <IconAvatar size={36} Icon={OpenAI} />}
                    </motion.div>
                );
            }
        },
        {
            key: 'model',
            title: '模型',
            dataIndex: 'model',
            width: 260,
            render: (mode: string) => (
                <div 
                    className={styles.modelName}
                    onClick={() => copyModelName(mode)}
                >
                    {mode}
                </div>
            )
        },
        {
            key: 'isRealTime',
            title: '实时接口',
            dataIndex: 'isVersion2',
            width: 90,
            align: 'center' as AlignType,
            render: (value: boolean) => (
                <MotionTag 
                    color={value ? 'green' : 'default'}
                    whileHover={{ scale: 1.05 }}
                    transition={{ type: "spring", stiffness: 400, damping: 10 }}
                >
                    {value ? '是' : '否'}
                </MotionTag>
            )
        },
        {
            key: 'description',
            title: '描述',
            dataIndex: 'description',
            render: (value: string) => (
                <Tooltip title={value}>
                    <div className="model-description">{value}</div>
                </Tooltip>
            )
        },
        {
            key: 'price',
            title: '模型价格',
            dataIndex: 'price',
            render: (_: any, item: any) => renderPriceInfo(item)
        },
        {
            key: 'quotaType',
            title: '模型类型',
            dataIndex: 'quotaType',
            align: 'center' as AlignType,
            render: (value: number) => (
                <MotionTag 
                    color={value === 1 ? 'blue' : 'orange'}
                    whileHover={{ scale: 1.05 }}
                    transition={{ type: "spring", stiffness: 400, damping: 10 }}
                >
                    {value === 1 ? '按量计费' : '按次计费'}
                </MotionTag>
            )
        },
        {
            key: 'quotaMax',
            title: '最大上文',
            dataIndex: 'quotaMax',
            align: 'center' as AlignType
        },
        {
            key: "tags",
            title: '标签',
            dataIndex: 'tags',
            render: (tags: string[]) => (
                <div className={styles.modelTags}>
                    {tags.map((tag: string, index: number) => (
                        <MotionTag 
                            key={tag} 
                            color='blue'
                            initial={{ opacity: 0, y: 10 }}
                            animate={{ opacity: 1, y: 0 }}
                            transition={{ 
                                delay: index * 0.05,
                                type: "spring", 
                                stiffness: 400, 
                                damping: 10 
                            }}
                            whileHover={{ scale: 1.05 }}
                        >
                            {tag}
                        </MotionTag>
                    ))}
                </div>
            )
        },
        {
            key: 'enable',
            title: '状态',
            dataIndex: 'enable',
            align: 'center' as AlignType,
            render: (value: boolean) => (
                <MotionTag 
                    color={value ? 'green' : 'red'}
                    whileHover={{ scale: 1.05 }}
                    transition={{ type: "spring", stiffness: 400, damping: 10 }}
                >
                    {value ? '可用' : '不可用'}
                </MotionTag>
            )
        }
    ], [isK, styles.modelName, styles.modelTags]);

    const renderPriceInfo = (item: any) => {
        if (item.quotaType === 1) {
            const tokenUnit = isK ? '1K' : '1M';
            const multiplier = isK ? 1000 : 1000000;
            
            return (
                <div className="model-price">
                    <div>
                        <MotionTag 
                            color='cyan'
                            whileHover={{ scale: 1.05 }}
                            transition={{ type: "spring", stiffness: 400, damping: 10 }}
                        >
                            提示{renderQuota(item.promptRate * multiplier, 6)}/{tokenUnit} tokens
                        </MotionTag>
                        <MotionTag 
                            color='geekblue' 
                            style={{ marginTop: 8 }}
                            whileHover={{ scale: 1.05 }}
                            transition={{ type: "spring", stiffness: 400, damping: 10 }}
                        >
                            完成{renderQuota((item.completionRate ? 
                                item.promptRate * multiplier * item.completionRate : 
                                getCompletionRatio(item.model) * multiplier), 6)}/{tokenUnit} tokens
                        </MotionTag>
                    </div>
                    
                    {item.isVersion2 && (
                        <div className="audio-price">
                            <MotionTag 
                                color='cyan'
                                whileHover={{ scale: 1.05 }}
                                transition={{ type: "spring", stiffness: 400, damping: 10 }}
                            >
                                音频输入{renderQuota(item.audioPromptRate * multiplier)}/{tokenUnit} tokens
                            </MotionTag>
                            <MotionTag 
                                color='geekblue' 
                                style={{ marginTop: 8 }}
                                whileHover={{ scale: 1.05 }}
                                transition={{ type: "spring", stiffness: 400, damping: 10 }}
                            >
                                音频完成{renderQuota((item.completionRate ? 
                                    item.audioPromptRate * multiplier * item.audioOutputRate : 
                                    getCompletionRatio(item.model) * multiplier))}/{tokenUnit} tokens
                            </MotionTag>
                        </div>
                    )}
                </div>
            );
        } else {
            return (
                <MotionTag 
                    color='geekblue'
                    whileHover={{ scale: 1.05 }}
                    transition={{ type: "spring", stiffness: 400, damping: 10 }}
                >
                    每次{renderQuota(item.promptRate, 6)}
                </MotionTag>
            );
        }
    };

    useEffect(() => {
        const searchParams = new URLSearchParams(location.search);
        searchParams.set('model', input.model);
        navigate({ search: searchParams.toString() }, { replace: true });
    }, [input.model, navigate]);

    useEffect(() => {
        const searchParams = new URLSearchParams(location.search);
        const model = searchParams.get('model');
        if (model) {
            setInput({ ...input, model, isFirst: false });
        }
    }, []);

    useEffect(() => {
        loadProvider();
    }, []);

    useEffect(() => {
        loadData();
    }, [input.page, input.pageSize, input.type]);

    const hasProviders = useMemo(() => Object.keys(provider).length > 0, [provider]);

    const containerVariants = {
        hidden: { opacity: 0 },
        visible: { 
            opacity: 1,
            transition: { 
                staggerChildren: 0.1,
                delayChildren: 0.2
            }
        }
    };
    
    const itemVariants = {
        hidden: { y: 20, opacity: 0 },
        visible: { 
            y: 0, 
            opacity: 1,
            transition: {
                type: "spring",
                stiffness: 300,
                damping: 24
            }
        }
    };

    const AnimatedBackground = () => (
        <>
            <MotionDiv
                style={{
                    position: 'absolute',
                    width: '50%',
                    height: '50%',
                    borderRadius: '50%',
                    background: `radial-gradient(circle, ${token.colorPrimaryBg} 0%, rgba(0,0,0,0) 70%)`,
                    top: '-20%',
                    right: '-10%',
                    filter: 'blur(80px)',
                    zIndex: 0,
                    opacity: 0.4
                }}
                animate={{
                    x: [0, 30, 0],
                    y: [0, 20, 0],
                }}
                transition={{
                    duration: 20,
                    repeat: Infinity,
                    ease: "easeInOut",
                    repeatType: "reverse"
                }}
            />
            <MotionDiv
                style={{
                    position: 'absolute',
                    width: '40%',
                    height: '40%',
                    borderRadius: '50%',
                    background: `radial-gradient(circle, ${token.colorInfoBg} 0%, rgba(0,0,0,0) 70%)`,
                    bottom: '10%',
                    left: '-10%',
                    filter: 'blur(80px)',
                    zIndex: 0,
                    opacity: 0.4
                }}
                animate={{
                    x: [0, -30, 0],
                    y: [0, 30, 0],
                }}
                transition={{
                    duration: 25,
                    repeat: Infinity,
                    ease: "easeInOut",
                    repeatType: "reverse"
                }}
            />
        </>
    );

    return (
        <MotionDiv 
            className={styles.modelContainer}
            initial="hidden"
            animate="visible"
            variants={containerVariants}
        >
            <AnimatedBackground />
            
            <MotionDiv variants={itemVariants}>
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
                                    onChange={setIsK}
                                />
                            </div>
                            <div className="search-wrapper">
                                <Input
                                    placeholder="请输入需要搜索的模型"
                                    value={input.model}
                                    className={styles.searchInput}
                                    onPressEnter={() => loadData()}
                                    suffix={
                                        <MotionButton
                                            type='primary'
                                            size="small"
                                            className={styles.primaryButton}
                                            icon={<Search size={16} />}
                                            onClick={() => loadData()}
                                            whileHover={{ scale: 1.05 }}
                                            whileTap={{ scale: 0.95 }}
                                        />
                                    }
                                    onChange={(e) => {
                                        setInput({
                                            ...input,
                                            model: e.target.value
                                        });
                                    }}
                                />
                            </div>
                        </div>
                    }
                />
            </MotionDiv>
            
            {hasProviders && (
                <MotionDiv 
                    className={styles.iconFilter}
                    variants={itemVariants}
                >
                    {Object.entries(provider).map(([key, value]: any, index: number) => (
                        <MotionDiv
                            key={key}
                            className={`${styles.iconItem} ${input.type === key ? 'selected' : ''}`}
                            onClick={() => handleProviderFilter(key)}
                            whileHover={{ scale: 1.05 }}
                            whileTap={{ scale: 0.95 }}
                            initial={{ opacity: 0, y: 20 }}
                            animate={{ opacity: 1, y: 0 }}
                            transition={{ 
                                delay: index * 0.05,
                                type: "spring", 
                                stiffness: 300, 
                                damping: 24 
                            }}
                        >
                            <Flexbox horizontal gap={8}>
                                {getIconByName(key, 24)?.icon ?? <IconAvatar size={24} Icon={OpenAI} />}
                                <span className="provider-name" style={{
                                    overflow: 'hidden',
                                    textOverflow: 'ellipsis',
                                    display: '-webkit-box',
                                    WebkitLineClamp: 1,
                                    WebkitBoxOrient: 'vertical',
                                    maxWidth: 120,
                                    color: input.type === key ? token.colorTextLightSolid : token.colorText
                                }}>
                                    {value || '其他'}
                                </span>
                            </Flexbox>
                        </MotionDiv>
                    ))}
                </MotionDiv>
            )}
            
            <MotionDiv 
                className={styles.tableContainer}
                variants={itemVariants}
            >
                <Table
                    rowKey={row => row.id}
                    pagination={{
                        total: total,
                        pageSize: input.pageSize,
                        current: input.page,
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
                    locale={{
                        emptyText: <Empty description="暂无数据" image={Empty.PRESENTED_IMAGE_SIMPLE} />
                    }}
                    columns={columns}
                />
            </MotionDiv>
            
            <style>{`
                .model-manager-container {
                    display: flex;
                    flex-direction: column;
                    height: 100%;
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
                    color: ${token.colorTextSecondary};
                }
                
                .search-wrapper {
                    position: relative;
                }
                
                .model-table {
                    margin: 0;
                }
                
                .model-icon {
                    display: flex;
                    justify-content: center;
                    align-items: center;
                }
                
                .model-description {
                    overflow: hidden;
                    text-overflow: ellipsis;
                    display: -webkit-box;
                    -webkit-line-clamp: 1;
                    -webkit-box-orient: vertical;
                }
                
                .model-price {
                    display: flex;
                    flex-direction: column;
                    gap: 8px;
                }
                
                .audio-price {
                    margin-top: 4px;
                }
                
                /* 滚动条美化 */
                ::-webkit-scrollbar {
                    width: 8px;
                    height: 8px;
                }
                
                ::-webkit-scrollbar-track {
                    background: rgba(0, 0, 0, 0.05);
                    border-radius: 4px;
                }
                
                ::-webkit-scrollbar-thumb {
                    background: rgba(0, 0, 0, 0.2);
                    border-radius: 4px;
                }
                
                ::-webkit-scrollbar-thumb:hover {
                    background: rgba(0, 0, 0, 0.3);
                }
                
                @media (max-width: 768px) {
                    .header-actions {
                        flex-direction: column;
                        align-items: flex-start;
                    }
                    
                    .icon-filter {
                        overflow-x: auto;
                        flex-wrap: nowrap;
                        padding: 12px;
                    }
                    
                    .table-container {
                        margin: 12px;
                        padding: 12px;
                    }
                }
            `}</style>
        </MotionDiv>
    );
}
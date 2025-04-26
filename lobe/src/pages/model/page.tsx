import { useEffect, useMemo, useState } from "react";
import { Header, Input, Tag, Tooltip } from "@lobehub/ui";
import { IconAvatar, OpenAI } from "@lobehub/icons";
import { Button, Card, Col, Descriptions, Divider, Empty, Modal, Row, Select, Space, Switch, Typography, message, theme, Badge } from "antd";
import { getCompletionRatio, renderQuota } from "../../utils/render";
import { getIconByName } from "../../utils/iconutils";
import { GetModelManagerList } from "../../services/ModelManagerService";
import { Copy, Info, Search, Tag as TagIcon, MessageSquare, Image, Mic, Headphones, FileText, Layers } from "lucide-react";
import { useLocation, useNavigate } from "react-router-dom";
import { Flexbox } from "react-layout-kit";
import { getProvider } from "../../services/ModelService";
import { motion } from "framer-motion";
import { createStyles } from "antd-style";
import useBreakpoint from "antd/es/grid/hooks/useBreakpoint";
import { getModelInfo } from "../../services/ModelService";
import { getTags } from "../../services/ChannelService";
import MODEL_TYPES from "../model-manager/constants/modelTypes";

const { Title, Paragraph } = Typography;
const MotionTag = motion(Tag);
const MotionButton = motion(Button);
const MotionDiv = motion.div;
const MotionCard = motion(Card);

const useStyles = createStyles(({ token, css }) => ({
    modelContainer: css`
        display: flex;
        flex-direction: column;
        height: 100%;
        background: linear-gradient(145deg, ${token.colorBgContainer} 0%, ${token.colorBgElevated} 100%);
        position: relative;
        overflow: hidden;
    `,
    searchContainer: css`
        display: flex;
        align-items: center;
        gap: 12px;
        margin: 16px 16px 0 16px;
        padding: 16px;
        border-radius: ${token.borderRadius}px;
        background-color: ${token.colorBgContainer};
        box-shadow: ${token.boxShadowSecondary};
        position: relative;
        z-index: 2;
        
        @media (max-width: 768px) {
            flex-direction: column;
            align-items: flex-start;
            padding: 12px;
            margin: 12px 12px 0 12px;
        }
    `,
    searchInput: css`
        width: 240px;
        transition: all 0.3s;
        &:focus, &:hover {
            width: 280px;
            box-shadow: 0 0 8px ${token.colorPrimaryBg};
        }
        
        @media (max-width: 768px) {
            width: 100%;
            &:focus, &:hover {
                width: 100%;
            }
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
        
        @media (max-width: 768px) {
            overflow-x: auto;
            flex-wrap: nowrap;
            padding: 12px;
            margin: 12px 12px 0 12px;
        }
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
    modelTypeFilter: css`
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
        margin: 16px 16px 0 16px;
        padding: 16px;
        border-radius: ${token.borderRadius}px;
        background-color: ${token.colorBgContainer};
        box-shadow: ${token.boxShadowSecondary};
        position: relative;
        z-index: 1;
        
        @media (max-width: 768px) {
            overflow-x: auto;
            flex-wrap: nowrap;
            padding: 12px;
            margin: 12px 12px 0 12px;
        }
    `,
    modelTypeFilterItem: css`
        cursor: pointer;
        padding: 8px 12px;
        border-radius: ${token.borderRadiusSM}px;
        transition: all 0.3s;
        border: 1px solid ${token.colorBorderSecondary};
        display: flex;
        align-items: center;
        gap: 8px;
        
        &:hover {
            border-color: ${token.colorPrimaryBorderHover};
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
        }
        
        &.selected {
            border-color: ${token.colorPrimary};
            background-color: ${token.colorPrimaryBg};
            transform: translateY(-2px);
            box-shadow: 0 4px 12px ${token.colorPrimaryBgHover};
        }
    `,
    resultSummary: css`
        margin: 16px 16px 0;
        font-size: 14px;
        color: ${token.colorTextSecondary};
        display: flex;
        justify-content: space-between;
        align-items: center;
        
        @media (max-width: 768px) {
            margin: 12px 12px 0;
            flex-direction: column;
            align-items: flex-start;
            gap: 8px;
        }
    `,
    modelsContainer: css`
        margin: 16px;
        flex: 1;
        overflow: auto;
        overflow-x: hidden !important;
        position: relative;
        z-index: 1;
        
        @media (max-width: 768px) {
            margin: 12px;
        }
    `,
    modelCard: css`
        border-radius: ${token.borderRadius}px;
        overflow: hidden;
        height: 100%;
        transition: all 0.3s cubic-bezier(0.2, 0.8, 0.2, 1);
        backdrop-filter: blur(10px);
        background-color: ${token.colorBgContainer};
        box-shadow: ${token.boxShadowSecondary};
        display: flex;
        flex-direction: column;
        
        &:hover {
            transform: translateY(-4px);
            box-shadow: 0 12px 24px rgba(0, 0, 0, 0.12);
        }
    `,
    modelCardMeta: css`
        display: flex;
        align-items: center;
        margin-bottom: 12px;
    `,
    modelCardContent: css`
        flex: 1;
        display: flex;
        flex-direction: column;
    `,
    modelCardFooter: css`
        margin-top: auto;
        padding-top: 12px;
        border-top: 1px solid ${token.colorBorderSecondary};
    `,
    modelCardHeader: css`
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-bottom: 12px;
    `,
    modelCardSection: css`
        margin-bottom: 8px;
    `,
    modelGridView: css`
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
        gap: 16px;
        
        @media (max-width: 768px) {
            grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
            gap: 12px;
        }
    `,
    viewToggle: css`
        display: flex;
        align-items: center;
        gap: 8px;
        margin-left: auto;
    `,
    modelName: css`
        cursor: pointer;
        color: ${token.colorPrimary};
        transition: color 0.3s;
        font-weight: 500;
        font-size: 14px;
        max-width: 180px;
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
    modelDescription: css`
        font-size: 12px;
        color: ${token.colorTextSecondary};
        margin-bottom: 12px;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
        overflow: hidden;
        height: 36px;
    `,
    modelTags: css`
        display: flex;
        flex-wrap: wrap;
        gap: 4px;
        margin-bottom: 8px;
    `,
    modelPriceSection: css`
        margin-top: 12px;
    `,
    modelStats: css`
        display: flex;
        justify-content: space-between;
        margin-top: 12px;
    `,
    modelStat: css`
        text-align: center;
    `,
    modelStatValue: css`
        font-weight: 500;
        color: ${token.colorText};
        font-size: 13px;
    `,
    modelStatLabel: css`
        font-size: 11px;
        color: ${token.colorTextSecondary};
    `,
    pagination: css`
        margin-top: 24px;
        text-align: center;
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
    `,
    tagSelect: css`
        min-width: 200px;
        
        @media (max-width: 768px) {
            width: 100%;
        }
    `,
    modelDetailModal: css`
        .ant-modal-content {
            background: linear-gradient(145deg, ${token.colorBgContainer} 0%, ${token.colorBgElevated} 100%);
            border-radius: ${token.borderRadiusLG}px;
            overflow: hidden;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
        }
        
        .ant-modal-header {
            background: transparent;
            border-bottom: 1px solid ${token.colorBorderSecondary};
            padding: 16px 24px;
        }
        
        .ant-modal-body {
            padding: 24px;
            max-height: 70vh;
            overflow-y: auto;
        }
        
        .ant-modal-footer {
            border-top: 1px solid ${token.colorBorderSecondary};
            padding: 16px 24px;
        }
        
        @media (max-width: 768px) {
            .ant-modal-body {
                padding: 16px;
                max-height: 80vh;
            }
        }
    `,
    modelDetailHeader: css`
        display: flex;
        align-items: center;
        margin-bottom: 24px;
        gap: 16px;
        
        @media (max-width: 768px) {
            flex-direction: column;
            align-items: flex-start;
            gap: 12px;
        }
    `,
    modelDetailIcon: css`
        display: flex;
        align-items: center;
        justify-content: center;
        width: 64px;
        height: 64px;
        background-color: ${token.colorBgContainer};
        border-radius: ${token.borderRadiusLG}px;
        box-shadow: ${token.boxShadowSecondary};
        padding: 12px;
        
        @media (max-width: 768px) {
            width: 48px;
            height: 48px;
            padding: 8px;
        }
    `,
    modelDetailInfo: css`
        flex: 1;
    `,
    modelDetailName: css`
        display: flex;
        align-items: center;
        gap: 8px;
        margin-bottom: 4px;
    `,
    modelDetailTitle: css`
        margin: 0;
        font-size: 20px;
        font-weight: 600;
        color: ${token.colorText};
        
        @media (max-width: 768px) {
            font-size: 18px;
        }
    `,
    modelDetailDescription: css`
        color: ${token.colorTextSecondary};
        font-size: 14px;
    `,
    modelDetailTags: css`
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
        margin-top: 16px;
        
        @media (max-width: 768px) {
            margin-top: 12px;
        }
    `,
    modelDetailSection: css`
        margin-bottom: 24px;
        
        @media (max-width: 768px) {
            margin-bottom: 16px;
        }
    `,
    modelDetailSectionTitle: css`
        font-size: 16px;
        font-weight: 500;
        margin-bottom: 16px;
        color: ${token.colorTextHeading};
        display: flex;
        align-items: center;
        gap: 8px;
        
        @media (max-width: 768px) {
            font-size: 15px;
            margin-bottom: 12px;
        }
    `,
    modelDetailPricing: css`
        display: flex;
        flex-wrap: wrap;
        gap: 16px;
        
        @media (max-width: 768px) {
            gap: 12px;
        }
    `,
    modelDetailPriceCard: css`
        background-color: ${token.colorBgContainer};
        border-radius: ${token.borderRadius}px;
        padding: 16px;
        flex: 1;
        min-width: 200px;
        box-shadow: ${token.boxShadowSecondary};
        
        @media (max-width: 768px) {
            min-width: 100%;
            padding: 12px;
        }
    `,
    modelDetailPriceTitle: css`
        font-size: 14px;
        font-weight: 500;
        color: ${token.colorTextSecondary};
        margin-bottom: 8px;
    `,
    modelDetailPriceValue: css`
        font-size: 18px;
        font-weight: 600;
        color: ${token.colorPrimary};
        
        @media (max-width: 768px) {
            font-size: 16px;
        }
    `,
    modelDetailStats: css`
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
        gap: 16px;
        
        @media (max-width: 768px) {
            grid-template-columns: repeat(2, 1fr);
            gap: 12px;
        }
    `,
    modelDetailStat: css`
        background-color: ${token.colorBgContainer};
        border-radius: ${token.borderRadius}px;
        padding: 16px;
        text-align: center;
        box-shadow: ${token.boxShadowSecondary};
        
        @media (max-width: 768px) {
            padding: 12px;
        }
    `,
    modelDetailStatValue: css`
        font-size: 18px;
        font-weight: 600;
        color: ${token.colorText};
        
        @media (max-width: 768px) {
            font-size: 16px;
        }
    `,
    modelDetailStatLabel: css`
        font-size: 13px;
        color: ${token.colorTextSecondary};
        margin-top: 4px;
    `,
    copyButton: css`
        display: inline-flex;
        align-items: center;
        justify-content: center;
        width: 28px;
        height: 28px;
        border-radius: ${token.borderRadius}px;
        background-color: ${token.colorBgContainer};
        border: 1px solid ${token.colorBorder};
        cursor: pointer;
        transition: all 0.3s;
        
        &:hover {
            background-color: ${token.colorBgTextHover};
            border-color: ${token.colorPrimaryBorder};
        }
        
        @media (max-width: 768px) {
            width: 24px;
            height: 24px;
        }
    `,
    modelTypeIcon: css`
        display: flex;
        align-items: center;
        justify-content: center;
        width: 24px;
        height: 24px;
        border-radius: 50%;
        background-color: ${token.colorPrimaryBg};
        margin-right: 4px;
    `,
    modelTypeBadge: css`
        position: absolute;
        top: 12px;
        right: 12px;
    `,
    modelTypeSummary: css`
        display: flex;
        gap: 12px;
        margin: 16px 16px 0 16px;
        padding: 16px;
        border-radius: ${token.borderRadius}px;
        background-color: ${token.colorBgContainer};
        box-shadow: ${token.boxShadowSecondary};
        position: relative;
        z-index: 1;
        overflow-x: auto;
        
        @media (max-width: 768px) {
            flex-wrap: nowrap;
        }
        
        @media (min-width: 769px) {
            flex-wrap: wrap;
        }
    `,
}));

export default function DesktopLayout() {
    const navigate = useNavigate();
    const location = useLocation();
    const [data, setData] = useState<any[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [isK, setIsK] = useState<boolean>(false);
    const [provider, setProvider] = useState<any>({});    
    const [total, setTotal] = useState<number>(0);
    const [allTags, setAllTags] = useState<string[]>([]);
    const [selectedModel, setSelectedModel] = useState<any>(null);
    const [modalVisible, setModalVisible] = useState<boolean>(false);
    const [input, setInput] = useState({
        page: 1,
        pageSize: 16,
        model: '',
        isFirst: true,
        type: '',
        tags: [] as string[],
        modelType: ''
    });
    const [selectedModelType, setSelectedModelType] = useState<string>('');
    const [isGridView, setIsGridView] = useState<boolean>(true);
    const modelTypes = useMemo(() => Object.values(MODEL_TYPES), []);
    
    const { styles } = useStyles();
    const { token } = theme.useToken();
    const screens = useBreakpoint();
    const isMobile = !screens.md;
    const [modelInfo, setModelInfo] = useState<any>({});

    useEffect(() => {
        loadModelInfo();
    }, []);

    function loadModelInfo() {
        getModelInfo()
            .then((res) => {
                setModelInfo(res.data);
            });
    }
    const loadProvider = () => {
        getProvider().then((res) => {
            setProvider(res.data);
        })
    }

    const loadData = () => {
        setLoading(true);
        GetModelManagerList(input.model, input.page, input.pageSize, true, input.type, input.modelType)
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

    const loadTags = () => {
        getTags()
            .then((res) => {
                if (res.data && Array.isArray(res.data)) {
                    setAllTags(res.data);
                }
            })
            .catch((error) => {
                console.error('Failed to load tags:', error);
                message.error('加载标签失败');
            });
    }

    const filterDataByTags = () => {
        if (!input.tags || input.tags.length === 0) return data;
        
        return data.filter(item => {
            if (!item.tags || !Array.isArray(item.tags)) return false;
            return input.tags.some(tag => item.tags.includes(tag));
        });
    }

    // Count models by type
    const modelCountByType = useMemo(() => {
        const counts: Record<string, number> = {};
        data.forEach(item => {
            if (item.type) {
                counts[item.type] = (counts[item.type] || 0) + 1;
            }
        });
        return counts;
    }, [data]);
    
    // Filter data by model type
    const filterDataByModelType = (data: any[]) => {
        if (!input.modelType) return data;
        return data.filter(item => item.type?.toLowerCase() === input.modelType.toLowerCase());
    };
    
    // Combined filters
    const filteredData = useMemo(() => {
        let filtered = filterDataByTags();
        filtered = filterDataByModelType(filtered);
        return filtered;
    }, [data, input.tags, input.modelType]);
    
    const handleModelTypeSelect = (type: string) => {
        const newType = selectedModelType === type ? '' : type;
        setSelectedModelType(newType);
        // When a model type is selected, update the input.modelType to filter the data
        setInput({
            ...input,
            modelType: newType, // Toggle the model type filter
            page: 1 // Reset to first page
        });
    };

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

    const handleTagsChange = (tags: string[]) => {
        setInput({
            ...input,
            tags,
            page: 1
        });
    };

    const handleModelClick = (item: any) => {
        setSelectedModel(item);
        setModalVisible(true);
    };

    const handleModalClose = () => {
        setModalVisible(false);
    };

    const renderPriceInfo = (item: any) => {
        if (item.quotaType === 1) {
            const tokenUnit = isK ? '1K' : '1M';
            const multiplier = isK ? 1000 : 1000000;
            
            return (
                <div className="model-price" style={{ display: 'flex', flexDirection: 'column', gap: '4px' }}>
                    <div style={{ display: 'flex', flexWrap: 'wrap', gap: '4px' }}>
                        <MotionTag 
                            color='cyan'
                            style={{ fontSize: '11px', display: 'flex', alignItems: 'center', gap: '2px' }}
                            whileHover={{ scale: 1.05 }}
                            transition={{ type: "spring", stiffness: 400, damping: 10 }}
                        >
                            <span style={{ fontWeight: 'bold' }}>提示:</span>
                            <span>{renderQuota(item.promptRate * multiplier, 6)}/{tokenUnit}</span>
                        </MotionTag>
                        <MotionTag 
                            color='geekblue' 
                            style={{ fontSize: '11px', display: 'flex', alignItems: 'center', gap: '2px' }}
                            whileHover={{ scale: 1.05 }}
                            transition={{ type: "spring", stiffness: 400, damping: 10 }}
                        >
                            <span style={{ fontWeight: 'bold' }}>完成:</span>
                            <span>{renderQuota((item.completionRate ? 
                                item.promptRate * multiplier * item.completionRate : 
                                getCompletionRatio(item.model) * multiplier), 6)}/{tokenUnit}</span>
                        </MotionTag>
                    </div>
                    
                    {item.isVersion2 && (
                        <div style={{ display: 'flex', flexWrap: 'wrap', gap: '4px', marginTop: '4px' }}>
                            <MotionTag 
                                color='cyan'
                                style={{ fontSize: '11px', display: 'flex', alignItems: 'center', gap: '2px' }}
                                whileHover={{ scale: 1.05 }}
                                transition={{ type: "spring", stiffness: 400, damping: 10 }}
                            >
                                <span style={{ fontWeight: 'bold' }}>音频输入:</span>
                                <span>{renderQuota(item.audioPromptRate * multiplier)}/{tokenUnit}</span>
                            </MotionTag>
                            <MotionTag 
                                color='geekblue' 
                                style={{ fontSize: '11px', display: 'flex', alignItems: 'center', gap: '2px' }}
                                whileHover={{ scale: 1.05 }}
                                transition={{ type: "spring", stiffness: 400, damping: 10 }}
                            >
                                <span style={{ fontWeight: 'bold' }}>音频完成:</span>
                                <span>{renderQuota((item.completionRate ? 
                                    item.audioPromptRate * multiplier * item.audioOutputRate : 
                                    getCompletionRatio(item.model) * multiplier))}/{tokenUnit}</span>
                            </MotionTag>
                        </div>
                    )}
                </div>
            );
        } else {
            return (
                <MotionTag 
                    color='geekblue'
                    style={{ fontSize: '11px', display: 'flex', alignItems: 'center', gap: '2px' }}
                    whileHover={{ scale: 1.05 }}
                    transition={{ type: "spring", stiffness: 400, damping: 10 }}
                >
                    <span style={{ fontWeight: 'bold' }}>每次:</span>
                    <span>{renderQuota(item.promptRate, 6)}</span>
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
    }, [input.page, input.pageSize, input.type, input.modelType]);
    
    useEffect(() => {
        loadTags();
    }, []);

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

    const cardVariants = {
        hidden: { y: 20, opacity: 0 },
        visible: (i: number) => ({ 
            y: 0, 
            opacity: 1,
            transition: {
                delay: i * 0.05,
                type: "spring",
                stiffness: 300,
                damping: 24
            }
        })
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

    const renderAllTags = (tags: string[]) => {
        if (!tags || !Array.isArray(tags) || tags.length === 0) return null;
        
        return (
            <div className={styles.modelDetailTags}>
                {tags.map((tag, index) => (
                    <MotionTag
                        key={tag}
                        color="blue"
                        initial={{ opacity: 0, scale: 0.8 }}
                        animate={{ opacity: 1, scale: 1 }}
                        transition={{ delay: index * 0.05 }}
                        whileHover={{ scale: 1.05 }}
                    >
                        {tag}
                    </MotionTag>
                ))}
            </div>
        );
    };

    const renderDetailedPriceInfo = (item: any) => {
        if (!item) return null;
        
        const tokenUnit = isK ? '1K' : '1M';
        const multiplier = isK ? 1000 : 1000000;
        
        if (item.quotaType === 1) {
            return (
                <div className={styles.modelDetailPricing}>
                    <MotionDiv 
                        className={styles.modelDetailPriceCard}
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: 0.1 }}
                    >
                        <div className={styles.modelDetailPriceTitle}>提示词价格</div>
                        <div className={styles.modelDetailPriceValue}>
                            {renderQuota(item.promptRate * multiplier, 6)}/{tokenUnit}
                        </div>
                    </MotionDiv>
                    
                    <MotionDiv 
                        className={styles.modelDetailPriceCard}
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: 0.2 }}
                    >
                        <div className={styles.modelDetailPriceTitle}>完成价格</div>
                        <div className={styles.modelDetailPriceValue}>
                            {renderQuota((item.completionRate ? 
                                item.promptRate * multiplier * item.completionRate : 
                                getCompletionRatio(item.model) * multiplier), 6)}/{tokenUnit}
                        </div>
                    </MotionDiv>
                    
                    {item.isVersion2 && (
                        <>
                            <MotionDiv 
                                className={styles.modelDetailPriceCard}
                                initial={{ opacity: 0, y: 20 }}
                                animate={{ opacity: 1, y: 0 }}
                                transition={{ delay: 0.3 }}
                            >
                                <div className={styles.modelDetailPriceTitle}>音频输入价格</div>
                                <div className={styles.modelDetailPriceValue}>
                                    {renderQuota(item.audioPromptRate * multiplier)}/{tokenUnit}
                                </div>
                            </MotionDiv>
                            
                            <MotionDiv 
                                className={styles.modelDetailPriceCard}
                                initial={{ opacity: 0, y: 20 }}
                                animate={{ opacity: 1, y: 0 }}
                                transition={{ delay: 0.4 }}
                            >
                                <div className={styles.modelDetailPriceTitle}>音频完成价格</div>
                                <div className={styles.modelDetailPriceValue}>
                                    {renderQuota((item.completionRate ? 
                                        item.audioPromptRate * multiplier * item.audioOutputRate : 
                                        getCompletionRatio(item.model) * multiplier))}/{tokenUnit}
                                </div>
                            </MotionDiv>
                        </>
                    )}
                </div>
            );
        } else {
            return (
                <div className={styles.modelDetailPricing}>
                    <MotionDiv 
                        className={styles.modelDetailPriceCard}
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: 0.1 }}
                    >
                        <div className={styles.modelDetailPriceTitle}>每次调用价格</div>
                        <div className={styles.modelDetailPriceValue}>
                            {renderQuota(item.promptRate, 6)}
                        </div>
                    </MotionDiv>
                </div>
            );
        }
    };

    const renderModelStats = (item: any) => {
        if (!item) return null;
        
        // Define stats to display
        const stats = [
            { label: '最大上下文', value: item.quotaMax || '-' },
            { label: '模型类型', value: item.type },
            { label: '计费方式', value: item.quotaType === 1 ? '按量计费' : '按次计费' },
            { label: '状态', value: item.enable ? '可用' : '不可用' },
        ];
        
        return (
            <div className={styles.modelDetailStats}>
                {stats.map((stat, index) => (
                    <MotionDiv 
                        key={stat.label}
                        className={styles.modelDetailStat}
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: 0.1 + index * 0.1 }}
                    >
                        <div className={styles.modelDetailStatValue}>{stat.value}</div>
                        <div className={styles.modelDetailStatLabel}>{stat.label}</div>
                    </MotionDiv>
                ))}
            </div>
        );
    };

    const getModelTypeIcon = (type: string) => {
        switch(type?.toLowerCase()) {
            case MODEL_TYPES.CHAT:
                return <MessageSquare size={14} color={token.colorPrimary} />;
            case MODEL_TYPES.IMAGE:
                return <Image size={14} color={token.colorSuccess} />;
            case MODEL_TYPES.AUDIO:
                return <Headphones size={14} color={token.colorWarning} />;
            case MODEL_TYPES.STT:
                return <Mic size={14} color={token.colorInfo} />;
            case MODEL_TYPES.TTS:
                return <Headphones size={14} color={token.colorWarning} />;
            case MODEL_TYPES.EMBEDDING:
                return <Layers size={14} color={token.colorError} />;
            default:
                return <FileText size={14} color={token.colorTextSecondary} />;
        }
    };

    const getModelTypeColor = (type: string) => {
        switch(type?.toLowerCase()) {
            case MODEL_TYPES.CHAT:
                return 'blue';
            case MODEL_TYPES.IMAGE:
                return 'green';
            case MODEL_TYPES.AUDIO:
                return 'gold';
            case MODEL_TYPES.STT:
                return 'cyan';
            case MODEL_TYPES.TTS:
                return 'orange';
            case MODEL_TYPES.EMBEDDING:
                return 'magenta';
            default:
                return 'default';
        }
    };

    const renderModelDetailModal = () => {
        if (!selectedModel) return null;
        
        const icon = getIconByName(selectedModel.icon);
        const modelTypeIcon = getModelTypeIcon(selectedModel.type);
        const modelTypeColor = getModelTypeColor(selectedModel.type);
        
        return (
            <Modal
                title={null}
                open={modalVisible}
                onCancel={handleModalClose}
                footer={[
                    <Button key="back" onClick={handleModalClose}>
                        关闭
                    </Button>,
                    <Button 
                        key="copy" 
                        type="primary"
                        onClick={() => copyModelName(selectedModel.model)}
                        className={styles.primaryButton}
                    >
                        复制模型名称
                    </Button>,
                ]}
                width={800}
                className={styles.modelDetailModal}
                destroyOnClose
                centered
            >
                <div className={styles.modelDetailHeader}>
                    <MotionDiv 
                        className={styles.modelDetailIcon}
                        initial={{ scale: 0.8, opacity: 0 }}
                        animate={{ scale: 1, opacity: 1 }}
                        transition={{ type: "spring", stiffness: 300, damping: 20 }}
                    >
                        {icon?.icon ?? <IconAvatar size={isMobile ? 32 : 40} Icon={OpenAI} />}
                    </MotionDiv>
                    
                    <div className={styles.modelDetailInfo}>
                        <div className={styles.modelDetailName}>
                            <Title level={4} className={styles.modelDetailTitle}>
                                {selectedModel.model}
                            </Title>
                            <MotionDiv 
                                className={styles.copyButton}
                                whileHover={{ scale: 1.1 }}
                                whileTap={{ scale: 0.9 }}
                                onClick={() => copyModelName(selectedModel.model)}
                            >
                                <Copy size={16} color={token.colorTextSecondary} />
                            </MotionDiv>
                            <MotionTag 
                                color={modelTypeColor}
                                style={{ marginLeft: 8 }}
                                icon={modelTypeIcon}
                            >
                                {selectedModel.type}
                            </MotionTag>
                        </div>
                        <Paragraph className={styles.modelDetailDescription}>
                            {selectedModel.description}
                        </Paragraph>
                        {renderAllTags(selectedModel.tags)}
                    </div>
                </div>
                
                <Divider />
                
                <div className={styles.modelDetailSection}>
                    <div className={styles.modelDetailSectionTitle}>
                        <Info size={16} color={token.colorTextSecondary} />
                        模型统计
                    </div>
                    {renderModelStats(selectedModel)}
                </div>
                
                <div className={styles.modelDetailSection}>
                    <div className={styles.modelDetailSectionTitle}>
                        <Info size={16} color={token.colorTextSecondary} />
                        价格信息 ({isK ? 'K' : 'M'} 单位)
                        <Switch
                            checked={isK}
                            size="small"
                            style={{ marginLeft: 8 }}
                            checkedChildren="K"
                            unCheckedChildren="M"
                            onChange={setIsK}
                        />
                    </div>
                    {renderDetailedPriceInfo(selectedModel)}
                </div>
                
                {selectedModel.capabilities && (
                    <div className={styles.modelDetailSection}>
                        <div className={styles.modelDetailSectionTitle}>
                            <Info size={16} color={token.colorTextSecondary} />
                            模型能力
                        </div>
                        <Descriptions
                            bordered
                            column={{ xs: 1, sm: 2 }}
                            size="small"
                        >
                            {Object.entries(selectedModel.capabilities).map(([key, value]: any) => (
                                <Descriptions.Item key={key} label={key}>
                                    {value ? (
                                        <Tag color="green">支持</Tag>
                                    ) : (
                                        <Tag color="red">不支持</Tag>
                                    )}
                                </Descriptions.Item>
                            ))}
                        </Descriptions>
                    </div>
                )}
            </Modal>
        );
    };

    const renderModelCard = (item: any, index: number) => {
        const icon = getIconByName(item.icon);
        const modelTypeIcon = getModelTypeIcon(item.type);
        const modelTypeColor = getModelTypeColor(item.type);
        
        return (
            <Col xs={12} sm={8} md={6} lg={6} xl={4} xxl={3} key={item.id}>
                <MotionCard
                    className={styles.modelCard}
                    loading={loading}
                    custom={index}
                    variants={cardVariants}
                    whileHover={{ y: -8, boxShadow: "0 12px 24px rgba(0, 0, 0, 0.15)" }}
                    size="small"
                    onClick={() => handleModelClick(item)}
                    style={{ cursor: 'pointer', position: 'relative' }}
                >
                    <Badge 
                        className={styles.modelTypeBadge}
                        count={
                            <Tooltip title={`模型类型: ${item.type}`}>
                                <div style={{ 
                                    backgroundColor: token.colorBgContainer,
                                    borderRadius: '50%',
                                    width: 28, 
                                    height: 28,
                                    display: 'flex',
                                    alignItems: 'center',
                                    justifyContent: 'center',
                                    boxShadow: token.boxShadowSecondary
                                }}>
                                    {modelTypeIcon}
                                </div>
                            </Tooltip>
                        }
                    />
                    
                    <div className={styles.modelCardMeta}>
                        <motion.div 
                            whileHover={{ scale: 1.1, rotate: 5 }}
                            transition={{ type: "spring", stiffness: 400, damping: 10 }}
                            style={{ marginRight: 8 }}
                        >
                            {icon?.icon ?? <IconAvatar size={28} Icon={OpenAI} />}
                        </motion.div>
                        <div 
                            className={styles.modelName}
                            onClick={(e) => {
                                e.stopPropagation();
                                copyModelName(item.model);
                            }}
                        >
                            {item.model}
                        </div>
                    </div>

                    <Tooltip title={item.description}>
                        <div className={styles.modelDescription}>
                            {item.description}
                        </div>
                    </Tooltip>

                    <div className={styles.modelTags}>
                        <MotionTag 
                            color={item.enable ? 'green' : 'red'}
                            whileHover={{ scale: 1.05 }}
                            style={{ fontSize: '11px' }}
                        >
                            {item.enable ? '可用' : '不可用'}
                        </MotionTag>
                        <MotionTag 
                            color={modelTypeColor}
                            whileHover={{ scale: 1.05 }}
                            style={{ fontSize: '11px' }}
                            icon={modelTypeIcon}
                        >
                            {item.type}
                        </MotionTag>
                        <MotionTag 
                            color={item.quotaType === 1 ? 'blue' : 'orange'}
                            whileHover={{ scale: 1.05 }}
                            style={{ fontSize: '11px' }}
                        >
                            {item.quotaType === 1 ? '按量' : '按次'}
                        </MotionTag>
                    </div>

                    {item.tags && item.tags.length > 0 && (
                        <div className={styles.modelTags}>
                            {item.tags.slice(0, 2).map((tag: string, idx: number) => (
                                <MotionTag 
                                    key={tag} 
                                    color='blue'
                                    style={{ fontSize: '11px' }}
                                    initial={{ opacity: 0, y: 10 }}
                                    animate={{ opacity: 1, y: 0 }}
                                    transition={{ delay: idx * 0.05 }}
                                    whileHover={{ scale: 1.05 }}
                                >
                                    {tag}
                                </MotionTag>
                            ))}
                            {item.tags.length > 2 && (
                                <MotionTag color="default" style={{ fontSize: '11px' }}>+{item.tags.length - 2}</MotionTag>
                            )}
                        </div>
                    )}

                    <div style={{ marginTop: 'auto' }}>
                        <div className={styles.modelPriceSection}>
                            {renderPriceInfo(item)}
                        </div>

                        <div className={styles.modelStats}>
                            <div className={styles.modelStat}>
                                <div className={styles.modelStatValue}>{item.quotaMax || '-'}</div>
                                <div className={styles.modelStatLabel}>最大上下文</div>
                            </div>
                        </div>
                    </div>
                </MotionCard>
            </Col>
        );
    };

    const renderSearchSection = () => (
        <MotionDiv 
            className={styles.searchContainer}
            variants={itemVariants}
        >
            <Space wrap={isMobile} style={{ width: isMobile ? '100%' : 'auto' }}>
                <Input
                    placeholder="请输入需要搜索的模型"
                    value={input.model}
                    className={styles.searchInput}
                    onPressEnter={() => {
                        loadData();
                        loadTags();
                    }}
                    prefix={<Search size={16} color={token.colorTextSecondary} />}
                    onChange={(e) => {
                        setInput({
                            ...input,
                            model: e.target.value
                        });
                    }}
                />
                    
                <Select
                    mode="multiple"
                    allowClear
                    placeholder="选择标签过滤"
                    value={input.tags}
                    onChange={handleTagsChange}
                    className={styles.tagSelect}
                    maxTagCount={3}
                    options={allTags.map(tag => ({
                        label: tag,
                        value: tag,
                    }))}
                    suffixIcon={<TagIcon size={16} color={token.colorTextSecondary} />}
                />
                
                <MotionButton
                    type='primary'
                    icon={<Search size={16} />}
                    className={styles.primaryButton}
                    onClick={() => {
                        loadData();
                        loadTags();
                    }}
                    whileHover={{ scale: 1.05 }}
                    whileTap={{ scale: 0.95 }}
                >
                    搜索
                </MotionButton>
            </Space>
            
            <div style={{ marginLeft: 'auto' }}>
                <Space>
                    <span className="switch-label">单位：</span>
                    <Switch
                        value={isK}
                        checkedChildren={<Tag color="blue">K</Tag>}
                        unCheckedChildren={<Tag color="green">M</Tag>}
                        defaultChecked
                        onChange={setIsK}
                    />
                </Space>
            </div>
        </MotionDiv>
    );

    // Render model type summary
    const renderModelTypeSummary = () => {
        if (!modelInfo.modelTypeCounts || modelInfo.modelTypeCounts.length === 0) return null;
        
        return (
            <MotionDiv 
                className={styles.modelTypeSummary}
                variants={itemVariants}
            >
                {modelInfo.modelTypeCounts.map((item: any, index: number) => {
                    const type = item.type;
                    const count = item.count;
                    const typeIcon = getModelTypeIcon(type);
                    const typeColor = getModelTypeColor(type);
                    
                    // Safely determine background color
                    let bgColor = token.colorBgContainer;
                    if (typeColor !== 'default') {
                        switch(typeColor) {
                            case 'blue':
                                bgColor = token.colorPrimaryBg;
                                break;
                            case 'green':
                                bgColor = token.colorSuccessBg;
                                break;
                            case 'gold':
                            case 'orange':
                                bgColor = token.colorWarningBg;
                                break;
                            case 'cyan':
                                bgColor = token.colorInfoBg;
                                break;
                            case 'magenta':
                                bgColor = token.colorErrorBg;
                                break;
                            default:
                                bgColor = token.colorBgContainer;
                        }
                    }
                    
                    return (
                        <MotionDiv
                            key={type}
                            whileHover={{ scale: 1.05 }}
                            whileTap={{ scale: 0.95 }}
                            initial={{ opacity: 0, y: 20 }}
                            animate={{ opacity: 1, y: 0 }}
                            transition={{ delay: index * 0.05 }}
                            onClick={() => handleModelTypeSelect(type)}
                            style={{
                                cursor: 'pointer',
                                padding: '12px 16px',
                                borderRadius: token.borderRadiusSM + 'px',
                                backgroundColor: selectedModelType === type ? token.colorPrimaryBg : token.colorBgElevated,
                                display: 'flex',
                                flexDirection: 'column',
                                alignItems: 'center',
                                gap: '8px',
                                minWidth: '100px',
                                boxShadow: selectedModelType === type ? `0 6px 16px ${token.colorPrimaryBgHover}` : token.boxShadowSecondary,
                                transition: 'all 0.3s',
                                border: `1px solid ${selectedModelType === type ? token.colorPrimaryBorder : 'transparent'}`
                            }}
                        >
                            <div style={{ 
                                width: 40, 
                                height: 40, 
                                borderRadius: '50%', 
                                backgroundColor: bgColor,
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'center',
                                boxShadow: token.boxShadowSecondary
                            }}>
                                {typeIcon}
                            </div>
                            <div style={{ 
                                fontWeight: 500, 
                                textTransform: 'capitalize',
                                color: selectedModelType === type ? token.colorPrimaryText : token.colorText
                            }}>
                                {type}
                            </div>
                            <Badge 
                                count={count} 
                                overflowCount={999} 
                                style={{ 
                                    backgroundColor: selectedModelType === type ? token.colorPrimary : token.colorTextSecondary
                                }} 
                            />
                        </MotionDiv>
                    );
                })}
            </MotionDiv>
        );
    };

    // Rendering the provider filter section
    const renderProviderFilter = () => (
        <MotionDiv 
            className={styles.iconFilter}
            variants={itemVariants}
        >
            {Object.entries(provider).map(([key, value]: any, index: number) => {
                const providerCount = data.filter(item => item.provider === key).length;
                
                return (
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
                            <Flexbox gap={4}>
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
                                <Badge count={providerCount} size="small" style={{ backgroundColor: input.type === key ? token.colorTextLightSolid : token.colorPrimary }} />
                            </Flexbox>
                        </Flexbox>
                    </MotionDiv>
                );
            })}
        </MotionDiv>
    );
    
    // Rendering the model type filter
    const renderModelTypeFilter = () => (
        <MotionDiv 
            className={styles.modelTypeFilter}
            variants={itemVariants}
        >
            {modelTypes.map((type, index) => {
                const typeIcon = getModelTypeIcon(type);
                const typeColor = getModelTypeColor(type);
                // Use modelInfo.modelTypeCounts to get the count
                const typeData = modelInfo.modelTypeCounts?.find((item: any) => item.type === type.toLowerCase());
                const count = typeData ? typeData.count : modelCountByType[type] || 0;
                
                // Map color names to corresponding background colors
                const getBgColorByType = (colorName: string): string => {
                    switch(colorName) {
                        case 'blue': return token.colorPrimaryBg;
                        case 'green': return token.colorSuccessBg;
                        case 'gold': return token.colorWarningBg;
                        case 'cyan': return token.colorInfoBg;
                        case 'orange': return token.colorWarningBg;
                        case 'magenta': return token.colorErrorBg;
                        default: return token.colorBgContainer;
                    }
                };
                
                return (
                    <MotionDiv
                        key={type}
                        className={`${styles.modelTypeFilterItem} ${selectedModelType === type ? 'selected' : ''}`}
                        onClick={() => handleModelTypeSelect(type)}
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
                        <div style={{ 
                            width: 24, 
                            height: 24, 
                            borderRadius: '50%', 
                            backgroundColor: getBgColorByType(typeColor),
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'center'
                        }}>
                            {typeIcon}
                        </div>
                        <span>{type}</span>
                        <Badge count={count} size="small" style={{ backgroundColor: selectedModelType === type ? token.colorPrimary : token.colorTextSecondary }} />
                    </MotionDiv>
                );
            })}
        </MotionDiv>
    );
    
    // Render a grid view of model cards
    const renderModelGrid = () => (
        <div className={styles.modelGridView}>
            {filteredData.map((item, index) => (
                <MotionCard
                    key={item.id}
                    className={styles.modelCard}
                    loading={loading}
                    custom={index}
                    variants={cardVariants}
                    whileHover={{ y: -8, boxShadow: "0 12px 24px rgba(0, 0, 0, 0.15)" }}
                    size="small"
                    onClick={() => handleModelClick(item)}
                    style={{ cursor: 'pointer', position: 'relative' }}
                >
                    <Badge 
                        className={styles.modelTypeBadge}
                        count={
                            <Tooltip title={`模型类型: ${item.type}`}>
                                <div style={{ 
                                    backgroundColor: token.colorBgContainer,
                                    borderRadius: '50%',
                                    width: 28, 
                                    height: 28,
                                    display: 'flex',
                                    alignItems: 'center',
                                    justifyContent: 'center',
                                    boxShadow: token.boxShadowSecondary
                                }}>
                                    {getModelTypeIcon(item.type)}
                                </div>
                            </Tooltip>
                        }
                    />
                    
                    <div className={styles.modelCardHeader}>
                        <div className={styles.modelCardMeta}>
                            <motion.div 
                                whileHover={{ scale: 1.1, rotate: 5 }}
                                transition={{ type: "spring", stiffness: 400, damping: 10 }}
                                style={{ marginRight: 8 }}
                            >
                                {getIconByName(item.icon)?.icon ?? <IconAvatar size={28} Icon={OpenAI} />}
                            </motion.div>
                            <div 
                                className={styles.modelName}
                                onClick={(e) => {
                                    e.stopPropagation();
                                    copyModelName(item.model);
                                }}
                            >
                                {item.model}
                            </div>
                        </div>
                        
                        <MotionTag 
                            color={item.enable ? 'green' : 'red'}
                            whileHover={{ scale: 1.05 }}
                            style={{ fontSize: '11px' }}
                        >
                            {item.enable ? '可用' : '不可用'}
                        </MotionTag>
                    </div>

                    <div className={styles.modelCardContent}>
                        <div className={styles.modelCardSection}>
                            <Tooltip title={item.description}>
                                <div className={styles.modelDescription}>
                                    {item.description}
                                </div>
                            </Tooltip>
                        </div>

                        <div className={styles.modelCardSection}>
                            <div className={styles.modelTags}>
                                <MotionTag 
                                    color={getModelTypeColor(item.type)}
                                    whileHover={{ scale: 1.05 }}
                                    style={{ fontSize: '11px' }}
                                    icon={getModelTypeIcon(item.type)}
                                >
                                    {item.type}
                                </MotionTag>
                                <MotionTag 
                                    color={item.quotaType === 1 ? 'blue' : 'orange'}
                                    whileHover={{ scale: 1.05 }}
                                    style={{ fontSize: '11px' }}
                                >
                                    {item.quotaType === 1 ? '按量' : '按次'}
                                </MotionTag>
                            </div>
                        </div>

                        {item.tags && item.tags.length > 0 && (
                            <div className={styles.modelCardSection}>
                                <div className={styles.modelTags}>
                                    {item.tags.slice(0, 2).map((tag: string, idx: number) => (
                                        <MotionTag 
                                            key={tag} 
                                            color='blue'
                                            style={{ fontSize: '11px' }}
                                            initial={{ opacity: 0, y: 10 }}
                                            animate={{ opacity: 1, y: 0 }}
                                            transition={{ delay: idx * 0.05 }}
                                            whileHover={{ scale: 1.05 }}
                                        >
                                            {tag}
                                        </MotionTag>
                                    ))}
                                    {item.tags.length > 2 && (
                                        <MotionTag color="default" style={{ fontSize: '11px' }}>+{item.tags.length - 2}</MotionTag>
                                    )}
                                </div>
                            </div>
                        )}
                    </div>

                    <div className={styles.modelCardFooter}>
                        <div className={styles.modelPriceSection}>
                            {renderPriceInfo(item)}
                        </div>

                        <div className={styles.modelStats}>
                            <div className={styles.modelStat}>
                                <div className={styles.modelStatValue}>{item.quotaMax || '-'}</div>
                                <div className={styles.modelStatLabel}>最大上下文</div>
                            </div>
                        </div>
                    </div>
                </MotionCard>
            ))}
        </div>
    );

    // Render results summary with view toggle
    const renderResultSummary = () => {
        const modelTypeName = input.modelType ? 
            modelInfo.modelTypeCounts?.find((item: any) => 
                item.type.toLowerCase() === input.modelType.toLowerCase()
            )?.name || input.modelType : '';
            
        return (
            <div className={styles.resultSummary}>
                <div>
                    共找到 <Typography.Text strong>{filteredData.length}</Typography.Text> 个结果
                    {input.modelType && (
                        <>, 模型类型: <Typography.Text type="secondary" style={{ fontStyle: 'italic' }}>{modelTypeName}</Typography.Text></>
                    )}
                    {input.type && (
                        <>, 提供商: <Typography.Text type="secondary" style={{ fontStyle: 'italic' }}>{provider[input.type] || input.type}</Typography.Text></>
                    )}
                </div>
                
                <div style={{ display: 'flex', gap: '12px', alignItems: 'center' }}>
                    {(input.modelType || input.type || input.tags.length > 0) && (
                        <Button 
                            size="small" 
                            onClick={() => {
                                setSelectedModelType('');
                                setInput({...input, type: '', tags: [], modelType: ''});
                            }}
                        >
                            清除筛选
                        </Button>
                    )}
                    
                    <div className={styles.viewToggle}>
                        <span>视图:</span>
                        <Switch
                            checked={isGridView}
                            size="small"
                            onChange={(checked) => setIsGridView(checked)}
                            checkedChildren="卡片"
                            unCheckedChildren="列表"
                        />
                    </div>
                </div>
            </div>
        );
    };

    return (
        <MotionDiv 
            className={styles.modelContainer}
            initial="hidden"
            animate="visible"
            variants={containerVariants}
        >
            <AnimatedBackground />
            
            <MotionDiv variants={itemVariants}>
                <Header nav={'模型列表'} />
            </MotionDiv>
            
            {renderSearchSection()}
            
            {modelInfo.modelTypeCounts && modelInfo.modelTypeCounts.length > 0 && renderModelTypeSummary()}
            
            {hasProviders && renderProviderFilter()}
            
            {renderModelTypeFilter()}
            
            {renderResultSummary()}
            
            <MotionDiv 
                className={styles.modelsContainer}
                variants={itemVariants}
            >
                {loading && <Empty description="加载中..." image={Empty.PRESENTED_IMAGE_SIMPLE} />}
                
                {!loading && filteredData.length === 0 && (
                    <Empty description="没有符合条件的模型" image={Empty.PRESENTED_IMAGE_SIMPLE} />
                )}
                
                {!loading && filteredData.length > 0 && (
                    isGridView ? renderModelGrid() : (
                        <Row gutter={[12, 12]}>
                            {filteredData.map((item, index) => renderModelCard(item, index))}
                        </Row>
                    )
                )}
                
                {total > 0 && (
                    <div className={styles.pagination}>
                        <Button.Group>
                            <Button 
                                disabled={input.page === 1}
                                onClick={() => setInput({...input, page: input.page - 1})}
                            >
                                上一页
                            </Button>
                            <Button type="primary">
                                {input.page} / {Math.ceil(total / input.pageSize)}
                            </Button>
                            <Button 
                                disabled={input.page >= Math.ceil(total / input.pageSize)}
                                onClick={() => setInput({...input, page: input.page + 1})}
                            >
                                下一页
                            </Button>
                        </Button.Group>
                    </div>
                )}
            </MotionDiv>
            
            {renderModelDetailModal()}
            
            <style>{`
                .mobile-header {
                    display: flex;
                    flex-direction: column;
                    width: 100%;
                }
                
                .mobile-search {
                    padding: 0 16px;
                    margin-top: 12px;
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
                }
            `}</style>
        </MotionDiv>
    );
}
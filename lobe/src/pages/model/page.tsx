import { useEffect, useMemo, useState } from "react";
import { Header, Input, Tag, Tooltip } from "@lobehub/ui";
import { IconAvatar, OpenAI } from "@lobehub/icons";
import { Button, Card, Col, Descriptions, Divider, Empty, Modal, Row, Select, Space, Switch, Typography, message, theme, Badge, Layout, Drawer, Pagination } from "antd";
import { getCompletionRatio, renderQuota } from "../../utils/render";
import { getIconByName } from "../../utils/iconutils";
import { getModelLibrary, getModelLibraryMetadata, getModelInfo, getProvider } from "../../services/ModelService";
import { Copy, Info, Search, Tag as TagIcon, MessageSquare, Image, Mic, Headphones, FileText, Layers, Filter, Menu } from "lucide-react";
import { useLocation, useNavigate } from "react-router-dom";
import { Flexbox } from "react-layout-kit";
import { createStyles } from "antd-style";
import useBreakpoint from "antd/es/grid/hooks/useBreakpoint";
import MODEL_TYPES from "../model-manager/constants/modelTypes";
import { useTranslation } from 'react-i18next';
import LanguageSwitcher from "../../components/LanguageSwitcher";

const { Title, Paragraph } = Typography;
const { Sider, Content } = Layout;

const useStyles = createStyles(({ token, css }) => ({
    // 主容器样式
    mainLayout: css`
        height: 100vh;
        background: linear-gradient(135deg, ${token.colorBgContainer} 0%, ${token.colorBgElevated} 100%);
        position: relative;
        overflow: hidden;
    `,

    // 左侧边栏样式
    leftSider: css`
        background: ${token.colorBgContainer};
        border-right: 1px solid ${token.colorBorderSecondary};
        box-shadow: 0 4px 24px rgba(0, 0, 0, 0.06);
        overflow-y: auto;
        height: 100vh;
        position: fixed;
        left: 0;
        top: 0;
        z-index: 100;
        
        &::-webkit-scrollbar {
            width: 4px;
        }
        
        &::-webkit-scrollbar-track {
            background: transparent;
        }
        
        &::-webkit-scrollbar-thumb {
            background: ${token.colorBorderSecondary};
            border-radius: 2px;
        }
        
        &::-webkit-scrollbar-thumb:hover {
            background: ${token.colorBorder};
        }
    `,

    // 右侧内容区样式
    rightContent: css`
        margin-left: 280px;
        padding: 0;
        background: ${token.colorBgLayout};
        height: 100vh;
        overflow: hidden;
        display: flex;
        flex-direction: column;
        
        @media (max-width: 768px) {
            margin-left: 0;
        }
    `,

    // 移动端抽屉按钮
    mobileFilterButton: css`
        position: fixed;
        top: 90px;
        left: 20px;
        z-index: 1000;
        background: ${token.colorPrimary};
        border: none;
        border-radius: 50%;
        width: 48px;
        height: 48px;
        box-shadow: 0 6px 20px rgba(0, 0, 0, 0.15);
        display: flex;
        align-items: center;
        justify-content: center;
        transition: all 0.3s ease;
        
        &:hover {
            transform: scale(1.05);
            box-shadow: 0 8px 24px rgba(0, 0, 0, 0.2);
        }
        
        @media (min-width: 769px) {
            display: none;
        }
    `,

    // 过滤器容器
    filterContainer: css`
        padding: 24px 20px;
        
        @media (max-width: 768px) {
            padding: 20px 16px;
        }
    `,

    // 过滤器组标题
    filterGroupTitle: css`
        font-size: 14px;
        font-weight: 600;
        color: ${token.colorTextHeading};
        margin-bottom: 16px;
        display: flex;
        align-items: center;
        gap: 8px;
        padding-bottom: 8px;
        border-bottom: 1px solid ${token.colorBorderSecondary};
        position: relative;
        
        &::after {
            content: '';
            position: absolute;
            bottom: -1px;
            left: 0;
            width: 24px;
            height: 2px;
            background: ${token.colorPrimary};
            border-radius: 1px;
        }
    `,

    // 搜索区域
    searchSection: css`
        margin-bottom: 24px;
        
        .ant-input-affix-wrapper {
            border-radius: ${token.borderRadiusLG}px;
            border: 1px solid ${token.colorBorderSecondary};
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);
            transition: all 0.3s ease;
            
            &:focus-within {
                border-color: ${token.colorPrimary};
                box-shadow: 0 0 0 2px ${token.colorPrimaryBg};
            }
        }
    `,

    // 过滤器组
    filterGroup: css`
        margin-bottom: 32px;
        
        &:last-child {
            margin-bottom: 0;
        }
    `,

    // 供应商过滤器
    providerFilter: css`
        display: flex;
        flex-direction: column;
        gap: 8px;
    `,

    providerItem: css`
        cursor: pointer;
        padding: 16px;
        border-radius: ${token.borderRadiusLG}px;
        transition: all 0.3s ease;
        border: 1px solid ${token.colorBorderSecondary};
        background: ${token.colorBgElevated};
        display: flex;
        align-items: center;
        gap: 12px;
        position: relative;
        overflow: hidden;
        
        &::before {
            content: '';
            position: absolute;
            left: 0;
            top: 0;
            bottom: 0;
            width: 0;
            background: linear-gradient(135deg, ${token.colorPrimary}, ${token.colorPrimaryActive});
            transition: width 0.3s ease;
        }
        
        &:hover {
            background: ${token.colorBgTextHover};
            border-color: ${token.colorPrimaryBorderHover};
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
            
            &::before {
                width: 4px;
            }
        }
        
        &.selected {
            background: ${token.colorPrimaryBg};
            border-color: ${token.colorPrimary};
            box-shadow: 0 4px 12px ${token.colorPrimaryBgHover};
            
            &::before {
                width: 4px;
            }
        }
    `,

    // 模型类型过滤器
    modelTypeFilter: css`
        display: flex;
        flex-direction: column;
        gap: 8px;
    `,

    modelTypeItem: css`
        cursor: pointer;
        padding: 12px 16px;
        border-radius: ${token.borderRadiusLG}px;
        transition: all 0.3s ease;
        border: 1px solid ${token.colorBorderSecondary};
        background: ${token.colorBgElevated};
        display: flex;
        align-items: center;
        gap: 12px;
        position: relative;
        overflow: hidden;
        
        &::before {
            content: '';
            position: absolute;
            left: 0;
            top: 0;
            bottom: 0;
            width: 0;
            background: linear-gradient(135deg, ${token.colorPrimary}, ${token.colorPrimaryActive});
            transition: width 0.3s ease;
        }
        
        &:hover {
            background: ${token.colorBgTextHover};
            border-color: ${token.colorPrimaryBorderHover};
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
            
            &::before {
                width: 4px;
            }
        }
        
        &.selected {
            background: ${token.colorPrimaryBg};
            border-color: ${token.colorPrimary};
            box-shadow: 0 4px 12px ${token.colorPrimaryBgHover};
            
            &::before {
                width: 4px;
            }
        }
    `,

    // 标签选择器
    tagSelector: css`
        .ant-select {
            width: 100%;
        }
        
        .ant-select-selector {
            border-radius: ${token.borderRadiusLG}px;
            min-height: 40px;
            border: 1px solid ${token.colorBorderSecondary};
            transition: all 0.3s ease;
            
            &:focus, &:focus-within {
                border-color: ${token.colorPrimary};
                box-shadow: 0 0 0 2px ${token.colorPrimaryBg};
            }
        }
    `,

    // 内容头部
    contentHeader: css`
        padding: 24px 32px;
        background: ${token.colorBgContainer};
        border-bottom: 1px solid ${token.colorBorderSecondary};
        display: flex;
        justify-content: space-between;
        align-items: center;
        flex-shrink: 0;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);
        
        @media (max-width: 768px) {
            padding: 20px 16px;
            flex-direction: column;
            align-items: flex-start;
            gap: 16px;
        }
    `,

    // 模型容器
    modelsContainer: css`
        flex: 1;
        padding: 32px;
        overflow-y: auto;
        background: ${token.colorBgLayout};
        
        @media (max-width: 768px) {
            padding: 20px 16px;
        }
        
        &::-webkit-scrollbar {
            width: 6px;
        }
        
        &::-webkit-scrollbar-track {
            background: transparent;
        }
        
        &::-webkit-scrollbar-thumb {
            background: ${token.colorBorderSecondary};
            border-radius: 3px;
        }
        
        &::-webkit-scrollbar-thumb:hover {
            background: ${token.colorBorder};
        }
    `,

    // 模型网格
    modelGrid: css`
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(360px, 1fr));
        gap: 24px;
        
        @media (max-width: 768px) {
            grid-template-columns: 1fr;
            gap: 16px;
        }
    `,

    // 模型卡片
    modelCard: css`
        background: ${token.colorBgContainer};
        border-radius: ${token.borderRadiusLG}px;
        padding: 24px;
        border: 1px solid ${token.colorBorderSecondary};
        transition: all 0.3s ease;
        cursor: pointer;
        position: relative;
        overflow: hidden;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);
        
        &::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 0;
            background: linear-gradient(90deg, ${token.colorPrimary} 0%, ${token.colorPrimaryActive} 100%);
            transition: height 0.3s ease;
        }
        
        &:hover {
            transform: translateY(-4px);
            box-shadow: 0 8px 32px rgba(0, 0, 0, 0.12);
            border-color: ${token.colorPrimaryBorderHover};
            
            &::before {
                height: 4px;
            }
        }
    `,

    // 其他样式保持不变...
    modelCardHeader: css`
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-bottom: 16px;
    `,

    modelCardMeta: css`
        display: flex;
        align-items: center;
        gap: 12px;
        flex: 1;
    `,

    modelName: css`
        font-size: 16px;
        font-weight: 600;
        color: ${token.colorText};
        margin: 0;
        cursor: pointer;
        transition: color 0.3s ease;
        line-height: 1.4;
        
        &:hover {
            color: ${token.colorPrimary};
        }
    `,

    modelDescription: css`
        font-size: 14px;
        color: ${token.colorTextSecondary};
        margin-bottom: 16px;
        line-height: 1.6;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
        overflow: hidden;
    `,

    modelTags: css`
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
        margin-bottom: 16px;
        
        .ant-tag {
            margin: 0;
            border-radius: ${token.borderRadius}px;
            font-size: 11px;
            line-height: 1.3;
        }
    `,

    modelStats: css`
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
        gap: 16px;
        margin-top: 16px;
        padding-top: 16px;
        border-top: 1px solid ${token.colorBorderSecondary};
    `,

    modelStat: css`
        text-align: center;
        padding: 8px;
        background: ${token.colorBgLayout};
        border-radius: ${token.borderRadius}px;
    `,

    modelStatValue: css`
        font-size: 14px;
        font-weight: 600;
        color: ${token.colorText};
        display: block;
        margin-bottom: 4px;
    `,

    modelStatLabel: css`
        font-size: 12px;
        color: ${token.colorTextSecondary};
        line-height: 1.3;
    `,

    // 模态框样式
    modelDetailModal: css`
        .ant-modal-content {
            background: ${token.colorBgContainer};
            border-radius: ${token.borderRadiusLG}px;
            overflow: hidden;
            box-shadow: 0 16px 48px rgba(0, 0, 0, 0.12);
        }
        
        .ant-modal-header {
            background: transparent;
            border-bottom: 1px solid ${token.colorBorderSecondary};
            padding: 20px 24px;
        }
        
        .ant-modal-body {
            padding: 24px;
            max-height: 70vh;
            overflow-y: auto;
        }
        
        .ant-modal-footer {
            border-top: 1px solid ${token.colorBorderSecondary};
            padding: 16px 24px;
            background: ${token.colorBgLayout};
        }
        
        @media (max-width: 768px) {
            .ant-modal-body {
                padding: 16px;
                max-height: 80vh;
            }
            
            .ant-modal-footer {
                padding: 12px 16px;
            }
        }
    `,

    // 工具提示样式
    resultInfo: css`
        display: flex;
        align-items: center;
        gap: 8px;
        font-size: 14px;
        color: ${token.colorTextSecondary};
        
        .ant-space {
            flex-wrap: wrap;
        }
        
        @media (max-width: 768px) {
            flex-direction: column;
            align-items: flex-start;
            gap: 12px;
        }
    `,

    // 清除过滤器按钮
    clearFiltersButton: css`
        color: ${token.colorTextSecondary};
        border-color: ${token.colorBorderSecondary};
        background: ${token.colorBgContainer};
        border-radius: ${token.borderRadiusLG}px;
        height: 40px;
        font-weight: 500;
        transition: all 0.3s ease;
        
        &:hover {
            color: ${token.colorPrimary};
            border-color: ${token.colorPrimary};
            background: ${token.colorPrimaryBg};
        }
    `,

    // 模型详情样式
    modelDetailTags: css`
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
        margin-top: 16px;
    `,

    modelDetailPricing: css`
        display: flex;
        flex-wrap: wrap;
        gap: 16px;
        margin-top: 16px;
    `,

    modelDetailPriceCard: css`
        background-color: ${token.colorBgContainer};
        border-radius: ${token.borderRadius}px;
        padding: 16px;
        flex: 1;
        min-width: 200px;
        box-shadow: ${token.boxShadowSecondary};
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
    `,

    // 其他模型详情样式
    modelDetailHeader: css`
        display: flex;
        align-items: center;
        margin-bottom: 24px;
        gap: 16px;
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
    `,

    modelDetailDescription: css`
        color: ${token.colorTextSecondary};
        font-size: 14px;
    `,

    modelDetailSection: css`
        margin-bottom: 24px;
    `,

    modelDetailSectionTitle: css`
        font-size: 16px;
        font-weight: 500;
        margin-bottom: 16px;
        color: ${token.colorTextHeading};
        display: flex;
        align-items: center;
        gap: 8px;
    `,

    modelDetailStats: css`
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
        gap: 16px;
        margin-top: 16px;
    `,

    modelDetailStat: css`
        background-color: ${token.colorBgContainer};
        border-radius: ${token.borderRadius}px;
        padding: 16px;
        text-align: center;
        box-shadow: ${token.boxShadowSecondary};
    `,

    modelDetailStatValue: css`
        font-size: 18px;
        font-weight: 600;
        color: ${token.colorText};
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
    `,

    // 主要按钮样式
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

    // 分页容器样式
    paginationContainer: css`
        padding: 24px 32px;
        background: ${token.colorBgContainer};
        border-top: 1px solid ${token.colorBorderSecondary};
        display: flex;
        justify-content: center;
        align-items: center;
        flex-shrink: 0;
        
        @media (max-width: 768px) {
            padding: 16px 20px;
        }
        
        .ant-pagination {
            .ant-pagination-item {
                border-radius: ${token.borderRadius}px;
                transition: all 0.3s ease;
                
                &:hover {
                    transform: translateY(-1px);
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }
            }
            
            .ant-pagination-item-active {
                background: ${token.colorPrimary};
                border-color: ${token.colorPrimary};
                
                a {
                    color: ${token.colorWhite};
                }
            }
            
            .ant-pagination-prev,
            .ant-pagination-next {
                border-radius: ${token.borderRadius}px;
                transition: all 0.3s ease;
                
                &:hover {
                    transform: translateY(-1px);
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }
            }
        }
    `,
}));

export default function DesktopLayout() {
    const navigate = useNavigate();
    const location = useLocation();
    const { t } = useTranslation();
    const [data, setData] = useState<any[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [isK, setIsK] = useState<boolean>(false);
    const [provider, setProvider] = useState<any>({});
    const [total, setTotal] = useState<number>(0);
    const [allTags, setAllTags] = useState<string[]>([]);
    const [modelMetadata, setModelMetadata] = useState<any>({
        tags: [],
        providers: {},
        icons: {},
        modelTypes: []
    });
    const [selectedModel, setSelectedModel] = useState<any>(null);
    const [modalVisible, setModalVisible] = useState<boolean>(false);
    const [drawerVisible, setDrawerVisible] = useState<boolean>(false);
    const [input, setInput] = useState({
        page: 1,
        pageSize: 40,
        model: '',
        isFirst: true,
        type: '',
        tags: [] as string[],
        modelType: ''
    });
    const [selectedModelType, setSelectedModelType] = useState<string>('');

    const { styles } = useStyles();
    const { token } = theme.useToken();
    const screens = useBreakpoint();
    const isMobile = !screens.md;
    const [modelInfo, setModelInfo] = useState<any>({});

    useEffect(() => {
        loadModelInfo();
        loadMetadata();
    }, []);

    useEffect(() => {
        loadData();
    }, [input.page, input.pageSize, input.type, input.modelType, input.tags, input.model]);

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

    function loadModelInfo() {
        getModelInfo()
            .then((res) => {
                setModelInfo(res.data);
            });
    }

    const loadMetadata = () => {
        getModelLibraryMetadata()
            .then((res) => {
                if (res.data) {
                    setModelMetadata(res.data);
                    // 为了向后兼容，同时设置原有的状态
                    if (res.data.tags && Array.isArray(res.data.tags)) {
                        setAllTags(res.data.tags);
                    }
                    if (res.data.providers) {
                        setProvider(res.data.providers);
                    }
                }
            })
            .catch((error) => {
                console.error('Failed to load metadata:', error);
                message.error(t('modelLibrary.loadFailed'));
                // 如果统一接口失败，回退到分别调用
                loadProviderFallback();
                loadTagsFallback();
            });
    }

    // 回退方法 - 如果统一接口不可用
    const loadProviderFallback = () => {
        getProvider().then((res) => {
            setProvider(res.data);
        }).catch(() => {
            console.warn('Provider fallback also failed');
        });
    }

    const loadTagsFallback = () => {
        // 这里应该有一个专门获取tags的方法，暂时使用元数据方法的fallback
        getModelLibraryMetadata()
            .then((res) => {
                if (res.data && res.data.tags && Array.isArray(res.data.tags)) {
                    setAllTags(res.data.tags);
                }
            })
            .catch(() => {
                console.warn('Tags fallback also failed');
            });
    }

    const loadData = () => {
        setLoading(true);
        getModelLibrary(input.model, input.page, input.pageSize, input.type, input.modelType, input.tags)
            .then((res) => {
                setData(res.data.items);
                setTotal(res.data.total);
            })
            .catch(() => {
                message.error(t('modelLibrary.loadFailed'));
            })
            .finally(() => {
                setLoading(false);
            });
    }

    // 计算供应商和模型类型的实际数量
    const getProviderCount = (providerKey: string) => {
        // 优先使用从metadata接口获取的数量统计
        if (modelMetadata.providerCounts && Array.isArray(modelMetadata.providerCounts)) {
            const providerData = modelMetadata.providerCounts.find((item: any) => 
                item.provider === providerKey
            );
            if (providerData) {
                return providerData.count;
            }
        }
        // 回退到从本地数据计算
        return data.filter(item => item.provider === providerKey).length;
    };

    const getModelTypeCount = (type: string) => {
        // 优先使用从metadata接口获取的数量统计
        if (modelMetadata.modelTypeCounts && Array.isArray(modelMetadata.modelTypeCounts)) {
            const typeData = modelMetadata.modelTypeCounts.find((item: any) =>
                item.type?.toLowerCase() === type?.toLowerCase()
            );
            if (typeData) {
                return typeData.count;
            }
        }
        // 回退到从本地数据计算
        return data.filter(item => item.type?.toLowerCase() === type.toLowerCase()).length;
    };

    // 获取模型类型的翻译文本
    const getModelTypeText = (type: string) => {
        const typeKey = type?.toLowerCase();
        switch (typeKey) {
            case 'chat':
                return t('modelLibrary.chat');
            case 'image':
                return t('modelLibrary.image');
            case 'audio':
                return t('modelLibrary.audio');
            case 'stt':
                return t('modelLibrary.stt');
            case 'tts':
                return t('modelLibrary.tts');
            case 'embedding':
                return t('modelLibrary.embedding');
            default:
                return type;
        }
    };

    const filteredData = useMemo(() => {
        // 服务器端已经处理了所有过滤逻辑，直接使用返回的数据
        return data;
    }, [data]);

    const handleModelTypeSelect = (type: string) => {
        const newType = selectedModelType?.toLowerCase() === type?.toLowerCase() ? '' : type;
        setSelectedModelType(newType);
        setInput({
            ...input,
            modelType: newType,
            page: 1
        });
    };

    const copyModelName = (modelName: string) => {
        try {
            navigator.clipboard.writeText(modelName)
                .then(() => {
                    message.success(t('modelLibrary.copySuccess'));
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
        message.success(t('modelLibrary.copySuccess'));
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

    const handlePageChange = (page: number, pageSize?: number) => {
        setInput({
            ...input,
            page,
            pageSize: pageSize || input.pageSize
        });
    };

    const handleSearch = () => {
        setInput({
            ...input,
            page: 1 // 搜索时重置到第一页
        });
    };

    const getModelTypeIcon = (type: string) => {
        switch (type?.toLowerCase()) {
            case MODEL_TYPES.CHAT.toLowerCase():
                return <MessageSquare size={14} color={token.colorPrimary} />;
            case MODEL_TYPES.IMAGE.toLowerCase():
                return <Image size={14} color={token.colorSuccess} />;
            case MODEL_TYPES.AUDIO.toLowerCase():
                return <Headphones size={14} color={token.colorWarning} />;
            case MODEL_TYPES.STT.toLowerCase():
                return <Mic size={14} color={token.colorInfo} />;
            case MODEL_TYPES.TTS.toLowerCase():
                return <Headphones size={14} color={token.colorWarning} />;
            case MODEL_TYPES.EMBEDDING.toLowerCase():
                return <Layers size={14} color={token.colorError} />;
            default:
                return <FileText size={14} color={token.colorTextSecondary} />;
        }
    };

    const getModelTypeColor = (type: string) => {
        switch (type?.toLowerCase()) {
            case MODEL_TYPES.CHAT.toLowerCase():
                return 'blue';
            case MODEL_TYPES.IMAGE.toLowerCase():
                return 'green';
            case MODEL_TYPES.AUDIO.toLowerCase():
                return 'gold';
            case MODEL_TYPES.STT.toLowerCase():
                return 'cyan';
            case MODEL_TYPES.TTS.toLowerCase():
                return 'orange';
            case MODEL_TYPES.EMBEDDING.toLowerCase():
                return 'magenta';
            default:
                return 'default';
        }
    };

    const renderPriceInfo = (item: any) => {
        if (item.quotaType === 1) {
            const tokenUnit = isK ? '1K' : '1M';
            const multiplier = isK ? 1000 : 1000000;

            return (
                <div className="model-price" style={{ display: 'flex', flexDirection: 'column', gap: '6px' }}>
                    <div style={{ display: 'flex', flexWrap: 'wrap', gap: '6px' }}>
                        <Tag
                            color='cyan'
                            style={{ fontSize: '11px', display: 'flex', alignItems: 'center', gap: '4px' }}
                        >
                            <span style={{ fontWeight: 'bold' }}>{t('modelLibrary.promptPrice')}:</span>
                            <span>{renderQuota(item.promptRate * multiplier, 6)}/{tokenUnit}</span>
                        </Tag>
                        <Tag
                            color='geekblue'
                            style={{ fontSize: '11px', display: 'flex', alignItems: 'center', gap: '4px' }}
                        >
                            <span style={{ fontWeight: 'bold' }}>{t('modelLibrary.completionPrice')}:</span>
                            <span>{renderQuota((item.completionRate ?
                                item.promptRate * multiplier * item.completionRate :
                                getCompletionRatio(item.model) * multiplier), 6)}/{tokenUnit}</span>
                        </Tag>
                    </div>
                </div>
            );
        } else {
            return (
                <Tag
                    color='geekblue'
                    style={{ fontSize: '11px', display: 'flex', alignItems: 'center', gap: '4px' }}
                >
                    <span style={{ fontWeight: 'bold' }}>{t('modelLibrary.perUsagePrice')}:</span>
                    <span>{renderQuota(item.promptRate, 6)}</span>
                </Tag>
            );
        }
    };

    const renderAllTags = (tags: string[]) => {
        if (!tags || !Array.isArray(tags) || tags.length === 0) return null;

        return (
            <div className={styles.modelDetailTags}>
                {tags.map((tag, index) => (
                    <Tag
                        key={tag}
                        color="blue"
                    >
                        {tag}
                    </Tag>
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
                    <div className={styles.modelDetailPriceCard}>
                        <div className={styles.modelDetailPriceTitle}>{t('modelLibrary.promptPrice')}</div>
                        <div className={styles.modelDetailPriceValue}>
                            {renderQuota(item.promptRate * multiplier, 6)}/{tokenUnit}
                        </div>
                    </div>

                    <div className={styles.modelDetailPriceCard}>
                        <div className={styles.modelDetailPriceTitle}>{t('modelLibrary.completionPrice')}</div>
                        <div className={styles.modelDetailPriceValue}>
                            {renderQuota((item.completionRate ?
                                item.promptRate * multiplier * item.completionRate :
                                getCompletionRatio(item.model) * multiplier), 6)}/{tokenUnit}
                        </div>
                    </div>
                </div>
            );
        } else {
            return (
                <div className={styles.modelDetailPricing}>
                    <div className={styles.modelDetailPriceCard}>
                        <div className={styles.modelDetailPriceTitle}>{t('modelLibrary.perUsagePrice')}</div>
                        <div className={styles.modelDetailPriceValue}>
                            {renderQuota(item.promptRate, 6)}
                        </div>
                    </div>
                </div>
            );
        }
    };

    const renderModelStats = (item: any) => {
        if (!item) return null;

        const stats = [
            { label: t('modelLibrary.maxContext'), value: item.quotaMax || '-' },
            { label: t('modelLibrary.modelType'), value: getModelTypeText(item.type) },
            { label: t('modelLibrary.billingType'), value: item.quotaType === 1 ? t('modelLibrary.volumeBilling') : t('modelLibrary.perUseBilling') },
            { label: t('modelLibrary.status'), value: item.enable ? t('modelLibrary.available') : t('modelLibrary.unavailable') },
        ];

        return (
            <div className={styles.modelDetailStats}>
                {stats.map((stat, index) => (
                    <div
                        key={stat.label}
                        className={styles.modelDetailStat}
                    >
                        <div className={styles.modelDetailStatValue}>{stat.value}</div>
                        <div className={styles.modelDetailStatLabel}>{stat.label}</div>
                    </div>
                ))}
            </div>
        );
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
                        {t('modelLibrary.close')}
                    </Button>,
                    <Button
                        key="copy"
                        type="primary"
                        onClick={() => copyModelName(selectedModel.model)}
                        className={styles.primaryButton}
                    >
                        {t('modelLibrary.copyModelName')}
                    </Button>,
                ]}
                width={800}
                className={styles.modelDetailModal}
                destroyOnClose
                centered
            >
                <div className={styles.modelDetailHeader}>
                    <div className={styles.modelDetailIcon}>
                        {icon?.icon ?? <IconAvatar size={isMobile ? 32 : 40} Icon={OpenAI} />}
                    </div>

                    <div className={styles.modelDetailInfo}>
                        <div className={styles.modelDetailName}>
                            <Title level={4} className={styles.modelDetailTitle}>
                                {selectedModel.model}
                            </Title>
                            <div
                                className={styles.copyButton}
                                onClick={() => copyModelName(selectedModel.model)}
                            >
                                <Copy size={16} color={token.colorTextSecondary} />
                            </div>
                            <Tag
                                color={modelTypeColor}
                                style={{ marginLeft: 8 }}
                                icon={modelTypeIcon}
                            >
                                {getModelTypeText(selectedModel.type)}
                            </Tag>
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
                        {t('modelLibrary.modelStats')}
                    </div>
                    {renderModelStats(selectedModel)}
                </div>

                <div className={styles.modelDetailSection}>
                    <div className={styles.modelDetailSectionTitle}>
                        <Info size={16} color={token.colorTextSecondary} />
                        {t('modelLibrary.pricingInfo')} ({isK ? 'K' : 'M'} {t('modelLibrary.unitSwitch')})
                        <Switch
                            checked={isK}
                            size="small"
                            style={{ marginLeft: 8 }}
                            checkedChildren={t('modelLibrary.unitK')}
                            unCheckedChildren={t('modelLibrary.unitM')}
                            onChange={setIsK}
                        />
                    </div>
                    {renderDetailedPriceInfo(selectedModel)}
                </div>
            </Modal>
        );
    };

    // 渲染左侧过滤器
    const renderLeftFilters = () => (
        <div className={styles.filterContainer}>
            {/* 搜索区域 */}
            <div className={styles.filterGroup}>
                <div className={styles.filterGroupTitle}>
                    <Search size={16} />
                    {t('modelLibrary.search')}
                </div>
                <div className={styles.searchSection}>
                    <Input
                        placeholder={t('modelLibrary.searchPlaceholder')}
                        value={input.model}
                        onPressEnter={handleSearch}
                        prefix={<Search size={16} color={token.colorTextSecondary} />}
                        onChange={(e) => setInput({ ...input, model: e.target.value })}
                        allowClear
                    />
                </div>
            </div>

            {/* 供应商过滤器 */}
            {Object.keys(provider).length > 0 && (
                <div className={styles.filterGroup}>
                    <div className={styles.filterGroupTitle}>
                        <Image size={16} />
                        {t('modelLibrary.providers')}
                    </div>
                    <div className={styles.providerFilter}>
                        {Object.entries(provider).map(([key, value]: any) => {
                            const providerCount = getProviderCount(key);
                            return (
                                <div
                                    key={key}
                                    className={`${styles.providerItem} ${input.type === key ? 'selected' : ''}`}
                                    onClick={() => handleProviderFilter(key)}
                                >
                                    {getIconByName(key, 24)?.icon ?? <IconAvatar size={24} Icon={OpenAI} />}
                                    <div style={{ flex: 1 }}>
                                        <div style={{ fontWeight: 500 }}>{value || '其他'}</div>
                                        <div style={{ fontSize: 12, color: token.colorTextSecondary }}>
                                            {providerCount} 个模型
                                        </div>
                                    </div>
                                </div>
                            );
                        })}
                    </div>
                </div>
            )}

            {/* 模型类型过滤器 */}
            <div className={styles.filterGroup}>
                <div className={styles.filterGroupTitle}>
                    <Layers size={16} />
                    {t('modelLibrary.modelTypes')}
                </div>
                <div className={styles.modelTypeFilter}>
                    {Object.values(MODEL_TYPES).map((type) => {
                        const typeIcon = getModelTypeIcon(type);
                        const count = getModelTypeCount(type);

                        return (
                            <div
                                key={type}
                                className={`${styles.modelTypeItem} ${selectedModelType?.toLowerCase() === type?.toLowerCase() ? 'selected' : ''}`}
                                onClick={() => handleModelTypeSelect(type)}
                            >
                                <div style={{
                                    width: 28,
                                    height: 28,
                                    borderRadius: '50%',
                                    backgroundColor: token.colorBgLayout,
                                    display: 'flex',
                                    alignItems: 'center',
                                    justifyContent: 'center'
                                }}>
                                    {typeIcon}
                                </div>
                                <div style={{ flex: 1 }}>
                                    <div style={{ fontWeight: 500 }}>{getModelTypeText(type)}</div>
                                    <div style={{ fontSize: 12, color: token.colorTextSecondary }}>
                                        {count} 个模型
                                    </div>
                                </div>
                            </div>
                        );
                    })}
                </div>
            </div>

            {/* 标签过滤器 */}
            <div className={styles.filterGroup}>
                <div className={styles.filterGroupTitle}>
                    <TagIcon size={16} />
                    {t('modelLibrary.tags')}
                </div>
                <div className={styles.tagSelector}>
                    <Select
                        mode="multiple"
                        placeholder={t('modelLibrary.selectTags')}
                        value={input.tags}
                        onChange={handleTagsChange}
                        options={allTags.map(tag => ({ label: tag, value: tag }))}
                        maxTagCount={2}
                        allowClear
                    />
                </div>
            </div>

            {/* 单位切换 */}
            <div className={styles.filterGroup}>
                <div className={styles.filterGroupTitle}>
                    <Info size={16} />
                    {t('modelLibrary.settings')}
                </div>
                <div style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
                    <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                        <span style={{ fontSize: 14 }}>{t('modelLibrary.unitSwitch')}:</span>
                        <Switch
                            checked={isK}
                            size="small"
                            checkedChildren="K"
                            unCheckedChildren="M"
                            onChange={setIsK}
                        />
                    </div>
                    <LanguageSwitcher size="small" />
                </div>
            </div>

            {/* 清除过滤器 */}
            {(input.type || selectedModelType || input.tags.length > 0) && (
                <div className={styles.filterGroup}>
                    <Button
                        block
                        onClick={() => {
                            setInput({ ...input, type: '', tags: [], modelType: '' });
                            setSelectedModelType('');
                        }}
                        className={styles.clearFiltersButton}
                    >
                        {t('modelLibrary.clearFilters')}
                    </Button>
                </div>
            )}
        </div>
    );

    // 渲染模型卡片 - 简化动画
    const renderModelCard = (item: any, index: number) => {
        const icon = getIconByName(item.icon);
        const modelTypeIcon = getModelTypeIcon(item.type);
        const modelTypeColor = getModelTypeColor(item.type);

        return (
            <div
                key={item.id}
                className={styles.modelCard}
                onClick={() => handleModelClick(item)}
            >
                <div className={styles.modelCardHeader}>
                    <div className={styles.modelCardMeta}>
                        <div style={{ marginRight: 12 }}>
                            {icon?.icon ?? <IconAvatar size={32} Icon={OpenAI} />}
                        </div>
                        <div>
                            <div
                                className={styles.modelName}
                                onClick={(e) => {
                                    e.stopPropagation();
                                    copyModelName(item.model);
                                }}
                            >
                                {item.model}
                            </div>
                            <div style={{ fontSize: 12, color: token.colorTextSecondary }}>
                                {provider[item.provider] || item.provider}
                            </div>
                        </div>
                    </div>
                    <Tag
                        color={item.enable ? 'green' : 'red'}
                    >
                        {item.enable ? t('modelLibrary.available') : t('modelLibrary.unavailable')}
                    </Tag>
                </div>

                <div className={styles.modelDescription}>
                    {item.description}
                </div>

                <div className={styles.modelTags}>
                    <Tag
                        color={modelTypeColor}
                        icon={modelTypeIcon}
                    >
                        {getModelTypeText(item.type)}
                    </Tag>
                    <Tag
                        color={item.quotaType === 1 ? 'blue' : 'orange'}
                    >
                        {item.quotaType === 1 ? t('modelLibrary.volumeBilling') : t('modelLibrary.perUseBilling')}
                    </Tag>

                    {item.tags && item.tags.slice(0, 2).map((tag: string) => (
                        <Tag
                            key={tag}
                            color='blue'
                            style={{ fontSize: 11 }}
                        >
                            {tag}
                        </Tag>
                    ))}
                    {item.tags && item.tags.length > 2 && (
                        <Tag color="default" style={{ fontSize: 11 }}>
                            +{item.tags.length - 2}
                        </Tag>
                    )}
                </div>

                {/* 价格信息 */}
                <div style={{ marginTop: 16 }}>
                    {renderPriceInfo(item)}
                </div>

                {/* 统计信息 */}
                <div className={styles.modelStats}>
                    <div className={styles.modelStat}>
                        <span className={styles.modelStatValue}>{item.quotaMax || '-'}</span>
                        <div className={styles.modelStatLabel}>{t('modelLibrary.maxContext')}</div>
                    </div>
                </div>
            </div>
        );
    };

    return (
        <Layout className={styles.mainLayout}>
            {isMobile && (
                <Button
                    className={styles.mobileFilterButton}
                    type="primary"
                    icon={<Filter size={16} />}
                    onClick={() => setDrawerVisible(true)}
                    shape="circle"
                >
                </Button>
            )}

            {!isMobile && (
                <Sider
                    width={280}
                    style={{
                        marginTop: 8,
                    }}
                    className={styles.leftSider}
                    theme="light"
                >
                    {renderLeftFilters()}
                </Sider>
            )}

            <Drawer
                title={
                    <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                        <Filter size={16} />
                        {t('modelLibrary.filters')}
                    </div>
                }
                placement="left"
                onClose={() => setDrawerVisible(false)}
                open={drawerVisible}
                width={280}
                bodyStyle={{ padding: 0 }}
            >
                {renderLeftFilters()}
            </Drawer>

            {/* 右侧内容区域 */}
            <Content className={styles.rightContent}>
                {/* 模型展示区域 */}
                <div className={styles.modelsContainer}>
                    {loading && (
                        <div style={{
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            height: 300,
                            flexDirection: 'column',
                            gap: 16
                        }}>
                            <Empty
                                description={t('modelLibrary.loading')}
                                image={Empty.PRESENTED_IMAGE_SIMPLE}
                            />
                        </div>
                    )}

                    {!loading && filteredData.length === 0 && (
                        <div style={{
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            height: 300,
                            flexDirection: 'column',
                            gap: 16
                        }}>
                            <Empty
                                description={t('modelLibrary.noResults')}
                                image={Empty.PRESENTED_IMAGE_SIMPLE}
                            />
                            {(input.type || selectedModelType || input.tags.length > 0) && (
                                <Button
                                    type="primary"
                                    onClick={() => {
                                        setInput({ ...input, type: '', tags: [], modelType: '' });
                                        setSelectedModelType('');
                                    }}
                                >
                                    {t('modelLibrary.clearFilters')}
                                </Button>
                            )}
                        </div>
                    )}

                    {!loading && filteredData.length > 0 && (
                        <div className={styles.modelGrid}>
                            {filteredData.map((item, index) => renderModelCard(item, index))}
                        </div>
                    )}
                </div>
                
                {/* 分页组件 */}
                {!loading && filteredData.length > 0 && (
                    <div className={styles.paginationContainer}>
                        <Pagination
                            current={input.page}
                            pageSize={input.pageSize}
                            total={total}
                            onChange={handlePageChange}
                            onShowSizeChange={handlePageChange}
                            showSizeChanger
                            showQuickJumper
                            showTotal={(total, range) => 
                                `${t('modelLibrary.showing')} ${range[0]}-${range[1]} ${t('modelLibrary.of')} ${total} ${t('modelLibrary.items')}`
                            }
                            pageSizeOptions={['10', '20', '50', '100']}
                            size={isMobile ? 'small' : 'default'}
                        />
                    </div>
                )}
            </Content>

            {/* 模型详情模态框 */}
            {renderModelDetailModal()}
        </Layout>
    );
}
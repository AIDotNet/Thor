import { useEffect, useState } from "react";
import { DeleteModelManager, EnableModelManager, GetModelManagerList, GetModelStats, GetModelTypes, GetAllTags } from "../../services/ModelManagerService";
import { Button, message, Space, Input as AntInput, Switch, Typography, ConfigProvider, theme, Tag as AntTag, Card, Flex, Divider, Badge, Drawer, Descriptions, Modal, Empty, Spin } from "antd";
import { getCompletionRatio, renderQuota } from "../../utils/render";
import CreateModelManagerPage from "./features/CreateModelManager";
import { getIconByName } from "../../utils/iconutils";
import { IconAvatar, OpenAI } from "@lobehub/icons";
import { PlusOutlined, SearchOutlined, SettingOutlined, FilterOutlined, ReloadOutlined, EyeOutlined, EditOutlined, DeleteOutlined, CheckCircleOutlined, CloseCircleOutlined } from "@ant-design/icons";
import UpdateModelManagerPage from "./features/UpdateModelManager";
import { useTranslation } from "react-i18next";
import { useResponsive, createStyles } from "antd-style";
import { motion } from 'framer-motion';

// 创建动画组件
const MotionDiv = motion.div;
const MotionCard = motion(Card);

// 使用 antd-style 创建样式
const useStyles = createStyles(({ token, css }) => ({
  container: css`
    height: 100vh;
    display: flex;
    flex-direction: column;
    background: ${token.colorBgLayout};
    overflow: hidden;
  `,
  
  header: css`
    background: ${token.colorBgContainer};
    border-bottom: 1px solid ${token.colorBorderSecondary};
    padding: 16px 24px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
    z-index: 10;
    
    @media (max-width: 768px) {
      padding: 12px 16px;
    }
  `,
  
  headerTitle: css`
    margin: 0;
    color: ${token.colorTextHeading};
    font-weight: 600;
    font-size: 20px;
    line-height: 1.4;
    
    @media (max-width: 768px) {
      font-size: 18px;
    }
  `,
  
  toolbar: css`
    background: ${token.colorBgContainer};
    border-bottom: 1px solid ${token.colorBorderSecondary};
    padding: 16px 24px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    flex-wrap: wrap;
    gap: 16px;
    
    @media (max-width: 768px) {
      padding: 12px 16px;
      flex-direction: column;
      align-items: stretch;
      gap: 12px;
    }
  `,
  
  toolbarLeft: css`
    display: flex;
    align-items: center;
    gap: 16px;
    flex: 1;
    
    @media (max-width: 768px) {
      flex-direction: column;
      align-items: stretch;
      gap: 12px;
    }
  `,
  
  toolbarRight: css`
    display: flex;
    align-items: center;
    gap: 12px;
    
    @media (max-width: 768px) {
      justify-content: center;
    }
  `,
  
  searchInput: css`
    width: 300px;
    
    .ant-input-affix-wrapper {
      border-radius: ${token.borderRadius}px;
      
      &:hover {
        border-color: ${token.colorPrimaryHover};
      }
      
      &:focus-within {
        border-color: ${token.colorPrimary};
        box-shadow: 0 0 0 2px ${token.colorPrimaryBg};
      }
    }
    
    @media (max-width: 768px) {
      width: 100%;
    }
  `,
  
  priceSwitch: css`
    display: flex;
    align-items: center;
    gap: 8px;
    white-space: nowrap;
    
    .ant-switch-checked {
      background: linear-gradient(90deg, ${token.colorPrimary} 0%, ${token.colorPrimaryActive} 100%);
    }
  `,
  
  actionButton: css`
    border-radius: ${token.borderRadius}px;
    transition: all 0.3s cubic-bezier(0.2, 0.8, 0.2, 1);
    
    &.ant-btn-primary {
      background: linear-gradient(90deg, ${token.colorPrimary} 0%, ${token.colorPrimaryActive} 100%);
      border: none;
      box-shadow: 0 2px 8px ${token.colorPrimaryBg};
      
      &:hover {
        background: linear-gradient(90deg, ${token.colorPrimaryHover} 0%, ${token.colorPrimaryActive} 100%);
        box-shadow: 0 4px 12px ${token.colorPrimaryBgHover};
        transform: translateY(-1px);
      }
    }
    
    &:not(.ant-btn-primary) {
      &:hover {
        border-color: ${token.colorPrimaryHover};
        color: ${token.colorPrimaryHover};
      }
    }
  `,
  
  mainContent: css`
    flex: 1;
    display: flex;
    overflow: hidden;
    background: ${token.colorBgLayout};
  `,
  
  sidebar: css`
    width: 280px;
    background: ${token.colorBgContainer};
    border-right: 1px solid ${token.colorBorderSecondary};
    padding: 20px;
    overflow-y: auto;
    
    @media (max-width: 1024px) {
      display: none;
    }
  `,
  
  sidebarSection: css`
    margin-bottom: 24px;
    
    .section-title {
      font-size: 14px;
      font-weight: 600;
      color: ${token.colorTextHeading};
      margin-bottom: 12px;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }
  `,
  
  filterItem: css`
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 8px 12px;
    border-radius: ${token.borderRadius}px;
    cursor: pointer;
    transition: all 0.2s;
    margin-bottom: 4px;
    
    &:hover {
      background: ${token.colorFillAlter};
    }
    
    &.active {
      background: ${token.colorPrimaryBg};
      color: ${token.colorPrimary};
      font-weight: 500;
    }
    
    .filter-name {
      flex: 1;
    }
    
    .filter-count {
      font-size: 12px;
      color: ${token.colorTextSecondary};
      background: ${token.colorFillSecondary};
      padding: 2px 6px;
      border-radius: 10px;
      min-width: 20px;
      text-align: center;
    }
    
    &.active .filter-count {
      background: ${token.colorPrimary};
      color: ${token.colorWhite};
    }
  `,
  
  contentArea: css`
    flex: 1;
    padding: 20px;
    overflow-y: auto;
    
    @media (max-width: 768px) {
      padding: 16px;
    }
  `,
  
  statsRow: css`
    background: ${token.colorBgContainer};
    border-radius: ${token.borderRadiusLG}px;
    padding: 16px 20px;
    margin-bottom: 20px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
    border: 1px solid ${token.colorBorderSecondary};
    
    @media (max-width: 768px) {
      padding: 12px 16px;
    }
  `,
  
  statItem: css`
    text-align: center;
    
    .stat-number {
      font-size: 20px;
      font-weight: 600;
      color: ${token.colorTextHeading};
      margin-bottom: 4px;
    }
    
    .stat-label {
      font-size: 12px;
      color: ${token.colorTextSecondary};
    }
  `,
  
  modelGrid: css`
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
    gap: 16px;
    
    @media (max-width: 768px) {
      grid-template-columns: 1fr;
      gap: 12px;
    }
  `,
  
  modelCard: css`
    background: ${token.colorBgContainer};
    border: 1px solid ${token.colorBorderSecondary};
    border-radius: ${token.borderRadiusLG}px;
    padding: 20px;
    cursor: pointer;
    transition: all 0.3s cubic-bezier(0.2, 0.8, 0.2, 1);
    position: relative;
    overflow: hidden;
    
    &:hover {
      border-color: ${token.colorPrimary};
      box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
      transform: translateY(-2px);
    }
    
    &.disabled {
      opacity: 0.6;
      
      &:hover {
        transform: none;
        box-shadow: none;
        border-color: ${token.colorBorderSecondary};
      }
    }
  `,
  
  cardHeader: css`
    display: flex;
    align-items: center;
    margin-bottom: 16px;
  `,
  
  iconContainer: css`
    display: flex;
    align-items: center;
    justify-content: center;
    width: 48px;
    height: 48px;
    border-radius: ${token.borderRadius}px;
    background: linear-gradient(135deg, ${token.colorPrimaryBg} 0%, ${token.colorBgContainer} 100%);
    margin-right: 12px;
    transition: all 0.3s;
  `,
  
  cardInfo: css`
    flex: 1;
    min-width: 0;
  `,
  
  modelName: css`
    font-size: 16px;
    font-weight: 600;
    color: ${token.colorTextHeading};
    margin-bottom: 4px;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  `,
  
  modelDescription: css`
    font-size: 12px;
    color: ${token.colorTextSecondary};
    line-height: 1.4;
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
  `,
  
  cardTags: css`
    margin-bottom: 12px;
    display: flex;
    flex-wrap: wrap;
    gap: 4px;
  `,
  
  statusBadge: css`
    position: absolute;
    top: 12px;
    right: 12px;
    
    .ant-badge-status-dot {
      width: 8px;
      height: 8px;
    }
  `,
  
  cardFooter: css`
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-top: 16px;
    padding-top: 16px;
    border-top: 1px solid ${token.colorBorderSecondary};
  `,
  
  priceInfo: css`
    flex: 1;
    
    .price-tag {
      font-size: 11px;
      padding: 2px 6px;
      border-radius: 4px;
      margin-right: 4px;
      margin-bottom: 4px;
      display: inline-block;
    }
  `,
  
  cardActions: css`
    display: flex;
    gap: 8px;
    
    .ant-btn {
      padding: 4px 8px;
      height: auto;
      font-size: 12px;
      border-radius: 4px;
    }
  `,
  
  mobileFilterButton: css`
    display: none;
    
    @media (max-width: 1024px) {
      display: inline-flex;
    }
  `,
  
  drawerContent: css`
    .ant-drawer-body {
      padding: 0;
    }
  `,
  
  emptyState: css`
    padding: 60px 20px;
    text-align: center;
    
    .empty-icon {
      font-size: 48px;
      color: ${token.colorTextDisabled};
      margin-bottom: 16px;
    }
    
    .empty-title {
      font-size: 16px;
      color: ${token.colorTextHeading};
      margin-bottom: 8px;
    }
    
    .empty-description {
      color: ${token.colorTextSecondary};
      margin-bottom: 24px;
    }
  `,
  
  loadingOverlay: css`
    position: relative;
    min-height: 200px;
    
    .ant-spin {
      position: absolute;
      top: 50%;
      left: 50%;
      transform: translate(-50%, -50%);
    }
  `
}));

// 页面动画变量
const pageVariants = {
  initial: { opacity: 0, y: 20 },
  animate: { opacity: 1, y: 0 },
  exit: { opacity: 0, y: -20 }
};

const cardVariants = {
  hidden: { opacity: 0, y: 20 },
  visible: { opacity: 1, y: 0 }
};

// 模型类型配置
const MODEL_TYPES = [
  { key: 'all', label: '全部模型', icon: '📱' },
  { key: 'chat', label: '对话模型', icon: '💬' },
  { key: 'image', label: '图像模型', icon: '🖼️' },
  { key: 'audio', label: '音频模型', icon: '🎵' },
  { key: 'embedding', label: '嵌入模型', icon: '🔗' },
  { key: 'stt', label: '语音识别', icon: '🎤' },
  { key: 'tts', label: '语音合成', icon: '🔊' }
];

export default function ModelManager() {
    const { t } = useTranslation();
    const { mobile } = useResponsive();
    const { token } = theme.useToken();
    const { styles } = useStyles();

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
        pageSize: 50,
        model: '',
    });
    const [selectedModel, setSelectedModel] = useState<any>(null);
    const [detailDrawerOpen, setDetailDrawerOpen] = useState<boolean>(false);
    const [filterDrawerOpen, setFilterDrawerOpen] = useState<boolean>(false);
    const [activeFilter, setActiveFilter] = useState<string>('all');
    const [enabledFilter, setEnabledFilter] = useState<string>('all'); // all, enabled, disabled
    const [modelStats, setModelStats] = useState<any>({});
    const [availableTypes, setAvailableTypes] = useState<string[]>([]);
    const [availableTags, setAvailableTags] = useState<string[]>([]);
    const [statsLoading, setStatsLoading] = useState<boolean>(false);

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

    // 加载统计信息
    function loadStats() {
        setStatsLoading(true);
        GetModelStats()
            .then((res) => {
                if (res.success) {
                    setModelStats(res.data);
                }
            }).finally(() => {
                setStatsLoading(false);
            });
    }

    // 加载模型类型
    function loadTypes() {
        GetModelTypes()
            .then((res) => {
                if (res.success) {
                    setAvailableTypes(res.data);
                }
            });
    }

    // 加载标签
    function loadTags() {
        GetAllTags()
            .then((res) => {
                if (res.success) {
                    setAvailableTags(res.data);
                }
            });
    }

    useEffect(() => {
        loadData();
        loadStats();
        loadTypes();
        loadTags();
    }, [input]);

    // 过滤数据
    const filteredData = data.filter(item => {
        // 类型过滤
        if (activeFilter !== 'all' && item.type !== activeFilter) {
            return false;
        }
        
        // 状态过滤
        if (enabledFilter === 'enabled' && !item.enable) {
            return false;
        }
        if (enabledFilter === 'disabled' && item.enable) {
            return false;
        }
        
        return true;
    });

    // 计算统计信息
    const getTypeCount = (type: string) => {
        if (type === 'all') return modelStats.total || 0;
        return modelStats[type] || 0;
    };

    const enabledCount = modelStats.enabled || 0;
    const disabledCount = modelStats.disabled || 0;

    const renderPrice = (item: any) => {
        if (isK) {
            if (item.quotaType === 1) {
                return (
                    <div className={styles.priceInfo}>
                        <span className="price-tag" style={{ background: token.colorInfoBg, color: token.colorInfoText }}>
                            输入: {renderQuota(item.promptRate * 1000, 6)}/1K
                        </span>
                        <span className="price-tag" style={{ background: token.colorSuccessBg, color: token.colorSuccessText }}>
                            输出: {renderQuota(item.completionRate ? item.completionRate * 1000 : getCompletionRatio(item.model) * 1000, 6)}/1K
                        </span>
                        {item.isVersion2 && (
                            <>
                                <span className="price-tag" style={{ background: token.colorWarningBg, color: token.colorWarningText }}>
                                    音频输入: {renderQuota(item.audioPromptRate * 1000)}/1K
                                </span>
                                <span className="price-tag" style={{ background: token.colorErrorBg, color: token.colorErrorText }}>
                                    音频输出: {renderQuota(item.audioOutputRate ? item.audioOutputRate * 1000 : getCompletionRatio(item.model) * 1000)}/1K
                                </span>
                            </>
                        )}
                    </div>
                );
            } else {
                return (
                    <div className={styles.priceInfo}>
                        <span className="price-tag" style={{ background: token.colorPrimaryBg, color: token.colorPrimary }}>
                            按次: {renderQuota(item.promptRate, 6)}
                        </span>
                    </div>
                );
            }
        } else {
            if (item.quotaType === 1) {
                return (
                    <div className={styles.priceInfo}>
                        <span className="price-tag" style={{ background: token.colorInfoBg, color: token.colorInfoText }}>
                            输入: {renderQuota(item.promptRate * 1000000)}/1M
                        </span>
                        <span className="price-tag" style={{ background: token.colorSuccessBg, color: token.colorSuccessText }}>
                            输出: {renderQuota(item.completionRate ? item.completionRate * 1000000 : getCompletionRatio(item.model) * 1000000)}/1M
                        </span>
                        {item.isVersion2 && (
                            <>
                                <span className="price-tag" style={{ background: token.colorWarningBg, color: token.colorWarningText }}>
                                    音频输入: {renderQuota(item.audioPromptRate * 1000000)}/1M
                                </span>
                                <span className="price-tag" style={{ background: token.colorErrorBg, color: token.colorErrorText }}>
                                    音频输出: {renderQuota(item.audioOutputRate ? item.audioOutputRate * 1000000 : getCompletionRatio(item.model) * 1000000)}/1M
                                </span>
                            </>
                        )}
                    </div>
                );
            } else {
                return (
                    <div className={styles.priceInfo}>
                        <span className="price-tag" style={{ background: token.colorPrimaryBg, color: token.colorPrimary }}>
                            按次: {renderQuota(item.promptRate, 6)}
                        </span>
                    </div>
                );
            }
        }
    };

    const handleModelClick = (model: any) => {
        setSelectedModel(model);
        setDetailDrawerOpen(true);
    };

    const handleEnableModel = async (id: string, currentStatus: boolean) => {
        try {
            const res = await EnableModelManager(id);
            if (res.success) {
                message.success(currentStatus ? '模型已禁用' : '模型已启用');
                loadData();
                loadStats();
            } else {
                message.error(res.message);
            }
        } catch (error) {
            message.error('操作失败');
        }
    };

    const handleDeleteModel = async (id: string) => {
        Modal.confirm({
            title: '确认删除',
            content: '确定要删除这个模型吗？删除后无法恢复。',
            okText: '确认',
            cancelText: '取消',
            okType: 'danger',
            onOk: async () => {
                try {
                    const res = await DeleteModelManager(id);
                    if (res.success) {
                        message.success('删除成功');
                        loadData();
                        loadStats();
                        loadTypes();
                        loadTags();
                    } else {
                        message.error(res.message);
                    }
                } catch (error) {
                    message.error('删除失败');
                }
            }
        });
    };

    // 渲染侧边栏
    const renderSidebar = () => (
        <div className={styles.sidebar}>
            {/* 模型类型 */}
            <div className={styles.sidebarSection}>
                <div className="section-title">模型类型</div>
                {MODEL_TYPES.map(type => (
                    <div
                        key={type.key}
                        className={`${styles.filterItem} ${activeFilter === type.key ? 'active' : ''}`}
                        onClick={() => setActiveFilter(type.key)}
                    >
                        <span className="filter-name">
                            <span style={{ marginRight: 8 }}>{type.icon}</span>
                            {type.label}
                        </span>
                        <span className="filter-count">{getTypeCount(type.key)}</span>
                    </div>
                ))}
                
                {/* 显示其他动态类型 */}
                {availableTypes.filter(type => !MODEL_TYPES.some(mt => mt.key === type)).map(type => (
                    <div
                        key={type}
                        className={`${styles.filterItem} ${activeFilter === type ? 'active' : ''}`}
                        onClick={() => setActiveFilter(type)}
                    >
                        <span className="filter-name">
                            <span style={{ marginRight: 8 }}>🔧</span>
                            {type}
                        </span>
                        <span className="filter-count">{getTypeCount(type)}</span>
                    </div>
                ))}
            </div>

            {/* 模型状态 */}
            <div className={styles.sidebarSection}>
                <div className="section-title">模型状态</div>
                <div
                    className={`${styles.filterItem} ${enabledFilter === 'all' ? 'active' : ''}`}
                    onClick={() => setEnabledFilter('all')}
                >
                    <span className="filter-name">全部状态</span>
                    <span className="filter-count">{modelStats.total || 0}</span>
                </div>
                <div
                    className={`${styles.filterItem} ${enabledFilter === 'enabled' ? 'active' : ''}`}
                    onClick={() => setEnabledFilter('enabled')}
                >
                    <span className="filter-name">
                        <CheckCircleOutlined style={{ color: token.colorSuccess, marginRight: 8 }} />
                        已启用
                    </span>
                    <span className="filter-count">{enabledCount}</span>
                </div>
                <div
                    className={`${styles.filterItem} ${enabledFilter === 'disabled' ? 'active' : ''}`}
                    onClick={() => setEnabledFilter('disabled')}
                >
                    <span className="filter-name">
                        <CloseCircleOutlined style={{ color: token.colorError, marginRight: 8 }} />
                        已禁用
                    </span>
                    <span className="filter-count">{disabledCount}</span>
                </div>
            </div>

            {/* 标签筛选 */}
            {availableTags.length > 0 && (
                <div className={styles.sidebarSection}>
                    <div className="section-title">标签筛选</div>
                    <div className="tag-filter-container" style={{ display: 'flex', flexWrap: 'wrap', gap: 4 }}>
                        {availableTags.slice(0, 10).map(tag => (
                            <AntTag 
                                key={tag} 
                                color="blue" 
                                style={{ 
                                    cursor: 'pointer', 
                                    marginBottom: 4,
                                    fontSize: 11 
                                }}
                                onClick={() => {
                                    // 可以添加标签筛选逻辑
                                    console.log('Filter by tag:', tag);
                                }}
                            >
                                {tag}
                            </AntTag>
                        ))}
                        {availableTags.length > 10 && (
                            <AntTag color="default" style={{ fontSize: 11 }}>
                                +{availableTags.length - 10}
                            </AntTag>
                        )}
                    </div>
                </div>
            )}
        </div>
    );

    // 渲染模型卡片
    const renderModelCard = (model: any, index: number) => {
        const icon = getIconByName(model.icon);
        const isDisabled = !model.enable;

        return (
            <MotionCard
                key={model.id}
                className={`${styles.modelCard} ${isDisabled ? 'disabled' : ''}`}
                variants={cardVariants}
                initial="hidden"
                animate="visible"
                transition={{ duration: 0.3, delay: index * 0.05 }}
                onClick={() => handleModelClick(model)}
            >
                {/* 状态标识 */}
                <Badge
                    className={styles.statusBadge}
                    status={model.enable ? 'success' : 'error'}
                    text={model.enable ? '已启用' : '已禁用'}
                />

                {/* 卡片头部 */}
                <div className={styles.cardHeader}>
                    <div className={styles.iconContainer}>
                        {icon?.icon ?? <IconAvatar size={32} Icon={OpenAI} />}
                    </div>
                    <div className={styles.cardInfo}>
                        <div className={styles.modelName} title={model.model}>
                            {model.model}
                        </div>
                        {model.description && (
                            <div className={styles.modelDescription} title={model.description}>
                                {model.description}
                            </div>
                        )}
                    </div>
                </div>

                {/* 标签 */}
                {model.tags && model.tags.length > 0 && (
                    <div className={styles.cardTags}>
                        {model.tags.slice(0, 3).map((tag: string) => (
                            <AntTag key={tag} color="blue">
                                {tag}
                            </AntTag>
                        ))}
                        {model.tags.length > 3 && (
                            <AntTag color="default">
                                +{model.tags.length - 3}
                            </AntTag>
                        )}
                    </div>
                )}

                {/* 卡片底部 */}
                <div className={styles.cardFooter}>
                    {renderPrice(model)}
                    <div className={styles.cardActions} onClick={(e) => e.stopPropagation()}>
                        <Button
                            size="small"
                            icon={<EditOutlined />}
                            onClick={() => setUpdateValue({ value: model, open: true })}
                        >
                            编辑
                        </Button>
                        <Button
                            size="small"
                            type={model.enable ? 'default' : 'primary'}
                            icon={model.enable ? <CloseCircleOutlined /> : <CheckCircleOutlined />}
                            onClick={() => handleEnableModel(model.id, model.enable)}
                        >
                            {model.enable ? '禁用' : '启用'}
                        </Button>
                        <Button
                            size="small"
                            danger
                            icon={<DeleteOutlined />}
                            onClick={() => handleDeleteModel(model.id)}
                        >
                            删除
                        </Button>
                    </div>
                </div>
            </MotionCard>
        );
    };

    return (
        <ConfigProvider theme={{
            components: {
                Card: {
                    headerBg: 'transparent',
                    colorBgContainer: token.colorBgContainer,
                },
                Switch: {
                    colorPrimary: token.colorPrimary,
                    colorPrimaryHover: token.colorPrimaryHover,
                },
                Drawer: {
                    colorBgElevated: token.colorBgContainer,
                }
            }
        }}>
            <MotionDiv 
                className={styles.container}
                variants={pageVariants}
                initial="initial"
                animate="animate"
                exit="exit"
                transition={{ duration: 0.3 }}
            >
                {/* 页面头部 */}
                <div className={styles.header}>
                    <Typography.Title level={3} className={styles.headerTitle}>
                        {t('modelManager.title')}
                    </Typography.Title>
                </div>

                {/* 工具栏 */}
                <div className={styles.toolbar}>
                    <div className={styles.toolbarLeft}>
                        <AntInput.Search
                            placeholder={t('modelManager.searchModel')}
                            value={input.model}
                            onChange={(e) => setInput({ ...input, model: e.target.value })}
                            onSearch={() => loadData()}
                            className={styles.searchInput}
                            allowClear
                        />
                        
                        <div className={styles.priceSwitch}>
                            <Typography.Text style={{ color: token.colorTextSecondary, fontSize: 14 }}>
                                {t('modelManager.priceUnit')}:
                            </Typography.Text>
                            <Switch 
                                value={isK} 
                                checkedChildren={t('modelManager.modelPricePerK')} 
                                unCheckedChildren={t('modelManager.modelPricePerM')} 
                                onChange={(checked) => setIsK(checked)} 
                            />
                        </div>
                        
                        <Button 
                            icon={<FilterOutlined />}
                            className={`${styles.actionButton} ${styles.mobileFilterButton}`}
                            onClick={() => setFilterDrawerOpen(true)}
                        >
                            筛选
                        </Button>
                    </div>
                    
                    <div className={styles.toolbarRight}>
                        <Button 
                            icon={<ReloadOutlined />}
                            onClick={() => loadData()}
                            className={styles.actionButton}
                            loading={loading}
                        >
                            {!mobile && '刷新'}
                        </Button>
                        
                        <Button 
                            type="primary"
                            icon={<PlusOutlined />} 
                            onClick={() => setCreateOpen(true)}
                            className={styles.actionButton}
                        >
                            {t('modelManager.createModel')}
                        </Button>
                    </div>
                </div>

                {/* 主要内容区域 */}
                <div className={styles.mainContent}>
                    {/* 左侧边栏 */}
                    {renderSidebar()}

                    {/* 右侧内容区域 */}
                    <div className={styles.contentArea}>
                        {/* 统计信息 */}
                        <div className={styles.statsRow}>
                            {statsLoading ? (
                                <Flex justify="center" align="center" style={{ padding: '20px 0' }}>
                                    <Spin size="small" />
                                    <span style={{ marginLeft: 8, color: token.colorTextSecondary }}>
                                        加载统计信息...
                                    </span>
                                </Flex>
                            ) : (
                                <Flex justify="space-around" align="center">
                                    <div className={styles.statItem}>
                                        <div className="stat-number">{modelStats.total || 0}</div>
                                        <div className="stat-label">总计</div>
                                    </div>
                                    <Divider type="vertical" style={{ height: 40 }} />
                                    <div className={styles.statItem}>
                                        <div className="stat-number" style={{ color: token.colorSuccess }}>{enabledCount}</div>
                                        <div className="stat-label">已启用</div>
                                    </div>
                                    <Divider type="vertical" style={{ height: 40 }} />
                                    <div className={styles.statItem}>
                                        <div className="stat-number" style={{ color: token.colorError }}>{disabledCount}</div>
                                        <div className="stat-label">已禁用</div>
                                    </div>
                                    <Divider type="vertical" style={{ height: 40 }} />
                                    <div className={styles.statItem}>
                                        <div className="stat-number" style={{ color: token.colorPrimary }}>{filteredData.length}</div>
                                        <div className="stat-label">当前显示</div>
                                    </div>
                                    <Divider type="vertical" style={{ height: 40 }} />
                                    <div className={styles.statItem}>
                                        <div className="stat-number" style={{ color: token.colorWarning }}>{modelStats.chat || 0}</div>
                                        <div className="stat-label">对话模型</div>
                                    </div>
                                    <Divider type="vertical" style={{ height: 40 }} />
                                    <div className={styles.statItem}>
                                        <div className="stat-number" style={{ color: token.colorInfo }}>{modelStats.image || 0}</div>
                                        <div className="stat-label">图像模型</div>
                                    </div>
                                </Flex>
                            )}
                        </div>

                        {/* 模型网格 */}
                        {loading ? (
                            <div className={styles.loadingOverlay}>
                                <Spin size="large" />
                            </div>
                        ) : filteredData.length > 0 ? (
                            <div className={styles.modelGrid}>
                                {filteredData.map((model, index) => renderModelCard(model, index))}
                            </div>
                        ) : (
                            <div className={styles.emptyState}>
                                <div className="empty-icon">📦</div>
                                <div className="empty-title">暂无模型</div>
                                <div className="empty-description">
                                    {input.model ? '没有找到匹配的模型，请尝试其他搜索条件' : '还没有添加任何模型，点击上方按钮创建第一个模型'}
                                </div>
                                {!input.model && (
                                    <Button 
                                        type="primary" 
                                        icon={<PlusOutlined />}
                                        onClick={() => setCreateOpen(true)}
                                        className={styles.actionButton}
                                    >
                                        创建模型
                                    </Button>
                                )}
                            </div>
                        )}
                    </div>
                </div>

                {/* 移动端筛选抽屉 */}
                <Drawer
                    title="筛选条件"
                    placement="left"
                    onClose={() => setFilterDrawerOpen(false)}
                    open={filterDrawerOpen}
                    width={280}
                    className={styles.drawerContent}
                >
                    {renderSidebar()}
                </Drawer>

                {/* 模型详情抽屉 */}
                <Drawer
                    title="模型详情"
                    placement="right"
                    onClose={() => setDetailDrawerOpen(false)}
                    open={detailDrawerOpen}
                    width={500}
                >
                    {selectedModel && (
                        <div>
                            <Descriptions column={1} bordered>
                                <Descriptions.Item label="模型名称">{selectedModel.model}</Descriptions.Item>
                                <Descriptions.Item label="模型描述">{selectedModel.description || '-'}</Descriptions.Item>
                                <Descriptions.Item label="模型类型">
                                    <AntTag color="blue">{selectedModel.type}</AntTag>
                                </Descriptions.Item>
                                <Descriptions.Item label="状态">
                                    <Badge 
                                        status={selectedModel.enable ? 'success' : 'error'}
                                        text={selectedModel.enable ? '已启用' : '已禁用'}
                                    />
                                </Descriptions.Item>
                                <Descriptions.Item label="计费类型">
                                    {selectedModel.quotaType === 1 ? '按量计费' : '按次计费'}
                                </Descriptions.Item>
                                <Descriptions.Item label="最大上下文">{selectedModel.quotaMax?.toLocaleString() || '-'}</Descriptions.Item>
                                <Descriptions.Item label="标签">
                                    {selectedModel.tags?.map((tag: string) => (
                                        <AntTag key={tag} color="blue" style={{ marginBottom: 4 }}>{tag}</AntTag>
                                    )) || '-'}
                                </Descriptions.Item>
                                <Descriptions.Item label="定价信息">
                                    {renderPrice(selectedModel)}
                                </Descriptions.Item>
                            </Descriptions>
                            
                            <div style={{ marginTop: 20 }}>
                                <Space>
                                    <Button 
                                        type="primary"
                                        icon={<EditOutlined />}
                                        onClick={() => {
                                            setUpdateValue({ value: selectedModel, open: true });
                                            setDetailDrawerOpen(false);
                                        }}
                                    >
                                        编辑模型
                                    </Button>
                                    <Button 
                                        type={selectedModel.enable ? 'default' : 'primary'}
                                        icon={selectedModel.enable ? <CloseCircleOutlined /> : <CheckCircleOutlined />}
                                        onClick={() => {
                                            handleEnableModel(selectedModel.id, selectedModel.enable);
                                            setDetailDrawerOpen(false);
                                        }}
                                    >
                                        {selectedModel.enable ? '禁用模型' : '启用模型'}
                                    </Button>
                                    <Button 
                                        danger
                                        icon={<DeleteOutlined />}
                                        onClick={() => {
                                            handleDeleteModel(selectedModel.id);
                                            setDetailDrawerOpen(false);
                                        }}
                                    >
                                        删除模型
                                    </Button>
                                </Space>
                            </div>
                        </div>
                    )}
                </Drawer>
                
                <CreateModelManagerPage 
                    open={createOpen} 
                    onClose={() => setCreateOpen(false)} 
                    onOk={() => {
                        loadData();
                        loadStats();
                        loadTypes();
                        loadTags();
                        setCreateOpen(false);
                    }} 
                />
                
                <UpdateModelManagerPage 
                    open={updateValue.open} 
                    onClose={() => setUpdateValue({ ...updateValue, open: false })} 
                    onOk={() => {
                        loadData();
                        loadStats();
                        loadTypes();
                        loadTags();
                        setUpdateValue({ ...updateValue, open: false });
                    }} 
                    value={updateValue.value} 
                />
            </MotionDiv>
        </ConfigProvider>
    );
}
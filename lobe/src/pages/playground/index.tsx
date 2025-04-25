import { useEffect, useState, useRef } from 'react';
import { Card, Select, Input, Button, Typography, Avatar, theme, message, Spin, Divider, Modal, Form, Radio, Empty, Grid, Space, Tag, Popconfirm, Tooltip } from 'antd';
import { OpenAI } from 'openai';
import { getModels } from '../../services/ModelService';
import { getTokens } from '../../services/TokenService';
import { SendOutlined, UserOutlined, DeleteOutlined, SettingOutlined, SaveOutlined, HistoryOutlined, StarFilled, StarOutlined, EditOutlined, ReloadOutlined, CopyOutlined, MessageOutlined, TagOutlined, SortAscendingOutlined } from '@ant-design/icons';
import { Flexbox } from 'react-layout-kit';
import { useTranslation } from 'react-i18next';
import { IconAvatar, OpenAI as OpenAIIcon } from '@lobehub/icons';
import ReactMarkdown from 'react-markdown';
import { isMobileDevice } from '../../utils/responsive';
import { info } from '../../services/UserService';

const { Option } = Select;
const { TextArea } = Input;
const { Title, Text, Paragraph } = Typography;
const { useBreakpoint } = Grid;

// 定义消息类型
interface Message {
    id?: string;
    role: 'user' | 'assistant' | 'system';
    content: string;
    timestamp?: number;
    performance?: {
        firstTokenTime?: number;   // 首个token的时间（ms）
        completionTime?: number;   // 完成时间（ms）
        totalTokens?: number;      // 总token数
        tokensPerSecond?: number;  // 每秒token数
    };
}

// 定义聊天记录类型
interface ChatHistory {
    id: string;
    title: string;
    messages: Message[];
    model: string;
    tokenId: string;
    createdAt: number;
    tags?: string[];
    pinned?: boolean;
}

export default function Playground() {
    const { token } = theme.useToken();
    const { t } = useTranslation();
    const screens = useBreakpoint();
    const isMobile = !screens.md || isMobileDevice();
    const [loading, setLoading] = useState(false);
    const [modelOptions, setModelOptions] = useState<string[]>([]);
    const [tokenOptions, setTokenOptions] = useState<any[]>([]);
    const [selectedModel, setSelectedModel] = useState<string>('');
    const [selectedToken, setSelectedToken] = useState<string>('');
    const [prompt, setPrompt] = useState<string>('');
    const [messages, setMessages] = useState<Message[]>([]);
    const [systemPrompt, setSystemPrompt] = useState<string>('你是一个有用的AI助手。');
    const [streaming, setStreaming] = useState(false);
    const [currentAssistantMessage, setCurrentAssistantMessage] = useState('');
    const [settingsVisible, setSettingsVisible] = useState(false);
    const [historyVisible, setHistoryVisible] = useState(false);
    const [chatHistories, setChatHistories] = useState<ChatHistory[]>([]);
    const [temperature, setTemperature] = useState(0.7);
    const [saveModalVisible, setSaveModalVisible] = useState(false);
    const [chatTitle, setChatTitle] = useState('');
    const chatContainerRef = useRef<HTMLDivElement>(null);
    const [form] = Form.useForm();
    const [searchHistoryText, setSearchHistoryText] = useState('');
    const [historyTags, setHistoryTags] = useState<string[]>([]);
    const [selectedHistoryTags, setSelectedHistoryTags] = useState<string[]>([]);
    const [historySort, setHistorySort] = useState<'newest' | 'oldest'>('newest');
    const [editHistoryId, setEditHistoryId] = useState<string | null>(null);
    const [editHistoryTags, setEditHistoryTags] = useState<string[]>([]);
    const [editHistoryTitle, setEditHistoryTitle] = useState('');
    const [pinnedHistories, setPinnedHistories] = useState<ChatHistory[]>([]);
    const [regularHistories, setRegularHistories] = useState<ChatHistory[]>([]);
    const [regeneratingMessageId, setRegeneratingMessageId] = useState<string | null>(null);

    const [user, setUser] = useState({} as any);

    const [performanceData, setPerformanceData] = useState<{
        startTime: number;
        firstTokenTime: number | null;
        completionTime: number | null;
        tokenCount: number;
    }>({
        startTime: 0,
        firstTokenTime: null,
        completionTime: null,
        tokenCount: 0,
    });

    function loadUser() {
        info()
            .then((res) => {
                setUser(res.data);
            })
    }

    // 加载模型和Token
    useEffect(() => {
        loadUser();
        fetchTokens();
        loadChatHistories();
    }, []);

    // 当用户选择了token时加载对应的模型
    useEffect(() => {
        if (selectedToken) {
            fetchModels(selectedToken);
        }
    }, [selectedToken]);

    // 当消息更新时滚动到底部
    useEffect(() => {
        if (chatContainerRef.current) {
            chatContainerRef.current.scrollTop = chatContainerRef.current.scrollHeight;
        }
    }, [messages, currentAssistantMessage]);

    // 处理聊天历史加载与分类
    useEffect(() => {
        if (chatHistories.length > 0) {
            // 提取所有标签
            const allTags = new Set<string>();
            chatHistories.forEach(history => {
                if (history.tags && history.tags.length > 0) {
                    history.tags.forEach(tag => allTags.add(tag));
                }
            });
            setHistoryTags(Array.from(allTags));

            // 分类历史记录
            const pinned: ChatHistory[] = [];
            const regular: ChatHistory[] = [];

            chatHistories.forEach(history => {
                if (history.pinned) {
                    pinned.push(history);
                } else {
                    regular.push(history);
                }
            });

            setPinnedHistories(pinned);
            setRegularHistories(regular);
        }
    }, [chatHistories]);

    // 处理历史记录过滤
    useEffect(() => {
        if (chatHistories.length > 0) {
            // 根据搜索、标签和排序过滤历史记录
            let filtered = [...chatHistories];
            
            // 搜索过滤
            if (searchHistoryText) {
                const searchLower = searchHistoryText.toLowerCase();
                filtered = filtered.filter(history => 
                    history.title.toLowerCase().includes(searchLower) || 
                    (history.tags && history.tags.some(tag => tag.toLowerCase().includes(searchLower)))
                );
            }
            
            // 标签过滤
            if (selectedHistoryTags.length > 0) {
                filtered = filtered.filter(history => 
                    history.tags && selectedHistoryTags.every(tag => history.tags?.includes(tag))
                );
            }
            
            // 排序
            filtered.sort((a, b) => {
                if (historySort === 'newest') {
                    return b.createdAt - a.createdAt;
                } else {
                    return a.createdAt - b.createdAt;
                }
            });
            
            // 分类历史记录
            const pinned: ChatHistory[] = [];
            const regular: ChatHistory[] = [];
            
            filtered.forEach(history => {
                if (history.pinned) {
                    pinned.push(history);
                } else {
                    regular.push(history);
                }
            });
            
            setPinnedHistories(pinned);
            setRegularHistories(regular);
        } else {
            setPinnedHistories([]);
            setRegularHistories([]);
        }
    }, [chatHistories, searchHistoryText, selectedHistoryTags, historySort]);

    const fetchModels = async (tokenId: string) => {
        setModelOptions([]);
        setLoading(true);
        try {
            // 使用选定的token获取模型列表
            const baseURL = window.location.origin;
            const openai = new OpenAI({
                apiKey: tokenId,
                baseURL: `${baseURL}/v1`,
                dangerouslyAllowBrowser: true,
            });

            // 尝试获取可用模型
            const response = await openai.models.list();
            if (response.data) {
                const models = response.data.map(model => model.id);
                setModelOptions(models);
                if (models.length > 0) {
                    setSelectedModel(models[0]);
                }
            }
        } catch (error) {
            console.error('Failed to fetch models:', error);
            message.error('获取模型列表失败，请检查Token是否有效');
            // 如果使用OpenAI API失败，尝试使用后端接口
            try {
                const res = await getModels();
                if (res && res.data) {
                    setModelOptions(res.data);
                    if (res.data.length > 0) {
                        setSelectedModel(res.data[0]);
                    }
                }
            } catch (fallbackError) {
                console.error('Fallback model fetch failed:', fallbackError);
            }
        } finally {
            setLoading(false);
        }
    };

    const fetchTokens = async () => {
        try {
            const res = await getTokens(1, 100);
            if (res && res.data && res.data.items) {
                setTokenOptions(res.data.items);
                // 自动选择第一个token
                if (res.data.items.length > 0) {
                    const firstToken = res.data.items[0].key;
                    setSelectedToken(firstToken);
                    // 选择token后会自动触发fetchModels
                } else {
                    message.warning('没有可用的Token，请先添加Token');
                }
            }
        } catch (error) {
            console.error('Failed to fetch tokens:', error);
            message.error('获取Token列表失败');
        }
    };

    // 处理token选择变化
    const handleTokenChange = (value: string) => {
        setSelectedToken(value);
        setSelectedModel(''); // 清空已选择的模型
        setModelOptions([]); // 清空模型列表
    };

    const loadChatHistories = () => {
        const savedHistories = localStorage.getItem('chatHistories');
        if (savedHistories) {
            setChatHistories(JSON.parse(savedHistories));
        }
    };

    const saveChatHistories = (histories: ChatHistory[]) => {
        localStorage.setItem('chatHistories', JSON.stringify(histories));
        setChatHistories(histories);
    };

    // 搜索和筛选历史记录
    const getFilteredHistories = () => {
        return { pinned: pinnedHistories, regular: regularHistories };
    };

    // 置顶/取消置顶对话
    const togglePinHistory = (id: string) => {
        const updatedHistories = chatHistories.map(history => {
            if (history.id === id) {
                return { ...history, pinned: !history.pinned };
            }
            return history;
        });
        saveChatHistories(updatedHistories);
        message.success(updatedHistories.find(h => h.id === id)?.pinned ? '对话已置顶' : '对话已取消置顶');
    };

    // 编辑对话信息
    const openEditHistory = (history: ChatHistory) => {
        setEditHistoryId(history.id);
        setEditHistoryTitle(history.title);
        setEditHistoryTags(history.tags || []);
    };

    const saveEditHistory = () => {
        if (!editHistoryId) return;

        const updatedHistories = chatHistories.map(history => {
            if (history.id === editHistoryId) {
                return {
                    ...history,
                    title: editHistoryTitle,
                    tags: editHistoryTags
                };
            }
            return history;
        });

        saveChatHistories(updatedHistories);
        setEditHistoryId(null);
        message.success('对话信息已更新');
    };

    // 添加标签
    const addHistoryTag = (tag: string) => {
        if (tag && !editHistoryTags.includes(tag)) {
            setEditHistoryTags([...editHistoryTags, tag]);
        }
    };

    // 生成对话标题
    const generateChatTitle = (messages: Message[]): string => {
        const firstUserMessage = messages.find(m => m.role === 'user');
        if (firstUserMessage) {
            const content = firstUserMessage.content.trim();
            // 如果是简短消息，直接使用
            if (content.length <= 20) {
                return content;
            }
            // 否则截取前20个字符
            return content.slice(0, 20) + '...';
        }
        return `对话 ${new Date().toLocaleString('zh-CN')}`;
    };

    // 给消息生成唯一ID
    const generateMessageId = () => {
        return Date.now().toString() + Math.random().toString(36).substring(2, 9);
    };

    // 准备完整的消息历史，排除系统消息
    const getVisibleMessages = () => {
        return messages.filter(msg => msg.role !== 'system');
    };

    // 删除单条消息
    const deleteMessage = (messageId: string) => {
        const newMessages = messages.filter(msg => msg.id !== messageId);
        setMessages(newMessages);
    };

    // 计算文本的大致token数量
    const estimateTokenCount = (text: string): number => {
        // OpenAI的标记化大约是每4个字符1个token
        // 这只是一个粗略估计，实际token数取决于模型的标记化算法
        return Math.ceil(text.length / 4);
    };

    // 重新生成回复
    const regenerateResponse = async (messageId: string) => {
        // 找到当前消息的索引
        const messageIndex = messages.findIndex(msg => msg.id === messageId);
        if (messageIndex === -1 || messages[messageIndex].role !== 'assistant') return;

        // 找到这条消息之前的最后一条用户消息
        let userMessageIndex = messageIndex - 1;
        while (userMessageIndex >= 0 && messages[userMessageIndex].role !== 'user') {
            userMessageIndex--;
        }

        if (userMessageIndex < 0) return;

        setRegeneratingMessageId(messageId);

        try {
            // 保留消息历史直到用户提问
            const historyToKeep = messages.slice(0, userMessageIndex + 1);
            setMessages(historyToKeep);

            // 使用当前域名作为baseURL
            const baseURL = window.location.origin;
            const openai = new OpenAI({
                apiKey: selectedToken,
                baseURL: `${baseURL}/v1`, // API路径
                dangerouslyAllowBrowser: true, // 允许在浏览器中使用
            });

            // 准备消息历史
            const completeMessages = [
                { role: 'system', content: systemPrompt },
                ...historyToKeep.map(m => ({ role: m.role, content: m.content }))
            ];

            setStreaming(true);
            setCurrentAssistantMessage('');

            // 重置性能数据
            const startTime = Date.now();
            setPerformanceData({
                startTime,
                firstTokenTime: null,
                completionTime: null,
                tokenCount: 0,
            });

            // 创建流式响应
            const stream = await openai.chat.completions.create({
                model: selectedModel,
                messages: completeMessages as any,
                stream: true,
                temperature: temperature,
            });

            let fullAssistantResponse = '';
            let hasRecordedFirstToken = false; // 添加标记确保只记录一次

            // 处理流式响应
            for await (const chunk of stream) {
                const content = chunk.choices[0]?.delta?.content || '';
                fullAssistantResponse += content;

                // 记录首个token的时间（只记录一次）
                if (content && !hasRecordedFirstToken) {
                    const firstTokenTime = Date.now() - startTime;
                    setPerformanceData(prev => ({
                        ...prev,
                        firstTokenTime,
                        tokenCount: prev.tokenCount + estimateTokenCount(content)
                    }));
                    hasRecordedFirstToken = true; // 标记已记录
                } else if (content) {
                    // 持续更新token计数
                    setPerformanceData(prev => ({
                        ...prev,
                        tokenCount: prev.tokenCount + estimateTokenCount(content)
                    }));
                }

                setCurrentAssistantMessage(fullAssistantResponse);
            }

            // 记录完成时间
            const completionTime = Date.now() - startTime;
            const tokenCount = estimateTokenCount(fullAssistantResponse);
            const tokensPerSecond = tokenCount / (completionTime / 1000);

            setPerformanceData(prev => ({
                ...prev,
                completionTime,
                tokenCount
            }));

            // 流结束后，添加完整的助手消息，包含性能数据
            const assistantMessage: Message = {
                id: generateMessageId(),
                role: 'assistant',
                content: fullAssistantResponse,
                timestamp: Date.now(),
                performance: {
                    firstTokenTime: performanceData.firstTokenTime || 0,
                    completionTime,
                    totalTokens: tokenCount,
                    tokensPerSecond
                }
            };

            setMessages([...historyToKeep, assistantMessage]);

        } catch (error) {
            console.error('Regenerate response error:', error);
            message.error('重新生成失败，请检查Token和模型设置');
        } finally {
            setRegeneratingMessageId(null);
            setStreaming(false);
            setCurrentAssistantMessage('');
        }
    };

    // 处理用户消息提交
    const handleSubmit = async () => {
        if (!prompt.trim()) {
            message.warning('请输入消息');
            return;
        }

        if (!selectedModel) {
            message.warning('请选择模型');
            return;
        }

        if (!selectedToken) {
            message.warning('请选择Token');
            return;
        }

        // 重置性能数据并记录开始时间
        const startTime = Date.now();
        setPerformanceData({
            startTime,
            firstTokenTime: null,
            completionTime: null,
            tokenCount: 0,
        });

        // 添加用户消息
        const userMessage: Message = {
            id: generateMessageId(),
            role: 'user',
            content: prompt,
            timestamp: Date.now()
        };
        setMessages((prev) => [...prev, userMessage]);
        setPrompt(''); // 清空输入框
        setLoading(true);
        setStreaming(true);
        setCurrentAssistantMessage('');

        try {
            // 使用当前域名作为baseURL
            const baseURL = window.location.origin;
            const openai = new OpenAI({
                apiKey: selectedToken,
                baseURL: `${baseURL}/v1`,
                dangerouslyAllowBrowser: true, // 允许在浏览器中使用
            });

            // 准备完整的消息历史
            const completeMessages = [
                { role: 'system', content: systemPrompt },
                ...messages.map(m => ({ role: m.role, content: m.content })),
                { role: 'user', content: prompt }
            ];

            // 创建流式响应
            const stream = await openai.chat.completions.create({
                model: selectedModel,
                messages: completeMessages as any,
                stream: true,
                temperature: temperature,
            });

            let fullAssistantResponse = '';
            let hasRecordedFirstToken = false; // 添加标记确保只记录一次

            // 处理流式响应
            for await (const chunk of stream) {
                const content = chunk.choices[0]?.delta?.content || '';
                fullAssistantResponse += content;

                // 记录首个token的时间（只记录一次）
                if (content && !hasRecordedFirstToken) {
                    const firstTokenTime = Date.now() - startTime;
                    setPerformanceData(prev => ({
                        ...prev,
                        firstTokenTime,
                        tokenCount: prev.tokenCount + estimateTokenCount(content)
                    }));
                    hasRecordedFirstToken = true; // 标记已记录
                } else if (content) {
                    // 持续更新token计数
                    setPerformanceData(prev => ({
                        ...prev,
                        tokenCount: prev.tokenCount + estimateTokenCount(content)
                    }));
                }

                setCurrentAssistantMessage(fullAssistantResponse);
            }

            // 记录完成时间
            const completionTime = Date.now() - startTime;
            const tokenCount = estimateTokenCount(fullAssistantResponse);
            const tokensPerSecond = tokenCount / (completionTime / 1000);

            setPerformanceData(prev => ({
                ...prev,
                completionTime,
                tokenCount
            }));

            // 流结束后，添加完整的助手消息
            const assistantMessage: Message = {
                id: generateMessageId(),
                role: 'assistant',
                content: fullAssistantResponse,
                timestamp: Date.now(),
                performance: {
                    firstTokenTime: performanceData.firstTokenTime || 0,
                    completionTime,
                    totalTokens: tokenCount,
                    tokensPerSecond
                }
            };
            setMessages((prev) => [...prev, assistantMessage]);

        } catch (error) {
            console.error('Chat completion error:', error);
            message.error('获取响应失败，请检查Token和模型设置');
        } finally {
            setLoading(false);
            setStreaming(false);
            setCurrentAssistantMessage('');
        }
    };

    const clearChat = () => {
        setMessages([]);
        message.success('对话已清空');
    };

    const saveChat = () => {
        if (messages.length === 0) {
            message.warning('没有对话内容可保存');
            return;
        }

        setSaveModalVisible(true);

        // 设置默认标题为第一条消息的前20个字符
        const defaultTitle = generateChatTitle(messages);
        setChatTitle(defaultTitle);
        form.setFieldsValue({
            title: defaultTitle,
            tags: []
        });
    };

    const handleSaveChat = () => {
        form.validateFields().then(values => {
            const newHistory: ChatHistory = {
                id: Date.now().toString(),
                title: values.title,
                messages: [...messages],
                model: selectedModel,
                tokenId: selectedToken,
                createdAt: Date.now(),
                tags: values.tags || []
            };

            const updatedHistories = [newHistory, ...chatHistories];
            saveChatHistories(updatedHistories);
            setSaveModalVisible(false);
            message.success('对话已保存');
        });
    };

    const loadChat = (history: ChatHistory) => {
        setMessages(history.messages);
        setSelectedModel(history.model);
        setSelectedToken(history.tokenId);
        setHistoryVisible(false);
        message.success('对话已加载');
    };

    const deleteHistory = (id: string) => {
        const updatedHistories = chatHistories.filter(h => h.id !== id);
        saveChatHistories(updatedHistories);
        message.success('对话已删除');
    };

    // 复制消息内容
    const copyMessageContent = (content: string) => {
        navigator.clipboard.writeText(content).then(
            () => {
                message.success('内容已复制到剪贴板');
            },
            () => {
                message.error('复制失败');
            }
        );
    };

    // 从当前消息创建新会话
    const createNewChatFromMessage = (messageId: string) => {
        // 找到当前消息
        const targetMessage = messages.find(msg => msg.id === messageId);
        if (!targetMessage) return;

        // 清空现有消息，添加新消息
        setMessages([{
            id: generateMessageId(),
            role: 'user',
            content: targetMessage.content,
            timestamp: Date.now()
        }]);

        // 触发生成响应
        const newUserMessage = {
            role: 'user' as const,
            content: targetMessage.content
        };

        setLoading(true);
        setStreaming(true);
        setCurrentAssistantMessage('');

        // 使用当前域名作为baseURL
        const baseURL = window.location.origin;
        const openai = new OpenAI({
            apiKey: selectedToken,
            baseURL: `${baseURL}/v1`,
            dangerouslyAllowBrowser: true, // 允许在浏览器中使用
        });

        // 准备完整的消息历史
        const completeMessages = [
            { role: 'system', content: systemPrompt },
            newUserMessage
        ];

        // 创建流式响应
        openai.chat.completions.create({
            model: selectedModel,
            messages: completeMessages as any,
            stream: true,
            temperature: temperature,
        }).then(async (stream) => {
            let fullAssistantResponse = '';

            // 处理流式响应
            for await (const chunk of stream) {
                const content = chunk.choices[0]?.delta?.content || '';
                fullAssistantResponse += content;
                setCurrentAssistantMessage(fullAssistantResponse);
            }

            // 流结束后，添加完整的助手消息
            const assistantMessage: Message = {
                id: generateMessageId(),
                role: 'assistant',
                content: fullAssistantResponse,
                timestamp: Date.now()
            };
            setMessages(prev => [...prev, assistantMessage]);

        }).catch(error => {
            console.error('New chat creation error:', error);
            message.error('创建新会话失败');
        }).finally(() => {
            setLoading(false);
            setStreaming(false);
            setCurrentAssistantMessage('');
        });
    };

    // 格式化性能数据
    const formatPerformanceData = (performance?: Message['performance']) => {
        if (!performance) return null;

        return (
            <>
                <span>首token: {performance.firstTokenTime}ms</span>
                <span>完成: {performance.completionTime}ms</span>
                <span>Tokens: ~{performance.totalTokens}</span>
                <span>速率: ~{performance.tokensPerSecond?.toFixed(1) ?? 0} tokens/s</span>
            </>
        );
    };

    // 渲染消息列表
    const renderMessages = () => {
        if (messages.length === 0 && !streaming) {
            return (
                <Flexbox
                    direction="vertical"
                    align="center"
                    justify="center"
                    style={{
                        height: '100%',
                        color: token.colorTextSecondary,
                        padding: isMobile ? '0 8px' : '0 16px'
                    }}
                >
                    <Avatar
                        size={isMobile ? 48 : 64}
                        icon={<IconAvatar Icon={OpenAIIcon} size={isMobile ? 32 : 44} />}
                        style={{
                            background: token.colorBgElevated,
                            marginBottom: 16
                        }}
                    />
                    <Text style={{ fontSize: isMobile ? 14 : 16, fontWeight: 'bold', marginBottom: 8 }}>AI 助手</Text>
                    <Paragraph type="secondary" style={{ textAlign: 'center', maxWidth: 400, fontSize: isMobile ? 12 : 14 }}>
                        选择一个Token和模型，开始与AI助手对话。AI助手可以帮助你回答问题、创作内容等。
                    </Paragraph>
                    <Flexbox direction="horizontal" gap={8} style={{ marginTop: 16 }}>
                        <Button
                            type="primary"
                            onClick={() => setPrompt('写一个短故事关于一个时间旅行者')}
                            style={{
                                border: `1px solid ${token.colorPrimaryBorder}`,
                                fontSize: isMobile ? 12 : 14
                            }}
                            size={isMobile ? "small" : "middle"}
                        >
                            写一个短故事
                        </Button>
                        <Button
                            type="primary"
                            onClick={() => setPrompt('解释量子计算的基本原理')}
                            style={{
                                border: `1px solid ${token.colorPrimaryBorder}`,
                                fontSize: isMobile ? 12 : 14
                            }}
                            size={isMobile ? "small" : "middle"}
                        >
                            解释量子计算
                        </Button>
                    </Flexbox>
                </Flexbox>
            );
        }

        const visibleMessages = getVisibleMessages();

        return (
            <Flexbox direction="vertical" gap={isMobile ? 8 : 16} style={{ width: '100%', padding: isMobile ? 8 : 16 }}>
                {visibleMessages.map((msg, index) => (
                    <Flexbox
                        key={msg.id || index}
                        direction="horizontal"
                        gap={12}
                        style={{
                            width: '100%',
                            alignItems: 'flex-start',
                            position: 'relative'
                        }}
                    >
                        {msg.role === 'user' ? (
                            <Avatar
                                icon={<UserOutlined />}
                                src={user?.avatar}
                                style={{
                                    marginBottom: 16,
                                }}
                            />
                        ) : (
                            <Avatar
                                icon={<IconAvatar Icon={OpenAIIcon} size={20} />}
                                style={{ background: token.colorBgElevated, border: `1px solid ${token.colorBorderSecondary}` }}
                            />
                        )}
                        <div
                            style={{
                                flex: 1,
                                padding: 12,
                                borderRadius: token.borderRadius,
                                backgroundColor: msg.role === 'user' ? token.colorPrimaryBg : token.colorBgElevated,
                                border: `1px solid ${msg.role === 'user' ? token.colorPrimaryBorder : token.colorBorderSecondary}`,
                                color: msg.role === 'user' ? token.colorPrimaryText : token.colorText,
                                position: 'relative'
                            }}
                            onMouseEnter={() => {
                                const messageActions = document.getElementById(`message-actions-${msg.id}`);
                                if (messageActions) {
                                    messageActions.style.display = 'flex';
                                }
                            }}
                            onMouseLeave={() => {
                                const messageActions = document.getElementById(`message-actions-${msg.id}`);
                                if (messageActions) {
                                    messageActions.style.display = 'none';
                                }
                            }}
                        >
                            <ReactMarkdown>{msg.content}</ReactMarkdown>

                            {/* 显示性能数据 */}
                            {msg.role === 'assistant' && msg.performance && (
                                <div style={{
                                    marginTop: 8,
                                    padding: '4px 8px',
                                    borderTop: `1px solid ${token.colorBorderSecondary}`,
                                    fontSize: 12,
                                    color: token.colorTextSecondary,
                                    display: 'flex',
                                    gap: 12,
                                    flexWrap: 'wrap'
                                }}>
                                    {formatPerformanceData(msg.performance)}
                                </div>
                            )}

                            {/* 消息操作按钮 */}
                            <div
                                id={`message-actions-${msg.id}`}
                                style={{
                                    position: 'absolute',
                                    top: 5,
                                    right: 5,
                                    display: 'none',
                                    background: token.colorBgElevated,
                                    borderRadius: token.borderRadius,
                                    padding: '2px 4px',
                                    boxShadow: token.boxShadowSecondary
                                }}
                            >
                                <Space size={4}>
                                    <Tooltip title="复制内容">
                                        <Button
                                            type="text"
                                            size="small"
                                            icon={<CopyOutlined />}
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                copyMessageContent(msg.content);
                                            }}
                                        />
                                    </Tooltip>

                                    {msg.role === 'assistant' && (
                                        <Tooltip title="重新生成">
                                            <Button
                                                type="text"
                                                size="small"
                                                icon={<ReloadOutlined />}
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    regenerateResponse(msg.id!);
                                                }}
                                                loading={regeneratingMessageId === msg.id}
                                            />
                                        </Tooltip>
                                    )}

                                    <Tooltip title="以此开始新对话">
                                        <Button
                                            type="text"
                                            size="small"
                                            icon={<MessageOutlined />}
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                createNewChatFromMessage(msg.id!);
                                            }}
                                        />
                                    </Tooltip>

                                    <Tooltip title="删除消息">
                                        <Popconfirm
                                            title="确定要删除此消息吗？"
                                            onConfirm={(e) => {
                                                e?.stopPropagation();
                                                deleteMessage(msg.id!);
                                            }}
                                            okText="删除"
                                            cancelText="取消"
                                        >
                                            <Button
                                                type="text"
                                                size="small"
                                                danger
                                                icon={<DeleteOutlined />}
                                                onClick={(e) => e.stopPropagation()}
                                            />
                                        </Popconfirm>
                                    </Tooltip>
                                </Space>
                            </div>
                        </div>
                    </Flexbox>
                ))}

                {/* 流式响应显示 */}
                {streaming && (
                    <Flexbox
                        direction="horizontal"
                        gap={12}
                        style={{
                            width: '100%',
                            alignItems: 'flex-start'
                        }}
                    >
                        <Avatar
                            icon={<IconAvatar Icon={OpenAIIcon} size={20} />}
                            style={{ background: token.colorBgElevated, border: `1px solid ${token.colorBorderSecondary}` }}
                        />
                        <div
                            style={{
                                flex: 1,
                                padding: 12,
                                borderRadius: token.borderRadius,
                                backgroundColor: token.colorBgElevated,
                                border: `1px solid ${token.colorBorderSecondary}`,
                                minHeight: 40
                            }}
                        >
                            <ReactMarkdown>{currentAssistantMessage || '...'}</ReactMarkdown>
                            {!currentAssistantMessage ? (
                                <Spin size="small" style={{ marginLeft: 8 }} />
                            ) : (
                                <div style={{
                                    marginTop: 8,
                                    padding: '4px 0',
                                    fontSize: 12,
                                    color: token.colorTextSecondary,
                                    display: 'flex',
                                    gap: 12,
                                    flexWrap: 'wrap'
                                }}>
                                    {performanceData.firstTokenTime && (
                                        <span>首token: {performanceData.firstTokenTime}ms</span>
                                    )}
                                    <span>Tokens: ~{performanceData.tokenCount}</span>
                                    {performanceData.tokenCount > 0 && performanceData.firstTokenTime && (
                                        <span>
                                            速率: ~{(performanceData.tokenCount / ((Date.now() - performanceData.startTime) / 1000)).toFixed(1)} tokens/s
                                        </span>
                                    )}
                                </div>
                            )}
                        </div>
                    </Flexbox>
                )}
            </Flexbox>
        );
    };

    // 渲染历史记录
    const renderChatHistories = () => {
        const { pinned, regular } = getFilteredHistories();
        const hasResults = pinned.length > 0 || regular.length > 0;
        
        return (
            <Flexbox direction="vertical" style={{ height: '100%' }}>
                <Flexbox direction="horizontal" align="center" gap={8} style={{ marginBottom: 16 }}>
                    <Input.Search 
                        placeholder={t('搜索对话标题或标签')}
                        value={searchHistoryText}
                        onChange={(e) => setSearchHistoryText(e.target.value)}
                        style={{ flex: 1 }}
                        allowClear
                    />
                    <Select
                        mode="multiple"
                        placeholder={t('筛选标签')}
                        value={selectedHistoryTags}
                        onChange={setSelectedHistoryTags}
                        style={{ minWidth: 120 }}
                        allowClear
                        maxTagCount={2}
                        suffixIcon={<TagOutlined />}
                    >
                        {historyTags.map(tag => (
                            <Option key={tag} value={tag}>{tag}</Option>
                        ))}
                    </Select>
                    <Select
                        value={historySort}
                        onChange={(value) => setHistorySort(value)}
                        style={{ width: 100 }}
                        suffixIcon={<SortAscendingOutlined />}
                    >
                        <Option value="newest">{t('最新')}</Option>
                        <Option value="oldest">{t('最早')}</Option>
                    </Select>
                </Flexbox>
                
                <div style={{ flex: 1, overflowY: 'auto' }}>
                    {!hasResults ? (
                        <Empty description={t('没有找到匹配的对话')} />
                    ) : (
                        <Flexbox direction="vertical" gap={8}>
                            {pinned.length > 0 && (
                                <>
                                    <Divider orientation="left" style={{ margin: '8px 0' }}>
                                        <Text type="secondary">{t('置顶对话')}</Text>
                                    </Divider>
                                    {pinned.map((history) => (
                                        <ChatHistoryCard 
                                            key={history.id}
                                            history={history}
                                            onLoad={() => loadChat(history)}
                                            onEdit={() => openEditHistory(history)}
                                            onPin={() => togglePinHistory(history.id)}
                                            onDelete={() => deleteHistory(history.id)}
                                        />
                                    ))}
                                </>
                            )}
                            
                            {regular.length > 0 && (
                                <>
                                    <Divider orientation="left" style={{ margin: '8px 0' }}>
                                        <Text type="secondary">{t('所有对话')}</Text>
                                    </Divider>
                                    
                                    {regular.map((history) => (
                                        <ChatHistoryCard 
                                            key={history.id}
                                            history={history}
                                            onLoad={() => loadChat(history)}
                                            onEdit={() => openEditHistory(history)}
                                            onPin={() => togglePinHistory(history.id)}
                                            onDelete={() => deleteHistory(history.id)}
                                        />
                                    ))}
                                </>
                            )}
                        </Flexbox>
                    )}
                </div>
            </Flexbox>
        );
    };

    // 对话历史卡片组件
    const ChatHistoryCard = ({ 
        history, 
        onLoad, 
        onDelete, 
        onEdit, 
        onPin 
    }: { 
        history: ChatHistory, 
        onLoad: () => void, 
        onDelete: () => void,
        onEdit: () => void,
        onPin: () => void
    }) => {
        const [hovering, setHovering] = useState(false);
        
        // 计算消息计数
        const messageCount = history.messages.filter(m => m.role !== 'system').length;
        // 获取最后更新时间
        const lastUpdated = new Date(history.createdAt).toLocaleString();
        
        const cardActions = (
            <Space>
                <Tooltip title={history.pinned ? t('取消置顶') : t('置顶对话')}>
                    <Button
                        type="text"
                        size="small"
                        icon={history.pinned ? <StarFilled /> : <StarOutlined />}
                        onClick={(e) => {
                            e.stopPropagation();
                            onPin();
                        }}
                    />
                </Tooltip>
                <Tooltip title={t('编辑信息')}>
                    <Button
                        type="text"
                        size="small"
                        icon={<EditOutlined />}
                        onClick={(e) => {
                            e.stopPropagation();
                            onEdit();
                        }}
                    />
                </Tooltip>
                <Popconfirm
                    title={t('确定要删除这个对话吗？')}
                    onConfirm={(e) => {
                        e?.stopPropagation();
                        onDelete();
                    }}
                    okText={t('删除')}
                    cancelText={t('取消')}
                >
                    <Tooltip title={t('删除对话')}>
                        <Button
                            type="text"
                            size="small"
                            danger
                            icon={<DeleteOutlined />}
                            onClick={(e) => e.stopPropagation()}
                        />
                    </Tooltip>
                </Popconfirm>
            </Space>
        );
        
        return (
            <Card 
                size="small"
                hoverable
                style={{ 
                    marginBottom: 8, 
                    borderLeft: history.pinned ? `2px solid ${token.colorPrimary}` : undefined,
                    transition: 'all 0.3s ease'
                }}
                onClick={onLoad}
                onMouseEnter={() => setHovering(true)}
                onMouseLeave={() => setHovering(false)}
            >
                <Flexbox direction="horizontal" justify="space-between" align="start">
                    <div style={{ flex: 1, overflow: 'hidden' }}>
                        <Title level={5} style={{ margin: 0, whiteSpace: 'nowrap', textOverflow: 'ellipsis', overflow: 'hidden' }}>
                            {history.title}
                        </Title>
                        <Text type="secondary" style={{ fontSize: 12, display: 'flex', gap: 8, alignItems: 'center' }}>
                            <span>{lastUpdated}</span>
                            <span>•</span>
                            <span>{history.model}</span>
                            {messageCount > 0 && (
                                <>
                                    <span>•</span>
                                    <span style={{ display: 'flex', alignItems: 'center' }}>
                                        <MessageOutlined style={{ fontSize: 12, marginRight: 4 }} />
                                        {messageCount}
                                    </span>
                                </>
                            )}
                        </Text>
                        {history.tags && history.tags.length > 0 && (
                            <div style={{ marginTop: 4 }}>
                                {history.tags.map(tag => (
                                    <Tag key={tag} color="blue" style={{ marginRight: 4 }}>{tag}</Tag>
                                ))}
                            </div>
                        )}
                    </div>
                    {hovering && (
                        <div onClick={(e) => e.stopPropagation()}>
                            {cardActions}
                        </div>
                    )}
                </Flexbox>
            </Card>
        );
    };

    return (
        <Flexbox
            gap={0}
            style={{
                height: '100%',
                position: 'relative',
                overflow: 'hidden'
            }}
        >
            <Card
                title={
                    <Flexbox
                        direction={isMobile ? "vertical" : "horizontal"}
                        align={isMobile ? "start" : "center"}
                        justify="space-between"
                        style={{
                            width: '100%',
                            gap: isMobile ? 8 : 0
                        }}
                    >
                        <span>AI 助手</span>
                        <Flexbox
                            direction="horizontal"
                            gap={8}
                            style={{
                                flexWrap: isMobile ? 'wrap' : 'nowrap'
                            }}
                        >
                            <Button
                                type="text"
                                icon={<SaveOutlined />}
                                onClick={saveChat}
                                disabled={messages.length === 0}
                                size={isMobile ? "small" : "middle"}
                            >
                                {isMobile ? "" : "保存对话"}
                            </Button>
                            <Button
                                type="text"
                                icon={<HistoryOutlined />}
                                onClick={() => setHistoryVisible(true)}
                                size={isMobile ? "small" : "middle"}
                            >
                                {isMobile ? "" : "历史记录"}
                            </Button>
                            <Button
                                type="text"
                                icon={<DeleteOutlined />}
                                onClick={clearChat}
                                disabled={messages.length === 0}
                                size={isMobile ? "small" : "middle"}
                            >
                                {isMobile ? "" : "清空对话"}
                            </Button>
                        </Flexbox>
                    </Flexbox>
                }
                extra={
                    <Flexbox
                        direction={isMobile ? "vertical" : "horizontal"}
                        gap={8}
                        style={{
                            width: isMobile ? '100%' : 'auto'
                        }}
                    >
                        <Select
                            style={{ width: isMobile ? '100%' : 200 }}
                            placeholder="选择Token"
                            value={selectedToken}
                            onChange={handleTokenChange}
                            showSearch
                            optionFilterProp="children"
                            size={isMobile ? "small" : "middle"}
                        >
                            {tokenOptions.map((token) => (
                                <Option key={token.key} value={token.key}>
                                    {token.name || token.key}
                                </Option>
                            ))}
                        </Select>
                        <Select
                            style={{ width: isMobile ? '100%' : 200 }}
                            placeholder="选择模型"
                            value={selectedModel}
                            onChange={(value) => setSelectedModel(value)}
                            showSearch
                            optionFilterProp="children"
                            size={isMobile ? "small" : "middle"}
                            loading={modelOptions.length === 0 && !!selectedToken}
                            disabled={!selectedToken || modelOptions.length === 0}
                        >
                            {modelOptions.map((model) => (
                                <Option key={model} value={model}>
                                    {model}
                                </Option>
                            ))}
                        </Select>
                        <Button
                            icon={<SettingOutlined />}
                            onClick={() => setSettingsVisible(true)}
                            size={isMobile ? "small" : "middle"}
                        />
                    </Flexbox>
                }
                style={{
                    width: '100%',
                    height: '100%',
                    display: 'flex',
                    flexDirection: 'column',
                    border: 'none',
                    borderRadius: 0,
                    overflow: 'hidden'
                }}
                bodyStyle={{
                    flex: '1 1 auto',
                    padding: 0,
                    display: 'flex',
                    flexDirection: 'column',
                    overflow: 'hidden'
                }}
                headStyle={{
                    padding: isMobile ? '8px 12px' : '16px 24px',
                    flexDirection: isMobile ? 'column' : 'row',
                    alignItems: isMobile ? 'flex-start' : 'center'
                }}
            >
                {/* 聊天容器 */}
                <div
                    ref={chatContainerRef}
                    style={{
                        flex: '1 1 auto',
                        overflowY: 'auto',
                        display: 'flex',
                        flexDirection: 'column',
                        padding: isMobile ? '0 8px' : '0 16px',
                    }}
                >
                    {renderMessages()}
                </div>

                <Divider style={{ margin: '8px 0' }} />

                {/* 输入区域 */}
                <Flexbox
                    direction="horizontal"
                    gap={8}
                    style={{
                        padding: isMobile ? '8px 12px' : '16px 24px',
                        background: token.colorBgContainer,
                        borderTop: `1px solid ${token.colorBorderSecondary}`,
                        flexShrink: 0
                    }}
                >
                    <TextArea
                        value={prompt}
                        onChange={(e) => setPrompt(e.target.value)}
                        placeholder="输入消息..."
                        autoSize={{ minRows: 1, maxRows: isMobile ? 3 : 4 }}
                        style={{
                            flex: 1,
                            resize: 'none',
                            borderRadius: token.borderRadiusLG,
                            padding: '8px 12px',
                        }}
                        onPressEnter={(e) => {
                            if (!e.shiftKey) {
                                e.preventDefault();
                                handleSubmit();
                            }
                        }}
                        disabled={loading}
                    />
                    <Button
                        type="primary"
                        icon={<SendOutlined />}
                        onClick={handleSubmit}
                        loading={loading}
                        style={{
                            height: 'auto',
                            borderRadius: token.borderRadiusLG,
                        }}
                        disabled={!prompt.trim() || !selectedModel || !selectedToken}
                    />
                </Flexbox>
            </Card>

            {/* 设置弹窗 */}
            <Modal
                title="对话设置"
                open={settingsVisible}
                onCancel={() => setSettingsVisible(false)}
                onOk={() => setSettingsVisible(false)}
                width={isMobile ? '90%' : 520}
            >
                <Form layout="vertical">
                    <Form.Item label="系统提示词">
                        <TextArea
                            value={systemPrompt}
                            onChange={(e) => setSystemPrompt(e.target.value)}
                            autoSize={{ minRows: 3, maxRows: 6 }}
                            placeholder="设置系统提示词，定义AI助手的角色和行为"
                        />
                    </Form.Item>
                    <Form.Item label={`随机性 (${temperature})`}>
                        <Radio.Group
                            value={temperature}
                            onChange={(e) => setTemperature(e.target.value)}
                            optionType="button"
                            buttonStyle="solid"
                            style={{ width: '100%', display: 'flex', justifyContent: 'space-between' }}
                        >
                            <Radio.Button value={0}>精确</Radio.Button>
                            <Radio.Button value={0.3}>创意低</Radio.Button>
                            <Radio.Button value={0.7}>平衡</Radio.Button>
                            <Radio.Button value={1}>创意高</Radio.Button>
                        </Radio.Group>
                    </Form.Item>
                </Form>
            </Modal>

            {/* 历史记录弹窗 */}
            <Modal
                title={t('对话历史')}
                open={historyVisible}
                onCancel={() => setHistoryVisible(false)}
                footer={null}
                width={isMobile ? '90%' : 600}
                bodyStyle={{ height: 500, padding: 16, overflow: 'hidden' }}
            >
                {renderChatHistories()}
            </Modal>

            {/* 保存对话弹窗 */}
            <Modal
                title={t('保存对话')}
                open={saveModalVisible}
                onCancel={() => setSaveModalVisible(false)}
                onOk={handleSaveChat}
                width={isMobile ? '90%' : 520}
            >
                <Form form={form} layout="vertical">
                    <Form.Item
                        name="title"
                        label={t('对话标题')}
                        rules={[{ required: true, message: t('请输入对话标题') }]}
                    >
                        <Input 
                            placeholder={t('为这个对话命名')}
                            value={chatTitle}
                            onChange={(e) => setChatTitle(e.target.value)}
                        />
                    </Form.Item>
                    <Form.Item
                        name="tags"
                        label={t('添加标签')}
                    >
                        <Select
                            mode="tags"
                            placeholder={t('输入标签名称后按回车键添加')}
                            open={false}
                            style={{ width: '100%' }}
                        />
                    </Form.Item>
                </Form>
            </Modal>

            {/* 编辑对话信息弹窗 */}
            <Modal
                title={t('编辑对话信息')}
                open={!!editHistoryId}
                onCancel={() => setEditHistoryId(null)}
                onOk={saveEditHistory}
                width={isMobile ? '90%' : 520}
            >
                <Flexbox direction="vertical" gap={16}>
                    <div>
                        <Text>{t('对话标题')}</Text>
                        <Input 
                            value={editHistoryTitle}
                            onChange={e => setEditHistoryTitle(e.target.value)}
                            placeholder={t('对话标题')}
                            style={{ marginTop: 8 }}
                        />
                    </div>
                    <div>
                        <Text>{t('标签')}</Text>
                        <Flexbox direction="horizontal" style={{ flexWrap: 'wrap', gap: 8, marginTop: 8 }}>
                            {editHistoryTags.map(tag => (
                                <Tag 
                                    key={tag} 
                                    closable 
                                    onClose={() => setEditHistoryTags(editHistoryTags.filter(t => t !== tag))}
                                >
                                    {tag}
                                </Tag>
                            ))}
                            <Input 
                                placeholder={t('输入标签并按回车')}
                                style={{ width: 150 }}
                                onPressEnter={(e) => {
                                    const target = e.target as HTMLInputElement;
                                    addHistoryTag(target.value);
                                    target.value = '';
                                }}
                            />
                        </Flexbox>
                    </div>
                </Flexbox>
            </Modal>
        </Flexbox>
    );
}

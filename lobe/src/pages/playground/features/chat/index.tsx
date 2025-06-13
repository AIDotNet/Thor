import { Card, Select, Input, Button, Avatar, theme, message, Spin, Modal, Form, Row, Col, Grid, Space, Popconfirm, Tooltip, Typography,  Tag } from 'antd';
import { SendOutlined, UserOutlined, DeleteOutlined, SettingOutlined, SaveOutlined, SyncOutlined, ClearOutlined, RobotOutlined, ThunderboltOutlined } from '@ant-design/icons';
import { Flexbox } from 'react-layout-kit';
import ReactMarkdown from 'react-markdown';
import { useTranslation } from 'react-i18next';
import { isMobileDevice } from '../../../../utils/responsive';
import { getTokens } from '../../../../services/TokenService';
import { getModels } from '../../../../services/ModelService';
import { createCompletion, isResponsesModel, processStreamResponse } from '../../../../services/ResponsesService';
import OpenAI from 'openai';
import { useEffect, useRef, useState } from 'react';

const { Option } = Select;
const { TextArea } = Input;
const { useBreakpoint } = Grid;
const { Text, Title } = Typography;

// Message type definition
export interface Message {
    id?: string;
    role: 'user' | 'assistant' | 'system';
    content: string;
    timestamp?: number;
    performance?: {
        firstTokenTime?: number;
        completionTime?: number;
        totalTokens?: number;
        tokensPerSecond?: number;
    };
}

// Chat history type definition
export interface ChatHistory {
    id: string;
    title: string;
    messages: Message[];
    model: string;
    tokenId: string;
    createdAt: number;
    tags?: string[];
    pinned?: boolean;
}

export default function ChatFeature({ modelInfo }: { modelInfo: any }) {
    const { token } = theme.useToken();
    const { t } = useTranslation();
    const screens = useBreakpoint();
    const isMobile = !screens.md || isMobileDevice();
    const messagesEndRef = useRef<HTMLDivElement>(null);
    
    // State
    const [loading, setLoading] = useState(false);
    const [modelOptions, setModelOptions] = useState<string[]>([]);
    const [tokenOptions, setTokenOptions] = useState<any[]>([]);
    const [selectedModel, setSelectedModel] = useState<string>('');
    const [selectedToken, setSelectedToken] = useState<string>('');
    const [prompt, setPrompt] = useState<string>('');
    const [messages, setMessages] = useState<Message[]>([]);
    const [systemPrompt, setSystemPrompt] = useState<string>(t('playground.systemPrompt') || '你是一个有用的AI助手。');
    const [streaming, setStreaming] = useState(false);
    const [currentAssistantMessage, setCurrentAssistantMessage] = useState('');
    const [settingsVisible, setSettingsVisible] = useState(false);
    const [regeneratingMessageIndex, setRegeneratingMessageIndex] = useState<number | null>(null);
    
    useEffect(() => {
        // Initial loading of tokens and models
        fetchTokens();
    }, [modelInfo]);

    useEffect(() => {
        // Scroll to bottom when messages change
        scrollToBottom();
    }, [messages, currentAssistantMessage]);
    
    const scrollToBottom = () => {
        messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
    };
    
    const fetchTokens = async () => {
        try {
            const res = await getTokens(1, 100);
            if (res && res.data && res.data.items) {
                setTokenOptions(res.data.items);
                if (res.data.items.length > 0) {
                    const firstToken = res.data.items[0];
                    setSelectedToken(firstToken.key);
                    fetchModels(firstToken.key);
                } else {
                    message.warning(t('playground.errorMessages.noTokensAvailable'));
                }
            }
        } catch (error) {
            console.error('Failed to fetch tokens:', error);
            message.error(t('common.error'));
        }
    };
    
    const fetchModels = async (token: string) => {
        try {
            setLoading(true);
            
            if (!token) {
                message.warning(t('playground.errorMessages.selectToken'));
                return;
            }
            
            try {
                console.log('Fetching models using OpenAI SDK with token:', token);
                
                // Initialize OpenAI client with the selected token
                const openai = new OpenAI({
                    apiKey: token,
                    baseURL: window.location.origin + '/v1',
                    dangerouslyAllowBrowser: true // For client-side usage
                });
                
                // Fetch models from OpenAI API
                const response = await openai.models.list();
                
                // Check if response follows the expected structure
                if (response && Array.isArray(response.data)) {
                    // Extract model IDs from the response
                    const availableModels = response.data.map(model => model.id);
                    // Filter models based on modelInfo
                    let filteredModels: string[] = [];
                    if (modelInfo && modelInfo.modelTypes) {
                        // Find the chat type models from modelInfo
                        const chatTypeInfo = modelInfo.modelTypes.find((type: any) => type.type === 'chat');
                        
                        if (chatTypeInfo && chatTypeInfo.models) {
                            // Filter models that exist in both the API response and modelInfo
                            filteredModels = chatTypeInfo.models.filter((model: string) => 
                                availableModels.includes(model)
                            );
                        }
                    }
                    
                    setModelOptions(filteredModels);
                    if (filteredModels.length > 0) {
                        setSelectedModel(filteredModels[0]);
                    }
                } else {
                    message.error(t('playground.errorMessages.responseFailed'));
                    throw new Error('Unexpected OpenAI API response structure');
                }
            } catch (apiError: any) {
                console.error('OpenAI API error:', apiError);
                console.error('Error details:', apiError.message, apiError.status, apiError.headers);
                message.error(`${t('playground.errorMessages.responseFailed')}: ${apiError.message || t('common.unknown')}`);
                
                try {
                    const res = await getModels();
                    if (res && res.data) {
                        // Filter for chat models based on modelInfo
                        let chatModels: string[] = [];
                        
                        if (modelInfo && modelInfo.modelTypes) {
                            const chatTypeInfo = modelInfo.modelTypes.find((type: any) => type.type === 'chat');
                            if (chatTypeInfo && chatTypeInfo.models) {
                                chatModels = res.data.filter((model: string) => 
                                    chatTypeInfo.models.includes(model)
                                );
                            } else {
                                // Fallback to generic filtering if modelInfo structure is not as expected
                                chatModels = res.data.filter((model: string) => 
                                    model.includes('gpt') || model.includes('chat')
                                );
                            }
                        } else {
                            // Fallback to generic filtering if modelInfo is not available
                            chatModels = res.data.filter((model: string) => 
                                model.includes('gpt') || model.includes('chat')
                            );
                        }
                        
                        setModelOptions(chatModels);
                        if (chatModels.length > 0) {
                            setSelectedModel(chatModels[0]);
                        }
                    }
                } catch (fallbackError) {
                    console.error('Fallback getModels error:', fallbackError);
                    message.error(t('common.error'));
                }
            }
        } catch (error) {
            console.error('Failed to fetch models:', error);
            message.error(t('common.error'));
        } finally {
            setLoading(false);
        }
    };
    
    const handleTokenChange = (value: string) => {
        setSelectedToken(value);
        setTimeout(() => fetchModels(value), 0);
    };
    
    const handleSendMessage = async () => {
        if (!prompt.trim()) {
            message.warning(t('playground.errorMessages.enterMessage'));
            return;
        }
        
        if (!selectedModel) {
            message.warning(t('playground.errorMessages.selectModel'));
            return;
        }
        
        if (!selectedToken) {
            message.warning(t('playground.errorMessages.selectToken'));
            return;
        }
        
        // Add user message
        const userMessage: Message = {
            role: 'user',
            content: prompt,
            timestamp: Date.now()
        };
        
        setMessages([...messages, userMessage]);
        setPrompt('');
        
        await generateResponse(userMessage.content);
    };
    
    const generateResponse = async (userPrompt: string, regenerateIndex?: number) => {
        setLoading(true);
        setStreaming(true);
        setCurrentAssistantMessage('');
        
        try {
            if (!selectedToken || !selectedModel) {
                throw new Error(t('playground.errorMessages.selectToken'));
            }
            
            // Prepare messages array including system prompt
            let messageArray = [];
            
            if (regenerateIndex !== undefined) {
                // If regenerating, only include messages up to the regenerated index
                messageArray = [
                    { role: 'system', content: systemPrompt },
                    ...messages.slice(0, regenerateIndex).map(msg => ({ role: msg.role, content: msg.content }))
                ];
            } else {
                // Regular flow - include all messages
                messageArray = [
                    { role: 'system', content: systemPrompt },
                    ...messages.map(msg => ({ role: msg.role, content: msg.content })),
                    { role: 'user', content: userPrompt }
                ];
            }
            
            // Start performance timer
            const startTime = Date.now();
            
            // 判断是否为需要使用responses接口的模型
            const useResponsesAPI = isResponsesModel(selectedModel);
            console.log(`模型 ${selectedModel} 使用${useResponsesAPI ? 'responses' : 'chat completions'}接口`);
            
            // 使用统一的API调用方法
            const stream = await createCompletion({
                model: selectedModel,
                messages: messageArray as any,
                token: selectedToken,
                baseURL: window.location.origin + '/v1',
                stream: true
            });
            
            let firstTokenTime = 0;
            let responseContent = '';
            
            // 处理流式响应
            await processStreamResponse(
                stream,
                (content: string) => {
                    // Record time of first token
                    if (responseContent === '' && content) {
                        firstTokenTime = Date.now();
                    }
                    
                    // Append the chunk content
                    responseContent += content;
                    setCurrentAssistantMessage(responseContent);
                },
                useResponsesAPI
            );
            
            // Completion time
            const completionTime = Date.now();
            
            // Calculate performance metrics
            const performance = {
                firstTokenTime: firstTokenTime - startTime,
                completionTime: completionTime - startTime,
                totalTokens: responseContent.length / 4, // Rough estimate
                tokensPerSecond: (responseContent.length / 4) / ((completionTime - startTime) / 1000)
            };
            
            // Add assistant message with full response
            const assistantMessage: Message = {
                role: 'assistant',
                content: responseContent,
                timestamp: Date.now(),
                performance
            };
            
            // Update messages based on whether we're regenerating or not
            if (regenerateIndex !== undefined) {
                // Replace messages starting from the regenerate index
                setMessages(prev => [...prev.slice(0, regenerateIndex), assistantMessage]);
                setRegeneratingMessageIndex(null);
            } else {
                // Normal flow - append the new message
                setMessages(prev => [...prev, assistantMessage]);
            }
            
        } catch (error: any) {
            console.error('Error in chat completion:', error);
            message.error(`${t('playground.errorMessages.responseFailed')}: ${error.message || t('common.unknown')}`);
            
            // Add error message
            const errorMessage: Message = {
                role: 'assistant',
                content: `${t('common.error')}: ${error.message || t('common.unknown')}`,
                timestamp: Date.now()
            };
            
            // Add error message based on context
            if (regenerateIndex !== undefined) {
                setMessages(prev => [...prev.slice(0, regenerateIndex), errorMessage]);
                setRegeneratingMessageIndex(null);
            } else {
                setMessages(prev => [...prev, errorMessage]);
            }
        } finally {
            setLoading(false);
            setStreaming(false);
            setCurrentAssistantMessage('');
        }
    };
    
    const handleRegenerateMessage = (index: number) => {
        if (index <= 0 || index >= messages.length) return;
        
        // Get user message before the assistant message we want to regenerate
        const userMessageIndex = index - 1;
        const userMessage = messages[userMessageIndex];
        
        if (userMessage.role !== 'user') return;
        
        setRegeneratingMessageIndex(index);
        
        // Regenerate the assistant response to this user message
        generateResponse(userMessage.content, index);
    };
    
    const handleDeleteMessage = (index: number) => {
        // Only allow deleting pairs of messages (user + assistant)
        if (index < 0 || index >= messages.length) return;
        
        // If deleting a user message, also delete the following assistant message
        if (messages[index].role === 'user' && index + 1 < messages.length) {
            setMessages((prev:any) => [...prev.slice(0, index), ...prev.slice(index + 2)]);
        } 
        // If deleting an assistant message, also delete the preceding user message
        else if (messages[index].role === 'assistant' && index > 0) {
            setMessages((prev:any) => [...prev.slice(0, index - 1), ...prev.slice(index + 1)]);
        }
        // If it's a standalone message, just delete it
        else {
            setMessages((prev:any) => [...prev.slice(0, index), ...prev.slice(index + 1)]);
        }
    };
    
    const clearMessages = () => {
        setMessages([]);
        setCurrentAssistantMessage('');
        message.success(t('playground.clearChat'));
    };
    
    const renderEmptyState = () => (
        <div style={{ 
            textAlign: 'center', 
            padding: isMobile ? '40px 20px' : '80px 40px',
            color: token.colorTextSecondary
        }} className="empty-state">
            <RobotOutlined style={{ fontSize: 64, color: token.colorPrimary, marginBottom: 16 }} />
            <Title level={3} style={{ color: token.colorTextSecondary, marginBottom: 8 }}>
                {t('playground.emptyChat.title')}
            </Title>
            <Text style={{ fontSize: 16, marginBottom: 24, display: 'block' }}>
                {t('playground.emptyChat.description')}
            </Text>
            <Space direction={isMobile ? 'vertical' : 'horizontal'} size="middle">
                <Button 
                    type="dashed" 
                    onClick={() => setPrompt(t('playground.emptyChat.suggestion1'))}
                    disabled={!selectedModel || !selectedToken}
                >
                    {t('playground.emptyChat.suggestion1')}
                </Button>
                <Button 
                    type="dashed" 
                    onClick={() => setPrompt(t('playground.emptyChat.suggestion2'))}
                    disabled={!selectedModel || !selectedToken}
                >
                    {t('playground.emptyChat.suggestion2')}
                </Button>
            </Space>
        </div>
    );
    
    const renderMessages = () => {
        if (messages.length === 0) {
            return renderEmptyState();
        }
        
        return (
            <div className="message-list" style={{ padding: isMobile ? '8px' : '16px' }}>
                {messages.map((msg:any, index:any) => (
                    <div 
                        key={msg.id || index} 
                        className={`message ${msg.role}`}
                        style={{
                            display: 'flex',
                            margin: '16px 0',
                            padding: isMobile ? '12px' : '16px 20px',
                            borderRadius: token.borderRadiusLG,
                            backgroundColor: msg.role === 'user' 
                                ? token.colorBgElevated 
                                : token.colorBgContainer,
                            border: `1px solid ${token.colorBorderSecondary}`,
                            position: 'relative',
                            transition: 'all 0.2s ease',
                        }}
                    >
                        <Avatar 
                            icon={msg.role === 'user' ? <UserOutlined /> : <RobotOutlined />}
                            style={{ 
                                marginRight: 12,
                                backgroundColor: msg.role === 'user' ? token.colorPrimary : token.colorSuccess,
                                flexShrink: 0
                            }}
                            size={isMobile ? 'default' : 'large'}
                        />
                        <div style={{ flex: 1, minWidth: 0 }}>
                            <div style={{ 
                                fontWeight: 600, 
                                marginBottom: 8,
                                display: 'flex',
                                justifyContent: 'space-between',
                                alignItems: 'center',
                                color: token.colorText
                            }}>
                                <span style={{ fontSize: isMobile ? 14 : 16 }}>
                                    {msg.role === 'user' ? t('common.you') : t('playground.title')}
                                </span>
                                
                                {/* Message action buttons */}
                                <Space size="small">
                                    {msg.role === 'assistant' && index > 0 && (
                                        <Tooltip title={t('playground.regenerate')}>
                                            <Button 
                                                type="text" 
                                                size="small"
                                                icon={<SyncOutlined />}
                                                loading={regeneratingMessageIndex === index}
                                                onClick={() => handleRegenerateMessage(index)}
                                                disabled={loading}
                                                style={{ 
                                                    color: token.colorTextSecondary,
                                                    opacity: 0.7
                                                }}
                                            />
                                        </Tooltip>
                                    )}
                                    <Tooltip title={t('playground.delete')}>
                                        <Popconfirm
                                            title={t('playground.confirmDelete')}
                                            onConfirm={() => handleDeleteMessage(index)}
                                            okText={t('common.confirm')}
                                            cancelText={t('common.cancel')}
                                        >
                                            <Button 
                                                type="text" 
                                                size="small"
                                                danger
                                                icon={<DeleteOutlined />}
                                                disabled={loading}
                                                style={{ opacity: 0.7 }}
                                            />
                                        </Popconfirm>
                                    </Tooltip>
                                </Space>
                            </div>
                            <div className="message-content" style={{ 
                                lineHeight: 1.6,
                                fontSize: isMobile ? 14 : 16,
                                color: token.colorText
                            }}>
                                {msg.role === 'assistant' ? (
                                    <ReactMarkdown>
                                        {msg.content}
                                    </ReactMarkdown>
                                ) : (
                                    <div style={{ whiteSpace: 'pre-wrap' }}>{msg.content}</div>
                                )}
                            </div>
                            {msg.role === 'assistant' && msg.performance && (
                                <div className="performance-tags" style={{ 
                                    fontSize: 12, 
                                    color: token.colorTextTertiary,
                                    marginTop: 12,
                                    display: 'flex',
                                    flexWrap: 'wrap',
                                    gap: '8px'
                                }}>
                                    <Tag icon={<ThunderboltOutlined />} color="blue">
                                        {t('playground.performance.firstToken')}: {msg.performance.firstTokenTime}ms
                                    </Tag>
                                    <Tag color="green">
                                        {t('playground.performance.completion')}: {msg.performance.completionTime}ms
                                    </Tag>
                                    <Tag color="orange">
                                        ~{Math.round(msg.performance.tokensPerSecond || 0)} {t('playground.performance.tokens')}/s
                                    </Tag>
                                </div>
                            )}
                        </div>
                    </div>
                ))}
                
                {streaming && currentAssistantMessage && (
                    <div 
                        className="message assistant streaming"
                        style={{
                            display: 'flex',
                            margin: '16px 0',
                            padding: isMobile ? '12px' : '16px 20px',
                            borderRadius: token.borderRadiusLG,
                            backgroundColor: token.colorBgContainer,
                            border: `1px solid ${token.colorPrimary}`,
                            boxShadow: `0 0 0 1px ${token.colorPrimary}20`,
                        }}
                    >
                        <Avatar 
                            icon={<RobotOutlined />}
                            style={{ 
                                marginRight: 12,
                                backgroundColor: token.colorSuccess,
                                flexShrink: 0
                            }}
                            size={isMobile ? 'default' : 'large'}
                        />
                        <div style={{ flex: 1, minWidth: 0 }}>
                            <div style={{ 
                                fontWeight: 600, 
                                marginBottom: 8,
                                display: 'flex',
                                alignItems: 'center',
                                color: token.colorText
                            }}>
                                <span style={{ fontSize: isMobile ? 14 : 16 }}>
                                    {t('playground.title')}
                                </span>
                                <Spin size="small" style={{ marginLeft: 8 }} />
                            </div>
                            <div className="message-content" style={{ 
                                lineHeight: 1.6,
                                fontSize: isMobile ? 14 : 16,
                                color: token.colorText
                            }}>
                                <ReactMarkdown>
                                    {currentAssistantMessage}
                                </ReactMarkdown>
                            </div>
                        </div>
                    </div>
                )}
                
                {loading && !streaming && (
                    <div style={{ 
                        textAlign: 'center', 
                        padding: '32px 0',
                        color: token.colorTextSecondary
                    }}>
                        <Spin size="large" />
                        <div style={{ marginTop: 16, fontSize: 16 }} className="loading-dots">
                            {t('common.loading')}
                        </div>
                    </div>
                )}
                
                <div ref={messagesEndRef} />
            </div>
        );
    };
    
    const renderSettingsModal = () => (
        <Modal
            title={
                <div style={{ display: 'flex', alignItems: 'center' }}>
                    <SettingOutlined style={{ marginRight: 8 }} />
                    {t('playground.chatSettings')}
                </div>
            }
            open={settingsVisible}
            onCancel={() => setSettingsVisible(false)}
            footer={[
                <Button key="cancel" onClick={() => setSettingsVisible(false)}>
                    {t('common.cancel')}
                </Button>,
                <Button 
                    key="save" 
                    type="primary" 
                    onClick={() => {
                        setSettingsVisible(false);
                        message.success(t('common.saveSuccess'));
                    }}
                >
                    {t('common.save')}
                </Button>
            ]}
            width={isMobile ? '90%' : 600}
        >
            <Form layout="vertical">
                <Form.Item 
                    label={t('playground.systemPrompt')}
                    tooltip={t('playground.systemPrompt')}
                >
                    <TextArea
                        rows={6}
                        value={systemPrompt}
                        onChange={e => setSystemPrompt(e.target.value)}
                        placeholder={t('playground.systemPrompt')}
                        showCount
                        maxLength={2000}
                    />
                </Form.Item>
            </Form>
        </Modal>
    );
    
    return (
        <Flexbox 
            gap={0}
            style={{
                height: '100%',
                width: '100%',
                position: 'relative',
                overflow: 'hidden',
                backgroundColor: token.colorBgLayout
            }}
        >
            <Card
                style={{
                    width: '100%',
                    height: '100%',
                    display: 'flex',
                    flexDirection: 'column',
                    border: 'none',
                    borderRadius: 0,
                    backgroundColor: token.colorBgContainer
                }}
                bodyStyle={{
                    flex: '1',
                    padding: 0,
                    display: 'flex',
                    flexDirection: 'column',
                    overflow: 'hidden'
                }}
            >
                {/* Header with model selection */}
                <div style={{ 
                    padding: isMobile ? '12px 16px' : '20px 24px',
                    borderBottom: `1px solid ${token.colorBorderSecondary}`,
                    backgroundColor: token.colorBgElevated,
                    boxShadow: token.boxShadowTertiary
                }}>
                    <Row gutter={[16, 16]} align="middle">
                        <Col xs={24} sm={12} md={8}>
                            <div style={{ marginBottom: isMobile ? 4 : 0 }}>
                                <Text strong style={{ fontSize: 12, color: token.colorTextSecondary }}>
                                    {t('playground.selectToken')}
                                </Text>
                            </div>
                            <Select
                                style={{ width: '100%' }}
                                placeholder={t('playground.selectToken')}
                                value={selectedToken}
                                onChange={handleTokenChange}
                                loading={loading}
                                size={isMobile ? 'middle' : 'large'}
                            >
                                {tokenOptions.map((token:any) => (
                                    <Option key={token.key} value={token.key}>
                                        {token.name || token.key}
                                    </Option>
                                ))}
                            </Select>
                        </Col>
                        <Col xs={24} md={12}>
                            <Space direction="vertical" style={{ width: '100%' }} size="small">
                                <Text type="secondary">{t('playground.selectModel')}</Text>
                                <Space wrap>
                                    <Select
                                        style={{ minWidth: 200, flex: 1 }}
                                        placeholder={t('playground.selectModel')}
                                        value={selectedModel}
                                        onChange={setSelectedModel}
                                        loading={loading}
                                        disabled={!selectedToken}
                                    >
                                        {modelOptions.map(model => (
                                            <Option key={model} value={model}>
                                                {model}
                                            </Option>
                                        ))}
                                    </Select>
                                    {selectedModel && (
                                        <Tag color={isResponsesModel(selectedModel) ? 'blue' : 'green'}>
                                            {isResponsesModel(selectedModel) ? 'Responses API' : 'Chat Completions API'}
                                        </Tag>
                                    )}
                                </Space>
                            </Space>
                        </Col>
                        <Col xs={24} md={8} style={{ textAlign: isMobile ? 'center' : 'right' }}>
                            <Space size="middle">
                                <Tooltip title={t('playground.chatSettings')}>
                                    <Button 
                                        icon={<SettingOutlined />} 
                                        onClick={() => setSettingsVisible(true)}
                                        size={isMobile ? 'middle' : 'large'}
                                    />
                                </Tooltip>
                                <Tooltip title={t('playground.clearChat')}>
                                    <Popconfirm
                                        title={t('playground.clearChat')}
                                        description={t('playground.confirmDeleteChat')}
                                        onConfirm={clearMessages}
                                        okText={t('common.confirm')}
                                        cancelText={t('common.cancel')}
                                    >
                                        <Button 
                                            icon={<ClearOutlined />} 
                                            disabled={messages.length === 0}
                                            size={isMobile ? 'middle' : 'large'}
                                        />
                                    </Popconfirm>
                                </Tooltip>
                                <Tooltip title={t('playground.saveChat')}>
                                    <Button 
                                        icon={<SaveOutlined />} 
                                        disabled={messages.length === 0}
                                        size={isMobile ? 'middle' : 'large'}
                                    />
                                </Tooltip>
                            </Space>
                        </Col>
                    </Row>
                </div>
                
                {/* Message area */}
                <div style={{ 
                    flex: 1, 
                    overflow: 'auto',
                    backgroundColor: token.colorBgLayout
                }}>
                    {renderMessages()}
                </div>
                
                {/* Input area */}
                <div style={{ 
                    padding: isMobile ? '16px' : '20px 24px',
                    borderTop: `1px solid ${token.colorBorderSecondary}`,
                    backgroundColor: token.colorBgElevated,
                    boxShadow: token.boxShadowTertiary
                }}>
                    <div style={{ display: 'flex', gap: 12, alignItems: 'flex-end' }}>
                        <div style={{ flex: 1 }}>
                            <TextArea
                                value={prompt}
                                onChange={e => setPrompt(e.target.value)}
                                placeholder={t('playground.inputMessage')}
                                autoSize={{ minRows: 1, maxRows: 6 }}
                                onKeyDown={e => {
                                    if (e.key === 'Enter' && !e.shiftKey) {
                                        e.preventDefault();
                                        handleSendMessage();
                                    }
                                }}
                                style={{ 
                                    fontSize: isMobile ? 14 : 16,
                                    borderRadius: token.borderRadiusLG
                                }}
                                size="large"
                            />
                        </div>
                        <Button
                            type="primary"
                            icon={<SendOutlined />}
                            onClick={handleSendMessage}
                            disabled={!prompt.trim() || !selectedModel || !selectedToken || loading}
                            loading={loading}
                            size="large"
                            style={{ 
                                height: 'auto',
                                minHeight: 40,
                                borderRadius: token.borderRadiusLG
                            }}
                        >
                            {!isMobile && t('playground.send')}
                        </Button>
                    </div>
                    <div style={{ 
                        marginTop: 8, 
                        textAlign: 'center',
                        fontSize: 12,
                        color: token.colorTextTertiary
                    }}>
                        {t('playground.enterToSend')}
                    </div>
                </div>
            </Card>
            
            {renderSettingsModal()}
        </Flexbox>
    );
} 
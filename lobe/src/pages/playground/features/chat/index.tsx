import { useEffect, useState, useRef } from 'react';
import { Card, Select, Input, Button, Avatar, theme, message, Spin, Modal, Form, Row, Col, Empty, Grid, Space, Popconfirm, Tooltip } from 'antd';
import { SendOutlined, UserOutlined, DeleteOutlined, SettingOutlined, SaveOutlined, SyncOutlined } from '@ant-design/icons';
import { Flexbox } from 'react-layout-kit';
import { IconAvatar } from '@lobehub/icons';
import ReactMarkdown from 'react-markdown';
import { isMobileDevice } from '../../../../utils/responsive';
import { getTokens } from '../../../../services/TokenService';
import { getModels } from '../../../../services/ModelService';
import OpenAI from 'openai';

const { Option } = Select;
const { TextArea } = Input;
const { useBreakpoint } = Grid;

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
    const [systemPrompt, setSystemPrompt] = useState<string>('你是一个有用的AI助手。');
    const [streaming, setStreaming] = useState(false);
    const [currentAssistantMessage, setCurrentAssistantMessage] = useState('');
    const [settingsVisible, setSettingsVisible] = useState(false);
    const [regeneratingMessageIndex, setRegeneratingMessageIndex] = useState<number | null>(null);
    
    useEffect(() => {
        // Initial loading of tokens and models
        fetchTokens();
    }, []);

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
                    message.warning('没有可用的Token，请先添加Token');
                }
            }
        } catch (error) {
            console.error('Failed to fetch tokens:', error);
            message.error('获取Token列表失败');
        }
    };
    
    const fetchModels = async (token: string) => {
        try {
            setLoading(true);
            
            if (!token) {
                message.warning('请先选择Token');
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
                    } else {
                        message.info('没有可用的聊天模型');
                    }
                } else {
                    message.error('OpenAI API返回了意外的响应结构');
                    throw new Error('Unexpected OpenAI API response structure');
                }
            } catch (apiError: any) {
                console.error('OpenAI API error:', apiError);
                console.error('Error details:', apiError.message, apiError.status, apiError.headers);
                message.error(`获取OpenAI模型列表失败: ${apiError.message || '未知错误'}`);
                
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
                    message.error('获取模型列表失败，请稍后再试');
                }
            }
        } catch (error) {
            console.error('Failed to fetch models:', error);
            message.error('获取模型列表失败');
        } finally {
            setLoading(false);
        }
    };
    
    const handleTokenChange = (value: string) => {
        setSelectedToken(value);
        setTimeout(() => fetchModels(value), 0);
    };
    
    const handleSendMessage = async () => {
        if (!prompt.trim()) return;
        
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
                throw new Error('请先选择Token和模型');
            }
            
            // Initialize OpenAI client
            const openai = new OpenAI({
                apiKey: selectedToken,
                dangerouslyAllowBrowser: true,
                baseURL: window.location.origin + '/v1'
            });
            
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
            
            // Create streaming completion
            const stream = await openai.chat.completions.create({
                model: selectedModel,
                messages: messageArray as any,
                stream: true
            });
            
            let firstTokenTime = 0;
            let responseContent = '';
            
            // Process the stream
            for await (const chunk of stream) {
                // Record time of first token
                if (responseContent === '' && chunk.choices[0]?.delta?.content) {
                    firstTokenTime = Date.now();
                }
                
                // Append the chunk content if available
                if (chunk.choices[0]?.delta?.content) {
                    responseContent += chunk.choices[0].delta.content;
                    setCurrentAssistantMessage(responseContent);
                }
            }
            
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
            message.error(`聊天失败: ${error.message || '未知错误'}`);
            
            // Add error message
            const errorMessage: Message = {
                role: 'assistant',
                content: `错误: ${error.message || '未知错误'}`,
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
            setMessages(prev => [...prev.slice(0, index), ...prev.slice(index + 2)]);
        } 
        // If deleting an assistant message, also delete the preceding user message
        else if (messages[index].role === 'assistant' && index > 0) {
            setMessages(prev => [...prev.slice(0, index - 1), ...prev.slice(index + 1)]);
        }
        // If it's a standalone message, just delete it
        else {
            setMessages(prev => [...prev.slice(0, index), ...prev.slice(index + 1)]);
        }
    };
    
    const clearMessages = () => {
        setMessages([]);
        setCurrentAssistantMessage('');
    };
    
    const renderMessages = () => {
        if (messages.length === 0) {
            return (
                <Empty 
                    image={Empty.PRESENTED_IMAGE_SIMPLE} 
                    description="No messages yet"
                    style={{ margin: '100px 0' }}
                />
            );
        }
        
        return (
            <div className="message-list">
                {messages.map((msg, index) => (
                    <div 
                        key={msg.id || index} 
                        className={`message ${msg.role}`}
                        style={{
                            display: 'flex',
                            margin: '16px 0',
                            padding: '12px 16px',
                            borderRadius: token.borderRadiusLG,
                            backgroundColor: msg.role === 'user' ? token.colorBgElevated : token.colorBgContainer,
                            position: 'relative', // For absolute positioned buttons
                        }}
                    >
                        <Avatar 
                            icon={msg.role === 'user' ? <UserOutlined /> : <IconAvatar size={18} />}
                            style={{ 
                                marginRight: 12,
                                backgroundColor: msg.role === 'user' ? token.colorPrimary : '#10a37f'
                            }}
                        />
                        <div style={{ flex: 1 }}>
                            <div style={{ 
                                fontWeight: 500, 
                                marginBottom: 4,
                                display: 'flex',
                                justifyContent: 'space-between',
                                alignItems: 'center'
                            }}>
                                <span>{msg.role === 'user' ? 'You' : 'Assistant'}</span>
                                
                                {/* Message action buttons */}
                                <Space size="small">
                                    {msg.role === 'assistant' && index > 0 && (
                                        <Tooltip title="Regenerate response">
                                            <Button 
                                                type="text" 
                                                size="small"
                                                icon={<SyncOutlined />}
                                                loading={regeneratingMessageIndex === index}
                                                onClick={() => handleRegenerateMessage(index)}
                                                disabled={loading}
                                            />
                                        </Tooltip>
                                    )}
                                    <Tooltip title="Delete message">
                                        <Button 
                                            type="text" 
                                            size="small"
                                            danger
                                            icon={<DeleteOutlined />}
                                            onClick={() => handleDeleteMessage(index)}
                                            disabled={loading}
                                        />
                                    </Tooltip>
                                </Space>
                            </div>
                            <div className="message-content">
                                {msg.role === 'assistant' ? (
                                    <ReactMarkdown>
                                        {msg.content}
                                    </ReactMarkdown>
                                ) : (
                                    <div>{msg.content}</div>
                                )}
                            </div>
                            {msg.role === 'assistant' && msg.performance && (
                                <div style={{ 
                                    fontSize: '12px', 
                                    color: token.colorTextSecondary,
                                    marginTop: '8px'
                                }}>
                                    First token: {msg.performance.firstTokenTime}ms | 
                                    Total time: {msg.performance.completionTime}ms | 
                                    ~{Math.round(msg.performance.tokensPerSecond || 0)} tokens/sec
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
                            padding: '12px 16px',
                            borderRadius: token.borderRadiusLG,
                            backgroundColor: token.colorBgContainer,
                        }}
                    >
                        <Avatar 
                            icon={<IconAvatar size={18} />}
                            style={{ 
                                marginRight: 12,
                                backgroundColor: '#10a37f'
                            }}
                        />
                        <div style={{ flex: 1 }}>
                            <div style={{ fontWeight: 500, marginBottom: 4 }}>
                                Assistant
                            </div>
                            <div className="message-content">
                                <ReactMarkdown>
                                    {currentAssistantMessage}
                                </ReactMarkdown>
                            </div>
                        </div>
                    </div>
                )}
                
                {loading && !streaming && (
                    <div style={{ textAlign: 'center', padding: '20px 0' }}>
                        <Spin size="small" />
                        <div style={{ marginTop: 8, color: token.colorTextSecondary }}>
                            Thinking...
                        </div>
                    </div>
                )}
                
                <div ref={messagesEndRef} />
            </div>
        );
    };
    
    const renderSettingsModal = () => (
        <Modal
            title="Chat Settings"
            open={settingsVisible}
            onCancel={() => setSettingsVisible(false)}
            footer={[
                <Button key="cancel" onClick={() => setSettingsVisible(false)}>
                    Cancel
                </Button>,
                <Button key="save" type="primary" onClick={() => setSettingsVisible(false)}>
                    Save
                </Button>
            ]}
        >
            <Form layout="vertical">
                <Form.Item label="System Prompt">
                    <TextArea
                        rows={4}
                        value={systemPrompt}
                        onChange={e => setSystemPrompt(e.target.value)}
                        placeholder="Instructions for the AI assistant..."
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
                overflow: 'hidden'
            }}
        >
            <Card
                style={{
                    width: '100%',
                    height: '100%',
                    display: 'flex',
                    flexDirection: 'column',
                    border: 'none',
                    borderRadius: 0
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
                    padding: isMobile ? '8px 12px' : '16px 24px',
                    borderBottom: `1px solid ${token.colorBorderSecondary}`,
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center'
                }}>
                    <Row gutter={16} style={{ flex: 1 }}>
                        <Col span={isMobile ? 12 : 8}>
                            <Select
                                style={{ width: '100%' }}
                                placeholder="Select a token"
                                value={selectedToken}
                                onChange={handleTokenChange}
                                loading={loading}
                            >
                                {tokenOptions.map(token => (
                                    <Option key={token.key} value={token.key}>
                                        {token.name || token.key}
                                    </Option>
                                ))}
                            </Select>
                        </Col>
                        <Col span={isMobile ? 12 : 8}>
                            <Select
                                style={{ width: '100%' }}
                                placeholder="Select a model"
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
                        </Col>
                        {!isMobile && (
                            <Col span={8} style={{ textAlign: 'right' }}>
                                <Space>
                                    <Tooltip title="Settings">
                                        <Button 
                                            icon={<SettingOutlined />} 
                                            onClick={() => setSettingsVisible(true)}
                                        />
                                    </Tooltip>
                                    <Tooltip title="Clear messages">
                                        <Popconfirm
                                            title="Clear all messages?"
                                            onConfirm={clearMessages}
                                            okText="Yes"
                                            cancelText="No"
                                        >
                                            <Button 
                                                icon={<DeleteOutlined />} 
                                                disabled={messages.length === 0}
                                            />
                                        </Popconfirm>
                                    </Tooltip>
                                    <Tooltip title="Save chat">
                                        <Button 
                                            icon={<SaveOutlined />} 
                                            disabled={messages.length === 0}
                                        />
                                    </Tooltip>
                                </Space>
                            </Col>
                        )}
                    </Row>
                </div>
                
                {/* Message area */}
                <div style={{ 
                    flex: 1, 
                    overflow: 'auto',
                    padding: isMobile ? '8px 12px' : '16px 24px'
                }}>
                    {renderMessages()}
                </div>
                
                {/* Input area */}
                <div style={{ 
                    padding: isMobile ? '8px 12px 12px' : '16px 24px 24px',
                    borderTop: `1px solid ${token.colorBorderSecondary}`,
                    backgroundColor: token.colorBgContainer
                }}>
                    <Row gutter={[16, 16]}>
                        {isMobile && (
                            <Col span={24} style={{ display: 'flex', justifyContent: 'center' }}>
                                <Space>
                                    <Button 
                                        icon={<SettingOutlined />} 
                                        onClick={() => setSettingsVisible(true)}
                                    />
                                    <Popconfirm
                                        title="Clear all messages?"
                                        onConfirm={clearMessages}
                                        okText="Yes"
                                        cancelText="No"
                                    >
                                        <Button 
                                            icon={<DeleteOutlined />} 
                                            disabled={messages.length === 0}
                                        />
                                    </Popconfirm>
                                    <Button 
                                        icon={<SaveOutlined />} 
                                        disabled={messages.length === 0}
                                    />
                                </Space>
                            </Col>
                        )}
                        <Col span={24}>
                            <div style={{ display: 'flex' }}>
                                <TextArea
                                    value={prompt}
                                    onChange={e => setPrompt(e.target.value)}
                                    placeholder="Type a message..."
                                    autoSize={{ minRows: 1, maxRows: 6 }}
                                    style={{ flex: 1 }}
                                    onKeyDown={e => {
                                        if (e.key === 'Enter' && !e.shiftKey) {
                                            e.preventDefault();
                                            handleSendMessage();
                                        }
                                    }}
                                />
                                <Button
                                    type="primary"
                                    icon={<SendOutlined />}
                                    style={{ marginLeft: 8 }}
                                    onClick={handleSendMessage}
                                    disabled={!prompt.trim() || !selectedModel || !selectedToken}
                                    loading={loading}
                                />
                            </div>
                            <div style={{ 
                                marginTop: 8, 
                                textAlign: 'center',
                                fontSize: 12,
                                color: token.colorTextSecondary
                            }}>
                                Press Enter to send, Shift+Enter for new line
                            </div>
                        </Col>
                    </Row>
                </div>
            </Card>
            
            {renderSettingsModal()}
        </Flexbox>
    );
} 
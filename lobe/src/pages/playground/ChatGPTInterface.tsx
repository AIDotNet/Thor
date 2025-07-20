import React, { useState, useEffect, useRef, useCallback } from 'react';
import { theme, Select, Input, Button, Avatar, message, Spin, Modal, Form, Row, Col, Grid, Space, Popconfirm, Tooltip, Typography, Tag, Card, Divider, Dropdown, MenuProps } from 'antd';
import { 
  SendOutlined, 
  UserOutlined, 
  DeleteOutlined, 
  SettingOutlined, 
  SaveOutlined, 
  SyncOutlined, 
  ClearOutlined, 
  RobotOutlined, 
  ThunderboltOutlined,
  CopyOutlined,
  DownloadOutlined,
  ShareAltOutlined,
  BookOutlined,
  HistoryOutlined,
  PlusOutlined,
  MenuOutlined,
  CloseOutlined,
  EditOutlined,
  CheckOutlined,
  StopOutlined
} from '@ant-design/icons';
import { Flexbox } from 'react-layout-kit';
import ReactMarkdown from 'react-markdown';
import { useTranslation } from 'react-i18next';
import { isMobileDevice } from '../../utils/responsive';
import { getTokens } from '../../services/TokenService';
import { getModels } from '../../services/ModelService';
import { createCompletion, isResponsesModel, processStreamResponse } from '../../services/ResponsesService';
import OpenAI from 'openai';
import styled from 'styled-components';

const { Option } = Select;
const { TextArea } = Input;
const { useBreakpoint } = Grid;
const { Text, Title } = Typography;

// ChatGPT-inspired styled components
const ChatGPTContainer = styled.div<{ $token: any }>`
  height: 100vh;
  display: flex;
  background: ${props => props.$token.colorBgLayout};
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  
  @media (max-width: 768px) {
    flex-direction: column;
  }
`;

const Sidebar = styled.div<{ $token: any; $collapsed?: boolean }>`
  width: ${props => props.$collapsed ? '0' : '260px'};
  background: ${props => props.$token.colorBgContainer};
  border-right: 1px solid ${props => props.$token.colorBorderSecondary};
  display: flex;
  flex-direction: column;
  transition: width 0.3s ease;
  overflow: hidden;
  
  @media (max-width: 768px) {
    position: absolute;
    top: 0;
    left: 0;
    height: 100%;
    z-index: 1000;
    width: ${props => props.$collapsed ? '0' : '100%'};
  }
`;

const SidebarHeader = styled.div<{ $token: any }>`
  padding: 16px;
  border-bottom: 1px solid ${props => props.$token.colorBorderSecondary};
  display: flex;
  align-items: center;
  justify-content: space-between;
`;

const SidebarContent = styled.div`
  flex: 1;
  overflow-y: auto;
  padding: 8px;
`;

const ChatArea = styled.div`
  flex: 1;
  display: flex;
  flex-direction: column;
  position: relative;
`;

const ChatHeader = styled.div<{ $token: any }>`
  padding: 16px 24px;
  border-bottom: 1px solid ${props => props.$token.colorBorderSecondary};
  background: ${props => props.$token.colorBgContainer};
  display: flex;
  align-items: center;
  justify-content: space-between;
  
  @media (max-width: 768px) {
    padding: 12px 16px;
  }
`;

const MessagesContainer = styled.div<{ $token: any }>`
  flex: 1;
  overflow-y: auto;
  padding: 24px;
  background: ${props => props.$token.colorBgLayout};
  
  @media (max-width: 768px) {
    padding: 16px;
  }
  
  &::-webkit-scrollbar {
    width: 6px;
  }
  
  &::-webkit-scrollbar-track {
    background: transparent;
  }
  
  &::-webkit-scrollbar-thumb {
    background: ${props => props.$token.colorFillTertiary};
    border-radius: 3px;
  }
`;

const MessageBubble = styled.div<{ $isUser?: boolean; $token: any }>`
  display: flex;
  margin-bottom: 24px;
  align-items: flex-start;
  gap: 12px;
  
  .message-content {
    max-width: 70%;
    padding: 16px 20px;
    border-radius: ${props => props.$isUser ? '20px 20px 4px 20px' : '20px 20px 20px 4px'};
    background: ${props => props.$isUser ? props.$token.colorPrimary : props.$token.colorBgContainer};
    color: ${props => props.$isUser ? '#fff' : props.$token.colorText};
    box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
    line-height: 1.6;
    
    @media (max-width: 768px) {
      max-width: 85%;
      padding: 12px 16px;
    }
  }
  
  .message-actions {
    opacity: 0;
    transition: opacity 0.2s ease;
    margin-top: 8px;
  }
  
  &:hover .message-actions {
    opacity: 1;
  }
`;

const InputContainer = styled.div<{ $token: any }>`
  padding: 24px;
  border-top: 1px solid ${props => props.$token.colorBorderSecondary};
  background: ${props => props.$token.colorBgContainer};
  
  @media (max-width: 768px) {
    padding: 16px;
  }
`;

const InputWrapper = styled.div<{ $token: any }>`
  display: flex;
  align-items: flex-end;
  gap: 12px;
  background: ${props => props.$token.colorBgElevated};
  border: 1px solid ${props => props.$token.colorBorderSecondary};
  border-radius: 12px;
  padding: 12px;
  
  &:focus-within {
    border-color: ${props => props.$token.colorPrimary};
    box-shadow: 0 0 0 2px ${props => props.$token.colorPrimary}20;
  }
`;

const ConversationItem = styled.div<{ $active?: boolean; $token: any }>`
  padding: 12px 16px;
  margin: 4px 0;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.2s ease;
  background: ${props => props.$active ? props.$token.colorPrimaryBg : 'transparent'};
  color: ${props => props.$active ? props.$token.colorPrimary : props.$token.colorText};
  
  &:hover {
    background: ${props => props.$token.colorFillSecondary};
  }
  
  .conversation-title {
    font-size: 14px;
    font-weight: 500;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }
  
  .conversation-time {
    font-size: 12px;
    color: ${props => props.$token.colorTextTertiary};
    margin-top: 4px;
  }
`;

// Message type definition
interface Message {
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
interface ChatHistory {
  id: string;
  title: string;
  messages: Message[];
  model: string;
  tokenId: string;
  createdAt: number;
  updatedAt: number;
}

export default function ChatGPTInterface() {
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
  const [systemPrompt, setSystemPrompt] = useState<string>('You are a helpful AI assistant.');
  const [streaming, setStreaming] = useState(false);
  const [currentAssistantMessage, setCurrentAssistantMessage] = useState<string>('');
  const [settingsVisible, setSettingsVisible] = useState(false);
  const [sidebarCollapsed, setSidebarCollapsed] = useState(isMobile);
  const [conversations, setConversations] = useState<ChatHistory[]>([]);
  const [currentConversationId, setCurrentConversationId] = useState<string>('');
  const [regeneratingMessageIndex, setRegeneratingMessageIndex] = useState<number | null>(null);
  
  useEffect(() => {
    fetchTokens();
    loadConversations();
  }, []);

  useEffect(() => {
    scrollToBottom();
  }, [messages, currentAssistantMessage]);

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  const fetchTokens = async () => {
    try {
      const res = await getTokens(1, 100);
      if (res?.data?.items) {
        setTokenOptions(res.data.items);
        if (res.data.items.length > 0) {
          const firstToken = res.data.items[0];
          setSelectedToken(firstToken.key);
          fetchModels(firstToken.key);
        }
      }
    } catch (error) {
      console.error('Failed to fetch tokens:', error);
    }
  };

  const fetchModels = async (token: string) => {
    try {
      const openai = new OpenAI({
        apiKey: token,
        baseURL: window.location.origin + '/v1',
        dangerouslyAllowBrowser: true
      });
      
      const response = await openai.models.list();
      const models = response.data.map(model => model.id);
      setModelOptions(models);
      if (models.length > 0) {
        setSelectedModel(models[0]);
      }
    } catch (error) {
      console.error('Failed to fetch models:', error);
      const res = await getModels();
      if (res?.data) {
        const chatModels = res.data.filter((model: string) => 
          model.includes('gpt') || model.includes('chat')
        );
        setModelOptions(chatModels);
        if (chatModels.length > 0) setSelectedModel(chatModels[0]);
      }
    }
  };

  const loadConversations = () => {
    const saved = localStorage.getItem('chatgpt-conversations');
    if (saved) {
      const convs = JSON.parse(saved);
      setConversations(convs);
      if (convs.length > 0) {
        loadConversation(convs[0]);
      }
    }
  };

  const saveConversations = (convs: ChatHistory[]) => {
    localStorage.setItem('chatgpt-conversations', JSON.stringify(convs));
  };

  const createNewConversation = () => {
    const newConv: ChatHistory = {
      id: Date.now().toString(),
      title: 'New Chat',
      messages: [],
      model: selectedModel,
      tokenId: selectedToken,
      createdAt: Date.now(),
      updatedAt: Date.now()
    };
    const updated = [newConv, ...conversations];
    setConversations(updated);
    saveConversations(updated);
    setCurrentConversationId(newConv.id);
    setMessages([]);
  };

  const loadConversation = (conv: ChatHistory) => {
    setCurrentConversationId(conv.id);
    setMessages(conv.messages);
    setSelectedModel(conv.model);
    setSelectedToken(conv.tokenId);
  };

  const updateConversation = (messages: Message[]) => {
    if (!currentConversationId) return;
    
    const updated = conversations.map(conv => 
      conv.id === currentConversationId 
        ? { ...conv, messages, updatedAt: Date.now(), title: messages[0]?.content?.slice(0, 50) || 'New Chat' }
        : conv
    );
    setConversations(updated);
    saveConversations(updated);
  };

  const handleSendMessage = async () => {
    if (!prompt.trim() || !selectedModel || !selectedToken) return;
    
    const userMessage: Message = {
      role: 'user',
      content: prompt,
      timestamp: Date.now()
    };
    
    const newMessages = [...messages, userMessage];
    setMessages(newMessages);
    updateConversation(newMessages);
    setPrompt('');
    
    await generateResponse(newMessages);
  };

  const generateResponse = async (currentMessages: Message[]) => {
    setLoading(true);
    setStreaming(true);
    setCurrentAssistantMessage('');
    
    try {
      const messageArray = [
        { role: 'system', content: systemPrompt },
        ...currentMessages.map(msg => ({ role: msg.role, content: msg.content }))
      ];
      
      const startTime = Date.now();
      const stream = await createCompletion({
        model: selectedModel,
        messages: messageArray as any,
        token: selectedToken,
        baseURL: window.location.origin + '/v1',
        stream: true
      });
      
      let firstTokenTime = 0;
      let responseContent = '';
      
      await processStreamResponse(
        stream,
        (content: string) => {
          if (responseContent === '' && content) {
            firstTokenTime = Date.now();
          }
          responseContent += content;
          setCurrentAssistantMessage(responseContent);
        },
        isResponsesModel(selectedModel)
      );
      
      const completionTime = Date.now();
      const performance = {
        firstTokenTime: firstTokenTime - startTime,
        completionTime: completionTime - startTime,
        totalTokens: responseContent.length / 4,
        tokensPerSecond: (responseContent.length / 4) / ((completionTime - startTime) / 1000)
      };
      
      const assistantMessage: Message = {
        role: 'assistant',
        content: responseContent,
        timestamp: Date.now(),
        performance
      };
      
      const finalMessages = [...currentMessages, assistantMessage];
      setMessages(finalMessages);
      updateConversation(finalMessages);
      
    } catch (error: any) {
      console.error('Error:', error);
      message.error(error.message || 'Failed to generate response');
    } finally {
      setLoading(false);
      setStreaming(false);
      setCurrentAssistantMessage('');
    }
  };

  const handleRegenerateMessage = async (index: number) => {
    if (index <= 0) return;
    
    const userMessageIndex = index - 1;
    const userMessage = messages[userMessageIndex];
    if (userMessage.role !== 'user') return;
    
    setRegeneratingMessageIndex(index);
    const newMessages = messages.slice(0, userMessageIndex + 1);
    setMessages(newMessages);
    updateConversation(newMessages);
    
    await generateResponse(newMessages);
    setRegeneratingMessageIndex(null);
  };

  const handleDeleteMessage = (index: number) => {
    const newMessages = messages.filter((_, i) => i !== index && i !== index + 1);
    setMessages(newMessages);
    updateConversation(newMessages);
  };

  const clearChat = () => {
    setMessages([]);
    updateConversation([]);
  };

  const renderSidebar = () => (
    <Sidebar $token={token} $collapsed={sidebarCollapsed}>
      <SidebarHeader $token={token}>
        <Button
          type="primary"
          icon={<PlusOutlined />}
          onClick={createNewConversation}
          style={{ width: '100%' }}
        >
          New Chat
        </Button>
        <Button
          type="text"
          icon={<CloseOutlined />}
          onClick={() => setSidebarCollapsed(true)}
          style={{ display: isMobile ? 'block' : 'none' }}
        />
      </SidebarHeader>
      
      <SidebarContent>
        {conversations.map(conv => (
          <ConversationItem
            key={conv.id}
            $active={conv.id === currentConversationId}
            $token={token}
            onClick={() => loadConversation(conv)}
          >
            <div className="conversation-title">{conv.title}</div>
            <div className="conversation-time">
              {new Date(conv.updatedAt).toLocaleDateString()}
            </div>
          </ConversationItem>
        ))}
      </SidebarContent>
    </Sidebar>
  );

  const renderMessages = () => {
    if (messages.length === 0) {
      return (
        <div style={{ 
          display: 'flex', 
          alignItems: 'center', 
          justifyContent: 'center', 
          height: '60%',
          flexDirection: 'column',
          color: token.colorTextSecondary
        }}>
          <RobotOutlined style={{ fontSize: 64, marginBottom: 16 }} />
          <Title level={3}>Welcome to Chat</Title>
          <Text>Select a model and start chatting!</Text>
        </div>
      );
    }

    return messages.map((msg, index) => (
      <MessageBubble key={index} $isUser={msg.role === 'user'} $token={token}>
        <Avatar
          icon={msg.role === 'user' ? <UserOutlined /> : <RobotOutlined />}
          style={{ 
            backgroundColor: msg.role === 'user' ? token.colorPrimary : token.colorSuccess,
            marginTop: 4
          }}
        />
        <div style={{ flex: 1 }} className="message-content">
          <div style={{ 
            backgroundColor: msg.role === 'user' ? token.colorPrimary : token.colorBgContainer,
            color: msg.role === 'user' ? '#fff' : token.colorText,
            padding: '16px 20px',
            borderRadius: msg.role === 'user' ? '20px 20px 4px 20px' : '20px 20px 20px 4px',
            boxShadow: '0 1px 2px rgba(0, 0, 0, 0.1)'
          }}>
            <ReactMarkdown>{msg.content}</ReactMarkdown>
          </div>
          
          {msg.role === 'assistant' && msg.performance && (
            <div style={{ marginTop: 8 }} className="message-actions">
              <Space size="small">
                <Tag icon={<ThunderboltOutlined />} size="small">
                  {msg.performance.completionTime}ms
                </Tag>
                <Button
                  size="small"
                  icon={<SyncOutlined />}
                  onClick={() => handleRegenerateMessage(index)}
                  loading={regeneratingMessageIndex === index}
                >
                  Regenerate
                </Button>
                <Button
                  size="small"
                  icon={<DeleteOutlined />}
                  onClick={() => handleDeleteMessage(index)}
                >
                  Delete
                </Button>
              </Space>
            </div>
          )}
        </div>
      </MessageBubble>
    ));
  };

  return (
    <ChatGPTContainer $token={token}>
      {renderSidebar()}
      
      <ChatArea>
        <ChatHeader $token={token}>
          <div style={{ display: 'flex', alignItems: 'center', gap: 12 }} >
            <Button
              icon={<MenuOutlined />}
              onClick={() => setSidebarCollapsed(!sidebarCollapsed)}
              style={{ display: isMobile ? 'block' : 'none' }}
            />
            <Title level={4} style={{ margin: 0 }}>ChatGPT Interface</Title>
          </div>
          
          <Space>
            <Select
              placeholder="Select Token"
              value={selectedToken}
              onChange={(value) => {
                setSelectedToken(value);
                fetchModels(value);
              }}
              style={{ width: 200 }}
              size="middle"
            >
              {tokenOptions.map(token => (
                <Option key={token.key} value={token.key}>
                  {token.name || token.key}
                </Option>
              ))}
            </Select>
            
            <Select
              placeholder="Select Model"
              value={selectedModel}
              onChange={setSelectedModel}
              style={{ width: 200 }}
              size="middle"
            >
              {modelOptions.map(model => (
                <Option key={model} value={model}>{model}</Option>
              ))}
            </Select>
            
            <Button
              icon={<SettingOutlined />}
              onClick={() => setSettingsVisible(true)}
            />
            
            <Popconfirm
              title="Clear chat history?"
              onConfirm={clearChat}
              okText="Yes"
              cancelText="No"
            >
              <Button icon={<ClearOutlined />} danger />
            </Popconfirm>
          </Space>
        </ChatHeader>

        <MessagesContainer $token={token}>
          {renderMessages()}
          
          {streaming && currentAssistantMessage && (
            <MessageBubble $isUser={false} $token={token}>
              <Avatar icon={<RobotOutlined />} style={{ backgroundColor: token.colorSuccess }} />
              <div style={{ flex: 1 }} className="message-content">
                <div style={{ 
                  backgroundColor: token.colorBgContainer,
                  padding: '16px 20px',
                  borderRadius: '20px 20px 20px 4px',
                  boxShadow: '0 1px 2px rgba(0, 0, 0, 0.1)'
                }}>
                  <ReactMarkdown>{currentAssistantMessage}</ReactMarkdown>
                  <div style={{ marginTop: 8 }} >
                    <Spin size="small" />
                  </div>
                </div>
              </div>
            </MessageBubble>
          )}
          
          <div ref={messagesEndRef} />
        </MessagesContainer>

        <InputContainer $token={token}>
          <InputWrapper $token={token}>
            <TextArea
              value={prompt}
              onChange={e => setPrompt(e.target.value)}
              placeholder="Message ChatGPT..."
              autoSize={{ minRows: 1, maxRows: 6 }}
              onKeyDown={e => {
                if (e.key === 'Enter' && !e.shiftKey) {
                  e.preventDefault();
                  handleSendMessage();
                }
              }}
              style={{ 
                border: 'none',
                boxShadow: 'none',
                resize: 'none',
                background: 'transparent'
              }}
            />
            
            <Button
              type="primary"
              icon={<SendOutlined />}
              onClick={handleSendMessage}
              disabled={!prompt.trim() || !selectedModel || !selectedToken || loading}
              loading={loading}
              style={{ 
                borderRadius: 8,
                height: 40,
                width: 40,
                minWidth: 40
              }}
            />
          </InputWrapper>
          
          <Text type="secondary" style={{ fontSize: 12, marginTop: 8 }}>
            Press Enter to send, Shift+Enter for new line
          </Text>
        </InputContainer>
      </ChatArea>

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
              placeholder="System prompt..."
            />
          </Form.Item>
        </Form>
      </Modal>
    </ChatGPTContainer>
  );
}
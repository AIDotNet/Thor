import React, { useState } from 'react';
import { theme, Tabs, Grid, Button } from 'antd';
import { MessageOutlined, PictureOutlined, RobotOutlined } from '@ant-design/icons';
import { Flexbox } from 'react-layout-kit';
import { useTranslation } from 'react-i18next';
import { isMobileDevice } from '../../utils/responsive';
import ChatFeature from './features/chat';
import ImageFeature from './features/image';
import ChatGPTInterface from './ChatGPTInterface';
import { getModelInfo } from '../../services/ModelService';
const { useBreakpoint } = Grid;

type TabKey = 'chat' | 'image' | 'chatgpt';

export default function Playground() {
    const { token } = theme.useToken();
    const { t } = useTranslation();
    const screens = useBreakpoint();
    const isMobile = !screens.md || isMobileDevice();
    const [activeTab, setActiveTab] = useState<TabKey>('chatgpt');

    const [modelInfo, setModelInfo] = useState<any>(null);

    React.useEffect(() => {
        getModelInfo().then((res) => {
            setModelInfo(res.data);
        });
    }, []);

    const handleTabChange = (key: string) => {
        setActiveTab(key as TabKey);
    };

    return (
        <Flexbox
            gap={0}
            style={{
                height: '100%',
                position: 'relative',
                overflow: 'hidden',
                backgroundColor: token.colorBgLayout
            }}
        >
            <Tabs
                activeKey={activeTab}
                onChange={handleTabChange}
                type="card"
                size={isMobile ? "small" : "middle"}
                style={{
                    width: '100%',
                    height: '100%'
                }}
                className="playground-tabs"
                tabBarStyle={{
                    marginBottom: 0,
                    padding: isMobile ? '8px 12px 0' : '16px 24px 0',
                    backgroundColor: token.colorBgContainer,
                    borderBottom: `1px solid ${token.colorBorderSecondary}`
                }}
                items={[
                    {
                        key: 'chatgpt',
                        label: (
                            <span>
                                <RobotOutlined />
                                {!isMobile && <span style={{ marginLeft: 8 }}>ChatGPT</span>}
                            </span>
                        ),
                        children: <ChatGPTInterface />
                    },
                    {
                        key: 'chat',
                        label: (
                            <span>
                                <MessageOutlined />
                                {!isMobile && <span style={{ marginLeft: 8 }}>{t('playground.tabs.chat')}</span>}
                            </span>
                        ),
                        children: <ChatFeature modelInfo={modelInfo} />
                    },
                    {
                        key: 'image',
                        label: (
                            <span>
                                <PictureOutlined />
                                {!isMobile && <span style={{ marginLeft: 8 }}>{t('playground.tabs.image')}</span>}
                            </span>
                        ),
                        children: <ImageFeature modelInfo={modelInfo} />
                    }
                ]}
            />
        </Flexbox>
    );
}

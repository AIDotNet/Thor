import { Header, Logo, TabsNav } from "@lobehub/ui";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import React from 'react';
import useThemeStore from '../../store/theme';

export default function ThorHeader() {
    const navigate = useNavigate();
    const { theme, toggleTheme } = useThemeStore();

    const [key, setKey] = useState(window.location.pathname.split('/')[1] || 'welcome');
    
    return (
        <Header
            logo={<Logo extra={'Thor雷神托尔'} />}
            nav={
                <TabsNav
                    activeKey={key}
                    onChange={(key) => {
                        switch (key) {
                            case 'panel':
                                navigate('/panel');
                                break;
                            case 'doc':
                                navigate('/doc');
                                break;
                            case 'model':
                                navigate('/model');
                                break;
                            case 'welcome':
                                navigate('/');
                                break;
                        }
                        setKey(key);
                    }}
                    items={[
                        {
                            key: 'welcome',
                            label: '首页',
                        },
                        {
                            key: 'model',
                            label: '模型',
                        },
                        {
                            key: 'doc',
                            label: '文档',
                        },
                        {
                            key: 'panel',
                            label: '面板',
                        }
                    ]}
                />
            }
        >
            <header>
                <h1>当前主题: {theme}</h1>
                <button onClick={toggleTheme}>切换主题</button>
            </header>
        </Header>
    )
}
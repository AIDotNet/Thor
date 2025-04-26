import { Header, Logo, TabsNav, ThemeSwitch } from "@lobehub/ui";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import useThemeStore from '../../store/theme';
import PwaInstall from '../PwaInstall';
import { Space } from 'antd';
import { useTranslation } from 'react-i18next';

export default function ThorHeader() {
    const navigate = useNavigate();
    const { themeMode, toggleTheme } = useThemeStore();
    const { t } = useTranslation();

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
                            case 'api_doc':
                                window.open('https://thor-ai.apifox.cn', '_blank');
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
                        },
                        {
                            key: 'api_doc',
                            label: 'API文档',
                        }
                    ]}
                />
            }
        >
            <header>
                <Space size={8}>
                    <PwaInstall 
                        buttonMode={true}
                        buttonText={t('common.installApp')}
                        buttonType='text'
                        buttonSize='middle'
                        buttonStyle={{ 
                            display: 'flex',
                            alignItems: 'center'
                        }}
                        logoUrl='/logo.png'
                    />
                    <ThemeSwitch 
                        onThemeSwitch={(model) => toggleTheme(model)}
                        themeMode={themeMode} 
                    />
                </Space>
            </header>
        </Header>
    )
}
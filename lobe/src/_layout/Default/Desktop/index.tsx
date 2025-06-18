import { Layout } from "@lobehub/ui";
import { Outlet, useNavigate } from "react-router-dom";
import ThorFooter from "../../../components/Footer";
import { Content, Header } from "antd/es/layout/layout";
import { MenuOutlined, ThunderboltOutlined } from "@ant-design/icons";
import { Button, Menu } from "antd";
import { useState, useMemo } from "react";
import { useTranslation } from "react-i18next";
import LanguageSwitcher from "../../../components/LanguageSwitcher";


export default function DesktopPage() {
    const navigate = useNavigate()
    const [mobileMenuVisible, setMobileMenuVisible] = useState(false);
    const { t, i18n } = useTranslation();

    const toggleMobileMenu = () => {
        setMobileMenuVisible(!mobileMenuVisible);
    };

    // 使用useMemo创建菜单项，这样语言变化时菜单会重新生成
    const menuItems = useMemo(() => [
        { key: 'home', label: t('nav.welcome') },
        { key: 'models', label: t('nav.model') },
        { key: 'pricing', label: t('nav.product') },
        { key: 'community', label: t('common.community') },
        {
            key: 'api_doc',
            label: 'API文档',
            onClick: () => {
                window.open('https://thor-ai.apifox.cn', '_blank');
            }
        },
    ], [t, i18n.language]); // 依赖i18n.language以响应语言变化

    // 使用useMemo创建移动端菜单项
    const mobileMenuItems = useMemo(() => [
        {
            key: 'home',
            label: t('nav.welcome'),
            onClick: () => {
                navigate('/');
                toggleMobileMenu();
            }
        },
        {
            key: 'models',
            label: t('nav.model'),
            onClick: () => {
                navigate('/model');
                toggleMobileMenu();
            }
        },
        {
            key: 'community',
            label: t('common.community'),
            onClick: () => {
                window.open('https://github.com/AIDotNet/');
            }
        },
        {
            key: 'api_doc',
            label: 'API文档',
            onClick: () => {
                window.open('https://thor-ai.apifox.cn', '_blank');
            }
        },
    ], [t, i18n.language, navigate]); // 依赖i18n.language以响应语言变化

    return (
        <Layout >
            <Header style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'space-between',
                background: '#141414',
                padding: '0 24px',
                position: 'sticky',
                top: 0,
                zIndex: 1000,
                boxShadow: '0 2px 8px rgba(0,0,0,0.15)'
            }}>
                <div style={{ display: 'flex', alignItems: 'center' }}>
                    <div
                        onClick={() => {
                            navigate('/')
                        }}
                        style={{ display: 'flex', alignItems: 'center', color: 'white', marginRight: 24 }}>
                        <ThunderboltOutlined style={{ color: '#faad14', fontSize: 24, marginRight: 8 }} />
                        <span style={{ fontSize: 20, fontWeight: 'bold' }}>Thor 雷神托尔</span>
                    </div>
                    {/* @ts-ignore */}
                    <div style={{ display: 'none', '@media (min-width: 768px)': { display: 'block' } }}>
                        <Menu
                            theme="dark"
                            mode="horizontal"
                            defaultSelectedKeys={['home']}
                            style={{ background: '#141414', border: 'none' }}
                            items={menuItems}
                        />
                    </div>
                </div>
                <div style={{ display: 'flex', alignItems: 'center' }}>
                    <LanguageSwitcher />
                    <Button type="primary"
                        onClick={() => {
                            navigate('/panel')
                        }}
                        style={{ marginRight: 8, background: '#1890ff' }}>
                        {t('nav.enterSystem')}
                    </Button>
                    <Button
                        onClick={() => {
                            navigate('/register')
                        }}
                    >{t('nav.register')}</Button>
                    <Button
                        icon={<MenuOutlined />}
                        type="text"
                        style={{
                            color: 'white', marginLeft: 8, display: 'block',
                            // @ts-ignore
                            '@media (min-width: 768px)': { display: 'none' }
                        }}
                        onClick={toggleMobileMenu}
                    />
                </div>
            </Header>

            {mobileMenuVisible && (
                <div style={{ background: '#1f1f1f', padding: '16px 24px' }}>
                    <Menu
                        theme="dark"
                        mode="inline"
                        style={{ background: '#1f1f1f', border: 'none' }}
                        items={mobileMenuItems}
                    />
                </div>
            )}
            <Content style={{
                height: 'calc(100vh - 64px)',
                overflowY: 'auto'
            }}>
                <Outlet />
                {location.pathname !== '/model' && <ThorFooter />}
            </Content>
        </Layout>
    )
}
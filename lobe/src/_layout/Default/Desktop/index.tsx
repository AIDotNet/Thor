import { Layout } from "@lobehub/ui";
import { Outlet, useNavigate } from "react-router-dom";
import ThorFooter from "../../../components/Footer";
import { Header } from "antd/es/layout/layout";
import { MenuOutlined, ThunderboltOutlined } from "@ant-design/icons";
import { Button, Menu } from "antd";
import { useState } from "react";


export default function DesktopPage() {
    const navigate = useNavigate()
    const [mobileMenuVisible, setMobileMenuVisible] = useState(false);

    const toggleMobileMenu = () => {
        setMobileMenuVisible(!mobileMenuVisible);
    };

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
                        onClick={()=>{
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
                            items={[
                                { key: 'home', label: '首页' },
                                { key: 'models', label: '模型库' },
                                { key: 'docs', label: '开发文档' },
                                { key: 'pricing', label: '价格方案' },
                                { key: 'community', label: '社区' },
                            ]}
                        />
                    </div>
                </div>
                <div style={{ display: 'flex', alignItems: 'center' }}>
                    <Button type="primary"
                        onClick={() => {
                            navigate('/panel')
                        }}
                        style={{ marginRight: 8, background: '#1890ff' }}>
                        进入系统
                    </Button>
                    <Button
                        onClick={() => {
                            navigate('/register')
                        }}
                    >注册</Button>
                    <Button
                        icon={<MenuOutlined />}
                        type="text"
                        style={{ color: 'white', marginLeft: 8, display: 'block', 
                            // @ts-ignore
                            '@media (min-width: 768px)': { display: 'none' } }}
                        onClick={toggleMobileMenu}
                    />
                </div>
            </Header>

            {/* Mobile Menu */}
            {mobileMenuVisible && (
                <div style={{ background: '#1f1f1f', padding: '16px 24px' }}>
                    <Menu
                        theme="dark"
                        mode="inline"
                        style={{ background: '#1f1f1f', border: 'none' }}
                        items={[
                            {
                                key: 'home',
                                label: '首页',
                                onClick: () => {
                                    navigate('/');
                                    toggleMobileMenu()
                                }
                            },
                            {
                                key: 'models',
                                label: '模型库',
                                onClick: () => {
                                    navigate('/model');
                                    toggleMobileMenu()
                                }
                            },
                            {
                                key: 'docs',
                                label: '开发文档',
                                onClick: () => {
                                    navigate('/doc');
                                    toggleMobileMenu()
                                }
                            },
                            {
                                key: 'community',
                                label: '社区',
                                onClick: () => {
                                    window.open('https://github.com/AIDotNet/')
                                }
                            },
                        ]}
                    />
                </div>
            )}
            <div style={{
                height: 'calc(100vh - 64px)',
                overflow: 'auto'
            }}>
                <Outlet />
                <ThorFooter />
            </div>
        </Layout>
    )
}
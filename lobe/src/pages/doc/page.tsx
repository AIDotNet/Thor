import { Tabs } from 'antd'
import DocsMarkdown from "./features/DevelopDoc";
import { useLocation, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';

export default function DesktopLayout() {
    const [activeKey, setActiveKey] = useState('developdoc');

    const location = useLocation();
    const navigate = useNavigate();

    const tabKey = location.pathname.split('/').pop();

    useEffect(() => {
        if (tabKey !== "doc") {
            setActiveKey(tabKey ?? 'developdoc');
        } else {
            setActiveKey('developdoc');
        }

    }, [tabKey]);


    return (
        <>
            <Tabs
                style={{
                    margin: '0 20px',
                    padding: '20px 0',
                }}
                tabPosition='left'
                activeKey={activeKey}
                onChange={(key) => {
                    navigate('/doc/' + key);
                }}
                items={[
                    {
                        key: 'developdoc',
                        label: '部署Thor',
                        children: <DocsMarkdown doc='develop.md' />
                    },
                    {
                        key: 'create-channel',
                        label: '创建渠道',
                        children: <DocsMarkdown doc='create-channel.md' />
                    },
                    {
                        key: 'create-token',
                        label: '创建令牌',
                        children: <DocsMarkdown doc='create-token.md' />
                    },
                ]}>
            </Tabs>
        </>
    );
}
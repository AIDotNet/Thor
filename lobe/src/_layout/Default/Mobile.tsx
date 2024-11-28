import { Outlet } from "react-router-dom";
import { Burger, Layout } from '@lobehub/ui';
import ThorFooter from "../../components/Footer";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
export default function MobilePage() {
    const navigate = useNavigate();

    const items = [
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
        }]

    const [opened, setOpened] = useState(false);
    return (
        <div style={{
            overflow: 'auto',
            height: '100%',
        }}>
            <Layout
                footer={
                    <ThorFooter />
                }
                header={
                    <Burger
                        onClick={(v) => {
                            setOpened(!opened);
                            switch (v.key) {
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
                            }
                        }}
                        items={items} opened={opened} setOpened={setOpened} />
                }
            >
                <Outlet />
            </Layout>
        </div>
    )
}
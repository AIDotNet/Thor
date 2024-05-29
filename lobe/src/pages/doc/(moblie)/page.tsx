import {  Layout, MobileTabBar, } from "@lobehub/ui";
import { LayoutProps } from "../../../_layout/type";
import { } from '@lobehub/icons';
import DevelopDoc from "../features/DevelopDoc";
import { MessageCircle } from 'lucide-react';
import { useState } from "react";
import VerifyGpt4 from "../features/VerifyGpt4";

export default function DocPage({ nav }: LayoutProps) {
    const [content, setContent] = useState(<DevelopDoc />);
    const [activeKey, setActiveKey] = useState('1');

    return <>
        <Layout
            header={nav}
            footer={
                <MobileTabBar
                    style={{
                        height: '50px',
                    }}
                    activeKey={activeKey}
                    onChange={(key) => {
                        if (key === "1") {
                            setContent(<DevelopDoc />)
                        } else if (key === "2") {
                            setContent(<VerifyGpt4 />)
                        }
                        setActiveKey(key)
                    }}
                    items={[
                        {
                            key: '1',
                            icon: <MessageCircle />,
                            title: '首页',
                            onClick: () => {

                            }
                        },
                        {
                            key: '2',
                            icon: <MessageCircle />,
                            title: '文档',

                        }
                    ]} />
            }
        >
            {content}
        </Layout>
    </>;
}
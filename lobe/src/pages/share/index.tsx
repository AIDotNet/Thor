import { Markdown } from '@lobehub/ui';
import { Card, Button } from 'antd';
import { useEffect } from 'react';
import { Share } from '../../services/SystemService';

export default function SharePage() {
    const md =
        `
## 支持多种模型

我们支持非常多的厂家的模型，包括但不限于：
- OpenAI
- Kimi
- Claude
- 智谱清言
- 讯飞

## 更低的价格

我们提供了比OpenAI更低的价格（最低六折），同时提供了更多的模型支持，并且提供更安全的服务。
`;

    useEffect(() => {
        const urlParams = new URLSearchParams(window.location.search);
        const userId = urlParams.get('userId');
        if (userId) {
            Share(userId)
                .then(() => {
                    console.log('分享成功');
                })
                .catch(() => {
                    console.log('分享失败');
                });
        }

    }, []);

    return (
        <div style={{ margin: '20px' }}>
            <h1 style={{ textAlign: 'center' }}>欢迎使用Thor（雷神托尔）AI智能网关</h1>
            <Card
                hoverable
                style={{ width: '100%', height: 'calc(100vh - 200px)', overflowY: 'auto', overflowX: 'hidden' }}
                cover={<img alt="宣传图1" src="/images/ai.jpg" />}
            >
                <Card.Meta title="超多模型支持" description={
                    <Markdown>
                        {md}
                    </Markdown>
                } />
            </Card>
            <Button style={{
                marginTop: '20px'
            }} block onClick={() => {
                window.location.href = '/login';
            }}>
                加入使用
            </Button>
        </div>
    );
}
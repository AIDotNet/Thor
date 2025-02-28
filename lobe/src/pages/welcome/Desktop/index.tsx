import { Button, Card, Layout, Space, Typography, Row, Col, Statistic } from 'antd';
import {
    GithubOutlined,
    RocketOutlined,
    ApiOutlined,
    ThunderboltOutlined,
    TeamOutlined,
    DollarOutlined,
    AppstoreOutlined,
    CheckCircleOutlined,
} from '@ant-design/icons';
import './styles.css';
import styled from 'styled-components';
import { createStyles } from 'antd-style';
import { useNavigate } from 'react-router-dom';

const { Content } = Layout;
const { Title, Paragraph, Text } = Typography;

// 使用 antd-style 创建样式
const useStyles = createStyles(({ token, css }) => ({
    desktopMenu: css`
    display: none;
    @media (min-width: 768px) {
      display: block;
    }
  `,
    mobileMenuButton: css`
    color: ${token.colorTextLightSolid};
    margin-left: 8px;
    display: block;
    @media (min-width: 768px) {
      display: none;
    }
  `,
    featureCard: css`
    height: 100%;
    transition: all 0.3s;
    &:hover {
      box-shadow: 0 4px 12px rgba(0,0,0,0.1);
    }
  `,
    heroSection: css`
    position: relative;
    padding: 60px 0;
    background: linear-gradient(135deg, #141414 0%, #1f1f1f 100%);
    color: ${token.colorTextLightSolid};
    overflow: hidden;
  `,
    heroBackground: css`
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    overflow: hidden;
    z-index: 0;
  `,
    heroContainer: css`
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 24px;
    display: flex;
    flex-direction: column;
    align-items: center;
    position: relative;
    z-index: 1;
    @media (min-width: 768px) {
      flex-direction: row;
      align-items: center;
    }
  `,
    heroContent: css`
    margin-bottom: 40px;
    @media (min-width: 768px) {
      width: 50%;
      margin-bottom: 0;
      padding-right: 24px;
    }
  `,
    heroImage: css`
    @media (min-width: 768px) {
      width: 50%;
    }
  `,
    heroTitle: css`
    color: ${token.colorTextLightSolid} !important;
    font-size: 48px;
    font-weight: bold;
    margin-bottom: 24px;
  `,
    heroButtons: css`
    display: flex;
    flex-direction: column;
    @media (min-width: 576px) {
      flex-direction: row;
    }
  `,
    primaryButton: css`
    background: linear-gradient(90deg, ${token.colorPrimary} 0%, ${token.colorPrimaryActive} 100%);
    border: none;
    height: 48px;
    padding: 0 24px;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(24, 144, 255, 0.3);
    margin-bottom: 16px;
    @media (min-width: 576px) {
      margin-bottom: 0;
    }
  `,
    // ... 其他样式
}));

// 添加 FeatureCard 组件定义
const FeatureCard = styled(Card)`
  height: 100%;
  transition: all 0.3s;
  &:hover {
    box-shadow: 0 4px 12px rgba(0,0,0,0.1);
  }
  border-top: 3px solid ${props => props.color || 'transparent'};
`;

// 添加项目卡片样式
const ProjectCard = styled(Card)`
  height: 100%;
  transition: all 0.3s;
  &:hover {
    box-shadow: 0 4px 12px rgba(0,0,0,0.1);
  }
`;


const ThorWebsite = () => {
    const { styles } = useStyles();
    const navigate = useNavigate();
    return (
        <Content>
            <div className={styles.heroSection}>
                <div className={styles.heroBackground}>
                    <div style={{
                        position: 'absolute',
                        width: 300,
                        height: 300,
                        borderRadius: '50%',
                        background: '#1890ff',
                        opacity: 0.1,
                        top: -50,
                        right: -50,
                        filter: 'blur(40px)'
                    }}></div>
                    <div style={{
                        position: 'absolute',
                        width: 200,
                        height: 200,
                        borderRadius: '50%',
                        background: '#722ed1',
                        opacity: 0.1,
                        bottom: 50,
                        left: 100,
                        filter: 'blur(30px)'
                    }}></div>
                </div>

                <div className={styles.heroContainer}>
                    <div className={styles.heroContent}>
                        <Title className={styles.heroTitle}>
                            Thor 雷神托尔
                        </Title>
                        <Paragraph style={{ color: '#d9d9d9', fontSize: 18, marginBottom: 24 }}>
                            使用标准的OpenAI接口协议访问<Text style={{ color: '#1890ff', fontWeight: 'bold' }}>68+</Text>模型，不限时间、按量计费、拒绝逆向、极速对话、明细透明，无隐藏消费。
                        </Paragraph>
                        <Paragraph style={{ color: '#d9d9d9', fontSize: 18, marginBottom: 32 }}>
                            —— 为您提供最好的AI服务！
                        </Paragraph>
                        <Space size="large" className={styles.heroButtons}>
                            <Button
                                type="primary"
                                size="large"
                                onClick={() => {
                                    navigate('/panel')
                                }}
                                className={styles.primaryButton}
                            >
                                <RocketOutlined /> 立即开始使用
                            </Button>
                            <Button
                                size="large"
                                onClick={() => {
                                    window.open('https://github.com/AIDotNet/Thor', '_blank')
                                }}
                                style={{
                                    borderColor: '#434343',
                                    height: 48,
                                    padding: '0 24px',
                                    borderRadius: 8
                                }}
                            >
                                <GithubOutlined /> 给项目 Star
                            </Button>
                        </Space>
                    </div>
                    <div className={styles.heroImage}>
                        <Card
                            className="tilted-card"
                            style={{
                                width: '100%',
                                background: 'linear-gradient(135deg, #1f1f1f 0%, #141414 100%)',
                                borderColor: '#434343',
                                borderRadius: 16,
                                boxShadow: '0 20px 25px -5px rgba(0, 0, 0, 0.3)'
                            }}
                        >
                            <Title level={3} style={{ color: 'white' }}>强大的社区</Title>
                            <Paragraph style={{ color: '#d9d9d9', marginBottom: 24 }}>
                                Thor由AIDotNet社区维护，社区拥有丰富的AI资源，包括模型、数据集、工具等。
                            </Paragraph>
                            <Space direction="vertical" size="large" style={{ width: '100%' }}>
                                <div style={{ display: 'flex', alignItems: 'flex-start' }}>
                                    <DollarOutlined style={{ color: '#1890ff', fontSize: 20, marginTop: 4, marginRight: 16 }} />
                                    <div>
                                        <Text strong style={{ color: 'white', fontSize: 16 }}>按量付费</Text>
                                        <Paragraph style={{ color: '#8c8c8c', marginBottom: 0 }}>
                                            支持用户额度管理，用户可自定义Token 管理，按量计费。
                                        </Paragraph>
                                    </div>
                                </div>
                                <div style={{ display: 'flex', alignItems: 'flex-start' }}>
                                    <AppstoreOutlined style={{ color: '#1890ff', fontSize: 20, marginTop: 4, marginRight: 16 }} />
                                    <div>
                                        <Text strong style={{ color: 'white', fontSize: 16 }}>应用支持</Text>
                                        <Paragraph style={{ color: '#8c8c8c', marginBottom: 0 }}>
                                            支持OpenAi官方库、大部分开源聊天应用、Utools GPT插件
                                        </Paragraph>
                                    </div>
                                </div>
                                <div style={{ display: 'flex', alignItems: 'flex-start' }}>
                                    <CheckCircleOutlined style={{ color: '#1890ff', fontSize: 20, marginTop: 4, marginRight: 16 }} />
                                    <div>
                                        <Text strong style={{ color: 'white', fontSize: 16 }}>明细可查</Text>
                                        <Paragraph style={{ color: '#8c8c8c', marginBottom: 0 }}>
                                            统计每次请求消耗明细，价格透明，无隐藏消费，用的放心
                                        </Paragraph>
                                    </div>
                                </div>
                            </Space>
                        </Card>
                    </div>
                </div>
            </div>

            {/* Stats Section */}
            <div style={{ padding: '48px 0', }}>
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px' }}>
                    <Row gutter={[24, 24]} justify="center">
                        <Col xs={12} md={6}>
                            <Card bordered={false} style={{ textAlign: 'center', height: '100%' }}>
                                <Statistic
                                    title="支持模型"
                                    value="200+"
                                    suffix="个"
                                    valueStyle={{ color: '#1890ff', fontWeight: 'bold' }}
                                />
                            </Card>
                        </Col>
                        <Col xs={12} md={6}>
                            <Card bordered={false} style={{ textAlign: 'center', height: '100%' }}>
                                <Statistic
                                    title="社区用户"
                                    value="8000+"
                                    suffix="人"
                                    valueStyle={{ color: '#1890ff', fontWeight: 'bold' }}
                                />
                            </Card>
                        </Col>
                        <Col xs={12} md={6}>
                            <Card bordered={false} style={{ textAlign: 'center', height: '100%' }}>
                                <Statistic
                                    title="每日请求量"
                                    value="1M+"
                                    suffix="次"
                                    valueStyle={{ color: '#1890ff', fontWeight: 'bold' }}
                                />
                            </Card>
                        </Col>
                        <Col xs={12} md={6}>
                            <Card bordered={false} style={{ textAlign: 'center', height: '100%' }}>
                                <Statistic
                                    title="代码贡献者"
                                    value="10+"
                                    suffix="位"
                                    valueStyle={{ color: '#1890ff', fontWeight: 'bold' }}
                                />
                            </Card>
                        </Col>
                    </Row>
                </div>
            </div>

            {/* Features Section */}
            <div style={{ padding: '80px 0', }}>
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px' }}>
                    <Title level={2} style={{ textAlign: 'center', marginBottom: 16 }}>我们的优势</Title>
                    <Paragraph style={{ textAlign: 'center', color: '#595959', marginBottom: 48, maxWidth: 700, margin: '0 auto 48px' }}>
                        Thor雷神托尔为开发者提供一站式AI模型调用服务，简化您的AI应用开发流程
                    </Paragraph>
                    <Row gutter={[24, 24]}>
                        <Col xs={24} md={8}>
                            <FeatureCard color="#1890ff" bordered={false}>
                                <ApiOutlined style={{ fontSize: 36, color: '#1890ff', marginBottom: 16 }} />
                                <Title level={4}>多模型支持</Title>
                                <Paragraph style={{ color: '#595959' }}>
                                    支持68+模型，包括主流的大型语言模型和专业领域模型，满足各种AI应用场景。
                                </Paragraph>
                                <Button type="link" style={{ padding: 0 }}>了解支持的模型 →</Button>
                            </FeatureCard>
                        </Col>
                        <Col xs={24} md={8}>
                            <FeatureCard color="#722ed1" bordered={false}>
                                <ThunderboltOutlined style={{ fontSize: 36, color: '#722ed1', marginBottom: 16 }} />
                                <Title level={4}>高速响应</Title>
                                <Paragraph style={{ color: '#595959' }}>
                                    优化的服务架构，确保极速对话体验，减少等待时间，提高工作效率。
                                </Paragraph>
                                <Button type="link" style={{ padding: 0 }}>查看性能测试 →</Button>
                            </FeatureCard>
                        </Col>
                        <Col xs={24} md={8}>
                            <FeatureCard color="#52c41a" bordered={false}>
                                <TeamOutlined style={{ fontSize: 36, color: '#52c41a', marginBottom: 16 }} />
                                <Title level={4}>社区驱动</Title>
                                <Paragraph style={{ color: '#595959' }}>
                                    由AIDotNet社区维护，持续更新，提供专业的技术支持和丰富的资源分享。
                                </Paragraph>
                                <Button type="link" style={{ padding: 0 }}>加入我们的社区 →</Button>
                            </FeatureCard>
                        </Col>
                    </Row>
                </div>
            </div>

            <div style={{
                padding: '64px 0',
                background: 'linear-gradient(135deg, #1890ff 0%, #722ed1 100%)',
                color: 'white'
            }}>
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px', textAlign: 'center' }}>
                    <Title level={2} style={{ color: 'white', marginBottom: 16 }}>准备好开始使用Thor雷神托尔了吗？</Title>
                    <Paragraph style={{ color: 'rgba(255,255,255,0.8)', marginBottom: 32, maxWidth: 700, margin: '0 auto 32px' }}>
                        立即注册并获取免费额度，开始您的AI开发之旅
                    </Paragraph>
                    <Space size="large">
                        <Button
                            type="primary"
                            onClick={() => {
                                navigate('/register')
                            }}
                            size="large"
                            style={{
                                color: '#1890ff',
                                height: 48,
                                padding: '0 24px',
                                borderRadius: 8
                            }}
                        >
                            免费注册账号
                        </Button>
                        <Button
                            size="large"
                            ghost
                            onClick={() => {
                                window.open('https://github.com/AIDotNet/Thor', '_blank')
                            }}
                            style={{
                                borderColor: 'white',
                                color: 'white',
                                height: 48,
                                padding: '0 24px',
                                borderRadius: 8
                            }}
                        >
                            查看开发文档
                        </Button>
                    </Space>
                </div>
            </div>

            {/* Projects Section */}
            <div style={{ padding: '64px 0', }}>
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px' }}>
                    <Title level={2} style={{ textAlign: 'center', marginBottom: 48 }}>相关开源项目</Title>
                    <Row gutter={[24, 24]}>
                        <Col xs={24} md={12}>
                            <ProjectCard bordered={false}>
                                <Title level={4}>AIDotNet</Title>
                                <Paragraph style={{ color: '#595959' }}>
                                    AIDotNet社区是一个热衷于AI开发者组成的社区，旨在推动AI技术的发展，为AI开发者提供更好的学习和交流平台。
                                </Paragraph>
                                <Button 
                                    onClick={()=>{
                                        window.open('https://github.com/AIDotNet/', '_blank')
                                    }}
                                    type="link" style={{ padding: 0 }}>了解更多 →</Button>
                            </ProjectCard>
                        </Col>
                        <Col xs={24} md={12}>
                            <ProjectCard bordered={false}>
                                <Title level={4}>FastWiki</Title>
                                <Paragraph style={{ color: '#595959' }}>
                                    一个智能知识库的开源项目，可用于开发企业级智能客服管理系统。支持多种知识库格式，提供高效的检索和答案生成能力。
                                </Paragraph>
                                <Button 
                                    onClick={()=>{
                                        window.open('https://github.com/AIDotNet/fast-wiki/', '_blank')
                                    }}
                                    type="link" style={{ padding: 0 }}>了解更多 →</Button>
                            </ProjectCard>
                        </Col>
                    </Row>
                </div>
            </div>
        </Content>
    );
};

export default ThorWebsite;
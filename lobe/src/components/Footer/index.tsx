import { ThunderboltOutlined, GithubOutlined } from '@ant-design/icons';
import {
    email
} from '../../../package.json'
import { Layout, Row, Col, Typography, Divider, Button, Space } from 'antd';
import { useNavigate } from 'react-router-dom';
import styled from 'styled-components';

const { Footer } = Layout;
const { Title, Paragraph, Text } = Typography;

// 添加响应式footer样式
const FooterContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  
  @media (min-width: 768px) {
    flex-direction: row;
    justify-content: space-between;
  }
`;


const FooterText = styled(Text)`
  color: #8c8c8c;
  margin-bottom: 8px;
  
  @media (min-width: 768px) {
    margin-bottom: 0;
  }
`;
export default function ThorFooter() {
    const navigate = useNavigate()

    return (
        <Footer style={{ background: '#141414', color: '#8c8c8c', padding: '48px 0' }}>
            <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px' }}>
                <Row gutter={[24, 32]}>
                    <Col xs={24} md={12}>
                        <div style={{ display: 'flex', alignItems: 'center', color: 'white', marginBottom: 16 }}>
                            <ThunderboltOutlined style={{ color: '#faad14', fontSize: 24, marginRight: 8 }} />
                            <span style={{ fontSize: 20, fontWeight: 'bold' }}>Thor 雷神托尔</span>
                        </div>
                        <Paragraph style={{ color: '#8c8c8c' }}>
                            Thor是开源的免费并且可商用的开源项目。致力于为开发者提供高效、稳定的AI服务接口。兼容OpenAI API，支持多种模型，按量计费，价格透明。
                        </Paragraph>
                        <Space style={{ marginTop: 16 }}>
                            <Button
                                onClick={()=>{
                                    window.open('https://github.com/AIDotNet/Thor','_blank')
                                }}
                                shape="circle" icon={<GithubOutlined />} />
                        </Space>
                    </Col>
                    <Col xs={12} md={6}>
                        <Title level={4} style={{ color: 'white' }}>产品</Title>
                        <ul style={{ listStyle: 'none', padding: 0 }}>
                            <li style={{ marginBottom: 8 }}><a
                                onClick={() => {
                                    navigate('/model')
                                }}
                                style={{ color: '#8c8c8c' }}>模型列表</a></li>
                        </ul>
                    </Col>
                    <Col xs={12} md={6}>
                        <Title level={4} style={{ color: 'white' }}>支持</Title>
                        <ul style={{ listStyle: 'none', padding: 0 }}>
                            <li style={{ marginBottom: 8 }}><a style={{ color: '#8c8c8c' }}>开发文档</a></li>
                            <li style={{ marginBottom: 8 }}><a style={{ color: '#8c8c8c' }}>常见问题</a></li>
                            <li style={{ marginBottom: 8 }}><a 
                                onClick={()=>{
                                    window.location.href = `mailto:${email}`;
                                }}
                                style={{ color: '#8c8c8c' }}>联系我们</a></li>
                        </ul>
                    </Col>
                </Row>
                <Divider style={{ borderColor: '#303030', margin: '32px 0' }} />
                <FooterContainer>
                    <FooterText>© 2025 AIDotNet. All rights reserved.</FooterText>
                    <Text style={{ color: '#595959' }}>Powered by .NET 9</Text>
                </FooterContainer>
            </div>
        </Footer>
    )
}
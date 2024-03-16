
import { Button, Typography } from '@douyinfe/semi-ui';
import styled from 'styled-components';
import { IconSemiLogo, IconGithubLogo } from '@douyinfe/semi-icons';
import { useNavigate } from 'react-router-dom'
import { config } from '../../config';
const { Text } = Typography;

const Header = styled.header`
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem;
    background-color: #ffffff;
`

const Logo = styled.div`
    > * {
        color: #ff5e62;
    }

`

const Nav = styled.nav`
    > * {
        margin-right: 1rem;
    }
`

const Title = styled.div`  
    margin-bottom: 1rem;
    font-size: 3rem;
    font-weight: bold;
    color: #ffffff;;
`

const Paragraph = styled.div`
    font-size: 1.2rem;
    color: #ffffff;
    margin-bottom: 1rem;
    line-height: 2rem;
`;

const MainContent = styled.main`
    flex: 1;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    user-select: none;
`

const Footer = styled.footer`
    padding: 1rem;
    background-color: #ffffff;
    text-align: center;
`;

const Container = styled.div`
    display: flex;
    flex-direction: column;
    height: 100vh;
    background: linear-gradient(to right, rgb(255, 153, 102), rgb(255, 94, 98));
`;

export default function Home() {
    const navigate = useNavigate()

    const openGithub = () => {
        window.open(config.VITE_GITHUB)
    }
    return (
        <Container>
            <Header >
                <Logo>
                    <IconSemiLogo style={{ fontSize: 36 }} />
                </Logo>
                <Nav>
                    <Button size='large' onClick={() => navigate('/')} theme="borderless">首页</Button>
                    <Button size='large' onClick={() => navigate('/panel')} theme='solid'>控制台</Button>
                </Nav>
            </Header>
            <MainContent>
                <Title >AIDotNet API</Title>
                <Paragraph>
                    All in one 的 OpenAI 接口
                    <br />
                    提供全套 API 访问方式
                    <br />
                    一键部署，开箱即用
                </Paragraph>
                <Button onClick={openGithub} size='large' icon={<IconGithubLogo />} theme="solid">
                    GitHub
                </Button>
            </MainContent>
            <Footer>
                <Text>AIDotNet API由 Token 设计,源代码使用 Apache-2.0 许可</Text>
            </Footer>
        </Container>)
}
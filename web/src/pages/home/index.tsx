
import { Button, Typography } from '@douyinfe/semi-ui';
import styled from 'styled-components';
import { IconGithubLogo } from '@douyinfe/semi-icons';
import { useNavigate } from 'react-router-dom'
import { config } from '../../config';
import { InitSetting, OtherSetting } from '../../services/SettingService';
import { useEffect } from 'react';

const { Text } = Typography;

const Header = styled.header`
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem;
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
`

const Paragraph = styled.div`
    font-size: 1.2rem;
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

    useEffect(() => navigate('/panel'), [])
}
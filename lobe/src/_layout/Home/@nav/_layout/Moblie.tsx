import { Burger, BurgerProps } from "@lobehub/ui";
import { memo, useState } from "react";
import { useNavigate } from "react-router-dom";
const Nav = memo(() => {
    const navigate = useNavigate();
    const [opened, setOpened] = useState(false);
    const items: BurgerProps['items'] = [
        {
            key: 'panel',
            label: '面板',
            onClick: () => {
                navigate('/panel')
            }
        },
        {
            key: "doc",
            label: "接入文档",
            onClick: () => {
                navigate('/doc')
            }
        }, {
            key: "model",
            label: "可用模型",
            onClick: () => {
                navigate('/model')
            }
        }, {
            key: "api_doc",
            label: "API文档",
            onClick: () => {
                window.open('https://thor-ai.apifox.cn', '_blank')
            }
        }
    ]

    return (<Burger items={items}
        opened={opened} setOpened={() => {
            setOpened(!opened)
        }} />)
})

Nav.displayName = 'MobileNav';

export default Nav;
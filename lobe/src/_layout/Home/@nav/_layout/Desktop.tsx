import { Header, TabsNav } from "@lobehub/ui";
import { memo, useState } from "react";
import { useNavigate } from "react-router-dom";

const Nav = memo(() => {
    const navigate = useNavigate();
    const [key, setKey] = useState(window.location.pathname.split('/')[1] || 'panel');

    return (<Header nav={<TabsNav
        onChange={(key) => {
            switch (key) {
                case 'panel':
                    navigate('/panel');
                    break;
                case 'doc':
                    navigate('/doc');
                    break;
                case 'model':
                    navigate('/model');
                    break;
                case 'api_doc':
                    window.open('https://thor-ai.apifox.cn', '_blank');
                    break;
            }
            setKey(key);
        }}
        activeKey={key}
        style={{
            height: '100%',
        }}
        items={[
            {
                key: 'panel',
                label: '面板',
            },
            {
                key: "doc",
                label: "接入文档",
            }, {
                key: "model",
                label: "可用模型"
            }, {
                key: "api_doc",
                label: "API文档"
            }
        ]}
        variant='compact'
    />} />)
});

Nav.displayName = 'DesktopNav';

export default Nav;
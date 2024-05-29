import { memo, useState } from 'react';
import { Flexbox } from 'react-layout-kit';
import { FloatButton } from 'antd';

import { LayoutProps } from './type';
import { Outlet, useNavigate } from 'react-router-dom';
import { MenuOutlined } from '@ant-design/icons';
import { Copilot } from '@lobehub/icons';
import { Tooltip } from '@lobehub/ui';

const Layout = memo<LayoutProps>(({ nav }) => {
    const [open, setOpen] = useState(true);
    const navigate = useNavigate();

    function onChange(checked: boolean) {
        setOpen(checked);
    }

    return (
        <Flexbox
            height={'100%'}
            horizontal
            style={{
                position: 'relative',
            }}
            width={'100%'}
        >
            {nav}
            <Outlet />

            <FloatButton.Group
                open={open}
                trigger="click"
                onClick={() => {
                    onChange(!open);
                }}
                style={{ right: 24 }}
                icon={<MenuOutlined />}
            >
                <Tooltip title="接入文档">
                    <FloatButton onClick={() => {
                        navigate('/doc')
                    }} />
                </Tooltip>
                <Tooltip title="可用模型列表">
                    <FloatButton onClick={() => {
                        navigate('/model')
                    }} icon={<Copilot.Color />} />
                </Tooltip>
            </FloatButton.Group>
        </Flexbox>
    );
});

Layout.displayName = 'DesktopMainLayout';

export default Layout;

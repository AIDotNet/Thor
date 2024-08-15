import { Tabs } from 'antd'
import DevelopDoc from "./features/DevelopDoc";
import VerifyGpt4 from "./features/VerifyGpt4";
export default function DesktopLayout() {
    return (
        <>
            <Tabs
                style={{
                    margin: '0 20px',
                    padding: '20px 0',
                }}
                tabPosition='left'
                items={[
                    {
                        key: '1',
                        label: '开发文档',
                        children: <DevelopDoc />
                    },
                    {
                        key: '2',
                        label: '验证GPT4',
                        children:<VerifyGpt4/>
                    }
                ]}>
            </Tabs>
        </>
    );
}
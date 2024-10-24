import { Layout } from "@lobehub/ui";
import { Outlet } from "react-router-dom";
import ThorFooter from "../../../components/Footer";
import ThorHeader from "../../../components/Header";

export default function DesktopPage() {
    return (
        <div style={{ overflow: 'auto' }}>
            <Layout
                footer={
                    <ThorFooter />
                }

                header={
                    <ThorHeader />
                }
            >
                <Outlet />
            </Layout>
        </div>
    )
}
import { LayoutProps } from "../../../_layout/type";
import Model from "../features/Model";

export default function DesktopLayout({ nav }: LayoutProps) {
    return (
        <>
            {nav}
            <Model />
        </>
    );
}
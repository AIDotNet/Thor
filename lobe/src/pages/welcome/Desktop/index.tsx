import { GradientButton } from "@lobehub/ui";
import { Card } from "antd";
import "../index.css";
import { homepage } from "../../../../package.json";
import { useNavigate } from "react-router-dom";

export default function WelcomePage() {
    const navigate = useNavigate();

    return (
        <div
            style={{
                display: "flex",
                justifyContent: "center",
                height: "100%",
                marginTop: 100,
            }}
        >
            <div
                style={{
                    width: "50%",
                    paddingLeft: 110,
                }}
            >
                <h1
                    style={{
                        fontSize: 40,
                        fontWeight: 700,
                        marginBottom: 10,
                    }}
                >
                    Thor 雷神托尔
                </h1>
                <div
                    style={{
                        fontSize: 20,
                        marginBottom: 10,
                    }}
                >
                    使用标准的OpenAI接口协议访问68+模型，不限时间、
                    按量计费、拒绝逆向、极速对话、明细透明，无隐藏消费。
                </div>
                <div
                    style={{
                        fontSize: 20,
                        marginBottom: 10,
                    }}
                >
                    —— 为您提供最好的AI服务！
                </div>
                <GradientButton
                    onClick={() => {
                        navigate("/panel");
                    }}
                >
                    前往控制台
                </GradientButton>
                <GradientButton
                    glow={true}
                    onClick={() => {
                        window.open(homepage);
                    }}
                    style={{
                        marginLeft: 20,
                    }}
                >
                    给项目Star
                </GradientButton>
            </div>
            <div
                style={{
                    flex: 1,
                    height: "100%",
                }}
            >
                <Card hoverable className="tilted-card" style={{ margin: 40 }}>
                    <h2>强大的社区</h2>
                    <div>
                        Thor由AIDotNet社区维护，社区拥有丰富的AI资源，包括模型、数据集、工具等。
                    </div>
                    <ul className="semi-timeline home-timeline semi-timeline-left">
                        <li className="className className-left">
                            <div className="className-content">
                                <b className="text-15px">按量付费</b>
                                <div className="className-content-time">
                                    支持用户额度管理，用户可自定义Token 管理，按量计费。
                                </div>
                            </div>
                        </li>
                        <li className="className className-left">
                            <div className="className-content">
                                <b className="text-15px">应用支持</b>
                                <div className="className-content-time">
                                    支持OpenAi官方库、大部分开源聊天应用、Utools GPT插件
                                </div>
                            </div>
                        </li>
                        <li className="className className-left">
                            <div className="className-content">
                                <b className="text-15px">明细可查</b>
                                <div className="className-content-time">
                                    统计每次请求消耗明细，价格透明，无隐藏消费，用的放心
                                </div>
                            </div>
                        </li>
                    </ul>
                </Card>
            </div>
        </div>
    );
}

import { Avatar, Dropdown, Modal, SideSheet, Tag } from "@douyinfe/semi-ui";
import { useEffect, useState } from "react";
import { info } from "../../services/UserService";
import { renderQuota } from "../../uitls/render";
import { useNavigate } from "react-router-dom";
import Pay from "../pay";
import Logger from "../../pages/logger";

interface IFrameUIProps {
    children: React.ReactNode;
}

export default function IFrameUI({
    children
}: IFrameUIProps) {
    const navigate = useNavigate()
    const [userInfo, setUserInfo] = useState({} as any)
    const [payVisible, setPayVisible] = useState(false)
    const [dropdownVisible, setDropdownVisible] = useState(false)
    const [logVisible, setLogVisible] = useState(false)

    function getInfo() {
        info()
            .then(res => {
                if (res.success) {
                    setUserInfo(res.data)
                }
            })
    }

    useEffect(() => {
        getInfo()
    }, [])

    return (
        <>
            <Dropdown
                render={
                    <div style={{
                        padding: '10px',
                    }}>
                        <div style={{
                            display: 'flex',
                            alignItems: 'center',
                            marginBottom: '10px'
                        }}>
                            <Avatar style={{
                                marginRight: '10px'
                            }} src={'/logo.png'} />
                            <div>
                                <div style={{
                                    fontSize: '16px',
                                    fontWeight: 'bold'
                                }}>{userInfo.userName}</div>
                                <div style={{
                                    fontSize: '12px',
                                    color: '#999'
                                }}>
                                    账号钱包
                                    <Tag style={{
                                        marginLeft: '5px'
                                    }} color="blue">{renderQuota(userInfo.residualCredit, 2)}</Tag>
                                </div>
                            </div>
                        </div>
                        <Dropdown.Menu>
                            <Dropdown.Item style={{
                                borderRadius: '12px',
                            }} onClick={() => {
                                setPayVisible(true)
                            }}>充值</Dropdown.Item>
                            <Dropdown.Item style={{
                                borderRadius: '12px',
                            }} onClick={() => {
                                setLogVisible(true)
                            }}>消费日志</Dropdown.Item>
                            <Dropdown.Item style={{
                                borderRadius: '12px',
                            }} onClick={() => {
                                localStorage.removeItem('token')
                                navigate('/login')
                            }}>退出登录</Dropdown.Item>
                        </Dropdown.Menu>
                    </div>
                }
            >
                <Avatar
                    onClick={() => {
                        setDropdownVisible(!dropdownVisible)
                    }}
                    style={{
                        position: 'fixed',
                        top: 0,
                        right: 0,
                        height: '40px',
                        backgroundColor: '#282828',
                        color: 'white',
                        borderRadius: '12px',
                        textAlign: 'center',
                    }} src={'/logo.png'} />
            </Dropdown>
            {children}
            <Modal
                footer={[
                    <>
                    </>
                ]}
                onCancel={() => {
                    setPayVisible(false)
                }}
                visible={payVisible}
                title='充值'
            >
                <Pay user={userInfo} />
            </Modal>
            <SideSheet
                width={'80%'}
                title="消费日志" visible={logVisible} onCancel={() => {
                    setLogVisible(false)
                }}>
                <Logger />
            </SideSheet>
        </>
    )
}
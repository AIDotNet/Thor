import { useEffect, useState } from 'react';
import { Input, Button, Avatar, Notification, Tabs, TabPane, Card, Divider, Select, Modal } from '@douyinfe/semi-ui';
import { info, update, updatePassword } from '../../services/UserService';
import { renderQuota } from '../../uitls/render';
import { Use } from '../../services/RedeemCodeService';
import { GeneralSetting, InitSetting, IsEnableAlipay } from '../../services/SettingService';
import { getProduct, startPayload } from '../../services/ProductService';
import QRCode from 'qrcode.react';

export default function ProfileForm() {
    const [user, setUser] = useState({} as any);
    const [code, setCode] = useState('');
    const [password, setPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [products, setProducts] = useState([] as any[]);
    const [product, setProduct] = useState({} as any);
    const [qrCode, setQrCode] = useState('');
    function loadProducts() {
        getProduct()
            .then((res) => {
                setProducts(res.data);
            });
    }

    function loadUser() {
        info()
            .then((res) => {
                setUser(res.data);
            });
    }

    useEffect(() => {
        loadUser();
        loadProducts();
    }, []);

    /**
     * 支付宝充值
     */
    function alipayRecharge(id: string) {
        startPayload(id)
            .then((res) => {
                if (res.success) {
                    setQrCode(res.data.qr);
                } else {
                    Notification.error({
                        title: '充值失败',
                        content: res.message
                    });
                }
            });
    }

    /**
     * 提交修改
     */
    const handleSubmit = () => {
        update(user)
            .then((res) => {
                res.success ? Notification.success({
                    title: '修改成功',
                }) : Notification.error({
                    title: '修改失败',
                });
            });
    };

    /**
     * 修改密码
     */
    function onUpdatePassword() {
        updatePassword({
            oldPassword: password,
            newPassword: newPassword
        })
            .then((res) => {
                res.success ? Notification.success({
                    title: '修改成功',
                }) : Notification.error({
                    title: '修改失败',
                    content: res.message
                });
            });
    }

    /**
     * 使用兑换码
     * @returns 
     */
    function useCode() {
        if (code === '') return Notification.error({
            title: '兑换失败',
            content: '兑换码不能为空'
        });

        Use(code)
            .then((res) => {
                res.success ? Notification.success({
                    title: '兑换成功',
                }) : Notification.error({
                    title: '兑换失败',
                    content: res.message
                });

                loadUser();
            });
    }

    return (
        <div style={{
            margin: 20
        }}>
            <Tabs
                style={{
                    width: '100%',
                }}
                tabPosition="left"
                type='button'>
                <TabPane
                    tab={
                        <span>
                            账号余额
                        </span>
                    }
                    itemKey="1"
                >
                    <div style={{ padding: '0 24px' }}>
                        <Card
                            title={
                                <span style={{
                                    fontSize: 24
                                }}>
                                    账号钱包 {renderQuota(user.residualCredit, 2)}
                                </span>}
                            style={{
                                width: '100%',
                                textAlign: 'center',
                            }}>
                            <Divider>兑换码充值</Divider>
                            <Input value={code}
                                onChange={(value) => {
                                    setCode(value);
                                }}
                                size='large'
                                suffix={<Button type='warning' onClick={() => {
                                    useCode();
                                }}>兑换余额</Button>}
                                placeholder={'输入您的兑换码'} style={{
                                    marginTop: 8
                                }} >
                            </Input>

                            <div
                                onClick={() => {
                                    const rechargeAddress = InitSetting?.find(s => s.key === GeneralSetting.RechargeAddress)?.value;
                                    if (rechargeAddress) {
                                        window.open(rechargeAddress, '_blank');
                                    } else {
                                        Notification.error({
                                            title: '充值失败',
                                            content: '未设置充值地址'
                                        });
                                    }
                                }}
                                style={{
                                    marginTop: 8,
                                    cursor: 'pointer',
                                    color: 'var(--semi-color-text-2)',
                                    userSelect: 'none',
                                }}>
                                如何获取兑换码？
                            </div>
                            {
                                IsEnableAlipay() && <div>
                                    <Divider>支付宝充值</Divider>
                                    <Select
                                        value={product.id}
                                        onChange={(value) => {
                                            setProduct(products.find(x => x.id === value));
                                        }}
                                        optionList={products.map(x => {
                                            return {
                                                label: x.name,
                                                value: x.id
                                            }
                                        })}
                                        placeholder='选择充值金额'
                                    >
                                    </Select>
                                    <Button onClick={() => {
                                        alipayRecharge(product.id)
                                    }} style={{
                                        marginTop: 8
                                    }} block type="primary" >
                                        充值
                                    </Button>
                                </div>
                            }
                        </Card>

                    </div>
                </TabPane>
                <TabPane
                    tab={
                        <span>
                            修改个人信息
                        </span>
                    }
                    itemKey="2"
                >
                    <div style={{
                        padding: '0 24px',
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center'
                    }}>
                        <Avatar style={{
                            marginTop: 8
                        }} src={(!user.avatar || user.avatar === '') ? "/logo.png" : user.avatar} />
                        <Input placeholder={'输入您的头像地址'} style={{
                            marginTop: 8
                        }} value={user.avatar} onChange={value => setUser({
                            ...user,
                            avatar: value
                        })} >
                        </Input>
                        <Input placeholder={'输入您的新的邮箱地址'} style={{
                            marginTop: 8
                        }} value={user.email} onChange={value => setUser({
                            ...user,
                            email: value
                        })} >
                        </Input>
                        <Button style={{
                            marginTop: 8
                        }} onClick={() => handleSubmit()} block type="primary" htmlType="submit">
                            保存修改
                        </Button>
                    </div>
                </TabPane>
                <TabPane
                    tab={
                        <span>
                            修改登录密码
                        </span>
                    }
                    itemKey="3"
                >
                    <div style={{
                        padding: '0 24px',
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center'
                    }}>
                        <Input value={password}
                            type='password'
                            onChange={(value) => {
                                setPassword(value);
                            }}
                            placeholder={'输入您原有密码'} style={{
                                marginTop: 8
                            }} >
                        </Input>
                        <Input value={newPassword}
                            type='password'
                            onChange={(value) => {
                                setNewPassword(value);
                            }}
                            placeholder={'输入您的新密码'} style={{
                                marginTop: 8
                            }} >
                        </Input>
                        <Button style={{
                            marginTop: 8
                        }} onClick={() => onUpdatePassword()} block type="primary" htmlType="submit">
                            保存修改
                        </Button>
                    </div>
                </TabPane>
            </Tabs>
            <Modal
                visible={qrCode !== ''}
                title='支付宝充值'
                onClose={() => {
                    setQrCode('');
                }}
                footer={
                    <>
                        <Button
                            type='primary'
                            onClick={()=>{
                                setQrCode('');
                                loadProducts();
                                loadUser();
                            }}
                        >
                            我已支付
                        </Button>
                        <Button 
                        onClick={() => {
                            setQrCode('');
                        }} type='danger'>关闭</Button>
                    </>
                }
            >
                <QRCode
                    id="qrCode"
                    value={qrCode}
                    size={380} // 二维码的大小
                    fgColor="#000000" // 二维码的颜色
                    style={{
                        margin: 'auto'
                    }}
                    imageSettings={{ // 二维码中间的logo图片
                        src: 'logoUrl',
                        height: 100,
                        width: 100,
                        excavate: true, // 中间图片所在的位置是否镂空
                    }}
                />
            </Modal>
        </div>
    );
}
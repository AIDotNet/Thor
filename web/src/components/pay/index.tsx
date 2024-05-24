import { Button, Card, Divider, Input, Modal, Notification, Select, TabPane, Tabs, Tag } from "@douyinfe/semi-ui";
import { renderQuota } from "../../uitls/render";
import { useEffect, useState } from "react";
import { GeneralSetting, InitSetting, IsEnableAlipay } from "../../services/SettingService";
import { Use } from "../../services/RedeemCodeService";
import { getProduct, startPayload } from "../../services/ProductService";
import QRCode from "qrcode.react";

interface IPayProps {
    user: any
}
export default function Pay({
    user
}: IPayProps) {
    const [code, setCode] = useState('');
    const [product, setProduct] = useState({} as any);
    const [products, setProducts] = useState([] as any[]);
    const [qrCode, setQrCode] = useState('');

    useEffect(() => {
        loadProducts();
    }, []);

    function loadProducts() {
        getProduct()
            .then((res) => {
                setProducts(res.data);
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
            });
    }

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

    return <Card
        title={
            <span style={{
                fontSize: 24
            }}>
                账号钱包 <Tag size='large' color='amber'>{renderQuota(user.residualCredit, 2)}</Tag>
            </span>}
        style={{
            width: '100%',
            textAlign: 'center',
        }}>
        <Tabs type="button">
            {
                IsEnableAlipay() &&
                <TabPane tab="支付宝支付" itemKey="1">
                    <div>
                        <Select
                            style={{
                                width: '100%',
                                fontSize: 26,
                                marginTop: 8
                            }}
                            size="large"
                            value={product.id}
                            onChange={(value) => {
                                setProduct(products.find(x => x.id === value));
                            }}
                            optionList={products.map(x => {
                                return {
                                    label: <div style={{
                                        display: 'flex',
                                        justifyContent: 'space-between',
                                        alignItems: 'center',

                                    }}>
                                        <span>{x.name}</span>
                                        <Tag  color='blue' style={{
                                            fontSize: 16,
                                            marginLeft: 8,
                                            height: 32,
                                        }}>{x.price} 元</Tag>
                                        <Tag  color='green' style={{
                                            fontSize: 16,
                                            marginLeft: 8,
                                            height: 32,
                                        }}>{renderQuota(x.remainQuota,6)} 额度</Tag>
                                        <span style={{
                                            marginLeft: 8,
                                        }}>{x.description}</span>
                                    </div>,
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
                </TabPane>
            }
            <TabPane tab="兑换码" itemKey="2">
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

                <Tag color='blue' 
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
                    size='large'
                    style={{
                        marginTop: 8,
                        cursor: 'pointer',
                        color: 'var(--semi-color-text-2)',
                        userSelect: 'none',
                    }}>
                    如何获取兑换码？
                </Tag>
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
                        onClick={() => {
                            setQrCode('');
                            loadProducts();
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
    </Card>
}
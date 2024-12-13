import { useEffect, useState } from "react";
import { message, Card, Tabs, Button, Input, Badge } from 'antd';
import { GeneralSetting, InitSetting, IsEnableAlipay } from "../../services/SettingService";
import { Use } from "../../services/RedeemCodeService";
import { getProduct, startPayload } from "../../services/ProductService";
import QRCode from "qrcode.react";
import { renderQuota } from "../../utils/render";
import { GradientButton, Modal, Tag } from "@lobehub/ui";


interface IPayProps {
    user: any
}
export default function Pay({
    user
}: IPayProps) {
    const [code, setCode] = useState('');
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
        if (code === '') return message.error({
            content: '兑换码不能为空'
        });

        Use(code)
            .then((res) => {
                res.success ? message.success({
                    content: '兑换成功',
                }) : message.error({
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
                    message.error({
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
                账号钱包 <Tag color='amber'>{renderQuota(user.residualCredit, 2)}</Tag>
            </span>}
        style={{
            width: '100%',
            textAlign: 'center',
        }}>
        <Tabs type='card'
            items={[
                {
                    key: '1',
                    label: '支付宝支付',
                    children: IsEnableAlipay() == false ? <>
                        <div>
                            未启用支付宝支付
                        </div>
                    </> :
                        <div style={{
                            marginTop: 16,
                            marginBottom: 16,
                            overflow: 'auto',
                            height: 'calc(100vh - 440px)',
                        }}>
                            {
                                products.length > 0 ? (
                                    <div
                                        style={{
                                            display: 'flex', // 使用 flexbox 布局
                                            flexWrap: 'wrap', // 允许换行
                                            justifyContent: 'flex-start', // 从左到右排列
                                            gap: '16px', // 卡片之间的间距
                                        }}
                                    >
                                        {products.map((x) => {
                                            const originalPrice = (x.price * 1.1).toFixed(2); // 原价增加 10%

                                            return (
                                                <Badge.Ribbon
                                                    text="超值充值"
                                                    color="red"
                                                    placement="start"
                                                    key={x.id}
                                                    style={{
                                                        fontSize: 16,
                                                        fontWeight: 'bold',
                                                        background: 'linear-gradient(90deg, #ff4d4f, #ff7a45)',
                                                        color: '#fff',
                                                    }}
                                                >
                                                    <Card
                                                        style={{
                                                            borderRadius: 16,
                                                            padding: 16,
                                                            width: 320, // 固定卡片宽度
                                                            boxShadow: '0 8px 16px rgba(0,0,0,0.1)',
                                                            transition: 'transform 0.3s, box-shadow 0.3s',
                                                            cursor: 'pointer',
                                                        }}
                                                        hoverable
                                                        onMouseEnter={(e) => {
                                                            e.currentTarget.style.transform = 'scale(1.05)';
                                                            e.currentTarget.style.boxShadow = '0 12px 24px rgba(0,0,0,0.2)';
                                                        }}
                                                        onMouseLeave={(e) => {
                                                            e.currentTarget.style.transform = 'scale(1)';
                                                            e.currentTarget.style.boxShadow = '0 8px 16px rgba(0,0,0,0.1)';
                                                        }}
                                                    >
                                                        <div style={{ textAlign: 'center' }}>
                                                            {/* 产品名称 */}
                                                            <div
                                                                style={{
                                                                    fontWeight: 'bold',
                                                                    fontSize: 24,
                                                                    marginBottom: 12,
                                                                    color: '#333',
                                                                }}
                                                            >
                                                                {x.name}
                                                            </div>

                                                            {/* 产品价格和原价 */}
                                                            <div
                                                                style={{
                                                                    fontSize: 22,
                                                                    fontWeight: 'bold',
                                                                    marginBottom: 8,
                                                                    color: '#ff4d4f',
                                                                }}
                                                            >
                                                                <span>{x.price} 元</span>
                                                                <Tag
                                                                    color="green"
                                                                    style={{
                                                                        marginLeft: 8,
                                                                        fontSize: 14,
                                                                        borderRadius: 8,
                                                                        padding: '4px 8px',
                                                                    }}
                                                                >
                                                                    限时优惠
                                                                </Tag>
                                                            </div>
                                                            <div
                                                                style={{
                                                                    fontSize: 16,
                                                                    color: '#aaa',
                                                                    textDecoration: 'line-through',
                                                                    marginBottom: 12,
                                                                }}
                                                            >
                                                                原价：{originalPrice} 元
                                                            </div>

                                                            {/* 产品额度 */}
                                                            <div
                                                                style={{
                                                                    fontSize: 18,
                                                                    color: '#52c41a',
                                                                    marginBottom: 12,
                                                                }}
                                                            >
                                                                剩余额度：{x.remainQuota}
                                                            </div>

                                                            {/* 产品描述 */}
                                                            <div
                                                                style={{
                                                                    fontSize: 14,
                                                                    color: '#888',
                                                                    marginBottom: 16,
                                                                    lineHeight: 1.5,
                                                                }}
                                                            >
                                                                {x.description}
                                                            </div>
                                                            <GradientButton
                                                                onClick={() => {
                                                                    message.loading({
                                                                        content: '正在生成二维码，请稍后...',
                                                                        duration: 2,
                                                                    });
                                                                    alipayRecharge(x.id);
                                                                }}
                                                                size="large"
                                                                children="立即充值"
                                                                block
                                                                icon={<i className="anticon anticon-pay-circle" />}
                                                            ></GradientButton>
                                                        </div>
                                                    </Card>
                                                </Badge.Ribbon>
                                            );
                                        })}
                                    </div>
                                ) : (
                                    <div
                                        style={{
                                            display: 'flex', // 使用 flexbox 布局
                                            justifyContent: 'center', // 水平居中
                                            alignItems: 'center', // 垂直居中
                                            height: '300px', // 设置高度
                                            flexDirection: 'column', // 垂直排列内容
                                            color: '#888', // 提示文字颜色
                                            fontSize: '18px', // 提示文字大小
                                            textAlign: 'center', // 居中对齐文字
                                        }}
                                    >
                                        <i
                                            className="anticon anticon-frown"
                                            style={{
                                                fontSize: '48px', // 图标大小
                                                color: '#ccc', // 图标颜色
                                                marginBottom: '16px', // 图标与文字的间距
                                            }}
                                        />
                                        暂无产品
                                    </div>
                                )
                            }

                        </div>
                },
                {
                    key: '2',
                    label: '兑换码',
                    children: <>
                        <Input value={code}
                            onChange={(value) => {
                                setCode(value.target.value);
                            }}
                            size='large'
                            suffix={<Button onClick={() => {
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
                                    message.error({
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
                        </Tag></>
                }
            ]}
        >
        </Tabs>
        <Modal
            open={qrCode !== ''}
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
                        }} >关闭</Button>
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
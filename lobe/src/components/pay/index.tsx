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
    const [countdown, setCountdown] = useState(300); // 5分钟倒计时

    useEffect(() => {
        loadProducts();
    }, []);

    useEffect(() => {
        let timer: any = null;
        if (qrCode && countdown > 0) {
            timer = setInterval(() => {
                setCountdown(prev => prev - 1);
            }, 1000);
        } else if (countdown === 0) {
            setQrCode('');
            setCountdown(300);
        }
        return () => {
            if (timer) clearInterval(timer);
        };
    }, [qrCode, countdown]);

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
            <div style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'space-between',
                padding: '8px 0'
            }}>
                <span style={{
                    fontSize: 24,
                    fontWeight: 600,
                    background: 'linear-gradient(90deg, #1677ff, #4096ff)',
                    WebkitBackgroundClip: 'text',
                    WebkitTextFillColor: 'transparent'
                }}>
                    账号钱包
                </span>
                <div style={{
                    display: 'flex',
                    alignItems: 'center',
                    gap: '8px'
                }}>
                    <span style={{ fontSize: 16 }}>当前余额:</span>
                    <Tag color='amber' style={{
                        fontSize: 18,
                        padding: '4px 12px',
                        borderRadius: '16px',
                        boxShadow: '0 2px 8px rgba(0,0,0,0.1)'
                    }}>{renderQuota(user.residualCredit, 2)}</Tag>
                </div>
            </div>}
        style={{
            width: '100%',
            borderRadius: '16px',
            overflow: 'hidden',
            boxShadow: '0 4px 20px rgba(0,0,0,0.08)'
        }}>
        <Tabs type='card'
            style={{
                marginTop: '8px'
            }}
            items={[
                {
                    key: '1',
                    label: <span style={{ padding: '0 8px', display: 'flex', alignItems: 'center', gap: '8px' }}>
                        <i className="anticon anticon-alipay" style={{ fontSize: '18px', color: '#1677ff' }} />
                        支付宝支付
                    </span>,
                    children: IsEnableAlipay() == false ? 
                    <div style={{
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        justifyContent: 'center',
                        padding: '40px 0',
                        color: '#999'
                    }}>
                        <i className="anticon anticon-warning" style={{ fontSize: '48px', marginBottom: '16px' }} />
                        <span style={{ fontSize: '18px' }}>未启用支付宝支付</span>
                    </div> :
                    <div style={{
                        marginTop: 16,
                        marginBottom: 16,
                        overflow: 'auto',
                        height: 'calc(100vh - 440px)',
                        padding: '0 8px'
                    }}>
                        {
                            products.length > 0 ? (
                                <div
                                    style={{
                                        display: 'flex',
                                        flexWrap: 'wrap',
                                        justifyContent: 'center',
                                        gap: '24px',
                                    }}
                                >
                                    {products.map((x) => {
                                        const originalPrice = (x.price * 1.1).toFixed(2);
                                        const discount = Math.round((1 - x.price / parseFloat(originalPrice)) * 100);

                                        return (
                                            <Badge.Ribbon
                                                text={`省${discount}%`}
                                                color="red"
                                                placement="start"
                                                key={x.id}
                                                style={{
                                                    fontSize: 16,
                                                    fontWeight: 'bold',
                                                    background: 'linear-gradient(90deg, #ff4d4f, #ff7a45)',
                                                    borderRadius: '0 0 8px 0',
                                                    boxShadow: '0 2px 8px rgba(255,77,79,0.3)'
                                                }}
                                            >
                                                <Card
                                                    style={{
                                                        borderRadius: 16,
                                                        padding: 16,
                                                        width: 320,
                                                        boxShadow: '0 8px 24px rgba(0,0,0,0.12)',
                                                        transition: 'all 0.3s ease',
                                                        border: '1px solid #f0f0f0',
                                                        overflow: 'hidden',
                                                        position: 'relative'
                                                    }}
                                                    hoverable
                                                    onMouseEnter={(e) => {
                                                        e.currentTarget.style.transform = 'translateY(-8px)';
                                                        e.currentTarget.style.boxShadow = '0 16px 32px rgba(0,0,0,0.15)';
                                                        e.currentTarget.style.borderColor = '#1677ff20';
                                                    }}
                                                    onMouseLeave={(e) => {
                                                        e.currentTarget.style.transform = 'translateY(0)';
                                                        e.currentTarget.style.boxShadow = '0 8px 24px rgba(0,0,0,0.12)';
                                                        e.currentTarget.style.borderColor = '#f0f0f0';
                                                    }}
                                                >
                                                    <div style={{ 
                                                        position: 'absolute', 
                                                        top: 0, 
                                                        left: 0, 
                                                        right: 0, 
                                                        height: '6px', 
                                                        background: 'linear-gradient(90deg, #1677ff, #4096ff)' 
                                                    }} />
                                                    <div style={{ textAlign: 'center', paddingTop: '10px' }}>
                                                        {/* 产品名称 */}
                                                        <div
                                                            style={{
                                                                fontWeight: 'bold',
                                                                fontSize: 24,
                                                                marginBottom: 16,
                                                                color: '#111',
                                                            }}
                                                        >
                                                            {x.name}
                                                        </div>

                                                        {/* 产品价格和原价 */}
                                                        <div style={{
                                                            display: 'flex',
                                                            alignItems: 'baseline',
                                                            justifyContent: 'center',
                                                            marginBottom: 8
                                                        }}>
                                                            <span style={{
                                                                fontSize: 16,
                                                                color: '#ff4d4f',
                                                                fontWeight: 'bold'
                                                            }}>¥</span>
                                                            <span style={{
                                                                fontSize: 32,
                                                                fontWeight: 'bold',
                                                                color: '#ff4d4f',
                                                                lineHeight: 1
                                                            }}>{x.price}</span>
                                                            <Tag
                                                                color="green"
                                                                style={{
                                                                    marginLeft: 8,
                                                                    fontSize: 12,
                                                                    borderRadius: 12,
                                                                    padding: '2px 8px',
                                                                }}
                                                            >
                                                                限时特惠
                                                            </Tag>
                                                        </div>
                                                        <div
                                                            style={{
                                                                fontSize: 14,
                                                                color: '#aaa',
                                                                textDecoration: 'line-through',
                                                                marginBottom: 16,
                                                            }}
                                                        >
                                                            原价：¥{originalPrice}
                                                        </div>

                                                        {/* 分割线 */}
                                                        <div style={{
                                                            height: '1px',
                                                            background: 'linear-gradient(90deg, transparent, #f0f0f0, transparent)',
                                                            margin: '16px 0'
                                                        }} />

                                                        {/* 产品额度 */}
                                                        <div
                                                            style={{
                                                                fontSize: 18,
                                                                color: '#52c41a',
                                                                marginBottom: 16,
                                                                fontWeight: 500
                                                            }}
                                                        >
                                                            剩余额度：<span style={{ fontWeight: 'bold' }}>{x.remainQuota}</span>
                                                        </div>

                                                        {/* 产品描述 */}
                                                        <div
                                                            style={{
                                                                fontSize: 14,
                                                                color: '#666',
                                                                marginBottom: 24,
                                                                lineHeight: 1.6,
                                                                height: '44px',
                                                                overflow: 'hidden',
                                                                textOverflow: 'ellipsis',
                                                                display: '-webkit-box',
                                                                WebkitLineClamp: 2,
                                                                WebkitBoxOrient: 'vertical'
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
                                                                setCountdown(300); // 重置倒计时
                                                            }}
                                                            size="large"
                                                            block
                                                            style={{
                                                                height: '46px',
                                                                borderRadius: '23px',
                                                                fontSize: '16px',
                                                                fontWeight: 'bold',
                                                                boxShadow: '0 8px 16px rgba(22, 119, 255, 0.2)'
                                                            }}
                                                            icon={<i className="anticon anticon-alipay" style={{ fontSize: '20px' }} />}
                                                        >
                                                            立即充值
                                                        </GradientButton>
                                                    </div>
                                                </Card>
                                            </Badge.Ribbon>
                                        );
                                    })}
                                </div>
                            ) : (
                                <div
                                    style={{
                                        display: 'flex',
                                        justifyContent: 'center',
                                        alignItems: 'center',
                                        height: '300px',
                                        flexDirection: 'column',
                                        color: '#888',
                                        fontSize: '18px',
                                        textAlign: 'center',
                                    }}
                                >
                                    <i
                                        className="anticon anticon-inbox"
                                        style={{
                                            fontSize: '64px',
                                            color: '#ddd',
                                            marginBottom: '16px',
                                        }}
                                    />
                                    暂无可购买的产品
                                    <div style={{ fontSize: '14px', color: '#aaa', marginTop: '8px' }}>
                                        请稍后再来查看
                                    </div>
                                </div>
                            )
                        }
                    </div>
                },
                {
                    key: '2',
                    label: <span style={{ padding: '0 8px', display: 'flex', alignItems: 'center', gap: '8px' }}>
                        <i className="anticon anticon-gift" style={{ fontSize: '18px', color: '#1677ff' }} />
                        兑换码
                    </span>,
                    children: <div style={{ padding: '24px 16px' }}>
                        <div style={{ 
                            background: '#f5f5f5', 
                            borderRadius: '12px', 
                            padding: '16px', 
                            marginBottom: '16px',
                            border: '1px dashed #d9d9d9'
                        }}>
                            <div style={{ fontSize: '16px', marginBottom: '8px', color: '#333', fontWeight: 500 }}>
                                <i className="anticon anticon-info-circle" style={{ marginRight: '8px', color: '#1677ff' }} />
                                使用兑换码快速充值
                            </div>
                            <div style={{ fontSize: '14px', color: '#666' }}>
                                输入有效的兑换码，即可为您的账户充值相应的额度。
                            </div>
                        </div>
                        
                        <Input value={code}
                            onChange={(value) => {
                                setCode(value.target.value);
                            }}
                            size='large'
                            style={{
                                borderRadius: '8px',
                                height: '48px',
                                fontSize: '16px',
                                marginBottom: '16px'
                            }}
                            prefix={<i className="anticon anticon-barcode" style={{ color: '#1677ff', fontSize: '18px' }} />}
                            suffix={
                                <Button 
                                    type="primary"
                                    onClick={() => { useCode(); }}
                                    style={{
                                        borderRadius: '6px',
                                        height: '36px',
                                        fontWeight: 500
                                    }}
                                >
                                    立即兑换
                                </Button>
                            }
                            placeholder={'请输入您的兑换码'} 
                        />

                        <div style={{ textAlign: 'center', marginTop: '24px' }}>
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
                                    cursor: 'pointer',
                                    color: '#1677ff',
                                    userSelect: 'none',
                                    fontSize: '14px',
                                    padding: '6px 12px',
                                    borderRadius: '16px',
                                    background: '#e6f4ff',
                                    border: 'none',
                                    boxShadow: '0 2px 0 rgba(0,0,0,0.05)'
                                }}>
                                <i className="anticon anticon-question-circle" style={{ marginRight: '4px' }} />
                                如何获取兑换码？
                            </Tag>
                        </div>
                    </div>
                }
            ]}
        >
        </Tabs>
        <Modal
            open={qrCode !== ''}
            title={
                <div style={{ 
                    display: 'flex', 
                    alignItems: 'center', 
                    justifyContent: 'space-between',
                    borderBottom: '1px solid #f0f0f0',
                    paddingBottom: '12px'
                }}>
                    <span style={{ fontSize: '18px', fontWeight: 'bold' }}>
                        <i className="anticon anticon-alipay" style={{ color: '#1677ff', marginRight: '8px', fontSize: '20px' }} />
                        支付宝扫码支付
                    </span>
                    <Tag color="blue" style={{ marginLeft: '8px' }}>
                        {Math.floor(countdown / 60)}:{(countdown % 60).toString().padStart(2, '0')}
                    </Tag>
                </div>
            }
            onClose={() => {
                setQrCode('');
                setCountdown(300);
            }}
            footer={
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <div style={{ color: '#ff4d4f', fontSize: '14px' }}>
                        <i className="anticon anticon-clock-circle" style={{ marginRight: '4px' }} />
                        请在{Math.floor(countdown / 60)}分{countdown % 60}秒内完成支付
                    </div>
                    <div>
                        <Button
                            type='primary'
                            onClick={() => {
                                setQrCode('');
                                loadProducts();
                                setCountdown(300);
                            }}
                            style={{
                                marginRight: '8px',
                                borderRadius: '6px',
                                background: '#52c41a',
                                borderColor: '#52c41a'
                            }}
                        >
                            <i className="anticon anticon-check-circle" style={{ marginRight: '4px' }} />
                            我已完成支付
                        </Button>
                        <Button
                            onClick={() => {
                                setQrCode('');
                                setCountdown(300);
                            }}
                            style={{ borderRadius: '6px' }}
                        >
                            取消
                        </Button>
                    </div>
                </div>
            }
            style={{ maxWidth: '480px' }}
        >
            <div style={{ 
                display: 'flex', 
                flexDirection: 'column', 
                alignItems: 'center',
                padding: '16px 0'
            }}>
                <div style={{ 
                    background: '#f5f5f5', 
                    padding: '16px', 
                    borderRadius: '8px',
                    marginBottom: '16px',
                    width: '100%',
                    textAlign: 'center'
                }}>
                    <div style={{ fontSize: '14px', color: '#666', marginBottom: '8px' }}>
                        请使用支付宝扫描下方二维码完成支付
                    </div>
                    <div style={{ fontSize: '16px', color: '#ff4d4f', fontWeight: 'bold' }}>
                        二维码有效期5分钟，请尽快支付
                    </div>
                </div>
                
                <div style={{ 
                    padding: '16px', 
                    border: '1px solid #1677ff', 
                    borderRadius: '8px',
                    background: '#fff'
                }}>
                    <QRCode
                        id="qrCode"
                        value={qrCode}
                        size={300}
                        fgColor="#000000"
                        style={{
                            margin: 'auto'
                        }}
                        imageSettings={{
                            src: 'logoUrl',
                            height: 60,
                            width: 60,
                            excavate: true,
                        }}
                    />
                </div>
                
                <div style={{ 
                    marginTop: '16px', 
                    fontSize: '14px', 
                    color: '#666',
                    textAlign: 'center'
                }}>
                    <i className="anticon anticon-mobile" style={{ marginRight: '4px', color: '#1677ff' }} />
                    手机支付宝扫一扫，立即完成支付
                </div>
            </div>
        </Modal>
    </Card>
}
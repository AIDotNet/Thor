import { useEffect, useState } from "react";
import { message, Card, Tabs, Button, Input, Badge, Typography, Space, Empty, Alert, theme } from 'antd';
import { GeneralSetting, InitSetting, IsEnableAlipay } from "../../services/SettingService";
import { Use } from "../../services/RedeemCodeService";
import { getProduct, startPayload } from "../../services/ProductService";
import QRCode from "qrcode.react";
import { renderQuota } from "../../utils/render";
import { GradientButton, Modal, Tag } from "@lobehub/ui";
import { AlipayOutlined, BarcodeOutlined, CheckCircleOutlined, ClockCircleOutlined, GiftOutlined, InfoCircleOutlined, InboxOutlined, MobileOutlined, QuestionCircleOutlined, WarningOutlined } from "@ant-design/icons";
import { useTranslation } from 'react-i18next';

const { Title, Text, Paragraph } = Typography;
const { useToken } = theme;

interface IPayProps {
    user: any
}

export default function Pay({
    user
}: IPayProps) {
    const { t } = useTranslation();
    const { token } = useToken();
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
            content: t('payment.emptyRedeemCode')
        });

        Use(code)
            .then((res) => {
                res.success ? message.success({
                    content: t('payment.redeemSuccess')
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

    const cardStyle = {
        width: '100%',
        borderRadius: token.borderRadiusLG,
        overflow: 'hidden',
        boxShadow: `0 4px 20px ${token.colorBgElevated}`
    };

    const headerGradient = {
        background: `linear-gradient(90deg, ${token.colorPrimary}, ${token.colorPrimaryBg})`,
        WebkitBackgroundClip: 'text',
        WebkitTextFillColor: 'transparent'
    };

    const balanceTagStyle = {
        fontSize: 18,
        padding: '4px 12px',
        borderRadius: token.borderRadiusLG,
        boxShadow: `0 2px 8px ${token.colorBgContainer}`
    };

    return (
        <Card
            title={
                <div style={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'space-between',
                    padding: '8px 0'
                }}>
                    <Title level={4} style={{ margin: 0, ...headerGradient }}>
                        {t('payment.wallet')}
                    </Title>
                    <Space>
                        <Text style={{ fontSize: 16 }}>{t('payment.currentBalance')}:</Text>
                        <Tag color='amber' style={balanceTagStyle}>
                            {renderQuota(user.residualCredit, 2)}
                        </Tag>
                    </Space>
                </div>}
            style={cardStyle}
        >
            <Tabs 
                type='card'
                style={{ marginTop: '8px' }}
                items={[
                    {
                        key: '1',
                        label: (
                            <Space size={8}>
                                <AlipayOutlined style={{ fontSize: '18px', color: token.colorPrimary }} />
                                {t('payment.alipayPayment')}
                            </Space>
                        ),
                        children: IsEnableAlipay() === false ? 
                            <div style={{
                                display: 'flex',
                                flexDirection: 'column',
                                alignItems: 'center',
                                justifyContent: 'center',
                                padding: '40px 0',
                            }}>
                                <Empty
                                    image={<WarningOutlined style={{ fontSize: '48px', color: token.colorTextQuaternary }} />}
                                    description={<Text type="secondary" style={{ fontSize: '18px' }}>{t('payment.alipayDisabled')}</Text>}
                                />
                            </div> :
                            <div style={{
                                marginTop: 16,
                                marginBottom: 16,
                                overflow: 'auto',
                                height: 'calc(100vh - 440px)',
                                padding: '0 8px'
                            }}>
                                {products.length > 0 ? (
                                    <div
                                        style={{
                                            display: 'grid',
                                            gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))',
                                            gap: '24px',
                                            justifyContent: 'center',
                                        }}
                                    >
                                        {products.map((x) => {
                                            const originalPrice = (x.price * 1.1).toFixed(2);
                                            const discount = Math.round((1 - x.price / parseFloat(originalPrice)) * 100);

                                            return (
                                                <Badge.Ribbon
                                                    text={`${t('payment.save')}${discount}%`}
                                                    color="red"
                                                    placement="start"
                                                    key={x.id}
                                                    style={{
                                                        fontSize: 16,
                                                        fontWeight: 'bold',
                                                        background: `linear-gradient(90deg, ${token.colorError}, ${token.colorErrorBg})`,
                                                        borderRadius: '0 0 8px 0',
                                                        boxShadow: `0 2px 8px ${token.colorBgContainer}`
                                                    }}
                                                >
                                                    <Card
                                                        style={{
                                                            borderRadius: token.borderRadiusLG,
                                                            padding: 16,
                                                            width: '100%',
                                                            boxShadow: `0 8px 24px ${token.colorBgElevated}`,
                                                            transition: 'all 0.3s ease',
                                                            border: `1px solid ${token.colorBorderSecondary}`,
                                                            overflow: 'hidden',
                                                            position: 'relative'
                                                        }}
                                                        hoverable
                                                        onMouseEnter={(e) => {
                                                            e.currentTarget.style.transform = 'translateY(-8px)';
                                                            e.currentTarget.style.boxShadow = `0 16px 32px ${token.colorBgElevated}`;
                                                            e.currentTarget.style.borderColor = token.colorPrimaryBgHover;
                                                        }}
                                                        onMouseLeave={(e) => {
                                                            e.currentTarget.style.transform = 'translateY(0)';
                                                            e.currentTarget.style.boxShadow = `0 8px 24px ${token.colorBgElevated}`;
                                                            e.currentTarget.style.borderColor = token.colorBorderSecondary;
                                                        }}
                                                    >
                                                        <div style={{ 
                                                            position: 'absolute', 
                                                            top: 0, 
                                                            left: 0, 
                                                            right: 0, 
                                                            height: '6px', 
                                                            background: `linear-gradient(90deg, ${token.colorPrimary}, ${token.colorPrimaryBg})` 
                                                        }} />
                                                        <div style={{ textAlign: 'center', paddingTop: '10px' }}>
                                                            {/* 产品名称 */}
                                                            <Title level={4} style={{ marginBottom: 16 }}>
                                                                {x.name}
                                                            </Title>

                                                            {/* 产品价格和原价 */}
                                                            <div style={{
                                                                display: 'flex',
                                                                alignItems: 'baseline',
                                                                justifyContent: 'center',
                                                                marginBottom: 8
                                                            }}>
                                                                <Text style={{
                                                                    fontSize: 16,
                                                                    color: token.colorError,
                                                                    fontWeight: 'bold'
                                                                }}>¥</Text>
                                                                <Text style={{
                                                                    fontSize: 32,
                                                                    fontWeight: 'bold',
                                                                    color: token.colorError,
                                                                    lineHeight: 1
                                                                }}>{x.price}</Text>
                                                                <Tag
                                                                    color="success"
                                                                    style={{
                                                                        marginLeft: 8,
                                                                        fontSize: 12,
                                                                        borderRadius: 12,
                                                                        padding: '2px 8px',
                                                                    }}
                                                                >
                                                                    {t('payment.limitedOffer')}
                                                                </Tag>
                                                            </div>
                                                            <Text
                                                                type="secondary"
                                                                style={{
                                                                    fontSize: 14,
                                                                    textDecoration: 'line-through',
                                                                    marginBottom: 16,
                                                                    display: 'block'
                                                                }}
                                                            >
                                                                {t('payment.originalPrice')}：¥{originalPrice}
                                                            </Text>

                                                            {/* 分割线 */}
                                                            <div style={{
                                                                height: '1px',
                                                                background: `linear-gradient(90deg, transparent, ${token.colorBorderSecondary}, transparent)`,
                                                                margin: '16px 0'
                                                            }} />

                                                            {/* 产品额度 */}
                                                            <Text
                                                                style={{
                                                                    fontSize: 18,
                                                                    color: token.colorSuccess,
                                                                    marginBottom: 16,
                                                                    fontWeight: 500,
                                                                    display: 'block'
                                                                }}
                                                            >
                                                                {t('payment.remainingQuota')}：<span style={{ fontWeight: 'bold' }}>{x.remainQuota}</span>
                                                            </Text>

                                                            {/* 产品描述 */}
                                                            <Paragraph
                                                                type="secondary"
                                                                ellipsis={{ rows: 2 }}
                                                                style={{
                                                                    fontSize: 14,
                                                                    marginBottom: 24,
                                                                    lineHeight: 1.6,
                                                                    height: '44px'
                                                                }}
                                                            >
                                                                {x.description}
                                                            </Paragraph>
                                                            <GradientButton
                                                                onClick={() => {
                                                                    message.loading({
                                                                        content: t('payment.generatingQRCode'),
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
                                                                    boxShadow: `0 8px 16px rgba(22, 119, 255, 0.2)`
                                                                }}
                                                                icon={<AlipayOutlined style={{ fontSize: '20px' }} />}
                                                            >
                                                                {t('payment.rechargeNow')}
                                                            </GradientButton>
                                                        </div>
                                                    </Card>
                                                </Badge.Ribbon>
                                            );
                                        })}
                                    </div>
                                ) : (
                                    <Empty
                                        image={<InboxOutlined style={{ fontSize: '64px', color: token.colorBorderSecondary }} />}
                                        description={
                                            <>
                                                <Text style={{ fontSize: '18px', color: token.colorTextSecondary }}>
                                                    {t('payment.noProducts')}
                                                </Text>
                                                <div style={{ fontSize: '14px', color: token.colorTextQuaternary, marginTop: '8px' }}>
                                                    {t('payment.checkLater')}
                                                </div>
                                            </>
                                        }
                                    />
                                )}
                            </div>
                    },
                    {
                        key: '2',
                        label: (
                            <Space size={8}>
                                <GiftOutlined style={{ fontSize: '18px', color: token.colorPrimary }} />
                                {t('payment.redeemCode')}
                            </Space>
                        ),
                        children: (
                            <div style={{ padding: '24px 16px' }}>
                                <Alert
                                    icon={<InfoCircleOutlined style={{ color: token.colorPrimary }} />}
                                    message={
                                        <Text strong style={{ fontSize: '16px' }}>
                                            {t('payment.redeemCodeInfo')}
                                        </Text>
                                    }
                                    description={t('payment.redeemCodeDescription')}
                                    type="info"
                                    showIcon
                                    style={{ 
                                        marginBottom: '16px',
                                        borderRadius: token.borderRadiusLG,
                                        background: token.colorInfoBg,
                                    }}
                                />
                                
                                <Input 
                                    value={code}
                                    onChange={(value) => {
                                        setCode(value.target.value);
                                    }}
                                    size='large'
                                    style={{
                                        borderRadius: token.borderRadiusSM,
                                        height: '48px',
                                        fontSize: '16px',
                                        marginBottom: '16px'
                                    }}
                                    prefix={<BarcodeOutlined style={{ color: token.colorPrimary, fontSize: '18px' }} />}
                                    suffix={
                                        <Button 
                                            type="primary"
                                            onClick={() => { useCode(); }}
                                            style={{
                                                borderRadius: token.borderRadiusSM,
                                                height: '36px',
                                                fontWeight: 500
                                            }}
                                        >
                                            {t('payment.redeemNow')}
                                        </Button>
                                    }
                                    placeholder={t('payment.enterRedeemCode')} 
                                />

                                <div style={{ textAlign: 'center', marginTop: '24px' }}>
                                    <Tag color='blue'
                                        onClick={() => {
                                            const rechargeAddress = InitSetting?.find(s => s.key === GeneralSetting.RechargeAddress)?.value;
                                            if (rechargeAddress) {
                                                window.open(rechargeAddress, '_blank');
                                            } else {
                                                message.error({
                                                    content: t('payment.noRechargeAddress')
                                                });
                                            }
                                        }}
                                        style={{
                                            cursor: 'pointer',
                                            color: token.colorPrimary,
                                            userSelect: 'none',
                                            fontSize: '14px',
                                            padding: '6px 12px',
                                            borderRadius: token.borderRadiusLG,
                                            background: token.colorPrimaryBg,
                                            border: 'none',
                                            boxShadow: '0 2px 0 rgba(0,0,0,0.05)'
                                        }}>
                                        <QuestionCircleOutlined style={{ marginRight: '4px' }} />
                                        {t('payment.howToGetRedeemCode')}
                                    </Tag>
                                </div>
                            </div>
                        )
                    }
                ]}
            />
            <Modal
                open={qrCode !== ''}
                title={
                    <div style={{ 
                        display: 'flex', 
                        alignItems: 'center', 
                        justifyContent: 'space-between',
                        borderBottom: `1px solid ${token.colorBorderSecondary}`,
                        paddingBottom: '12px'
                    }}>
                        <Space align="center">
                            <AlipayOutlined style={{ color: token.colorPrimary, fontSize: '20px' }} />
                            <Text strong style={{ fontSize: '18px' }}>
                                {t('payment.scanQRCode')}
                            </Text>
                        </Space>
                        <Tag color="blue" style={{ marginLeft: '8px' }}>
                            {Math.floor(countdown / 60)}:{(countdown % 60).toString().padStart(2, '0')}
                        </Tag>
                    </div>
                }
                onCancel={() => {
                    setQrCode('');
                    setCountdown(300);
                }}
                footer={
                    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                        <div style={{ color: token.colorError, fontSize: '14px' }}>
                            <ClockCircleOutlined style={{ marginRight: '4px' }} />
                            {t('payment.timeRemaining', { minutes: Math.floor(countdown / 60), seconds: countdown % 60 })}
                        </div>
                        <Space>
                            <Button
                                type='primary'
                                onClick={() => {
                                    setQrCode('');
                                    loadProducts();
                                    setCountdown(300);
                                }}
                                style={{
                                    borderRadius: token.borderRadiusSM,
                                    background: token.colorSuccess,
                                    borderColor: token.colorSuccess
                                }}
                                icon={<CheckCircleOutlined />}
                            >
                                {t('payment.paymentCompleted')}
                            </Button>
                            <Button
                                onClick={() => {
                                    setQrCode('');
                                    setCountdown(300);
                                }}
                                style={{ borderRadius: token.borderRadiusSM }}
                            >
                                {t('payment.cancel')}
                            </Button>
                        </Space>
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
                    <Alert
                        message={
                            <Text strong type="danger" style={{ fontSize: '16px' }}>
                                {t('payment.qrCodeExpiry')}
                            </Text>
                        }
                        description={t('payment.scanInstruction')}
                        type="info"
                        showIcon
                        style={{ 
                            marginBottom: '16px',
                            width: '100%',
                            textAlign: 'center',
                            borderRadius: token.borderRadiusSM,
                        }}
                    />
                    
                    <div style={{ 
                        padding: '16px', 
                        border: `1px solid ${token.colorPrimary}`, 
                        borderRadius: token.borderRadiusSM,
                        background: token.colorBgContainer
                    }}>
                        <QRCode
                            id="qrCode"
                            value={qrCode}
                            size={280}
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
                        textAlign: 'center'
                    }}>
                        <Space>
                            <MobileOutlined style={{ color: token.colorPrimary }} />
                            <Text type="secondary">{t('payment.mobilePayment')}</Text>
                        </Space>
                    </div>
                </div>
            </Modal>
        </Card>
    );
}
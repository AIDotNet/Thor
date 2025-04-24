import {theme, Button, Card, Layout, Space, Typography, Row, Col, Statistic } from 'antd';
import {
    GithubOutlined,
    RocketOutlined,
    ApiOutlined,
    ThunderboltOutlined,
    TeamOutlined,
    DollarOutlined,
    AppstoreOutlined,
    CheckCircleOutlined,
} from '@ant-design/icons';
import './styles.css';
import styled from 'styled-components';
import { createStyles } from 'antd-style';
import { useNavigate } from 'react-router-dom';
import { motion } from 'framer-motion'; // 引入Framer Motion动画库
import { useInView } from 'react-intersection-observer'; // 引入滚动检测
import { useState, useEffect } from 'react';
import { modelHot } from '../../../services/LoggerService';
import { BarList, DonutChart } from '@lobehub/charts';
import { getIconByName } from '../../../utils/iconutils';
import { useTranslation } from 'react-i18next';

const { Content } = Layout;
const { Title, Paragraph, Text } = Typography;

// 创建动画组件
const MotionCard = motion(Card);
const MotionTitle = motion(Title);
const MotionParagraph = motion(Paragraph);
const MotionButton = motion(Button);
const MotionRow = motion(Row);
const MotionCol = motion(Col);

// 使用 antd-style 创建样式
const useStyles = createStyles(({ token, css }) => ({
    desktopMenu: css`
    display: none;
    @media (min-width: 768px) {
      display: block;
    }
  `,
    mobileMenuButton: css`
    color: ${token.colorTextLightSolid};
    margin-left: 8px;
    display: block;
    @media (min-width: 768px) {
      display: none;
    }
  `,
    featureCard: css`
    height: 100%;
    transition: all 0.3s;
    &:hover {
      box-shadow: 0 4px 12px rgba(0,0,0,0.1);
    }
  `,
    heroSection: css`
    position: relative;
    padding: 60px 0;
    background: linear-gradient(135deg, #141414 0%, #1f1f1f 100%);
    color: ${token.colorTextLightSolid};
    overflow: hidden;
  `,
    heroBackground: css`
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    overflow: hidden;
    z-index: 0;
  `,
    heroContainer: css`
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 24px;
    display: flex;
    flex-direction: column;
    align-items: center;
    position: relative;
    z-index: 1;
    @media (min-width: 768px) {
      flex-direction: row;
      align-items: center;
    }
  `,
    heroContent: css`
    margin-bottom: 40px;
    @media (min-width: 768px) {
      width: 50%;
      margin-bottom: 0;
      padding-right: 24px;
    }
  `,
    heroImage: css`
    @media (min-width: 768px) {
      width: 50%;
    }
  `,
    heroTitle: css`
    color: ${token.colorTextLightSolid} !important;
    font-size: 48px;
    font-weight: bold;
    margin-bottom: 24px;
  `,
    heroButtons: css`
    display: flex;
    flex-direction: column;
    @media (min-width: 576px) {
      flex-direction: row;
    }
  `,
    primaryButton: css`
    background: linear-gradient(90deg, ${token.colorPrimary} 0%, ${token.colorPrimaryActive} 100%);
    border: none;
    height: 48px;
    padding: 0 24px;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(24, 144, 255, 0.3);
    margin-bottom: 16px;
    @media (min-width: 576px) {
      margin-bottom: 0;
    }
  `,
    // ... 其他样式
}));

// 添加动画版的 FeatureCard 组件
const MotionFeatureCard = motion(styled(Card)`
  height: 100%;
  transition: all 0.3s;
  &:hover {
    box-shadow: 0 4px 12px rgba(0,0,0,0.1);
  }
  border-top: 3px solid ${props => props.color || 'transparent'};
`);

// 添加动画版的项目卡片
const MotionProjectCard = motion(styled(Card)`
  height: 100%;
  transition: all 0.3s;
  &:hover {
    box-shadow: 0 4px 12px rgba(0,0,0,0.1);
  }
`);

// 优化 AnimatedBackground 组件，使用更高效的动画方式
const AnimatedBackground = () => {
  return (
    <>
      <motion.div 
        style={{
          position: 'absolute',
          width: 300,
          height: 300,
          borderRadius: '50%',
          background: '#1890ff',
          opacity: 0.1,
          top: -50,
          right: -50,
          filter: 'blur(40px)'
        }}
        initial={{ x: 0, y: 0 }}
        animate={{
          x: [0, 20, 0],
          y: [0, 15, 0],
        }}
        transition={{
          duration: 8,
          repeat: Infinity,
          ease: "easeInOut",
          repeatType: "reverse"
        }}
      />
      <motion.div 
        style={{
          position: 'absolute',
          width: 200,
          height: 200,
          borderRadius: '50%',
          background: '#722ed1',
          opacity: 0.1,
          bottom: 50,
          left: 100,
          filter: 'blur(30px)'
        }}
        initial={{ x: 0, y: 0 }}
        animate={{
          x: [0, -20, 0],
          y: [0, 20, 0],
        }}
        transition={{
          duration: 10,
          repeat: Infinity,
          ease: "easeInOut",
          repeatType: "reverse"
        }}
      />
      <motion.div 
        style={{
          position: 'absolute',
          width: 150,
          height: 150,
          borderRadius: '50%',
          background: '#52c41a',
          opacity: 0.08,
          bottom: -30,
          right: 100,
          filter: 'blur(25px)'
        }}
        initial={{ x: 0, y: 0 }}
        animate={{
          x: [0, 15, 0],
          y: [0, -15, 0],
        }}
        transition={{
          duration: 9,
          repeat: Infinity,
          ease: "easeInOut",
          repeatType: "reverse"
        }}
      />
    </>
  );
};


// 优化 CountUpStatistic 组件，提高性能
const CountUpStatistic = ({ title, value, suffix, valueStyle }: { title: string, value: string, suffix: string, valueStyle: React.CSSProperties }) => {
  const [count, setCount] = useState(0);
  const [ref, inView] = useInView({
    triggerOnce: true,
    threshold: 0.1,
  });

  useEffect(() => {
    if (inView) {
      const numericValue = parseInt(value.replace(/[^0-9]/g, ''));
      let startTime: number | undefined;
      const duration = 1500; // 缩短动画时间提高响应感
      
      const step = (timestamp: number) => {
        if (!startTime) startTime = timestamp;
        const progress = Math.min((timestamp - startTime) / duration, 1);
        // 使用缓动函数使动画更自然
        const easedProgress = easeOutQuart(progress);
        setCount(Math.floor(easedProgress * numericValue));
        
        if (progress < 1) {
          window.requestAnimationFrame(step);
        }
      };
      
      window.requestAnimationFrame(step);
    }
  }, [inView, value]);

  // 缓动函数，使数字增长更自然
  const easeOutQuart = (x: number): number => {
    return 1 - Math.pow(1 - x, 4);
  };

  // 提取数字后面的文本（如"+"）
  const suffixText = value.replace(/[0-9]/g, '');

  return (
    <motion.div 
      ref={ref}
      initial={{ scale: 0.9, opacity: 0 }}
      animate={inView ? { scale: 1, opacity: 1 } : { scale: 0.9, opacity: 0 }}
      transition={{ duration: 0.5 }}
    >
      <Statistic
        title={title}
        value={inView ? count : 0}
        suffix={suffix}
        valueStyle={valueStyle}
        formatter={(value) => `${value}${suffixText}`}
      />
    </motion.div>
  );
};

const ThorWebsite = () => {
    const { styles } = useStyles();
    const navigate = useNavigate();
    const { token } = theme.useToken();
    const { t } = useTranslation();
    
    // 创建滚动检测钩子
    const [featuresRef, featuresInView] = useInView({
      triggerOnce: true,
      threshold: 0.1,
    });
    
    const [statsRef, statsInView] = useInView({
      triggerOnce: true,
      threshold: 0.1,
    });
    
    const [projectsRef, projectsInView] = useInView({
      triggerOnce: true,
      threshold: 0.1,
    });
    
    // 添加模型热度数据状态
    const [modelHotData, setModelHotData] = useState<{model: string, percentage: number}[]>([]);
    const [modelHotLoading, setModelHotLoading] = useState(true);
    
    // 添加获取模型热度数据的函数
    useEffect(() => {
        const fetchModelHotData = async () => {
            try {
                setModelHotLoading(true);
                const response = await modelHot();
                if (response && response.data) {
                    setModelHotData(response.data);
                }
            } catch (error) {
                console.error('获取模型热度数据失败:', error);
            } finally {
                setModelHotLoading(false);
            }
        };
        
        fetchModelHotData();
    }, []);
    
    // 定义各种动画变体
    const containerVariants = {
      hidden: { opacity: 0 },
      visible: {
        opacity: 1,
        transition: {
          staggerChildren: 0.1
        }
      }
    };
    
    const itemVariants = {
      hidden: { opacity: 0, y: 20 },
      visible: { 
        opacity: 1, 
        y: 0,
        transition: {
          duration: 0.6,
          ease: "easeOut"
        }
      }
    };
    
    const cardVariants = {
      hidden: { opacity: 0, y: 30 },
      visible: { 
        opacity: 1, 
        y: 0,
        transition: {
          duration: 0.5,
          ease: "easeOut"
        }
      }
    };
    
    // 添加模型热度引用
    const [modelHotRef, modelHotInView] = useInView({
      triggerOnce: true,
      threshold: 0.1,
    });
    
    return (
        <Content>
            <div className={styles.heroSection}>
                <div className={styles.heroBackground}>
                    <AnimatedBackground />
                </div>

                <div className={styles.heroContainer}>
                    <motion.div 
                      className={styles.heroContent}
                      initial={{ opacity: 0, x: -30 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.8, ease: "easeOut" }}
                    >
                        <MotionTitle 
                          className={styles.heroTitle}
                          initial={{ opacity: 0, y: -20 }}
                          animate={{ opacity: 1, y: 0 }}
                          transition={{ duration: 0.6, delay: 0.2 }}
                        >
                            {t('welcome.title')}
                        </MotionTitle>
                        <MotionParagraph 
                          style={{ color: '#d9d9d9', fontSize: 18, marginBottom: 24 }}
                          initial={{ opacity: 0 }}
                          animate={{ opacity: 1 }}
                          transition={{ duration: 0.6, delay: 0.4 }}
                        >
                            {t('welcome.description')} <Text style={{ color: '#1890ff', fontWeight: 'bold' }}>68+</Text>
                        </MotionParagraph>
                        <MotionParagraph 
                          style={{ color: '#d9d9d9', fontSize: 18, marginBottom: 32 }}
                          initial={{ opacity: 0 }}
                          animate={{ opacity: 1 }}
                          transition={{ duration: 0.6, delay: 0.5 }}
                        >
                            —— {t('welcome.tagline')}
                        </MotionParagraph>
                        <Space size="large" className={styles.heroButtons}>
                            <MotionButton
                                type="primary"
                                size="large"
                                onClick={() => {
                                    navigate('/panel')
                                }}
                                className={styles.primaryButton}
                                initial={{ opacity: 0, scale: 0.9 }}
                                animate={{ opacity: 1, scale: 1 }}
                                transition={{ duration: 0.5, delay: 0.7 }}
                                whileHover={{ scale: 1.05 }}
                                whileTap={{ scale: 0.95 }}
                            >
                                <RocketOutlined /> {t('welcome.startNow')}
                            </MotionButton>
                            <MotionButton
                                size="large"
                                onClick={() => {
                                    window.open('https://github.com/AIDotNet/Thor', '_blank')
                                }}
                                style={{
                                    borderColor: '#434343',
                                    height: 48,
                                    padding: '0 24px',
                                    borderRadius: 8
                                }}
                                initial={{ opacity: 0, scale: 0.9 }}
                                animate={{ opacity: 1, scale: 1 }}
                                transition={{ duration: 0.5, delay: 0.8 }}
                                whileHover={{ scale: 1.05 }}
                                whileTap={{ scale: 0.95 }}
                            >
                                <GithubOutlined /> {t('welcome.giveStar')}
                            </MotionButton>
                        </Space>
                    </motion.div>
                    <motion.div 
                      className={styles.heroImage}
                      initial={{ opacity: 0, x: 30 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.8, delay: 0.3, ease: "easeOut" }}
                    >
                        <MotionCard
                            className="tilted-card"
                            style={{
                                width: '100%',
                                background: 'linear-gradient(135deg, #1f1f1f 0%, #141414 100%)',
                                borderColor: '#434343',
                                borderRadius: 16,
                                boxShadow: '0 20px 25px -5px rgba(0, 0, 0, 0.3)'
                            }}
                            whileHover={{ 
                              y: -10,
                              boxShadow: '0 30px 35px -5px rgba(0, 0, 0, 0.4)'
                            }}
                            transition={{ duration: 0.3 }}
                        >
                            <Title level={3} style={{ color: 'white' }}>{t('welcome.community.title')}</Title>
                            <Paragraph style={{ color: '#d9d9d9', marginBottom: 24 }}>
                                {t('welcome.community.description')}
                            </Paragraph>
                            <Space direction="vertical" size="large" style={{ width: '100%' }}>
                                <motion.div 
                                  style={{ display: 'flex', alignItems: 'flex-start' }}
                                  initial={{ opacity: 0, x: 20 }}
                                  animate={{ opacity: 1, x: 0 }}
                                  transition={{ duration: 0.5, delay: 0.5 }}
                                >
                                    <DollarOutlined style={{ color: '#1890ff', fontSize: 20, marginTop: 4, marginRight: 16 }} />
                                    <div>
                                        <Text strong style={{ color: 'white', fontSize: 16 }}>{t('welcome.community.payPerUse.title')}</Text>
                                        <Paragraph style={{ color: '#8c8c8c', marginBottom: 0 }}>
                                            {t('welcome.community.payPerUse.description')}
                                        </Paragraph>
                                    </div>
                                </motion.div>
                                <motion.div 
                                  style={{ display: 'flex', alignItems: 'flex-start' }}
                                  initial={{ opacity: 0, x: 20 }}
                                  animate={{ opacity: 1, x: 0 }}
                                  transition={{ duration: 0.5, delay: 0.6 }}
                                >
                                    <AppstoreOutlined style={{ color: '#1890ff', fontSize: 20, marginTop: 4, marginRight: 16 }} />
                                    <div>
                                        <Text strong style={{ color: 'white', fontSize: 16 }}>{t('welcome.community.appSupport.title')}</Text>
                                        <Paragraph style={{ color: '#8c8c8c', marginBottom: 0 }}>
                                            {t('welcome.community.appSupport.description')}
                                        </Paragraph>
                                    </div>
                                </motion.div>
                                <motion.div 
                                  style={{ display: 'flex', alignItems: 'flex-start' }}
                                  initial={{ opacity: 0, x: 20 }}
                                  animate={{ opacity: 1, x: 0 }}
                                  transition={{ duration: 0.5, delay: 0.7 }}
                                >
                                    <CheckCircleOutlined style={{ color: '#1890ff', fontSize: 20, marginTop: 4, marginRight: 16 }} />
                                    <div>
                                        <Text strong style={{ color: 'white', fontSize: 16 }}>{t('welcome.community.transparency.title')}</Text>
                                        <Paragraph style={{ color: '#8c8c8c', marginBottom: 0 }}>
                                            {t('welcome.community.transparency.description')}
                                        </Paragraph>
                                    </div>
                                </motion.div>
                            </Space>
                        </MotionCard>
                    </motion.div>
                </div>
            </div>

            {/* Stats Section */}
            <div ref={statsRef} style={{ padding: '48px 0', }}>
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px' }}>
                    <MotionRow 
                      gutter={[24, 24]} 
                      justify="center"
                      variants={containerVariants}
                      initial="hidden"
                      animate={statsInView ? "visible" : "hidden"}
                    >
                        <MotionCol xs={12} md={6} variants={itemVariants}>
                            <MotionCard 
                              bordered={false} 
                              style={{ textAlign: 'center', height: '100%' }}
                              whileHover={{ y: -5, boxShadow: '0 10px 20px rgba(0,0,0,0.1)' }}
                            >
                                <CountUpStatistic
                                    title={t('welcome.statistics.models')}
                                    value="200+"
                                    suffix="个"
                                    valueStyle={{ color: '#1890ff', fontWeight: 'bold' }}
                                />
                            </MotionCard>
                        </MotionCol>
                        <MotionCol xs={12} md={6} variants={itemVariants}>
                            <MotionCard 
                              bordered={false} 
                              style={{ textAlign: 'center', height: '100%' }}
                              whileHover={{ y: -5, boxShadow: '0 10px 20px rgba(0,0,0,0.1)' }}
                            >
                                <CountUpStatistic
                                    title={t('welcome.statistics.users')}
                                    value="8000+"
                                    suffix="人"
                                    valueStyle={{ color: '#1890ff', fontWeight: 'bold' }}
                                />
                            </MotionCard>
                        </MotionCol>
                        <MotionCol xs={12} md={6} variants={itemVariants}>
                            <MotionCard 
                              bordered={false} 
                              style={{ textAlign: 'center', height: '100%' }}
                              whileHover={{ y: -5, boxShadow: '0 10px 20px rgba(0,0,0,0.1)' }}
                            >
                                <CountUpStatistic
                                    title={t('welcome.statistics.requests')}
                                    value="1M+"
                                    suffix="次"
                                    valueStyle={{ color: '#1890ff', fontWeight: 'bold' }}
                                />
                            </MotionCard>
                        </MotionCol>
                        <MotionCol xs={12} md={6} variants={itemVariants}>
                            <MotionCard 
                              bordered={false} 
                              style={{ textAlign: 'center', height: '100%' }}
                              whileHover={{ y: -5, boxShadow: '0 10px 20px rgba(0,0,0,0.1)' }}
                            >
                                <CountUpStatistic
                                    title={t('welcome.statistics.contributors')}
                                    value="10+"
                                    suffix="位"
                                    valueStyle={{ color: '#1890ff', fontWeight: 'bold' }}
                                />
                            </MotionCard>
                        </MotionCol>
                    </MotionRow>
                </div>
            </div>

            {/* 添加模型热度部分 */}
            <div ref={modelHotRef} style={{ padding: '80px 0', background: token.colorBgLayout }}>
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px' }}>
                    <motion.div
                      initial={{ opacity: 0, y: 30 }}
                      animate={modelHotInView ? { opacity: 1, y: 0 } : { opacity: 0, y: 30 }}
                      transition={{ duration: 0.6 }}
                    >
                        <Title level={2} style={{ textAlign: 'center', marginBottom: 16 }}>{t('welcome.modelHot.title')}</Title>
                        <Paragraph style={{ textAlign: 'center', color: token.colorTextSecondary, marginBottom: 48, maxWidth: 700, margin: '0 auto 48px' }}>
                            {t('welcome.modelHot.description')}
                        </Paragraph>
                    </motion.div>
                    
                    <MotionRow gutter={[24, 24]}>
                        <MotionCol xs={24} md={12}>
                            <MotionCard
                                loading={modelHotLoading}
                                style={{ height: '100%', background: token.colorBgLayout }}
                                whileHover={{ 
                                    y: -10,
                                    boxShadow: `0 15px 30px ${token.colorPrimary}` 
                                }}
                            >
                                <Title level={4}>{t('welcome.modelHot.distribution')}</Title>
                                <Paragraph style={{ color: token.colorTextSecondary, marginBottom: 24 }}>
                                    {t('welcome.modelHot.distributionDesc')}
                                </Paragraph>
                                {!modelHotLoading && modelHotData.length > 0 && (
                                    <motion.div
                                        initial={{ opacity: 0, scale: 0.9 }}
                                        animate={{ opacity: 1, scale: 1 }}
                                        transition={{ duration: 0.5 }}
                                    >
                                        <DonutChart
                                            data={modelHotData.map(item => ({
                                                name: item.model,
                                                value: item.percentage
                                            }))}
                                            valueFormatter={(value) => `${value.toFixed(1)}%`}
                                            variant="pie"
                                            showAnimation
                                        />
                                    </motion.div>
                                )}
                            </MotionCard>
                        </MotionCol>
                        
                        <MotionCol xs={24} md={12}>
                            <MotionCard
                                bordered={false}
                                loading={modelHotLoading}
                                style={{ height: '100%' }}
                                whileHover={{ 
                                    y: -10,
                                    boxShadow: `0 15px 30px ${token.colorPrimary}` 
                                }}
                            >
                                <Title level={4}>{t('welcome.modelHot.ranking')}</Title>
                                <Paragraph style={{ color: token.colorTextSecondary, marginBottom: 24 }}>
                                    {t('welcome.modelHot.rankingDesc')}
                                </Paragraph>
                                {!modelHotLoading && modelHotData.length > 0 && (
                                    <motion.div
                                        initial={{ opacity: 0, y: 20 }}
                                        animate={{ opacity: 1, y: 0 }}
                                        transition={{ duration: 0.5 }}
                                    >
                                        <BarList 
                                            data={modelHotData
                                              // 只要前10个
                                              .slice(0, 10)
                                              .map(item => ({
                                                name: item.model,
                                                value: item.percentage,
                                                // 可以添加图标
                                                icon: getIconByName(item.model)?.icon || null
                                            }))}
                                            showAnimation
                                            valueFormatter={(value) => `${value.toFixed(1)}%`}
                                            sortOrder='descending'
                                        />
                                    </motion.div>
                                )}
                            </MotionCard>
                        </MotionCol>
                    </MotionRow>
                </div>
            </div>

            {/* Features Section */}
            <div ref={featuresRef} style={{ padding: '80px 0', }}>
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px' }}>
                    <motion.div
                      initial={{ opacity: 0, y: 30 }}
                      animate={featuresInView ? { opacity: 1, y: 0 } : { opacity: 0, y: 30 }}
                      transition={{ duration: 0.6 }}
                    >
                        <Title level={2} style={{ textAlign: 'center', marginBottom: 16 }}>{t('welcome.features.title')}</Title>
                        <Paragraph style={{ textAlign: 'center', color: '#595959', marginBottom: 48, maxWidth: 700, margin: '0 auto 48px' }}>
                            {t('welcome.features.description')}
                        </Paragraph>
                    </motion.div>
                    <MotionRow 
                      gutter={[24, 24]}
                      variants={containerVariants}
                      initial="hidden"
                      animate={featuresInView ? "visible" : "hidden"}
                    >
                        <MotionCol xs={24} md={8} variants={cardVariants}>
                            <MotionFeatureCard 
                              color="#1890ff" 
                              bordered={false}
                              whileHover={{ 
                                y: -10,
                                boxShadow: '0 15px 30px rgba(0,0,0,0.1)' 
                              }}
                            >
                                <motion.div
                                  initial={{ scale: 0.8, opacity: 0 }}
                                  animate={{ scale: 1, opacity: 1 }}
                                  transition={{ duration: 0.5, delay: 0.2 }}
                                >
                                  <ApiOutlined style={{ fontSize: 36, color: '#1890ff', marginBottom: 16 }} />
                                </motion.div>
                                <Title level={4}>{t('welcome.features.multiModel.title')}</Title>
                                <Paragraph style={{ color: '#595959' }}>
                                    {t('welcome.features.multiModel.description')}
                                </Paragraph>
                                <Button type="link" style={{ padding: 0 }}>{t('welcome.features.multiModel.action')} →</Button>
                            </MotionFeatureCard>
                        </MotionCol>
                        <MotionCol xs={24} md={8} variants={cardVariants}>
                            <MotionFeatureCard 
                              color="#722ed1" 
                              bordered={false}
                              whileHover={{ 
                                y: -10,
                                boxShadow: '0 15px 30px rgba(0,0,0,0.1)' 
                              }}
                            >
                                <motion.div
                                  initial={{ scale: 0.8, opacity: 0 }}
                                  animate={{ scale: 1, opacity: 1 }}
                                  transition={{ duration: 0.5, delay: 0.3 }}
                                >
                                  <ThunderboltOutlined style={{ fontSize: 36, color: '#722ed1', marginBottom: 16 }} />
                                </motion.div>
                                <Title level={4}>{t('welcome.features.fastResponse.title')}</Title>
                                <Paragraph style={{ color: '#595959' }}>
                                    {t('welcome.features.fastResponse.description')}
                                </Paragraph>
                                <Button type="link" style={{ padding: 0 }}>{t('welcome.features.fastResponse.action')} →</Button>
                            </MotionFeatureCard>
                        </MotionCol>
                        <MotionCol xs={24} md={8} variants={cardVariants}>
                            <MotionFeatureCard 
                              color="#52c41a" 
                              bordered={false}
                              whileHover={{ 
                                y: -10,
                                boxShadow: '0 15px 30px rgba(0,0,0,0.1)' 
                              }}
                            >
                                <motion.div
                                  initial={{ scale: 0.8, opacity: 0 }}
                                  animate={{ scale: 1, opacity: 1 }}
                                  transition={{ duration: 0.5, delay: 0.4 }}
                                >
                                  <TeamOutlined style={{ fontSize: 36, color: '#52c41a', marginBottom: 16 }} />
                                </motion.div>
                                <Title level={4}>{t('welcome.features.community.title')}</Title>
                                <Paragraph style={{ color: '#595959' }}>
                                    {t('welcome.features.community.description')}
                                </Paragraph>
                                <Button type="link" style={{ padding: 0 }}>{t('welcome.features.community.action')} →</Button>
                            </MotionFeatureCard>
                        </MotionCol>
                    </MotionRow>
                </div>
            </div>

            <motion.div 
              style={{
                padding: '64px 0',
                background: 'linear-gradient(135deg, #1890ff 0%, #722ed1 100%)',
                color: 'white',
                position: 'relative',
                overflow: 'hidden'
              }}
              initial={{ backgroundPosition: '0% 0%' }}
              animate={{ backgroundPosition: '100% 100%' }}
              transition={{ 
                duration: 20, 
                repeat: Infinity, 
                repeatType: 'reverse',
                ease: "linear"
              }}
            >
                {/* 添加动态背景粒子效果 */}
                {[...Array(5)].map((_, index) => (
                  <motion.div
                    key={index}
                    style={{
                      position: 'absolute',
                      width: Math.random() * 100 + 50,
                      height: Math.random() * 100 + 50,
                      borderRadius: '50%',
                      background: 'rgba(255, 255, 255, 0.1)',
                      top: `${Math.random() * 100}%`,
                      left: `${Math.random() * 100}%`,
                    }}
                    animate={{
                      y: [0, Math.random() * 50 - 25],
                      x: [0, Math.random() * 50 - 25],
                      scale: [1, Math.random() * 0.5 + 0.8, 1],
                    }}
                    transition={{
                      duration: Math.random() * 5 + 5,
                      repeat: Infinity,
                      repeatType: 'reverse',
                      ease: "easeInOut"
                    }}
                  />
                ))}
                
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px', textAlign: 'center', position: 'relative', zIndex: 1 }}>
                    <motion.div
                      initial={{ opacity: 0, y: 30 }}
                      whileInView={{ opacity: 1, y: 0 }}
                      transition={{ duration: 0.6 }}
                      viewport={{ once: true, amount: 0.3 }}
                    >
                        <Title level={2} style={{ color: 'white', marginBottom: 16 }}>{t('welcome.callToAction.title')}</Title>
                        <Paragraph style={{ color: 'rgba(255,255,255,0.8)', marginBottom: 32, maxWidth: 700, margin: '0 auto 32px' }}>
                            {t('welcome.callToAction.description')}
                        </Paragraph>
                    </motion.div>
                    <Space size="large">
                        <MotionButton
                            type="primary"
                            onClick={() => {
                                navigate('/register')
                            }}
                            size="large"
                            style={{
                                color: '#1890ff',
                                height: 48,
                                padding: '0 24px',
                                borderRadius: 8
                            }}
                            initial={{ opacity: 0, y: 20 }}
                            whileInView={{ opacity: 1, y: 0 }}
                            transition={{ duration: 0.5, delay: 0.2 }}
                            whileHover={{ scale: 1.05, boxShadow: '0 6px 20px rgba(0,0,0,0.15)' }}
                            whileTap={{ scale: 0.95 }}
                            viewport={{ once: true, amount: 0.3 }}
                        >
                            {t('welcome.callToAction.register')}
                        </MotionButton>
                        <MotionButton
                            size="large"
                            ghost
                            onClick={() => {
                                window.open('https://github.com/AIDotNet/Thor', '_blank')
                            }}
                            style={{
                                borderColor: 'white',
                                color: 'white',
                                height: 48,
                                padding: '0 24px',
                                borderRadius: 8
                            }}
                            initial={{ opacity: 0, y: 20 }}
                            whileInView={{ opacity: 1, y: 0 }}
                            transition={{ duration: 0.5, delay: 0.3 }}
                            whileHover={{ scale: 1.05, boxShadow: '0 6px 20px rgba(0,0,0,0.15)' }}
                            whileTap={{ scale: 0.95 }}
                            viewport={{ once: true, amount: 0.3 }}
                        >
                            {t('welcome.callToAction.docs')}
                        </MotionButton>
                    </Space>
                </div>
            </motion.div>

            {/* Projects Section */}
            <div ref={projectsRef} style={{ padding: '64px 0', }}>
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px' }}>
                    <motion.div
                      initial={{ opacity: 0, y: 30 }}
                      animate={projectsInView ? { opacity: 1, y: 0 } : { opacity: 0, y: 30 }}
                      transition={{ duration: 0.6 }}
                    >
                        <Title level={2} style={{ textAlign: 'center', marginBottom: 48 }}>{t('welcome.projects.title')}</Title>
                    </motion.div>
                    <MotionRow 
                      gutter={[24, 24]}
                      variants={containerVariants}
                      initial="hidden"
                      animate={projectsInView ? "visible" : "hidden"}
                    >
                        <MotionCol xs={24} md={12} variants={cardVariants}>
                            <MotionProjectCard 
                              bordered={false}
                              whileHover={{ 
                                y: -10,
                                boxShadow: '0 15px 30px rgba(0,0,0,0.1)' 
                              }}
                            >
                                <Title level={4}>{t('welcome.projects.aidotnet.title')}</Title>
                                <Paragraph style={{ color: '#595959' }}>
                                    {t('welcome.projects.aidotnet.description')}
                                </Paragraph>
                                <Button 
                                    onClick={()=>{
                                        window.open('https://github.com/AIDotNet/', '_blank')
                                    }}
                                    type="link" style={{ padding: 0 }}
                                >{t('welcome.projects.aidotnet.action')} →</Button>
                            </MotionProjectCard>
                        </MotionCol>
                        <MotionCol xs={24} md={12} variants={cardVariants}>
                            <MotionProjectCard 
                              bordered={false}
                              whileHover={{ 
                                y: -10,
                                boxShadow: '0 15px 30px rgba(0,0,0,0.1)' 
                              }}
                            >
                                <Title level={4}>{t('welcome.projects.fastwiki.title')}</Title>
                                <Paragraph style={{ color: '#595959' }}>
                                    {t('welcome.projects.fastwiki.description')}
                                </Paragraph>
                                <Button 
                                    onClick={()=>{
                                        window.open('https://github.com/AIDotNet/fast-wiki/', '_blank')
                                    }}
                                    type="link" style={{ padding: 0 }}
                                >{t('welcome.projects.fastwiki.action')} →</Button>
                            </MotionProjectCard>
                        </MotionCol>
                    </MotionRow>
                </div>
            </div>
        </Content>
    );
};

export default ThorWebsite;
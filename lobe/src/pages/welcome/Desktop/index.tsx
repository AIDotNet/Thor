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
    ArrowRightOutlined,
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

// 添加3D悬浮卡片效果
const MotionFeatureCard3D = motion(styled(Card)`
  height: 100%;
  transition: all 0.5s cubic-bezier(0.2, 0.8, 0.2, 1);
  transform-style: preserve-3d;
  perspective: 1000px;
  &:hover {
    transform: translateY(-10px) rotateX(2deg) rotateY(2deg);
    box-shadow: 0 15px 30px rgba(0,0,0,0.15);
  }
  border-top: 4px solid ${props => props.color || 'transparent'};
  border-radius: 12px;
  overflow: hidden;
`);

// 添加波浪背景组件
const WaveBackground = () => {
  return (
    <div style={{ position: 'absolute', bottom: 0, left: 0, width: '100%', overflow: 'hidden', zIndex: 0 }}>
      <svg 
        xmlns="http://www.w3.org/2000/svg" 
        viewBox="0 0 1440 320"
        style={{ display: 'block', width: 'calc(100% + 1.5px)', height: 120 }}
      >
        <motion.path 
          fill="rgba(24, 144, 255, 0.1)" 
          d="M0,64L48,80C96,96,192,128,288,122.7C384,117,480,75,576,64C672,53,768,75,864,96C960,117,1056,139,1152,122.7C1248,107,1344,53,1392,26.7L1440,0L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z"
          initial={{ d: "M0,64L48,80C96,96,192,128,288,122.7C384,117,480,75,576,64C672,53,768,75,864,96C960,117,1056,139,1152,122.7C1248,107,1344,53,1392,26.7L1440,0L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z" }}
          animate={{ 
            d: [
              "M0,64L48,80C96,96,192,128,288,122.7C384,117,480,75,576,64C672,53,768,75,864,96C960,117,1056,139,1152,122.7C1248,107,1344,53,1392,26.7L1440,0L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z",
              "M0,96L48,122.7C96,149,192,203,288,192C384,181,480,107,576,80C672,53,768,75,864,106.7C960,139,1056,181,1152,170.7C1248,160,1344,96,1392,64L1440,32L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z",
              "M0,64L48,80C96,96,192,128,288,122.7C384,117,480,75,576,64C672,53,768,75,864,96C960,117,1056,139,1152,122.7C1248,107,1344,53,1392,26.7L1440,0L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z"
            ]
          }}
          transition={{ 
            duration: 20, 
            repeat: Infinity, 
            repeatType: 'reverse',
            ease: "easeInOut"
          }}
        />
        <motion.path 
          fill="rgba(114, 46, 209, 0.1)" 
          d="M0,224L48,213.3C96,203,192,181,288,154.7C384,128,480,96,576,101.3C672,107,768,149,864,160C960,171,1056,149,1152,128C1248,107,1344,85,1392,74.7L1440,64L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z"
          initial={{ d: "M0,224L48,213.3C96,203,192,181,288,154.7C384,128,480,96,576,101.3C672,107,768,149,864,160C960,171,1056,149,1152,128C1248,107,1344,85,1392,74.7L1440,64L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z" }}
          animate={{ 
            d: [
              "M0,224L48,213.3C96,203,192,181,288,154.7C384,128,480,96,576,101.3C672,107,768,149,864,160C960,171,1056,149,1152,128C1248,107,1344,85,1392,74.7L1440,64L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z",
              "M0,160L48,170.7C96,181,192,203,288,213.3C384,224,480,224,576,202.7C672,181,768,139,864,138.7C960,139,1056,181,1152,176C1248,171,1344,117,1392,90.7L1440,64L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z",
              "M0,224L48,213.3C96,203,192,181,288,154.7C384,128,480,96,576,101.3C672,107,768,149,864,160C960,171,1056,149,1152,128C1248,107,1344,85,1392,74.7L1440,64L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z"
            ]
          }}
          transition={{ 
            duration: 15, 
            repeat: Infinity, 
            repeatType: 'reverse',
            ease: "easeInOut"
          }}
        />
      </svg>
    </div>
  );
};

// 优化动画背景组件
const AnimatedBackground = () => {
  return (
    <>
      <motion.div 
        style={{
          position: 'absolute',
          width: 400,
          height: 400,
          borderRadius: '50%',
          background: 'radial-gradient(circle, rgba(24,144,255,0.3) 0%, rgba(24,144,255,0) 70%)',
          opacity: 0.2,
          top: -100,
          right: -100,
          filter: 'blur(40px)'
        }}
        initial={{ x: 0, y: 0 }}
        animate={{
          x: [0, 30, 0],
          y: [0, 20, 0],
        }}
        transition={{
          duration: 15,
          repeat: Infinity,
          ease: "easeInOut",
          repeatType: "reverse"
        }}
      />
      <motion.div 
        style={{
          position: 'absolute',
          width: 300,
          height: 300,
          borderRadius: '50%',
          background: 'radial-gradient(circle, rgba(114,46,209,0.3) 0%, rgba(114,46,209,0) 70%)',
          opacity: 0.2,
          bottom: 50,
          left: 100,
          filter: 'blur(40px)'
        }}
        initial={{ x: 0, y: 0 }}
        animate={{
          x: [0, -30, 0],
          y: [0, 30, 0],
        }}
        transition={{
          duration: 18,
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
          background: 'radial-gradient(circle, rgba(82,196,26,0.3) 0%, rgba(82,196,26,0) 70%)',
          opacity: 0.15,
          bottom: -50,
          right: 150,
          filter: 'blur(30px)'
        }}
        initial={{ x: 0, y: 0 }}
        animate={{
          x: [0, 20, 0],
          y: [0, -20, 0],
        }}
        transition={{
          duration: 12,
          repeat: Infinity,
          ease: "easeInOut",
          repeatType: "reverse"
        }}
      />
      <WaveBackground />
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
            <div className={styles.heroSection} style={{ 
                minHeight: '100vh', 
                display: 'flex', 
                alignItems: 'center',
                background: 'linear-gradient(135deg, #141414 0%, #1f1f1f 100%)'
            }}>
                <div className={styles.heroBackground}>
                    <AnimatedBackground />
                </div>

                <div className={styles.heroContainer}>
                    <motion.div 
                      className={styles.heroContent}
                      initial={{ opacity: 0, x: -50 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.8, ease: "easeOut" }}
                    >
                        <MotionTitle 
                          className={styles.heroTitle}
                          initial={{ opacity: 0, y: -30 }}
                          animate={{ opacity: 1, y: 0 }}
                          transition={{ duration: 0.7, delay: 0.2 }}
                          style={{
                            fontSize: 56,
                            fontWeight: 800,
                            background: 'linear-gradient(90deg, #fff 0%, #d9d9d9 100%)',
                            WebkitBackgroundClip: 'text',
                            WebkitTextFillColor: 'transparent'
                          }}
                        >
                            {t('welcome.title')}
                        </MotionTitle>
                        <MotionParagraph 
                          style={{ color: '#d9d9d9', fontSize: 20, marginBottom: 24, lineHeight: 1.6 }}
                          initial={{ opacity: 0 }}
                          animate={{ opacity: 1 }}
                          transition={{ duration: 0.6, delay: 0.4 }}
                        >
                            {t('welcome.description')} <Text style={{ color: '#1890ff', fontWeight: 'bold', fontSize: 22 }}>68+</Text>
                        </MotionParagraph>
                        <MotionParagraph 
                          style={{ color: '#d9d9d9', fontSize: 18, marginBottom: 40, fontStyle: 'italic' }}
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
                                whileHover={{ scale: 1.05, boxShadow: '0 8px 25px rgba(24, 144, 255, 0.5)' }}
                                whileTap={{ scale: 0.95 }}
                                style={{
                                    background: 'linear-gradient(90deg, #1890ff 0%, #096dd9 100%)',
                                    fontSize: 16,
                                    height: 52,
                                    borderRadius: 10
                                }}
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
                                    height: 52,
                                    padding: '0 24px',
                                    borderRadius: 10,
                                    fontSize: 16,
                                    background: 'rgba(255, 255, 255, 0.05)'
                                }}
                                initial={{ opacity: 0, scale: 0.9 }}
                                animate={{ opacity: 1, scale: 1 }}
                                transition={{ duration: 0.5, delay: 0.8 }}
                                whileHover={{ scale: 1.05, boxShadow: '0 8px 25px rgba(0, 0, 0, 0.3)', background: 'rgba(255, 255, 255, 0.1)' }}
                                whileTap={{ scale: 0.95 }}
                            >
                                <GithubOutlined /> {t('welcome.giveStar')}
                            </MotionButton>
                        </Space>
                    </motion.div>
                    <motion.div 
                      className={styles.heroImage}
                      initial={{ opacity: 0, x: 50 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.8, delay: 0.3, ease: "easeOut" }}
                    >
                        <MotionCard
                            className="tilted-card"
                            style={{
                                width: '100%',
                                background: 'linear-gradient(135deg, rgba(31, 31, 31, 0.8) 0%, rgba(20, 20, 20, 0.8) 100%)',
                                backdropFilter: 'blur(10px)',
                                borderColor: '#434343',
                                borderRadius: 20,
                                boxShadow: '0 30px 60px -10px rgba(0, 0, 0, 0.4), 0 18px 36px -18px rgba(0, 0, 0, 0.3)'
                            }}
                            whileHover={{ 
                              y: -15,
                              rotateY: 5,
                              rotateX: 5,
                              boxShadow: '0 40px 70px -15px rgba(0, 0, 0, 0.5), 0 25px 45px -20px rgba(0, 0, 0, 0.4)'
                            }}
                            transition={{ duration: 0.5 }}
                        >
                            <Title level={3} style={{ color: 'white', marginBottom: 5 }}>{t('welcome.community.title')}</Title>
                            <Paragraph style={{ color: '#d9d9d9', marginBottom: 30, fontSize: 16 }}>
                                {t('welcome.community.description')}
                            </Paragraph>
                            <Space direction="vertical" size="large" style={{ width: '100%' }}>
                                <motion.div 
                                  style={{ display: 'flex', alignItems: 'flex-start' }}
                                  initial={{ opacity: 0, x: 30 }}
                                  animate={{ opacity: 1, x: 0 }}
                                  transition={{ duration: 0.5, delay: 0.5 }}
                                  whileHover={{ x: 5 }}
                                >
                                    <div style={{ 
                                        backgroundColor: 'rgba(24, 144, 255, 0.2)', 
                                        padding: 10,
                                        borderRadius: 12,
                                        marginRight: 16,
                                        display: 'flex',
                                        alignItems: 'center',
                                        justifyContent: 'center'
                                    }}>
                                        <DollarOutlined style={{ color: '#1890ff', fontSize: 24 }} />
                                    </div>
                                    <div>
                                        <Text strong style={{ color: 'white', fontSize: 18 }}>{t('welcome.community.payPerUse.title')}</Text>
                                        <Paragraph style={{ color: '#8c8c8c', marginBottom: 0, fontSize: 15 }}>
                                            {t('welcome.community.payPerUse.description')}
                                        </Paragraph>
                                    </div>
                                </motion.div>
                                <motion.div 
                                  style={{ display: 'flex', alignItems: 'flex-start' }}
                                  initial={{ opacity: 0, x: 30 }}
                                  animate={{ opacity: 1, x: 0 }}
                                  transition={{ duration: 0.5, delay: 0.6 }}
                                  whileHover={{ x: 5 }}
                                >
                                    <div style={{ 
                                        backgroundColor: 'rgba(24, 144, 255, 0.2)', 
                                        padding: 10,
                                        borderRadius: 12,
                                        marginRight: 16,
                                        display: 'flex',
                                        alignItems: 'center',
                                        justifyContent: 'center'
                                    }}>
                                        <AppstoreOutlined style={{ color: '#1890ff', fontSize: 24 }} />
                                    </div>
                                    <div>
                                        <Text strong style={{ color: 'white', fontSize: 18 }}>{t('welcome.community.appSupport.title')}</Text>
                                        <Paragraph style={{ color: '#8c8c8c', marginBottom: 0, fontSize: 15 }}>
                                            {t('welcome.community.appSupport.description')}
                                        </Paragraph>
                                    </div>
                                </motion.div>
                                <motion.div 
                                  style={{ display: 'flex', alignItems: 'flex-start' }}
                                  initial={{ opacity: 0, x: 30 }}
                                  animate={{ opacity: 1, x: 0 }}
                                  transition={{ duration: 0.5, delay: 0.7 }}
                                  whileHover={{ x: 5 }}
                                >
                                    <div style={{ 
                                        backgroundColor: 'rgba(24, 144, 255, 0.2)', 
                                        padding: 10,
                                        borderRadius: 12,
                                        marginRight: 16,
                                        display: 'flex',
                                        alignItems: 'center',
                                        justifyContent: 'center'
                                    }}>
                                        <CheckCircleOutlined style={{ color: '#1890ff', fontSize: 24 }} />
                                    </div>
                                    <div>
                                        <Text strong style={{ color: 'white', fontSize: 18 }}>{t('welcome.community.transparency.title')}</Text>
                                        <Paragraph style={{ color: '#8c8c8c', marginBottom: 0, fontSize: 15 }}>
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
            <div ref={statsRef} style={{ padding: '80px 0', background: 'linear-gradient(180deg, #141414 0%, #1a1a1a 100%)' }}>
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px' }}>
                    <motion.div
                      initial={{ opacity: 0, y: 30 }}
                      animate={statsInView ? { opacity: 1, y: 0 } : { opacity: 0, y: 30 }}
                      transition={{ duration: 0.6 }}
                      style={{ marginBottom: 50 }}
                    >
                        <Title level={2} style={{ textAlign: 'center', marginBottom: 16, color: 'white' }}>{t('welcome.statistics.title')}</Title>
                        <Paragraph style={{ textAlign: 'center', color: '#aaa', marginBottom: 30, maxWidth: 700, margin: '0 auto' }}>
                            {t('welcome.statistics.description')}
                        </Paragraph>
                    </motion.div>
                    <MotionRow 
                      gutter={[32, 32]} 
                      justify="center"
                      variants={containerVariants}
                      initial="hidden"
                      animate={statsInView ? "visible" : "hidden"}
                    >
                        <MotionCol xs={12} md={6} variants={itemVariants}>
                            <MotionCard 
                              bordered={false} 
                              style={{ 
                                textAlign: 'center', 
                                height: '100%',
                                borderRadius: 16,
                                background: 'rgba(24, 144, 255, 0.05)',
                                border: '1px solid rgba(24, 144, 255, 0.1)'
                              }}
                              whileHover={{ 
                                y: -10, 
                                boxShadow: '0 20px 40px rgba(0,0,0,0.2)',
                                background: 'rgba(24, 144, 255, 0.1)',
                                border: '1px solid rgba(24, 144, 255, 0.2)'
                              }}
                            >
                                <CountUpStatistic
                                    title={t('welcome.statistics.models')}
                                    value="200+"
                                    suffix="个"
                                    valueStyle={{ color: '#1890ff', fontWeight: 'bold', fontSize: 36 }}
                                />
                            </MotionCard>
                        </MotionCol>
                        <MotionCol xs={12} md={6} variants={itemVariants}>
                            <MotionCard 
                              bordered={false} 
                              style={{ 
                                textAlign: 'center', 
                                height: '100%',
                                borderRadius: 16,
                                background: 'rgba(114, 46, 209, 0.05)',
                                border: '1px solid rgba(114, 46, 209, 0.1)'
                              }}
                              whileHover={{ 
                                y: -10, 
                                boxShadow: '0 20px 40px rgba(0,0,0,0.2)',
                                background: 'rgba(114, 46, 209, 0.1)',
                                border: '1px solid rgba(114, 46, 209, 0.2)'
                              }}
                            >
                                <CountUpStatistic
                                    title={t('welcome.statistics.users')}
                                    value="8000+"
                                    suffix="人"
                                    valueStyle={{ color: '#722ed1', fontWeight: 'bold', fontSize: 36 }}
                                />
                            </MotionCard>
                        </MotionCol>
                        <MotionCol xs={12} md={6} variants={itemVariants}>
                            <MotionCard 
                              bordered={false} 
                              style={{ 
                                textAlign: 'center', 
                                height: '100%',
                                borderRadius: 16,
                                background: 'rgba(82, 196, 26, 0.05)',
                                border: '1px solid rgba(82, 196, 26, 0.1)'
                              }}
                              whileHover={{ 
                                y: -10, 
                                boxShadow: '0 20px 40px rgba(0,0,0,0.2)',
                                background: 'rgba(82, 196, 26, 0.1)',
                                border: '1px solid rgba(82, 196, 26, 0.2)'
                              }}
                            >
                                <CountUpStatistic
                                    title={t('welcome.statistics.requests')}
                                    value="1M+"
                                    suffix="次"
                                    valueStyle={{ color: '#52c41a', fontWeight: 'bold', fontSize: 36 }}
                                />
                            </MotionCard>
                        </MotionCol>
                        <MotionCol xs={12} md={6} variants={itemVariants}>
                            <MotionCard 
                              bordered={false} 
                              style={{ 
                                textAlign: 'center', 
                                height: '100%',
                                borderRadius: 16,
                                background: 'rgba(250, 173, 20, 0.05)',
                                border: '1px solid rgba(250, 173, 20, 0.1)'
                              }}
                              whileHover={{ 
                                y: -10, 
                                boxShadow: '0 20px 40px rgba(0,0,0,0.2)',
                                background: 'rgba(250, 173, 20, 0.1)',
                                border: '1px solid rgba(250, 173, 20, 0.2)'
                              }}
                            >
                                <CountUpStatistic
                                    title={t('welcome.statistics.contributors')}
                                    value="10+"
                                    suffix="位"
                                    valueStyle={{ color: '#faad14', fontWeight: 'bold', fontSize: 36 }}
                                />
                            </MotionCard>
                        </MotionCol>
                    </MotionRow>
                </div>
            </div>

            {/* 添加模型热度部分 */}
            <div ref={modelHotRef} style={{ 
                padding: '100px 0', 
                background: 'linear-gradient(180deg, #1a1a1a 0%, #262626 100%)',
                position: 'relative',
                overflow: 'hidden'
            }}>
                {/* 添加背景元素 */}
                    <motion.div
                    style={{
                        position: 'absolute',
                        width: '60%',
                        height: '60%',
                        borderRadius: '50%',
                        background: 'radial-gradient(circle, rgba(24,144,255,0.15) 0%, rgba(24,144,255,0) 70%)',
                        top: '20%',
                        left: '50%',
                        filter: 'blur(80px)',
                        transform: 'translateX(-50%)',
                        opacity: 0.6,
                        zIndex: 0
                    }}
                    animate={{
                        scale: [1, 1.2, 1],
                        opacity: [0.6, 0.4, 0.6],
                    }}
                    transition={{
                        duration: 15,
                        repeat: Infinity,
                        ease: "easeInOut"
                    }}
                />
                
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px', position: 'relative', zIndex: 1 }}>
                    <motion.div
                      initial={{ opacity: 0, y: 40 }}
                      animate={modelHotInView ? { opacity: 1, y: 0 } : { opacity: 0, y: 40 }}
                      transition={{ duration: 0.8 }}
                    >
                        <Title level={2} style={{ 
                            textAlign: 'center', 
                            marginBottom: 16, 
                            color: 'white',
                            fontSize: 36,
                            fontWeight: 800,
                        }}>{t('welcome.modelHot.title')}</Title>
                        <Paragraph style={{ 
                            textAlign: 'center', 
                            color: '#aaa', 
                            marginBottom: 60, 
                            maxWidth: 700, 
                            margin: '0 auto 60px',
                            fontSize: 16,
                            lineHeight: 1.6
                        }}>
                            {t('welcome.modelHot.description')}
                        </Paragraph>
                    </motion.div>
                    
                    <MotionRow gutter={[40, 40]}>
                        <MotionCol xs={24} md={12}>
                            <MotionCard
                                loading={modelHotLoading}
                                style={{ 
                                    height: '100%', 
                                    background: 'rgba(30, 30, 30, 0.7)',
                                    backdropFilter: 'blur(10px)',
                                    border: '1px solid rgba(255, 255, 255, 0.05)',
                                    borderRadius: 20,
                                    overflow: 'hidden'
                                }}
                                bodyStyle={{ padding: 30 }}
                                whileHover={{ 
                                    y: -15,
                                    boxShadow: `0 20px 40px rgba(0, 0, 0, 0.3), 0 0 80px rgba(24, 144, 255, 0.1)`,
                                    border: '1px solid rgba(24, 144, 255, 0.2)'
                                }}
                                transition={{ duration: 0.5 }}
                            >
                                <Title level={4} style={{ color: 'white', marginBottom: 20 }}>{t('welcome.modelHot.distribution')}</Title>
                                <Paragraph style={{ color: '#aaa', marginBottom: 30, fontSize: 15 }}>
                                    {t('welcome.modelHot.distributionDesc')}
                                </Paragraph>
                                {!modelHotLoading && modelHotData.length > 0 && (
                                    <motion.div
                                        initial={{ opacity: 0, scale: 0.9 }}
                                        animate={{ opacity: 1, scale: 1 }}
                                        transition={{ duration: 0.5 }}
                                        style={{ height: 300 }}
                                    >
                                        <DonutChart
                                            data={modelHotData.map(item => ({
                                                name: item.model,
                                                value: item.percentage
                                            }))}
                                            valueFormatter={(value) => `${value.toFixed(1)}%`}
                                            variant="pie"
                                            showAnimation
                                            colors={['#1890ff', '#13c2c2', '#722ed1', '#eb2f96', '#faad14']}
                                        />
                                    </motion.div>
                                )}
                            </MotionCard>
                        </MotionCol>
                        
                        <MotionCol xs={24} md={12}>
                            <MotionCard
                                bordered={false}
                                loading={modelHotLoading}
                                style={{ 
                                    height: '100%',
                                    background: 'rgba(30, 30, 30, 0.7)',
                                    backdropFilter: 'blur(10px)',
                                    border: '1px solid rgba(255, 255, 255, 0.05)',
                                    borderRadius: 20,
                                    overflow: 'hidden'
                                }}
                                bodyStyle={{ padding: 30 }}
                                whileHover={{ 
                                    y: -15,
                                    boxShadow: `0 20px 40px rgba(0, 0, 0, 0.3), 0 0 80px rgba(114, 46, 209, 0.1)`,
                                    border: '1px solid rgba(114, 46, 209, 0.2)'
                                }}
                                transition={{ duration: 0.5 }}
                            >
                                <Title level={4} style={{ color: 'white', marginBottom: 20 }}>{t('welcome.modelHot.ranking')}</Title>
                                <Paragraph style={{ color: '#aaa', marginBottom: 30, fontSize: 15 }}>
                                    {t('welcome.modelHot.rankingDesc')}
                                </Paragraph>
                                {!modelHotLoading && modelHotData.length > 0 && (
                                    <motion.div
                                        initial={{ opacity: 0, y: 20 }}
                                        animate={{ opacity: 1, y: 0 }}
                                        transition={{ duration: 0.5 }}
                                        style={{ height: 300, overflowY: 'auto' }}
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
                                            color="#1890ff"
                                        />
                                    </motion.div>
                                )}
                            </MotionCard>
                        </MotionCol>
                    </MotionRow>
                </div>
            </div>

            {/* Features Section */}
            <div ref={featuresRef} style={{ 
                padding: '100px 0', 
                background: 'linear-gradient(180deg, #262626 0%, #1a1a1a 100%)',
                position: 'relative',
                overflow: 'hidden'
            }}>
                {/* 添加装饰线条 */}
                <svg
                    style={{
                        position: 'absolute',
                        width: '100%',
                        height: '100%',
                        top: 0,
                        left: 0,
                        zIndex: 0,
                        opacity: 0.05
                    }}
                >
                    <pattern
                        id="grid"
                        width="40"
                        height="40"
                        patternUnits="userSpaceOnUse"
                    >
                        <path
                            d="M 40 0 L 0 0 0 40"
                            fill="none"
                            stroke="white"
                            strokeWidth="0.5"
                        />
                    </pattern>
                    <rect width="100%" height="100%" fill="url(#grid)" />
                </svg>
                
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px', position: 'relative', zIndex: 1 }}>
                    <motion.div
                      initial={{ opacity: 0, y: 40 }}
                      animate={featuresInView ? { opacity: 1, y: 0 } : { opacity: 0, y: 40 }}
                      transition={{ duration: 0.8 }}
                    >
                        <Title level={2} style={{ 
                            textAlign: 'center', 
                            marginBottom: 16, 
                            color: 'white',
                            fontSize: 36, 
                            fontWeight: 800
                        }}>{t('welcome.features.title')}</Title>
                        <Paragraph style={{ 
                            textAlign: 'center', 
                            color: '#aaa', 
                            marginBottom: 60, 
                            maxWidth: 700, 
                            margin: '0 auto 60px',
                            fontSize: 16,
                            lineHeight: 1.6
                        }}>
                            {t('welcome.features.description')}
                        </Paragraph>
                    </motion.div>
                    <MotionRow 
                      gutter={[40, 40]}
                      variants={containerVariants}
                      initial="hidden"
                      animate={featuresInView ? "visible" : "hidden"}
                    >
                        <MotionCol xs={24} md={8} variants={cardVariants}>
                            <MotionFeatureCard3D 
                              color="#1890ff" 
                              bordered={false}
                              style={{
                                  padding: 30,
                                  background: 'rgba(30, 30, 30, 0.8)',
                                  backdropFilter: 'blur(10px)',
                                  borderRadius: 20
                              }}
                            >
                                <motion.div
                                  initial={{ scale: 0.8, opacity: 0 }}
                                  animate={{ scale: 1, opacity: 1 }}
                                  transition={{ duration: 0.5, delay: 0.2 }}
                                  style={{
                                      background: 'rgba(24, 144, 255, 0.1)',
                                      width: 70,
                                      height: 70,
                                      borderRadius: 15,
                                      display: 'flex',
                                      alignItems: 'center',
                                      justifyContent: 'center',
                                      marginBottom: 25
                                  }}
                                >
                                  <ApiOutlined style={{ fontSize: 36, color: '#1890ff' }} />
                                </motion.div>
                                <Title level={4} style={{ color: 'white', marginBottom: 15 }}>{t('welcome.features.multiModel.title')}</Title>
                                <Paragraph style={{ color: '#aaa', marginBottom: 20, fontSize: 15, lineHeight: 1.6 }}>
                                    {t('welcome.features.multiModel.description')}
                                </Paragraph>
                                <Button type="link" style={{ padding: 0, color: '#1890ff', fontSize: 15 }}>{t('welcome.features.multiModel.action')} →</Button>
                            </MotionFeatureCard3D>
                        </MotionCol>
                        <MotionCol xs={24} md={8} variants={cardVariants}>
                            <MotionFeatureCard3D 
                              color="#722ed1" 
                              bordered={false}
                              style={{
                                  padding: 30,
                                  background: 'rgba(30, 30, 30, 0.8)',
                                  backdropFilter: 'blur(10px)',
                                  borderRadius: 20
                              }}
                            >
                                <motion.div
                                  initial={{ scale: 0.8, opacity: 0 }}
                                  animate={{ scale: 1, opacity: 1 }}
                                  transition={{ duration: 0.5, delay: 0.3 }}
                                  style={{
                                      background: 'rgba(114, 46, 209, 0.1)',
                                      width: 70,
                                      height: 70,
                                      borderRadius: 15,
                                      display: 'flex',
                                      alignItems: 'center',
                                      justifyContent: 'center',
                                      marginBottom: 25
                                  }}
                                >
                                  <ThunderboltOutlined style={{ fontSize: 36, color: '#722ed1' }} />
                                </motion.div>
                                <Title level={4} style={{ color: 'white', marginBottom: 15 }}>{t('welcome.features.fastResponse.title')}</Title>
                                <Paragraph style={{ color: '#aaa', marginBottom: 20, fontSize: 15, lineHeight: 1.6 }}>
                                    {t('welcome.features.fastResponse.description')}
                                </Paragraph>
                                <Button type="link" style={{ padding: 0, color: '#722ed1', fontSize: 15 }}>{t('welcome.features.fastResponse.action')} →</Button>
                            </MotionFeatureCard3D>
                        </MotionCol>
                        <MotionCol xs={24} md={8} variants={cardVariants}>
                            <MotionFeatureCard3D 
                              color="#52c41a" 
                              bordered={false}
                              style={{
                                  padding: 30,
                                  background: 'rgba(30, 30, 30, 0.8)',
                                  backdropFilter: 'blur(10px)',
                                  borderRadius: 20
                              }}
                            >
                                <motion.div
                                  initial={{ scale: 0.8, opacity: 0 }}
                                  animate={{ scale: 1, opacity: 1 }}
                                  transition={{ duration: 0.5, delay: 0.4 }}
                                  style={{
                                      background: 'rgba(82, 196, 26, 0.1)',
                                      width: 70,
                                      height: 70,
                                      borderRadius: 15,
                                      display: 'flex',
                                      alignItems: 'center',
                                      justifyContent: 'center',
                                      marginBottom: 25
                                  }}
                                >
                                  <TeamOutlined style={{ fontSize: 36, color: '#52c41a' }} />
                                </motion.div>
                                <Title level={4} style={{ color: 'white', marginBottom: 15 }}>{t('welcome.features.community.title')}</Title>
                                <Paragraph style={{ color: '#aaa', marginBottom: 20, fontSize: 15, lineHeight: 1.6 }}>
                                    {t('welcome.features.community.description')}
                                </Paragraph>
                                <Button type="link" style={{ padding: 0, color: '#52c41a', fontSize: 15 }}>{t('welcome.features.community.action')} →</Button>
                            </MotionFeatureCard3D>
                        </MotionCol>
                    </MotionRow>
                </div>
            </div>

            <motion.div 
              style={{
                padding: '100px 0',
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
                {/* 添加动态背景效果 */}
                  <motion.div
                    style={{
                      position: 'absolute',
                    width: '100%',
                    height: '100%',
                    background: 'url("data:image/svg+xml,%3Csvg width=\'60\' height=\'60\' viewBox=\'0 0 60 60\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cg fill=\'none\' fill-rule=\'evenodd\'%3E%3Cg fill=\'%23ffffff\' fill-opacity=\'0.05\'%3E%3Cpath d=\'M36 34v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6 34v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6 4V0H4v4H0v2h4v4h2V6h4V4H6z\'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E")',
                    backgroundSize: '60px 60px',
                    opacity: 0.2,
                    top: 0,
                    left: 0,
                    zIndex: 0
                  }}
                  animate={{ 
                    backgroundPosition: ['0px 0px', '60px 60px']
                  }}
                  transition={{ 
                    duration: 10, 
                    repeat: Infinity, 
                    ease: "linear"
                  }}
                />
                
                {/* 添加光晕效果 */}
                <motion.div
                  style={{
                    position: 'absolute',
                    width: '60%',
                    height: '60%',
                      borderRadius: '50%',
                    background: 'radial-gradient(circle, rgba(255,255,255,0.2) 0%, rgba(255,255,255,0) 70%)',
                    top: '20%',
                    left: '20%',
                    filter: 'blur(50px)',
                    zIndex: 0
                    }}
                    animate={{
                    scale: [1, 1.2, 1],
                    opacity: [0.2, 0.3, 0.2],
                    x: [0, 30, 0]
                    }}
                    transition={{
                    duration: 15, 
                      repeat: Infinity,
                      ease: "easeInOut"
                    }}
                  />
                
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px', textAlign: 'center', position: 'relative', zIndex: 1 }}>
                    <motion.div
                      initial={{ opacity: 0, y: 40 }}
                      whileInView={{ opacity: 1, y: 0 }}
                      transition={{ duration: 0.8 }}
                      viewport={{ once: true, amount: 0.3 }}
                    >
                        <Title level={2} style={{ 
                            color: 'white', 
                            marginBottom: 20, 
                            fontSize: 40, 
                            fontWeight: 800,
                            textShadow: '0 2px 10px rgba(0,0,0,0.2)'
                        }}>{t('welcome.callToAction.title')}</Title>
                        <Paragraph style={{ 
                            color: 'rgba(255,255,255,0.9)', 
                            marginBottom: 40, 
                            maxWidth: 700, 
                            margin: '0 auto 40px',
                            fontSize: 18,
                            lineHeight: 1.6
                        }}>
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
                                background: 'white',
                                color: '#1890ff',
                                height: 54,
                                padding: '0 30px',
                                borderRadius: 10,
                                fontSize: 16,
                                fontWeight: 600,
                                border: 'none'
                            }}
                            initial={{ opacity: 0, y: 20 }}
                            whileInView={{ opacity: 1, y: 0 }}
                            transition={{ duration: 0.5, delay: 0.2 }}
                            whileHover={{ 
                                scale: 1.05, 
                                boxShadow: '0 10px 30px rgba(0,0,0,0.2)',
                                y: -5 
                            }}
                            whileTap={{ scale: 0.95 }}
                            viewport={{ once: true, amount: 0.3 }}
                        >
                            {t('welcome.callToAction.register')}
                        </MotionButton>
                        <MotionButton
                            size="large"
                            onClick={() => {
                                window.open('https://github.com/AIDotNet/Thor', '_blank')
                            }}
                            style={{
                                borderColor: 'rgba(255,255,255,0.8)',
                                color: 'white',
                                height: 54,
                                padding: '0 30px',
                                borderRadius: 10,
                                fontSize: 16,
                                fontWeight: 600,
                                background: 'rgba(255,255,255,0.1)'
                            }}
                            initial={{ opacity: 0, y: 20 }}
                            whileInView={{ opacity: 1, y: 0 }}
                            transition={{ duration: 0.5, delay: 0.3 }}
                            whileHover={{ 
                                scale: 1.05, 
                                boxShadow: '0 10px 30px rgba(0,0,0,0.2)',
                                background: 'rgba(255,255,255,0.15)',
                                y: -5
                            }}
                            whileTap={{ scale: 0.95 }}
                            viewport={{ once: true, amount: 0.3 }}
                        >
                            {t('welcome.callToAction.docs')}
                        </MotionButton>
                    </Space>
                </div>
            </motion.div>

            {/* Projects Section */}
            <div ref={projectsRef} style={{ 
                padding: '100px 0', 
                background: 'linear-gradient(180deg, #1a1a1a 0%, #141414 100%)',
                position: 'relative',
                overflow: 'hidden'
            }}>
                {/* 装饰背景 */}
                <svg
                    style={{
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        width: '100%',
                        height: '100%',
                        zIndex: 0,
                        opacity: 0.03
                    }}
                >
                    <defs>
                        <pattern id="smallGrid" width="20" height="20" patternUnits="userSpaceOnUse">
                            <path d="M 20 0 L 0 0 0 20" fill="none" stroke="white" strokeWidth="0.5" />
                        </pattern>
                        <pattern id="grid" width="100" height="100" patternUnits="userSpaceOnUse">
                            <rect width="100" height="100" fill="url(#smallGrid)" />
                            <path d="M 100 0 L 0 0 0 100" fill="none" stroke="white" strokeWidth="1" />
                        </pattern>
                    </defs>
                    <rect width="100%" height="100%" fill="url(#grid)" />
                </svg>
                
                <div style={{ maxWidth: 1200, margin: '0 auto', padding: '0 24px', position: 'relative', zIndex: 1 }}>
                    <motion.div
                      initial={{ opacity: 0, y: 40 }}
                      animate={projectsInView ? { opacity: 1, y: 0 } : { opacity: 0, y: 40 }}
                      transition={{ duration: 0.8 }}
                    >
                        <Title level={2} style={{ 
                            textAlign: 'center', 
                            marginBottom: 16, 
                            color: 'white',
                            fontSize: 36,
                            fontWeight: 800
                        }}>{t('welcome.projects.title')}</Title>
                        <Paragraph style={{ 
                            textAlign: 'center', 
                            color: '#aaa', 
                            marginBottom: 60, 
                            maxWidth: 700, 
                            margin: '0 auto 60px',
                            fontSize: 16,
                            lineHeight: 1.6
                        }}>
                            {t('welcome.projects.description')}
                        </Paragraph>
                    </motion.div>
                    <MotionRow 
                      gutter={[40, 40]}
                      variants={containerVariants}
                      initial="hidden"
                      animate={projectsInView ? "visible" : "hidden"}
                    >
                        <MotionCol xs={24} md={12} variants={cardVariants}>
                            <motion.div
                              whileHover={{ 
                                y: -15,
                                boxShadow: '0 20px 40px rgba(0,0,0,0.3), 0 0 80px rgba(24, 144, 255, 0.1)'
                              }}
                              transition={{ duration: 0.5 }}
                              style={{ height: '100%' }}
                            >
                                <Card 
                                  bordered={false}
                                  style={{
                                      height: '100%',
                                      background: 'rgba(30, 30, 30, 0.8)',
                                      backdropFilter: 'blur(10px)',
                                      border: '1px solid rgba(255, 255, 255, 0.05)',
                                      borderRadius: 20,
                                      overflow: 'hidden'
                                  }}
                                  bodyStyle={{ padding: 30 }}
                                  cover={
                                    <div style={{ 
                                        height: 160,
                                        background: 'linear-gradient(135deg, #1890ff 0%, #096dd9 100%)',
                                        display: 'flex',
                                        alignItems: 'center',
                                        justifyContent: 'center'
                                    }}>
                                        <motion.div
                                            initial={{ scale: 0.8, opacity: 0.5 }}
                                            animate={{ scale: 1, opacity: 1 }}
                                            transition={{
                                                duration: 0.8,
                                                ease: "easeOut"
                                            }}
                                        >
                                            <svg width="120" height="120" viewBox="0 0 200 200" fill="none" xmlns="http://www.w3.org/2000/svg">
                                                <path d="M100 20C55.8 20 20 55.8 20 100C20 144.2 55.8 180 100 180C144.2 180 180 144.2 180 100C180 55.8 144.2 20 100 20ZM100 160C66.9 160 40 133.1 40 100C40 66.9 66.9 40 100 40C133.1 40 160 66.9 160 100C160 133.1 133.1 160 100 160Z" fill="white" fillOpacity="0.5"/>
                                                <circle cx="100" cy="100" r="40" fill="white"/>
                                            </svg>
                                        </motion.div>
                                    </div>
                                  }
                                >
                                    <Title level={4} style={{ color: 'white', marginBottom: 15 }}>{t('welcome.projects.aidotnet.title')}</Title>
                                    <Paragraph style={{ color: '#aaa', fontSize: 15, lineHeight: 1.6, marginBottom: 25 }}>
                                    {t('welcome.projects.aidotnet.description')}
                                </Paragraph>
                                <Button 
                                    onClick={()=>{
                                        window.open('https://github.com/AIDotNet/', '_blank')
                                    }}
                                        type="primary"
                                        style={{ 
                                            background: 'linear-gradient(90deg, #1890ff 0%, #096dd9 100%)',
                                            border: 'none',
                                            borderRadius: 8,
                                            height: 40
                                        }}
                                    >{t('welcome.projects.aidotnet.action')} <ArrowRightOutlined /></Button>
                                </Card>
                            </motion.div>
                        </MotionCol>
                        <MotionCol xs={24} md={12} variants={cardVariants}>
                            <motion.div
                              whileHover={{ 
                                y: -15,
                                boxShadow: '0 20px 40px rgba(0,0,0,0.3), 0 0 80px rgba(114, 46, 209, 0.1)'
                              }}
                              transition={{ duration: 0.5 }}
                              style={{ height: '100%' }}
                            >
                                <Card 
                                  bordered={false}
                                  style={{
                                      height: '100%',
                                      background: 'rgba(30, 30, 30, 0.8)',
                                      backdropFilter: 'blur(10px)',
                                      border: '1px solid rgba(255, 255, 255, 0.05)',
                                      borderRadius: 20,
                                      overflow: 'hidden'
                                  }}
                                  bodyStyle={{ padding: 30 }}
                                  cover={
                                    <div style={{ 
                                        height: 160,
                                        background: 'linear-gradient(135deg, #722ed1 0%, #531dab 100%)',
                                        display: 'flex',
                                        alignItems: 'center',
                                        justifyContent: 'center'
                                    }}>
                                        <motion.div
                                            initial={{ scale: 0.8, opacity: 0.5 }}
                                            animate={{ scale: 1, opacity: 1 }}
                                            transition={{
                                                duration: 0.8,
                                                ease: "easeOut",
                                                delay: 0.2
                                            }}
                                        >
                                            <svg width="120" height="120" viewBox="0 0 200 200" fill="none" xmlns="http://www.w3.org/2000/svg">
                                                <rect x="40" y="40" width="120" height="120" rx="10" stroke="white" strokeWidth="8" strokeOpacity="0.5"/>
                                                <path d="M70 100H130M100 70V130" stroke="white" strokeWidth="8" strokeLinecap="round"/>
                                            </svg>
                                        </motion.div>
                                    </div>
                                  }
                                >
                                    <Title level={4} style={{ color: 'white', marginBottom: 15 }}>{t('welcome.projects.fastwiki.title')}</Title>
                                    <Paragraph style={{ color: '#aaa', fontSize: 15, lineHeight: 1.6, marginBottom: 25 }}>
                                    {t('welcome.projects.fastwiki.description')}
                                </Paragraph>
                                <Button 
                                    onClick={()=>{
                                        window.open('https://github.com/AIDotNet/fast-wiki/', '_blank')
                                    }}
                                        type="primary" 
                                        style={{ 
                                            background: 'linear-gradient(90deg, #722ed1 0%, #531dab 100%)',
                                            border: 'none',
                                            borderRadius: 8,
                                            height: 40
                                        }}
                                    >{t('welcome.projects.fastwiki.action')} <ArrowRightOutlined /></Button>
                                </Card>
                            </motion.div>
                        </MotionCol>
                    </MotionRow>
                </div>
            </div>
        </Content>
    );
};

export default ThorWebsite;
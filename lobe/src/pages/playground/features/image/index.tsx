import { useState, useEffect } from 'react';
import { Card, Input, Button, Typography, Select, Form, Upload, message, Divider, Row, Col, Image, Grid, Empty } from 'antd';
import { PictureOutlined, DeleteOutlined, InboxOutlined } from '@ant-design/icons';
import { Flexbox } from 'react-layout-kit';
import { useTranslation } from 'react-i18next';
import { getTokens } from '../../../../services/TokenService';
import { isMobileDevice } from '../../../../utils/responsive';
import type { RcFile, UploadFile, UploadProps } from 'antd/es/upload/interface';
import { processImage } from '../../../../services/ImageService';
const { Option } = Select;
const { TextArea } = Input;
const { Text } = Typography;
const { useBreakpoint } = Grid;
const { Dragger } = Upload;

// Interface for generated image
interface GeneratedImage {
    id: string;
    url: string;
    prompt: string;
    model: string;
    timestamp: number;
    size: string;
}

// Interface for source image
interface SourceImage {
    uid: string;
    file: RcFile;
    dataUrl: string;
}

export default function ImageFeature({ modelInfo }: { modelInfo: any }) {
    const { t } = useTranslation();
    const screens = useBreakpoint();
    const isMobile = !screens.md || isMobileDevice();

    const [tokenOptions, setTokenOptions] = useState<any[]>([]);
    const [selectedToken, setSelectedToken] = useState<string>('');
    const [modelOptions, setModelOptions] = useState<string[]>([]);
    const [selectedModel, setSelectedModel] = useState<string>('');
    const [prompt, setPrompt] = useState<string>('');
    const [loading, setLoading] = useState(false);
    const [generatedImages, setGeneratedImages] = useState<GeneratedImage[]>([]);
    const [imageSize, setImageSize] = useState<string>('1024x1024');
    const [sourceImages, setSourceImages] = useState<SourceImage[]>([]);
    const [fileList, setFileList] = useState<UploadFile[]>([]);

    useEffect(() => {
        // Initial loading of tokens and models
        fetchTokens();
        // Set model options from modelInfo
        if (modelInfo && modelInfo.modelTypes) {
            const imageModels = modelInfo.modelTypes.find((type: any) => type.type === 'image')?.models || [];
            setModelOptions(imageModels);
            if (imageModels.length > 0) {
                setSelectedModel(imageModels[0]);
            }
        }
    }, [modelInfo]);

    const fetchTokens = async () => {
        try {
            const res = await getTokens(1, 100);
            if (res && res.data && res.data.items) {
                setTokenOptions(res.data.items);
                if (res.data.items.length > 0) {
                    const firstToken = res.data.items[0].key;
                    setSelectedToken(firstToken);
                } else {
                    message.warning(t('imageFeature.noTokensAvailable'));
                }
            }
        } catch (error) {
            console.error('Failed to fetch tokens:', error);
            message.error(t('common.error'));
        }
    };

    // Handle token selection change
    const handleTokenChange = (value: string) => {
        setSelectedToken(value);
    };

    // Handle file upload for img2img
    const handleUpload: UploadProps['onChange'] = ({ fileList: newFileList }) => {
        setFileList(newFileList);

        // Process each file to create source images
        const processFiles = async () => {
            const newSourceImages: SourceImage[] = [];

            for (const file of newFileList) {
                if (file.originFileObj) {
                    try {
                        const dataUrl = await readFileAsDataURL(file.originFileObj);
                        newSourceImages.push({
                            uid: file.uid,
                            file: file.originFileObj as RcFile,
                            dataUrl
                        });
                    } catch (error) {
                        console.error('Failed to read file:', error);
                    }
                }
            }

            setSourceImages(newSourceImages);
        };

        processFiles();
    };

    // Convert file to data URL
    const readFileAsDataURL = (file: RcFile): Promise<string> => {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onload = () => resolve(reader.result as string);
            reader.onerror = reject;
            reader.readAsDataURL(file);
        });
    };

    // Clear all images
    const handleClearAllImages = () => {
        setFileList([]);
        setSourceImages([]);
    };

    // Generate image from text or transform existing image
    const handleGenerateImage = async () => {
        if (!prompt.trim()) {
            message.warning(t('imageFeature.promptRequired'));
            return;
        }

        if (!selectedToken) {
            message.warning(t('imageFeature.tokenRequired'));
            return;
        }

        if (!selectedModel) {
            message.warning(t('imageFeature.modelRequired'));
            return;
        }

        setLoading(true);

        try {
            const hasSourceImages = sourceImages.length > 0;

            // Base parameters for all requests
            const baseParams = {
                prompt,
                model: selectedModel,
                size: imageSize,
                token: selectedToken,
            };

            if (hasSourceImages) {
                // Process each source image
                for (const sourceImage of sourceImages) {
                    try {
                        // Source image already contains the data URL with prefix
                        const params = {
                            ...baseParams,
                            image: sourceImage.dataUrl, // Pass complete dataURL - our service will handle extraction
                        };

                        const result = await processImage(params);

                        if (result && result.data) {
                            // Add new image to the list
                            const newImage: GeneratedImage = {
                                id: result.data.id || Date.now().toString() + Math.random().toString(36).substring(2, 9),
                                url: result.data.b64_json?.startsWith("http") ? result.data.b64_json : `data:image/png;base64,${result.data.b64_json}`,
                                prompt,
                                model: selectedModel,
                                timestamp: Date.now(),
                                size: imageSize
                            };

                            setGeneratedImages(prev => [newImage, ...prev]);
                        }
                    } catch (error) {
                        console.error('Failed to process image:', error);
                        message.error(`Failed to process image: ${sourceImage.file.name}`);
                    }
                }

                message.success(t('imageFeature.generateSuccess'));
            } else {
                // Text-to-image generation using OpenAI
                const result = await processImage(baseParams);

                if (result && result.data) {
                    // Add new image to the list
                    const newImage: GeneratedImage = {
                        id: result.data.id || Date.now().toString(),
                        url: result.data.b64_json?.startsWith("http") ? result.data.b64_json : `data:image/png;base64,${result.data.b64_json}`,
                        prompt,
                        model: selectedModel,
                        timestamp: Date.now(),
                        size: imageSize
                    };

                    setGeneratedImages(prev => [newImage, ...prev]);
                    message.success(t('imageFeature.generateSuccess'));
                }
            }
        } catch (error: any) {
            console.error('Failed to generate image:', error);
            message.error(`${t('imageFeature.generateFailed')}: ${error.message || 'Unknown error'}`);
        } finally {
            setLoading(false);
        }
    };

    const hasSourceImages = sourceImages.length > 0;

    return (
        <Flexbox
            gap={0}
            style={{
                height: '100%',
                width: '100%',
                position: 'relative',
                overflow: 'auto'
            }}
        >
            <Card
                style={{
                    width: '100%',
                    height: '100%',
                    display: 'flex',
                    flexDirection: 'column',
                    border: 'none',
                    borderRadius: 0
                }}
                bodyStyle={{
                    flex: '1 1 auto',
                    padding: 16,
                    display: 'flex',
                    flexDirection: 'column',
                    overflow: 'auto',
                }}
            >
                <Flexbox direction="vertical" gap={16}>
                    <Typography.Title level={5}>{t('imageFeature.title')}</Typography.Title>

                    <Form layout="vertical">
                        <Form.Item label={t('imageFeature.sourceImages')}>
                            <Dragger
                                multiple
                                listType="picture"
                                fileList={fileList}
                                onChange={handleUpload}
                                beforeUpload={() => false}
                                disabled={loading}
                                accept="image/*"
                                style={{ padding: '20px 0' }}
                            >
                                <p className="ant-upload-drag-icon">
                                    <InboxOutlined />
                                </p>
                                <p className="ant-upload-text">
                                    {t('imageFeature.dragImageHint')}
                                </p>
                                <p className="ant-upload-hint">
                                    {t('imageFeature.supportMultipleFiles')}
                                </p>
                            </Dragger>

                            {fileList.length > 0 && (
                                <Button
                                    danger
                                    icon={<DeleteOutlined />}
                                    onClick={handleClearAllImages}
                                    style={{ marginTop: 8 }}
                                >
                                    {t('imageFeature.clearAllImages')}
                                </Button>
                            )}
                        </Form.Item>

                        <Form.Item label={hasSourceImages ? t('imageFeature.transformationPrompt') : t('imageFeature.prompt')}>
                            <TextArea
                                rows={4}
                                value={prompt}
                                onChange={(e) => setPrompt(e.target.value)}
                                placeholder={hasSourceImages ? t('imageFeature.transformationPromptPlaceholder') : t('imageFeature.promptPlaceholder')}
                            />
                        </Form.Item>

                        <Row gutter={16}>
                            <Col span={isMobile ? 24 : 8}>
                                <Form.Item label={t('imageFeature.token')}>
                                    <Select
                                        value={selectedToken}
                                        onChange={handleTokenChange}
                                        placeholder={t('imageFeature.selectToken')}
                                        style={{ width: '100%' }}
                                    >
                                        {tokenOptions.map((token) => (
                                            <Option key={token.key} value={token.key}>
                                                {token.name || token.key}
                                            </Option>
                                        ))}
                                    </Select>
                                </Form.Item>
                            </Col>
                            <Col span={isMobile ? 24 : 8}>
                                <Form.Item label={t('imageFeature.model')}>
                                    <Select
                                        value={selectedModel}
                                        onChange={(value) => setSelectedModel(value)}
                                        placeholder={t('imageFeature.selectModel')}
                                        style={{ width: '100%' }}
                                        disabled={!selectedToken}
                                    >
                                        {modelOptions.map((model) => (
                                            <Option key={model} value={model}>
                                                {model}
                                            </Option>
                                        ))}
                                    </Select>
                                </Form.Item>
                            </Col>
                            <Col span={isMobile ? 24 : 8}>
                                <Form.Item label={t('imageFeature.imageSize')}>
                                    <Select
                                        value={imageSize}
                                        onChange={(value) => setImageSize(value)}
                                        style={{ width: '100%' }}
                                    >
                                        <Option value="1024x1024">1024 x 1024 ({t('imageFeature.square')})</Option>
                                        <Option value="1792x1024">1792 x 1024 ({t('imageFeature.landscape')})</Option>
                                        <Option value="1024x1792">1024 x 1792 ({t('imageFeature.portrait')})</Option>
                                    </Select>
                                </Form.Item>
                            </Col>
                        </Row>

                        <Form.Item>
                            <Button
                                type="primary"
                                onClick={handleGenerateImage}
                                loading={loading}
                                icon={<PictureOutlined />}
                                block
                            >
                                {hasSourceImages ? t('imageFeature.transformImage') : t('imageFeature.generateImage')}
                            </Button>
                        </Form.Item>
                    </Form>

                    <Divider>{t('imageFeature.generatedImages')}</Divider>

                    {generatedImages.length === 0 ? (
                        <Empty description={t('imageFeature.noImagesYet')} />
                    ) : (
                        <Row gutter={[16, 16]}>
                            {generatedImages.map((img) => (
                                <Col key={img.id} xs={24} sm={12} md={8} lg={6}>
                                    <Card
                                        hoverable
                                        cover={
                                            <Image
                                                alt={img.prompt}
                                                src={img.url}
                                                style={{ objectFit: 'cover', height: 200 }}
                                            />
                                        }
                                        bodyStyle={{ padding: '8px 12px' }}
                                    >
                                        <div style={{ marginBottom: 8 }}>
                                            <Text ellipsis style={{ width: '100%' }} title={img.prompt}>
                                                {img.prompt}
                                            </Text>
                                        </div>
                                        <div>
                                            <Text type="secondary" style={{ fontSize: '0.8rem' }}>
                                                {img.model} • {img.size}
                                            </Text>
                                        </div>
                                    </Card>
                                </Col>
                            ))}
                        </Row>
                    )}
                </Flexbox>
            </Card>
        </Flexbox>
    );
} 
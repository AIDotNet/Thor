import { Modal, Tag } from "@lobehub/ui";
import { Button, Form, Input, InputNumber, message, Select, Space, Typography, Divider, Col, Row, Tooltip, theme } from "antd";
import { UpdateModelManager } from "../../../services/ModelManagerService";
import { getIconByNames } from "../../../utils/iconutils";
import { useState, useEffect } from "react";
import { renderQuota } from "../../../utils/render";
import { useTranslation } from "react-i18next";
import { useResponsive } from "antd-style";
import { InfoCircleOutlined, QuestionCircleOutlined } from "@ant-design/icons";
import { MODEL_TYPES } from "../constants/modelTypes";

interface UpdateModelManagerProps {
    open: boolean;
    onClose: () => void;
    onOk: () => void;
    value: any;
}

export default function UpdateModelManagerPage({
    open,
    onClose,
    onOk,
    value
}: UpdateModelManagerProps) {
    const { t } = useTranslation();
    const { mobile } = useResponsive();
    const { token } = theme.useToken();
    
    const [form] = Form.useForm();
    const [promptRate, setPromptRate] = useState(0);
    
    useEffect(() => {
        if (open && value) {
            form.setFieldsValue(value);
            setPromptRate(value.promptRate || 0);
        }
    }, [open, value, form]);

    function save(v: any) {
        v.id = value.id;
        UpdateModelManager(v)
            .then((res) => {
                if (res.success) {
                    message.success(t('modelManager.updateSuccess'));
                    onOk();
                } else {
                    message.error(res.message || t('modelManager.operationFailed'));
                }
            });
    }

    const formItemLayout = {
        labelCol: { span: 24 },
        wrapperCol: { span: 24 }
    };

    return (open && 
        <Modal 
            open={open} 
            onCancel={onClose} 
            onClose={onClose} 
            footer={null} 
            title={t('modelManager.updateModel')}
            style={{ maxWidth: '100%', width: mobile ? '100%' : '580px' }}
            bodyStyle={{ padding: '16px 24px' }}
        >
            <Form 
                form={form}
                onFinish={save} 
                initialValues={value}
                layout="vertical"
                requiredMark={false}
                size="large"
                {...formItemLayout}
                style={{ 
                    color: token.colorTextSecondary
                }}
            >
                <Typography.Title level={5} style={{ marginBottom: 16, color: token.colorTextSecondary }}>
                    {t('modelManager.basicInfo')}
                </Typography.Title>
                
                <Form.Item 
                    rules={[
                        {
                            required: true,
                            message: t('modelManager.modelNameRequired')
                        }
                    ]} 
                    name="model" 
                    label={
                        <Space>
                            {t('modelManager.modelName')}
                            {!mobile && (
                                <Tooltip title={t('modelManager.modelDescription')}>
                                    <QuestionCircleOutlined style={{ color: token.colorTextQuaternary, cursor: 'pointer' }} />
                                </Tooltip>
                            )}
                        </Space>
                    }
                >
                    <Input placeholder={t('modelManager.enterModelName')} />
                </Form.Item>
                
                <Row gutter={16}>
                    <Col span={mobile ? 24 : 12}>
                        <Form.Item 
                            rules={[
                                {
                                    required: true,
                                    message: t('modelManager.modelTypeRequired')
                                }
                            ]} 
                            name="quotaType" 
                            label={t('modelManager.modelType')}
                        >
                            <Select>
                                <Select.Option value={1}>{t('modelManager.volumeBilling')}</Select.Option>
                                <Select.Option value={2}>{t('modelManager.perUseBilling')}</Select.Option>
                            </Select>
                        </Form.Item>
                    </Col>
                    <Col span={mobile ? 24 : 12}>
                        <Form.Item 
                            name="isVersion2" 
                            label={t('modelManager.isRealTimeModel')}
                        >
                            <Select defaultValue={false}>
                                <Select.Option value={false}>{t('modelManager.no')}</Select.Option>
                                <Select.Option value={true}>{t('modelManager.yes')}</Select.Option>
                            </Select>
                        </Form.Item>
                    </Col>
                </Row>
                
                <Form.Item 
                    rules={[
                        {
                            required: true,
                            message: t('modelManager.typeRequired')
                        }
                    ]} 
                    name="type" 
                    label={t('modelManager.modelCategory')}
                >
                    <Select placeholder={t('modelManager.selectCategory')}>
                        <Select.Option value={MODEL_TYPES.CHAT}>{t('modelManager.typeChat')}</Select.Option>
                        <Select.Option value={MODEL_TYPES.AUDIO}>{t('modelManager.typeAudio')}</Select.Option>
                        <Select.Option value={MODEL_TYPES.IMAGE}>{t('modelManager.typeImage')}</Select.Option>
                        <Select.Option value={MODEL_TYPES.STT}>{t('modelManager.typeSTT')}</Select.Option>
                        <Select.Option value={MODEL_TYPES.TTS}>{t('modelManager.typeTTS')}</Select.Option>
                        <Select.Option value={MODEL_TYPES.EMBEDDING}>{t('modelManager.typeEmbedding')}</Select.Option>
                    </Select>
                </Form.Item>
                
                <Form.Item 
                    rules={[
                        {
                            required: true,
                            message: t('modelManager.descriptionRequired')
                        }
                    ]} 
                    name="description" 
                    label={t('modelManager.modelDescription')}
                >
                    <Input placeholder={t('modelManager.enterDescription')} />
                </Form.Item>
                
                <Row gutter={16}>
                    <Col span={mobile ? 24 : 12}>
                        <Form.Item 
                            rules={[
                                {
                                    required: true,
                                    message: t('modelManager.iconRequired')
                                }
                            ]} 
                            name="icon" 
                            label={t('modelManager.modelIcon')}
                        >
                            <Select 
                                options={getIconByNames(25)} 
                                placeholder={t('modelManager.selectIcon')}
                                showSearch
                                optionFilterProp="label"
                                dropdownStyle={{ 
                                    minWidth: '300px'
                                }}
                            />
                        </Form.Item>
                    </Col>
                    <Col span={mobile ? 24 : 12}>
                        <Form.Item 
                            name="tags" 
                            label={`${t('modelManager.modelTags')} (${t('common.optional')})`}
                        >
                            <Select
                                mode="tags"
                                options={[
                                    { label: '文本', value: '文本' },
                                    { label: "视觉", value: "视觉" },
                                    { label: "多模态", value: "多模态" },
                                    { label: "图像分析", value: "图像分析" },
                                    { label: "文件分析", value: "文件分析" }
                                ]}
                                placeholder={t('modelManager.enterTags')}
                                maxTagCount={3}
                                maxTagTextLength={10}
                                dropdownStyle={{ 
                                    minWidth: '300px'
                                }}
                            />
                        </Form.Item>
                    </Col>
                </Row>
                
                <Divider style={{ margin: '16px 0', borderColor: token.colorBorderSecondary }} />
                
                <Typography.Title level={5} style={{ marginBottom: 16, display: 'flex', alignItems: 'center', color: token.colorTextSecondary }}>
                    {t('modelManager.rateConfiguration')}
                    <Tooltip title="Configure pricing rates for this model">
                        <InfoCircleOutlined style={{ marginLeft: 8, fontSize: 14, color: token.colorTextQuaternary, cursor: 'pointer' }} />
                    </Tooltip>
                </Typography.Title>
                
                <Form.Item 
                    style={{ marginBottom: 0 }} 
                    shouldUpdate={(prevValues, currentValues) => prevValues.quotaType !== currentValues.quotaType}
                >
                    {({ getFieldValue }) => {
                        const quotaType = getFieldValue('quotaType');
                        return quotaType === 1 ? (
                            <Row gutter={16}>
                                <Col span={mobile ? 24 : 12}>
                                    <Form.Item
                                        rules={[
                                            {
                                                required: true,
                                                message: t('modelManager.promptRateRequired')
                                            },
                                            {
                                                validator: (_, value) => {
                                                    return value > 0 
                                                        ? Promise.resolve() 
                                                        : Promise.reject(t('modelManager.promptRatePositive'));
                                                }
                                            }
                                        ]}
                                        name="promptRate" 
                                        label={t('modelManager.promptRate')}
                                    >
                                        <InputNumber 
                                            style={{ width: '100%' }} 
                                            placeholder={t('modelManager.enterPromptRate')}
                                            min={0}
                                            step={0.1}
                                            precision={4}
                                        />
                                    </Form.Item>
                                </Col>
                                <Col span={mobile ? 24 : 12}>
                                    <Form.Item name="completionRate" label={t('modelManager.completionRate')}>
                                        <InputNumber 
                                            style={{ width: '100%' }} 
                                            placeholder={t('modelManager.enterCompletionRate')}
                                            min={0}
                                            step={0.1}
                                            precision={4}
                                        />
                                    </Form.Item>
                                </Col>
                            </Row>
                        ) : quotaType === 2 ? (
                            <Form.Item
                                rules={[
                                    {
                                        required: true,
                                        message: t('modelManager.promptRateRequired')
                                    },
                                    {
                                        validator: (_, value) => {
                                            return value > 0 
                                                ? Promise.resolve() 
                                                : Promise.reject(t('modelManager.promptRatePositive'));
                                        }
                                    },
                                ]} 
                                name="promptRate" 
                                label={t('modelManager.perUsageFee')}
                            >
                                <InputNumber
                                    onChange={(value) => setPromptRate(value ?? 0)}
                                    value={promptRate}
                                    suffix={<Tag>{renderQuota(promptRate, 6)}</Tag>}
                                    style={{ width: '100%' }} 
                                    placeholder={t('modelManager.enterPerUsageFee')}
                                    min={0}
                                    step={0.1}
                                    precision={4}
                                />
                            </Form.Item>
                        ) : null;
                    }}
                </Form.Item>
                
                <Divider style={{ margin: '16px 0', borderColor: token.colorBorderSecondary }} />
                
                <Typography.Title level={5} style={{ marginBottom: 16, color: token.colorTextSecondary }}>
                    {t('modelManager.advancedSettings')}
                </Typography.Title>
                
                <Row gutter={16}>
                    <Col span={mobile ? 24 : 12}>
                        <Form.Item name="audioPromptRate" label={t('modelManager.audioPromptRate')}>
                            <InputNumber 
                                style={{ width: '100%' }} 
                                placeholder={t('modelManager.enterAudioPromptRate')}
                                min={0}
                                step={0.1}
                                precision={4}
                            />
                        </Form.Item>
                    </Col>
                    <Col span={mobile ? 24 : 12}>
                        <Form.Item name="AudioOutputRate" label={t('modelManager.audioCompletionRate')}>
                            <InputNumber 
                                style={{ width: '100%' }} 
                                placeholder={t('modelManager.enterAudioCompletionRate')}
                                min={0}
                                step={0.1}
                                precision={4}
                            />
                        </Form.Item>
                    </Col>
                </Row>
                
                <Row gutter={16}>
                    <Col span={mobile ? 24 : 12}>
                        <Form.Item name="cacheRate" label={t('modelManager.cacheWriteRate')}>
                            <InputNumber 
                                style={{ width: '100%' }} 
                                placeholder={t('modelManager.enterCacheWriteRate')}
                                min={0}
                                step={0.1}
                                precision={4}
                            />
                        </Form.Item>
                    </Col>
                    <Col span={mobile ? 24 : 12}>
                        <Form.Item name="cacheHitRate" label={t('modelManager.cacheHitRate')}>
                            <InputNumber 
                                style={{ width: '100%' }} 
                                placeholder={t('modelManager.enterCacheHitRate')}
                                min={0}
                                step={0.1}
                                precision={4}
                            />
                        </Form.Item>
                    </Col>
                </Row>
                
                <Row gutter={16}>
                    <Col span={mobile ? 24 : 12}>
                        <Form.Item name="audioCacheRate" label={t('modelManager.audioCacheRate')}>
                            <InputNumber 
                                style={{ width: '100%' }} 
                                placeholder={t('modelManager.enterAudioCacheRate')}
                                min={0}
                                step={0.1}
                                precision={4}
                            />
                        </Form.Item>
                    </Col>
                    <Col span={mobile ? 24 : 12}>
                        <Form.Item name="quotaMax" label={t('modelManager.modelMaxContext')}>
                            <InputNumber 
                                style={{ width: '100%' }} 
                                placeholder={t('modelManager.enterMaxContext')}
                                min={0}
                                step={512}
                            />
                        </Form.Item>
                    </Col>
                </Row>
                
                <Divider style={{ margin: '16px 0', borderColor: token.colorBorderSecondary }} />
                
                <Form.Item style={{ marginBottom: 0, textAlign: 'right' }}>
                    <Space>
                        <Button onClick={onClose} size="middle">{t('common.cancel')}</Button>
                        <Button type="primary" htmlType="submit" size="middle">{t('common.update')}</Button>
                    </Space>
                </Form.Item>
            </Form>
        </Modal>
    );
}
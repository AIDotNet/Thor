import { Modal, Tag } from "@lobehub/ui";
import { Button, Form, Input, InputNumber, message, Select, Space } from "antd";
import { CreateModelManager } from "../../../services/ModelManagerService";
import { getIconByNames } from "../../../utils/iconutils";
import { useState } from "react";
import { renderQuota } from "../../../utils/render";
import { useTranslation } from "react-i18next";
import { useResponsive } from "antd-style";

interface CreateModelManagerProps {
    open: boolean;
    onClose: () => void;
    onOk: () => void;
}

export default function CreateModelManagerPage({
    open,
    onClose,
    onOk
}: CreateModelManagerProps) {
    const { t } = useTranslation();
    const { mobile } = useResponsive();
    
    const [form] = Form.useForm();
    const [promptRate, setPromptRate] = useState(0);

    function save(value: any) {
        CreateModelManager(value)
            .then((res) => {
                if (res.success) {
                    message.success(t('modelManager.createSuccess'));
                    onOk();
                } else {
                    message.error(res.message || t('modelManager.operationFailed'));
                }
            });
    }

    return (
        <Modal 
            open={open} 
            onCancel={onClose} 
            onClose={onClose} 
            footer={[]} 
            title={t('modelManager.createModel')}
            style={{ maxWidth: '100%' }}
        >
            <Form 
                form={form} 
                onFinish={save}
                layout={mobile ? "vertical" : "horizontal"}
                labelCol={mobile ? undefined : { span: 8 }}
                wrapperCol={mobile ? undefined : { span: 16 }}
            >
                <Form.Item 
                    rules={[
                        {
                            required: true,
                            message: t('modelManager.modelNameRequired')
                        }
                    ]} 
                    name="model" 
                    label={t('modelManager.modelName')}
                >
                    <Input placeholder={t('modelManager.enterModelName')} />
                </Form.Item>
                
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
                
                <Form.Item 
                    style={{ padding: 0, margin: 0 }} 
                    shouldUpdate={(prevValues, currentValues) => prevValues.quotaType !== currentValues.quotaType}
                >
                    {({ getFieldValue }) => {
                        const quotaType = getFieldValue('quotaType');
                        return quotaType === 1 ? (
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
                                label={t('modelManager.promptRate')}
                            >
                                <InputNumber 
                                    style={{ width: '100%' }} 
                                    placeholder={t('modelManager.enterPromptRate')}
                                />
                            </Form.Item>
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
                                style={{ padding: 0, margin: 0 }}
                                name="promptRate" 
                                label={t('modelManager.perUsageFee')}
                            >
                                <InputNumber
                                    onChange={(value) => setPromptRate(value ?? 0)}
                                    value={promptRate}
                                    suffix={<Tag>{renderQuota(promptRate, 6)}</Tag>}
                                    style={{ width: '100%' }} 
                                    placeholder={t('modelManager.enterPerUsageFee')}
                                />
                            </Form.Item>
                        ) : null;
                    }}
                </Form.Item>
                
                <Form.Item 
                    style={{ padding: 0, margin: 0 }} 
                    shouldUpdate={(prevValues, currentValues) => prevValues.quotaType !== currentValues.quotaType}
                >
                    {({ getFieldValue }) => {
                        return getFieldValue('quotaType') === 1 ? (
                            <Form.Item name="completionRate" label={t('modelManager.completionRate')}>
                                <InputNumber 
                                    style={{ width: '100%' }} 
                                    placeholder={t('modelManager.enterCompletionRate')}
                                />
                            </Form.Item>
                        ) : null;
                    }}
                </Form.Item>

                <Form.Item name="isVersion2" label={t('modelManager.isRealTimeModel')}>
                    <Select defaultValue={false}>
                        <Select.Option value={true}>{t('modelManager.yes')}</Select.Option>
                        <Select.Option value={false}>{t('modelManager.no')}</Select.Option>
                    </Select>
                </Form.Item>
                
                <Form.Item name="audioPromptRate" label={t('modelManager.audioPromptRate')}>
                    <Input placeholder={t('modelManager.enterAudioPromptRate')} />
                </Form.Item>
                
                <Form.Item name="AudioOutputRate" label={t('modelManager.audioCompletionRate')}>
                    <Input placeholder={t('modelManager.enterAudioCompletionRate')} />
                </Form.Item>
                
                <Form.Item name="cacheRate" label={t('modelManager.cacheWriteRate')}>
                    <InputNumber 
                        style={{ width: '100%' }} 
                        placeholder={t('modelManager.enterCacheWriteRate')} 
                    />
                </Form.Item>
                
                <Form.Item name="cacheHitRate" label={t('modelManager.cacheHitRate')}>
                    <InputNumber 
                        style={{ width: '100%' }} 
                        placeholder={t('modelManager.enterCacheHitRate')} 
                    />
                </Form.Item>
                
                <Form.Item name="audioCacheRate" label={t('modelManager.audioCacheRate')}>
                    <InputNumber 
                        style={{ width: '100%' }} 
                        placeholder={t('modelManager.enterAudioCacheRate')} 
                    />
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
                
                <Form.Item name="quotaMax" label={t('modelManager.modelMaxContext')}>
                    <Input placeholder={t('modelManager.enterMaxContext')} />
                </Form.Item>
                
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
                    <Select options={getIconByNames(25)} placeholder={t('modelManager.selectIcon')} />
                </Form.Item>
                
                <Form.Item name="tags" label={t('modelManager.modelTags')}>
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
                    />
                </Form.Item>
                
                <Form.Item wrapperCol={mobile ? undefined : { offset: 8, span: 16 }}>
                    <Space style={{ width: '100%', justifyContent: 'flex-end' }}>
                        <Button onClick={onClose}>{t('common.cancel')}</Button>
                        <Button type="primary" htmlType="submit">{t('modelManager.confirm')}</Button>
                    </Space>
                </Form.Item>
            </Form>
        </Modal>
    );
}
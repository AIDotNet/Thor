import { useCallback } from 'react';
import { useTranslation } from 'react-i18next';
import { Dropdown, Button } from 'antd';
import { GlobalOutlined } from '@ant-design/icons';

/**
 * 语言切换器组件
 */
const LanguageSwitcher = () => {
  const { i18n } = useTranslation();

  const changeLanguage = useCallback((lng: string) => {
    i18n.changeLanguage(lng);
  }, [i18n]);

  const items = [
    {
      key: 'zh-CN',
      label: '简体中文',
      onClick: () => changeLanguage('zh-CN'),
    },
    {
      key: 'en-US',
      label: 'English',
      onClick: () => changeLanguage('en-US'),
    },
  ];

  return (
    <Dropdown menu={{ items }}>
      <Button type="text" icon={<GlobalOutlined />} />
    </Dropdown>
  );
};

export default LanguageSwitcher; 
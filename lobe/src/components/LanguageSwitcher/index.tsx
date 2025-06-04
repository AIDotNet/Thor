import { Select } from 'antd';
import { useTranslation } from 'react-i18next';
import { GlobalOutlined } from '@ant-design/icons';

const { Option } = Select;

interface LanguageSwitcherProps {
  size?: 'small' | 'middle' | 'large';
  style?: React.CSSProperties;
}

const LanguageSwitcher: React.FC<LanguageSwitcherProps> = ({ 
  size = 'middle', 
  style 
}) => {
  const { i18n } = useTranslation();

  const handleLanguageChange = (language: string) => {
    i18n.changeLanguage(language);
    localStorage.setItem('language', language);
  };

  const languages = [
    { code: 'zh-CN', name: '中文', flag: '🇨🇳' },
    { code: 'en-US', name: 'English', flag: '🇺🇸' }
  ];

  return (
    <Select
      value={i18n.language}
      onChange={handleLanguageChange}
      size={size}
      style={{ minWidth: 120, ...style }}
      suffixIcon={<GlobalOutlined />}
      dropdownStyle={{ minWidth: 140 }}
    >
      {languages.map(lang => (
        <Option key={lang.code} value={lang.code}>
          <span style={{ marginRight: 8 }}>{lang.flag}</span>
          {lang.name}
        </Option>
      ))}
    </Select>
  );
};

export default LanguageSwitcher; 
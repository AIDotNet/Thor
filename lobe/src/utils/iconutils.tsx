import { IconAvatar } from "@lobehub/icons";

import {
    OpenAI,
    ChatGLM,
    Claude,
    Baichuan,
    Ai21,
    Google,
    Grok,
    Hunyuan,
    Minimax,
    Spark,
    Moonshot,
    Wenxin,
    Yi,
    Zhipu,
    DeepSeek,
    Qingyan,
    Qwen,
    Meta,
    Gemini,
    Ollama,
    SiliconCloud,
    Gemma,
    AssemblyAI,
    Doubao,
    Stability
} from '@lobehub/icons';

export function getIconByNames(size: number = 24) {
    const models = [
        'OpenAI',
        'ChatGLM',
        'Claude',
        'Baichuan',
        'Ai21',
        'Google',
        'Grok',
        'Hunyuan',
        'Minimax',
        'Spark',
        'Wenxin',
        'Yi',
        'Zhipu',
        "Moonshot",
        'DeepSeek',
        "Qingyan",
        "Qwen",
        "SiliconCloud",
        "Gemini",
        "Ollama",
        "Meta",
        "Yi",
        "Zhipu",
        "Wenxin",
        "Gemma",
        "AssemblyAI",
        "Doubao",
        "Stability"
    ]

    return models.map((name) => {
        const icon = getIconByName(name, size);
        return {
            label: <div style={{
                display: 'flex',
                alignItems: 'center',
            }}>
                {icon?.icon}
                <span style={{
                    marginLeft: 8,

                }}>{icon?.label}</span>
            </div>,
            value: name,
        }
    })
}

// 根据名称获取对应的图标
export function getIconByName(name: string, size: number = 36) {
    switch (name) {
        case 'OpenAI':
            return {
                icon: <IconAvatar Icon={OpenAI} size={size} />,
                label: 'OpenAI'
            };
        case 'ChatGLM':
            return {
                icon: <IconAvatar Icon={ChatGLM} size={size} />,
                label: 'ChatGLM'
            };
        case 'Claude':
            return {
                icon: <IconAvatar Icon={Claude} size={size} />,
                label: 'Claude'
            };
        case 'Baichuan':
            return {
                icon: <IconAvatar Icon={Baichuan} size={size} />,
                label: '百川'
            };
        case 'Ai21':
            return {
                icon: <IconAvatar Icon={Ai21} size={size} />,
                label: 'Ai21'
            };
        case 'Google':
            return {
                icon: <IconAvatar Icon={Google} size={size} />,
                label: 'Google'
            };
        case 'Grok':
            return {
                icon: <IconAvatar Icon={Grok} size={size} />,
                label: 'Grok'
            };
        case 'Hunyuan':
            return {
                icon: <IconAvatar Icon={Hunyuan} size={size} />,
                label: '混元'
            };
        case 'Minimax':
            return {
                icon: <IconAvatar Icon={Minimax} size={size} />,
                label: 'Minimax'
            };
        case 'Spark':
            return {
                icon: <IconAvatar Icon={Spark} size={size} />,
                label: 'Spark'
            };
        case 'Wenxin':
            return {
                icon: <IconAvatar Icon={Wenxin} size={size} />,
                label: '文心一言'
            };
        case 'Yi':
            return {
                icon: <IconAvatar Icon={Yi} size={size} />,
                label: 'Yi'
            };
        case 'Moonshot':
            return {
                icon: <IconAvatar Icon={Moonshot} size={size} />,
                label: '月之暗面'
            };
        case 'DeepSeek':
            return {
                icon: <IconAvatar Icon={DeepSeek} size={size} />,
                label: '深度求索'
            };
        case 'Qingyan':
            return {
                icon: <IconAvatar Icon={Qingyan} size={size} />,
                label: 'Qingyan'
            };
        case 'Qwen':
            return {
                icon: <IconAvatar Icon={Qwen} size={size} />,
                label: 'Qwen'
            };
        case 'SiliconCloud':
            return {
                icon: <IconAvatar Icon={SiliconCloud} size={size} />,
                label: '硅基流动'
            };  
        case 'Gemini':
            return {
                icon: <IconAvatar Icon={Gemini} size={size} />,
                label: 'Gemini'
            };  
        case 'Ollama':
            return {
                icon: <IconAvatar Icon={Ollama} size={size} />,
                label: 'Ollama'
            };
        case 'Meta':    
            return {
                icon: <IconAvatar Icon={Meta} size={size} />,
                label: 'Meta'
            };
        case 'Zhipu':   
            return {
                icon: <IconAvatar Icon={Zhipu} size={size} />,
                label: 'Zhipu'
            };
        case 'Gemma':
            return {
                icon: <IconAvatar Icon={Gemma} size={size} />,
                label: 'Gemma'
            };
        case 'AssemblyAI':
            return {
                icon: <IconAvatar Icon={AssemblyAI} size={size} />,
                label: 'AssemblyAI'
            };
        case 'Doubao':
            return {
                icon: <IconAvatar Icon={Doubao} size={size} />,
                label: 'Doubao'
            };
        case 'Stability':
            return {
                icon: <IconAvatar Icon={Stability} size={size} />,
                label: 'Stability'
            };
        default:
            return {
                icon: <IconAvatar Icon={OpenAI} size={size} />,
                label: 'OpenAI'
            };
    }
}
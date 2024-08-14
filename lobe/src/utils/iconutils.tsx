import { IconAvatar } from "@lobehub/icons";

import {
    OpenAI,
    ChatGLM,
    Claude,
    CodeGeeX,
    Baichuan,
    Ai21,
    Doubao,
    Google,
    Grok,
    Hunyuan,
    Minimax,
    Spark,
    Stepfun,
    Tongyi,
    Moonshot,
    Wenxin,
    Yi,
    Zhipu,
} from '@lobehub/icons';

export function getIconByNames(size: number = 24) {
    const models = [
        'OpenAI',
        'ChatGLM',
        'Claude',
        'CodeGeeX',
        'Baichuan',
        'Ai21',
        'Doubao',
        'Google',
        'Grok',
        'Hunyuan',
        'Minimax',
        'Spark',
        'Stepfun',
        'Tongyi',
        'Wenxin',
        'Yi',
        'Zhipu',
        "Moonshot"
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
        case 'CodeGeeX':
            return {
                icon: <IconAvatar Icon={CodeGeeX} size={size} />,
                label: 'CodeGeeX'
            };
        case 'Baichuan':
            return {
                icon: <IconAvatar Icon={Baichuan} size={size} />,
                label: 'Baichuan'
            };
        case 'Ai21':
            return {
                icon: <IconAvatar Icon={Ai21} size={size} />,
                label: 'Ai21'
            };
        case 'Doubao':
            return {
                icon: <IconAvatar Icon={Doubao} size={size} />,
                label: '豆包'
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
                label: 'Hunyuan'
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
        case 'Stepfun':
            return {
                icon: <IconAvatar Icon={Stepfun} size={size} />,
                label: 'Stepfun'
            };
        case 'Tongyi':
            return {
                icon: <IconAvatar Icon={Tongyi} size={size} />,
                label: 'Tongyi'
            };
        case 'Wenxin':
            return {
                icon: <IconAvatar Icon={Wenxin} size={size} />,
                label: 'Wenxin'
            };
        case 'Yi':
            return {
                icon: <IconAvatar Icon={Yi} size={size} />,
                label: 'Yi'
            };
        case 'Zhipu':
            return {
                icon: <IconAvatar Icon={Zhipu} size={size} />,
                label: 'Zhipu'
            };
        default:
            return null;
    }
}
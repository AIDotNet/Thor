import { useEffect, useState } from "react"
import { GeneralSetting, InitSetting } from "../../services/SettingService"

export default function Chat() {
    const [chatLink, setChatLink] = useState('' as string)

    useEffect(() => {
        const chatLink = InitSetting?.find(x => x.key === GeneralSetting.ChatLink)?.value ?? ''
        
        // TODO: 定义规则，嵌入Chat外部链接的时候会自动带上token。这里需要根据规则进行处理
        const token = localStorage.getItem('token')
        if (token && chatLink) {
            const url = new URL(chatLink)
            url.searchParams.append('token', token)
            setChatLink(url.toString())
        } else {
            setChatLink(chatLink ?? '')
        }

    }, [])

    return (
        <iframe
            src={chatLink}
            style={{
                width: '100%',
                height: '100%',
                border: 'none'
            }}
        >

        </iframe>
    )
}
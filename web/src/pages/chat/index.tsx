import { useEffect, useState } from "react"
import { GeneralSetting, InitSetting } from "../../services/SettingService"

export default function Chat(){
    const [chatLink, setChatLink] = useState('' as string)

    useEffect(() => {
        const chatLink = InitSetting?.find(x => x.key === GeneralSetting.ChatLink)?.value
        setChatLink(chatLink ?? '')
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
import { useEffect, useState } from "react"
import { GeneralSetting, InitSetting } from "../../services/SettingService"

export default function Vidol() {
    const [vidol, setVidol] = useState('' as string)

    useEffect(() => {
        const vidol = InitSetting?.find(x => x.key === GeneralSetting.VidolLink)?.value ?? ''
        
        // TODO: 定义规则，嵌入Chat外部链接的时候会自动带上token。这里需要根据规则进行处理
        const token = localStorage.getItem('token')
        if (token && vidol) {
            const url = new URL(vidol)
            url.searchParams.append('token', token)
            setVidol(url.toString())
        } else {
            setVidol(vidol ?? '')
        }

    }, [])

    return (
        <iframe
            src={vidol}
            style={{
                width: '100%',
                height: '100vh',
                border: 'none'
            }}
        >

        </iframe>
    )
}
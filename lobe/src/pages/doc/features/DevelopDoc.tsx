import { Markdown } from "@lobehub/ui";
import { useEffect, useState } from "react";

interface DocsMarkdownProps {
    doc: string;
}

export default function DocsMarkdown({
    doc
}:DocsMarkdownProps) {
    const [markdown, setMarkdown] = useState<string>('')

    function loadDoc() {
        fetch("/docs/"+doc)
            .then(res => res.text())
            .then(setMarkdown)
    }

    useEffect(() => {
        loadDoc()
    }  , [doc])

    return (<>
        <Markdown
            style={{
                overflow: 'auto',
                height: 'calc(100vh - 120px)'
            }}
            variant='chat'
            children={markdown}
        />
    </>)
}
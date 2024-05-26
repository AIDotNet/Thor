import { Banner, Collapse, TabPane, Tabs, Tag } from "@douyinfe/semi-ui";
import ReactMarkdown from 'react-markdown'
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { vscDarkPlus } from 'react-syntax-highlighter/dist/esm/styles/prism';// 代码高亮主题风格



export default function Doc() {
    const doc =
        `

所有接口与openai请求传参完全一致。可查阅[OpenAI官方文档](https://platform.openai.com/docs/introduction/overview)

请求接口时将\`https://api.openai.com\`改为我们的API地址 

查看API地址，若因网络问题连接失败请更换api地址为备用地址。

部分应用需要在Api地址后面添加\`/v1\`、\`/v1/chat/completions\`等后缀，请注意。

### OpenAi官方python库

> 在openai官方库开发时传入baseurl和apikey即可。
> [openai-python](https://github.com/openai/openai-python)

以官网的\`python\`库为例：注意，需要传入\`/v1/\`后缀。如果例子失效 请查阅官方最新github仓库。

\`\`\`python
import os
import openai

openai.api_key = "您的apikey"

openai.base_url = "https://api.v3.cm/v1/"
openai.default_headers = {"x-foo": "true"}

completion = openai.chat.completions.create(
    model="gpt-3.5-turbo",
        messages=[
            {
                "role": "user",
                "content": "Hello world!",
            },
        ],
    )
print(completion.choices[0].message.content)

# 正常会输出结果：Hello there! How can I assist you today ?
\`\`\`

### OpenAi官方node库

> 在openai官方库开发时传入baseurl和apikey即可。
>
> [openai-node](https://github.com/openai/openai-node)

以官网的\`node\`库为例：注意，需要传入\`/v1\`后缀。如果例子失效 请查阅官方最新github仓库。

\`\`\`python
const { Configuration, OpenAIApi } = require("openai");

const configuration = new Configuration({
apiKey: "您的apikey",
basePath: "https://api.v3.cm/v1"
});
const openai = new OpenAIApi(configuration);

const chatCompletion = await openai.createChatCompletion({
model: "gpt-3.5-turbo",
messages: [{role: "user", content: "Hello world"}],
});

console.log(chatCompletion.data.choices[0].message.content);
\`\`\`
`;

    return (<Tabs tabPosition="left" type='card'>
        <TabPane
            tab={
                <span>
                    开发手册
                </span>
            }
            style={{
                padding: '24px',
                overflowY: 'auto',
                height: 'calc(100vh - 100px)',
            }}
            itemKey="1"
        >

            <Banner
                type="success"
                style={{
                    borderRadius: '8px',
                    marginBottom: '24px',
                }}
                closeIcon={null}
                description={<>
                    <span>我们的API，完全兼容OpenAI接口协议，支持无缝对接各种支持OpenAI接口的应用。</span>
                    <br />
                    <span style={{
                        fontWeight: 'bold',
                    }}>注意：所有聊天模型（包括非openai模型）都支持openai官方库，请求url和格式请都遵循openai的请求方式。</span>
                </>}
            />

            <ReactMarkdown
                components={{
                    code({ node, inline, className, children, ...props }: any) {
                        const match = /language-(\w+)/.exec(className || '')
                        return !inline && match ? (
                            <SyntaxHighlighter style={vscDarkPlus} language={match[1]} PreTag="div" children={String(children).replace(/\n$/, '')} {...props} />
                        ) : (
                            <code className={className} {...props}>
                                {children}
                            </code>
                        )
                    },
                }}
            >
                {doc}
            </ReactMarkdown>
        </TabPane>
        <TabPane
            style={{
                padding: '24px',
                overflowY: 'auto',
                height: 'calc(100vh - 100px)',
            }}
            tab={
                <span>
                    验证GPT4
                </span>
            }
            itemKey="2"
        >
            <Banner
                type='warning'
                style={{
                    borderRadius: '8px',
                    marginBottom: '24px',
                }}
                closeIcon={null}
                icon={null}
                description={<>
                    <span>GPT系列模型都是训练于2021年，那时候还没有GPT3.5和GPT4。</span>
                    <br />
                    <span>虽然版本迭代更新到了GPT4，但他们都是基于3.0模型的升级加强，模型依然只记得自己的版本是3.0，请按本站提供的方法进行验证GPT4。</span>
                    <br />
                    <span>目前只有GPT4 preview系列的模型知识库是2023年，您可以提问：“您的知识库截止于什么时候？”即可验证是否是真正的preview系列模型</span>
                </>}
            />
            <Collapse>
                <Collapse.Panel header="Q:鲁迅为什么暴打周树人？" itemKey="1">
                    <span style={{
                        display: 'flex'
                    }}>
                        <Tag style={{
                            marginRight: '8px'
                        }} color='blue'>GPT4</Tag>
                        <p> 表示鲁迅和周树人是同一个人。</p>
                    </span>
                    <br />
                    <span style={{
                        display: 'flex'
                    }}>
                        <Tag style={{
                            marginRight: '8px'
                        }} color="red">GPT3.5</Tag>
                        <p> 会一本正经的胡说八道。</p>
                    </span>
                </Collapse.Panel>
                <Collapse.Panel header="Q: 我爸妈结婚时为什么没有邀请我？" itemKey="2">
                    <span style={{
                        display: 'flex'
                    }}>
                        <Tag style={{
                            marginRight: '8px'
                        }} color='blue'>GPT4</Tag>
                        <p> 他们结婚时你还没出生。</p>
                    </span>
                    <br />
                    <span style={{
                        display: 'flex'
                    }}>
                        <Tag style={{
                            marginRight: '8px'
                        }} color="red">GPT3.5</Tag>
                        <p>  他们当时认为你还太小，所以没有邀请你。</p>
                    </span>
                </Collapse.Panel>
                <Collapse.Panel header="Q: What yesterday's today is tomorrow's?" itemKey="3">
                    <span style={{
                        display: 'flex'
                    }}>
                        <Tag style={{
                            marginRight: '8px'
                        }} color='blue'>GPT4</Tag>
                        <p> Past (前天)</p>
                    </span>
                    <br />
                    <span style={{
                        display: 'flex'
                    }}>
                        <Tag style={{
                            marginRight: '8px'
                        }} color="red">GPT3.5</Tag>
                        <p>   Yesterday (昨天)</p>
                    </span>
                </Collapse.Panel>
            </Collapse>
        </TabPane>
    </Tabs>

    )
}
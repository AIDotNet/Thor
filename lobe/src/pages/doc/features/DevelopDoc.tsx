import { Markdown, TocProps } from "@lobehub/ui";

const doc =
    `

所有接口与openai请求传参完全一致。可查阅[OpenAI官方文档](https://platform.openai.com/docs/introduction/overview)

请求接口时将\`https://api.openai.com\`改为我们的API地址 

查看API地址，若因网络问题连接失败请更换api地址为备用地址。

部分应用需要在Api地址后面添加\`/v1\`、\`/v1/chat/completions\`等后缀，请注意。

## OpenAi官方python库

> 在openai官方库开发时传入baseurl和apikey即可。
> [openai-python](https://github.com/openai/openai-python)

以官网的\`python\`库为例：注意，需要传入\`/v1/\`后缀。如果例子失效 请查阅官方最新github仓库。

\`\`\`python
import os
import openai

openai.api_key = "您的apikey"

openai.base_url = "https://api.token-ai.cn/v1/"
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

正常会输出结果：Hello there! How can I assist you today ?
\`\`\`

## OpenAi官方node库

> 在openai官方库开发时传入baseurl和apikey即可。
>
> [openai-node](https://github.com/openai/openai-node)

以官网的\`node\`库为例：注意，需要传入\`/v1\`后缀。如果例子失效 请查阅官方最新github仓库。

\`\`\`python
const { Configuration, OpenAIApi } = require("openai");

const configuration = new Configuration({
apiKey: "您的apikey",
basePath: "https://api.token-ai.cn/v1"
});
const openai = new OpenAIApi(configuration);

const chatCompletion = await openai.createChatCompletion({
model: "gpt-3.5-turbo",
messages: [{role: "user", content: "Hello world"}],
});

console.log(chatCompletion.data.choices[0].message.content);

\`\`\`

## C#接入示例

安装Nuget包

\`\`\`bash
Install-Package Betalgo.OpenAI
\`\`\`


\`\`\`csharp
var openAiService = new OpenAIService(new OpenAiOptions()
{
    ApiKey ="在https:///api.token-ai.cn/申请的apikey",
    BaseDomain = "https://api.token-ai.cn/"
});

var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
{
    Messages = new List<ChatMessage>
    {
        ChatMessage.FromSystem("You are a helpful assistant."),
        ChatMessage.FromUser("Who won the world series in 2020?"),
        ChatMessage.FromAssistant("The Los Angeles Dodgers won the World Series in 2020."),
        ChatMessage.FromUser("Where was it played?")
    },
    Model = Models.Gpt_4o,
});
if (completionResult.Successful)
{
    Console.WriteLine(completionResult.Choices.First().Message.Content);
}

\`\`\`


`;

export default function DevelopDoc() {
    return (<>
        <Markdown
            style={{
                overflow: 'auto',
                height: 'calc(100vh - 120px)'
            }}
            variant='chat'
            children={doc}
        />
    </>)
}
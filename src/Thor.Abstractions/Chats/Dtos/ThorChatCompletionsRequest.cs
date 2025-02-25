using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Thor.Abstractions.Chats.Consts;
using Thor.Abstractions.ObjectModels.ObjectModels;
using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace Thor.Abstractions.Chats.Dtos;

/// <summary>
/// 对话补全请求参数对象
/// </summary>
public class ThorChatCompletionsRequest : IOpenAiModels.ITemperature, IOpenAiModels.IModel, IOpenAiModels.IUser
{
    public ThorChatCompletionsRequest()
    {
        Messages = new List<ThorChatMessage>();
    }

    /// <summary>
    /// 包含迄今为止对话的消息列表
    /// </summary>
    [JsonPropertyName("messages")]
    public List<ThorChatMessage> Messages { get; set; }

    /// <summary>
    /// 模型唯一编码值，如 gpt-4，gpt-3.5-turbo,moonshot-v1-8k，看底层具体平台定义
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }

    /// <summary>
    /// 温度采样的替代方法称为核采样，介于 0 和 1 之间，其中模型考虑具有 top_p 概率质量的标记的结果。
    /// 因此 0.1 意味着仅考虑包含前 10% 概率质量的标记。
    /// 我们通常建议更改此项或 temperature ，但不要同时更改两者。
    /// 默认 1
    /// </summary>
    [JsonPropertyName("top_p")]
    public float? TopP { get; set; } 

    /// <summary>
    /// 使用什么采样温度，介于 0 和 2 之间。
    /// 较高的值（如 0.8）将使输出更加随机，而较低的值（如 0.2）将使其更加集中和确定性。
    /// 我们通常建议更改此项或 top_p ，但不要同时更改两者。
    /// 默认 1
    /// </summary>
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }

    /// <summary>
    /// 为每条输入消息生成多少个结果
    /// <para>
    /// 默认为 1，不得大于 5。特别的，当 temperature 非常小靠近 0 的时候，
    /// 我们只能返回 1 个结果，如果这个时候 n 已经设置并且 > 1，
    /// 我们的服务会返回不合法的输入参数(invalid_request_error)
    /// </para>
    /// </summary>
    [JsonPropertyName("n")]
    public int? N { get; set; } 

    /// <summary>
    /// 如果设置，将发送部分消息增量，就像在 ChatGPT 中一样。
    /// 令牌可用时将作为仅数据服务器发送事件发送，流由 data: [DONE] 消息终止。
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }

    /// <summary>
    /// 流响应选项。仅当您设置 stream: true 时才设置此项。
    /// </summary>
    [JsonPropertyName("stream_options")]
    public ThorStreamOptions? StreamOptions { get; set; }

    /// <summary>
    /// 停止词，当全匹配这个（组）词后会停止输出，这个（组）词本身不会输出。
    /// 最多不能超过 5 个字符串，每个字符串不得超过 32 字节,
    /// 默认 null
    /// </summary>
    [JsonIgnore]
    public string? Stop { get; set; }

    /// <summary>
    /// 停止词，当全匹配这个（组）词后会停止输出，这个（组）词本身不会输出。
    /// 最多不能超过 5 个字符串，每个字符串不得超过 32 字节,
    /// 默认 null
    /// </summary>
    [JsonIgnore]
    public IList<string>? StopAsList { get; set; }

    /// <summary>
    /// 停止词，当全匹配这个（组）词后会停止输出，这个（组）词本身不会输出。
    /// 最多不能超过 5 个字符串，每个字符串不得超过 32 字节,
    /// 默认 null
    /// </summary>
    [JsonPropertyName("stop")]
    public IList<string>? StopCalculated
    {
        get
        {
            if (Stop is not null && StopAsList is not null)
            {
                throw new ValidationException(
                    "Stop 和 StopAsList 不能同时有值，其中一个应该为 null");
            }

            if (Stop is not null)
            {
                return new List<string> { Stop };
            }

            return StopAsList;
        }
    }

    /// <summary>
    /// 生成的答案允许的最大令牌数。默认情况下，模型可以返回的令牌数量为（4096个提示令牌）。
    /// </summary>
    /// <see href="https://platform.openai.com/docs/api-reference/completions/create#completions/create-max_tokens" />
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    /// <summary>
    /// 可为补全生成的令牌数量的上限，包括可见输出令牌和推理令牌。
    /// </summary>
    [JsonPropertyName("max_completion_tokens")]
    public int? MaxCompletionTokens { get; set; }

    /// <summary>
    /// 存在惩罚，介于 -2.0 到 2.0 之间的数字。
    /// 正值会根据新生成的词汇是否出现在文本中来进行惩罚，增加模型讨论新话题的可能性,
    /// 默认为 0
    /// </summary>
    /// <seealso href="https://platform.openai.com/docs/api-reference/parameter-details" />
    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; set; }


    /// <summary>
    /// 频率惩罚，介于-2.0到2.0之间的数字。
    /// 正值会根据新生成的词汇在文本中现有的频率来进行惩罚，减少模型一字不差重复同样话语的可能性.
    /// 默认为 0
    /// </summary>
    /// <seealso href="https://platform.openai.com/docs/api-reference/parameter-details" />
    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; set; }

    /// <summary>
    /// 接受一个 JSON 对象，该对象将标记（由标记生成器中的标记 ID 指定）映射到从 -100 到 100 的关联偏差值。
    /// 从数学上讲，偏差会在采样之前添加到模型生成的 logits 中。
    /// 每个模型的确切效果会有所不同，但 -1 和 1 之间的值应该会降低或增加选择的可能性；
    /// 像 -100 或 100 这样的值应该会导致相关令牌的禁止或独占选择。
    /// </summary>
    /// <seealso href="https://platform.openai.com/tokenizer?view=bpe" />
    [JsonPropertyName("logit_bias")]
    public object? LogitBias { get; set; }

    /// <summary>
    /// 是否返回输出标记的对数概率。如果为 true，则返回 message 的 content 中返回的每个输出标记的对数概率。
    /// </summary>
    [JsonPropertyName("logprobs")]
    public bool? Logprobs { get; set; }

    /// <summary>
    /// 0 到 20 之间的整数，指定每个标记位置最有可能返回的标记数量，每个标记都有关联的对数概率。
    /// 如果使用此参数， logprobs 必须设置为 true 。
    /// </summary>
    [JsonPropertyName("top_logprobs")]
    public int? TopLogprobs { get; set; }

    /// <summary>
    /// 指定用于处理请求的延迟层。此参数与订阅规模层服务的客户相关：
    /// 如果设置为“auto”，系统将使用规模等级积分，直至用完。
    /// 如果设置为“default”，则将使用具有较低正常运行时间 SLA 且无延迟保证的默认服务层来处理请求。
    /// 默认null
    /// </summary>
    [JsonPropertyName("service_tier")]
    public string? ServiceTier { get; set; }

    /// <summary>
    /// 模型可能调用的工具列表。目前，仅支持函数作为工具。使用它来提供模型可以为其生成 JSON 输入的函数列表。最多支持 128 个功能。
    /// </summary>
    [JsonPropertyName("tools")]
    public List<ThorToolDefinition>? Tools { get; set; }


    /// <summary>
    /// 控制模型调用哪个（如果有）工具。
    /// none 表示模型不会调用任何工具，而是生成一条消息。 
    /// auto 表示模型可以在生成消息或调用一个或多个工具之间进行选择。 
    /// required 表示模型必须调用一个或多个工具。
    /// 通过 {"type": "function", "function": {"name": "my_function"}} 指定特定工具会强制模型调用该工具。
    /// 当不存在任何工具时， none 是默认值。如果存在工具，则 auto 是默认值。
    /// </summary>
    [JsonIgnore]
    public ThorToolChoice? ToolChoice { get; set; }

    [JsonPropertyName("tool_choice")]
    public object? ToolChoiceCalculated
    {
        get
        {
            if (ToolChoice != null &&
                ToolChoice.Type != ThorToolChoiceTypeConst.Function &&
                ToolChoice.Function != null)
            {
                throw new ValidationException(
                    "当 type 为 \"function\" 时，属性 Function 不可为null。");
            }

            if (ToolChoice?.Type == ThorToolChoiceTypeConst.Function)
            {
                return ToolChoice;
            }

            return ToolChoice?.Type;
        }
        set
        {
            if (value is JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == JsonValueKind.String)
                {
                    ToolChoice = new ThorToolChoice
                    {
                        Type = jsonElement.GetString()
                    };
                }
                else if (jsonElement.ValueKind == JsonValueKind.Object)
                {
                    ToolChoice = jsonElement.Deserialize<ThorToolChoice>();
                }
            }
            else
            {
                ToolChoice = (ThorToolChoice)value;
            }
        }
    }

    /// <summary>
    /// 设置为 {"type": "json_object"} 可启用 JSON 模式，从而保证模型生成的信息是有效的 JSON。
    /// 当你将 response_format 设置为 {"type": "json_object"} 时，
    /// 你需要在 prompt 中明确地引导模型输出 JSON 格式的内容，
    /// 并告知模型该 JSON 的具体格式，否则将可能导致不符合预期的结果。
    /// 默认为 {"type": "text"}
    /// </summary>
    [JsonPropertyName("response_format")]
    public ThorResponseFormat? ResponseFormat { get; set; }

    [JsonPropertyName("metadata")] public Dictionary<string, string> Metadata { get; set; }

    /// <summary>
    /// 此功能处于测试阶段。
    /// 如果指定，我们的系统将尽最大努力进行确定性采样，
    /// 以便具有相同 seed 和参数的重复请求应返回相同的结果。
    /// 不保证确定性，您应该参考 system_fingerprint 响应参数来监控后端的变化。
    /// </summary>
    [JsonPropertyName("seed")]
    public int? Seed { get; set; }

    /// <summary>
    /// 代表您的最终用户的唯一标识符，可以帮助 OpenAI 监控和检测滥用行为。
    /// </summary>
    [JsonPropertyName("user")]
    public string User { get; set; }

    [JsonPropertyName("thinking")] public ThorChatClaudeThinking Thinking { get; set; }

    /// <summary>
    /// 参数验证
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<ValidationResult> Validate()
    {
        throw new NotImplementedException();
    }
}
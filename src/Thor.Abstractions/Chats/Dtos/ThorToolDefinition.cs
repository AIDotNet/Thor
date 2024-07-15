using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenAI.ObjectModels;
using Thor.Abstractions.Chats.Consts;
using Thor.Abstractions.ObjectModels.ObjectModels;

namespace Thor.Abstractions.Chats.Dtos;

/// <summary>
/// 有效工具的定义。
/// </summary>
public class ThorToolDefinition
{
    /// <summary>
    /// 必修的。工具的类型。目前仅支持 function 。
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = ThorToolTypeConst.Function;

    /// <summary>
    /// 函数对象
    /// </summary>
    [JsonPropertyName("function")]
    public ThorToolFunctionDefinition? Function { get; set; }

    /// <summary>
    /// 创建函数工具
    /// </summary>
    /// <param name="function"></param>
    /// <returns></returns>
    public static ThorToolDefinition CreateFunctionTool(ThorToolFunctionDefinition function) => new()
    {
        Type = ThorToolTypeConst.Function,
        Function = function
    };
}
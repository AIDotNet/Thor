using System.Text.Json.Serialization;

namespace Thor.Abstractions.Chats.Dtos;

/// <summary>
/// 函数参数是JSON格式对象
/// https://json-schema.org/understanding-json-schema/reference/object.html
/// </summary>
/// <example>
/// 定义属性示例：
/// [JsonPropertyName("location")]
/// public ThorToolFunctionPropertyDefinition Location = ThorToolFunctionPropertyDefinition.DefineString("The city and state, e.g. San Francisco, CA");
/// 
/// [JsonPropertyName("unit")]
/// public ThorToolFunctionPropertyDefinition Unit = ThorToolFunctionPropertyDefinition.DefineEnum(["celsius", "fahrenheit"]);
/// </example>
public class ThorToolFunctionPropertyDefinition
{
    /// <summary>
    /// 定义了函数对象的类型枚举
    /// </summary>
    public enum FunctionObjectTypes
    {
        /// <summary>
        /// 表示字符串类型的函数对象
        /// </summary>
        String,
        /// <summary>
        /// 表示整数类型的函数对象
        /// </summary>
        Integer,
        /// <summary>
        /// 表示数字（包括浮点数等）类型的函数对象
        /// </summary>
        Number,
        /// <summary>
        /// 表示对象类型的函数对象
        /// </summary>
        Object,
        /// <summary>
        /// 表示数组类型的函数对象
        /// </summary>
        Array,
        /// <summary>
        /// 表示布尔类型的函数对象
        /// </summary>
        Boolean,
        /// <summary>
        /// 表示空值类型的函数对象
        /// </summary>
        Null
    }

    /// <summary>
    /// 必填的。函数参数对象类型。默认值为“object”。
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "object";

    /// <summary>
    /// 可选。“函数参数”列表，作为从参数名称映射的字典
    /// 对于描述类型的对象，可能还有可能的枚举值等等。
    /// </summary>
    [JsonPropertyName("properties")]
    public IDictionary<string, ThorToolFunctionPropertyDefinition>? Properties { get; set; }

    /// <summary>
    /// 可选。列出必需的“function arguments”列表。
    /// </summary>
    [JsonPropertyName("required")]
    public List<string>? Required { get; set; }

    /// <summary>
    /// 可选。是否允许附加属性。默认值为true。
    /// </summary>
    [JsonPropertyName("additionalProperties")]
    public bool? AdditionalProperties { get; set; }

    /// <summary>
    ///  可选。参数描述。
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// 可选。此参数的允许值列表。
    /// </summary>
    [JsonPropertyName("enum")]
    public List<string>? Enum { get; set; }

    /// <summary>
    /// 可以使用minProperties和maxProperties关键字限制对象上的属性数量。每一个
    /// 这些必须是非负整数。
    /// </summary>
    [JsonPropertyName("minProperties")]
    public int? MinProperties { get; set; }

    /// <summary>
    /// 可以使用minProperties和maxProperties关键字限制对象上的属性数量。每一个
    /// 这些必须是非负整数。
    /// </summary>
    [JsonPropertyName("maxProperties")]
    public int? MaxProperties { get; set; }

    /// <summary>
    /// 如果type为“array”，则指定数组中所有项目的元素类型。
    /// 如果类型不是“array”，则应为null。
    /// 有关更多详细信息，请参阅 https://json-schema.org/understanding-json-schema/reference/array.html
    /// </summary>
    [JsonPropertyName("items")]
    public ThorToolFunctionPropertyDefinition? Items { get; set; }

    /// <summary>
    /// 定义数组
    /// </summary>
    /// <param name="arrayItems"></param>
    /// <returns></returns>
    public static ThorToolFunctionPropertyDefinition DefineArray(ThorToolFunctionPropertyDefinition? arrayItems = null)
    {
        return new ThorToolFunctionPropertyDefinition
        {
            Items = arrayItems,
            Type = ConvertTypeToString(FunctionObjectTypes.Array)
        };
    }

    /// <summary>
    /// 定义枚举
    /// </summary>
    /// <param name="enumList"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static ThorToolFunctionPropertyDefinition DefineEnum(List<string> enumList, string? description = null)
    {
        return new ThorToolFunctionPropertyDefinition
        {
            Description = description,
            Enum = enumList,
            Type = ConvertTypeToString(FunctionObjectTypes.String)
        };
    }

    /// <summary>
    /// 定义整型
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public static ThorToolFunctionPropertyDefinition DefineInteger(string? description = null)
    {
        return new ThorToolFunctionPropertyDefinition
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.Integer)
        };
    }

    /// <summary>
    /// 定义数字
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public static ThorToolFunctionPropertyDefinition DefineNumber(string? description = null)
    {
        return new ThorToolFunctionPropertyDefinition
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.Number)
        };
    }

    /// <summary>
    /// 定义字符串
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public static ThorToolFunctionPropertyDefinition DefineString(string? description = null)
    {
        return new ThorToolFunctionPropertyDefinition
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.String)
        };
    }

    /// <summary>
    /// 定义布尔值
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public static ThorToolFunctionPropertyDefinition DefineBoolean(string? description = null)
    {
        return new ThorToolFunctionPropertyDefinition
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.Boolean)
        };
    }

    /// <summary>
    /// 定义null
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public static ThorToolFunctionPropertyDefinition DefineNull(string? description = null)
    {
        return new ThorToolFunctionPropertyDefinition
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.Null)
        };
    }

    /// <summary>
    /// 定义对象
    /// </summary>
    /// <param name="properties"></param>
    /// <param name="required"></param>
    /// <param name="additionalProperties"></param>
    /// <param name="description"></param>
    /// <param name="enum"></param>
    /// <returns></returns>
    public static ThorToolFunctionPropertyDefinition DefineObject(IDictionary<string, ThorToolFunctionPropertyDefinition>? properties,
                                                                  List<string>? required,
                                                                  bool? additionalProperties,
                                                                  string? description,
                                                                  List<string>? @enum)
    {
        return new ThorToolFunctionPropertyDefinition
        {
            Properties = properties,
            Required = required,
            AdditionalProperties = additionalProperties,
            Description = description,
            Enum = @enum,
            Type = ConvertTypeToString(FunctionObjectTypes.Object)
        };
    }


    /// <summary>
    /// 将 `FunctionObjectTypes` 枚举值转换为其对应的字符串表示形式。
    /// </summary>
    /// <param name="type">要转换的类型</param>
    /// <returns>给定类型的字符串表示形式</returns>

    public static string ConvertTypeToString(FunctionObjectTypes type)
    {
        return type switch
        {
            FunctionObjectTypes.String => "string",
            FunctionObjectTypes.Integer => "integer",
            FunctionObjectTypes.Number => "number",
            FunctionObjectTypes.Object => "object",
            FunctionObjectTypes.Array => "array",
            FunctionObjectTypes.Boolean => "boolean",
            FunctionObjectTypes.Null => "null",
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unknown type: {type}")
        };
    }
}
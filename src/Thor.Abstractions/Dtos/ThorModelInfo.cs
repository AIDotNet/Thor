using Thor.Abstractions.Consts;

namespace Thor.Abstractions.Dtos
{
    /// <summary>
    /// 模型信息
    /// </summary>
    public class ThorModelInfo
    {
        /// <summary>
        /// 模型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模型编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 模型类型，值有 chat,embeddings 等等，使用<see cref="ThorModelTypeConst"/> 赋值
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 模型简介
        /// </summary>
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// 支持 Function Call
        /// </summary>
        public bool HasFunctionCall { get; set; }

        /// <summary>
        /// 上下文大小，如 4k,8k,单位 k
        /// </summary>
        public int ContextSize { get; set; }
    }
}

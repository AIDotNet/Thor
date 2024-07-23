namespace Thor.Abstractions
{
    /// <summary>
    /// 全局对象挂载
    /// </summary>
    public static class ThorGlobal
    {
        /// <summary>
        ///  平台名集合，key：平台名，value:平台编码
        /// </summary>
        /// <example>如：key:通义千问（阿里云）,value:Qiansail</example>
        public static Dictionary<string, string> PlatformNames { get; } = [];

        /// <summary>
        /// 每个平台支持的模型列表,key:平台编码,value:模型名称列表
        /// </summary>
        /// <example>如：key:Qiansail,value:Qwen-Max</example>
        public static Dictionary<string, List<string>> ModelNames { get; } = new();
    }
}

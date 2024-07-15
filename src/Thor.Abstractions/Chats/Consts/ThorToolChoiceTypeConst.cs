namespace Thor.Abstractions.Chats.Consts
{
    public class ThorToolChoiceTypeConst
    {
        /// <summary>
        /// 指定特定工具会强制模型调用该工具
        /// </summary>
        public static string Function => "function";

        /// <summary>
        /// 表示模型可以在生成消息或调用一个或多个工具之间进行选择
        /// </summary>
        public static string Auto => "auto";

        /// <summary>
        /// 表示模型不会调用任何工具
        /// </summary>
        public static string None => "none";

        /// <summary>
        /// 表示模型必须调用一个或多个工具
        /// </summary>
        public static string Required => "required ";
    }
}

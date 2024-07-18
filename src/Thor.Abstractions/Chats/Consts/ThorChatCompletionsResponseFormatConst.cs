using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Abstractions.Chats.Consts
{
    /// <summary>
    /// 对话补全服务响应格式类型定义
    /// </summary>
    internal class ThorChatCompletionsResponseFormatConst
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public static string Text => "text";

        /// <summary>
        /// json对象类型
        /// </summary>
        public static string JsonObject => "json_object";
    }
}

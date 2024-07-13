using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Abstractions.Chats.Consts
{
    /// <summary>
    /// 对话消息角色定义
    /// </summary>
    public class ThorChatMessageRoleConst
    {
        /// <summary>
        /// 系统角色
        /// <para>
        /// 用于为聊天助手分配特定的行为或上下文，以影响对话的模型行为。
        /// 例如，可以将系统角色设定为“您是足球专家”，
        /// 那么 ChatGPT 在对话中会表现出特定的个性或专业知识。
        /// </para>
        /// </summary>
        public static string System => "system";

        /// <summary>
        /// 用户角色
        /// <para>
        /// 代表实际的最终用户，向 ChatGPT 发送提示或消息，
        /// 用于指示消息/提示来自最终用户或人类。
        /// </para>
        /// </summary>
        public static string User => "user";

        /// <summary>
        /// 助手角色
        /// <para>
        /// 表示对最终用户提示的响应实体，用于保持对话的连贯性。
        /// 它是由模型自动生成并回复的，用于设置模型的先前响应，以继续对话流程。
        /// </para>
        /// </summary>
        public static string Assistant => "assistant";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Abstractions.Chats.Consts
{
    /// <summary>
    /// 支持图片识别的消息体内容类型
    /// </summary>
    public class ThorMessageContentTypeConst
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public static string Text => "text";

        /// <summary>
        /// 图片 Url 类型
        /// </summary>
        public static string ImageUrl => "image_url";

        /// <summary>
        /// 图片 Url 类型
        /// </summary>
        public static string Image => "image";
    }
}

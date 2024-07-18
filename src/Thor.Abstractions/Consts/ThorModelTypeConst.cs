using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Abstractions.Consts
{
    /// <summary>
    /// 模型类型定义
    /// </summary>
    public class ThorModelTypeConst
    {
        /// <summary>
        /// 聊天对话模型
        /// </summary>
        public static string Chat => "chat";

        /// <summary>
        /// 向量嵌入模型
        /// </summary>
        public static string Embeddings => "embeddings";

        /// <summary>
        /// 生图模型
        /// </summary>
        public static string Image => "image";

        /// <summary>
        /// 声音模型
        /// </summary>
        public static string Audio => "audio";

        /// <summary>
        /// 视频模型
        /// </summary>
        public static string Video => "video";
    }
}

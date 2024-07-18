using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.SparkDesk.Helpers
{
    internal class SparkDeskEventStreamHelper
    {
        /// <summary>
        /// 移除文本前缀
        /// </summary>
        /// <param name="text"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string RemovePrefix(string text, string prefix = "data: ")
        {
            if (text is not null && text.StartsWith(prefix))
            {
                return text.Substring(prefix.Length);
            }
            else
            {
                return text;
            }
        }

        /// <summary>
        /// 是否流结束内容
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsStreamEndText(string text) => text == "[DONE]";
    }
}

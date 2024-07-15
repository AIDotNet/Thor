using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.SparkDesk.Helpers.Dtos
{
    /// <summary>
    /// 星火大模型信息
    /// </summary>
    public class SparkDeskModelInfo
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
        /// 模型类型，值有 chat,embeddings
        /// </summary>
        public string Type { get; set; }
    }
}

using Thor.MetaGLM.Models.RequestModels.FunctionModels;

namespace Thor.MetaGLM.Models.RequestModels
{
    public class TextRequestBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string request_id { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string model { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public List<MessageItem> messages { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public List<FunctionTool> tools { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public string tool_choice { get; set; } = string.Empty;

        private float? _top_p = null;

        /// <summary>
        /// 参考文档:https://open.bigmodel.cn/dev/api#glm-4
        /// 默认值： 0.7
        /// </summary>
        public float top_p
        {
            get
            {
                if (_top_p is null)
                {
                    _top_p = 0.7f;
                }

                return _top_p.Value;
            }

            set
            {
                if (value is <= 0.0f or >= 1.0f)
                {
                    _top_p = 0.7f;
                }
                else
                {
                    _top_p = value;
                }
            }
        }

        private float? _temperature = null;
        /// <summary>
        /// 参考文档:https://open.bigmodel.cn/dev/api#glm-4
        /// 默认值： 0.95
        /// </summary>
        public float temperature
        {
            get
            {
                if (_temperature is null)
                {
                    _temperature = 0.95f;
                }

                return _temperature.Value;
            }

            set
            {
                if (value is <= 0.0f or >= 1.0f)
                {
                    _temperature = 0.95f;
                }
                else
                {
                    _temperature = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool stream { get; set; } = true;
    }
}
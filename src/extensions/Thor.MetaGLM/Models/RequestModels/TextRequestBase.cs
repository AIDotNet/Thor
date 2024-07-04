using Thor.MetaGLM.Models.RequestModels.FunctionModels;

namespace Thor.MetaGLM.Models.RequestModels
{
    public class TextRequestBase
    {
        public string request_id { get; private set; }
        public string model { get; private set; }
        public MessageItem[] messages { get; private set; }
        public List<FunctionTool> tools { get; private set; }
        public string tool_choice { get; private set; }
        public float top_p { get; private set; }
        public float temperature { get; private set; }

        public bool stream { get; set; }

        public TextRequestBase()
        {
            this.stream = true;
        }

        public TextRequestBase SetRequestId(string requestId)
        {
            this.request_id = requestId;
            return this;
        }

        public TextRequestBase SetModel(string model)
        {
            this.model = model;
            return this;
        }

        public TextRequestBase SetMessages(MessageItem[] messages)
        {
            this.messages = messages;
            return this;
        }

        public TextRequestBase SetTools(List<FunctionTool> tools)
        {
            this.tools = tools;
            return this;
        }

        public TextRequestBase SetToolChoice(string toolChoice)
        {
            this.tool_choice = toolChoice;
            return this;
        }

        public TextRequestBase SetTopP(float topP)
        {
            // 智谱AI开放平台:https://open.bigmodel.cn/dev/api#glm-4
            // 默认值为 0.7

            if (topP is <= 0.0f or >= 1.0f)
            {
                topP = 0.7f;
            }
            this.top_p = topP;
            return this;
        }

        public TextRequestBase SetTemperature(float temperature)
        {
            // 智谱AI开放平台:https://open.bigmodel.cn/dev/api#glm-4
            // 默认值为 0.95
            if (temperature is <= 0.0f or >= 1.0f)
            {
                temperature = 0.95f;
            }

            this.temperature = temperature;
            return this;
        }
    }
}
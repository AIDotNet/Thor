using AIDotNet.MetaGLM.Models.RequestModels.FunctionModels;

namespace AIDotNet.MetaGLM.Models.RequestModels
{
    public class TextRequestBase
    {
        public string request_id { get; private set; }
        public string model { get; private set; }
        public MessageItem[] messages { get; private set; }
        public FunctionTool[] tools { get; private set; }
        public string tool_choice { get; private set; }
        public double top_p { get; private set; }
        public double temperature { get; private set; }

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

        public TextRequestBase SetTools(FunctionTool[] tools)
        {
            this.tools = tools;
            return this;
        }

        public TextRequestBase SetToolChoice(string toolChoice)
        {
            this.tool_choice = toolChoice;
            return this;
        }

        public TextRequestBase SetTopP(double topP)
        {
            this.top_p = topP;
            return this;
        }

        public TextRequestBase SetTemperature(double temperature)
        {
            this.temperature = temperature;
            return this;
        }
    }
}
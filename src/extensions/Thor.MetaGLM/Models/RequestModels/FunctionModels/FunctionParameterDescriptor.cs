namespace Thor.MetaGLM.Models.RequestModels.FunctionModels
{

    public class FunctionParameterDescriptor(string type, string description)
    {
        public string type { get; set; } = type;
        
        public string description { get; set; } = description;
    }
}
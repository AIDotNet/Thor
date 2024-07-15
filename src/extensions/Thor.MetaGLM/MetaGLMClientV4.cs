using Thor.MetaGLM.Modules;

namespace Thor.MetaGLM
{
    public class MetaGLMClientV4()
    {
        public Modules.Chat Chat { get; private set; } = new();

        public Modules.Images Images { get; private set; } = new();

        public Modules.Embeddings Embeddings { get; private set; } = new();
    }
}

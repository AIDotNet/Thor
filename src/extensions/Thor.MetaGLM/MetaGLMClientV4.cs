using Thor.MetaGLM.Modules;

namespace Thor.MetaGLM
{
    public class MetaGLMClientV4()
    {
        public Chat Chat { get; private set; } = new();

        public Images Images { get; private set; } = new();

        public Embeddings Embeddings { get; private set; } = new();
    }
}

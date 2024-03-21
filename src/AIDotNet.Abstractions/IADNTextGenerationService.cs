using Microsoft.SemanticKernel.TextGeneration;

namespace AIDotNet.Abstractions;

public interface IADNTextGenerationService : ITextGenerationService
{
    /// <summary>
    ///  The service name
    /// </summary>
    public static List<string> ServiceNames { get; } = new();
}
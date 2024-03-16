using Microsoft.SemanticKernel.ChatCompletion;

namespace AIDotNet.Abstractions;

public interface IADNChatCompletionService : IChatCompletionService
{
    /// <summary>
    ///  The service name
    /// </summary>
    public static List<string> ServiceNames { get; }
}
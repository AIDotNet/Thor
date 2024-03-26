using Microsoft.SemanticKernel.ChatCompletion;

namespace AIDotNet.Abstractions;

public interface IADNChatCompletionService : IChatCompletionService
{
    /// <summary>
    ///  The service name
    /// </summary>
    public static Dictionary<string,string> ServiceNames { get; } = new();
}
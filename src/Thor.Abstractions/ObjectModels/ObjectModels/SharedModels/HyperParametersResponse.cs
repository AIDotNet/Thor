using System.Text.Json.Serialization;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.FineTuningJobResponseModels;

namespace Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;

public record HyperParametersResponse
{
    [JsonPropertyName("batch_size")] public int? BatchSize { get; set; }

    [JsonPropertyName("learning_rate_multiplier")]
    public float? LearningRateMultiplier { get; set; }

    [JsonPropertyName("n_epochs")]
    [JsonConverter(typeof(NEpochsConverter))]
    public int? NEpochs { get; set; }

    [JsonPropertyName("prompt_loss_weight")]
    public float? PromptLossWeight { get; set; }
}
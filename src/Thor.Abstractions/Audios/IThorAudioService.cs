using Thor.Abstractions.Dtos;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Thor.Abstractions.Realtime.Dto;

namespace Thor.Abstractions.Audios;

public interface IThorAudioService
{
    Task<AudioCreateTranscriptionResponse> TranscriptionsAsync(AudioCreateTranscriptionRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<AudioCreateTranscriptionResponse> TranslationsAsync(AudioCreateTranscriptionRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<(Stream, ThorUsageResponse? usage)> SpeechAsync(AudioCreateSpeechRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);
}
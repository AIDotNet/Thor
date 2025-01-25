using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.Abstractions.Audios;

public interface IThorAudioService
{
    Task<AudioCreateTranscriptionResponse> TranscriptionsAsync(AudioCreateTranscriptionRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);
    
    Task<AudioCreateTranscriptionResponse> TranslationsAsync(AudioCreateTranscriptionRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);
    
    Task<Stream> SpeechAsync(AudioCreateSpeechRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);
}
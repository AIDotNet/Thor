using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.Abstractions.Audios;

public interface IThorAudioService
{
    Task<AudioCreateTranscriptionResponse> AudioCompletionsAsync(AudioCreateTranscriptionRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);
}
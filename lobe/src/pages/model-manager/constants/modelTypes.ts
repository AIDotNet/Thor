/**
 * Model type constants
 * Define the model types used throughout the application
 */

export interface ModelType {
    CHAT: string;
    AUDIO: string;
    IMAGE: string;
    STT: string;
    TTS: string;
    EMBEDDING: string;
}

export const MODEL_TYPES: ModelType = {
    CHAT: "chat",
    AUDIO: "audio",
    IMAGE: "image",
    STT: "stt",
    TTS: "tts",
    EMBEDDING: "embedding"
};

export default MODEL_TYPES; 
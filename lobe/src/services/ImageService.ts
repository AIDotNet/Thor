import OpenAI from 'openai';

/**
 * Process an image using the OpenAI API
 * This function supports both text-to-image and image-to-image generation
 * 
 * @param params - Parameters for image generation
 * @returns Promise with the generated image data
 */
export const processImage = async (params: {
  prompt: string;
  negativePrompt?: string;
  model: string;
  size: string;
  token: string;
  image?: string; // Base64 image data (without prefix) for image-to-image
  mask?: string;  // Optional mask image in base64 format
  transformationStrength?: number; // Only used for image-to-image (0-1)
}) => {
  try {
    // Initialize OpenAI client
    const openai = new OpenAI({
      apiKey: params.token,
      dangerouslyAllowBrowser: true,
      baseURL: window.location.origin + '/v1'
    });

    // Map requested size to valid OpenAI size
    let validSize: "256x256" | "512x512" | "1024x1024" | "1792x1024" | "1024x1792" = "1024x1024"; // Default
    
    if (params.size === "1024x1024" || 
        params.size === "512x512" || 
        params.size === "256x256" ||
        params.size === "1792x1024" ||
        params.size === "1024x1792") {
      validSize = params.size as any;
    }

    // For image-to-image transformation
    if (params.image) {
      // Convert base64 string to File object for the API
      const imageFile = base64ToFile(params.image, 'image.png', 'image/png');
      
      // For image edit, valid sizes are more limited
      const editSize: "256x256" | "512x512" | "1024x1024" = 
        (validSize === "1024x1024" || validSize === "512x512" || validSize === "256x256") 
          ? validSize 
          : "1024x1024";
      
      // Prepare request parameters
      const editParams: any = {
        model: params.model,
        image: imageFile,
        prompt: params.prompt,
        n: 1,
        size: editSize,
        response_format: 'url',
      };
      
      // Add mask if provided
      if (params.mask) {
        editParams.mask = base64ToFile(params.mask, 'mask.png', 'image/png');
      }
      
      // Call image edit endpoint with correct parameters
      const response = await openai.images.edit(editParams);
      
      return {
        data: {
          id: (response as any).created?.toString() || Date.now().toString(),
          url: response.data?.[0]?.url || '',
        }
      };
    } else {
      // Text-to-image generation
      const response = await openai.images.generate({
        model: params.model,
        prompt: params.prompt,
        n: 1,
        size: validSize,
        response_format: 'url',
        ...(params.negativePrompt ? { negative_prompt: params.negativePrompt } : {})
      });
      
      return {
        data: {
          id: (response as any).created?.toString() || Date.now().toString(),
          url: response.data?.[0]?.url || '',
        }
      };
    }
  } catch (error: any) {
    console.error('OpenAI image generation error:', error);
    throw new Error(`Image generation failed: ${error.message || 'Unknown error'}`);
  }
};

/**
 * Convert a base64 string to a File object
 * @param base64String Base64 string (without data URI prefix)
 * @param filename Filename for the created File
 * @param mimeType MIME type of the file
 * @returns File object containing the binary data
 */
function base64ToFile(base64String: string, filename: string, mimeType: string): File {
  // If the base64 string includes a data URI prefix, remove it
  const base64Data = base64String.includes(',') 
    ? base64String.split(',')[1] 
    : base64String;
  
  // Convert base64 to binary
  const binaryString = atob(base64Data);
  const bytes = new Uint8Array(binaryString.length);
  for (let i = 0; i < binaryString.length; i++) {
    bytes[i] = binaryString.charCodeAt(i);
  }
  
  // Create Blob and then File
  const blob = new Blob([bytes], { type: mimeType });
  return new File([blob], filename, { type: mimeType });
} 
/**
 * ResponsesService - 处理以"o"开头模型的responses接口调用
 * 这个服务专门用于处理需要使用responses接口而非传统completions接口的模型
 */

export interface ResponsesRequest {
  model: string;
  messages: Array<{
    role: 'system' | 'user' | 'assistant';
    content: string;
  }>;
  temperature?: number;
  max_tokens?: number;
  top_p?: number;
}

export interface ResponsesResponse {
  id: string;
  object: string;
  created: number;
  model: string;
  choices: Array<{
    index: number;
    message: {
      role: 'assistant';
      content: string;
    };
    finish_reason: string;
  }>;
  usage: {
    prompt_tokens: number;
    completion_tokens: number;
    total_tokens: number;
  };
}

/**
 * 判断模型是否需要使用responses接口
 * @param modelName 模型名称
 * @returns 是否使用responses接口
 */
export const shouldUseResponsesInterface = (modelName: string): boolean => {
  return modelName.toLowerCase().startsWith('o');
};

/**
 * 调用responses接口进行聊天
 * @param token API Token
 * @param request 请求参数
 * @returns 响应数据
 */
export const chatWithResponses = async (
  token: string,
  request: ResponsesRequest
): Promise<ResponsesResponse> => {
  const response = await fetch(window.location.origin + '/v1/responses', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(request)
  });

  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  return await response.json();
};

/**
 * 调用responses接口进行图像生成
 * @param token API Token
 * @param params 图像生成参数
 * @returns 图像响应数据
 */
export const generateImageWithResponses = async (
  token: string,
  params: {
    model: string;
    prompt: string;
    size: string;
    n?: number;
    negative_prompt?: string;
    image?: string;
    mask?: string;
  }
) => {
  const response = await fetch(window.location.origin + '/v1/images/responses', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify({
      n: 1,
      ...params
    })
  });

  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  return await response.json();
}; 
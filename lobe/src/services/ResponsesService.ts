import OpenAI from 'openai';

/**
 * 判断是否为需要使用responses接口的模型
 * @param modelName 模型名称
 * @returns 是否使用responses接口
 */
export const isResponsesModel = (modelName: string): boolean => {
  return modelName.startsWith('o1') || modelName.startsWith('o-');
};

/**
 * 使用responses接口进行对话
 * @param params 请求参数
 * @returns Promise with response data
 */
export const createResponsesCompletion = async (params: {
  model: string;
  messages: any[];
  token: string;
  baseURL?: string;
  stream?: boolean;
}) => {
  const { model, messages, token, baseURL, stream = false } = params;
  
  // 使用responses接口的请求体格式
  const requestBody = {
    model,
    messages,
    stream
  };

  try {
    // 根据是否流式返回选择不同的调用方式
    if (stream) {
      // 流式响应
      const response = await fetch(`${baseURL || (window.location.origin + '/v1')}/responses`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(requestBody)
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      return response;
    } else {
      // 非流式响应
      const response = await fetch(`${baseURL || (window.location.origin + '/v1')}/responses`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(requestBody)
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      return await response.json();
    }
  } catch (error) {
    console.error('Responses API error:', error);
    throw error;
  }
};

/**
 * 使用传统的chat completions接口进行对话
 * @param params 请求参数
 * @returns Promise with response data
 */
export const createChatCompletion = async (params: {
  model: string;
  messages: any[];
  token: string;
  baseURL?: string;
  stream?: boolean;
}) => {
  const { model, messages, token, baseURL, stream = false } = params;
  
  const openai = new OpenAI({
    apiKey: token,
    dangerouslyAllowBrowser: true,
    baseURL: baseURL || (window.location.origin + '/v1')
  });

  return await openai.chat.completions.create({
    model,
    messages,
    stream
  });
};

/**
 * 智能选择接口进行对话
 * 根据模型名称自动选择使用responses接口或chat completions接口
 * @param params 请求参数
 * @returns Promise with response data
 */
export const createCompletion = async (params: {
  model: string;
  messages: any[];
  token: string;
  baseURL?: string;
  stream?: boolean;
}) => {
  const { model } = params;
  
  if (isResponsesModel(model)) {
    console.log(`使用responses接口处理模型: ${model}`);
    return await createResponsesCompletion(params);
  } else {
    console.log(`使用chat completions接口处理模型: ${model}`);
    return await createChatCompletion(params);
  }
};

/**
 * 处理流式响应的通用方法
 * @param response 响应对象
 * @param onChunk 处理每个chunk的回调函数
 * @param isResponsesAPI 是否为responses接口
 */
export const processStreamResponse = async (
  response: any,
  onChunk: (content: string) => void,
  isResponsesAPI: boolean = false
) => {
  if (isResponsesAPI) {
    // 处理responses接口的流式响应
    const reader = response.body?.getReader();
    const decoder = new TextDecoder();
    
    if (!reader) {
      throw new Error('无法获取响应流');
    }

    try {
      while (true) {
        const { done, value } = await reader.read();
        
        if (done) break;
        
        const chunk = decoder.decode(value);
        const lines = chunk.split('\n');
        
        for (const line of lines) {
          if (line.startsWith('data: ')) {
            const data = line.slice(6);
            if (data === '[DONE]') return;
            
            try {
              const parsed = JSON.parse(data);
              // responses接口的响应格式可能不同，需要根据实际情况调整
              const content = parsed.choices?.[0]?.delta?.content || parsed.content || '';
              if (content) {
                onChunk(content);
              }
            } catch (e) {
              console.warn('解析响应数据失败:', e);
            }
          }
        }
      }
    } finally {
      reader.releaseLock();
    }
  } else {
    // 处理chat completions接口的流式响应
    for await (const chunk of response) {
      const content = chunk.choices[0]?.delta?.content;
      if (content) {
        onChunk(content);
      }
    }
  }
}; 
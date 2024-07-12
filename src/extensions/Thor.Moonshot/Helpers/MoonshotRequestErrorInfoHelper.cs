using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.Moonshot.Dtos;

namespace Thor.Moonshot.Helpers
{
    internal class MoonshotRequestErrorInfoHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public List<MoonshotRequestErrorInfo> ErrorInfoList { get; set; }

        public MoonshotRequestErrorInfoHelper()
        {
            ErrorInfoList = new List<MoonshotRequestErrorInfo>()
            {
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 400,
                    ErrorType = "content_filter",
                    ErrorMessage = "The request was rejected because it was considered high risk",
                    Description = "内容审查拒绝，您的输入或生成内容可能包含不安全或敏感内容，请您避免输入易产生敏感内容的提示语，谢谢"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 400,
                    ErrorType = "invalid_request_error",
                    ErrorMessage = "Invalid request: Input token length too long",
                    Description = "请求中的 tokens 长度过长，请求不要超过模型 tokens 的最长限制"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 400,
                    ErrorType = "invalid_request_error",
                    ErrorMessage = "Your request exceeded model token limit : {max_model_length}",
                    Description = "请求的 tokens 数和设置的 max_tokens 加和超过了模型规格长度，请检查请求体的规格或选择合适长度的模型"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 400,
                    ErrorType = "invalid_request_error",
                    ErrorMessage = "Invalid purpose: only 'file-extract' accepted",
                    Description = "请求中的目的（purpose）不正确，当前只接受 'file-extract'，请修改后重新请求"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 400,
                    ErrorType = "invalid_request_error",
                    ErrorMessage = "File size is too large, max file size is 100MB, please confirm and re-upload the file",
                    Description = "上传的文件大小超过了限制，请重新上传"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 400,
                    ErrorType = "invalid_request_error",
                    ErrorMessage = "File size is zero, please confirm and re-upload the file",
                    Description = "上传的文件大小为 0，请重新上传"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 400,
                    ErrorType = "invalid_request_error",
                    ErrorMessage = "The number of files you have uploaded exceeded the max file count {max_file_count}, please delete previous uploaded files",
                    Description = "上传的文件总数超限，请删除不用的早期的文件后重新上传"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 401,
                    ErrorType = "invalid_authentication_error",
                    ErrorMessage = "Invalid Authentication",
                    Description = "鉴权失败，请检查 apikey 是否正确，请修改后重试"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 401,
                    ErrorType = "invalid_authentication_error",
                    ErrorMessage = "Incorrect API key provided",
                    Description = "鉴权失败，请检查 apikey 是否提供以及 apikey 是否正确，请修改后重试"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 403,
                    ErrorType = "exceeded_current_quota_error",
                    ErrorMessage = "Your account {uid}<{ak-id}> is not active, current state: {current state}, you may consider to check your account balance",
                    Description = "账户异常，请检查您的账户余额"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 403,
                    ErrorType = "permission_denied_error",
                    ErrorMessage = "The API you are accessing is not open",
                    Description = "访问的 API 暂未开放"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 403,
                    ErrorType = "permission_denied_error",
                    ErrorMessage = "You are not allowed to get other user info",
                    Description = "访问其他用户信息的行为不被允许，请检查"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 404,
                    ErrorType = "resource_not_found_error",
                    ErrorMessage = "Not found the model or Permission denied",
                    Description = "不存在此模型或者没有授权访问此模型，请检查后重试"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 404,
                    ErrorType = "resource_not_found_error",
                    ErrorMessage = "Users {user_id} not found",
                    Description = "找不到该用户，请检查后重试"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 429,
                    ErrorType = "engine_overloaded_error",
                    ErrorMessage = "The engine is currently overloaded, please try again later",
                    Description = "当前并发请求过多，节点限流中，请稍后重试；建议充值升级 tier，享受更丝滑的体验"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 429,
                    ErrorType = "exceeded_current_quota_error",
                    ErrorMessage = "You exceeded your current token quota: {token_credit}, please check your account balance",
                    Description = "账户额度不足，请检查账户余额，保证账户余额可匹配您 tokens 的消耗费用后重试"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 429,
                    ErrorType = "rate_limit_reached_error",
                    ErrorMessage = "Your account {uid}<{ak-id}> request reached max concurrency: {Concurrency}, please try again after {time} seconds",
                    Description = "请求触发了账户并发个数的限制，请等待指定时间后重试"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 429,
                    ErrorType = "rate_limit_reached_error",
                    ErrorMessage = "Your account {uid}<{ak-id}> request reached max request: {RPM}, please try again after {time} seconds",
                    Description = "请求触发了账户 RPM 速率限制，请等待指定时间后重试"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 429,
                    ErrorType = "rate_limit_reached_error",
                    ErrorMessage = "Your account {uid}<{ak-id}> request reached TPM rate limit, current:{current_tpm}, limit:{max_tpm}",
                    Description = "请求触发了账户 TPM 速率限制，请等待指定时间后重试"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 429,
                    ErrorType = "rate_limit_reached_error",
                    ErrorMessage = "Your account {uid}<{ak-id}> request reached TPD rate limit, current:{current_tpd}, limit:{max_tpd}",
                    Description = "请求触发了账户 TPD 速率限制，请等待指定时间后重试"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 500,
                    ErrorType = "server_error",
                    ErrorMessage = "Failed to extract file: {error}",
                    Description = "解析文件失败，请重试"
                },
                new MoonshotRequestErrorInfo()
                {
                    HTTPStatusCode = 500,
                    ErrorType = "unexpected_output",
                    ErrorMessage = "invalid state transition",
                    Description = "内部错误，请联系管理员"
                }
            };
        }
    }
}
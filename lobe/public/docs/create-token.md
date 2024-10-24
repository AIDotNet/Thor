# 创建平台token

1. 打开平台控制台，打开AI服务/令牌
![alt text](/images/12378978797912.png)

所有的账号都会默认创建一个token，并且这个token是密钥额度限制额过期时间

当然你也可以吧这个token删除，创建一个新的token，也可以直接使用这个token

![alt text](/images/12789798127983.png)

如果您的系统部署的Thor服务没有提供Https，那么您将无法使用复制Key，需要点击查看，然后token会以弹窗的形式进行显示
然后只需要选择然后复制弹窗的token即可

![alt text](/images/18751241231245.png)

复制了`sk-51PNh4SrPg7wYNwl8dXwBxtDyCnlgHgAUyCVKr`token，然后我们打开一个curl工具

```shell
curl --location --request POST 'http://localhost:11112/v1/chat/completions' \
--header 'Accept: application/json' \
--header 'User-Agent: Thor-Docs' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer sk-51PNh4SrPg7wYNwl8dXwBxtDyCnlgHgAUyCVKr' \
--data-raw '{
    "model": "gpt-4o-mini",
    "messages": [
      {
        "role": "user",
        "content": "Hello!"
      }
    ]
  }'
```

通过上面的wget请求到我们的服务尝试是否可用

执行以后得到以下接口

```shell
curl --location --request POST 'http://localhost:11112/v1/chat/completions' \
--header 'Accept: application/json' \
--header 'User-Agent: Thor-Docs' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer sk-51PNh4SrPg7wYNwl8dXwBxtDyCnlgHgAUyCVKr' \
--data-raw '{
    "model": "gpt-4o-mini",
    "messages": [
      {
        "role": "user",
        "content": "Hello!"
      }
    ]
  }'
\{"id":"chatcmpl-ALwUywgBCa8csodL7aqYzaXKHBYlo","model":"gpt-4o-mini-2024-07-18","object":"chat.completion","choices":[{"delta":{"role":"assistant","content":"Hello! How can I assist you today?"},"message":{"role":"assistant","content":"Hello! How can I assist you today?"},"index":0,"finish_reason":"stop"}],"usage":{"prompt_tokens":9,"completion_tokens":9,"total_tokens":18},"created":1729793120,"system_fingerprint":"fp_f59a81427f"}
```

我们也可以打开`系统服务/日志` 查看到我们刚刚发起的请求和请求使用的token，金额。

![alt text](/images/18092380128930.png)

## 注意事项

1. 每个用户都可以创建多个token
2. 每一个token创建了以后如果不进行禁用或过期，那么他将是一直可用，当然，您也可以对单个token的使用额度进行限制，当token的额度用完的时候将会提示余额不足，但是需要注意的是账号的额度是总额度，token额度只是用于限制token使用额度上限。
3. token过期以后也会不可用，禁用也是一样，你需要注意token的过期时间。

namespace AIDotNet.AliyunFC;

public class AliyunFCOutputDto
{
    public int Code { get; set; }
    public AliyunFCResultDto Data { get; set; }
    public string Message { get; set; }
    public string RequestId { get; set; }
    public bool Success { get; set; }
}
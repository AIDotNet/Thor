namespace Thor.Service.Dto;

public class AliPayResponseDto
{
    public Alipay_Trade_Precreate_Response alipay_trade_precreate_response { get; set; }

    public string sign { get; set; }
}

public class Alipay_Trade_Precreate_Response
{
    public string code { get; set; }
    public string msg { get; set; }
    public string out_trade_no { get; set; }
    public string qr_code { get; set; }
    public string share_code { get; set; }
}

namespace Atelie.Application.Models.Sms;

public class SmsStatusResponse
{
    public string status { get; set; }
    public SmsData data { get; set; }
}

public class SmsData
{
    public long id { get; set; }
    public decimal price { get; set; }
    public string to { get; set; }
    public string Message { get; set; }
    public string status { get; set; }
    public DateTime sent_at { get; set; }
    public DateTime? delivery_sm_at { get; set; }
}
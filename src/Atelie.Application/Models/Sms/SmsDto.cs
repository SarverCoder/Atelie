namespace Atelie.Application.Models.Sms;

public class AuthRequest
{
    public string email { get; set; }
    public string password { get; set; }
}

public class AuthResponse
{
    public string message { get; set; }
    public TokenData data { get; set; }
    public string token_type { get; set; }
}

public class TokenData
{
    public string token { get; set; }
}

public class SmsRequest
{

    public string mobile_phone { get; set; }
    public string message { get; set; }


}

public class SmsResponseBatch
{
    public string id { get; set; }
    public string message { get; set; }
    public List<string> status { get; set; }
}

public class SmsBatchMessage
{
    public string user_sms_id { get; set; }
    public string to { get; set; }
    public string text { get; set; }
}

public class SmsBatchRequestDto
{
    public List<SmsBatchMessage> messages { get; set; }
}


public class SmsBatchRequest
{
    public List<SmsBatchMessage> messages { get; set; }
    public string from { get; set; } = "4546";
    public int dispatch_id { get; set; }
    public string callback_url { get; set; }
}

public class SmsResponse
{
    public string id { get; set; }
    public string message { get; set; }
    public string status { get; set; }
}

public class SmsCallback
{
    public string request_id { get; set; }
    public string message_id { get; set; }
    public string user_sms_id { get; set; }
    public string country { get; set; }
    public string phone_number { get; set; }
    public string sms_count { get; set; }
    public string status { get; set; }
    public string status_date { get; set; }
}

// Token Cache Model
public class CachedToken
{
    public string Token { get; set; }
    public DateTime ExpiryTime { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiryTime;
}
namespace Atelie.Application.Helpers.SmsProvider;

public interface ISmsTokenProvider
{
    Task<string> GetValidTokenAsync();
    Task<string> RefreshTokenAsync();
    void ClearToken();
}
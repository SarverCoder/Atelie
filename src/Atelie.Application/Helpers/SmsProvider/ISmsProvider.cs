using Atelie.Application.Models.Sms;

namespace Atelie.Application.Helpers.SmsProvider;

public interface ISmsProvider
{
    Task<SmsResponse> SendSmsAsync(string phoneNumber, string message);
    Task<SmsResponseBatch> SendSmsBatchAsync(List<SmsBatchMessage> messages, int dispatchId = 0);
    Task<SmsStatusResponse> GetSmsStatusAsync(string id);
}
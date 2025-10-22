using Atelie.Application.Models.Sms;
using Atelie.Application.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Atelie.Application.Helpers.SmsProvider;

public class SmsProvider : ISmsProvider, IDisposable
{

    private readonly HttpClient _httpClient;
    private readonly ISmsTokenProvider _tokenService;
    private readonly ILogger<SmsProvider> _logger;
    private readonly string _baseUrl = "https://notify.eskiz.uz/api/message/sms";
    private readonly string _fromNumber;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SmsProvider(
        HttpClient httpClient,
        ISmsTokenProvider tokenService,
        ILogger<SmsProvider> logger,
        IOptions<SmsOption> configuration,
        IHttpContextAccessor httpContextAccessor)
    {

        _httpClient = httpClient;
        _tokenService = tokenService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _fromNumber = configuration.Value.FromNumber;
    }

    private string BuildCallbackUrl()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
            return null;

        // Hozirgi requestning schema (http/https), host va portini olamiz
        var baseUrl = $"{request.Scheme}://{request.Host}";
        return $"{baseUrl}/api/Sms/callback"; // sizning callback endpointingiz
    }

    private async Task<HttpClient> GetAuthorizedHttpClientAsync()
    {
        var token = await _tokenService.GetValidTokenAsync();

        // Eski Authorization header ni o'chiramiz
        _httpClient.DefaultRequestHeaders.Authorization = null;

        // Yangi token bilan Authorization header ni qo'shamiz
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return _httpClient;
    }

    public async Task<SmsResponse> SendSmsAsync(string phoneNumber, string message)
    {
        var maxRetries = 2;
        var currentRetry = 0;
        var callbackUrl = BuildCallbackUrl();

        while (currentRetry <= maxRetries)
        {
            try
            {
                var httpClient = await GetAuthorizedHttpClientAsync();

                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(phoneNumber), "mobile_phone");
                formData.Add(new StringContent(message), "message");
                formData.Add(new StringContent(_fromNumber), "from");

                if (!string.IsNullOrEmpty(callbackUrl))
                {
                    formData.Add(new StringContent(callbackUrl), "callback_url");
                }

                var response = await httpClient.PostAsync($"{_baseUrl}/send", formData);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var smsResponse = JsonSerializer.Deserialize<SmsResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogInformation($"SMS yuborildi. ID: {smsResponse.id}, Status: {smsResponse.status}");
                    return smsResponse;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && currentRetry < maxRetries)
                {
                    _logger.LogWarning("Token noto'g'ri, yangilanmoqda...");
                    _tokenService.ClearToken(); // Cache dan o'chiramiz
                    currentRetry++;
                    continue; // Qayta urinib ko'ramiz
                }
                else
                {
                    _logger.LogError($"SMS yuborishda xatolik: {response.StatusCode} - {responseContent}");
                    throw new Exception($"SMS yuborishda xatolik: {responseContent}");
                }
            }
            catch (Exception ex) when (currentRetry < maxRetries)
            {
                _logger.LogWarning(ex, $"SMS yuborishda xatolik, qayta urinilmoqda... ({currentRetry + 1}/{maxRetries + 1})");
                currentRetry++;

                if (currentRetry <= maxRetries)
                {
                    await Task.Delay(1000 * currentRetry); // Exponential backoff
                }
            }
        }

        throw new Exception("SMS yuborishda barcha urinishlar muvaffaqiyatsiz tugadi");
    }

    public async Task<SmsResponseBatch> SendSmsBatchAsync(List<SmsBatchMessage> messages, int dispatchId = 0)
    {
        var maxRetries = 2;
        var currentRetry = 0;
        var callbackUrl = BuildCallbackUrl();

        while (currentRetry <= maxRetries)
        {
            try
            {
                var httpClient = await GetAuthorizedHttpClientAsync();

                var batchRequest = new SmsBatchRequest
                {
                    messages = messages,
                    from = _fromNumber,
                    dispatch_id = dispatchId,
                    callback_url = callbackUrl ?? ""
                };

                var jsonContent = JsonSerializer.Serialize(batchRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{_baseUrl}/send-batch", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var smsResponse = JsonSerializer.Deserialize<SmsResponseBatch>(responseContent);

                    _logger.LogInformation($"Batch SMS yuborildi. ID: {smsResponse.id}, Status: {smsResponse.status}");
                    return smsResponse;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && currentRetry < maxRetries)
                {
                    _logger.LogWarning("Token noto'g'ri, yangilanmoqda...");
                    _tokenService.ClearToken(); // Cache dan o'chiramiz
                    currentRetry++;
                    continue; // Qayta urinib ko'ramiz
                }
                else
                {
                    _logger.LogError($"Batch SMS yuborishda xatolik: {response.StatusCode} - {responseContent}");
                    throw new Exception($"Batch SMS yuborishda xatolik: {responseContent}");
                }
            }
            catch (Exception ex) when (currentRetry < maxRetries)
            {
                _logger.LogWarning(ex, $"Batch SMS yuborishda xatolik, qayta urinilmoqda... ({currentRetry + 1}/{maxRetries + 1})");
                currentRetry++;

                if (currentRetry <= maxRetries)
                {
                    await Task.Delay(1000 * currentRetry); // Exponential backoff
                }
            }
        }

        throw new Exception("Batch SMS yuborishda barcha urinishlar muvaffaqiyatsiz tugadi");
    }

    public async Task<SmsStatusResponse> GetSmsStatusAsync(string id)
    {
        var maxRetries = 2;
        var currentRetry = 0;

        while (currentRetry <= maxRetries)
        {
            try
            {
                var httpClient = await GetAuthorizedHttpClientAsync();

                var response = await httpClient.GetAsync($"{_baseUrl}/status_by_id/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var smsStatus = JsonSerializer.Deserialize<SmsStatusResponse>(
                        responseContent);

                    _logger.LogInformation($"SMS status olindi. ID: {id}, Status: {smsStatus?.data?.status}");
                    return smsStatus;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && currentRetry < maxRetries)
                {
                    _logger.LogWarning("Token noto‘g‘ri, yangilanmoqda...");
                    _tokenService.ClearToken(); // Cache dan o‘chiramiz
                    currentRetry++;
                    continue;
                }
                else
                {
                    _logger.LogError($"SMS status olishda xatolik: {response.StatusCode} - {responseContent}");
                    throw new Exception($"SMS status olishda xatolik: {responseContent}");
                }
            }
            catch (Exception ex) when (currentRetry < maxRetries)
            {
                _logger.LogWarning(ex, $"SMS status olishda xatolik, qayta urinilmoqda... ({currentRetry + 1}/{maxRetries + 1})");
                currentRetry++;

                if (currentRetry <= maxRetries)
                {
                    await Task.Delay(1000 * currentRetry); // Exponential backoff
                }
            }
        }

        throw new Exception("SMS status olishda barcha urinishlar muvaffaqiyatsiz tugadi");
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
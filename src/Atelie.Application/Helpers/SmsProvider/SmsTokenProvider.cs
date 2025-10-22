using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Atelie.Application.Models.Sms;
using Atelie.Application.Options;

namespace Atelie.Application.Helpers.SmsProvider;

public class SmsTokenProvider : ISmsTokenProvider
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<SmsTokenProvider> _logger;
    private readonly string _cacheKey = "eskiz_token";
    private readonly SemaphoreSlim _tokenSemaphore = new SemaphoreSlim(1, 1);

    private readonly string _email;
    private readonly string _password;
    private readonly string _authUrl = "https://notify.eskiz.uz/api/auth/login";

    public SmsTokenProvider(
        HttpClient httpClient,
        IMemoryCache memoryCache,
        IOptions<SmsOption> configuration,
        ILogger<SmsTokenProvider> logger)
    {
        _httpClient = httpClient;
        _memoryCache = memoryCache;
        _logger = logger;

        _email = configuration.Value.Email;
        _password = configuration.Value.Password;
    }


    public async Task<string> GetValidTokenAsync()
    {
        await _tokenSemaphore.WaitAsync();
        try
        {
            // Cache dan tokenni tekshiramiz
            if (_memoryCache.TryGetValue(_cacheKey, out CachedToken cachedToken))
            {
                if (!cachedToken.IsExpired)
                {
                    _logger.LogDebug("Cache dan token olinindi");
                    return cachedToken.Token;
                }
                else
                {
                    _logger.LogInformation("Token muddati tugagan, yangilanmoqda...");
                }
            }

            // Token yo'q yoki muddati tugagan, yangilaymiz
            return await RefreshTokenAsync();
        }
        finally
        {
            _tokenSemaphore.Release();
        }
    }

    public async Task<string> RefreshTokenAsync()
    {
        try
        {
            _logger.LogInformation("Yangi token so'ralmoqda...");

            var authRequest = new AuthRequest
            {
                email = _email,
                password = _password
            };

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(authRequest.email), "email");
            formData.Add(new StringContent(authRequest.password), "password");

            var response = await _httpClient.PostAsync(_authUrl, formData);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (authResponse?.data?.token != null)
                {
                    // Tokenni cache ga saqlaymiz (29 kun muddatga)
                    var cachedToken = new CachedToken
                    {
                        Token = authResponse.data.token,
                        ExpiryTime = DateTime.UtcNow.AddDays(29) // Token 30 kun amal qiladi, 29 kuni ishlatamiz
                    };

                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(29),
                        Priority = CacheItemPriority.High
                    };

                    _memoryCache.Set(_cacheKey, cachedToken, cacheOptions);

                    _logger.LogInformation("Yangi token muvaffaqiyatli olindi va cache ga saqlandi");
                    return authResponse.data.token;
                }
                else
                {
                    throw new Exception("Token response da token mavjud emas");
                }
            }
            else
            {
                _logger.LogError($"Token olishda xatolik: {response.StatusCode} - {responseContent}");
                throw new Exception($"Token olishda xatolik: {responseContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token olishda kutilmagan xatolik");
            throw;
        }
    }

    public void ClearToken()
    {
        _memoryCache.Remove(_cacheKey);
        _logger.LogInformation("Token cache dan o'chirildi");
    }
}
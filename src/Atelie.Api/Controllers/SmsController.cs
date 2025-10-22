using Atelie.Application.Helpers.SmsProvider;
using Atelie.Application.Models.Sms;
using Microsoft.AspNetCore.Mvc;

namespace Atelie.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SmsController : ControllerBase
{
    private readonly ISmsProvider _smsProvider;
    private readonly ISmsTokenProvider _smsTokenProvider;

    public SmsController(ISmsProvider smsProvider, ISmsTokenProvider smsTokenProvider)
    {
        _smsProvider = smsProvider;
        _smsTokenProvider = smsTokenProvider;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SmsSend(SmsRequest request)
    {
        var result = await _smsProvider.SendSmsAsync(request.mobile_phone, request.message);

        return Ok(result);
    }

    [HttpPost("send-batch")]
    public async Task<IActionResult> SendSmsBatch(SmsBatchRequestDto request)
    {
        return Ok(await _smsProvider.SendSmsBatchAsync(request.messages));
    }

    [HttpGet("token-status")]
    public async Task<IActionResult> GetTokenStatus()
    {
        var token = await _smsTokenProvider.GetValidTokenAsync();
        return Ok(new { message = "Token aktiv", tokenExists = !string.IsNullOrEmpty(token) });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        _smsTokenProvider.ClearToken();
        var newToken = await _smsTokenProvider.GetValidTokenAsync();
        return Ok(new { message = "Token yangilandi", tokenExists = !string.IsNullOrEmpty(newToken) });
    }

}

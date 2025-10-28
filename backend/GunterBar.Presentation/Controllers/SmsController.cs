using System.Net;
using System.Security.Claims;
using GunterBar.Application.Common.Models;
using GunterBar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GunterBar.Presentation.Controllers;

/// <summary>
/// Controller for SMS verification operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SmsController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ISmsService _smsService;
    private readonly ILogger<SmsController> _logger;

    public SmsController(IUserService userService, ISmsService smsService, ILogger<SmsController> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Sends SMS verification code to user's phone number
    /// </summary>
    /// <param name="request">SMS request with phone number</param>
    /// <returns>Success response if SMS sent</returns>
    /// <response code="200">SMS sent successfully</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("send")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<ApiResponse<bool>>> SendSmsVerification([FromBody] SendSmsRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                return BadRequest(ApiResponse<bool>.Fail("Número de teléfono requerido"));
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(ApiResponse<bool>.Fail("Usuario no autenticado"));
            }

            // Generate verification code
            var codeResult = await _userService.GenerateSmsVerificationCodeAsync(userId, request.PhoneNumber);
            if (!codeResult.Success)
            {
                return BadRequest(codeResult);
            }

            // Send SMS
            var smsResult = await _smsService.SendSmsAsync(request.PhoneNumber, $"Tu código de verificación es: {codeResult.Data}");
            if (!smsResult)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ApiResponse<bool>.Fail("Error al enviar SMS"));
            }

            return Ok(ApiResponse<bool>.Succeed(true, "SMS enviado correctamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending SMS verification");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                ApiResponse<bool>.Fail("Error interno del servidor"));
        }
    }

    /// <summary>
    /// Verifies SMS code
    /// </summary>
    /// <param name="request">Verification request with code</param>
    /// <returns>Success response if code is valid</returns>
    /// <response code="200">Code verified successfully</response>
    /// <response code="400">Invalid code or request</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("verify")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<ApiResponse<bool>>> VerifySmsCode([FromBody] VerifySmsRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                return BadRequest(ApiResponse<bool>.Fail("Código requerido"));
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(ApiResponse<bool>.Fail("Usuario no autenticado"));
            }

            var result = await _userService.VerifySmsCodeAsync(userId, request.Code);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying SMS code");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                ApiResponse<bool>.Fail("Error interno del servidor"));
        }
    }
}

/// <summary>
/// Request model for sending SMS
/// </summary>
public class SendSmsRequest
{
    /// <summary>
    /// Phone number to send SMS to
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;
}

/// <summary>
/// Request model for verifying SMS code
/// </summary>
public class VerifySmsRequest
{
    /// <summary>
    /// Verification code received via SMS
    /// </summary>
    public string Code { get; set; } = string.Empty;
}

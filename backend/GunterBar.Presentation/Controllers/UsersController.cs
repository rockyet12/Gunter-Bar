using System.Net;
using System.Security.Claims;
using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs;
using GunterBar.Application.DTOs.User;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GunterBar.Presentation.Controllers;

/// <summary>
/// Controller for managing user operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;
    private readonly IEmailService _emailService;

    public UsersController(IUserService userService, ILogger<UsersController> logger, IEmailService emailService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    /// <summary>
    /// Gets all users (Admin only)
    /// </summary>
    /// <returns>A list of all users</returns>
    /// <response code="200">Returns the list of users</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsers()
    {
        try
        {
            var result = await _userService.GetAllUsersAsync();
            _logger.LogInformation("Retrieved all users successfully");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all users");
            return StatusCode((int)HttpStatusCode.InternalServerError, 
                ApiResponse<IEnumerable<UserDto>>.Fail("An error occurred while processing your request"));
        }
    }

    /// <summary>
    /// Gets a specific user by their ID
    /// <param name="id">The ID of the user</param>
    /// <returns>The user details</returns>
    /// <response code="200">Returns the user details</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="404">If the user is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(int id)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (id != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await _userService.GetByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting user {UserId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, 
                ApiResponse<UserDto>.Fail("An error occurred while processing your request"));
        }
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="createUserDto">The user creation details</param>
    /// <returns>The created user</returns>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">If the user data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var result = await _userService.CreateAsync(createUserDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return CreatedAtAction(nameof(GetUserById), new { id = result.Data.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating user");
            return StatusCode((int)HttpStatusCode.InternalServerError, 
                ApiResponse<UserDto>.Fail("An error occurred while processing your request"));
        }
    }

    /// <summary>
    /// Updates an existing user
    /// </summary>
    /// <param name="id">The ID of the user to update</param>
    /// <param name="updateUserDto">The updated user details</param>
    /// <returns>The updated user</returns>
    /// <response code="200">Returns the updated user</response>
    /// <response code="400">If the user data is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized</response>
    /// <response code="404">If the user is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (id != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await _userService.UpdateAsync(id, updateUserDto);
            if (!result.Success)
            {
                return result.Message.Contains("no encontrado") ? NotFound(result) : BadRequest(result);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user {UserId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, 
                ApiResponse<UserDto>.Fail("An error occurred while processing your request"));
        }
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id">The ID of the user to delete</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the user was successfully deleted</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized</response>
    /// <response code="404">If the user is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            var result = await _userService.DeleteAsync(id);
            if (!result.Success)
            {
                return result.Message.Contains("no encontrado") ? NotFound(result) : BadRequest(result);
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting user {UserId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, 
                ApiResponse<bool>.Fail("An error occurred while processing your request"));
        }
    }

    /// <summary>
    /// Updates a user's role (Admin only)
    /// </summary>
    /// <param name="id">The ID of the user</param>
    /// <param name="role">The new role to assign</param>
    /// <returns>The updated user</returns>
    /// <response code="200">Returns the updated user</response>
    /// <response code="400">If the role update is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized</response>
    /// <response code="404">If the user is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPatch("{id:int}/role")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUserRole(int id, [FromBody] UserRole role)
    {
        try
        {
            // Allow users to change their own role, but only admin can assign vendor role to others
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim.Value, out int currentUserId))
            {
                return Unauthorized(new ApiResponse<UserDto> { Success = false, Message = "Usuario no autenticado" });
            }

            if (role == UserRole.Vendor && id != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid("Solo el administrador puede asignar el rol de vendedor a otros usuarios.");
            }

            var result = await _userService.UpdateRoleAsync(id, role);
            if (!result.Success)
            {
                return result.Message.Contains("no encontrado") ? NotFound(result) : BadRequest(result);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating role for user {UserId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, 
                ApiResponse<UserDto>.Fail("An error occurred while processing your request"));
        }
    }

    /// <summary>
    /// Changes a user's password
    /// </summary>
    /// <param name="id">The ID of the user</param>
    /// <param name="changePasswordDto">The current and new password</param>
    /// <returns>Success or failure response</returns>
    /// <response code="200">If the password was changed successfully</response>
    /// <response code="400">If the password change request is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized</response>
    /// <response code="404">If the user is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost("{id:int}/change-password")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<ApiResponse<bool>>> ChangePassword(int id, [FromBody] ChangePasswordDto changePasswordDto)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (id != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await _userService.ChangePasswordAsync(id, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Success)
            {
                return result.Message.Contains("no encontrado") ? NotFound(result) : BadRequest(result);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while changing password for user {UserId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, 
                ApiResponse<bool>.Fail("An error occurred while processing your request"));
        }
    }

    /// <summary>
    /// Uploads a profile image for the user
    /// </summary>
    /// <param name="id">The ID of the user</param>
    /// <param name="file">The image file</param>
    /// <returns>The updated user with new profile image URL</returns>
    /// <response code="200">Returns the updated user</response>
    /// <response code="400">If the file is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized</response>
    /// <response code="404">If the user is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost("{id:int}/profile-image")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<ApiResponse<UserDto>>> UploadProfileImage(int id, IFormFile file)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (id != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }
            if (file == null || file.Length == 0)
            {
                return BadRequest(ApiResponse<UserDto>.Fail("No file uploaded"));
            }
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Profiles");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
            var ext = Path.GetExtension(file.FileName);
            var fileName = $"user_{id}_{Guid.NewGuid().ToString().Substring(0,8)}{ext}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var imageUrl = $"/uploads/profiles/{fileName}";
            var updateDto = new UpdateUserDto { ProfileImageUrl = imageUrl };
            var result = await _userService.UpdateAsync(id, updateDto);
            if (!result.Success)
            {
                return result.Message.Contains("no encontrado") ? NotFound(result) : BadRequest(result);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading profile image for user {UserId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError,
                ApiResponse<UserDto>.Fail("An error occurred while uploading the image"));
        }
    }

    /// <summary>
    /// Solicita recuperación de contraseña (envía email con link y código)
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPost("request-password-reset")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> RequestPasswordReset([FromBody] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(ApiResponse<bool>.Fail("Email es requerido"));

        // Generar código de seguridad único
        var code = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        // Guardar el código en la base de datos asociado al usuario (implementa en UserService)
        var result = await _userService.SetPasswordResetCodeAsync(email, code);
        if (!result.Success)
            return BadRequest(result);

        // Enviar email con link y código
        var resetLink = $"https://gunterbar.com/reset-password?email={email}&code={code}";
        await _emailService.SendPasswordResetAsync(email, code + "\nLink: " + resetLink);
    return Ok(ApiResponse<bool>.Succeed(true, "Email de recuperación enviado"));
    }

    /// <summary>
    /// Restablece la contraseña usando email y código de seguridad
    /// </summary>
    /// <param name="dto">Datos para el reseteo</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<bool>.Fail("Datos inválidos"));
        var result = await _userService.ResetPasswordWithCodeAsync(dto.Email, dto.Code, dto.NewPassword);
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Gets a vendor's profile including bar information
    /// </summary>
    /// <param name="id">Vendor ID</param>
    /// <returns>Vendor profile with bar details</returns>
    [HttpGet("vendor/{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<VendorProfileDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ApiResponse<VendorProfileDto>>> GetVendorProfile(int id)
    {
        try
        {
            var result = await _userService.GetVendorProfileAsync(id);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vendor profile for user {UserId}", id);
            return StatusCode(500, ApiResponse<VendorProfileDto>.Fail("Error interno del servidor"));
        }
    }
}

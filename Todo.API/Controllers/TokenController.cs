using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Contracts;
using Todo.Application.DTOs.Request;

namespace Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(
        ITokenService _tokenService, 
        IHttpContextAccessor _httpContextAccessor) : ControllerBase
    {
        [HttpPost]
        public async Task <IActionResult> Get(TokenRequestDto requestDto)
        {
            var response = await _tokenService.GetTokenAsync(requestDto);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Error);
            }

            return Ok(response.Value);
        }

        //An access token lets the user access your API, while a refresh token lets the client obtain new access credentials without requiring the user to log in again.

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto requestDto)
        {
            var clientIp = GetClientIpAddress();
            var response = await _tokenService.RefreshTokenAsync(requestDto, clientIp);

            return Ok(response);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(RefreshTokenRequestDto requestDto)
        {
            var clientIp = GetClientIpAddress();
            var result = await _tokenService.RevokeTokenAsync(requestDto.refreshToken, clientIp);

            if (result.IsSuccess)
                return Ok(new { message = "Token revoked successfully." });

            return BadRequest(new { message = "Failed to revoke token." });
        }

        private string? GetClientIpAddress()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return null;

            // Check X-Forwarded-For header first (for proxy scenarios)
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                var ip = forwardedFor.ToString().Split(',').FirstOrDefault()?.Trim();
                if (!string.IsNullOrWhiteSpace(ip))
                    return ip;
            }

            // Fall back to RemoteIpAddress
            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
}

using Todo.Application.Common;
using Todo.Application.DTOs.Request;
using Todo.Application.DTOs.Response;

namespace Todo.Application.Contracts;

public interface ITokenService
{
    /// <summary>
    /// means an asynchronous operation that produces a success-or-failure result containing a token response.
    /// </summary>
    /// <param name="requestDto"></param>
    /// <returns></returns>
    Task<Result<TokenResponseDto>> GetTokenAsync(TokenRequestDto requestDto);

    Task<Result<TokenResponseDto>> RefreshTokenAsync(
        RefreshTokenRequestDto requestDto, string? clientIp = null);

    Task<Result> RevokeTokenAsync(string refreshToken, string? clientIp = null);

    Task<bool> RevokeAllUserTokensAsync(Guid userId, string? clientIp = null);
}


/*
 
Task = asynchronous operation.

Result<T> = success or failure outcome.

TokenResponseDto = the data returned on success.

TokenRequestDto = the data passed into the method.

*/
using Microsoft.AspNetCore.Http;
using Todo.Application.Contracts;

namespace Todo.Application.Implementations;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserId() // A public method that returns a string containing the user ID.
    {
        var userId = _httpContextAccessor.HttpContext.Request.Headers["CurrentUserId"]; //Provides access to the current HTTP request outside the controller. and Reads the CurrentUserId header from the request.

        if (!string.IsNullOrWhiteSpace(userId)) //Checks whether the value is missing, empty, or only whitespace.
            return userId; //Returns the user ID if it exists.

        return string.Empty; //Returns an empty string if no user ID is available.
    }
}


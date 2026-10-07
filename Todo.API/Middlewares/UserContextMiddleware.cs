using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace Todo.API.Middlewares
{
    public class UserContextMiddleware
    {
        // if we want to modify the request/response, we need to use a middleware.
        // This middleware will extract the user id from the access token and add it to the request headers
        // so that it can be accessed in the controller or service layer.
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context) // Trying to fetch the access token from the request and add the current user id to the request headers
        {
            var token = await context.GetTokenAsync("access_token");

            if (string.IsNullOrWhiteSpace(token)) 
            {
                await _next(context); // don't break if token doesn't exist
            }
            else // if token exists, extract the user id from the token and add it to the request headers
            {
                var currentUserId = GetUserId(token);

                context.Request.Headers.Add("CurrentUserId", currentUserId.ToString()); 
                await _next(context);
            }
        }


        private Guid GetUserId(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            // Reading the user id from the token claims. The claim type is "UserId" which is set when the token is generated in tokenService
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value; 

            return Guid.Parse(userId);
        }

    }
}


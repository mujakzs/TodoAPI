
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Todo.API.Middlewares;
using Todo.Application.Constants;
using Todo.Application.Contracts;

namespace Todo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services
                .AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());});
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            //builder.Services.AddMemoryCache();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "RedisDemo:";
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHttpClient(
                ApplicationConstant.EmailServiceClient, client =>
                {
                    client.BaseAddress = new Uri(builder.Configuration.GetSection("EmailService:BaseUrl").Value!);
                })
                //.AddStandardResilienceHandler();
                .AddResilienceHandler("retry", (pipeline, context) =>
                {
                    var loggerFactory = context.ServiceProvider?.GetService<ILoggerFactory>();
                    var logger = loggerFactory?.CreateLogger("EmailServiceResilience");

                    // 3. RETRY (Recover from transient failures)
                    pipeline.AddRetry(new HttpRetryStrategyOptions
                    {
                        MaxRetryAttempts = 3,
                        Delay = TimeSpan.FromSeconds(2),
                        UseJitter = true,
                        BackoffType = DelayBackoffType.Exponential,
                        OnRetry = args =>
                        {
                            // Log as Warning (not Error), because a retry means we are still attempting to recover
                            logger?.LogWarning(
                                "Retry attempt {Attempt}. Delay: {Delay}ms. Cause: {Reason}",
                                args.AttemptNumber + 1,
                                args.RetryDelay.TotalMilliseconds,
                                args.Outcome.Exception?.Message ?? args.Outcome.Result?.StatusCode.ToString());

                            return ValueTask.CompletedTask;
                        }
                    });
                });

            builder.Services.AddHealthChecks();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:SecretKey").Value)),
                        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
                        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var sidClaim = context.Principal?.FindFirst("sid");
                            if (sidClaim == null || !Guid.TryParse(sidClaim.Value, out var sessionId))
                            {
                                context.Fail("Invalid token: missing session reference");
                                return;
                            }

                            var revocationService = context.HttpContext.RequestServices
                                .GetRequiredService<ITokenRevocationService>();

                            bool isRevoked = await revocationService.IsSessionRevokedAsync(sessionId);

                            if (isRevoked)
                            {
                                context.Fail("Session has been revoked");
                            }
                        }

                    };
                });

            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddApplication();

            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        status = report.Status.ToString(),
                        totalDuration = report.TotalDuration.TotalMilliseconds,
                        checks = report.Entries.Select(entry => new
                        {
                            name = entry.Key,
                            status = entry.Value.Status.ToString(),
                            exception = entry.Value.Exception?.Message,
                            duration = entry.Value.Duration.ToString(),
                            description = entry.Value.Description
                        })
                    };

                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(response, new JsonSerializerOptions
                        {
                            WriteIndented = true
                        }));
                }
            });


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserContextMiddleware>();


            app.MapControllers();

            app.Run();

        }
    }
}


using System;
using System.Text.Json;
using System.Threading.Tasks;
using CisApi.Core.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CisApi.Presentation.Middlewares
{
    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context, IUserQueryService userQueryService)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                return;
            }

            try
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader))
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(
                    new
                    {
                        Error = "Authorization header is missing or empty."
                    }));
                    return;
                }

                if (authHeader.StartsWith("Basic "))
                {
                    var encodedAuth = authHeader.Substring("Basic ".Length).Trim();
                    var decodedAuth = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedAuth));
                    var credentials = decodedAuth.Split(':', 2);

                    if (credentials.Length != 2)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(
                        new
                        {
                            Error = "Invalid Basic authentication header format."
                        }));
                        return;
                    }

                    var username = credentials[0];
                    var password = credentials[1];

                    var user = await userQueryService.GetUserAsync(username, password);
                    if (user == null)
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(
                        new
                        {
                            Error = "Invalid username or password."
                        }));
                        return;
                    }
                    else
                    {
                        context.Items["User"] = user;
                        await _next(context);
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(
                    new
                    {
                        Error = "Unsupported authentication scheme."
                    }));
                    return;
                }
            }
            catch (HttpRequestException)
            {
                context.Response.StatusCode = 503; // Service Unavailable
                await context.Response.WriteAsync("Unable to connect to the authentication service.");
            }
            catch (Exception)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid authentication header");
            }
        }
    }
}

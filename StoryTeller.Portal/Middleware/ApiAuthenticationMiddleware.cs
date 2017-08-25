using System;
using System.Data.SqlClient;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using StoryTeller.ResultAggregation.Settings;

namespace StoryTeller.Portal.Middleware
{
    public class ApiAuthenticationMiddleware
    {
        private const string _apiKeyHeaderName = "x-api-key";
        private readonly ILoggerFactory _loggerFactory;
        private readonly ISqlSettings _sqlSettings;

        public ApiAuthenticationMiddleware(ILoggerFactory loggerFactory, ISqlSettings sqlSettings)
        {
            _loggerFactory = loggerFactory;
            _sqlSettings = sqlSettings;
        }

        public async Task Invoke(HttpContext context, Func<Task> next)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                IHeaderDictionary headers = context.Request.Headers;

                if (!headers.ContainsKey(_apiKeyHeaderName))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync($"Request must contain {_apiKeyHeaderName} header");
                    return;
                }

                StringValues apiKeyValue = headers[_apiKeyHeaderName];

                var apiKey = default(Guid);

                if (!Guid.TryParse(apiKeyValue, out apiKey))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("ApiKey is either not found or invalid");
                    return;
                }

                using (var conn = new SqlConnection(_sqlSettings.ResultsDbConnStr))
                {
                    object applicationId =
                        await conn.ExecuteScalarAsync("select top 1 id from [app] where apikey = @apiKey",
                            new {apiKey});

                    if (applicationId == null)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsync("ApiKey is either not found or invalid");
                        return;
                    }

                    var appPrincipal = new ClaimsPrincipal();
                    appPrincipal.AddIdentity(new ClaimsIdentity(new[]
                    {
                        new Claim("ApplicationId", applicationId.ToString())
                    }));

                    context.User = appPrincipal;
                }
            }

            await next();
        }
    }
}

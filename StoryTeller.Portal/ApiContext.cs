using Microsoft.AspNetCore.Http;
using System.Linq;

namespace StoryTeller.Portal
{
    public class ApiContext : IApiContext
    {
        public int AppId { get; }

        public ApiContext(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor.HttpContext == null)
                return;

            var applicationIdClaim = contextAccessor.HttpContext.User.Claims.SingleOrDefault(c => c.Type == "ApplicationId");
            AppId = int.Parse(applicationIdClaim.Value);
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ItLabs.MultiTenant.Core
{
    /// <summary>
    /// Identify the tenant by the HTTP Request Host value
    /// </summary>
    public class RequestHostTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestHostTenantIdentificationStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetTenantIdentifierAsync()
        {
            if(_httpContextAccessor.HttpContext.Request == null || !_httpContextAccessor.HttpContext.Request.Host.HasValue)
            {
                throw new Exception("Request Host is not found");
            }

            var requestHost = _httpContextAccessor.HttpContext.Request.Host.Value;
            return await Task.FromResult(requestHost);
        }
    }
}

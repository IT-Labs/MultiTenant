using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MultiTenant.Core
{
    public class SQLDatabaseTenantStorage : ITenantStorage<Tenant>
    {
        private readonly ICache _cache;
        private readonly IConfiguration _configuration;

        public SQLDatabaseTenantStorage(ICache cache, IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        public async Task<Tenant> GetTenantAsync(string identifier)
        {
            var tenant = _cache.GetOrSet(identifier, () => GetTenantFromSQLDatabase(identifier));
            return await Task.FromResult(tenant);
        }

        private Tenant GetTenantFromSQLDatabase(string identifier)
        {
            Tenant tenant;
            using (var tenantsDbContext = new TenantsDbContext(new DbContextOptionsBuilder<TenantsDbContext>().Options, _configuration))
            {
                tenant = tenantsDbContext.Tenants.SingleOrDefault(t => t.Identifier == identifier);
            }

            if (tenant == null)
            {
                throw new Exception($"Tenant with identifier: {identifier} is not found");
            }

            return tenant;
        }
    }
}

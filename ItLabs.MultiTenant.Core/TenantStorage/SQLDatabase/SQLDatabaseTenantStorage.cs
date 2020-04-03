using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MultiTenant.Core
{
    /// <summary>
    /// SQL Database storage for tenant data
    /// </summary>
    public class SQLDatabaseTenantStorage : ITenantStorage<Tenant>
    {
        private readonly ICache _cache;
        private readonly IConfiguration _configuration;

        public SQLDatabaseTenantStorage(ICache cache, IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        /// <summary>
        /// Get the tenant data from storage by tenant identifier 
        /// Check cache before getting the data
        /// </summary>
        /// <param name="identifier">The tenant identifier</param>
        /// <returns>The Tenant</returns>
        public async Task<Tenant> GetTenantAsync(string identifier)
        {
            var tenant = _cache.GetOrSet(identifier, () => GetTenantFromSQLDatabase(identifier));
            return await Task.FromResult(tenant);
        }

        /// <summary>
        /// Get tenant data by identifier from the database using EF
        /// </summary>
        /// <param name="identifier">The tenant identifier</param>
        /// <returns>The Tenant</returns>
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

using Amazon;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MultiTenant.Core
{
    public class AWSSecretsManagerTenantStorage : ITenantStorage<Tenant>
    {
        private readonly ICache _cache;
        private readonly IConfiguration _configuration;

        public AWSSecretsManagerTenantStorage(ICache cache, IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        public async Task<Tenant> GetTenantAsync(string identifier)
        {
            var tenant = _cache.GetOrSet(identifier, () => GetTenantFromAWSSecretsManager(secretTagKeyIdentifier: identifier));
            return await Task.FromResult(tenant);
        }

        private Tenant GetTenantFromAWSSecretsManager(string secretTagKeyIdentifier)
        {
            var awsSecrets = new ConfigurationBuilder().AddSecretsManager(
                region: RegionEndpoint.GetBySystemName(_configuration["AWSRegion"]),
                configurator: opts =>
                {
                    opts.SecretFilter = entry => entry.Tags.Any(tag => tag.Key == secretTagKeyIdentifier);
                    opts.KeyGenerator = (entry, key) => key.Split(":").LastOrDefault();
                }).Build();

            var tenantId = awsSecrets.GetValue("Id", defaultValue: string.Empty);
            var connectionString = awsSecrets.GetValue("ConnectionString", defaultValue: string.Empty);

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"Tenant data with secret tag identifier: {secretTagKeyIdentifier} is not found");
            }

            var tenant = new Tenant
            {
                Id = tenantId,
                Identifier = secretTagKeyIdentifier,
                ConnectionString = connectionString
            };

            return tenant;
        }
    }
}

using System.Threading.Tasks;

namespace ItLabs.MultiTenant.Core
{
    public class TenantService<T> where T : Tenant
    {
        private readonly ITenantIdentificationStrategy _tenantIdentificationStrategy;
        private readonly ITenantStorage<T> _tenantStorage;

        public TenantService(ITenantIdentificationStrategy tenantIdentificationStrategy, ITenantStorage<T> tenantStorage)
        {
            _tenantIdentificationStrategy = tenantIdentificationStrategy;
            _tenantStorage = tenantStorage;
        }

        public async Task<T> GetTenantAsync()
        {
            var identifier = await _tenantIdentificationStrategy.GetTenantIdentifierAsync();
            var tenant = await _tenantStorage.GetTenantAsync(identifier);

            return tenant;
        }
    }
}

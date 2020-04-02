using System.Threading.Tasks;

namespace ItLabs.MultiTenant.Core
{
    public interface ITenantStorage<T> where T : Tenant
    {
        Task<T> GetTenantAsync(string identifier);
    }
}

using System.Threading.Tasks;

namespace ItLabs.MultiTenant.Core
{
    public interface ITenantIdentificationStrategy
    {
        Task<string> GetTenantIdentifierAsync();
    }
}

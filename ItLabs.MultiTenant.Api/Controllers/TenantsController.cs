using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItLabs.MultiTenant.Api.Controllers
{
    /// <summary>
    /// Controller used to showcase the different tasks per tenant
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly TasksDbContext _dbContext;

        public TenantsController(TasksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public string ConfigureDatabase()
        {
             _dbContext.Database.Migrate();
            return "Tenant Database has been successfully created and migrated to latest schema version";
        }
    }
}

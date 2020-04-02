using ItLabs.MultiTenant.Core;
using Microsoft.EntityFrameworkCore;

namespace ItLabs.MultiTenant.Api
{
    public class TasksDbContext : DbContext
    {
        private readonly TenantService<Tenant> _tenantService;

        public TasksDbContext(DbContextOptions<TasksDbContext> options, TenantService<Tenant> tenantService)
          : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var tenant = _tenantService.GetTenantAsync().GetAwaiter().GetResult();

                optionsBuilder.UseSqlServer(tenant.ConnectionString);
            }
        }
    }
}

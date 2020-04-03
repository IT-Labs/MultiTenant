using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ItLabs.MultiTenant.Core
{
    /// <summary>
    /// The EF DBContext used for storing tenant data, when SQL database is used as tenant storage
    /// </summary>
    public class TenantsDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public TenantsDbContext(DbContextOptions<TenantsDbContext> options, IConfiguration configuration)
          : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MasterDatabase"), 
                    o => o.MigrationsAssembly(typeof(TenantsDbContext).Namespace));
            }
        }
    }
}

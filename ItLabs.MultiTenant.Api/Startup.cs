using ItLabs.MultiTenant.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

namespace ItLabs.MultiTenant.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.TryAddSingleton(provider => Configuration);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.TryAddSingleton<ICache>(cache => new ConcurrentDictionaryCache(new ConcurrentDictionary<string, object>()));
            services.TryAddSingleton<ITenantIdentificationStrategy, RequestHostTenantIdentificationStrategy>();
            services.TryAddSingleton<ITenantStorage<Tenant>, AWSSecretsManagerTenantStorage>();
            services.TryAddTransient<TenantService<Tenant>>();

            services.AddDbContext<TenantsDbContext>();
            services.AddDbContext<TasksDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

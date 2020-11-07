using System;
using LiepaService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using LiepaService.Handlers;
using Microsoft.AspNetCore.Authentication;
using LiepaService.Services;
using LiepaService.Extensions;
using LiepaService.Middleware;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace LiepaService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<LiepaDemoDatabaseContext>(opt =>
               opt.UseLazyLoadingProxies().UseMySql(Configuration.GetConnectionString("DefaultConnection"), mysqlOptions =>      
                        mysqlOptions.ServerVersion(new ServerVersion(new Version(10, 1, 37), ServerType.MariaDb))));
            
            services.AddControllers().
                AddXmlSerializerFormatters();

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddMemoryCache();

            services.AddScoped<ILiepaAuthenticationService, LiepaAuthenticationService>();
            services.AddScoped<IUserDatabaseAccessService, CachedUserDatabaseAccessService>();

            services.AddSingleton<IActionResultExecutor<ObjectResult>, LiepaResponseWrapper>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseLiepaExceptionHandling();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

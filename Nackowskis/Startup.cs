using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nackowskis.Infrastructure;
using Nackowskis.Models;
using Nackowskis.Repository;

namespace Nackowskis
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"
                )));

            services.AddTransient<IAuctionRepository, AuctionRepository>();
            services.AddTransient<IBidRepository, BidRepository>();
            services.AddTransient<UserMethods>();
            services.AddTransient<AuctionMethods>();
            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 4;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddDistributedMemoryCache();

            services.AddSingleton<HttpClientServices>();

            
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "370379800654-1n13e3gonhc8e618bpvtm5sf5h20bsde.apps.googleusercontent.com";
                options.ClientSecret = "Mb7MjlCwWURUpp5tTK7Soj_z";
                options.CallbackPath = "/User/GoogleResponse";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });
        }
    }
}

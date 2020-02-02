using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using terminus.shared.models;
using terminus_webapp.Areas.Identity;
using terminus_webapp.Data;

namespace terminus_webapp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
           
      
          


            var connectionString = Configuration["connectionStrings:dbconn"];

            services.AddDbContext<AppDBContext>(options =>
                   options.UseSqlServer(
                      connectionString));

            services.AddIdentity<AppUser, AppRole>(options => { options.SignIn.RequireConfirmedEmail = false; 

            })
                .AddEntityFrameworkStores<AppDBContext>();
            
            services.AddRazorPages();
            services.AddServerSideBlazor();
            //SQL database Connnection(name defined in appsettings.json)
            var SQLConnectionConfiguration = new SQLConnectionConfiguration(Configuration.GetConnectionString("dbconn"));
            services.AddSingleton(SQLConnectionConfiguration);

            //

            services.AddServerSideBlazor(o => o.DetailedErrors = true);
            
            //services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>()
            services.AddBlazoredSessionStorage();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            services.AddScoped<DapperManager, DapperManager>();
            services.AddTransient<IdentitySeeder>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(  IApplicationBuilder app, 
                                IWebHostEnvironment env
                                )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
              //  app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            //identitySeeder.Seed().Wait();
        }
    }
}

using DonationHubWeb.Context;
using DonationHubWeb.Services;
using DonationHubWeb.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DonationHubWeb
{
    public class Startup
    {
        protected IConfigurationRoot Configuration;
        public Startup()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            Configuration = configurationBuilder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {     
            services.AddDbContext<DonationHubWebContext>(builder =>
            {
                var config = Configuration["ConnectionStrings:DefaultConnection"];
                builder.UseSqlServer(config);
            });
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<DonationHubWebContext>();
            services.AddScoped<IDonationService, DonationService>();
            services.AddScoped<IInstitutionService, InstitutionService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using InstagramClone.Domain.Models;
using InstagramClone.Domain.Infrastructure;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Infrastucture;
using System.Text;
using InstagramClone.Domain.Services;
using System;
using Microsoft.AspNetCore.Identity;

namespace InstagramClone
{
    public class Startup
    {
        private IServiceCollection _services;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            string connectionString = Configuration.GetConnectionString("DefaultConnection")
                .Replace("{AppDir}", Environment.CurrentDirectory);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IAdminService, AdminService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            _services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();

            if (!roleManager.RoleExistsAsync("admin").Result)
                roleManager.CreateAsync(new IdentityRole("admin")).Wait();

            if (!roleManager.RoleExistsAsync("moder").Result)
                roleManager.CreateAsync(new IdentityRole("moder")).Wait();

            if (!roleManager.RoleExistsAsync("user").Result)
                roleManager.CreateAsync(new IdentityRole("user")).Wait();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "admin",
                    template: "admin",
                    defaults: new { controller = "Admin", action = "GetAllUsers"}
                );

                routes.MapRoute(
                    name: "profile1",
                    template: "{alias}",
                    defaults: new { controller = "Profile", action = "GetUserProfile" }
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}",
                    defaults: new { controller = "home", action = "index" }
                );

                routes.MapRoute(
                    name: "profile2",
                    template: "profile/{alias}",
                    defaults: new { controller = "Profile", action = "GetUserProfile" }
                );

                routes.MapRoute(
                    name: "profile3",
                    template: "profile/follow/{alias}",
                    defaults: new { controller = "Profile", action = "Follow" }
                );

                routes.MapRoute(
                    name: "post",
                    template: "post/{id}",
                    defaults: new { controller = "Post", action = "ShowPost" }
                );
            });
        }

        private void ListAllRegisteredServices(IApplicationBuilder app)
        {
            app.Map("/allservices", builder => builder.Run(async context =>
            {
                var sb = new StringBuilder();
                sb.Append("<h1>All Services</h1>");
                sb.Append("<table><thead>");
                sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
                sb.Append("</thead><tbody>");
                foreach (var svc in _services)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                    sb.Append($"<td>{svc.Lifetime}</td>");
                    sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table>");
                await context.Response.WriteAsync(sb.ToString());
            }));
        }
    }
}

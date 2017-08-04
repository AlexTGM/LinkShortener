using System.Net;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using LinkShortener.API.Models.Database;
using LinkShortener.API.Repository;
using LinkShortener.API.Repository.Impl;
using LinkShortener.API.Services.LinkShortener;
using LinkShortener.API.Services.LinkShortener.Impl;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LinkShortener
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<LinkShortenerContext>(options =>
            {
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=link_shorneter;Trusted_Connection=True;");
                options.UseOpenIddict();
            });

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<LinkShortenerContext>()
                    .AddDefaultTokenProviders();

            services.AddOpenIddict(options =>
            {
                options.AddEntityFrameworkCoreStores<LinkShortenerContext>();
                options.AddMvcBinders();
                options.EnableTokenEndpoint("/connect/token");
                options.AllowPasswordFlow();
                options.DisableHttpsRequirement();
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;

                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == (int) HttpStatusCode.OK)
                            ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        else
                            ctx.Response.Redirect(ctx.RedirectUri);
                        return Task.FromResult(0);
                    }
                };
            });

            services.AddSingleton<ICollisionResolverFactory<ICollisionResolver>, BasicCollisionResolverFactory>();

            services.AddSingleton<LinkShortenerContext>();
            services.AddSingleton<IRepository<ShortLink>, Repository<ShortLink>>();

            services.AddSingleton<IShortLinkGenerator, ShortLinkGenerator>();
            services.AddSingleton<ILinkShortenerService, LinkShortenerService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseOAuthValidation();
            app.UseIdentity();
            app.UseOpenIddict();
            app.UseStaticFiles();
            app.UseRewriter(new RewriteOptions().AddRewrite("([A-Z0-9]{7})", "api/shortened/$1", false));
            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
        }
    }
}
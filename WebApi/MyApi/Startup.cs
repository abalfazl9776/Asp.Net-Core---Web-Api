using Autofac;
using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Hosting;
using WebFramework.Configuration;
using WebFramework.CustomMapping;
using WebFramework.Middlewares;
using WebFramework.Swagger;

namespace MyApi
{
    public class Startup
    {
        private readonly SiteSettings _siteSetting;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddUserSecrets("f7a95b8f-5002-457c-a28c-e1198aa96645", true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();

            _siteSetting = Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));

            services.InitializeAutoMapper();
            
            services.AddDbContext(Configuration);

            services.AddCustomIdentity(_siteSetting.IdentitySettings);

            //services.AddIs4Authentication(Configuration);
            services.AddCors();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("default", policy =>
            //    {
            //        policy.WithOrigins(Configuration["clientUrl"])
            //            .AllowAnyHeader()
            //            .AllowAnyMethod();
            //    });
            //});

            //services.AddCors(corsOptions =>
            //{
            //    corsOptions.AddPolicy("fully permissive", configurePolicy =>
            //    {
            //        configurePolicy.WithOrigins("http://localhost:4200")
            //            .AllowAnyHeader()
            //            .AllowAnyMethod()
            //            .AllowCredentials();
            //    });
            //});

            services.AddControllers(options => options.Filters.Add(new AuthorizeFilter()));

            services.AddJwtAuthentication(_siteSetting.JwtSettings);
            services.AddGoogleAuthentication(Configuration);

            services.AddCustomHttpClient(Configuration);

            services.AddCustomApiVersioning();

            services.AddSwagger();
        }

        // ConfigureContainer is where you can register things directly with Autofac.
        // This runs after ConfigureServices so the things here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac, like:
            builder.RegisterModule(new AutofacConfigurationModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.InitializeDatabase();

            app.UseCustomExceptionHandler();

            if (env.IsProduction())
            {
                app.UseHsts(env);
            }

            app.UseHttpsRedirection();

            app.UseSwaggerAndUi();

			// must be before UseRouting
            app.MapWhen(context => context.Request.Path.Value.StartsWith("/assets"), 
                appBuilder => appBuilder.UseStaticFiles());
            //app.UseStaticFiles();

            app.UseRouting();

            // it's better UseCors placed before UseEndpoints and after Routing
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

			// must be after UseRouting
            app.UseAuthentication();

			// must be after UseAuthentication
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()/*.RequireAuthorization()*/;
                //config.MapDefaultControllerRoute(); // Map default route {controller=Home}/{action=Index}/{id?}
                /*endpoints.MapDefaultControllerRoute();*/
            });
        }
    }
}

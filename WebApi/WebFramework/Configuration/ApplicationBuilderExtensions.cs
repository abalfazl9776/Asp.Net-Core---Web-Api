using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using Common.Utilities;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.DataInitializer;

namespace WebFramework.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseHsts(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            Assert.NotNull(app, nameof(app));
            Assert.NotNull(env, nameof(env));

            if (!env.IsDevelopment())
                app.UseHsts();
        }

        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>(); //Service locator

            //Dos not use Migrations, just Create Database with latest changes
            //dbContext.Database.EnsureCreated();
            //Applies any pending migrations for the context to the database like (Update-Database)
            //dbContext.Database.Migrate();

            //Initialize database with some default values (Seeding database)
            //var dataInitializers = scope.ServiceProvider.GetServices<IDataInitializer>();
            //foreach (var dataInitializer in dataInitializers)
            //    dataInitializer.InitializeData();

            if (dbContext.Database.CanConnect())
            {
                dbContext.Database.Migrate();
            }
            else
            {
                dbContext.Database.Migrate();
                var dataInitializers = scope.ServiceProvider.GetServices<IDataInitializer>();
                foreach (var dataInitializer in dataInitializers)
                    dataInitializer.InitializeData();
            }
        }
    }
}

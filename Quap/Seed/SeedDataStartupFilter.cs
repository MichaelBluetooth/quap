using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Quap.Models;
using Quap.Services.UserManagement;

namespace Quap.Seed
{
    public class SeedDataStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    using (var scope = builder.ApplicationServices.CreateScope())
                    {
                        IUserManagementService users = scope.ServiceProvider.GetService<IUserManagementService>();
                        QuapDbContext db = scope.ServiceProvider.GetService<QuapDbContext>();
                        SeedDataHelper.Seed(users, db);
                    };
                }

                next(builder);
            };
        }
    }
}
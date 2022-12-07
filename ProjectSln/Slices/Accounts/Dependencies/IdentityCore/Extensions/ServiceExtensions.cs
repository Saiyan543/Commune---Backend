using Main.Slices.Accounts.Dependencies.IdentityCore.Context;
using Main.Slices.Accounts.Dependencies.IdentityCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Main.Slices.Accounts.Dependencies.IdentityCore.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureEFCoreSqlServer(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<IdentityContext>(opts =>
            {
                opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

        public static void ConfigureIdentityStores(this IServiceCollection services) =>

            services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            })
           .AddEntityFrameworkStores<IdentityContext>()
           .AddDefaultTokenProviders();
    }
}
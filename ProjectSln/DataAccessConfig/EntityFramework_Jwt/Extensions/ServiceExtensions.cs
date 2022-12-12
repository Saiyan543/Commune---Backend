using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Main.DataAccessConfig.EntityFramework_Jwt.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureEFCoreSqlServer(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<IdentityContext>(opts =>
            {
                opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = new JwtOptions();
            services.Configure<JwtOptions>(configuration.GetSection(jwtOptions.Section));
            configuration.Bind(jwtOptions.Section, jwtOptions);

            var secret = "sEcretlOlooooloolol";//Environment.GetEnvironmentVariable("JwtSecret");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };
            });
        }

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
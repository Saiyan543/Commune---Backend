
using Main.Slices.Accounts.Dependencies.IdentityCore.Configuration.Models.DbModels;
using Main.Slices.Accounts.Dependencies.IdentityCore.Configuration.Roles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Main.Slices.Accounts.Dependencies.IdentityCore.Context
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public IdentityContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
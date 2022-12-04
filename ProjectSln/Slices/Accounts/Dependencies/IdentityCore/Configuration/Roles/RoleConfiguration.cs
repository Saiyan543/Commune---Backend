using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Main.Slices.Accounts.Dependencies.IdentityCore.Configuration.Roles
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
              new IdentityRole
              {
                  Name = "Security",
                  NormalizedName = "SECURITY"
              },

            new IdentityRole
            {
                Name = "Company",
                NormalizedName = "COMPANY"
            },
              new IdentityRole
              {
                  Name = "Administrator",
                  NormalizedName = "ADMINISTRATOR"
              }

            );
        }
    }
}
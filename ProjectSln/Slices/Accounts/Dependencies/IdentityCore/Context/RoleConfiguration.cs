using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Main.Slices.Accounts.Dependencies.IdentityCore.Context
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
                Name = "Club",
                NormalizedName = "CLUB"
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
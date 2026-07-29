using FormulaFlow.Data.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "c8d0259a-9bc3-4aee-ade0-a1459f78c87e",
                    Name = RoleNames.Admin,
                    NormalizedName = RoleNames.Admin.ToUpper(),
                    ConcurrencyStamp = "50b0f96f-1209-414c-b9b9-d730c540e5c4",
                },
                new IdentityRole
                {
                    Id = "9a5dd50e-453c-4926-afca-722595a8f3b2",
                    Name = RoleNames.User,
                    NormalizedName = RoleNames.User.ToUpper(),
                    ConcurrencyStamp = "3df15088-d9ae-4996-95a2-90b94eb4255f",
                }
            );
        }
    }
}

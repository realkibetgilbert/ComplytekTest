using ComplytekTest.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplytekTest.API.Infrastructure.ModelConfigurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(d => d.Projects)
                .WithOne(p => p.Department)
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

using ComplytekTest.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplytekTest.API.Infrastructure.ModelConfigurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Email)
                .IsUnique();

            builder.Property(e => e.Salary)
                .IsRequired()
                .HasPrecision(18, 2);
        }
    }
}

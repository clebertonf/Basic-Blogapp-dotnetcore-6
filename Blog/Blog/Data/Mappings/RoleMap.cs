using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings;

public class RoleMap: IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
       builder.ToTable("Roles");
       builder.HasKey(x => x.Id);
       builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
       builder.Property(x => x.Slug).HasMaxLength(50).IsRequired();
    }
}
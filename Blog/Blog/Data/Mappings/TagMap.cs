using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings;

public class TagMap : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(50).IsRequired();
    }
}
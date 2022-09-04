using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessment.Data.Entities.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Images");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar");

            builder.Property(i => i.ImagePath)
                .IsRequired()
                .HasColumnType("nvarchar(MAX)");
        }
    }
}

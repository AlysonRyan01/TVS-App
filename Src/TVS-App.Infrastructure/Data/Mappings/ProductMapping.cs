using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TVS_App.Domain.Entities;

namespace TVS_App.Infrastructure.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.OwnsOne(p => p.Brand, brand =>
        {
            brand.Property(b => b.ProductBrand)
                .HasColumnName("Brand")
                .HasMaxLength(100)
                .IsRequired(false);
        });

        builder.OwnsOne(p => p.Model, model =>
        {
            model.Property(m => m.ProductModel)
                .HasColumnName("Model")
                .HasMaxLength(100)
                .IsRequired(false);
        });

        builder.OwnsOne(p => p.SerialNumber, serial =>
        {
            serial.Property(s => s.ProductSerialNumber)
                .HasColumnName("SerialNumber")
                .HasMaxLength(100)
                .IsRequired(false);
        });

        builder.OwnsOne(p => p.Defect, defect =>
        {
            defect.Property(d => d.ProductDefect)
                .HasColumnName("Defect")
                .HasMaxLength(200);
        });

        builder.Property(p => p.Accessories)
            .HasMaxLength(200);

        builder.Property(p => p.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(p => p.ServiceOrder)
               .WithOne(so => so.Product)
               .HasForeignKey<Product>(p => p.ServiceOrderId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
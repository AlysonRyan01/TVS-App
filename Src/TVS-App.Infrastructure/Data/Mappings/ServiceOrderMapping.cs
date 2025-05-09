using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TVS_App.Domain.Entities;

namespace TVS_App.Infrastructure.Data.Mappings;

public class ServiceOrderMapping : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        builder.ToTable("ServiceOrders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntryDate).IsRequired();
        builder.Property(x => x.InspectionDate);
        builder.Property(x => x.RepairDate);
        builder.Property(x => x.DeliveryDate);

        builder.Property(x => x.Enterprise)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ServiceOrderStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.RepairStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.RepairResult)
            .HasConversion<int?>();

        builder.HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Product)
            .WithOne(p => p.ServiceOrder)
            .HasForeignKey<ServiceOrder>(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(x => x.Solution, s =>
        {
            s.Property(x => x.ServiceOrderSolution)
                .HasColumnName("Solution")
                .HasMaxLength(500);
        });

        builder.OwnsOne(x => x.PartCost, pc =>
        {
            pc.Property(x => x.ServiceOrderPartCost)
                .HasColumnName("PartCost")
                .HasPrecision(18, 2);
        });

        builder.OwnsOne(x => x.LaborCost, lc =>
        {
            lc.Property(x => x.ServiceOrderLaborCost)
                .HasColumnName("LaborCost")
                .HasPrecision(18, 2);
        });
    }
}
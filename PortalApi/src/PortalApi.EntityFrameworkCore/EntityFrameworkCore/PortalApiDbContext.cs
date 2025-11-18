using Microsoft.EntityFrameworkCore;
using PortalApi.Products;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace PortalApi.EntityFrameworkCore;

[ConnectionStringName("Default")]
public class PortalApiDbContext : AbpDbContext<PortalApiDbContext>
{
    public DbSet<Product> Products { get; set; }

    public PortalApiDbContext(DbContextOptions<PortalApiDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>(b =>
        {
            b.ToTable("Products");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.Description).HasMaxLength(1000);
            b.Property(x => x.Price).HasColumnType("decimal(18,2)");
            b.Property(x => x.Stock).IsRequired();
            b.Property(x => x.CreatedDate).IsRequired();
        });
    }
}

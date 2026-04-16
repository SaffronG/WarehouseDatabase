using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public partial class AppDbContext
{
    public virtual DbSet<AppUser> Users { get; set; }
    public virtual DbSet<SalesOrderItem> SalesOrderItems { get; set; }
    public virtual DbSet<Order> Orders { get; set; }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("app_user_pkey");
            entity.ToTable("app_user", "warehouse");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasColumnName("password_hash");
            entity.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasColumnName("password_salt");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("ux_app_user_email");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sales_order_pkey");
            entity.ToTable("order", "warehouse");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateOrdered)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_ordered");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsRequired()
                .HasDefaultValue(OrderStatus.Created.ToString().ToUpperInvariant())
                .HasColumnName("status");
            entity.Property(e => e.DatePicked)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_picked");
            entity.Property(e => e.DatePacked)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_packed");
            entity.Property(e => e.DateShipped)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_shipped");
        });

        modelBuilder.Entity<SalesOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sales_order_item_pkey");
            entity.ToTable("sales_order_item", "warehouse");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SalesOrderId).HasColumnName("order_id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.SalesOrderItems)
                .HasForeignKey(d => d.SalesOrderId)
                .HasConstraintName("sales_order_item_order_id_fkey");

            entity.HasOne(d => d.Item)
                .WithMany()
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("sales_order_item_item_id_fkey");
        });

        modelBuilder.Entity<Bin>(entity =>
        {
            entity.HasCheckConstraint("ck_bin_quantity_nonnegative", "quantity >= 0");
        });
    }
}

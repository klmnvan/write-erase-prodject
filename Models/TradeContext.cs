using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WriteErase.Models;

public partial class TradeContext : DbContext
{
    public TradeContext()
    {
    }

    public TradeContext(DbContextOptions<TradeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<PickUpPoint> PickUpPoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=trade;Username=postgres;Password=1");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categoty_pkey");

            entity.ToTable("category");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('categoty_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("manufacturer_pkey");

            entity.ToTable("manufacturer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.DateDelivery).HasColumnName("date_delivery");
            entity.Property(e => e.DateOrder).HasColumnName("date_order");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.PickUp).HasColumnName("pick_up");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("orders_users_fk");

            entity.HasOne(d => d.PickUpNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PickUp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_pick_up_fkey");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_status_fkey");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderproduct_pkey");

            entity.ToTable("order_product");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('orderproduct_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductArticleNumber)
                .HasMaxLength(100)
                .HasColumnName("product_article_number");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_order_id_fkey");

            entity.HasOne(d => d.ProductArticleNumberNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductArticleNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_product_article_number_fkey");
        });

        modelBuilder.Entity<PickUpPoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pick_up_point_pkey");

            entity.ToTable("pick_up_point");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.ArticleNumber)
                .HasMaxLength(100)
                .HasColumnName("article_number");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Cost)
                .HasPrecision(19, 4)
                .HasColumnName("cost");
            entity.Property(e => e.CurrentDiscount)
                .HasPrecision(19, 4)
                .HasColumnName("current_discount");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .HasColumnName("image");
            entity.Property(e => e.Manufacturer).HasColumnName("manufacturer");
            entity.Property(e => e.MaxDiscountAmount).HasColumnName("max_discount_amount");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.QuantityInStock).HasColumnName("quantity_in_stock");
            entity.Property(e => e.Supplier).HasColumnName("supplier");
            entity.Property(e => e.Unit).HasColumnName("unit");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_category_fkey");

            entity.HasOne(d => d.ManufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Manufacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_manufacturer_fkey");

            entity.HasOne(d => d.SupplierNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Supplier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_supplier_fkey");

            entity.HasOne(d => d.UnitNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Unit)
                .HasConstraintName("product_units_fk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");

            entity.ToTable("status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("supplier_pkey");

            entity.ToTable("supplier");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.IdUnit).HasName("units_pk");

            entity.ToTable("units");

            entity.Property(e => e.IdUnit).HasColumnName("id_unit");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(100)
                .HasColumnName("patronymic");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .HasColumnName("surname");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_role_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

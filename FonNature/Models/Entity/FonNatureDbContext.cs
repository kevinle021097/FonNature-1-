namespace Models.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FonNatureDbContext : DbContext
    {
        public FonNatureDbContext()
            : base("name=FonNatureDbContext")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<ExportInvoice> ExportInvoices { get; set; }
        public virtual DbSet<ImportInvoice> ImportInvoices { get; set; }
        public virtual DbSet<ImportInvoiceInformation> ImportInvoiceInformations { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<OrderInformation> OrderInformations { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<StatusOrder> StatusOrders { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(e => e.Phone)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.IdCustomer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ImportInvoice>()
                .HasMany(e => e.ImportInvoiceInformations)
                .WithRequired(e => e.ImportInvoice)
                .HasForeignKey(e => e.IdImportInvoice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ImportInvoiceInformation>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Customers)
                .WithOptional(e => e.Member)
                .HasForeignKey(e => e.IdMember);

            modelBuilder.Entity<OrderInformation>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.TotalPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.ExportInvoices)
                .WithOptional(e => e.Order)
                .HasForeignKey(e => e.IdOrder);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderInformations)
                .WithRequired(e => e.Order)
                .HasForeignKey(e => e.IdOrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Position>()
                .HasMany(e => e.Staffs)
                .WithOptional(e => e.Position)
                .HasForeignKey(e => e.IdPosition);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ImportInvoiceInformations)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.IdProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderInformations)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.IdProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.ProductCategory)
                .HasForeignKey(e => e.IdCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Promotion>()
                .Property(e => e.Coupon)
                .IsFixedLength();

            modelBuilder.Entity<Promotion>()
                .Property(e => e.PromotionPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Promotion>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.Promotion)
                .HasForeignKey(e => e.IdPromotion);

            modelBuilder.Entity<Staff>()
                .Property(e => e.Phone)
                .IsFixedLength();

            modelBuilder.Entity<Staff>()
                .HasMany(e => e.Accounts)
                .WithOptional(e => e.Staff)
                .HasForeignKey(e => e.IdStaff);

            modelBuilder.Entity<StatusOrder>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.StatusOrder)
                .HasForeignKey(e => e.IdStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Phone)
                .IsFixedLength();

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.ImportInvoices)
                .WithRequired(e => e.Supplier)
                .HasForeignKey(e => e.IdSupplier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Supplier)
                .HasForeignKey(e => e.IdSupplier)
                .WillCascadeOnDelete(false);
        }
    }
}

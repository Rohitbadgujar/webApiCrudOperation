using Microsoft.EntityFrameworkCore;

namespace webApiSoftwareOne.Models
{
    public class ApidbContext : DbContext
    {
        public ApidbContext(DbContextOptions<ApidbContext> options)
            : base(options)
        {
        }

        public DbSet<CustomerModel> CustomerList { get; set; }
        public DbSet<SalesRepModel> SalesList { get; set; }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<CusomterSale>()
        .HasKey(bc => new { bc.SalesId, bc.CustId });  
    modelBuilder.Entity<CusomterSale>()
        .HasOne(bc => bc.customer)
        .WithMany(b => b.CustomerSales)
        .HasForeignKey(bc => bc.CustId);  
    modelBuilder.Entity<CusomterSale>()
        .HasOne(bc => bc.salesrep)
        .WithMany(c => c.CustomerSales)
        .HasForeignKey(bc => bc.SalesId);
}
    }
}
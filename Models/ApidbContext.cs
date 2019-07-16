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

    }
}
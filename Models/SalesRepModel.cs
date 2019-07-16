using System.Collections.Generic;
namespace webApiSoftwareOne.Models
{
    public class SalesRepModel
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<CusomterSale> CustomerSales {get;set;}
    }
}
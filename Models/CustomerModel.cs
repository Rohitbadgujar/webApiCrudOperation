using System;
using System.Collections.Generic;
namespace webApiSoftwareOne.Models
{
    public class CustomerModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CusomterSale> CustomerSales {get;set;}
    }
}
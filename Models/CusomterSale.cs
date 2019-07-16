using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace webApiSoftwareOne.Models
{
    public class CusomterSale
    {
        [Key]
        [Column(Order=1)]
        public long CustId { get; set; }
         [ForeignKey("CustId")]
        public CustomerModel customer { get; set; }
        [Key]
        [Column(Order=2)]
         public long SalesId { get; set; }
        [ForeignKey("salesId")]
        public SalesRepModel salesrep { get; set; }
    }
}
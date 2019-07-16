using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApiSoftwareOne.Models;

namespace ApiController.Controllers
{
    [Route("api/api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApidbContext _context;

        public ApiController(ApidbContext context)
        {
            _context = context;
            if (_context.CustomerList.Count() == 0)
            {
                // Create a new CustomerList if collection is empty
                List<CusomterSale> cs = new List<CusomterSale>();
                _context.CustomerList.Add(new CustomerModel { Name = "Customer 1", CustomerSales = cs });
                _context.CustomerList.Add(new CustomerModel { Name = "Customer 2", CustomerSales = cs });
                _context.CustomerList.Add(new CustomerModel { Name = "Customer 3", CustomerSales = cs });
                _context.CustomerList.Add(new CustomerModel { Name = "Customer 4", CustomerSales = cs });
                _context.CustomerList.Add(new CustomerModel { Name = "Customer 5", CustomerSales = cs });
                _context.SaveChanges();
            }
            if (_context.SalesList.Count() == 0)
            {
                // Create a new SalesList if collection is empty
                _context.SalesList.Add(new SalesRepModel { FirstName = "Sale Rep1", LastName = "Sales1" });
                _context.SalesList.Add(new SalesRepModel { FirstName = "Sale Rep2", LastName = "Sales2" });
                _context.SalesList.Add(new SalesRepModel { FirstName = "Sale Rep3", LastName = "Sales3" });
                _context.SalesList.Add(new SalesRepModel { FirstName = "Sale Rep4", LastName = "Sale4" });
                _context.SaveChanges();
            }
        }
        [HttpGet]
        [Route("getcustomer")]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCutomerList()
        {
            return await _context.CustomerList.ToListAsync();
        }

        [HttpGet]
        [Route("getsales")]
        public async Task<ActionResult<IEnumerable<SalesRepModel>>> GetSaleRepList()
        {
            return await _context.SalesList.ToListAsync();
        }

        [HttpGet]
        [Route("getcustomer/{id:long}")]
        public async Task<ActionResult<CustomerModel>> GetCutomerList(long id)
        {
            var cust = await _context.CustomerList.FindAsync(id);

            if (cust == null)
            {
                return NotFound();
            }

            return cust;
        }
        [HttpGet]
        [Route("getsales/{id:long}")]
        public async Task<ActionResult<SalesRepModel>> GetSaleRepList(long id)
        {
            var salerep = await _context.SalesList.FindAsync(id);

            if (salerep == null)
            {
                return NotFound();
            }

            return salerep;
        }
        // POST: api/api
        [HttpPost]
        [Route("postcustomer")]
        public async Task<ActionResult<CustomerModel>> PostCustomerDetails(CustomerModel item)
        {
            _context.CustomerList.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCutomerList), new { id = item.ID }, item);
        }

        [HttpPost]
        [Route("postsales")]
        public async Task<ActionResult<SalesRepModel>> PostSaleRepDetails(SalesRepModel item)
        {
            _context.SalesList.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSaleRepList), new { id = item.ID }, item);
        }

        [HttpDelete]
        [Route("deletecustomer/{id:long}")]
        public async Task<IActionResult> DeleteCustomerById(long id)
        {
            var cust = await _context.CustomerList.FindAsync(id);

            if (cust == null)
            {
                return NotFound();
            }

            _context.CustomerList.Remove(cust);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete]
        [Route("deletesales/{id:long}")]
        public async Task<IActionResult> DeleteSalesRepById(long id)
        {
            var sales = await _context.SalesList.FindAsync(id);

            if (sales == null)
            {
                return NotFound();
            }

            _context.SalesList.Remove(sales);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut]
        [Route("linkcustomersales/{custId:long}/{salesId:long}")]
        public async Task<IActionResult> LinkCustomerSalesRep(long custId, long salesId)
        {
            CustomerModel cust = await _context.CustomerList.FindAsync(custId);

            SalesRepModel sale = await _context.SalesList.FindAsync(salesId);
            //ATTEMPT
            var customerObj = _context.CustomerList
                .Single(p => p.ID == custId);

            var salesObj = _context.SalesList
                .Single(p => p.ID == salesId);

            CusomterSale cs = new CusomterSale();
            cs.customer = cust;
            cs.salesrep = salesObj;
            //customerObj.CustomerSales = new System.Collections.Generic.ICollection<webApiSoftwareOne.Models.CusomterSale>();
            customerObj.CustomerSales.Add(new CusomterSale()
            {
                customer = cust,
                salesrep = salesObj
            });
            _context.CustomerList.Attach(customerObj);
            _context.Entry(customerObj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
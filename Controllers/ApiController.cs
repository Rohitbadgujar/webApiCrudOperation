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
                _context.CustomerList.Add(new CustomerModel { Name = "Customer 1", saleIdList = "" });
                _context.CustomerList.Add(new CustomerModel { Name = "Customer 2", saleIdList = "" });
                _context.CustomerList.Add(new CustomerModel { Name = "Customer 3", saleIdList = "" });
                _context.CustomerList.Add(new CustomerModel { Name = "Customer 4", saleIdList = "" });
                _context.CustomerList.Add(new CustomerModel { Name = "Customer 5", saleIdList = "" });
                _context.SaveChanges();
            }
            if (_context.SalesList.Count() == 0)
            {
                // Create a new SalesList if collection is empty
                _context.SalesList.Add(new SalesRepModel { FirstName = "Sale Rep1", LastName = "Sales1", custIdList = "" });
                _context.SalesList.Add(new SalesRepModel { FirstName = "Sale Rep2", LastName = "Sales2", custIdList = "" });
                _context.SalesList.Add(new SalesRepModel { FirstName = "Sale Rep3", LastName = "Sales3", custIdList = "" });
                _context.SalesList.Add(new SalesRepModel { FirstName = "Sale Rep4", LastName = "Sale4", custIdList = "" });
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
            if( cust == null){
                    return NotFound("Resource not found");
             } 
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
            if( sales == null){
                    return NotFound("Resource not found");
             } 
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
            var cust = await _context.CustomerList.FindAsync(custId);

            var sale = await _context.SalesList.FindAsync(salesId);
            if(cust == null || sale == null){
                    return NotFound("Resource not found");
             }  
            if (cust.saleIdList.Length > 0)
            {

                cust.saleIdList = cust.saleIdList.ToString() + "," + salesId.ToString();
            }
            else
            {
                cust.saleIdList = salesId.ToString();
            }
            _context.CustomerList.Attach(cust);
            _context.Entry(cust).State = EntityState.Modified;

            if (sale.custIdList.Length > 0)
            {

                sale.custIdList = sale.custIdList.Length.ToString() + "," + custId.ToString();
            }
            else
            {
                sale.custIdList = custId.ToString();
            }

            _context.SalesList.Attach(sale);
            _context.Entry(sale).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut]
        [Route("unlinkcustomersales/{custId:long}/{salesId:long}")]
        public async Task<IActionResult> UnLinkCustomerSalesRep(long custId, long salesId)
        {
            var cust = await _context.CustomerList.FindAsync(custId);
            var sale = await _context.SalesList.FindAsync(salesId);
            if(cust == null || sale == null){
                    return NotFound("Resource not found");
            }
            var custIdStr = sale.custIdList;
            var saleArr = custIdStr.Split(",");
            int numIndex1 = System.Array.IndexOf(saleArr, custId.ToString());
            saleArr = saleArr.Where((val, idx) => idx != numIndex1).ToArray();
            custIdStr = string.Join(",", saleArr);
            sale.custIdList = custIdStr;

            var salesIdStr = cust.saleIdList;
            var custArr = salesIdStr.Split(",");
            int numIndex2 = System.Array.IndexOf(custArr, salesId.ToString());
            custArr = custArr.Where((val, idx) => idx != numIndex2).ToArray();
            salesIdStr = string.Join(",", custArr);
            cust.saleIdList = salesIdStr;

            _context.CustomerList.Attach(cust);
            _context.Entry(cust).State = EntityState.Modified;
            _context.SalesList.Attach(sale);
            _context.Entry(sale).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet]
        [Route("getspecificsalesrepresentative/{custId:long}")]
        public async Task<ActionResult<List<SalesRepModel>>> GetSpecificSalesRepresentative(long custId)
        {
            var cust = await _context.CustomerList.FindAsync(custId);
            if(cust == null){
                    return NotFound("Resource not found");
            }
            List<SalesRepModel> sp = new List<SalesRepModel>();

            var ct = cust.saleIdList;
            var a = ct.Split(",");

            foreach (var item in a)
            {

                var c = await _context.SalesList.FindAsync(long.Parse(item));
                sp.Add(c);
            }


            return sp;
        }
        [HttpGet]
        [Route("getspecificcustomer/{saleId:long}")]
        public async Task<ActionResult<List<CustomerModel>>> GetSpecificCustomer(long custId)
        {
            var sale = await _context.SalesList.FindAsync(custId);
            if(sale == null){
                    return NotFound("Resource not found");
            }
            List<CustomerModel> cm = new List<CustomerModel>();

            var ct = sale.custIdList;
            var a = ct.Split(",");

            foreach (var item in a)
            {

                var c = await _context.CustomerList.FindAsync(long.Parse(item));
                cm.Add(c);
            }
            return cm;
        }
    }
}
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiInMiddleware.Models;

namespace WebApiInMiddleware.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController : ApiController
    {

        ApplicationDbContext dbContext
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationDbContext>();
            }
        }

        public IEnumerable<Company> Get()
        {
            return dbContext.Companies;
        }

        public async Task<Company> Get(int id)
        {
            Company company = dbContext.Companies.FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                throw new HttpResponseException(
                  HttpStatusCode.NotFound);
            }
            return company;
        }

        public async Task<IHttpActionResult> Post(Company company)
        {
            if (company == null)
            {
                return BadRequest("Argument null");
            }

            bool companyExists = dbContext.Companies.Any(c => c.Id == company.Id);
            if (companyExists)
            {
                return BadRequest("Exists");
            }

            dbContext.Companies.Add(company);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        public async Task<IHttpActionResult> Put(Company company)
        {
            if (company == null)
            {
                return BadRequest("Argument null");
            }

            Company existing = dbContext.Companies.FirstOrDefault(c => c.Id == company.Id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = company.Name;
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            Company company = dbContext.Companies.FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            dbContext.Companies.Remove(company);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}

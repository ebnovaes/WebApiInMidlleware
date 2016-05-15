using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiInMiddleware.Models;

namespace WebApiInMiddleware.Controllers
{
    public class CompaniesController : ApiController
    {

        ApplicationDbContext _Db = new ApplicationDbContext();

        public IEnumerable<Company> Get()
        {
            return _Db.Companies;
        }

        public async Task<Company> Get(int id)
        {
            Company company = _Db.Companies.FirstOrDefault(c => c.Id == id);
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

            bool companyExists = _Db.Companies.Any(c => c.Id == company.Id);
            if (companyExists)
            {
                return BadRequest("Exists");
            }

            _Db.Companies.Add(company);
            await _Db.SaveChangesAsync();
            return Ok();
        }

        public async Task<IHttpActionResult> Put(Company company)
        {
            if (company == null)
            {
                return BadRequest("Argument null");
            }

            Company existing = _Db.Companies.FirstOrDefault(c => c.Id == company.Id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = company.Name;
            await _Db.SaveChangesAsync();
            return Ok();
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            Company company = _Db.Companies.FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            _Db.Companies.Remove(company);
            await _Db.SaveChangesAsync();
            return Ok();
        }
    }
}

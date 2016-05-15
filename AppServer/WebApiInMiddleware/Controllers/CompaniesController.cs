﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebApiInMiddleware.Models;

namespace WebApiInMiddleware.Controllers
{
    public class CompaniesController : ApiController
    {

        private static List<Company> _Db = new List<Company>()
            {
                new Company { Id = 1, Name = "Microsoft" },
                new Company { Id = 2, Name = "Google" },
                new Company { Id = 3, Name = "Apple" }
            };

        public IEnumerable<Company> Get()
        {
            return _Db;
        }

        public Company Get(int id)
        {
            Company company = _Db.FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                throw new HttpResponseException(
                  HttpStatusCode.NotFound);
            }
            return company;
        }

        public IHttpActionResult Post(Company company)
        {
            if (company == null)
            {
                return BadRequest("Argument null");
            }

            bool companyExists = _Db.Any(c => c.Id == company.Id);
            if (companyExists)
            {
                return BadRequest("Exists");
            }

            _Db.Add(company);
            return Ok();
        }

        public IHttpActionResult Put(Company company)
        {
            if (company == null)
            {
                return BadRequest("Argument null");
            }

            Company existing = _Db.FirstOrDefault(c => c.Id == company.Id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = company.Name;
            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            Company company = _Db.FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            _Db.Remove(company);
            return Ok();
        }
    }
}
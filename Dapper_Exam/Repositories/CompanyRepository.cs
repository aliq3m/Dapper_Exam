using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper_Exam.Models;

namespace Dapper_Exam.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private DapperDBContext _context;

        public CompanyRepository(DapperDBContext context)
        {
            _context = context;
        }

        public Company Find(int id)
        {
            return _context.Companies.Find(id);
        }

        public List<Company> GetAll()
        {
            return _context.Companies.ToList();
        }

        public Company Add(Company company)
        {
            _context.Companies.Add(company);
            _context.SaveChanges();
            return company;
        }

        public Company Update(Company company)
        {
            _context.Companies.Update(company);
            _context.SaveChanges();
            return company;
        }

        public void Remove(int id)
        {
            var company = Find(id);
            _context.Companies.Remove(company);
            _context.SaveChanges();
        }
    }
}

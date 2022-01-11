using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper_Exam.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Dapper_Exam.Repositories
{
    public class CompanyRepositoryDapper:ICompanyRepository
    {
        private IDbConnection _db;

        public CompanyRepositoryDapper(IConfiguration configuration)
        {
            this._db = new SqlConnection(configuration.GetConnectionString("DapperDb"));
        }
        public Company Find(int id)
        {
            string query = "SELECT * FROM companies WHERE CompanyId = @CompanyId";
            return _db.Query<Company>(query, new {@CompanyId = id}).Single();
        }

        public List<Company> GetAll()
        {
            string query = "SELECT * FROM companies";
          return  _db.Query<Company>(query).ToList();
        }

        public Company Add(Company company)
        {
            string query =
                "INSERT INTO companies (CompanyName,Address,City,State,PostalCode) VALUES (@CompanyName,@Address,@City,@State,@PostalCode);" +
                "SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = _db.Query<int>(query, new
            {
               company.CompanyName,
               company.Address,
               company.City,
               company.State,
               company.PostalCode

            }).Single();
            company.CompanyId = id;
            return company;
        }

        public Company Update(Company company)
        {
            string query = "UPDATE companies SET CompanyName = @CompanyName , Address = @Address , City = @City ," +
                           "State = @State , PostalCode = @PostalCode WHERE CompanyId = @CompanyId";
            _db.Execute(query, company);
            return company;
        }

        public void Remove(int id)
        {
            string query = "DELETE FROM companies WHERE CompanyID = @id ";
            _db.Execute(query, new {id});
        }
    }
}

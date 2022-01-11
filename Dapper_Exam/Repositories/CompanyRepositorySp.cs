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
    public class CompanyRepositorySp:ICompanyRepository
    {
        private IDbConnection _db;

        public CompanyRepositorySp(IConfiguration configuration)
        {
            this._db = new SqlConnection(configuration.GetConnectionString("DapperDb"));
        }
        public Company Find(int id)
        {
            
            return _db.Query<Company>("usp_GetCompany", new { CompanyId = id},commandType:CommandType.StoredProcedure).Single();
        }

        public List<Company> GetAll()
        {
           
          return  _db.Query<Company>("usp_GetAllCompany",commandType:CommandType.StoredProcedure).ToList();
        }

        public Company Add(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId",0,DbType.Int32,ParameterDirection.Output);
            parameters.Add("@CompanyName",company.CompanyName);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            _db.Execute("usp_AddCompany", parameters, commandType: CommandType.StoredProcedure);
            company.CompanyId = parameters.Get<int>("CompanyId");
            return company;
        }

        public Company Update(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", company.CompanyId, DbType.Int32);
            parameters.Add("@CompanyName", company.CompanyName);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            _db.Execute("usp_UpdateCompany", parameters, commandType: CommandType.StoredProcedure);
           
            return company;
        }

        public void Remove(int id)
        {
            
            _db.Execute("usp_DeleteCompany", new {CompanyId = id},commandType:CommandType.StoredProcedure);
        }
    }
}

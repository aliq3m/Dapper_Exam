using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Dapper_Exam.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Dapper_Exam.Repositories
{
    public class BonusRepository:IBonusRepository
    {
        private IDbConnection db;

        public BonusRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DapperDb"));
        }
        public List<Employee> GetAllEmployeeWhitCompany()
        {
            var query = @"SELECT E.* , C.* FROM Employees AS E INNER JOIN Companies As C ON E.CompanyId = C.CompanyId";
           
            var employees = db.Query<Employee, Company, Employee>(query, (e, c) =>
            {
                e.Company =c;
                return e;
            },splitOn:"CompanyId");
            return employees.ToList();
        }

        public Company GetCompanyWithEmployee(int id)
        {
            var param = new
            {
                id = id
            };
            var query = @"SELECT * FROM Companies WHERE CompanyId = @ID ";
            query += "SELECT * FROM Employees WHERE CompanyId = @ID";
            Company company; 
            using (var lists = db.QueryMultiple(query,param))
            {
                company = lists.Read<Company>().ToList().FirstOrDefault();
                company.Employees = lists.Read<Employee>().ToList();    
            }

            return company;
        }

        public List<Company> GetAllCompanyWithEmployees()
        {
            var query = @"SELECT C.* , E.* FROM Employees AS E INNER JOIN Companies AS C ON C.CompanyId = E.CompanyId";
            var companyDic = new Dictionary<int, Company>();
            var companies = db.Query<Company, Employee, Company>(query, (c, e) =>
            {
                if (!companyDic.TryGetValue(c.CompanyId,out  var currentCompany))
                {
                    currentCompany = c;
                    companyDic.Add(currentCompany.CompanyId,currentCompany);
                }
                currentCompany.Employees.Add(e);
                return currentCompany;
            },splitOn:"EmployeeId");
            return companies.Distinct().ToList();
        }

        public void AddTestRecords(Company company)
        {
            var query =
              "INSERT INTO Companies (CompanyName,Address,City,State,PostalCode) VALUES (@CompanyName,@Address,@City,@State,@PostalCode);"
                + "SELECT CAST(SCOPE_IDENTITY() AS int);";
            var id = db.Query<int>(query,company).Single();
            company.CompanyId = id;
            //foreach (var em in company.Employees)
            //{
            //    em.CompanyId = company.CompanyId;
            //    var queryEm =
            //       "INSERT INTO Employees (Name,Email,Phone,Title,CompanyId) VALUES (@Name,@Email,@Phone,@Title,@CompanyId);"
            //        + "SELECT CAST(SCOPE_IDENTITY() AS int);";
            //    var idEm = db.Query<int>(queryEm,em).Single();
            //}

            var queryEm =
                       "INSERT INTO Employees (Name,Email,Phone,Title,CompanyId) VALUES (@Name,@Email,@Phone,@Title,@CompanyId);"
                        + "SELECT CAST(SCOPE_IDENTITY() AS int);";
           var employees = company.Employees.Select(c =>
            {
                c.CompanyId = id;
                return c;
            }).ToList();
           db.Execute(queryEm, employees);

        }

        public void AddTestRecordsWithTransAction(Company company)
        {
            try
            {
                using (var trans = new TransactionScope())
                {
                    var query =
                        "INSERT INTO Companies (CompanyName,Address,City,State,PostalCode) VALUES (@CompanyName,@Address,@City,@State,@PostalCode);"
                        + "SELECT CAST(SCOPE_IDENTITY() AS int);";
                    var id = db.Query<int>(query, company).Single();
                    company.CompanyId = id;
                    var queryEm =
                        "INSERT INTO Employee (Name,Email,Phone,Title,CompanyId) VALUES (@Name,@Email,@Phone,@Title,@CompanyId);"
                        + "SELECT CAST(SCOPE_IDENTITY() AS int);";
                    var employees = company.Employees.Select(c =>
                    {
                        c.CompanyId = id;
                        return c;
                    }).ToList();
                    db.Execute(queryEm, employees);
                    trans.Complete();
                }
            }
            catch (Exception e)
            {
               
            }
        }

        public void DeleteCompany(Company company)
        {
            var queryEm = "DELETE FROM Employees Where CompanyId = @CompanyId ; DELETE FROM Companies WHERE CompanyId = @CompanyId";
           

           
            db.Query(queryEm, new {@CompanyId = company.CompanyId});
        }
    }
}

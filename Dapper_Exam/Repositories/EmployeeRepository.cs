using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper_Exam.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Configuration;

namespace Dapper_Exam.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IDbConnection db;

        public EmployeeRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DapperDb"));
        }
        public Employee Find(int id)
        {
            string query = "Select * From Employees Where EmployeeId=@EmployeeId";
            return db.Query<Employee>(query, new { @EmployeeId = id}).Single();
        }

        public List<Employee> GetAll()
        {
            string query = "SELECT * FROM Employees";
            return db.Query<Employee>(query).ToList();
        }

        public Employee Add(Employee employee)
        {
            string query =
                "Insert Into Employees (Name,Email,Phone,Title,CompanyId) Values (@Name,@Email,@Phone,@Title,@CompanyId);"
                + "Select CAST(SCOPE_IDENTITY() as int)";
            var id = db.Query<int>(query, new
            {
                employee.Name,
                employee.Email,
                employee.Phone,
                employee.Title,
                employee.CompanyId
            }).Single();

            employee.EmployeeId = id;
            return employee;
        }

        public Employee Update(Employee employee)
        {
            string query = "UPDATE Employees SET Name = @Name, Email = @Email, Phone = @Phone," +
                           "Title= @Title, CompanyId = @CompanyId Where EmployeeId = @EmployeeId";
            db.Execute(query, employee);
            return employee;
        }

        public void Remove(int id)
        {
            string query = "DELETE FROM Employees WHERE EmployeeId = @Id";
            db.Execute(query, new {id});
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper_Exam.Models;

namespace Dapper_Exam.Repositories
{
   public interface IBonusRepository
   {
       List<Employee> GetAllEmployeeWhitCompany();
       Company GetCompanyWithEmployee(int id);
       List<Company> GetAllCompanyWithEmployees();
       void AddTestRecords(Company company);
       void AddTestRecordsWithTransAction(Company company);
       void DeleteCompany(Company company);

   }
}

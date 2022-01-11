using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper_Exam.Models;

namespace Dapper_Exam.Repositories
{
   public interface ICompanyRepository
   {
       Company Find(int id);
       List<Company> GetAll();
       Company Add(Company company);
       Company Update(Company company);
       void Remove(int id);
   }

  

}

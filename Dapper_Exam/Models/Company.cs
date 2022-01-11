using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dapper_Exam.Models
{
    public class Company
    {
        public Company()
        {
            Employees = new List<Employee>();
        }
        [Key]
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public List<Employee> Employees { get; set; }
    }
}

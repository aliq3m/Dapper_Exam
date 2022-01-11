using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dapper_Exam.Models
{
    public class DapperDBContext:DbContext
    {
        public DapperDBContext(DbContextOptions<DapperDBContext> options):base(options)
        {
            
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Ignore(t => t.Employees);

            modelBuilder.Entity<Employee>()
                .HasOne(c=>c.Company)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CompanyId);
            base.OnModelCreating(modelBuilder);
        }
    }
}

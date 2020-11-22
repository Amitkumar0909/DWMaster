using CodeSample.Entities; 
using System.Data.Entity;

namespace CodeSample.Logics.DAL
{
    public class CodeSampleContext : DbContext
    {
        public CodeSampleContext()
            : base("name=CodeSampleContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
         
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CountryState> CountryStates { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Department> Departments { get; set; }


    }
}

using Domain.Model;
using System.Data.Entity;

namespace Domain.Logic
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DBConnection")
        { }
        public DataContext(string connectionString) : base(connectionString)
        { }

        public DbSet<Passport> Passports { get; set; }
    }
}

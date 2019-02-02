using System.Data.Entity;
using Domain.Model;

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
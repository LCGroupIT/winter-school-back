using Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Logic
{
    public class DataContext : DbContext
    {
        public DataContext(string connectionString) : base(connectionString)
        { }

        public DbSet<Passport> Passports { get; set; }
    }
}

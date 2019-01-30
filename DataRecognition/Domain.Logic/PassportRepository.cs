using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Model;

namespace Domain.Logic
{
    public class PassportRepository : IRepository<Passport>
    {
        private DataContext db;

        public PassportRepository(string connectionString)
        {
            this.db = new DataContext(connectionString);
        }

        public Task CreateAsync(Passport passport)
        {
            return Task.FromResult(db.Passports.Add(passport));
        }

        public Task SaveAsync()
        {
            return Task.FromResult(db.SaveChanges());
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

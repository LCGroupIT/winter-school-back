using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepository<in T> where T : class
    {
        Task CreateAsync(T item); 
    }
}

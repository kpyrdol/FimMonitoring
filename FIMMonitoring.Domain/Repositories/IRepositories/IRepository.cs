using System.Data.Entity;
using System.Linq;

namespace FIMMonitoring.Domain.Repositories.IRepositories
{
    public interface IRepository<T>
    {
        void SaveChanges();
        T GetById(int id);
        void Insert(T item);
        void Update(T item);
        void Delete(T item);
        IQueryable<T> All();
        DbContextTransaction BeginTransaction();
    }
}

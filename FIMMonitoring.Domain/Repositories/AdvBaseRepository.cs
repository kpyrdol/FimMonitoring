using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using FIMMonitoring.Domain.Repositories.IRepositories;

namespace FIMMonitoring.Domain.Repositories
{
    public class AdvBaseRepository<T> : BaseRepository, IRepository<T> where T : EntityBase
    {
        public delegate void ReturnP2SignatureEventHandler(string content, SignatureEventArgs args);


        public void SaveChanges()
        {
            try
            {
                FimContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw;
            }
        }
        #region Overrides

        public DbContextTransaction BeginTransaction()
        {
            return FimContext.Database.BeginTransaction();
        }

        public T GetById(int id)
        {
            return FimContext.Set<T>().FirstOrDefault(p => p.Id == id);
        }

        public void Insert(T item)
        {
            FimContext.Set<T>().Add(item);
            FimContext.SaveChanges();
        }

        public void Update(T item)
        {
            FimContext.Set<T>().Attach(item);
            FimContext.Entry(item).State = EntityState.Modified;
            FimContext.SaveChanges();
        }

        public void Delete(T item)
        {
            FimContext.Set<T>().Remove(item);
            FimContext.SaveChanges();
        }

        public IQueryable<T> All()
        {
            return FimContext.Set<T>().AsQueryable().OrderBy(i => i.Id);
        }
        #endregion Overrides

        public string SqlParam(DateTime? param)
        {
            return param != null ? param.Value.ToShortDateString() : "null";
        }

        public string SqlParam(string param)
        {
            return string.IsNullOrWhiteSpace(param) ? "null" : string.Format("'{0}'", param);
        }
    }

    public class SignatureEventArgs : EventArgs
    {
        public string Signature { get; set; }
    }
}

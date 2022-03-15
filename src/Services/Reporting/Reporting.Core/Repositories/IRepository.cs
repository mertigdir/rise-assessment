using Reporting.MongoDb.Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.MongoDb.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class, IAggregateRoot
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
        void Update(TEntity obj);
        void Remove(Guid id);
    }
}

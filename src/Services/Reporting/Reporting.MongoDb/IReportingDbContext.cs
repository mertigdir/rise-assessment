using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.MongoDb
{
    public interface IReportingDbContext : IDisposable
    {
        IClientSessionHandle Session { get; }
        void AddCommand(Func<Task> func, object entity);
        Task CommitAsync();
        Task SaveChangesAsync();
        void CreateTransaction();
        IMongoCollection<T> GetCollection<T>(string name);


    }
}

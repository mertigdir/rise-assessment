using System;
using System.Threading.Tasks;
using Reporting.MongoDb;

namespace Reporting.MongoDb.Shared
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        Task SaveChangesAsync();
    }
}

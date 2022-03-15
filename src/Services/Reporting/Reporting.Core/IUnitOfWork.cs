using System;
using System.Threading.Tasks;
using Reporting.MongoDb;

namespace Reporting.Core
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
    }
}

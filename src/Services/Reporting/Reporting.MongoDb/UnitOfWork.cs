using System;
using System.Threading.Tasks;
using Reporting.Core;

namespace Reporting.MongoDb
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IReportingDbContext _context;

        public UnitOfWork(IReportingDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _context.CommitAsync();
        }


        public void Dispose()
        {
            _context.Dispose();
        }

    }
}

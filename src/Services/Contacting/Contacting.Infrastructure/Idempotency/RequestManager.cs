using Contacting.Domain.Exceptions;
using Contacting.Infrastructure.Idempotency;
using System;
using System.Threading.Tasks;

namespace Contacting.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly ContactingContext _context;

        public RequestManager(ContactingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<bool> ExistAsync(Guid id)
        {
            var request = await _context.
                FindAsync<ClientRequest>(id);

            return request != null;
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        { 
            var exists = await ExistAsync(id);

            var request = exists ? 
                throw new Exception("RequestIdAlreadyExists") : 
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };

            _context.Add(request);

            await _context.SaveChangesAsync();
        }
    }
}

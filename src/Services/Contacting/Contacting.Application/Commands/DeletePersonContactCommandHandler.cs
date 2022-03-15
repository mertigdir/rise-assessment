using Contacting.Application.IntegrationEvents;
using Contacting.Application.IntegrationEvents.Events;
using Contacting.Domain.Auctions;
using Contacting.Domain.Persons;
using Contacting.Domain.SeedWork;
using Contacting.Infrastructure.Idempotency;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility.Session;

namespace Contacting.Application.Commands
{
    public class DeletePersonContactCommandHandler : IRequestHandler<DeletePersonContactCommand, bool>
    {
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly ILogger<DeletePersonContactCommandHandler> _logger;

        public DeletePersonContactCommandHandler(
            IRepository<Person, Guid> personRepository,
            ILogger<DeletePersonContactCommandHandler> logger
           )
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }
        public async Task<bool> Handle(DeletePersonContactCommand request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetAsync(request.PersonId);
            person.DeleteContact(request.PersonContactId);

            return true;
        }
    }
}

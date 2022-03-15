using Contacting.Application.IntegrationEvents;
using Contacting.Application.IntegrationEvents.Events;
using Contacting.Domain.Auctions;
using Contacting.Domain.Persons;
using Contacting.Domain.SeedWork;
using Contacting.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility.Session;

namespace Contacting.Application.Commands
{
    public class CreatePersonContactCommandHandler : IRequestHandler<CreatePersonContactCommand, bool>
    {
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly ILogger<CreatePersonContactCommandHandler> _logger;

        public CreatePersonContactCommandHandler(
            IRepository<Person, Guid> personRepository,
            ILogger<CreatePersonContactCommandHandler> logger
           )
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }
        public async Task<bool> Handle(CreatePersonContactCommand request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetAsync(request.PersonId);
            person.AddContact(new PersonContact(Guid.NewGuid(), person.Id, request.ContactType, request.Content));

            await _personRepository.UpdateAsync(person);
            return true;
        }
    }

}

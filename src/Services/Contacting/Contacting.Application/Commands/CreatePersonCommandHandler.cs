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
    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, bool>
    {
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly ILogger<CreatePersonCommandHandler> _logger;

        public CreatePersonCommandHandler(
            IRepository<Person, Guid> personRepository,
            ILogger<CreatePersonCommandHandler> logger
           )
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }
        public async Task<bool> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = new Person(Guid.NewGuid(), request.Name, request.Surname, request.Company);

            await _personRepository.InsertAsync(person);

            return true;
        }
    }

}

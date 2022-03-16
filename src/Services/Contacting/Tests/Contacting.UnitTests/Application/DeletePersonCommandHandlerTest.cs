using Contacting.Application.Commands;
using Contacting.Application.IntegrationEvents;
using Contacting.Application.IntegrationEvents.Events;
using Contacting.Domain.Persons;
using Contacting.Domain.SeedWork;
using Contacting.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility.Session;
using Xunit;

namespace Contacting.UnitTests.Application
{
    public class DeletePersonCommandHandlerTest
    {
        private readonly Mock<IRepository<Person, Guid>> _personRepository;
        private readonly Mock<ILogger<DeletePersonCommandHandler>> _logger;

        public DeletePersonCommandHandlerTest()
        {
            _personRepository = new Mock<IRepository<Person, Guid>>();
            _logger = new Mock<ILogger<DeletePersonCommandHandler>>();
        }

        [Fact]
        public async Task Shoud_always_successful()
        {
            //Arrange
            var command = new DeletePersonCommand();
            _personRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult(default(Person)));
            _personRepository.Setup(x => x.DeleteAsync(It.IsAny<Person>())).Returns(Task.CompletedTask);

            //Act
            var handler = new DeletePersonCommandHandler(_personRepository.Object, _logger.Object);
            var cltToken = new System.Threading.CancellationToken();

            var res = await handler.Handle(command, cltToken);


            //Asset
            Assert.True(res);
        }
    }
}

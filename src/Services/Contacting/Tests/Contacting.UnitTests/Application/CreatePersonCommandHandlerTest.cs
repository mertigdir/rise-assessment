using Contacting.Application.Commands;
using Contacting.Domain.Persons;
using Contacting.Domain.SeedWork;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Contacting.UnitTests.Application
{
    public class CreatePersonCommandHandlerTest
    {
        private readonly Mock<IRepository<Person, Guid>> _personRepository;
        private readonly Mock<ILogger<CreatePersonCommandHandler>> _logger;

        public CreatePersonCommandHandlerTest()
        {
            _personRepository = new Mock<IRepository<Person, Guid>>();
            _logger = new Mock<ILogger<CreatePersonCommandHandler>>();
        }

        [Fact]
        public async Task Should_always_successful()
        {
            //Arrange
            var command = new CreatePersonCommand("", "", "");

            _personRepository.Setup(x => x.InsertAsync(It.IsAny<Person>())).Returns(Task.FromResult(default(Person)));

            //Act
            var handler = new CreatePersonCommandHandler(_personRepository.Object, _logger.Object);
            var cltToken = new System.Threading.CancellationToken();

            var res = await handler.Handle(command, cltToken);


            //Asset
            Assert.True(res);
        }
    }
}

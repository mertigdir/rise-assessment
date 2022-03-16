using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Contacting.API.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Contacting.API;
using Contacting.Infrastructure;
using Contacting.Application.IntegrationEvents;
using Contacting.Application.Queries;
using Contacting.Application.Commands;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Utility.Extensions;
using Contacting.Application.IntegrationEvents.Events;
using DotNetCore.CAP;
using Autofac;
using Contacting.Dto.Persons.Inputs;
using Contacting.Dto.Persons;

namespace Services.Contacting.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPersonQueries _personQueries;
        private readonly ILogger<PersonController> _logger;

        public PersonController(
            IMediator mediator,
            ILogger<PersonController> logger,
            IPersonQueries personQueries
            )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _personQueries = personQueries;

        }

        [Route("list")]
        [HttpGet]
        [ProducesResponseType(typeof(List<PersonDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPersonsAsync(GetPersonListInput input)
        {
            var persons = await _personQueries.GetPersonsAsync(skip: input.SkipCount, take: input.MaxResultCount);
            return Ok(persons);
        }

        [HttpGet("{personId}")]
        [ProducesResponseType(typeof(PersonDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPersonWithContactsAsync(Guid personId)
        {
            var person = await _personQueries.GetPersonWithContactsAsync(personId);
            return Ok(person);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePersonAsync([FromBody] CreatePersonInput input)
        {
            bool commandResult = false;

            var command = new CreatePersonCommand(input.Name, input.Surname, input.Company);

            _logger.LogInformation(
                "----- Sending command: {CommandName}",
                command.GetGenericTypeName(),
                command);



            commandResult = await _mediator.Send(command);

            return Ok(commandResult);
        }

        [HttpDelete("{personId}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeletePersonAsync(Guid personId)
        {
            bool commandResult = false;

            var command = new DeletePersonCommand(personId);

            _logger.LogInformation(
                "----- Sending command: {CommandName}",
                command.GetGenericTypeName(),
                command);

            commandResult = await _mediator.Send(command);

            return Ok(commandResult);
        }


        [HttpDelete("{personId}/person-contact/{personContactId}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeletePersonContactAsync(Guid personId, Guid personContactId)
        {
            bool commandResult = false;

            var command = new DeletePersonContactCommand(personId, personContactId);

            _logger.LogInformation(
                "----- Sending command: {CommandName}",
                command.GetGenericTypeName(),
                command);

            commandResult = await _mediator.Send(command);

            return Ok(commandResult);
        }

        [HttpPut("{personId}/person-contact")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatPersonContactAsync(Guid personId, [FromBody] CreatePersonContactInput input)
        {
            bool commandResult = false;

            var command = new CreatePersonContactCommand(personId, input.ContactType, input.Content);

            _logger.LogInformation(
                "----- Sending command: {CommandName}",
                command.GetGenericTypeName(),
                command);

            commandResult = await _mediator.Send(command);

            return Ok(commandResult);
        }
    }
}

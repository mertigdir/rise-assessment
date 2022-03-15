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


        [Route("getPersons")]
        [HttpGet]
        [ProducesResponseType(typeof(List<PersonDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPersonsAsync(GetPersonListInput input)
        {
            var persons = await _personQueries.GetPersonsAsync(skip: input.SkipCount, take: input.MaxResultCount);
            return Ok(persons);
        }


        [Route("getPersonWithContacts")]
        [HttpGet]
        [ProducesResponseType(typeof(PersonDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPersonWithContacts(Guid personId)
        {
            var person = await _personQueries.GetPersonWithContactsAsync(personId);
            return Ok(person);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePersonAsync([FromBody] CreatePersonCommand command)
        {
            bool commandResult = false;

            _logger.LogInformation(
                "----- Sending command: {CommandName}",
                command.GetGenericTypeName(),
                command);

            commandResult = await _mediator.Send(command);

            return Ok(commandResult);
        }

    }
}

using Contacting.Application.Commands;
using Contacting.Application.IntegrationEvents.Events;
using Contacting.Domain.Shared;
using DotNetCore.CAP;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utility.Extensions;

namespace Contacting.Application.IntegrationEvents.EventHandling
{
    public class TestIntegrationEventHandler : ICapSubscribe
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TestIntegrationEvent> _logger;
        private AppSettings _appSettings;

        public TestIntegrationEventHandler(
            IMediator mediator,
            ILogger<TestIntegrationEvent> logger,
            AppSettings appSettings)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _appSettings = appSettings;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// When a new user is added to the system, that user is added to the Contacting.API database.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        [CapSubscribe(nameof(TestIntegrationEvent))]
        public async Task Handle(TestIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{_appSettings.ServiceName}"))
            {
                _logger.LogInformation("----- Handling integration event: {AppName} - ({@IntegrationEvent})", _appSettings.ServiceName, @event);
            }
        }
    }
}

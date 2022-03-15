using Contacting.Application;
using Contacting.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using Contacting.Application.IntegrationEvents;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore.Storage;
using Utility.Extensions;

namespace Contacting.Application.IntegrationEvents
{
    public class ContactingIntegrationEventService : IContactingIntegrationEventService
    {
        private readonly ICapPublisher _eventBus;
        private readonly ILogger<ContactingIntegrationEventService> _logger;

        public ContactingIntegrationEventService(
            ICapPublisher eventBus,
            ILogger<ContactingIntegrationEventService> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAndSaveEventAsync(object evt)
        {
            _logger.LogInformation("----- Enqueuing integration event to repository ({@IntegrationEvent})", evt);
            await _eventBus.PublishAsync(evt.GetGenericTypeName(), evt);
        }
    }
}

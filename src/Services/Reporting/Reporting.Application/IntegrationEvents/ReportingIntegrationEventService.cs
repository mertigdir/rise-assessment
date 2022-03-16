using Reporting.Application;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Reporting.Core.Shared.Extensions;

namespace Reporting.Application.IntegrationEvents
{
    public class ReportingIntegrationEventService : IReportingIntegrationEventService
    {
        private readonly ICapPublisher _eventBus;
        private readonly ILogger<ReportingIntegrationEventService> _logger;

        public ReportingIntegrationEventService(
            ICapPublisher eventBus,
            ILogger<ReportingIntegrationEventService> logger)
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

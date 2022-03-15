using System;
using System.Threading.Tasks;

namespace Reporting.Application.IntegrationEvents
{
    public interface IReportingIntegrationEventService
    {
        Task AddAndSaveEventAsync(object evt);
    }
}

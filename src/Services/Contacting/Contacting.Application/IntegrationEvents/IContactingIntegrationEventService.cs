using System;
using System.Threading.Tasks;

namespace Contacting.Application.IntegrationEvents
{
    public interface IContactingIntegrationEventService
    {
        Task AddAndSaveEventAsync(object evt);
    }
}

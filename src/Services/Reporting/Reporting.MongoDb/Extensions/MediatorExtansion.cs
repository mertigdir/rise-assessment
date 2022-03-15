using Reporting.MongoDb.Shared.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.MongoDb.Extensions
{
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, List<object> entities)
        {
            var domainEntities = entities
                .Where(x => x.GetType().BaseType == typeof(DomainEvent))
                .Select(x => (DomainEvent)x)
                .Where(x => x.DomainEvents != null && x.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}

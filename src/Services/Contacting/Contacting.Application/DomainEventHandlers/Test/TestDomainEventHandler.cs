using Contacting.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Contacting.Application.DomainEventHandlers.Test
{
    public class TestDomainEventHandler : INotificationHandler<TestDomainEvent>
    {
        public async Task Handle(TestDomainEvent notification, CancellationToken cancellationToken)
        {
             await Task.Delay(1);
        }
    }
}

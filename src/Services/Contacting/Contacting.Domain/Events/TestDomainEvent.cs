using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contacting.Domain.Events
{
    public class TestDomainEvent : INotification
    {
        public string TestField { get; set; }
    }
}

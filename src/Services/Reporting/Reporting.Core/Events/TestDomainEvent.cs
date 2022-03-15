using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reporting.Core.Events
{
    public class TestDomainEvent : INotification
    {
        public string TestField { get; set; }
    }
}

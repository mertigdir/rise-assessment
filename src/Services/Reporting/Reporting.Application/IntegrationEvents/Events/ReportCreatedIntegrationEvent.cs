using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Application.IntegrationEvents.Events
{
    public class ReportCreatedIntegrationEvent
    {
        public ReportCreatedIntegrationEvent(string reportId)
        {
            ReportId = reportId;
        }

        public string ReportId { get; set; }
    }
}

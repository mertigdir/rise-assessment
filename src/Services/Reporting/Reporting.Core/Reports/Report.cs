using Reporting.Core.Shared.Reports;
using Reporting.MongoDb.Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Core.Models.Reports
{
    public class Report : Entity, IAggregateRoot
    {
        public DateTime RequestDate { get; set; }
        public ReportState State { get; set; }
    }
}

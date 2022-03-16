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

        public Report(DateTime requestDate)
        {
            RequestDate = requestDate;
            State = ReportState.InProcess;
        }

        public DateTime RequestDate { get; protected set; }
        public ReportState State { get; protected set; }
        public string FileUrl { get; protected set; }



        public void ReportPrepared(string fileUrl)
        {
            State = ReportState.Completed;
            FileUrl = fileUrl;
        }

    }
}

using Reporting.Core.Shared.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Application.Shared.Reports.Dto
{
    public class ReportDto
    {
        public string Id { get; set; }
        public ReportState State { get; set; }
        public string FileUrl { get; set; }

        public DateTime RequestDate { get; set; }
    }
}

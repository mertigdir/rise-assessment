using Contacting.Dto.Persons.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Dependency;

namespace Reporting.Application.Reports.ReportCreators
{
    public interface IExcelReportCreater : ITransientDependency
    {
        string CreateLocationReport(string fileName, List<LocationReport> reportData);
    }
}

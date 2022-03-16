using MongoDB.Bson;
using Reporting.Application.Shared.Reports.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Dependency;

namespace Reporting.Application.Reports
{
    public interface IReportAppService : ITransientDependency
    {
        Task CreateReportRequestAsync();
        Task<string> CreateReportAsync(ObjectId id);
        Task<List<ReportDto>> GetReports();
    }
}

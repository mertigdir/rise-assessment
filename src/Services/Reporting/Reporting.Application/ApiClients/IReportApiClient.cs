using Contacting.Dto.Persons;
using Contacting.Dto.Persons.Reports;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Application.Services
{
    public interface IReportApiClient
    {
        [Get("/location/list")]
        Task<List<LocationReport>> GetLocationList();
 }
}

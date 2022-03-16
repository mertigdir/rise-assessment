using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Reporting.MongoDb.Repositories;
using Reporting.API.Infrastructure;
using Reporting.MongoDb.Shared;
using Reporting.Core.Models.Reports;
using Reporting.Core.Shared.Reports;
using Reporting.Application.Reports;
using Reporting.Application.Shared.Reports.Dto;

namespace Reporting.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IReportAppService _reportAppService;

        public ReportController(IReportAppService reportAppService, ILogger<ReportController> logger)
        {
            _reportAppService = reportAppService;
            _logger = logger;
        }


        [HttpPut]
        public async Task<IActionResult> CreateReportAsync()
        {
            await _reportAppService.CreateReportRequestAsync();
            return Ok();
        }


        [Route("list")]
        [HttpGet]
        [ProducesResponseType(typeof(List<ReportDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetLocationList()
        {
            var reports = await _reportAppService.GetReports();
            return Ok(reports);
        }

    }
}

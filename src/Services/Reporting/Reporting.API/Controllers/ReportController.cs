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

namespace Reporting.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IRepository<Report> _reportRepository;

        public ReportController(ILogger<ReportController> logger, IRepository<Report> reportRepository)
        {
            _logger = logger;
            _reportRepository = reportRepository;
        }


        [Route("deneme")]
        [HttpPost]
        public async Task<IActionResult> deneme()
        {
            _reportRepository.Add(new Report()
            {
                RequestDate = DateTime.Now,
                State = ReportState.InProcess,
            });

            return Ok();
        }
    }
}

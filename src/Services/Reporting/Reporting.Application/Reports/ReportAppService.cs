using AutoMapper;
using ClosedXML.Excel;
using Contacting.Dto.Persons.Reports;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Reporting.Application.IntegrationEvents;
using Reporting.Application.IntegrationEvents.Events;
using Reporting.Application.Reports.ReportCreators;
using Reporting.Application.Services;
using Reporting.Application.Shared.Reports.Dto;
using Reporting.Core.Models.Reports;
using Reporting.Core.Shared;
using Reporting.Core.Shared.Reports;
using Reporting.MongoDb.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Application.Reports
{
    public class ReportAppService : IReportAppService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Report> _reportRepository;
        private readonly IReportingIntegrationEventService _reportingIntegrationEventService;
        private readonly IReportApiClient _reportApiClient;
        private readonly IMapper _mapper;
        private readonly IExcelReportCreater _excelReportCreater;
        private readonly IHostingEnvironment _environment;
        private readonly AppSettings _appSettings;


        public ReportAppService(
            IUnitOfWork uow,
            IRepository<Report> reportRepository,
            IReportingIntegrationEventService reportingIntegrationEventService,
            IReportApiClient reportApiClient,
            IMapper mapper,
            IExcelReportCreater excelReportCreater,
            IHostingEnvironment environment,
            IOptions<AppSettings> appSettings)
        {
            _uow = uow;
            _reportRepository = reportRepository;
            _reportingIntegrationEventService = reportingIntegrationEventService;
            _reportApiClient = reportApiClient;
            _mapper = mapper;
            _excelReportCreater = excelReportCreater;
            _environment = environment;
            _appSettings = appSettings.Value;
        }

        public async Task<string> CreateReportAsync(ObjectId reportId)
        {
            var report = await _reportRepository.GetById(reportId);

            if (report.State == ReportState.Completed)
                throw new Exception("ReportAlreadyCompleted");

            var reportData = await _reportApiClient.GetLocationList();

            var fileUrl = _excelReportCreater.CreateLocationReport(reportId.ToString(), reportData);

            report.ReportPrepared(fileUrl);

            _reportRepository.Update(report);

            return report.Id.ToString();
        }

        public async Task CreateReportRequestAsync()
        {
            var report = new Report(DateTime.Now);

            _reportRepository.Add(report);

            await _uow.SaveChangesAsync();

            await _reportingIntegrationEventService.AddAndSaveEventAsync(new ReportCreatedIntegrationEvent(report.Id.ToString()));
        }

        public async Task<List<ReportDto>> GetReports()
        {
            var reports = await _reportRepository.GetAll();
            return _mapper.Map<List<ReportDto>>(reports);
        }


        #region private


        #endregion

    }
}

using AutoMapper;
using ClosedXML.Excel;
using Contacting.Dto.Persons.Reports;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Reporting.Application.IntegrationEvents;
using Reporting.Application.IntegrationEvents.Events;
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
        private IHostingEnvironment _environment;
        private AppSettings _appSettings;


        public ReportAppService(
            IUnitOfWork uow,
            IRepository<Report> reportRepository,
            IReportingIntegrationEventService reportingIntegrationEventService,
            IReportApiClient reportApiClient,
            IMapper mapper,
            IHostingEnvironment environment,
            IOptions<AppSettings> appSettings)
        {
            _uow = uow;
            _reportRepository = reportRepository;
            _reportingIntegrationEventService = reportingIntegrationEventService;
            _reportApiClient = reportApiClient;
            _mapper = mapper;
            _environment = environment;
            _appSettings = appSettings.Value;
        }

        public async Task CreateReportAsync(string reportId)
        {
            var report = await _reportRepository.GetById(new ObjectId(reportId));

            if (report.State == ReportState.Completed)
                return;

            var reportData = await _reportApiClient.GetLocationList();
            var reportTable = GetReportTable("LocationReport", reportData);
            var fileUrl = WriteReport(reportId, reportTable);

            report.ReportPrepared(fileUrl);

            _reportRepository.Update(report);
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
        private DataTable GetReportTable(string tableName, List<LocationReport> reportData)
        {
            DataTable table = new DataTable { TableName = tableName };

            table.Columns.Add("Location", typeof(String));
            table.Columns.Add("PersonCount", typeof(int));
            table.Columns.Add("PhoneCount", typeof(int));

            reportData.ForEach(x =>
            {
                table.Rows.Add(x.Location, x.PersonCount, x.PhoneCount);
            });

            return table;
        }
        private string WriteReport(string fileName, DataTable reportTable)
        {
            fileName = $"{fileName}.xlsx";
            var basePath = $"reports";
            var fullBasePath = $"{_environment.WebRootPath}/{basePath}";
            var fullFilePath = $"{fullBasePath}/{fileName}";

            if (!Directory.Exists(fullBasePath))
            {
                Directory.CreateDirectory(fullBasePath);
            }

            var ds = new DataSet();
            var wb = new XLWorkbook();

            ds.Tables.Add(reportTable);
            wb.Worksheets.Add(ds);
            wb.SaveAs(fullFilePath);

            return $"{_appSettings.ApplicationUrl}/{basePath}/{fileName}";
        }

        #endregion

    }
}

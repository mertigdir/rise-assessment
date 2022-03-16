using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Moq;
using Reporting.Application.IntegrationEvents;
using Reporting.Application.Reports;
using Reporting.Application.Reports.ReportCreators;
using Reporting.Application.Services;
using Reporting.Core.Models.Reports;
using Reporting.Core.Shared;
using Reporting.MongoDb.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Reporting.UnitTests.Application
{
    public class ReportAppServiceTest
    {

        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IRepository<Report>> _reportRepository;
        private readonly Mock<IReportingIntegrationEventService> _reportingIntegrationEventService;
        private readonly Mock<IReportApiClient> _reportApiClient;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IExcelReportCreater> _excelReportCreater;
        private readonly Mock<IHostingEnvironment> _environment;
        private readonly Mock<IOptions<AppSettings>> _appSettings;


        public ReportAppServiceTest()
        {
            _uow = new Mock<IUnitOfWork>(); ;
            _reportRepository = new Mock<IRepository<Report>>();
            _reportingIntegrationEventService = new Mock<IReportingIntegrationEventService>();
            _reportApiClient = new Mock<IReportApiClient>();
            _mapper = new Mock<IMapper>();
            _excelReportCreater = new Mock<IExcelReportCreater>();
            _environment = new Mock<IHostingEnvironment>();
            _appSettings = new Mock<IOptions<AppSettings>>();
        }

        [Fact]
        public void CreateReportAsync_ReportStateCompleted_Should_return_exception()
        {
            //Arrange
            var report = new Report(DateTime.Now);
            report.ReportPrepared("file-url");

            _reportRepository.Setup(x => x.GetById(It.IsAny<ObjectId>())).Returns(Task.FromResult(report));
            _uow.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(true));

            //Act
            var reportAppService = new ReportAppService(
                                                        _uow.Object,
                                                        _reportRepository.Object,
                                                        _reportingIntegrationEventService.Object,
                                                        _reportApiClient.Object,
                                                        _mapper.Object,
                                                        _excelReportCreater.Object,
                                                        _environment.Object,
                                                        _appSettings.Object);

            Assert.ThrowsAsync<Exception>(async () => { await reportAppService.CreateReportAsync(ObjectId.GenerateNewId()); });
        }
    }
}

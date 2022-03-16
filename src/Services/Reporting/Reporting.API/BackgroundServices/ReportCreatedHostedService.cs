using Autofac;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Reporting.Application.IntegrationEvents.Events;
using Reporting.Application.Reports;
using Reporting.MongoDb;
using Reporting.MongoDb.Shared;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Utility.RabbimMQ;

namespace Reporting.API.BackgroundServices
{
    public class ReportCreatedHostedService : BackgroundService
    {
        private readonly ILogger<ReportCreatedHostedService> _logger;
        private readonly RabbitMQClientService _rabbitMQClientService;
        private ILifetimeScope _lifetimeScope;
        private IModel _channel;

        public ReportCreatedHostedService(ILogger<ReportCreatedHostedService> logger, RabbitMQClientService rabbitMQClientService, ILifetimeScope lifetimeScope)
        {
            _logger = logger;
            _rabbitMQClientService = rabbitMQClientService;
            _lifetimeScope = lifetimeScope;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMQClientService.Connect();
            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            _channel.QueueDeclare("reporting.v1", false, false, false, null);
            _channel.QueueBind("reporting.v1", "cap.default.router", nameof(ReportCreatedIntegrationEvent), null);
            _channel.BasicConsume("reporting.v1", false, consumer);

            consumer.Received += Consumer_Received;

            return Task.CompletedTask;
        }


        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var result = JsonConvert.DeserializeObject<ReportCreatedIntegrationEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));


            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                try
                {
                    var dbContext = _lifetimeScope.Resolve<IReportingDbContext>();
                    var uow = _lifetimeScope.Resolve<IUnitOfWork>();

                    dbContext.CreateTransaction();

                    var reportAppService = _lifetimeScope.Resolve<IReportAppService>();

                    await reportAppService.CreateReportAsync(result.ReportId);

                    await uow.CommitAsync();

                    _channel.BasicAck(@event.DeliveryTag, false);
                }
                catch (Exception ex)
                {

                }
            }
        }


        public override void Dispose()
        {
            base.Dispose();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reporting.MongoDb.Extensions;
using DotNetCore.CAP;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Reporting.MongoDb
{
    public class ReportingDbContext : IReportingDbContext
    {
        private IMongoDatabase Database { get; set; }
        public IClientSessionHandle Session { get; set; }
        public IMongoClient MongoClient { get; set; }
        private readonly List<Command> _commands;
        private readonly IConfiguration _configuration;
        private readonly ICapPublisher _capPublisher;
        private readonly IMediator _mediator;
        public ReportingDbContext(IConfiguration configuration, ICapPublisher capPublisher, IMediator mediator)
        {
            _configuration = configuration;
            _capPublisher = capPublisher;
            _mediator = mediator;

            // Every command will be stored and it'll be processed at SaveChanges
            _commands = new List<Command>();

            CreateMongoClient();
        }

        public async Task SaveChangesAsync()
        {
            var commandTasks = _commands.Where(x => x.Function != null).Select(c => c.Function());
            await Task.WhenAll(commandTasks);

            _commands.Clear();
        }

        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync();

                var entities = _commands.Select(c => c.Entity).ToList();
                await _mediator.DispatchDomainEventsAsync(entities);

                await Session.CommitTransactionAsync();
                CreateNewTransaction();

            }
            catch (Exception ex)
            {
                await Session.AbortTransactionAsync();
            }
        }

        public void CreateTransaction()
        {
            if (Session == null)
            {
                Session = MongoClient.StartTransaction(_capPublisher, autoCommit: false);
            }
        }

        private void CreateNewTransaction()
        {
            Session = MongoClient.StartTransaction(_capPublisher, autoCommit: false);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func, object entity)
        {
            _commands.Add(new Command(func, entity));
        }


        private void CreateMongoClient()
        {
            if (MongoClient == null)
            {
                MongoClient = new MongoClient(_configuration["MongoSettings:Connection"]);
                Database = MongoClient.GetDatabase(_configuration["MongoSettings:DatabaseName"]);
            }
        }
    }

    public class Command
    {
        public Command(Func<Task> func, object entity)
        {
            Function = func;
            Entity = entity;
        }
        public Func<Task> Function { get; set; }
        public object Entity { get; set; }
    }
}

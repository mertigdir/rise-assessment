using Contacting.Infrastructure.EntityConfigurations;
using Contacting.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contacting.Domain.SeedWork;
using DotNetCore.CAP;
using Contacting.Domain.Persons;

namespace Contacting.Infrastructure
{
    public class ContactingContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "contacting";

        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonContact> PersonContacts { get; set; }

        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;
        public ContactingContext()
        {

        }
        public ContactingContext(DbContextOptions<ContactingContext> options) : base(options) { }
        public ContactingContext(DbContextOptions<ContactingContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            System.Diagnostics.Debug.WriteLine("ContactingContext::ctor ->" + this.GetHashCode());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PersonContactEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PersonEntityTypeConfiguration());
        }


        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;


        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        public IDbContextTransaction BeginTransaction(ICapPublisher capBus, CancellationToken cancellationToken)
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = Database.BeginTransaction(capBus);
            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveEntitiesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

    }
}

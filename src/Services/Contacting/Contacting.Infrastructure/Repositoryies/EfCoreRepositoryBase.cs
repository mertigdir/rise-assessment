using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Contacting.Domain.SeedWork;
using Contacting.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Utility.Extensions;
using Utility.Reflection;

namespace Contacting.Infrastructure.Repositoryies
{
    /// <summary>
    /// Implements IRepository for Entity Framework.
    /// </summary>
    /// <typeparam name="TDbContext">DbContext which contains <typeparamref name="TEntity"/>.</typeparam>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
    public class EfCoreRepositoryBase<TDbContext, TEntity, TPrimaryKey> :
        RepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>, IAggregateRoot
        where TDbContext : DbContext
    {

        private TDbContext _dbContext;


        public EfCoreRepositoryBase(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
        public virtual async Task<TDbContext> GetContextAsync()
        {
            return await Task.FromResult(_dbContext);
        }

        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
        public virtual TDbContext GetContext()
        {
            return _dbContext;

        }

        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual async Task<DbSet<TEntity>> GetTableAsync()
        {
            var context = await GetContextAsync();
            return context.Set<TEntity>();
        }

        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> GetTable()
        {
            var context = GetContext();
            return context.Set<TEntity>();
        }

        /// <summary>
        /// Gets DbQuery for given entity.
        /// </summary>
        public virtual async Task<DbSet<TEntity>> GetDbQueryTableAsync()
        {
            return (await GetContextAsync()).Set<TEntity>();
        }

        /// <summary>
        /// Gets DbQuery for given entity.
        /// </summary>
        public virtual DbSet<TEntity> GetDbQueryTable()
        {
            return GetContext().Set<TEntity>();
        }

        private static readonly ConcurrentDictionary<Type, bool> EntityIsDbQuery =
            new ConcurrentDictionary<Type, bool>();


        protected virtual IQueryable<TEntity> GetQueryable()
        {
            if (EntityIsDbQuery.GetOrAdd(typeof(TEntity), key => GetContext().GetType().GetProperties().Any(property =>
                ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
                ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0],
                    typeof(IEntity<>)) &&
                property.PropertyType.GetGenericArguments().Any(x => x == typeof(TEntity)))))
            {
                return GetDbQueryTable().AsQueryable();
            }

            return GetTable().AsQueryable();
        }

        protected virtual async Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            if (EntityIsDbQuery.GetOrAdd(typeof(TEntity), key => GetContext().GetType().GetProperties().Any(property =>
                ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
                ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0],
                    typeof(IEntity<>)) &&
                property.PropertyType.GetGenericArguments().Any(x => x == typeof(TEntity)))))
            {
                return (await GetDbQueryTableAsync()).AsQueryable();
            }

            return (await GetTableAsync()).AsQueryable();
        }


        public virtual DbConnection GetConnection()
        {
            var connection = GetContext().Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        public virtual async Task<DbConnection> GetConnectionAsync()
        {
            var context = await GetContextAsync();
            var connection = context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            return connection;
        }


        public override IQueryable<TEntity> GetAll()
        {
            return GetAllIncluding();
        }

        public override async Task<IQueryable<TEntity>> GetAllAsync()
        {
            return await GetAllIncludingAsync();
        }

        public override IQueryable<TEntity> GetAllIncluding(
            params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetQueryable();

            if (propertySelectors.IsNullOrEmpty())
            {
                return query;
            }

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        public override async Task<IQueryable<TEntity>> GetAllIncludingAsync(
            params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = await GetQueryableAsync();

            if (propertySelectors.IsNullOrEmpty())
            {
                return query;
            }

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }


        public override async Task<List<TEntity>> GetAllListAsync()
        {
            return await (await GetAllAsync()).ToListAsync();
        }

        public override async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await (await GetAllAsync()).Where(predicate).ToListAsync();
        }

        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await (await GetAllAsync()).SingleAsync(predicate);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await (await GetAllAsync()).FirstOrDefaultAsync(
                CreateEqualityExpressionForId(id)
            );
        }

        public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await (await GetAllAsync()).FirstOrDefaultAsync(predicate);
        }

        public override TEntity Insert(TEntity entity)
        {
            return GetTable().Add(entity).Entity;
        }

        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                GetContext().SaveChanges();
            }

            return entity.Id;
        }

        public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                var context = await GetContextAsync();
                await context.SaveChangesAsync();
            }

            return entity.Id;
        }

        public override TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            entity = InsertOrUpdate(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                GetContext().SaveChanges();
            }

            return entity.Id;
        }

        public override async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            entity = await InsertOrUpdateAsync(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                var context = await GetContextAsync();
                await context.SaveChangesAsync();
            }

            return entity.Id;
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            GetContext().Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public override Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity = Update(entity);
            return Task.FromResult(entity);
        }

        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            GetTable().Remove(entity);
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }
        }

        public override async Task<int> CountAsync()
        {
            return await (await GetAllAsync()).CountAsync();
        }

        public override async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await (await GetAllAsync()).Where(predicate).CountAsync();
        }

        public override async Task<long> LongCountAsync()
        {
            return await (await GetAllAsync()).LongCountAsync();
        }

        public override async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await (await GetAllAsync()).Where(predicate).LongCountAsync();
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = GetContext().ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            GetTable().Attach(entity);
        }

        public DbContext GetDbContext()
        {
            return GetContext();
        }

        public async Task<DbContext> GetDbContextAsync()
        {
            return await GetContextAsync();
        }

        public async Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            var context = await GetContextAsync();
            await context.Entry(entity).Collection(collectionExpression).LoadAsync(cancellationToken);
        }

        public void EnsureCollectionLoaded<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            GetContext().Entry(entity).Collection(collectionExpression).Load();
        }

        public async Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            var context = await GetContextAsync();
            await context.Entry(entity).Reference(propertyExpression).LoadAsync(cancellationToken);
        }

        public void EnsurePropertyLoaded<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            GetContext().Entry(entity).Reference(propertyExpression).Load();
        }

        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = GetContext().ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, (ent.Entity as TEntity).Id)
                );

            return entry?.Entity as TEntity;
        }

        private static bool MayHaveTemporaryKey(TEntity entity)
        {
            if (typeof(TPrimaryKey) == typeof(byte))
            {
                return true;
            }

            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(entity.Id) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(entity.Id) <= 0;
            }

            return false;
        }
    }
}

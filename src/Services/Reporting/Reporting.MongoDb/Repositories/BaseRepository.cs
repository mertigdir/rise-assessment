using Reporting.MongoDb.Shared.SeedWork;
using MongoDB.Driver;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Reporting.MongoDb.Shared;
using MongoDB.Bson;

namespace Reporting.MongoDb.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        protected readonly IReportingDbContext Context;
        protected IMongoCollection<TEntity> DbSet;

        public BaseRepository(IReportingDbContext context)
        {
            Context = context;

            DbSet = Context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual void Add(TEntity obj)
        {
            Context.AddCommand(() => DbSet.InsertOneAsync(Context.Session, obj), obj);
        }

        public virtual async Task<TEntity> GetById(ObjectId id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual void Update(TEntity obj)
        {
            Context.AddCommand(() => DbSet.ReplaceOneAsync(Context.Session,Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj), obj);
        }

        public virtual void Remove(ObjectId id)
        {
            Context.AddCommand(() => DbSet.DeleteOneAsync(Context.Session,Builders<TEntity>.Filter.Eq("_id", id)), null);
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}

using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Reporting.MongoDb.Shared.SeedWork
{
    /// <summary>
    /// Basic implementation of IEntity interface.
    /// An entity can inherit this class of directly implement to IEntity interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public class Entity : DomainEvent, IEntity<ObjectId>
    {
        public ObjectId Id { get; set; }
    }
}

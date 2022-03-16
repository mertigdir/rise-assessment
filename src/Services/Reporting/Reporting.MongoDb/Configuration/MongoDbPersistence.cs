using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reporting.MongoDb.Configuration
{
    public static class MongoDbPersistence
    {
        public static void Configure()
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;
      
            var pack = new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfDefaultConvention(true)
                };
            ConventionRegistry.Register("MyConversation", pack, t => true);
        }
    }
}

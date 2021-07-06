using CompraAplicativos.Infrastructure.DataAccess.Schemas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;

namespace CompraAplicativos.Infrastructure.DataAccess
{
    public class MongoDB
    {
        public IMongoDatabase DB { get; }

        public MongoDB(
            IConfiguration configuration,
            ILogger<MongoDB> logger)
        {
            try
            {
                var client = new MongoClient(configuration["ConnectionString"]);
                DB = client.GetDatabase(configuration["NomeBanco"]);
                MapClasses();
            }
            catch (Exception ex)
            {
                logger.LogError("Não foi possivel se conectar ao MongoDB");
                throw new MongoException("Não foi possivel se conectar ao MongoDB", ex);
            }
        }

        private void MapClasses()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(AplicativoSchema)))
            {
                BsonClassMap.RegisterClassMap<AplicativoSchema>(i =>
                {
                    i.AutoMap();
                    i.MapIdMember(c => c.Id);
                    i.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ClienteSchema)))
            {
                BsonClassMap.RegisterClassMap<ClienteSchema>(i =>
                {
                    i.AutoMap();
                    i.MapIdMember(c => c.Id);
                    i.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}

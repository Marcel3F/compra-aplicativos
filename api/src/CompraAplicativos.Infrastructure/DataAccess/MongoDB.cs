using CompraAplicativos.Infrastructure.DataAccess.Schemas;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;

namespace CompraAplicativos.Infrastructure.DataAccess
{
    public class MongoDB
    {
        public IMongoDatabase DB { get; }

        public MongoDB(IConfiguration configuration)
        {
            try
            {
                var client = new MongoClient(configuration["ConnectionString"]);
                DB = client.GetDatabase(configuration["NomeBanco"]);
                MapClasses();
            }
            catch (Exception ex)
            {
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
        }
    }
}

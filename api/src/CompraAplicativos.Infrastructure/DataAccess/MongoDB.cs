using CompraAplicativos.Core.Compras.Enums;
using CompraAplicativos.Infrastructure.DataAccess.Schemas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;

namespace CompraAplicativos.Infrastructure.DataAccess
{
    public sealed class MongoDB
    {
        public IMongoDatabase Database { get; }

        public MongoDB(
            IConfiguration configuration,
            ILogger<MongoDB> logger)
        {
            try
            {
                MongoClient client = new MongoClient(configuration["ConnectionString"]);
                Database = client.GetDatabase(configuration["NomeBanco"]);
                MapClasses();
            }
            catch (Exception ex)
            {
                const string messagem = "Não foi possivel se conectar ao MongoDB";
                logger.LogError(messagem);
                throw new MongoException(messagem, ex);
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

            if (!BsonClassMap.IsClassMapRegistered(typeof(CompraSchema)))
            {
                BsonClassMap.RegisterClassMap<CompraSchema>(i =>
                {
                    i.AutoMap();
                    i.MapIdMember(c => c.Id);
                    i.MapMember(c => c.Status).SetSerializer(new EnumSerializer<Status>(BsonType.Int32));
                    i.MapMember(c => c.ModoPagamento).SetSerializer(new EnumSerializer<ModoPagamento>(BsonType.Int32));
                    i.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompraAplicativos.Infrastructure.DataAccess.Schemas
{
    public sealed class ClienteCartaoSchema
    {
        public ObjectId Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ClienteId { get; set; }
        public CartaoSchema Cartao { get; set; }
    }
}
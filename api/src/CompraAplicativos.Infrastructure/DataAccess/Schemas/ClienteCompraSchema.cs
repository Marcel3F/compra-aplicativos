using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompraAplicativos.Infrastructure.DataAccess.Schemas
{
    public sealed class ClienteCompraSchema
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
    }
}
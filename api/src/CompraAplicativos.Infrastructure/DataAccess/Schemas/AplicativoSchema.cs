using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompraAplicativos.Infrastructure.DataAccess.Schemas
{
    public class AplicativoSchema
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
    }
}

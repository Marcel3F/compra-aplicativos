using CompraAplicativos.Consumer.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompraAplicativos.Consumer.Models
{
    public sealed class Compra
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Status Status { get; set; }
        public ModoPagamento ModoPagamento { get; set; }
    }
}

using CompraAplicativos.Core.Compras.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CompraAplicativos.Infrastructure.DataAccess.Schemas
{
    public sealed class CompraSchema
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Status Status { get; set; }
        public DateTime DataCompra { get; set; }
        public decimal Valor { get; set; }
        public ModoPagamento ModoPagamento { get; set; }
        public CartaoSchema Cartao { get; set; }
        public ClienteCompraSchema Cliente { get; set; }
        public AplicativoSchema Aplicativo { get; set; }
    }
}

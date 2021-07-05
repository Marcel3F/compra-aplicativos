using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CompraAplicativos.Infrastructure.DataAccess.Schemas
{
    public class ClienteSchema
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public EnderecoSchema Endereco { get; set; }
    }
}

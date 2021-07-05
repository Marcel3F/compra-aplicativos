using CompraAplicativos.Core.Clientes.ValueObjects;

namespace CompraAplicativos.Infrastructure.DataAccess.Schemas.Extensions
{
    public static class ClienteSchemaExtension
    {
        public static Core.Clientes.Cliente SchemaToEntity(this ClienteSchema schema)
        {
            var endereco = new Endereco(schema.Endereco.Logradouro, schema.Endereco.Numero, schema.Endereco.Complemento, schema.Endereco.Cep, schema.Endereco.Cidade, schema.Endereco.UF);
            Core.Clientes.Cliente cliente = new Core.Clientes.Cliente(schema.Id, schema.Nome, schema.Cpf, schema.DataNascimento, schema.Sexo, endereco);

            return cliente;
        }
    }
}

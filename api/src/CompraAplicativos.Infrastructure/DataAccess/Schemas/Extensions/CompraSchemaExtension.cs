namespace CompraAplicativos.Infrastructure.DataAccess.Schemas.Extensions
{
    public static class CompraSchemaExtension
    {
        public static Core.Compras.Compra SchemaToEntity(this CompraSchema schema)
        {
            Core.Clientes.Cliente cliente = new Core.Clientes.Cliente(schema.Cliente.Id, schema.Cliente.Nome, schema.Cliente.Cpf);
            
            Core.Aplicativos.Aplicativo aplicativo = new Core.Aplicativos.Aplicativo(schema.Aplicativo.Id, schema.Aplicativo.Nome, schema.Valor);

            Core.Compras.Compra compra = new Core.Compras.Compra(schema.Id, cliente, aplicativo, schema.Valor, schema.ModoPagamento);
            
            compra.AtribuirStatus(schema.Status);
            compra.AtribuirDataCompra(schema.DataCompra);

            return compra;
        }
    }
}

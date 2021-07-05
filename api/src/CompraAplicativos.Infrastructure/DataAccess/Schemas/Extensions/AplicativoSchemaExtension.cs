namespace CompraAplicativos.Infrastructure.DataAccess.Schemas.Extensions
{
    public static class AplicativoSchemaExtension
    {
        public static Core.Aplicativos.Aplicativo SchemaToEntity(this AplicativoSchema schema)
        {
            Core.Aplicativos.Aplicativo aplicativo = new Core.Aplicativos.Aplicativo(schema.Id, schema.Nome, schema.Valor);
            return aplicativo;
        }
    }
}

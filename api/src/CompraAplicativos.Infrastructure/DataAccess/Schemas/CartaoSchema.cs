namespace CompraAplicativos.Infrastructure.DataAccess.Schemas
{
    public sealed class CartaoSchema
    {
        public string Numero { get; set; }
        public string CCV { get; set; }
        public string Validade { get; set; }
    }
}
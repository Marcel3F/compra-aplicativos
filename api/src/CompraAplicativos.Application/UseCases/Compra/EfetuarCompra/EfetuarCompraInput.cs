namespace CompraAplicativos.Application.UseCases.Compra.EfetuarCompra
{
    public sealed class EfetuarCompraInput
    {
        public string ClienteId { get; set; }
        public string AplicativoId { get; set; }
    }
}
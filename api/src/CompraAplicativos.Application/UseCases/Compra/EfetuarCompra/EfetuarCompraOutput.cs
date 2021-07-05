using CompraAplicativos.Core.Compras.Enums;
using System;

namespace CompraAplicativos.Application.UseCases.Compra.EfetuarCompra
{
    public sealed class EfetuarCompraOutput
    {
        public EfetuarCompraOutput(Core.Compras.Compra compra)
        {
            //Id = compra.Id;
            //ClienteId = compra.Cliente.Id;
            //AplicativoId = compra.Aplicativo.Id;
            //DataRegistro = compra.DataRegistro;
            Status = compra.Status;
        }

        public string Id { get; }
        public string ClienteId { get; }
        public string AplicativoId { get; }
        public DateTime DataRegistro { get; }
        public Status Status { get; }
    }
}
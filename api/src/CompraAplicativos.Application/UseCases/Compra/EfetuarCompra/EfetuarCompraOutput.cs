using CompraAplicativos.Core.Compras.Enums;
using System;

namespace CompraAplicativos.Application.UseCases.Compra.EfetuarCompra
{
    public sealed class EfetuarCompraOutput
    {
        public EfetuarCompraOutput(Core.Compras.Compra compra)
        {
            Id = compra.Id;
            ClienteId = compra.Cliente.Id;
            AplicativoId = compra.Aplicativo.Id;
            DataCompra = compra.DataCompra;
            Status = compra.Status;
            Valor = compra.Valor;
            ModoPagamento = compra.ModoPagamento;
        }

        public string Id { get; }
        public string ClienteId { get; }
        public string AplicativoId { get; }
        public DateTime DataCompra { get; }
        public Status Status { get; }
        public decimal Valor { get; }
        public ModoPagamento ModoPagamento { get; }
    }
}
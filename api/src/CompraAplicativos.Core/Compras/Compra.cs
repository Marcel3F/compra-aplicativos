using CompraAplicativos.Core.Aplicativos;
using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Core.Compras.Enums;
using System;

namespace CompraAplicativos.Core.Compras
{
    public sealed class Compra
    {
        public Compra(
            string id,
            Cliente cliente,
            Aplicativo aplicativo,
            decimal valor,
            ModoPagamento modoPagamento)
        {
            Id = id;
            Cliente = cliente;
            Aplicativo = aplicativo;
            Valor = valor;
            ModoPagamento = modoPagamento;

            AtribuirStatus(Status.AguardandoProcessamento);
            AtribuirDataCompra(DateTime.Now);
        }

        public Compra(
            Cliente cliente,
            Aplicativo aplicativo,
            decimal valor,
            ModoPagamento modoPagamento)
        {
            Cliente = cliente;
            Aplicativo = aplicativo;
            Valor = valor;
            ModoPagamento = modoPagamento;

            AtribuirStatus(Status.AguardandoProcessamento);
            AtribuirDataCompra(DateTime.Now);
        }

        public void AtribuirDataCompra(DateTime dataCompra)
        {
            DataCompra = dataCompra;
        }

        public void AtribuirStatus(Status status)
        {
            Status = status;
        }

        public string Id { get; }
        public Cliente Cliente { get; }
        public Aplicativo Aplicativo { get; }
        public decimal Valor { get; }
        public ModoPagamento ModoPagamento { get; }
        public Status Status { get; private set; }
        public DateTime DataCompra { get; private set; }
    }
}

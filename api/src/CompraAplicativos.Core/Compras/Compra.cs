using CompraAplicativos.Core.Aplicativos;
using CompraAplicativos.Core.Cartoes.ValueObjects;
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
        }

        public Compra(
            Cliente cliente,
            Aplicativo aplicativo,
            decimal valor,
            ModoPagamento modoPagamento,
            Cartao cartao)
        {
            Cliente = cliente;
            Aplicativo = aplicativo;
            Valor = valor;
            ModoPagamento = modoPagamento;
            Cartao = cartao;

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

        public void AtribuirCartao(Cartao cartao)
        {
            Cartao = cartao;
        }

        public string Id { get; }
        public Cliente Cliente { get; }
        public Aplicativo Aplicativo { get; }
        public decimal Valor { get; }
        public ModoPagamento ModoPagamento { get; }
        public Status Status { get; private set; }
        public DateTime DataCompra { get; private set; }
        public Cartao Cartao { get; private set; }
    }
}

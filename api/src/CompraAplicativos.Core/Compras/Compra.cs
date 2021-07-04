using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Core.Compras.Enums;
using CompraAplicativos.Core.Compras.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace CompraAplicativos.Core.Compras
{
    public sealed class Compra
    {
        public Compra(
            Cliente cliente,
            List<Item> itens)
        {
            Cliente = cliente;
            _itens = itens;

            AtribuirStatus(Status.AguardandoProcessamento);
        }

        public void AtribuirStatus(Status status)
        {
            Status = status;
        }

        public Cliente Cliente { get; }
        public decimal ValorTotal 
        { 
            get
            {
                return _itens.Sum(item => item.Valor);
            }
        }
        public readonly List<Item> _itens;
        public IReadOnlyCollection<Item> Itens => _itens;
        public Status Status { get; private set; }

        

    }
}

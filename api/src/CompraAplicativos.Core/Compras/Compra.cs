using CompraAplicativos.Core.Aplicativos;
using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Core.Compras.Enums;

namespace CompraAplicativos.Core.Compras
{
    public sealed class Compra
    {
        public Compra(
            Cliente cliente,
            Aplicativo aplicativo)
        {
            Cliente = cliente;
            Aplicativo = aplicativo;

            AtribuirStatus(Status.AguardandoProcessamento);
        }

        public void AtribuirStatus(Status status)
        {
            Status = status;
        }

        public Cliente Cliente { get; }
        public Aplicativo Aplicativo { get; }
        public Status Status { get; private set; }
    }
}

using CompraAplicativos.Consumer.Models;

namespace CompraAplicativos.Infrastructure.MessageBroker
{
    public interface IProcessaCompraReceiver
    {
        Compra RecuperarMensagemCompra();
        void Limpar();
    }
}
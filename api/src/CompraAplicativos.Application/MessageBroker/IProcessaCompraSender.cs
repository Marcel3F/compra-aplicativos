using CompraAplicativos.Core.Compras;
using System.Threading.Tasks;

namespace CompraAplicativos.Application.MessageBroker
{
    public interface IProcessaCompraSender
    {
        Task Enviar(Compra compra);
    }
}

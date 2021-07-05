using System.Threading.Tasks;

namespace CompraAplicativos.Core.Compras
{
    public interface ICompraRepository
    {
        Task Registrar(Compra compra);
    }
}

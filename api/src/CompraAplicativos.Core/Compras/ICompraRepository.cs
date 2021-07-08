using System.Threading.Tasks;

namespace CompraAplicativos.Core.Compras
{
    public interface ICompraRepository
    {
        Task<bool> AlterarStatusCompraParaFalha(string compraId);
        Task<Compra> Registrar(Compra compra);
    }
}

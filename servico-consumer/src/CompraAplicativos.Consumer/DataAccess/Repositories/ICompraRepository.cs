using System.Threading.Tasks;

namespace CompraAplicativos.Consumer.DataAccess.Repositories
{
    public interface ICompraRepository
    {
        Task<bool> AlterarStatusCompraParaProcessado(string compraId);
    }
}
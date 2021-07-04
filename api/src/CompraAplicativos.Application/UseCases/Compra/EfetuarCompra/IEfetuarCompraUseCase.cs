using System.Threading.Tasks;

namespace CompraAplicativos.Application.UseCases.Compra.EfetuarCompra
{
    public interface IEfetuarCompraUseCase
    {
        Task<EfetuarCompraOutput> Executar(EfetuarCompraInput input);
    }
}
using CompraAplicativos.Core.Compras;
using System.Threading.Tasks;

namespace CompraAplicativos.Application.UseCases.Compra.EfetuarCompra
{
    public sealed class EfetuarCompraUseCase : IEfetuarCompraUseCase
    {
        private readonly ICompraRepository compraRepository;

        public EfetuarCompraUseCase(ICompraRepository compraRepository)
        {
            this.compraRepository = compraRepository;
        }

        public Task<EfetuarCompraOutput> Executar(EfetuarCompraInput input)
        {
            throw new System.NotImplementedException();
        }
    }
}

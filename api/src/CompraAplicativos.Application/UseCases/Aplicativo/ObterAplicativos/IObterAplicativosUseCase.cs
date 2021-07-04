using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompraAplicativos.Application.UseCases.Aplicativo.ObterAplicativos
{
    public interface IObterAplicativosUseCase
    {
        Task<IEnumerable<ObterAplicativosOutput>> Executar();
    }
}

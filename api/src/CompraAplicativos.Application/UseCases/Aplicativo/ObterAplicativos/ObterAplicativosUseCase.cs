using CompraAplicativos.Core.Aplicativos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CompraAplicativos.Application.UseCases.Aplicativo.ObterAplicativos
{
    public sealed class ObterAplicativosUseCase : IObterAplicativosUseCase
    {
        private readonly IAplicativoRepository _aplicativoRepository;

        public ObterAplicativosUseCase(IAplicativoRepository aplicativoRepository)
        {
            _aplicativoRepository = aplicativoRepository;
        }

        public async Task<IEnumerable<ObterAplicativosOutput>> Executar()
        {
            var aplicativos = await _aplicativoRepository.ObterAplicativos();
            
            return aplicativos.Select(aplicativo => new ObterAplicativosOutput(aplicativo.Nome, aplicativo.Valor));
        }
    }
}

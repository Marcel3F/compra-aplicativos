using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompraAplicativos.Core.Aplicativos
{
    public interface IAplicativoRepository
    {
        Task<IEnumerable<Aplicativo>> ObterAplicativos();
    }
}

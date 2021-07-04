using CompraAplicativos.Core.Aplicativos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompraAplicativos.Infrastructure.DataAccess.Repositories
{
    public sealed class AplicativoRepository : IAplicativoRepository
    {
        public async Task<IEnumerable<Aplicativo>> ObterAplicativos()
        {
            return new List<Aplicativo> { new Aplicativo("teste", 10) };
        }
    }
}

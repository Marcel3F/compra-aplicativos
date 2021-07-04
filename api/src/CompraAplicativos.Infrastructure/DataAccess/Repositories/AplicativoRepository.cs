using CompraAplicativos.Core.Aplicativos;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompraAplicativos.Infrastructure.DataAccess.Repositories
{
    public sealed class AplicativoRepository : IAplicativoRepository
    {
        private readonly IMemoryCache _memoryCache;

        public AplicativoRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Aplicativo>> ObterAplicativos()
        {
            List<Aplicativo> aplicativos = _memoryCache.GetOrCreate("aplicativos", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return new List<Aplicativo> { new Aplicativo("teste", 10) };
            });

            return aplicativos;
        }
    }
}

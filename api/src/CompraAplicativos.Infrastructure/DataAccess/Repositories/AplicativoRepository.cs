using CompraAplicativos.Core.Aplicativos;
using CompraAplicativos.Infrastructure.DataAccess.Schemas;
using CompraAplicativos.Infrastructure.DataAccess.Schemas.Extensions;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompraAplicativos.Infrastructure.DataAccess.Repositories
{
    public sealed class AplicativoRepository : IAplicativoRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IMongoCollection<AplicativoSchema> _aplicativos;

        public AplicativoRepository(
            IMemoryCache memoryCache,
            MongoDB mongoDB)
        {
            _memoryCache = memoryCache;
            _aplicativos = mongoDB.DB.GetCollection<AplicativoSchema>("aplicativos");
        }

        public async Task<Aplicativo> ObterAplicativoPorId(string aplicativoId)
        {
            AplicativoSchema aplicativoSchema = await _aplicativos.AsQueryable().FirstOrDefaultAsync(aplicativo => aplicativo.Id == aplicativoId).ConfigureAwait(false);

            if (aplicativoSchema is null)
            {
                return default;
            }

            return aplicativoSchema.SchemaToEntity();
        }

        public async Task<IEnumerable<Aplicativo>> ObterAplicativos()
        {
            List<Aplicativo> aplicativos = (List<Aplicativo>)await _memoryCache.GetOrCreate("aplicativos", async entry =>
             {
                 entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                 return await ObterAplicativosBD();
             }).ConfigureAwait(false);

            return aplicativos;
        }

        private async Task<IEnumerable<Aplicativo>> ObterAplicativosBD()
        {
            List<Aplicativo> aplicativos = new List<Aplicativo>();

            await _aplicativos.AsQueryable().ForEachAsync(d =>
            {
                aplicativos.Add(new Aplicativo(d.Id.ToString(), d.Nome, d.Valor));
            });

            return aplicativos;
        }
    }
}

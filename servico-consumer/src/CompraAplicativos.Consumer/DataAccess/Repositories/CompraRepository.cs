using CompraAplicativos.Consumer.Models;
using CompraAplicativos.Consumer.Models.Enums;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace CompraAplicativos.Consumer.DataAccess.Repositories
{
    public sealed class CompraRepository : ICompraRepository
    {
        private readonly IMongoCollection<Compra> _compras;

        public CompraRepository(MongoDB mongoDB)
        {
            _compras = mongoDB.Database.GetCollection<Compra>("compras");
        }

        public async Task<bool> AlterarStatusCompraParaProcessado(string compraId)
        {
            UpdateDefinition<Compra> atualizarStatus = Builders<Compra>.Update.Set(compra => compra.Status, Status.Processado);

            UpdateResult resultado = await _compras.UpdateOneAsync(compra => compra.Id == compraId, atualizarStatus).ConfigureAwait(false);

            return resultado.ModifiedCount > 0;
        }
    }
}

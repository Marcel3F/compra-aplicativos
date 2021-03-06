using CompraAplicativos.Core.Compras;
using CompraAplicativos.Core.Compras.Enums;
using CompraAplicativos.Infrastructure.DataAccess.Schemas;
using CompraAplicativos.Infrastructure.DataAccess.Schemas.Extensions;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace CompraAplicativos.Infrastructure.DataAccess.Repositories
{
    public sealed class CompraRepository : ICompraRepository
    {
        private readonly IMongoCollection<CompraSchema> _compras;

        public CompraRepository(MongoDB mongoDB)
        {
            _compras = mongoDB.Database.GetCollection<CompraSchema>("compras");
        }

        public async Task<Compra> Registrar(Compra compra)
        {
            CompraSchema compraSchema = new CompraSchema
            {
                Status = compra.Status,
                DataCompra = compra.DataCompra,
                Valor = compra.Valor,
                ModoPagamento = compra.ModoPagamento,
                Cartao = new CartaoSchema
                {
                    Numero = compra.Cartao.Numero,
                    CCV = compra.Cartao.Ccv,
                    Validade = compra.Cartao.Validade
                },
                Cliente = new ClienteCompraSchema
                {
                    Id = compra.Cliente.Id,
                    Nome = compra.Cliente.Nome,
                    Cpf = compra.Cliente.Cpf
                },
                Aplicativo = new AplicativoSchema
                {
                    Id = compra.Aplicativo.Id,
                    Nome = compra.Aplicativo.Nome,
                    Valor = compra.Aplicativo.Valor
                }
            };

            await _compras.InsertOneAsync(compraSchema).ConfigureAwait(false);
            return compraSchema.SchemaToEntity();
        }

        public async Task<bool> AlterarStatusCompraParaFalha(string compraId)
        {
            UpdateDefinition<CompraSchema> atualizarStatus = Builders<CompraSchema>.Update.Set(compra => compra.Status, Status.Falha);

            UpdateResult resultado = await _compras.UpdateOneAsync(compra => compra.Id == compraId, atualizarStatus).ConfigureAwait(false);

            return resultado.ModifiedCount > 0;
        }
    }
}

using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Infrastructure.DataAccess.Schemas;
using CompraAplicativos.Infrastructure.DataAccess.Schemas.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

namespace CompraAplicativos.Infrastructure.DataAccess.Repositories
{
    public sealed class ClienteRepository : IClienteRepository
    {
        private readonly IMongoCollection<ClienteSchema> _clientes;

        public ClienteRepository(MongoDB mongoDB)
        {
            _clientes = mongoDB.DB.GetCollection<ClienteSchema>("clientes");
        }

        public async Task Cadastrar(Cliente cliente)
        {
            ClienteSchema clienteSchema = new ClienteSchema
            {
                Nome = cliente.Nome,
                Cpf = cliente.Cpf,
                DataNascimento = cliente.DataNascimento,
                Sexo = cliente.Sexo,
                Endereco = new EnderecoSchema
                {
                    Logradouro = cliente.Endereco.Logradouro,
                    Numero = cliente.Endereco.Numero,
                    Complemento = cliente.Endereco.Complemento,
                    Cidade = cliente.Endereco.Cidade,
                    Cep = cliente.Endereco.Cep,
                    UF = cliente.Endereco.Uf
                }
            };

            await _clientes.InsertOneAsync(clienteSchema).ConfigureAwait(false);
        }

        public async Task<Cliente> ObterClientePorCpf(string cpf)
        {
            ClienteSchema clienteSchema = await _clientes.AsQueryable().FirstOrDefaultAsync(cliente => cliente.Cpf == cpf).ConfigureAwait(false);

            if (clienteSchema is null)
            {
                return default;
            }

            return clienteSchema.SchemaToEntity();
        }

        public async Task<Cliente> ObterClientePorId(string clienteId)
        {
            ClienteSchema clienteSchema = await _clientes.AsQueryable().FirstOrDefaultAsync(cliente => cliente.Id == clienteId).ConfigureAwait(false);

            if (clienteSchema is null)
            {
                return default;
            }

            return clienteSchema.SchemaToEntity();
        }

        public async Task<bool> VerificarClienteExistePorCpf(string cpf)
        {
            return await _clientes.AsQueryable().AnyAsync(cliente => cliente.Cpf == cpf).ConfigureAwait(false);
        }
    }
}

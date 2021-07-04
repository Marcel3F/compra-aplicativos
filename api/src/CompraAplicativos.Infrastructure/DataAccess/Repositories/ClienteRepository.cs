using CompraAplicativos.Core.Clientes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CompraAplicativos.Infrastructure.DataAccess.Repositories
{
    public sealed class ClienteRepository : IClienteRepository
    {
        private readonly List<Cliente> clientes;

        public ClienteRepository()
        {
            clientes = new List<Cliente> { new Cliente("teste", "33333333333", DateTime.Now, "M", null) };
        }

        public async Task Cadastrar(Cliente cliente)
        {
            clientes.Add(cliente);
        }

        public async Task<Cliente> ObterClientePorCpf(string cpf)
        {
            return clientes.FirstOrDefault(cliente => cliente.Cpf == cpf);
        }

        public async Task<bool> VerificarClienteExistePorCpf(string cpf)
        {
            return clientes.Any(cliente => cliente.Cpf == cpf);
        }
    }
}

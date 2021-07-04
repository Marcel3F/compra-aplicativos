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
            clientes = new List<Cliente> { new Cliente("teste", "10", DateTime.Now, "M", null) };
        }

        public async Task Cadastrar(Cliente cliente)
        {
            clientes.Add(new Cliente("teste", "10", DateTime.Now, "F", null));
        }

        public async Task<bool> VerificarClienteExistePorCpf(string cpf)
        {
            return clientes.Any(cliente => cliente.Cpf == cpf);
        }
    }
}

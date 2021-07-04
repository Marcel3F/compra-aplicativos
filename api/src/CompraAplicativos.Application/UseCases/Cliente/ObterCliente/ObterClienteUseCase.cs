using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Core.Clientes;
using System;
using System.Threading.Tasks;

namespace CompraAplicativos.Application.UseCases.Cliente.ObterCliente
{
    public sealed class ObterClienteUseCase : IObterClienteUseCase
    {
        private readonly IClienteRepository _clienteRepository;

        public ObterClienteUseCase(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ObterClienteOutput> Executar(string cpf)
        {
            Core.Clientes.Cliente cliente = await ObterClientePorCpf(cpf);

            return new ObterClienteOutput(cliente);
        }

        private async Task<Core.Clientes.Cliente> ObterClientePorCpf(string cpf)
        {
            Core.Clientes.Cliente cliente = await _clienteRepository.ObterClientePorCpf(cpf);

            if (cliente is null)
            {
                throw new NotFoundException("Cliente  não cadastrado");
            }

            return cliente;
        }
    }
}

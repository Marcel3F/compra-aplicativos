using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Core.Clientes.ValueObjects;
using System.Threading.Tasks;

namespace CompraAplicativos.Application.UseCases.Cliente.CadastrarCliente
{
    public sealed class CadastrarClienteUseCase : ICadastrarClienteUseCase
    {
        private readonly IClienteRepository _clienteRepository;

        public CadastrarClienteUseCase(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<CadastrarClienteOutput> Executar(CadastrarClienteInput input)
        {
            bool clienteExiste = await ValidarClienteExiste(input.Cpf);

            if (clienteExiste)
            {
                throw new BusinessException("Cliente já cadastrado");
            }

            Core.Clientes.Cliente cliente = await CadastrarCliente(input);

            return new CadastrarClienteOutput(cliente);
        }

        private async Task<Core.Clientes.Cliente> CadastrarCliente(CadastrarClienteInput input)
        {
            Core.Clientes.Cliente cliente = new Core.Clientes.Cliente(
                input.Nome,
                input.Cpf,
                input.DataNascimento,
                input.Sexo,
                new Endereco(
                input.Logradouro,
                input.Numero,
                input.Complemento,
                input.Cep,
                input.Cidade,
                input.Uf));

            return await _clienteRepository.Cadastrar(cliente);
        }

        private async Task<bool> ValidarClienteExiste(string cpf)
        {
            return await _clienteRepository.VerificarClienteExistePorCpf(cpf);
        }
    }
}

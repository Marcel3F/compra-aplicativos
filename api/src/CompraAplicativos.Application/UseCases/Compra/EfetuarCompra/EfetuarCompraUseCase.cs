using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Core.Aplicativos;
using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Core.Compras;
using CompraAplicativos.Core.Compras.Enums;
using System;
using System.Threading.Tasks;

namespace CompraAplicativos.Application.UseCases.Compra.EfetuarCompra
{
    public sealed class EfetuarCompraUseCase : IEfetuarCompraUseCase
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IAplicativoRepository _aplicativoRepository;

        private Core.Clientes.Cliente Cliente;
        private Core.Aplicativos.Aplicativo Aplicativo;

        public EfetuarCompraUseCase(
            ICompraRepository compraRepository,
            IClienteRepository clienteRepository,
            IAplicativoRepository aplicativoRepository)
        {
            _compraRepository = compraRepository;
            _clienteRepository = clienteRepository;
            _aplicativoRepository = aplicativoRepository;
        }

        public async Task<EfetuarCompraOutput> Executar(EfetuarCompraInput input)
        {
            await RecuperarEntidadesDependentes(input);

            if (!Enum.IsDefined(typeof(ModoPagamento), input.ModoPagamento))
            {
                throw new BusinessException("Modo de pagamento indisponível");
            }

            Core.Compras.Compra compra = await RegistrarCompra(input);

            return new EfetuarCompraOutput(compra);
        }

        private async Task RecuperarEntidadesDependentes(EfetuarCompraInput input)
        {
            Cliente = await ObterCliente(input.ClienteId);

            if (Cliente is null)
            {
                throw new NotFoundException("Cliente não está cadastrado");
            }

            Aplicativo = await ObterAplicativo(input.AplicativoId);

            if (Aplicativo is null)
            {
                throw new NotFoundException("Aplicativo não está cadastrado");
            }
        }

        private async Task<Core.Aplicativos.Aplicativo> ObterAplicativo(string aplicativoId)
        {
            return await _aplicativoRepository.ObterAplicativoPorId(aplicativoId);
        }

        private async Task<Core.Clientes.Cliente> ObterCliente(string clienteId)
        {
            return await _clienteRepository.ObterClientePorId(clienteId);
        }

        private async Task<Core.Compras.Compra> RegistrarCompra(EfetuarCompraInput input)
        {
            Core.Compras.Compra compra = new Core.Compras.Compra(
                Cliente,
                Aplicativo,
                input.Valor,
                (ModoPagamento)input.ModoPagamento);

            return await _compraRepository.Registrar(compra);
        }
    }
}

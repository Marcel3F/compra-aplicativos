using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Core.Aplicativos;
using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Core.Compras;
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
            Cliente = await ObterCliente(input.ClienteId);

            if (Cliente is null)
            {
                throw new NotFoundException();
            }

            Aplicativo = await ObterAplicativo(input.AplicativoId);

            if (Aplicativo is null)
            {
                throw new NotFoundException();
            }

            Core.Compras.Compra compra = await RegistrarCompra();

            return new EfetuarCompraOutput(compra);
        }

        private async Task<Core.Aplicativos.Aplicativo> ObterAplicativo(string aplicativoId)
        {
            return await _aplicativoRepository.ObterAplicativoPorId(aplicativoId);
        }

        private async Task<Core.Clientes.Cliente> ObterCliente(string clienteId)
        {
            return await _clienteRepository.ObterClientePorId(clienteId);
        }

        private async Task<Core.Compras.Compra> RegistrarCompra()
        {
            Core.Compras.Compra compra = new Core.Compras.Compra(
                Cliente,
                Aplicativo);

            await _compraRepository.Registrar(compra);

            return compra;
        }
    }
}

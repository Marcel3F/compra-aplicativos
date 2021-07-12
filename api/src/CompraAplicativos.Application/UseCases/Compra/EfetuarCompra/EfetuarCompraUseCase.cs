using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Application.MessageBroker;
using CompraAplicativos.Core.Aplicativos;
using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Core.Compras;
using CompraAplicativos.Core.Compras.Enums;
using CompraAplicativos.Core.Compras.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CompraAplicativos.Application.UseCases.Compra.EfetuarCompra
{
    public sealed class EfetuarCompraUseCase : IEfetuarCompraUseCase
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IAplicativoRepository _aplicativoRepository;
        private readonly IProcessaCompraSender _processaCompraSender;

        private readonly ILogger<EfetuarCompraUseCase> _logger;

        private Core.Clientes.Cliente Cliente;
        private Core.Aplicativos.Aplicativo Aplicativo;

        public EfetuarCompraUseCase(
            ICompraRepository compraRepository,
            IClienteRepository clienteRepository,
            IAplicativoRepository aplicativoRepository,
            IProcessaCompraSender processaCompraSender,
            ILogger<EfetuarCompraUseCase> logger)
        {
            _compraRepository = compraRepository;
            _clienteRepository = clienteRepository;
            _aplicativoRepository = aplicativoRepository;
            _processaCompraSender = processaCompraSender;
            _logger = logger;
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
            Core.Compras.Compra compra = default;
            try
            {
                compra = new Core.Compras.Compra(
                Cliente,
                Aplicativo,
                input.Valor,
                (ModoPagamento)input.ModoPagamento);

                if (input.GuardarCartao)
                {
                    compra.GuardarCartao(new Cartao(input.Cartao));
                }

                compra = await _compraRepository.Registrar(compra);
                _logger.LogInformation("Compra {Id}: registrada com sucesso", compra.Id);

                await _processaCompraSender.Enviar(compra);
                return compra;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ocorreu algum erro ao registrar compra");

                if (compra != null && !string.IsNullOrEmpty(compra.Id))
                {
                    await _compraRepository.AlterarStatusCompraParaFalha(compra.Id);
                    _logger.LogInformation("Compra {Id}: registrada como falha", compra.Id);
                }

                throw;
            }

        }
    }
}

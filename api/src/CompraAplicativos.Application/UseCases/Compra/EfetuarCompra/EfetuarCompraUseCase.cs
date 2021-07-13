using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Application.MessageBroker;
using CompraAplicativos.Core.Aplicativos;
using CompraAplicativos.Core.Cartoes.ValueObjects;
using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Core.Compras;
using CompraAplicativos.Core.Compras.Enums;
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
        private Cartao Cartao;

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

            await ObterCartaoParaCompra(input);

            Core.Compras.Compra compra = await RegistrarCompra(input);

            if (input.GuardarCartao)
            {
                await GuardarCartao(compra);
            }

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

        private async Task<Cartao> ObterCartaoParaCompra(EfetuarCompraInput input)
        {
            await _clienteRepository.ObterCartaoCliente(Cliente, input.NumeroCartao);

            Cartao = Cliente.Cartao;
            if (Cartao is null)
            {
                Cartao = new Cartao(input.NumeroCartao, input.CCVCartao, input.ValidadeCartao);
            }

            ValidarDadosCartao(Cartao);

            return Cartao;
        }

        private static void ValidarDadosCartao(Cartao cartao)
        {
            if (string.IsNullOrEmpty(cartao.Ccv))
            {
                throw new BusinessException("CCV do cartão está inválido");
            }

            if (string.IsNullOrEmpty(cartao.Validade))
            {
                throw new BusinessException("Validade do cartão está inválida");
            }
            else
            {
                int mes = Convert.ToInt32(cartao.Validade.Substring(0, 2));
                int ano = Convert.ToInt32(cartao.Validade.Substring(2, 2));

                DateTime dataAtual = DateTime.Today;
                int anoAtual = Convert.ToInt32(dataAtual.ToString("yy"));
                int mesAtual = Convert.ToInt32(dataAtual.ToString("MM"));

                if (ano < anoAtual || (ano == anoAtual && mes < mesAtual))
                {
                    throw new BusinessException("Validade do cartão está inválida");
                }
            }
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
                (ModoPagamento)input.ModoPagamento,
                Cartao);

                compra = await _compraRepository.Registrar(compra);
                _logger.LogInformation("Compra {Id}: registrada com sucesso", compra.Id);

                await _processaCompraSender.Enviar(compra);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ocorreu algum erro ao registrar compra");

                if (compra != null && !string.IsNullOrEmpty(compra.Id))
                {
                    compra.AtribuirStatus(Status.Falha);
                    await _compraRepository.AlterarStatusCompraParaFalha(compra.Id);
                    _logger.LogInformation("Compra {Id}: registrada como falha", compra.Id);
                }

                throw;
            }

            return compra;
        }

        private async Task GuardarCartao(Core.Compras.Compra compra)
        {
            try
            {
                await _clienteRepository.GuardarCartaoCliente(Cliente.Id, compra.Cartao);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ocorreu algum erro ao guardar cartão");
            }
        }
    }
}

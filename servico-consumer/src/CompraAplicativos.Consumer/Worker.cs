using CompraAplicativos.Consumer.DataAccess.Repositories;
using CompraAplicativos.Infrastructure.MessageBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CompraAplicativos.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICompraRepository _compraRepository;
        private readonly IProcessaCompraReceiver _processaCompraReceiver;

        public Worker(
            ICompraRepository compraRepository,
            IProcessaCompraReceiver processaCompraReceiver,
            IConfiguration configuration,
            ILogger<Worker> logger)
        {
            _configuration = configuration;
            _compraRepository = compraRepository;
            _processaCompraReceiver = processaCompraReceiver;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int tempoDelay = Convert.ToInt32(_configuration["TempoDelay"]);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Iniciando busca das compras com cartão de crédito");

                ProcessarCompra();

                await Task.Delay(tempoDelay, stoppingToken);
            }
        }

        private void ProcessarCompra()
        {
            Models.Compra compra = _processaCompraReceiver.RecuperarMensagemCompra();

            if (compra != null)
            {
                _logger.LogInformation("Compra {Id}: início do processamento", compra.Id);

                _compraRepository.AlterarStatusCompraParaProcessado(compra.Id);
                _processaCompraReceiver.Limpar();

                _logger.LogInformation("Compra {Id}: processada com sucesso", compra.Id);
            }
        }
    }
}

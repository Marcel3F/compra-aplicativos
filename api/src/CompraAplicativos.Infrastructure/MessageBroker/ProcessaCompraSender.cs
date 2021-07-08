using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Application.MessageBroker;
using CompraAplicativos.Core.Compras;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CompraAplicativos.Infrastructure.MessageBroker
{
    public class ProcessaCompraSender : IProcessaCompraSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProcessaCompraSender> _logger;
        private IConnection _connection;

        public ProcessaCompraSender(
            IConfiguration configuration,
            ILogger<ProcessaCompraSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Task Enviar(Compra compra)
        {
            if (ConexaoExiste())
            {
                string queue = _configuration["RabbitMQ:Queue"];
                using IModel channel = _connection.CreateModel();
                channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

                string json = JsonConvert.SerializeObject(compra);
                byte[] body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
            }

            return Task.CompletedTask;
        }

        private void CriarConexao()
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMQ:HostName"],
                    UserName = _configuration["RabbitMQ:UserName"],
                    Password = _configuration["RabbitMQ:Password"],
                    Port = Convert.ToInt32(_configuration["RabbitMQ:Port"])
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                const string messagem = "Não foi possivel se conectar ao RabbitMQ";
                _logger.LogError(messagem);
                throw new MessageBrokerException(messagem, ex);
            }
        }

        private bool ConexaoExiste()
        {
            if (_connection != null)
            {
                return true;
            }

            CriarConexao();

            return _connection != null;
        }
    }
}

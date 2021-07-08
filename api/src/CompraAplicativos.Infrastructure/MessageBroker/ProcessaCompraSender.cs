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
            if (ConnectionExists())
            {
                string queueName = _configuration["RabbitMQ:QueueName"];
                using IModel channel = _connection.CreateModel();
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                string json = JsonConvert.SerializeObject(compra);
                byte[] body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }

            return Task.CompletedTask;
        }

        private void CreateConnection()
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMQ:HostName"],
                    UserName = _configuration["RabbitMQ:UserName"],
                    Password = _configuration["RabbitMQ:Password"],
                    Port = 5672
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

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}

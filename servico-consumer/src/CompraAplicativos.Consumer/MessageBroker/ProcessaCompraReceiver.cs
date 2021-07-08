using CompraAplicativos.Consumer.Exceptions;
using CompraAplicativos.Consumer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace CompraAplicativos.Infrastructure.MessageBroker
{
    public class ProcessaCompraReceiver : IProcessaCompraReceiver
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProcessaCompraReceiver> _logger;
        
        private readonly string _queue;
        private IConnection _connection;
        private IModel _channel;

        private Compra compra = default;

        public ProcessaCompraReceiver(
            IConfiguration configuration,
            ILogger<ProcessaCompraReceiver> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _queue = _configuration["RabbitMQ:Queue"];

            CriarConexao();
            RegistrarConsumer();
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

        private void RegistrarConsumer()
        {
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            
            consumer.Received += (sender, eventArg) =>
            {
                string content = Encoding.UTF8.GetString(eventArg.Body.ToArray());
                compra = JsonConvert.DeserializeObject<Compra>(content);

                _channel.BasicAck(eventArg.DeliveryTag, false);
            };

            _channel.BasicConsume(_queue, false, consumer);
        }

        public Compra RecuperarMensagemCompra()
        {
            return compra;
        }

        public void Limpar()
        {
            compra = default;
        }
    }
}

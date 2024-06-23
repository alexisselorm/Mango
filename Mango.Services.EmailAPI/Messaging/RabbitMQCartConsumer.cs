using Mango.Services.EmailAPI.Models.DTO;
using Mango.Services.EmailAPI.Service;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class RabbitMQCartConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IConfiguration _config;
        private readonly EmailService _emailService;
        public RabbitMQCartConsumer(IConfiguration config, EmailService emailService)
        {
            _config = config;
            _emailService = emailService;


            var factory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(_config["TopicAndQueueNames:EmailShoppingCartQueue"], false, false, false, null);


        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                CartDTO cartDTO = JsonConvert.DeserializeObject<CartDTO>(content);
                HandleMessage(cartDTO).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(_config["TopicAndQueueNames:EmailShoppingCartQueue"], false, consumer);

            return Task.CompletedTask;
        }
        private async Task HandleMessage(CartDTO cartDTO)
        {
            await _emailService.EmailCartAndLog(cartDTO);
        }
    }
}

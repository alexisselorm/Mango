using Mango.Services.EmailAPI.Service;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class RabbitMQAuthConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IConfiguration _config;
        private readonly EmailService _emailService;
        public RabbitMQAuthConsumer(IConfiguration config, EmailService emailService)
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
            _channel.QueueDeclare(_config["TopicAndQueueNames:RegisterUserQueue"], false, false, false, null);


        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                String email = JsonConvert.DeserializeObject<string>(content);
                HandleMessage(email);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(_config["TopicAndQueueNames:RegisterUserQueue"], false, consumer);

            return Task.CompletedTask;
        }
        private async Task HandleMessage(string email) { }
    }
}

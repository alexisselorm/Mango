using Mango.Services.EmailAPI.Service;
using RabbitMQ.Client;

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
            throw new NotImplementedException();
        }
    }
}

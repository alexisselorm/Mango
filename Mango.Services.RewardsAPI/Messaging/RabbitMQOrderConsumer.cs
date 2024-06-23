using Mango.Services.EmailAPI.Service;
using Mango.Services.RewardsAPI.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class RabbitMQOrderConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IConfiguration _config;
        private readonly RewardService _rewardService;
        private string ExchangeName = "";
        string queueName = "";
        private const string OrderCreated_RewardsUpdateQueue = "RewardsUpdateQueue";
        public RabbitMQOrderConsumer(IConfiguration config, RewardService rewardService)
        {
            _config = config;
            _rewardService = rewardService;
            ExchangeName = _config["TopicAndQueueNames:OrderCreatedTopic"];


            var factory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);

            _channel.QueueDeclare(OrderCreated_RewardsUpdateQueue);
            _channel.QueueBind(OrderCreated_RewardsUpdateQueue, ExchangeName, "RewardsUpdate");



        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                RewardMessage rewardMessage = JsonConvert.DeserializeObject<RewardMessage>(content);
                HandleMessage(rewardMessage).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(queueName, false, consumer);
            return Task.CompletedTask;
        }
        private async Task HandleMessage(RewardMessage rewardMessage)
        {
            await _rewardService.UpdateRewards(rewardMessage);
        }
    }
}

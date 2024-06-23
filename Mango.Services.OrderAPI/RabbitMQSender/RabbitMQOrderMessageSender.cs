using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.OrderAPI.RabbitMQSender
{
    public class RabbitMQOrderMessageSender : IRabbitMQOrderMessageSender
    {
        private string _username;
        private string _password;
        private string _hostname;
        private IConnection _connection;
        private const string OrderCreated_RewardsUpdateQueue = "RewardsUpdateQueue";
        private const string OrderCreated_EmailUpdateQueue = "EmailUpdateQueue";
        public RabbitMQOrderMessageSender()
        {
            _hostname = "localhost";
            _username = "guest";
            _password = "guest";

        }

        public void SendMessage(object message, string exchangeName)
        {
            if (ConnectionExists())
            {

                using var channel = _connection.CreateModel();
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: false);
                channel.QueueDeclare(OrderCreated_EmailUpdateQueue, false, false, false, null);
                channel.QueueDeclare(OrderCreated_RewardsUpdateQueue, false, false, false, null);

                channel.QueueBind(OrderCreated_EmailUpdateQueue, exchangeName, "EmailUpdate");
                channel.QueueBind(OrderCreated_RewardsUpdateQueue, exchangeName, "RewardsUpdate");
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: exchangeName, routingKey: "EmailUpdate", null, body: body);
                channel.BasicPublish(exchange: exchangeName, routingKey: "RewardsUpdate", null, body: body);
            }

        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    UserName = _username,
                    Password = _password,
                    HostName = _hostname,
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ConnectionExists()
        {
            try
            {
                if (_connection != null)
                {
                    return true;
                }
                CreateConnection();
                return true;

            }
            catch (Exception)
            {
                return false;

                throw;
            }
        }
    }
}

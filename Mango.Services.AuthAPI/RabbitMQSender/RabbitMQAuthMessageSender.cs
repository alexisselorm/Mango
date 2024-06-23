using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.AuthAPI.RabbitMQSender
{
    public class RabbitMQAuthMessageSender : IRabbitMQAuthMessageSender
    {
        private string _username;
        private string _password;
        private string _hostname;
        private IConnection _connection;
        public RabbitMQAuthMessageSender()
        {
            _hostname = "localhost";
            _username = "guest";
            _password = "guest";

        }

        public void SendMessage(object message, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                UserName = _username,
                Password = _password,
                HostName = _hostname,
            };

            _connection = factory.CreateConnection();
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queueName, false, false, false, null);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: queueName, null, body: body);
        }
    }
}

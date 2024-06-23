using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.ShoppingCartAPI.RabbitMQSender
{
    public class RabbitMQCartMessageSender : IRabbitMQCartMessageSender
    {
        private string _username;
        private string _password;
        private string _hostname;
        private IConnection _connection;
        public RabbitMQCartMessageSender()
        {
            _hostname = "localhost";
            _username = "guest";
            _password = "guest";

        }

        public void SendMessage(object message, string queueName)
        {
            if (ConnectionExists())
            {

                using var channel = _connection.CreateModel();
                channel.QueueDeclare(queueName, false, false, false, null);
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: "", routingKey: queueName, null, body: body);
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

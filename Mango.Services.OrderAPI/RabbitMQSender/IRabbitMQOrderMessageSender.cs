namespace Mango.Services.OrderAPI.RabbitMQSender
{
    public interface IRabbitMQOrderMessageSender
    {
        public void SendMessage(object message, string exchangeName);
    }
}

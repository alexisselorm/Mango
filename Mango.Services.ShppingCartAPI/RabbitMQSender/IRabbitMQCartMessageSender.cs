﻿namespace Mango.Services.ShoppingCartAPI.RabbitMQSender
{
    public interface IRabbitMQCartMessageSender
    {
        public void SendMessage(object message, string queueName);
    }
}

﻿namespace Mango.Services.AuthAPI.RabbitMQSender
{
    public interface IRabbitMQAuthMessageSender
    {
        public void SendMessage(object message, string queueName);
    }
}

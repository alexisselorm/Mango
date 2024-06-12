using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Messages;
using Mango.Services.EmailAPI.Models.DTO;
using Mango.Services.EmailAPI.Service;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string registerCartQueue;
        private readonly string orderCreatedTopic;
        private readonly string orderCreated_Email_Subscription;
        private readonly IConfiguration _config;
        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _registerUserProcesser;
        private ServiceBusProcessor _emailOrderPlaceProcessor;
        private EmailService _emailService;
        public AzureServiceBusConsumer(IConfiguration config, EmailService emailService)
        {

            _config = config;
            serviceBusConnectionString = _config["ServiceBusConnectionString"];
            emailCartQueue = _config["TopicAndQueueNames:EmailShoppingCartQueue"];
            registerCartQueue = _config["TopicAndQueueNames:RegisterUserQueue"];
            orderCreatedTopic = _config["TopicAndQueueNames:OrderCreatedTopic"];
            orderCreated_Email_Subscription = _config["TopicAndQueueNames:OrderCreated_Email_Subscription"];

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _registerUserProcesser = client.CreateProcessor(registerCartQueue);
            _emailOrderPlaceProcessor = client.CreateProcessor(orderCreatedTopic, orderCreated_Email_Subscription);
            _emailService = emailService;

        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _registerUserProcesser.ProcessMessageAsync += OnUserRegisterRequestReceived;
            _registerUserProcesser.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _emailOrderPlaceProcessor.ProcessMessageAsync += OnOnOrderPlacedRequestReceived;
            _emailOrderPlaceProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlaceProcessor.StartProcessingAsync();
        }

        private async Task OnOnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RewardMessage objMessage = JsonConvert.DeserializeObject<RewardMessage>(body);
            try
            {
                await _emailService.LogOrderPlaced(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _registerUserProcesser.StopProcessingAsync();
            await _registerUserProcesser.DisposeAsync();

            await _emailOrderPlaceProcessor.StopProcessingAsync();
            await _emailOrderPlaceProcessor.DisposeAsync();
        }

        private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            string email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                await _emailService.RegisterUserEmailAndLog(email);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDTO objMessage = JsonConvert.DeserializeObject<CartDTO>(body);
            try
            {
                await _emailService.EmailCartAndLog(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }


    }
}

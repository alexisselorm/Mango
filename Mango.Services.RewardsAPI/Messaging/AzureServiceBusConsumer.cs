using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Service;
using Mango.Services.RewardAPI.Messaging;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.RewardsAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string registerCartQueue;
        private readonly IConfiguration _config;
        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _registerUserProcesser;
        private RewardService _emailService;
        public AzureServiceBusConsumer(IConfiguration config, RewardService emailService)
        {

            _config = config;
            serviceBusConnectionString = _config["ServiceBusConnectionString"];
            emailCartQueue = _config["TopicAndQueueNames:RewardsShoppingCartQueue"];
            registerCartQueue = _config["TopicAndQueueNames:RegisterUserQueue"];

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _registerUserProcesser = client.CreateProcessor(registerCartQueue);
            _emailService = emailService;

        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnRewardsCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _registerUserProcesser.ProcessMessageAsync += OnUserRegisterRequestReceived;
            _registerUserProcesser.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _registerUserProcesser.StopProcessingAsync();
            await _registerUserProcesser.DisposeAsync();
        }

        private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            string email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                await _emailService.RegisterUserRewardsAndLog(email);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task OnRewardsCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDTO objMessage = JsonConvert.DeserializeObject<CartDTO>(body);
            try
            {
                await _emailService.RewardsCartAndLog(objMessage);
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

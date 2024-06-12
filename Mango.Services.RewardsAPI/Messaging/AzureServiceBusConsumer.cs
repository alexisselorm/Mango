using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Service;
using Mango.Services.RewardsAPI.Messages;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.RewardsAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string orderCreatedTopic;
        private readonly string orderCreatedRewardSubscription;
        private readonly IConfiguration _config;
        private ServiceBusProcessor _rewardProcessor;
        private RewardService _rewardService;
        public AzureServiceBusConsumer(IConfiguration config, RewardService rewardService)
        {

            _config = config;
            serviceBusConnectionString = _config["ServiceBusConnectionString"];
            orderCreatedTopic = _config["TopicAndQueueNames:OrderCreatedTopic"];
            orderCreatedRewardSubscription = _config["TopicAndQueueNames:OrderCreated_Rewards_Subscription:"];

            var client = new ServiceBusClient(serviceBusConnectionString);
            _rewardProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedRewardSubscription);
            _rewardService = rewardService;

        }

        public async Task Start()
        {
            _rewardProcessor.ProcessMessageAsync += OnNewOrderRequestReceived;
            _rewardProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardProcessor.StartProcessingAsync();


        }


        public async Task Stop()
        {
            await _rewardProcessor.StopProcessingAsync();
            await _rewardProcessor.DisposeAsync();

        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnNewOrderRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            RewardMessage rewardMessage = JsonConvert.DeserializeObject<RewardMessage>(body);
            try
            {
                await _rewardService.UpdateRewards(rewardMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}

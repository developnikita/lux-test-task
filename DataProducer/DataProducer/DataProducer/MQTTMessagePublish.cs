using Common.PubSub;
using MQTTnet;
using MQTTnet.Client;
using System.Threading;

namespace DataProducer
{
    public class MQTTMessagePublish : IMessagePublish
    {
        public MQTTMessagePublish(IMqttClient client, string messageTopic)
        {
            _client = client;
            _messageTopic = messageTopic;
        }

        public async void Publish(string message)
        {
            var mqttMessage = new MqttApplicationMessageBuilder()
                                  .WithTopic(_messageTopic)
                                  .WithPayload(message)
                                  .WithQualityOfServiceLevel(1)
                                  .WithRetainFlag(true)
                                  .Build();
            await _client.PublishAsync(mqttMessage, CancellationToken.None);
        }

        private string _messageTopic;
        private IMqttClient _client;
    }
}

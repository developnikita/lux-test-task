using Common.DataSendType;
using Common.PubSub;
using Common.Settings;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Receiving;
using System;
using System.Text.Json;

namespace DataConsumer.Consumer
{
    public class SpeedAndPumpConsumer : IMessageConsume<SpeedAndPump>
    {
        public SpeedAndPumpConsumer(IMqttClient client, MQTTBrokerTopic topic)
        {
            _client = client;
            _client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnMessageRecieved);

            var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic(topic.Topic)
                .Build();
            _client.SubscribeAsync(topicFilter);
        }

        private void OnMessageRecieved(MqttApplicationMessageReceivedEventArgs e)
        {
            var speedAndPump = JsonSerializer.Deserialize<SpeedAndPump>(e.ApplicationMessage.Payload);

            if (speedAndPump != null)
            {
                OnConsume?.Invoke(this, speedAndPump);
            }
        }

        public event EventHandler<SpeedAndPump> OnConsume;

        private IMqttClient _client;
    }
}

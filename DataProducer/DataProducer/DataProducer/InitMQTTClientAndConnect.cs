using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DataProducer
{
    public static class InitMQTTClientAndConnect
    {
        public async static Task<IMqttClient> InitMqttClient(MQTTBrokerSettings setting)
        {
            var factory = new MqttFactory();
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(setting.Host, setting.Port)
                .WithCredentials(setting.User, setting.Password)
                .WithClientId(Guid.NewGuid().ToString())
                .Build();

            var client = factory.CreateMqttClient();
            await client.ConnectAsync(options, CancellationToken.None);
            return client;
        }
    }
}

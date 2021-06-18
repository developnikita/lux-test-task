using Common.DataSendType;
using Common.Settings;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace DataProducer
{
    class Program
    {
        public static IConfiguration Configuration;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json", optional: false);

            Configuration = builder.Build();

            var mqttBrokerSettings = Configuration.GetSection(nameof(MQTTBrokerSettings)).Get<MQTTBrokerSettings>();
            var mqttMessageTpoic = Configuration.GetSection(nameof(MQTTBrokerTopic)).Get<MQTTBrokerTopic>();
            var dataFileSettings = Configuration.GetSection(nameof(DataFileSettings)).Get<DataFileSettings>();

            var client = MQTTConnectionFactory.InitMqttClient(mqttBrokerSettings).Result;
            var list = new XlsxFileReader<SpeedAndPump>(SpeedAndPumpConverter.ConvertFromColumnCollection);
            var publisher = new MQTTMessagePublish(client, mqttMessageTpoic.Topic);

            foreach (var item in list.ReadFile(dataFileSettings.FileName, dataFileSettings.IsTableContainsHeader))
            {
                publisher.Publish(JsonSerializer.Serialize<SpeedAndPump>(item));
                Thread.Sleep(100);
            }
        }
    }
}

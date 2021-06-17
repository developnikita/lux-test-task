using Common.Settings;
using DataConsumer.Consumer;
using DataConsumer.View;
using DataConsumer.ViewModel;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client.Options;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace DataConsumer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration Configuration;

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = configurationBuilder.Build();

            var mqttBrokerSettings = Configuration.GetSection(nameof(MQTTBrokerSettings)).Get<MQTTBrokerSettings>();
            var mqttTopicSettings = Configuration.GetSection(nameof(MQTTBrokerTopic)).Get<MQTTBrokerTopic>();

            var factory = new MqttFactory();
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(mqttBrokerSettings.Host, mqttBrokerSettings.Port)
                .WithCredentials(mqttBrokerSettings.User, mqttBrokerSettings.Password)
                .WithClientId(Guid.NewGuid().ToString())
                .Build();

            var client = factory.CreateMqttClient();
            await client.ConnectAsync(options, CancellationToken.None);

            var speedAndPumpConsumer = new SpeedAndPumpConsumer(client, mqttTopicSettings);
            var mainWindowViewModel = new MainWindowViewModel(speedAndPumpConsumer);
            MainWindow = new MainWindowView(mainWindowViewModel);
        }
    }
}

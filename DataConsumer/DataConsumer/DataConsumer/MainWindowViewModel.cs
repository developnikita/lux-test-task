using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace DataConsumer
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            _text = string.Empty;
            var factory = new MqttFactory();
            var options = new MqttClientOptionsBuilder()
                .WithTls(new MqttClientOptionsBuilderTlsParameters()
                {
                    UseTls = false,
                    IgnoreCertificateChainErrors = true,
                    IgnoreCertificateRevocationErrors = true,
                    AllowUntrustedCertificates = true,
                })
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("localhost", 1883)
                .WithCredentials("admin", "admin")
                .WithCleanSession(true)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(5))
                .Build();

            _client = factory.CreateMqttClient();
            _client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnSubscriberConnected);
            _client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnSubscriberDisconnected);
            _client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnSubscriberMessageReceived);

            Task.Run(() => _client.ConnectAsync(options)).Wait();


            Task.Run(() =>
            {
                var topicFilter = new MqttTopicFilterBuilder()
                    .WithTopic("SpeedAndPump")
                    .Build();
                _client.SubscribeAsync(topicFilter);
            }).Wait();
        }

        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                if (_text == value)
                    return;

                _text = value;
                RaisePropertyChanged(nameof(Text));
            }
        }

        private void OnSubscriberMessageReceived(MqttApplicationMessageReceivedEventArgs x)
        {
            var item = $"Timestamp: {DateTime.Now:O} | Topic: {x.ApplicationMessage.Topic} | Payload: {x.ApplicationMessage.ConvertPayloadToString()} | QoS: {x.ApplicationMessage.QualityOfServiceLevel}";
            Text = x.ApplicationMessage.ConvertPayloadToString();
        }

        private void OnSubscriberConnected(MqttClientConnectedEventArgs e)
        {
            MessageBox.Show("Subscriber connected", "ConnectHandler");
        }

        private void OnSubscriberDisconnected(MqttClientDisconnectedEventArgs e)
        {
            MessageBox.Show("Subscriber disconnected", "DisconnectHandler");
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private IMqttClient _client;
        private string _text;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

using Common.DataSendType;
using Common.PubSub;

namespace DataConsumer.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(IMessageConsume<SpeedAndPump> data)
        {
            _data = data;
            _data.OnConsume += OnConsume;
        }

        private void OnConsume(object sender, SpeedAndPump e)
        {
            AdvisedLineSpeed = e.Speed.ToString(".##");
            AdvisedPumpRate = e.Pump.ToString(".##");
        }

        public string AdvisedLineSpeed
        {
            get
            {
                return _advisedLineSpeed;
            }
            set
            {
                if (_advisedLineSpeed == value)
                    return;

                _advisedLineSpeed = value;
                RaisePropertyChanged(nameof(AdvisedLineSpeed));
            }
        }

        public string AdvisedPumpRate
        {
            get
            {
                return _advisedPumpRate;
            }
            set
            {
                if (_advisedPumpRate == value)
                    return;

                _advisedPumpRate = value;
                RaisePropertyChanged(nameof(AdvisedPumpRate));
            }
        }

        private string _advisedLineSpeed;
        private string _advisedPumpRate;
        private IMessageConsume<SpeedAndPump> _data;
    }
}

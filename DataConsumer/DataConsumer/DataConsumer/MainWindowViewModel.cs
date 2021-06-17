using Common.DataSendType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConsumer
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {

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

        public string AdvicedPumpRate
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
                RaisePropertyChanged(nameof(AdvicedPumpRate));
            }
        }

        private string _advisedLineSpeed;
        private string _advisedPumpRate;
    }
}

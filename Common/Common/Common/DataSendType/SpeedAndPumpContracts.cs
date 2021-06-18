using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.DataSendType
{
    public record SpeedAndPump(double Time, double Speed, double Pump);

    public static class SpeedAndPumpConverter
    {
        public static SpeedAndPump ConvertFromColumnCollection(IEnumerable<string> columnsValue)
        {
            if (columnsValue.Count() != 3)
                throw new ArgumentException(nameof(columnsValue));

            if (double.TryParse(columnsValue.ElementAt(0), out double firstColumnValue) &&
                double.TryParse(columnsValue.ElementAt(1), out double secondColumnValue) &&
                double.TryParse(columnsValue.ElementAt(2), out double thirdColumnValue))
                return new SpeedAndPump(firstColumnValue, secondColumnValue, thirdColumnValue);

            throw new ArgumentException(nameof(columnsValue));
        }
    }
}

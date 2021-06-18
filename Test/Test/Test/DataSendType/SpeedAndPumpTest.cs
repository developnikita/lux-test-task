using Common.DataSendType;
using System;
using System.Collections.Generic;
using Xunit;

namespace Test.DataSendType
{
    public class SpeedAndPumpTest
    {
        [Fact]
        public void TestSpeedAndPumpConvertFunc()
        {
            var stringList = new List<string> { "0,1", "0,2", "0,3" };
            var expectedSpeedAndPumpModel = new SpeedAndPump(0.1, 0.2, 0.3);

            var actualSpeedAndPumpModel = SpeedAndPumpConverter.ConvertFromColumnCollection(stringList);

            Assert.Equal(expectedSpeedAndPumpModel, actualSpeedAndPumpModel);
        }

        [Fact]
        public void TestSpeedAndPumpConvertFuncWithNonParsableDataException()
        {
            var stringList = new List<string> { "0,1", "0,2", "asd" };
            Assert.Throws<ArgumentException>(() => SpeedAndPumpConverter.ConvertFromColumnCollection(stringList));
        }

        [Fact]
        public void TestSpeedAndPumpConvertFuncWithIncorrectListException()
        {
            var stringList = new List<string> { "0,1" };
            Assert.Throws<ArgumentException>(() => SpeedAndPumpConverter.ConvertFromColumnCollection(stringList));
            var stringList2 = new List<string> { "0,1", "0,2", "0,3", "0,4" };
            Assert.Throws<ArgumentException>(() => SpeedAndPumpConverter.ConvertFromColumnCollection(stringList2));
        }
    }
}

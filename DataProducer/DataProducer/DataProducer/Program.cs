using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client.Options;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DataProducer
{
    class Program
    {
        public static IConfiguration Configuration;

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            Configuration = builder.Build();

            var mqttBrokerSettings = Configuration.GetSection(nameof(MQTTBrokerSettings)).Get<MQTTBrokerSettings>();

            var factory = new MqttFactory();
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(mqttBrokerSettings.Host, mqttBrokerSettings.Port)
                .WithCredentials(mqttBrokerSettings.User, mqttBrokerSettings.Password)
                .WithClientId(Guid.NewGuid().ToString())
                .Build();

            var client = factory.CreateMqttClient();
            var result = await client.ConnectAsync(options, CancellationToken.None);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var sb = new StringBuilder();
            var list = new List<SpeedAndPump>();
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"C:\Users\Nekit\Desktop\SpeedAndPump.xlsx")))
            {
                var worksheet = excelPackage.Workbook.Worksheets.First();
                var totalRows = worksheet.Dimension.End.Row;
                var totalColumns = worksheet.Dimension.End.Column;

                for (int rowNum = 1; rowNum <= totalRows; ++rowNum)
                {
                    var row = worksheet.Cells[rowNum, 1, rowNum, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
                    if (double.TryParse(row.ElementAt(0), out double firstColumnValue) && 
                        double.TryParse(row.ElementAt(1), out double secondColumnValue) &&
                        double.TryParse(row.ElementAt(2), out double thirdColumnValue))
                    {
                        list.Add(new SpeedAndPump(firstColumnValue, secondColumnValue, thirdColumnValue));
                    }

                    sb.AppendLine(string.Join(",", row));
                }
            }

            foreach (var item in list)
            {
                var message = new MqttApplicationMessageBuilder()
                            .WithTopic("MyTopic")
                            .WithPayload(JsonSerializer.Serialize<SpeedAndPump>(item))
                            .WithQualityOfServiceLevel(1)
                            .WithRetainFlag(true)
                            .Build();
                await client.PublishAsync(message, CancellationToken.None);
                Thread.Sleep(10000);
            }
        }
    }
}

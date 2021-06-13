using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DataProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var sb = new StringBuilder();
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"C:\Users\Nekit\Desktop\SpeedAndPump.xlsx")))
            {
                var worksheet = excelPackage.Workbook.Worksheets.First();
                var totalRows = worksheet.Dimension.End.Row;
                var totalColumns = worksheet.Dimension.End.Column;

                for (int rowNum = 1; rowNum <= totalRows; ++rowNum)
                {
                    var row = worksheet.Cells[rowNum, 1, rowNum, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
                    sb.AppendLine(string.Join(",", row));
                }
            }
            Console.WriteLine(sb.ToString());
        }
    }
}

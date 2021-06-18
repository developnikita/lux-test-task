using Common.FileReader;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataProducer
{
    public class XlsxFileReader<TResult> : ITableFileReader<TResult>
    {
        public XlsxFileReader(Func<IEnumerable<string>, TResult> fromStringColumnCollectionToResultConverter)
        {
            _fromStringColumnCollectionToResultConverter = fromStringColumnCollectionToResultConverter;
        }

        public IEnumerable<TResult> ReadFile(string fileName, bool isTabelContainsHeader = false)
        {
            var resultList = new List<TResult>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileName)))
            {
                var worksheet = excelPackage.Workbook.Worksheets.First();
                var totalRows = worksheet.Dimension.End.Row;
                var totalColumns = worksheet.Dimension.End.Column;

                int rowNum = isTabelContainsHeader ? 2 : 1;
                for (; rowNum <= totalRows; ++rowNum)
                {
                    try
                    {
                        var row = worksheet.Cells[rowNum, 1, rowNum, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
                        resultList.Add(_fromStringColumnCollectionToResultConverter(row));
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Incorrect data from file. Cannot convert to {typeof(TResult)}. Error message: {ex.Message}");
                    }
                }
            }

            return resultList;
        }

        private Func<IEnumerable<string>, TResult> _fromStringColumnCollectionToResultConverter;
    }
}

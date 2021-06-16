using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProducer
{
    public class XlsxFileReader<TResult> : ITableFileReader<TResult>
    {
        public XlsxFileReader(Func<IEnumerable<string>, TResult> fromStringColumnCollectionToResultConverter)
        {
            _fromStringColumnCollectionToResultConverter = fromStringColumnCollectionToResultConverter;
        }

        public IEnumerable<TResult> ReadFile(string fileName)
        {
            throw new NotImplementedException();
        }

        private Func<IEnumerable<string>, TResult> _fromStringColumnCollectionToResultConverter;
    }
}

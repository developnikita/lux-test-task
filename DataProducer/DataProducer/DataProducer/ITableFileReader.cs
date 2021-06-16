using System.Collections.Generic;

namespace DataProducer
{
    public interface ITableFileReader<TResult>
    {
        public IEnumerable<TResult> ReadFile(string fileName);
    }
}

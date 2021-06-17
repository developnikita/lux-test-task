using System.Collections.Generic;

namespace Common.FileReader
{
    public interface ITableFileReader<TResult>
    {
        public IEnumerable<TResult> ReadFile(string fileName, bool includeTableHeader = false);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.FlatFile;

namespace NBi.Core.FlatFile
{
    public class FlatFileReaderFactory
    {
        public IFlatFileReader Instantiate(string parserName, IFlatFileProfile profile)
        {
            if (string.IsNullOrEmpty(parserName))
                return new CsvReader();

            //if (readers.ContainsKey(parserName))
            //    return Instantiate(readers[parserName], cmd);
            throw new ArgumentException();
        }
    }
}

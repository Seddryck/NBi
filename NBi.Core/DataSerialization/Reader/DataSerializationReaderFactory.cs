using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Reader
{
    class DataSerializationReaderFactory
    {
        public IDataSerializationReader Instantiate(IReaderArgs args)
        {
            switch (args)
            {
                case FileReaderArgs fileArgs: return new FileReader(fileArgs.BasePath, fileArgs.Path);
                case UrlReaderArgs urlArgs: return new UrlReader(urlArgs.Url);
                case RestReaderArgs restArgs: return new RestReader(restArgs.Rest);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}

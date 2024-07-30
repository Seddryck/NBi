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
            return args switch
            {
                FileReaderArgs fileArgs => new FileReader(fileArgs.BasePath, fileArgs.Path),
                UrlReaderArgs urlArgs => new UrlReader(urlArgs.Url),
                RestReaderArgs restArgs => new RestReader(restArgs.Rest),
                ScalarReaderArgs scalarArgs => new ScalarReader(scalarArgs.Value),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}

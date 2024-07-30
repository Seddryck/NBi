using NBi.Core.DataSerialization.Flattening.Json;
using NBi.Core.DataSerialization.Flattening.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Flattening
{
    class DataSerializationFlattenizerFactory
    {
        public IDataSerializationFlattenizer Instantiate(IFlattenizerArgs args)
        {
            return args switch
            {
                XPathArgs xpathArgs => new XPathEngine(xpathArgs.From, xpathArgs.Selects, xpathArgs.DefaultNamespacePrefix, xpathArgs.IsIgnoreNamespace),
                JsonPathArgs jsonPathArgs => new JsonPathEngine(jsonPathArgs.From, jsonPathArgs.Selects),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}

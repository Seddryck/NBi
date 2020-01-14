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
            switch (args)
            {
                case XPathArgs xpathArgs: return new XPathEngine(xpathArgs.From, xpathArgs.Selects, xpathArgs.DefaultNamespacePrefix, xpathArgs.IsIgnoreNamespace);
                case JsonPathArgs jsonPathArgs: return new JsonPathEngine(jsonPathArgs.From, jsonPathArgs.Selects);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}

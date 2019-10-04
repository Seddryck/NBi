using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NBi.Core.Xml
{
    class XmlIgnoreNamespaceReader : XmlWrappingReader
    {
        public XmlIgnoreNamespaceReader(TextReader input, XmlReaderSettings settings)
            : base(Create(input, settings)) { }

        public override string NamespaceURI { get => string.Empty; }
    }
}

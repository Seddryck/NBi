using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NBi.Xml
{
    class LocalXmlUrlResolver : XmlUrlResolver
    {
        private readonly string path;

        public LocalXmlUrlResolver(string path)
            : base()
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                path += Path.DirectorySeparatorChar;
            this.path = path;
        }

        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            if (baseUri != null)
                return base.ResolveUri(baseUri, relativeUri);
            else
                return base.ResolveUri(new Uri(path), relativeUri);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NBi.Core.Xml
{
    public class XPathFileEngine : XPathEngine
    {
        public string FilePath { get; private set; }

        public XPathFileEngine(string filePath, string from, IEnumerable<AbstractSelect> selects)
            : base(from, selects)
        {
            this.FilePath = filePath;
        }

        public override NBi.Core.ResultSet.ResultSet Execute()
        {
            if (!File.Exists(FilePath))
                throw new InvalidOperationException(string.Format("File '{0}' doesn't exist!", FilePath));

            var doc = XDocument.Load(FilePath);
            return Execute(doc);
        }
    }
}

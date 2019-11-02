using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using NBi.Core.Hierarchical;
using NBi.Core.ResultSet;

namespace NBi.Core.Hierarchical
{
    public abstract class AbstractPathEngine
    {
        protected IEnumerable<AbstractSelect> Selects { get; }
        protected string From { get; }

        public AbstractPathEngine(string from, IEnumerable<AbstractSelect> selects)
            => (From, Selects) = (from, selects);

        public abstract IEnumerable<object> Execute();
    }
}

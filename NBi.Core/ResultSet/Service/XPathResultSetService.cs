using NBi.Core.Query;
using NBi.Core.Xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Service
{
    class XPathResultSetService : IResultSetService
    {
        private readonly XPathEngine xpath;

        public XPathResultSetService(XPathEngine xpath)
        {
            this.xpath = xpath;
        }

        public virtual ResultSet Execute()
        {
            var rows = xpath.Execute();
            return new ResultSet();
        }
    }
}

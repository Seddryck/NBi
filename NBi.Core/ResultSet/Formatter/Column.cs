using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Formatter
{
    class Column
    {
        public Header Header { get; private set; }
        public IList<object> Values { get; private set; }

        public Column(Header header, IList<object> values)
        {
            Header = header;
            Values = values;
        }

        public Column()
        {
            Header = new Header();
            Values = new List<object>();
        }

    }
}

using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public abstract class QueryableXml : BaseItem
    {       
        public abstract string GetQuery();
    }
}

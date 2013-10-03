using System.Xml.Serialization;
using NBi.Core.ResultSet;

namespace NBi.Xml.Items.ResultSet
{
    public class CellXml : ICell
    {
        [XmlText]
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}

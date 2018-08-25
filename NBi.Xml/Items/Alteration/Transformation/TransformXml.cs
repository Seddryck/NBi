using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;
using System.Xml.Serialization;
using System.ComponentModel;

namespace NBi.Xml.Items.Alteration.Transform
{
    public class TransformXml : LightTransformXml
    {
        [XmlAttribute("column-index")]
        public int ColumnIndex { get; set; }
    }
}

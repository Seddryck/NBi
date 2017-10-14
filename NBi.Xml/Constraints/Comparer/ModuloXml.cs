using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer
{
    public class ModuloXml : AbstractComparerXml
    {
        [XmlAttribute("second-operand")]
        public string SecondOperand { get; set; }
    }
}

using NBi.Core.Evaluate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Validate
{
    public class PredicateXml
    {
        [XmlIgnore()]
        public List<IColumnVariable> Variables
        {
            get
            {
                return VariablesInternal.ToList<IColumnVariable>();
            }
        }

        [XmlElement("formula")]
        public FormulaXml Formula { get; set; }

        [XmlElement("variable")]
        public List<VariableXml> VariablesInternal { get; set; }

        [XmlElement("compare")]
        public ComparisonXml Comparison { get; set; }
    }
}

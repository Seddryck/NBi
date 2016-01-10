using NBi.Core.Evaluate;
using NBi.Xml.Items.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation
{
    public class FilterXml
    {
        [XmlIgnore()]
        public List<IColumnVariable> Variables
        {
            get
            {
                return VariablesInternal.ToList<IColumnVariable>();
            }
        }

        [XmlElement("expression")]
        public ExpressionXml Expression { get; set; }

        [XmlElement("variable")]
        public List<VariableXml> VariablesInternal { get; set; }

        [XmlElement("predicate")]
        public PredicateXml Predicate { get; set; }

        public FilterXml()
        {
            VariablesInternal = new List<VariableXml>();
        }
    }
}

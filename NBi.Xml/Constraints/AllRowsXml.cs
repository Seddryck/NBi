using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NBi.Xml.Constraints.Comparer;
using NBi.Xml.Items.Calculation;
using NBi.Core.Evaluate;

namespace NBi.Xml.Constraints
{
    public class AllRowsXml : AbstractConstraintXml
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

        public AllRowsXml()
        {
            VariablesInternal = new List<VariableXml>();
        }
    }
}

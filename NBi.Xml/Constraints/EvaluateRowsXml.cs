using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Evaluate;
using NBi.Xml.Items.Validate;

namespace NBi.Xml.Constraints
{
    public class EvaluateRowsXml: AbstractConstraintXml, IEvaluationRowsDefinition
    {
        [XmlIgnore()]
        public List<IColumnVariable> Variables
        {
            get
            {
                return VariablesInternal.ToList< IColumnVariable>(); 
            }
        }

        [XmlIgnore()]
        public List<IColumnExpression> Expressions
        {
            get
            {
                return ExpressionsInternal.ToList<IColumnExpression>();
            }
        }

        [XmlElement("variable")]
        public List<VariableXml> VariablesInternal { get; set; }

        [XmlElement("expression")]
        public List<ExpressionXml> ExpressionsInternal { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Evaluate;
using NBi.Xml.Items.Calculation;

namespace NBi.Xml.Constraints;

public class EvaluateRowsXml: AbstractConstraintXml, IEvaluationRowsDefinition
{
    [XmlIgnore()]
    public List<IColumnAlias> Variables
    {
        get
        {
            return VariablesInternal.ToList< IColumnAlias>(); 
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
    public List<AliasXml> VariablesInternal { get; set; }

    [XmlElement("expression")]
    public List<ExpressionXml> ExpressionsInternal { get; set; }
}

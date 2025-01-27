using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NBi.Xml.Constraints.Comparer;
using NBi.Xml.Items.Calculation;
using NBi.Core.Evaluate;
using System;

namespace NBi.Xml.Constraints;

public class NoRowsXml : AbstractConstraintXml
{
    [XmlIgnore()]
    public List<IColumnAlias> Aliases { get => InternalAliases.ToList<IColumnAlias>(); }


    [XmlElement("alias", Order = 1)]
    
    public List<AliasXml> InternalAliases
    {
        get { return internalAliases; }
        set { internalAliases = value; }
    }

    //Receiving the order 2 when readonly attribute is activated
    [XmlIgnore()]
    [Obsolete("Use InternalAlias in place of InternalAliasOld")]
    public List<AliasXml> InternalAliasesOld
    {
        get { return internalAliases; }
        set { internalAliases = value; }
    }


    [XmlElement("expression", Order = 3)]
    public List<ExpressionXml> Expressions { get; set; }

    private List<AliasXml> internalAliases;

    [XmlElement("predicate", Order = 4)]
    public SinglePredicationXml Predication { get; set; }

    [XmlElement("combination", Order = 5)]
    public CombinationPredicationXml Combination { get; set; }

    public NoRowsXml()
    {
        internalAliases = new List<AliasXml>();
        Expressions = new List<ExpressionXml>();
    }
}

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

namespace NBi.Xml.Constraints
{
    public class NoRowsXml : AbstractConstraintXml
    {
        [XmlIgnore()]
        public List<IColumnAlias> Aliases
        {
            get
            {
                return InternalAliases.ToList<IColumnAlias>();
            }
        }


        [XmlElement("alias", Order = 1)]
        
        public List<AliasXml> InternalAliases
        {
            get { return internalAliases; }
            set { internalAliases = value; }
        }


        [XmlElement("expression", Order = 2)]
        public List<ExpressionXml> Expressions { get; set; }

        [XmlIgnore]
        [Obsolete("Use InternalAlias in place of InternalAliasOld")]
        public List<AliasXml> InternalAliasesOld
        {
            get { return internalAliases; }
            set { internalAliases = value; }
        }

        private List<AliasXml> internalAliases;

        [XmlElement("predicate", Order = 3)]
        public PredicationXml Predication { get; set; }

        [XmlElement("combination", Order = 4)]
        public CombinationPredicateXml Combination { get; set; }

        public NoRowsXml()
        {
            internalAliases = new List<AliasXml>();
            Expressions = new List<ExpressionXml>();
        }
    }
}

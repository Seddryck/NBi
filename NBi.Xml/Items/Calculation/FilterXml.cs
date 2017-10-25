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
        public List<IColumnAlias> Aliases
        {
            get
            {
                return internalAliases.ToList<IColumnAlias>();
            }
        }

        [XmlElement("alias")]
        public List<AliasXml> InternalAliases
        {
            get { return internalAliases; }
            set { internalAliases = value; }
        }

        [XmlIgnore]
        public List<AliasXml> InternalAliasesOld
        {
            get { return internalAliases; }
            set { internalAliases = value; }
        }

        private List<AliasXml> internalAliases;

        [XmlElement("expression")]
        public ExpressionXml Expression { get; set; }

        [XmlElement("predicate")]
        public PredicateXml Predicate { get; set; }

        public FilterXml()
        {
            internalAliases = new List<AliasXml>();
        }
    }
}

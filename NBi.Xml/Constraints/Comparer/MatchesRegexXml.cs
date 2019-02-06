using NBi.Core.Calculation;
using NBi.Xml.SerializationOption;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer
{
    public class MatchesRegexXml : CaseSensitiveTextPredicateXml
    {

        [XmlIgnore]
        public CData ValueWrite { get => Value; set => Value = value; }

        public override bool ShouldSerializeValue() => false;

        internal override ComparerType ComparerType { get => ComparerType.MatchesRegex; }
    }
}

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
        [XmlText]
        public CData ValueWrite { get => base.Value; set => base.Value = value; }
        
        internal override ComparerType ComparerType { get => ComparerType.MatchesRegex; }
    }
}

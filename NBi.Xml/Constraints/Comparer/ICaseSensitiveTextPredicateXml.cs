using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer;

public interface ICaseSensitiveTextPredicateXml : IPredicateXml
{
    [XmlAttribute("ignore-case")]
    [DefaultValue(false)]
    bool IgnoreCase { get; set; }
    string Reference { get; set; }
}

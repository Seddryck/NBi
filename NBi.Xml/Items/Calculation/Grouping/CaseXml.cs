using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation.Grouping;

public class CaseXml
{
    [XmlElement(Type = typeof(SinglePredicationXml), ElementName = "predicate"),
    XmlElement(Type = typeof(CombinationPredicationXml), ElementName = "combination"),]
    public AbstractPredicationXml Predication { get; set; }
}

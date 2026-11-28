using NBi.Core.ResultSet;
using NBi.Model.Xml.Constraints.Comparer;
using NBi.Model.Xml.Items.Calculation;
using NBi.Model.Xml.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Model.Xml.Items.Alteration.Duplication;

public class DuplicateXml : AlterationXml
{
    [XmlElement("predicate")]
    public SinglePredicationXml Predication { get; set; }

    [XmlElement("times")]
    public string Times { get; set; } = "1";

    [XmlElement("output")]
    public List<OutputXml> Outputs { get; set; } = new List<OutputXml>();
}

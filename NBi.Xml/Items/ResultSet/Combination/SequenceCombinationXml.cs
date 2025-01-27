using NBi.Core.ResultSet.Combination;
using NBi.Xml.Variables.Sequence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.ResultSet.Combination;

public class SequenceCombinationXml
{
    [XmlAttribute("operation")]
    [DefaultValue(SequenceCombinationOperation.CartesianProduct)]
    public SequenceCombinationOperation Operation { get; set; }

    [XmlElement("sequence")]
    public List<SequenceXml> Sequences { get; set; } = new List<SequenceXml>();

    [XmlIgnore]
    public bool SequencesSpecified { get => Sequences.Count > 0; set { } }

    public SequenceCombinationXml() => Operation = SequenceCombinationOperation.CartesianProduct;
}

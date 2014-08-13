using NBi.Core.Analysis.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NBi.Xml.Decoration.Command
{
    public class PartitionProcessXml : IPartitionProcess
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
